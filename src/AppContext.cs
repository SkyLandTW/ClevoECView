using System;
using System.Drawing;
using System.IO;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ECView_CSharp.Properties;
using Timer = System.Threading.Timer;

namespace ECView_CSharp
{
    public class AppContext : ApplicationContext
    {
        public const int MAX_FAN_RPM = 4400;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool DestroyIcon(IntPtr handle);

        protected readonly Font font = new Font("Arial Narrow", 8, FontStyle.Bold);
        protected readonly Icon icon = Resources.ClevoECView;
        protected readonly Bitmap iconBitmap = new Bitmap(16, 16);
        protected readonly Graphics canvas;
        protected readonly Brush brushBg = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
        protected readonly Brush brushFg = new SolidBrush(Color.FromArgb(255, 96, 0, 0));
        protected readonly Brush brushText = new SolidBrush(Color.FromArgb(255, 255, 128, 0));
        protected readonly Brush brushDrop = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
        protected readonly Pen penLine = new Pen(Color.FromArgb(255, 128, 0, 0), 1);
        protected readonly NotifyIcon notifyIcon;
        protected readonly Timer timerFan;
        protected readonly System.Windows.Forms.Timer timerUI;
        protected readonly String logFilePath;
        protected readonly ManagementObject clevoWmiInstance;
        protected readonly ManagementObject msTz0WmiInstance;
        protected int currentRpms = -1;
        protected int currentTemp = -1;
        protected int lastFanCuty = -1;

        public AppContext()
        {
            canvas = Graphics.FromImage(iconBitmap);
            logFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) ?? @"C:\WINDOWS\TEMP", "error.log");
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Resources.ClevoECView;
            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Set Fan 100%", null, MenuItemSet100_Click));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Set Fan 90%", null, MenuItemSet90_Click));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Set Fan 80%", null, MenuItemSet80_Click));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Set Fan 70%", null, MenuItemSet70_Click));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Set Fan 60%", null, MenuItemSet60_Click));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Set Fan Auto", null, MenuItemSetAuto_Click));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Exit", null, MenuItemExit_Click));
            notifyIcon.Text = "ECView (initializing)";
            notifyIcon.Visible = true;
            timerFan = new Timer(s => UpdateFanStateFromWmi(), null, 500, 500);
            timerUI = new System.Windows.Forms.Timer { Interval = 500, Enabled = true };
            timerUI.Tick += TimerUI_Tick;
            timerUI.Start();
            clevoWmiInstance = new ManagementObject("root\\WMI", "CLEVO_GET.InstanceName='ACPI\\PNP0C14\\0_0'", null);
            clevoWmiInstance.Get();
            using (var searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature"))
            {
                // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
                foreach (ManagementObject obj in searcher.Get())
                {
                    if (@"ACPI\ThermalZone\TZ0__0".Equals(obj["InstanceName"].ToString()))
                    {
                        msTz0WmiInstance = obj;
                        break;
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (msTz0WmiInstance != null)
                    msTz0WmiInstance.Dispose();
                clevoWmiInstance.Dispose();
                timerFan.Dispose();
                timerUI.Stop();
                timerUI.Dispose();
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
                canvas.Dispose();
                icon.Dispose();
                iconBitmap.Dispose();
                font.Dispose();
                brushBg.Dispose();
                brushFg.Dispose();
                brushText.Dispose();
                brushDrop.Dispose();
                penLine.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void MenuItemSet100_Click(object sender, EventArgs e)
        {
            File.AppendAllText(logFilePath, DateTime.Now + ": set 100%\r\n");
            ThreadPool.QueueUserWorkItem(p => SetFanManual(1.0m));
            UpdateMenuItemsUI(sender);
        }

        protected void MenuItemSet90_Click(object sender, EventArgs e)
        {
            File.AppendAllText(logFilePath, DateTime.Now + ": set 90%\r\n");
            ThreadPool.QueueUserWorkItem(p => SetFanManual(0.9m));
            UpdateMenuItemsUI(sender);
        }

        protected void MenuItemSet80_Click(object sender, EventArgs e)
        {
            File.AppendAllText(logFilePath, DateTime.Now + ": set 80%\r\n");
            ThreadPool.QueueUserWorkItem(p => SetFanManual(0.8m));
            UpdateMenuItemsUI(sender);
        }

        protected void MenuItemSet70_Click(object sender, EventArgs e)
        {
            File.AppendAllText(logFilePath, DateTime.Now + ": set 70%\r\n");
            ThreadPool.QueueUserWorkItem(p => SetFanManual(0.7m));
            UpdateMenuItemsUI(sender);
        }

        protected void MenuItemSet60_Click(object sender, EventArgs e)
        {
            File.AppendAllText(logFilePath, DateTime.Now + ": set 60%\r\n");
            ThreadPool.QueueUserWorkItem(p => SetFanManual(0.6m));
            UpdateMenuItemsUI(sender);
        }

        protected void MenuItemSetAuto_Click(object sender, EventArgs e)
        {
            File.AppendAllText(logFilePath, DateTime.Now + ": set auto\r\n");
            ThreadPool.QueueUserWorkItem(p => SetFanAuto());
            UpdateMenuItemsUI(sender);
        }

        protected void TimerUI_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdateIcon();
                GC.Collect();
            }
            catch (Exception ex)
            {
                File.WriteAllText(logFilePath, DateTime.Now + "\r\n" + ex + "\r\n");
            }
        }

        protected void UpdateMenuItemsUI(object sender)
        {
            foreach (var item in notifyIcon.ContextMenuStrip.Items)
            {
                if (item == sender)
                    ((ToolStripMenuItem) item).ForeColor = Color.Blue;
                else
                    ((ToolStripMenuItem) item).ForeColor = Control.DefaultForeColor;
            }
        }

        protected void SetFanAuto()
        {
            ThreadPool.QueueUserWorkItem(p =>
                {
                    CallingVariations.SetFANDutyAuto(1);
                    UpdateFanStateFromEc();
                });
        }

        protected void SetFanManual(decimal fanDuty)
        {
            ThreadPool.QueueUserWorkItem(p =>
                {
                    CallingVariations.SetFanDuty(1, (int) (255 * fanDuty));
                    for (var i = 0; i < 3; i++)
                    {
                        Thread.Sleep(500);
                        UpdateFanStateFromEc();
                    }
                });
        }

        protected void UpdateFanStateFromEc()
        {
            // FAN
            var rawCpuRpm = CallingVariations.GetCPUFANRPM();
            currentRpms = CalculateRpms(rawCpuRpm);
            // CPU
            var ecData = CallingVariations.GetTempFanDuty(1);
            currentTemp = ecData.Local;
            lastFanCuty = (int) Math.Round(ecData.FanDuty / 2.55m);
        }

        protected void UpdateFanStateFromWmi()
        {
            // FAN
            var rawCpuRpmStr = ExecClevoWmiMethod("GetFan12RPM");
            int rawCpuRpmInt;
            if (Int32.TryParse(rawCpuRpmStr, out rawCpuRpmInt))
            {
                currentRpms = CalculateRpms(rawCpuRpmInt);
            }
            else
            {
                currentRpms = -1;
            }
            // CPU
            if (msTz0WmiInstance != null)
            {
                msTz0WmiInstance.Get();
                var temp = Convert.ToDouble(msTz0WmiInstance["CurrentTemperature"].ToString());
                temp = (temp - 2732) / 10.0;
                currentTemp = (int) Math.Round(temp);
            }
        }

        protected static int CalculateRpms(int getCpuRpm)
        {
            if (getCpuRpm <= 0)
                return 0;
            var flCpuRpm = 60.0m / (1.39130434782609E-05m * getCpuRpm * 4.0m);
            flCpuRpm = flCpuRpm * 2.0m;
            return (int) Math.Round(flCpuRpm, 0);
        }

        protected void UpdateIcon()
        {
            var rpmRatio = currentRpms / (decimal) MAX_FAN_RPM;
            if (rpmRatio >= 1)
                rpmRatio = 1;
            var iconSize = 16;
            var rpmsSize = (int) Math.Round(iconSize * rpmRatio);
            canvas.Clear(Color.Transparent);
            canvas.DrawIcon(icon, 0, 0);
            canvas.FillRectangle(brushBg, 0, 0, iconSize, iconSize);
            canvas.FillRectangle(brushFg,
                0, iconSize - rpmsSize,
                iconSize, rpmsSize);
            canvas.DrawLine(penLine, 0, iconSize - rpmsSize, iconSize, iconSize - rpmsSize);
            canvas.DrawString(
                currentTemp.ToString(),
                font, brushDrop,
                new RectangleF(1, 1, iconSize, iconSize),
                new StringFormat { Alignment = StringAlignment.Center }
                );
            canvas.DrawString(
                currentTemp.ToString(),
                font, brushText,
                new RectangleF(0, 0, iconSize, iconSize),
                new StringFormat { Alignment = StringAlignment.Center }
                );
            var oldIcon = notifyIcon.Icon;
            using (var newIcon = Icon.FromHandle(iconBitmap.GetHicon()))
            {
                notifyIcon.Icon = newIcon;
            }
            if (oldIcon != null)
            {
                DestroyIcon(oldIcon.Handle);
                oldIcon.Dispose();
            }
            notifyIcon.Text = String.Format(@"ECView
CPU: {0:N0}℃
FAN: {1:N0}rpm / {2:N0}%",
                currentTemp, currentRpms, lastFanCuty);
        }

        private void MenuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private String ExecClevoWmiMethod(string name)
        {
            var outParams = clevoWmiInstance.InvokeMethod(name, null, null);
            if (outParams != null)
            {
                return outParams["Data"].ToString();
            }
            else
            {
                return null;
            }
        }
    }
}