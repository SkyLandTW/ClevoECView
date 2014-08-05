// Type: ECView_CSharp.CallingVariations
// Assembly: ClevoECView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 466451CB-A6EA-4E8C-846C-A99227909B6B
// Assembly location: C:\ECView\ClevoECView.exe

using System.Runtime.InteropServices;

namespace ECView_CSharp
{
    public class CallingVariations
    {
        [DllImport("ecview.dll", EntryPoint = "#3")]
        public static extern void SetFanDuty(int p1, int p2);

        [DllImport("ecview.dll", EntryPoint = "#4")]
        public static extern int SetFANDutyAuto(int p1);

        [DllImport("ecview.dll", EntryPoint = "#5")]
        public static extern ECData GetTempFanDuty(int p1);

        [DllImport("ecview.dll", EntryPoint = "#6")]
        public static extern int GetFANCounter();

        [DllImport("ecview.dll", EntryPoint = "#8")]
        public static extern string GetECVersion();

        [DllImport("ecview.dll")]
        public static extern int GetCPUFANRPM();

        [DllImport("ecview.dll")]
        public static extern int GetGPUFANRPM();

        [DllImport("ecview.dll")]
        public static extern int GetGPU1FANRPM();

        [DllImport("ecview.dll")]
        public static extern int GetX72FANRPM();

        public struct ECData
        {
            public byte Remote;
            public byte Local;
            public byte FanDuty;
        }
    }
}