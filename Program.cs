// Type: ECView_CSharp.Program
// Assembly: ClevoECView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 466451CB-A6EA-4E8C-846C-A99227909B6B
// Assembly location: C:\ECView\ClevoECView.exe

using System;
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
            Application.Run(new AppContext());
        }
    }
}