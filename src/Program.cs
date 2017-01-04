// Type: ECView_CSharp.Program
// Assembly: ClevoECView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 466451CB-A6EA-4E8C-846C-A99227909B6B
// Assembly location: C:\ECView\ClevoECView.exe

using System;
using System.Threading;
using System.Windows.Forms;

namespace ECView_CSharp
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var mutex = new Mutex(false, "Global\\ClevoECView"))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("ClevoECView already running");
                    return;
                }
                Application.Run(new AppContext());
            }
        }
    }
}