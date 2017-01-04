using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ECView_CSharp.Properties;
using Timer = System.Windows.Forms.Timer;

namespace ECView_CSharp
{
    public sealed class AppContext : ApplicationContext
    {
        private const int MAX_FAN_RPM = 4400;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool DestroyIcon(IntPtr handle);

        private readonly string m_logFilePath;
        private readonly Font font = new Font("Arial Narrow", 8, FontStyle.Bold);
        private readonly Icon icon = Resources.ClevoECView;
        private readonly Bitmap iconBitmap = new Bitmap(16, 16);
        private readonly Graphics canvas;
        private readonly Brush brushBg = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
        private readonly Brush brushFg = new SolidBrush(Color.FromArgb(255, 96, 0, 0));
        private readonly Brush brushText = new SolidBrush(Color.FromArgb(255, 255, 128, 0));
        private readonly Brush brushDrop = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
        private readonly Pen penLine = new Pen(Color.FromArgb(255, 128, 0, 0), 1);
        private readonly NotifyIcon notifyIcon;
        private readonly Timer timerUI;
        private readonly ClevoControlWorker clevoControlWorker;

        public AppContext()
        {
            canvas = Graphics.FromImage(iconBitmap);
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Resources.ClevoECView;
            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Set Fan 100%", null, MenuItemSet100_Click));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Set Fan 90%", null, MenuItemSet90_Click));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Set Fan 85%", null, MenuItemSet85_Click));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Set Fan 80%", null, MenuItemSet80_Click));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Set Fan 75%", null, MenuItemSet75_Click));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Set Fan 70%", null, MenuItemSet70_Click));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Set Fan 60%", null, MenuItemSet60_Click));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Set Fan Auto", null, MenuItemSetAuto_Click) { ForeColor = Color.Blue });
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Exit", null, MenuItemExit_Click));
            notifyIcon.Text = "ECView (initializing)";
            notifyIcon.Visible = true;
            m_logFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) ?? @"C:\WINDOWS\TEMP", "error.log");
            clevoControlWorker = new ClevoControlWorker(msg => File.AppendAllText(m_logFilePath, msg));
            timerUI = new Timer { Interval = 500, Enabled = true };
            timerUI.Tick += TimerUI_Tick;
            timerUI.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
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
                clevoControlWorker.Dispose();
            }
            base.Dispose(disposing);
        }

        private void MenuItemSet100_Click(object sender, EventArgs e)
        {
            SetFanManual(100);
            UpdateMenuItemsUI(sender);
        }

        private void MenuItemSet90_Click(object sender, EventArgs e)
        {
            SetFanManual(90);
            UpdateMenuItemsUI(sender);
        }

        private void MenuItemSet85_Click(object sender, EventArgs e)
        {
            SetFanManual(85);
            UpdateMenuItemsUI(sender);
        }

        private void MenuItemSet80_Click(object sender, EventArgs e)
        {
            SetFanManual(80);
            UpdateMenuItemsUI(sender);
        }

        private void MenuItemSet75_Click(object sender, EventArgs e)
        {
            SetFanManual(75);
            UpdateMenuItemsUI(sender);
        }

        private void MenuItemSet70_Click(object sender, EventArgs e)
        {
            SetFanManual(70);
            UpdateMenuItemsUI(sender);
        }

        private void MenuItemSet60_Click(object sender, EventArgs e)
        {
            SetFanManual(60);
            UpdateMenuItemsUI(sender);
        }

        private void MenuItemSetAuto_Click(object sender, EventArgs e)
        {
            SetFanAuto();
            UpdateMenuItemsUI(sender);
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdateIcon();
                GC.Collect();
            }
            catch (Exception ex)
            {
                File.WriteAllText(m_logFilePath, DateTime.Now + "\r\n" + ex + "\r\n");
            }
        }

        private void UpdateMenuItemsUI(object sender)
        {
            foreach (var item in notifyIcon.ContextMenuStrip.Items)
                if (item == sender)
                    ((ToolStripMenuItem) item).ForeColor = Color.Blue;
                else
                    ((ToolStripMenuItem) item).ForeColor = Control.DefaultForeColor;
        }

        private void SetFanAuto()
        {
            clevoControlWorker.SetAutoFanDuty();
        }

        private void SetFanManual(int percentage)
        {
            clevoControlWorker.SetManualFanDuty(percentage);
        }

        private void UpdateIcon()
        {
            var currentRpms = clevoControlWorker.CurrentRpms;
            var currentTemp = clevoControlWorker.CurrentTemp;
            var lastFanDuty = clevoControlWorker.FanDutyRead;
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
                notifyIcon.Icon = newIcon;
            if (oldIcon != null)
            {
                DestroyIcon(oldIcon.Handle);
                oldIcon.Dispose();
            }
            notifyIcon.Text = string.Format(@"ECView
CPU: {0:N0}℃
FAN: {1:N0}rpm / {2:N0}%",
                currentTemp, currentRpms, lastFanDuty);
        }

        private void MenuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}