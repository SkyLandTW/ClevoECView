using System;
using System.Management;
using System.Threading;

namespace ECView_CSharp
{
    internal sealed class ClevoControlWorker : IDisposable
    {
        private readonly Action<string> m_log;
        private readonly Thread m_thread;
        private readonly ManagementObject m_clevoWmiInstance;
        private readonly ManagementObject m_msTz0WmiInstance;

        private volatile bool m_stopRequested;
        private bool m_disposed;
        private int m_currentRpms;
        private int m_currentTemp;
        private int m_fanDutyWritten;
        private int m_fanDutyRead;
        private bool m_autoFanMode;
        private int m_autoPrevFanDuty;
        private int? m_manualNextFanDuty;
        private int m_manualPrevFanDuty;

        public int CurrentRpms => m_currentRpms;
        public int CurrentTemp => m_currentTemp;
        public int FanDutyRead => m_fanDutyRead;

        public ClevoControlWorker(Action<string> log)
        {
            m_log = log;
            m_thread = new Thread(WorkerThreadStart) { IsBackground = true, Name = "ClevoControlWorker" };
            m_clevoWmiInstance = new ManagementObject("root\\WMI", "CLEVO_GET.InstanceName='ACPI\\PNP0C14\\0_0'", null);
            m_clevoWmiInstance.Get();
            using (var searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature"))
                // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
                foreach (ManagementObject obj in searcher.Get())
                    if (@"ACPI\ThermalZone\TZ0__0".Equals(obj["InstanceName"].ToString()))
                    {
                        m_msTz0WmiInstance = obj;
                        break;
                    }
            m_autoFanMode = true;
            m_thread.Start();
        }

        public void SetAutoFanDuty()
        {
            m_log(DateTime.Now + ": set auto\r\n");
            m_autoFanMode = true;
            m_autoPrevFanDuty = 0;
            m_manualNextFanDuty = 0;
        }

        public void SetManualFanDuty(int percentage)
        {
            m_log(DateTime.Now + $": set {percentage}%\r\n");
            m_autoFanMode = false;
            m_autoPrevFanDuty = 0;
            m_manualNextFanDuty = percentage;
        }

        public void Dispose()
        {
            if (m_disposed)
                return;
            m_stopRequested = true;
            m_thread.Join();
            m_msTz0WmiInstance?.Dispose();
            m_clevoWmiInstance.Dispose();
            m_disposed = true;
        }

        private void WorkerThreadStart()
        {
            UpdateFanStateFromEc();
            while (!m_stopRequested)
            {
                var manualNextFanDuty = m_manualNextFanDuty;
                if (manualNextFanDuty.HasValue && manualNextFanDuty.Value != m_manualPrevFanDuty)
                {
                    SetFanDutyToEc(manualNextFanDuty.Value);
                    UpdateFanStateFromEc();
                    m_fanDutyWritten = m_manualPrevFanDuty = manualNextFanDuty.Value;
                }
                UpdateFanStateFromWmi();
                if (m_autoFanMode)
                {
                    var autoNextFanDuty = CalculateAutoFanDuty();
                    if (autoNextFanDuty != 0 && autoNextFanDuty != m_autoPrevFanDuty)
                    {
                        m_log($"{DateTime.Now} CPU={m_currentTemp}°C, auto fan duty to {autoNextFanDuty}%\r\n");
                        SetFanDutyToEc(autoNextFanDuty);
                        UpdateFanStateFromEc();
                        m_fanDutyWritten = m_autoPrevFanDuty = autoNextFanDuty;
                    }
                }
                Thread.Sleep(250);
                if (m_fanDutyWritten != 0 && m_fanDutyRead != m_fanDutyWritten)
                    UpdateFanStateFromEc();
            }
        }

        private int CalculateAutoFanDuty()
        {
            var temp = m_currentTemp;
            var duty = Math.Max(m_fanDutyRead, m_fanDutyWritten);
            //
            if (temp >= 80 && duty < 100)
                return 100;
            if (temp >= 70 && duty < 90)
                return 90;
            if (temp >= 60 && duty < 80)
                return 80;
            if (temp >= 50 && duty < 70)
                return 70;
            if (temp >= 40 && duty < 60)
                return 60;
            if (temp >= 30 && duty < 50)
                return 50;
            if (temp >= 20 && duty < 40)
                return 40;
            if (temp >= 10 && duty < 30)
                return 30;
            //
            if (temp <= 15 && duty > 30)
                return 30;
            if (temp <= 25 && duty > 40)
                return 40;
            if (temp <= 35 && duty > 50)
                return 50;
            if (temp <= 45 && duty > 60)
                return 60;
            if (temp <= 55 && duty > 70)
                return 70;
            if (temp <= 65 && duty > 80)
                return 80;
            if (temp <= 75 && duty > 90)
                return 90;
            //
            return 0;
        }

        private void UpdateFanStateFromWmi()
        {
            // FAN
            var rawCpuRpmStr = ExecClevoWmiMethod("GetFan12RPM");
            int rawCpuRpmInt;
            if (int.TryParse(rawCpuRpmStr, out rawCpuRpmInt))
                m_currentRpms = CalculateRpms(rawCpuRpmInt);
            else
                m_currentRpms = -1;
            // CPU
            if (m_msTz0WmiInstance != null)
            {
                m_msTz0WmiInstance.Get();
                var temp = Convert.ToDouble(m_msTz0WmiInstance["CurrentTemperature"].ToString());
                temp = (temp - 2732) / 10.0;
                m_currentTemp = (int) Math.Round(temp);
            }
        }

        private void UpdateFanStateFromEc()
        {
            // FAN
            var rawCpuRpm = CallingVariations.GetCPUFANRPM();
            m_currentRpms = CalculateRpms(rawCpuRpm);
            // CPU
            var ecData = CallingVariations.GetTempFanDuty(1);
            m_currentTemp = ecData.Local;
            m_fanDutyRead = (int) Math.Round(ecData.FanDuty / 2.55m);
        }

        private void SetFanDutyToEc(int percentage)
        {
            var fanDutyIn256 = (int) Math.Round(percentage / 100.0m * 255.0m);
            CallingVariations.SetFanDuty(1, fanDutyIn256);
        }

        private string ExecClevoWmiMethod(string name)
        {
            var outParams = m_clevoWmiInstance?.InvokeMethod(name, null, null);
            return outParams?["Data"].ToString();
        }

        private static int CalculateRpms(int rawCpuRpm)
        {
            if (rawCpuRpm <= 0)
                return 0;
            var flCpuRpm = 60.0m / (1.39130434782609E-05m * rawCpuRpm * 4.0m);
            flCpuRpm = flCpuRpm * 2.0m;
            return (int) Math.Round(flCpuRpm, 0);
        }
    }
}