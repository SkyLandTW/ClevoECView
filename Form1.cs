// Type: ECView_CSharp.Form1
// Assembly: ClevoECView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 466451CB-A6EA-4E8C-846C-A99227909B6B
// Assembly location: C:\ECView\ClevoECView.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using Timer = System.Windows.Forms.Timer;

namespace ECView_CSharp
{
    public class Form1 : Form
    {
        private ManagementObjectSearcher Searcher_BaseBoard = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
        public const int WM_CLOSE = 16;
        private RegistryKey LSPKey;
        public int hours;
        public int HDDTemp;
        public bool FAN_Auto1;
        public bool FAN_Auto2;
        public bool FAN_Auto3;
        public bool FAN_Auto4;
        public int FAN_Counter;
        public string varFanAmount;
        public string varTimer;
        public bool Lock;
        public bool LockHDD;
        public bool LockRPM;
        public int HDDInstance;
        public string[,] services;
        public bool RAID;
        public int getCPURPM;
        public int getGPURPM;
        public int getGPU1RPM;
        public int getSYSRPM;
        public double fl_CPURPM;
        public double fl_GPURPM;
        public double fl_GPU1RPM;
        public double fl_SYSRPM;
        public Decimal de_CPURPM;
        public Decimal de_GPURPM;
        public Decimal de_GPU1RPM;
        public Decimal de_SYSRPM;
        public bool Rec;
        private IContainer components;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem settingToolStripMenuItem;
        private ToolStripMenuItem timerToolStripMenuItem;
        private ToolStripMenuItem fanToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem lockToolStripMenuItem;
        private Label label1;
        private Label LabNBModel;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label labCPURemote;
        private Label label3;
        private Label label4;
        private Label labVGA1Remote;
        private Label label6;
        private Label labVGA2Remote;
        private Label label8;
        private Label labCPULocal;
        private Label label10;
        private Label labVGA1Local;
        private Label label12;
        private Label labVGA2Local;
        private Label label14;
        private CheckBox checkBox1;
        private TextBox txtCPUFanDuty;
        private TextBox txtCPUDuty_Hex;
        private Label label15;
        private Label label16;
        private TextBox txtVGA1FanDuty;
        private TextBox txtVGA1Duty_Hex;
        private CheckBox checkBox2;
        private Label label17;
        private Label label18;
        private TextBox txtVGA2FanDuty;
        private TextBox txtVGA2Duty_Hex;
        private CheckBox checkBox3;
        private Label label19;
        private Timer timer1;
        private Label label20;
        private Label LabECVersion;
        private GroupBox groupBox4;
        private Label labHDD1Temp;
        private Label LabHDD1Model;
        private Label labHDD1UseHours;
        private GroupBox groupBox5;
        private Label labHDD2UseHours;
        private Label LabHDD2Model;
        private Label labHDD2Temp;
        private GroupBox groupBox6;
        private Label labHDD3UseHours;
        private Label LabHDD3Model;
        private Label labHDD3Temp;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown3;
        private Label labRecord;
        private ToolStripMenuItem hddStripMenuItem;
        private Label labCPURPM;
        private Label labGPURPM;
        private ToolStripMenuItem rPMToolStripMenuItem;
        private Label label2;
        private Label label5;
        private Label labLock;
        private GroupBox groupBox7;
        private Label labGPU1RPM;
        private Label label7;
        private GroupBox groupBox8;
        private Label labSYSRPM;
        private Label label11;
        private NumericUpDown numericUpDown4;
        private Label label13;
        private Label label21;
        private Label label22;
        private TextBox txtSYSFanDuty;
        private Label labSYSLocal;
        private TextBox txtSYSDuty_Hex;
        private Label labSYSRemote;
        private CheckBox checkBox4;
        private Label label25;

        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private void Timer_Tick(object sender, EventArgs e)
        {
            KillMessageBox();
            ((Timer) sender).Stop();
        }

        private void KillMessageBox()
        {
            var window = FindWindow(null, "MessageBox");
            if (!(window != IntPtr.Zero))
                return;
            PostMessage(window, 16, IntPtr.Zero, IntPtr.Zero);
        }

        /*
        private void FixUACSettings()
        {
            try
            {
                using (LSPKey = Registry.LocalMachine)
                {
                    LSPKey = LSPKey.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
                    // ReSharper disable PossibleNullReferenceException
                    LSPKey.SetValue("EnableLUA", 0);
                    LSPKey.SetValue("ConsentPromptBehaviorAdmin", 0);
                    LSPKey.SetValue("ConsentPromptBehaviorUser", 1);
                    // ReSharper restore PossibleNullReferenceException
                }
            }
            catch (Exception ex)
            {
                var num = (int) MessageBox.Show(ex.ToString());
            }
            finally
            {
                // ReSharper disable PossibleNullReferenceException
                LSPKey.Close();
                // ReSharper restore PossibleNullReferenceException
            }
        }
         */

        private void Form1_Load(object sender, EventArgs e)
        {
            Lock = false;
            LockHDD = true;
            LockRPM = true;
            foreach (var managementBaseObject in Searcher_BaseBoard.Get())
                LabNBModel.Text = Convert.ToString(managementBaseObject["Product"]);
            LabECVersion.Text = "1." + CallingVariations.GetECVersion();
            FAN_Counter = CallingVariations.GetFANCounter();
            varFanAmount = Convert.ToString(FAN_Counter);
            if (FAN_Counter > 4)
                FAN_Counter = 0;
            if (FAN_Counter == 0)
                FAN_Counter = 1;
            if (FAN_Counter == 1)
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = false;
                groupBox3.Enabled = false;
                groupBox7.Size = new Size(453, 221);
            }
            else if (FAN_Counter == 2)
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                groupBox3.Enabled = false;
                groupBox7.Size = new Size(453, 221);
            }
            else if (FAN_Counter == 3)
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                groupBox3.Enabled = true;
                groupBox7.Size = new Size(453, 221);
            }
            else if (FAN_Counter == 4)
                groupBox7.Size = new Size(607, 221);
            if (!LockRPM)
            {
                label2.Text = CallingVariations.GetCPUFANRPM().ToString();
                label5.Text = CallingVariations.GetGPUFANRPM().ToString();
            }
            else
            {
                labCPURPM.Visible = false;
                labGPURPM.Visible = false;
            }
            var managementClass = new ManagementClass("Win32_DiskDrive");
            managementClass.CreateInstance();
            services = new string[10, 3];
            var index = 0;
            foreach (ManagementObject managementObject in managementClass.GetInstances())
            {
                var str1 = Convert.ToString(managementObject["Model"]);
                var str2 = "USB";
                var str3 = "STRIPE";
                var flag1 = str1.Contains(str2);
                var flag2 = str1.Contains(str3);
                if (!flag1 && !flag2)
                {
                    var moIndex = Convert.ToString(managementObject["Index"]);
                    var moSize = Convert.ToString(managementObject["Size"]);
                    services[index, 0] = moIndex;
                    services[index, 1] = str1;
                    services[index, 2] = moSize;
                }
                else if (flag2)
                    RAID = true;
                ++index;
            }
            if (services != null)
            {
                if (RAID)
                {
                    groupBox4.Enabled = false;
                    groupBox5.Enabled = false;
                    groupBox6.Enabled = false;
                }
                else
                {
                    groupBox4.Text = "HDD index: " + services[0, 0];
                    groupBox5.Text = "HDD index: " + services[1, 0];
                    groupBox6.Text = "HDD index: " + services[2, 0];
                    LabHDD1Model.Text = services[0, 1];
                    LabHDD1Model.Size = new Size(110, 50);
                    LabHDD2Model.Text = services[1, 1];
                    LabHDD2Model.Size = new Size(110, 50);
                    LabHDD3Model.Text = services[2, 1];
                    LabHDD3Model.Size = new Size(110, 50);
                }
            }
            managementClass.Dispose();
            FAN_Auto1 = true;
            FAN_Auto2 = true;
            FAN_Auto3 = true;
            FAN_Auto4 = true;
            Rec = false;
            checkBox1.Checked = true;
            numericUpDown1.Enabled = false;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            checkBox2.Checked = true;
            numericUpDown2.Enabled = false;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            checkBox3.Checked = true;
            numericUpDown3.Enabled = false;
            checkBox3.CheckedChanged += checkBox3_CheckedChanged;
            checkBox4.Checked = true;
            numericUpDown4.Enabled = false;
            checkBox4.CheckedChanged += checkBox4_CheckedChanged;
            timer1.Enabled = true;
            hddStripMenuItem.PerformClick();
            rPMToolStripMenuItem.PerformClick();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (FAN_Counter > 0)
            {
                var tempFanDuty = CallingVariations.GetTempFanDuty(1);
                int num1 = tempFanDuty.Remote;
                if (num1 == 144 || num1 < 10)
                    labCPURemote.Text = "---";
                else
                    labCPURemote.Text = Convert.ToString(num1);
                int num2 = tempFanDuty.Local;
                if (num2 == 144 || num2 < 10)
                    labCPULocal.Text = "---";
                else
                    labCPULocal.Text = Convert.ToString(num2);
                int num3 = tempFanDuty.FanDuty;
                txtCPUFanDuty.Text = Convert.ToString(num3);
                if (num3 < 16)
                    txtCPUDuty_Hex.Text = "0" + Convert.ToString(num3, 16);
                else
                    txtCPUDuty_Hex.Text = Convert.ToString(num3, 16);
                if (checkBox1.Checked)
                {
                    numericUpDown1.Text = Convert.ToString(Math.Ceiling(num3 / 2.55));
                    var num4 = num3 * 100 / byte.MaxValue;
                }
            }
            if (FAN_Counter > 1)
            {
                Thread.Sleep(100);
                var tempFanDuty = CallingVariations.GetTempFanDuty(2);
                int num1 = tempFanDuty.Remote;
                if (num1 == 144 || num1 < 10)
                    labVGA1Remote.Text = "---";
                else
                    labVGA1Remote.Text = Convert.ToString(num1);
                int num2 = tempFanDuty.Local;
                if (num2 == 144 || num2 < 10)
                    labVGA1Local.Text = "---";
                else
                    labVGA1Local.Text = Convert.ToString(num2);
                int num3 = tempFanDuty.FanDuty;
                txtVGA1FanDuty.Text = Convert.ToString(num3);
                if (num3 < 16)
                    txtVGA1Duty_Hex.Text = "0" + Convert.ToString(num3, 16);
                else
                    txtVGA1Duty_Hex.Text = Convert.ToString(num3, 16);
                if (checkBox2.Checked)
                {
                    numericUpDown2.Text = Convert.ToString(Math.Ceiling(num3 / 2.55));
                    var num4 = num3 * 100 / byte.MaxValue;
                }
            }
            if (FAN_Counter > 2)
            {
                Thread.Sleep(100);
                var tempFanDuty = CallingVariations.GetTempFanDuty(3);
                int num1 = tempFanDuty.Remote;
                if (num1 == 144 || num1 < 10)
                    labVGA2Remote.Text = "---";
                else
                    labVGA2Remote.Text = Convert.ToString(num1);
                int num2 = tempFanDuty.Local;
                if (num2 == 144 || num2 < 10)
                    labVGA2Local.Text = "---";
                else
                    labVGA2Local.Text = Convert.ToString(num2);
                int num3 = tempFanDuty.FanDuty;
                txtVGA2FanDuty.Text = Convert.ToString(num3);
                if (num3 < 16)
                    txtVGA2Duty_Hex.Text = "0" + Convert.ToString(num3, 16);
                else
                    txtVGA2Duty_Hex.Text = Convert.ToString(num3, 16);
                if (checkBox3.Checked)
                {
                    numericUpDown3.Text = Convert.ToString(Math.Ceiling(num3 / 2.55));
                    var num4 = num3 * 100 / byte.MaxValue;
                }
            }
            if (varFanAmount == "4")
            {
                Thread.Sleep(100);
                var tempFanDuty = CallingVariations.GetTempFanDuty(4);
                int num1 = tempFanDuty.Remote;
                if (num1 == 144 || num1 < 10)
                    labSYSRemote.Text = "---";
                else
                    labSYSRemote.Text = Convert.ToString(num1);
                int num2 = tempFanDuty.Local;
                if (num2 == 144 || num2 < 10)
                    labSYSLocal.Text = "---";
                else
                    labSYSLocal.Text = Convert.ToString(tempFanDuty);
                int num3 = tempFanDuty.FanDuty;
                txtSYSFanDuty.Text = Convert.ToString(num3);
                if (num3 < 16)
                    txtSYSDuty_Hex.Text = "0" + Convert.ToString(num3, 16);
                else
                    txtSYSDuty_Hex.Text = Convert.ToString(num3, 16);
                if (checkBox4.Checked)
                {
                    numericUpDown4.Text = Convert.ToString(Math.Ceiling(num3 / 2.55));
                    var num4 = num3 * 100 / byte.MaxValue;
                }
            }
            if (!LockRPM)
            {
                Thread.Sleep(100);
                getCPURPM = CallingVariations.GetCPUFANRPM();
                if (FAN_Counter > 1)
                {
                    Thread.Sleep(100);
                    getGPURPM = CallingVariations.GetGPUFANRPM();
                }
                if (FAN_Counter > 2)
                {
                    Thread.Sleep(100);
                    getGPU1RPM = CallingVariations.GetGPU1FANRPM();
                }
                if (FAN_Counter > 3)
                {
                    Thread.Sleep(100);
                    getSYSRPM = CallingVariations.GetX72FANRPM();
                }
                if (getCPURPM != 0)
                {
                    fl_CPURPM = 60.0 / (1.39130434782609E-05 * getCPURPM * 4.0);
                    fl_CPURPM = fl_CPURPM * 2.0;
                    de_CPURPM = Math.Round(Convert.ToDecimal(fl_CPURPM), 0);
                }
                else
                    de_CPURPM = new Decimal(0);
                if (getGPURPM != 0)
                {
                    fl_GPURPM = 60.0 / (1.39130434782609E-05 * getGPURPM * 4.0);
                    fl_GPURPM = fl_GPURPM * 2.0;
                    de_GPURPM = Math.Round(Convert.ToDecimal(fl_GPURPM), 0);
                }
                else
                    de_GPURPM = new Decimal(0);
                if (getGPU1RPM != 0)
                {
                    fl_GPU1RPM = 60.0 / (1.39130434782609E-05 * getGPU1RPM * 4.0);
                    fl_GPU1RPM = fl_GPU1RPM * 2.0;
                    de_GPU1RPM = Math.Round(Convert.ToDecimal(fl_GPU1RPM), 0);
                }
                else
                    de_GPU1RPM = new Decimal(0);
                if (getSYSRPM != 0)
                {
                    fl_SYSRPM = 60.0 / (1.39130434782609E-05 * getSYSRPM * 4.0);
                    fl_SYSRPM = fl_SYSRPM * 2.0;
                    de_SYSRPM = Math.Round(Convert.ToDecimal(fl_SYSRPM), 0);
                }
                else
                    de_SYSRPM = new Decimal(0);
                labCPURPM.Text = Convert.ToString(de_CPURPM);
                labGPURPM.Text = Convert.ToString(de_GPURPM);
                if (LabNBModel.Text.Contains("X72"))
                {
                    labGPU1RPM.Text = Convert.ToString(de_SYSRPM);
                    labSYSRPM.Text = Convert.ToString(de_GPU1RPM);
                }
                else
                {
                    labGPU1RPM.Text = Convert.ToString(de_GPU1RPM);
                    labSYSRPM.Text = Convert.ToString(de_SYSRPM);
                }
            }
            if (!RAID)
            {
                if (!LockHDD)
                {
                    var managementObjectSearcher = new ManagementObjectSearcher("root\\WMI", "select * from MSStorageDriver_ATAPISmartData");
                    HDDInstance = managementObjectSearcher.Get().Count;
                    var numArray1 = new int[30];
                    var numArray2 = new int[30];
                    var index = 0;
                    foreach (var managementBaseObject in managementObjectSearcher.Get())
                    {
                        var numArray3 = (byte[]) managementBaseObject["VendorSpecific"];
                        foreach (int num in numArray3)
                        {
                            numArray1[index] = numArray3[151];
                            var str = Convert.ToString((int) numArray3[92], 16) + Convert.ToString((int) numArray3[91], 16);
                            numArray2[index] = Convert.ToInt32(str, 16);
                        }
                        ++index;
                    }
                    if (numArray1 != null && numArray2 != null)
                    {
                        labHDD1Temp.Text = Convert.ToString(numArray1[0]);
                        labHDD2Temp.Text = Convert.ToString(numArray1[1]);
                        labHDD3Temp.Text = Convert.ToString(numArray1[2]);
                        labHDD1UseHours.Text = "UseHours: " + Convert.ToString(numArray2[0]);
                        labHDD2UseHours.Text = "UseHours: " + Convert.ToString(numArray2[1]);
                        labHDD3UseHours.Text = "UseHours: " + Convert.ToString(numArray2[2]);
                    }
                }
            }
            else
            {
                labHDD1Temp.Text = "";
                labHDD2Temp.Text = "";
                labHDD3Temp.Text = "";
            }
            if (Rec)
            {
                labRecord.Text = "Save file";
                var file = "C:\\ECView\\ECView.txt";
                try
                {
                    if (FAN_Counter == 4)
                    {
                        var text1 = DateTime.Now + "  CPU-> " + Convert.ToString(labCPURemote.Text) + "℃ " + Convert.ToString(labCPULocal.Text) + "℃ " + txtCPUDuty_Hex.Text + "-" + Convert.ToString(numericUpDown1.Value) + "%-" + labCPURPM.Text;
                        FileSystem.WriteAllText(file, text1, true);
                        var text2 = ",  VGA1-> " + Convert.ToString(labVGA1Remote.Text) + "℃ " + Convert.ToString(labVGA1Local.Text) + "℃ " + txtVGA1Duty_Hex.Text + "-" + Convert.ToString(numericUpDown2.Value) + "%-" + labGPURPM.Text;
                        FileSystem.WriteAllText(file, text2, true);
                        var text3 = ",  VGA2-> " + Convert.ToString(labVGA2Remote.Text) + "℃ " + Convert.ToString(labVGA2Local.Text) + "℃ " + txtVGA2Duty_Hex.Text + "-" + Convert.ToString(numericUpDown3.Value) + "%-" + labGPU1RPM.Text;
                        FileSystem.WriteAllText(file, text3, true);
                        var text4 = ",  SYS-> " + Convert.ToString(labSYSRemote.Text) + "℃ " + Convert.ToString(labSYSLocal.Text) + "℃ " + txtSYSDuty_Hex.Text + "-" + Convert.ToString(numericUpDown4.Value) + "%-" + labSYSRPM.Text + "\r\n";
                        FileSystem.WriteAllText(file, text4, true);
                    }
                    if (FAN_Counter == 3)
                    {
                        var text1 = DateTime.Now + "  CPU-> " + Convert.ToString(labCPURemote.Text) + "℃ " + Convert.ToString(labCPULocal.Text) + "℃ " + txtCPUDuty_Hex.Text + "-" + Convert.ToString(numericUpDown1.Value) + "%-" + labCPURPM.Text;
                        FileSystem.WriteAllText(file, text1, true);
                        var text2 = ",  VGA1-> " + Convert.ToString(labVGA1Remote.Text) + "℃ " + Convert.ToString(labVGA1Local.Text) + "℃ " + txtVGA1Duty_Hex.Text + "-" + Convert.ToString(numericUpDown2.Value) + "%-" + labGPURPM.Text;
                        FileSystem.WriteAllText(file, text2, true);
                        var text3 = ",  VGA2-> " + Convert.ToString(labVGA2Remote.Text) + "℃ " + Convert.ToString(labVGA2Local.Text) + "℃ " + txtVGA2Duty_Hex.Text + "-" + Convert.ToString(numericUpDown3.Value) + "%-" + labGPU1RPM.Text + "\r\n";
                        FileSystem.WriteAllText(file, text3, true);
                    }
                    else if (FAN_Counter == 2)
                    {
                        var text1 = DateTime.Now + "  CPU-> " + Convert.ToString(labCPURemote.Text) + "℃ " + Convert.ToString(labCPULocal.Text) + "℃ " + txtCPUDuty_Hex.Text + "-" + Convert.ToString(numericUpDown1.Value) + "%-" + labCPURPM.Text;
                        FileSystem.WriteAllText(file, text1, true);
                        var text2 = ",  VGA1-> " + Convert.ToString(labVGA1Remote.Text) + "℃ " + Convert.ToString(labVGA1Local.Text) + "℃ " + txtVGA1Duty_Hex.Text + "-" + Convert.ToString(numericUpDown2.Value) + "%-" + labGPURPM.Text + "\r\n";
                        FileSystem.WriteAllText(file, text2, true);
                    }
                    else if (FAN_Counter == 1)
                    {
                        var text = DateTime.Now + "  CPU-> " + Convert.ToString(labCPURemote.Text) + "℃ " + Convert.ToString(labCPULocal.Text) + "℃ " + txtCPUDuty_Hex.Text + "-" + Convert.ToString(numericUpDown1.Value) + "%-" + labCPURPM.Text + "\r\n";
                        FileSystem.WriteAllText(file, text, true);
                    }
                    if (RAID)
                        return;
                    if (HDDInstance == 1)
                    {
                        var text = " HDD " + services[0, 0] + " ; " + Convert.ToString(labHDD1Temp.Text) + "\r\n";
                        FileSystem.WriteAllText(file, text, true);
                    }
                    else if (HDDInstance == 2)
                    {
                        var text = " HDD " + services[0, 0] + " ; " + Convert.ToString(labHDD1Temp.Text) + " ; HDD " + services[1, 0] + " ; " + Convert.ToString(labHDD2Temp.Text) + "\r\n";
                        FileSystem.WriteAllText(file, text, true);
                    }
                    else
                    {
                        if (HDDInstance != 3)
                            return;
                        var text = " HDD " + services[0, 0] + " ; " + Convert.ToString(labHDD1Temp.Text) + " ; HDD " + services[1, 0] + " ; " + Convert.ToString(labHDD2Temp.Text) + " ; HDD " + services[2, 0] + " ; " + Convert.ToString(labHDD3Temp.Text) + "\r\n";
                        FileSystem.WriteAllText(file, text, true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
                labRecord.Text = "Not Save";
        }

        public void fanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var inputFanCount = new Input_FanCount();
            if (inputFanCount.ShowDialog() == DialogResult.OK)
                varFanAmount = inputFanCount.FanCount;
            FAN_Counter = Convert.ToInt32(varFanAmount);
            if (FAN_Counter == 1)
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = false;
                groupBox3.Enabled = false;
                groupBox7.Size = new Size(453, 221);
            }
            else if (FAN_Counter == 2)
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                groupBox3.Enabled = false;
                groupBox7.Size = new Size(453, 221);
            }
            else if (FAN_Counter == 3)
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                groupBox3.Enabled = true;
                groupBox7.Size = new Size(453, 221);
            }
            else
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                groupBox3.Enabled = true;
                groupBox7.Size = new Size(607, 221);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveToolStripMenuItem.Checked)
                Rec = true;
            else
                Rec = false;
        }

        private void timerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var inputSetTimer = new Input_SetTimer();
            if (inputSetTimer.ShowDialog() != DialogResult.OK)
                return;
            varTimer = inputSetTimer.SetTimer;
            if (varTimer == null)
                timer1.Interval = 3000;
            else
                timer1.Interval = Convert.ToInt32(varTimer) * 1000;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                CallingVariations.SetFANDutyAuto(1);
                numericUpDown1.Enabled = false;
                FAN_Auto1 = true;
            }
            else
            {
                numericUpDown1.Enabled = true;
                numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
                FAN_Auto1 = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                CallingVariations.SetFANDutyAuto(2);
                numericUpDown2.Enabled = false;
                FAN_Auto2 = true;
            }
            else
            {
                numericUpDown2.Enabled = true;
                numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;
                FAN_Auto2 = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                CallingVariations.SetFANDutyAuto(3);
                numericUpDown3.Enabled = false;
                FAN_Auto3 = true;
            }
            else
            {
                numericUpDown3.Enabled = true;
                numericUpDown3.ValueChanged += numericUpDown3_ValueChanged;
                FAN_Auto3 = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                CallingVariations.SetFANDutyAuto(4);
                numericUpDown4.Enabled = false;
                FAN_Auto4 = true;
            }
            else
            {
                numericUpDown4.Enabled = true;
                numericUpDown4.ValueChanged += numericUpDown4_ValueChanged;
                FAN_Auto4 = false;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // CallingVariations.SetFANDutyAuto(4);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (FAN_Auto1 || Lock)
                return;
            var num = (double) numericUpDown1.Value * 2.55;
            if (numericUpDown1.Value == new Decimal(100))
                num = byte.MaxValue;
            CallingVariations.SetFanDuty(1, (int) num);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (FAN_Auto2 || Lock)
                return;
            var num = (double) numericUpDown2.Value * 2.55;
            if (numericUpDown2.Value == new Decimal(100))
                num = byte.MaxValue;
            CallingVariations.SetFanDuty(2, (int) num);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (FAN_Auto3 || Lock)
                return;
            var num = (double) numericUpDown3.Value * 2.55;
            if (numericUpDown3.Value == new Decimal(100))
                num = byte.MaxValue;
            CallingVariations.SetFanDuty(3, (int) num);
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (FAN_Auto4 || Lock)
                return;
            var num = (double) numericUpDown4.Value * 2.55;
            if (numericUpDown4.Value == new Decimal(100))
                num = byte.MaxValue;
            CallingVariations.SetFanDuty(4, (int) num);
        }

        private void lockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lockToolStripMenuItem.Checked)
            {
                Lock = true;
                txtCPUDuty_Hex.Enabled = false;
                txtCPUFanDuty.Enabled = false;
                numericUpDown1.Enabled = false;
                checkBox1.Enabled = false;
                txtVGA1Duty_Hex.Enabled = false;
                txtVGA1FanDuty.Enabled = false;
                numericUpDown2.Enabled = false;
                checkBox2.Enabled = false;
                txtVGA2Duty_Hex.Enabled = false;
                txtVGA2FanDuty.Enabled = false;
                numericUpDown3.Enabled = false;
                checkBox3.Enabled = false;
                labLock.Text = "Lock";
            }
            else
            {
                Lock = false;
                txtCPUDuty_Hex.Enabled = true;
                txtCPUFanDuty.Enabled = true;
                numericUpDown1.Enabled = true;
                checkBox1.Enabled = true;
                txtVGA1Duty_Hex.Enabled = true;
                txtVGA1FanDuty.Enabled = true;
                numericUpDown2.Enabled = true;
                checkBox2.Enabled = true;
                txtVGA2Duty_Hex.Enabled = true;
                txtVGA2FanDuty.Enabled = true;
                numericUpDown3.Enabled = true;
                checkBox3.Enabled = true;
                labLock.Text = "";
            }
        }

        private void hddStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hddStripMenuItem.Checked)
            {
                LockHDD = false;
                groupBox4.Enabled = true;
                groupBox5.Enabled = true;
                groupBox6.Enabled = true;
                groupBox4.Visible = true;
                groupBox5.Visible = true;
                groupBox6.Visible = true;
            }
            else
            {
                LockHDD = true;
                groupBox4.Enabled = false;
                groupBox5.Enabled = false;
                groupBox6.Enabled = false;
                groupBox4.Visible = false;
                groupBox5.Visible = false;
                groupBox6.Visible = false;
            }
        }

        private void rPMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rPMToolStripMenuItem.Checked)
            {
                LockRPM = false;
                label2.Visible = true;
                label5.Visible = true;
                label7.Visible = true;
                label11.Visible = true;
                labCPURPM.Visible = true;
                labGPURPM.Visible = true;
                labGPU1RPM.Visible = true;
                labSYSRPM.Visible = true;
            }
            else
            {
                label2.Visible = false;
                label5.Visible = false;
                label7.Visible = false;
                label11.Visible = false;
                LockRPM = true;
                labCPURPM.Visible = false;
                labGPURPM.Visible = false;
                labGPU1RPM.Visible = false;
                labSYSRPM.Visible = false;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void txtVGA2FanDuty_TextChanged(object sender, EventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            var componentResourceManager = new ComponentResourceManager(typeof(Form1));
            menuStrip1 = new MenuStrip();
            settingToolStripMenuItem = new ToolStripMenuItem();
            timerToolStripMenuItem = new ToolStripMenuItem();
            fanToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            lockToolStripMenuItem = new ToolStripMenuItem();
            hddStripMenuItem = new ToolStripMenuItem();
            rPMToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            LabNBModel = new Label();
            groupBox1 = new GroupBox();
            label2 = new Label();
            labCPURPM = new Label();
            numericUpDown1 = new NumericUpDown();
            label15 = new Label();
            txtCPUFanDuty = new TextBox();
            txtCPUDuty_Hex = new TextBox();
            checkBox1 = new CheckBox();
            label14 = new Label();
            label8 = new Label();
            label3 = new Label();
            labCPULocal = new Label();
            labCPURemote = new Label();
            groupBox2 = new GroupBox();
            label5 = new Label();
            labGPURPM = new Label();
            numericUpDown2 = new NumericUpDown();
            label16 = new Label();
            label10 = new Label();
            label4 = new Label();
            txtVGA1FanDuty = new TextBox();
            labVGA1Local = new Label();
            txtVGA1Duty_Hex = new TextBox();
            labVGA1Remote = new Label();
            checkBox2 = new CheckBox();
            label17 = new Label();
            groupBox3 = new GroupBox();
            labGPU1RPM = new Label();
            label7 = new Label();
            numericUpDown3 = new NumericUpDown();
            label18 = new Label();
            label12 = new Label();
            label6 = new Label();
            txtVGA2FanDuty = new TextBox();
            labVGA2Local = new Label();
            txtVGA2Duty_Hex = new TextBox();
            labVGA2Remote = new Label();
            checkBox3 = new CheckBox();
            label19 = new Label();
            timer1 = new Timer(components);
            label20 = new Label();
            LabECVersion = new Label();
            groupBox4 = new GroupBox();
            labHDD1UseHours = new Label();
            LabHDD1Model = new Label();
            labHDD1Temp = new Label();
            groupBox5 = new GroupBox();
            labHDD2UseHours = new Label();
            LabHDD2Model = new Label();
            labHDD2Temp = new Label();
            groupBox6 = new GroupBox();
            labHDD3UseHours = new Label();
            LabHDD3Model = new Label();
            labHDD3Temp = new Label();
            labRecord = new Label();
            labLock = new Label();
            groupBox7 = new GroupBox();
            groupBox8 = new GroupBox();
            labSYSRPM = new Label();
            label11 = new Label();
            numericUpDown4 = new NumericUpDown();
            label13 = new Label();
            label21 = new Label();
            label22 = new Label();
            txtSYSFanDuty = new TextBox();
            labSYSLocal = new Label();
            txtSYSDuty_Hex = new TextBox();
            labSYSRemote = new Label();
            checkBox4 = new CheckBox();
            label25 = new Label();
            menuStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            numericUpDown1.BeginInit();
            groupBox2.SuspendLayout();
            numericUpDown2.BeginInit();
            groupBox3.SuspendLayout();
            numericUpDown3.BeginInit();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox7.SuspendLayout();
            groupBox8.SuspendLayout();
            numericUpDown4.BeginInit();
            SuspendLayout();
            menuStrip1.Items.AddRange(new ToolStripItem[]
                {
                    settingToolStripMenuItem
                });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(620, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            settingToolStripMenuItem.AccessibleRole = AccessibleRole.Grip;
            settingToolStripMenuItem.Checked = true;
            settingToolStripMenuItem.CheckOnClick = true;
            settingToolStripMenuItem.CheckState = CheckState.Checked;
            settingToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
                {
                    timerToolStripMenuItem,
                    fanToolStripMenuItem,
                    saveToolStripMenuItem,
                    lockToolStripMenuItem,
                    hddStripMenuItem,
                    rPMToolStripMenuItem
                });
            settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            settingToolStripMenuItem.Size = new Size(56, 20);
            settingToolStripMenuItem.Text = "Setting";
            timerToolStripMenuItem.Name = "timerToolStripMenuItem";
            timerToolStripMenuItem.Size = new Size(165, 22);
            timerToolStripMenuItem.Text = "Timer";
            timerToolStripMenuItem.Click += timerToolStripMenuItem_Click;
            fanToolStripMenuItem.Name = "fanToolStripMenuItem";
            fanToolStripMenuItem.Size = new Size(165, 22);
            fanToolStripMenuItem.Text = "Fan Amount";
            fanToolStripMenuItem.Click += fanToolStripMenuItem_Click;
            saveToolStripMenuItem.CheckOnClick = true;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(165, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            lockToolStripMenuItem.CheckOnClick = true;
            lockToolStripMenuItem.Name = "lockToolStripMenuItem";
            lockToolStripMenuItem.Size = new Size(165, 22);
            lockToolStripMenuItem.Text = "Lock";
            lockToolStripMenuItem.Click += lockToolStripMenuItem_Click;
            hddStripMenuItem.CheckOnClick = true;
            hddStripMenuItem.Name = "hddStripMenuItem";
            hddStripMenuItem.Size = new Size(165, 22);
            hddStripMenuItem.Text = "HDD Information";
            hddStripMenuItem.Click += hddStripMenuItem_Click;
            rPMToolStripMenuItem.CheckOnClick = true;
            rPMToolStripMenuItem.Name = "rPMToolStripMenuItem";
            rPMToolStripMenuItem.Size = new Size(165, 22);
            rPMToolStripMenuItem.Text = "RPM";
            rPMToolStripMenuItem.Click += rPMToolStripMenuItem_Click;
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.MidnightBlue;
            label1.Location = new Point(159, 33);
            label1.Name = "label1";
            label1.Size = new Size(68, 15);
            label1.TabIndex = 1;
            label1.Text = "NB Model :";
            LabNBModel.AutoSize = true;
            LabNBModel.Font = new Font("Times New Roman", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            LabNBModel.ForeColor = Color.Red;
            LabNBModel.Location = new Point(223, 33);
            LabNBModel.Name = "LabNBModel";
            LabNBModel.Size = new Size(27, 15);
            LabNBModel.TabIndex = 2;
            LabNBModel.Text = "-----";
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(labCPURPM);
            groupBox1.Controls.Add(numericUpDown1);
            groupBox1.Controls.Add(label15);
            groupBox1.Controls.Add(txtCPUFanDuty);
            groupBox1.Controls.Add(txtCPUDuty_Hex);
            groupBox1.Controls.Add(checkBox1);
            groupBox1.Controls.Add(label14);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(labCPULocal);
            groupBox1.Controls.Add(labCPURemote);
            groupBox1.Font = new Font("Times New Roman", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(12, 19);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(141, 178);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "CPU";
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(6, 101);
            label2.Name = "label2";
            label2.Size = new Size(34, 15);
            label2.TabIndex = 16;
            label2.Text = "RPM";
            label2.Visible = false;
            label2.Click += label2_Click;
            labCPURPM.AutoSize = true;
            labCPURPM.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            labCPURPM.Location = new Point(69, 100);
            labCPURPM.Name = "labCPURPM";
            labCPURPM.Size = new Size(19, 15);
            labCPURPM.TabIndex = 15;
            labCPURPM.Text = "---";
            labCPURPM.Visible = false;
            numericUpDown1.Font = new Font("Times New Roman", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            numericUpDown1.Location = new Point(66, 152);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(44, 22);
            numericUpDown1.TabIndex = 3;
            label15.AutoSize = true;
            label15.Location = new Point(110, 155);
            label15.Name = "label15";
            label15.Size = new Size(25, 19);
            label15.TabIndex = 14;
            label15.Text = "%";
            txtCPUFanDuty.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCPUFanDuty.ForeColor = SystemColors.HotTrack;
            txtCPUFanDuty.Location = new Point(95, 121);
            txtCPUFanDuty.Name = "txtCPUFanDuty";
            txtCPUFanDuty.Size = new Size(34, 20);
            txtCPUFanDuty.TabIndex = 12;
            txtCPUFanDuty.Text = "000";
            txtCPUDuty_Hex.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCPUDuty_Hex.ForeColor = SystemColors.HotTrack;
            txtCPUDuty_Hex.Location = new Point(66, 121);
            txtCPUDuty_Hex.Name = "txtCPUDuty_Hex";
            txtCPUDuty_Hex.Size = new Size(26, 20);
            txtCPUDuty_Hex.TabIndex = 11;
            txtCPUDuty_Hex.Text = "aa";
            checkBox1.AutoSize = true;
            checkBox1.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            checkBox1.Location = new Point(9, 158);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(51, 19);
            checkBox1.TabIndex = 10;
            checkBox1.Text = "Auto";
            checkBox1.UseVisualStyleBackColor = true;
            label14.AutoSize = true;
            label14.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label14.Location = new Point(6, 126);
            label14.Name = "label14";
            label14.Size = new Size(51, 15);
            label14.TabIndex = 9;
            label14.Text = "FanDuty";
            label8.AutoSize = true;
            label8.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(83, 69);
            label8.Name = "label8";
            label8.Size = new Size(33, 14);
            label8.TabIndex = 8;
            label8.Text = "Local";
            label3.AutoSize = true;
            label3.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 69);
            label3.Name = "label3";
            label3.Size = new Size(43, 14);
            label3.TabIndex = 1;
            label3.Text = "Remote";
            labCPULocal.AutoSize = true;
            labCPULocal.Font = new Font("Tahoma", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
            labCPULocal.ForeColor = SystemColors.Highlight;
            labCPULocal.Location = new Point(77, 30);
            labCPULocal.Name = "labCPULocal";
            labCPULocal.Size = new Size(58, 29);
            labCPULocal.TabIndex = 7;
            labCPULocal.Text = "000";
            labCPURemote.AutoSize = true;
            labCPURemote.Font = new Font("Tahoma", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
            labCPURemote.ForeColor = SystemColors.HotTrack;
            labCPURemote.Location = new Point(10, 30);
            labCPURemote.Name = "labCPURemote";
            labCPURemote.Size = new Size(58, 29);
            labCPURemote.TabIndex = 0;
            labCPURemote.Text = "000";
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(labGPURPM);
            groupBox2.Controls.Add(numericUpDown2);
            groupBox2.Controls.Add(label16);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(txtVGA1FanDuty);
            groupBox2.Controls.Add(labVGA1Local);
            groupBox2.Controls.Add(txtVGA1Duty_Hex);
            groupBox2.Controls.Add(labVGA1Remote);
            groupBox2.Controls.Add(checkBox2);
            groupBox2.Controls.Add(label17);
            groupBox2.Font = new Font("Times New Roman", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(157, 19);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(141, 178);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "VGA 1";
            label5.AutoSize = true;
            label5.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(6, 101);
            label5.Name = "label5";
            label5.Size = new Size(34, 15);
            label5.TabIndex = 22;
            label5.Text = "RPM";
            label5.Visible = false;
            labGPURPM.AutoSize = true;
            labGPURPM.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            labGPURPM.Location = new Point(71, 101);
            labGPURPM.Name = "labGPURPM";
            labGPURPM.Size = new Size(19, 15);
            labGPURPM.TabIndex = 21;
            labGPURPM.Text = "---";
            labGPURPM.Visible = false;
            numericUpDown2.Font = new Font("Times New Roman", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            numericUpDown2.Location = new Point(64, 152);
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(44, 22);
            numericUpDown2.TabIndex = 11;
            label16.AutoSize = true;
            label16.Location = new Point(110, 155);
            label16.Name = "label16";
            label16.Size = new Size(25, 19);
            label16.TabIndex = 20;
            label16.Text = "%";
            label10.AutoSize = true;
            label10.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.Location = new Point(86, 69);
            label10.Name = "label10";
            label10.Size = new Size(33, 14);
            label10.TabIndex = 10;
            label10.Text = "Local";
            label4.AutoSize = true;
            label4.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(17, 69);
            label4.Name = "label4";
            label4.Size = new Size(43, 14);
            label4.TabIndex = 8;
            label4.Text = "Remote";
            txtVGA1FanDuty.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtVGA1FanDuty.ForeColor = SystemColors.HotTrack;
            txtVGA1FanDuty.Location = new Point(97, 120);
            txtVGA1FanDuty.Name = "txtVGA1FanDuty";
            txtVGA1FanDuty.Size = new Size(34, 20);
            txtVGA1FanDuty.TabIndex = 18;
            labVGA1Local.AutoSize = true;
            labVGA1Local.Font = new Font("Tahoma", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
            labVGA1Local.ForeColor = SystemColors.Highlight;
            labVGA1Local.Location = new Point(77, 30);
            labVGA1Local.Name = "labVGA1Local";
            labVGA1Local.Size = new Size(58, 29);
            labVGA1Local.TabIndex = 9;
            labVGA1Local.Text = "000";
            txtVGA1Duty_Hex.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtVGA1Duty_Hex.ForeColor = SystemColors.HotTrack;
            txtVGA1Duty_Hex.Location = new Point(66, 120);
            txtVGA1Duty_Hex.Name = "txtVGA1Duty_Hex";
            txtVGA1Duty_Hex.Size = new Size(26, 20);
            txtVGA1Duty_Hex.TabIndex = 17;
            labVGA1Remote.AutoSize = true;
            labVGA1Remote.Font = new Font("Tahoma", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
            labVGA1Remote.ForeColor = SystemColors.HotTrack;
            labVGA1Remote.Location = new Point(14, 30);
            labVGA1Remote.Name = "labVGA1Remote";
            labVGA1Remote.Size = new Size(58, 29);
            labVGA1Remote.TabIndex = 7;
            labVGA1Remote.Text = "000";
            checkBox2.AutoSize = true;
            checkBox2.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            checkBox2.Location = new Point(9, 157);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(51, 19);
            checkBox2.TabIndex = 16;
            checkBox2.Text = "Auto";
            checkBox2.UseVisualStyleBackColor = true;
            label17.AutoSize = true;
            label17.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label17.Location = new Point(6, 126);
            label17.Name = "label17";
            label17.Size = new Size(51, 15);
            label17.TabIndex = 15;
            label17.Text = "FanDuty";
            groupBox3.Controls.Add(labGPU1RPM);
            groupBox3.Controls.Add(label7);
            groupBox3.Controls.Add(numericUpDown3);
            groupBox3.Controls.Add(label18);
            groupBox3.Controls.Add(label12);
            groupBox3.Controls.Add(label6);
            groupBox3.Controls.Add(txtVGA2FanDuty);
            groupBox3.Controls.Add(labVGA2Local);
            groupBox3.Controls.Add(txtVGA2Duty_Hex);
            groupBox3.Controls.Add(labVGA2Remote);
            groupBox3.Controls.Add(checkBox3);
            groupBox3.Controls.Add(label19);
            groupBox3.Font = new Font("Times New Roman", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox3.Location = new Point(304, 19);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(141, 178);
            groupBox3.TabIndex = 5;
            groupBox3.TabStop = false;
            groupBox3.Text = "VGA 2";
            labGPU1RPM.AutoSize = true;
            labGPU1RPM.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            labGPU1RPM.Location = new Point(81, 100);
            labGPU1RPM.Name = "labGPU1RPM";
            labGPU1RPM.Size = new Size(19, 15);
            labGPU1RPM.TabIndex = 23;
            labGPU1RPM.Text = "---";
            labGPU1RPM.Visible = false;
            label7.AutoSize = true;
            label7.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(9, 100);
            label7.Name = "label7";
            label7.Size = new Size(34, 15);
            label7.TabIndex = 23;
            label7.Text = "RPM";
            label7.Visible = false;
            numericUpDown3.Font = new Font("Times New Roman", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            numericUpDown3.Location = new Point(67, 152);
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new Size(44, 22);
            numericUpDown3.TabIndex = 11;
            label18.AutoSize = true;
            label18.Location = new Point(114, 155);
            label18.Name = "label18";
            label18.Size = new Size(25, 19);
            label18.TabIndex = 20;
            label18.Text = "%";
            label12.AutoSize = true;
            label12.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label12.Location = new Point(86, 69);
            label12.Name = "label12";
            label12.Size = new Size(33, 14);
            label12.TabIndex = 10;
            label12.Text = "Local";
            label6.AutoSize = true;
            label6.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(17, 69);
            label6.Name = "label6";
            label6.Size = new Size(43, 14);
            label6.TabIndex = 8;
            label6.Text = "Remote";
            txtVGA2FanDuty.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtVGA2FanDuty.ForeColor = SystemColors.HotTrack;
            txtVGA2FanDuty.Location = new Point(103, 120);
            txtVGA2FanDuty.Name = "txtVGA2FanDuty";
            txtVGA2FanDuty.Size = new Size(34, 20);
            txtVGA2FanDuty.TabIndex = 18;
            txtVGA2FanDuty.TextChanged += txtVGA2FanDuty_TextChanged;
            labVGA2Local.AutoSize = true;
            labVGA2Local.Font = new Font("Tahoma", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
            labVGA2Local.ForeColor = SystemColors.Highlight;
            labVGA2Local.Location = new Point(78, 28);
            labVGA2Local.Name = "labVGA2Local";
            labVGA2Local.Size = new Size(58, 29);
            labVGA2Local.TabIndex = 9;
            labVGA2Local.Text = "000";
            txtVGA2Duty_Hex.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtVGA2Duty_Hex.ForeColor = SystemColors.HotTrack;
            txtVGA2Duty_Hex.Location = new Point(67, 121);
            txtVGA2Duty_Hex.Name = "txtVGA2Duty_Hex";
            txtVGA2Duty_Hex.Size = new Size(26, 20);
            txtVGA2Duty_Hex.TabIndex = 17;
            labVGA2Remote.AutoSize = true;
            labVGA2Remote.Font = new Font("Tahoma", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
            labVGA2Remote.ForeColor = SystemColors.HotTrack;
            labVGA2Remote.Location = new Point(10, 28);
            labVGA2Remote.Name = "labVGA2Remote";
            labVGA2Remote.Size = new Size(58, 29);
            labVGA2Remote.TabIndex = 7;
            labVGA2Remote.Text = "000";
            checkBox3.AutoSize = true;
            checkBox3.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            checkBox3.Location = new Point(17, 155);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(51, 19);
            checkBox3.TabIndex = 16;
            checkBox3.Text = "Auto";
            checkBox3.UseVisualStyleBackColor = true;
            label19.AutoSize = true;
            label19.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label19.Location = new Point(9, 126);
            label19.Name = "label19";
            label19.Size = new Size(51, 15);
            label19.TabIndex = 15;
            label19.Text = "FanDuty";
            timer1.Interval = 3000;
            timer1.Tick += timer1_Tick;
            label20.AutoSize = true;
            label20.Font = new Font("Times New Roman", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label20.ForeColor = Color.MidnightBlue;
            label20.Location = new Point(306, 33);
            label20.Name = "label20";
            label20.Size = new Size(71, 15);
            label20.TabIndex = 6;
            label20.Text = "EC Version :";
            LabECVersion.AutoSize = true;
            LabECVersion.Font = new Font("Times New Roman", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            LabECVersion.ForeColor = Color.Red;
            LabECVersion.Location = new Point(376, 33);
            LabECVersion.Name = "LabECVersion";
            LabECVersion.Size = new Size(27, 15);
            LabECVersion.TabIndex = 7;
            LabECVersion.Text = "-----";
            groupBox4.Controls.Add(labHDD1UseHours);
            groupBox4.Controls.Add(LabHDD1Model);
            groupBox4.Controls.Add(labHDD1Temp);
            groupBox4.Font = new Font("Times New Roman", 11.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox4.Location = new Point(5, 264);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(177, 98);
            groupBox4.TabIndex = 8;
            groupBox4.TabStop = false;
            groupBox4.Visible = false;
            labHDD1UseHours.AutoSize = true;
            labHDD1UseHours.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            labHDD1UseHours.Location = new Point(63, 74);
            labHDD1UseHours.Name = "labHDD1UseHours";
            labHDD1UseHours.Size = new Size(0, 15);
            labHDD1UseHours.TabIndex = 2;
            LabHDD1Model.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            LabHDD1Model.Location = new Point(63, 21);
            LabHDD1Model.Name = "LabHDD1Model";
            LabHDD1Model.Size = new Size(108, 50);
            LabHDD1Model.TabIndex = 1;
            labHDD1Temp.AutoSize = true;
            labHDD1Temp.Font = new Font("Tahoma", 21.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            labHDD1Temp.ForeColor = Color.Green;
            labHDD1Temp.Location = new Point(6, 41);
            labHDD1Temp.Name = "labHDD1Temp";
            labHDD1Temp.Size = new Size(51, 35);
            labHDD1Temp.TabIndex = 0;
            labHDD1Temp.Text = "00";
            groupBox5.Controls.Add(labHDD2UseHours);
            groupBox5.Controls.Add(LabHDD2Model);
            groupBox5.Controls.Add(labHDD2Temp);
            groupBox5.Font = new Font("Times New Roman", 11.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox5.Location = new Point(197, 264);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(177, 98);
            groupBox5.TabIndex = 9;
            groupBox5.TabStop = false;
            groupBox5.Visible = false;
            labHDD2UseHours.AutoSize = true;
            labHDD2UseHours.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            labHDD2UseHours.Location = new Point(63, 74);
            labHDD2UseHours.Name = "labHDD2UseHours";
            labHDD2UseHours.Size = new Size(0, 15);
            labHDD2UseHours.TabIndex = 2;
            LabHDD2Model.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            LabHDD2Model.Location = new Point(63, 21);
            LabHDD2Model.Name = "LabHDD2Model";
            LabHDD2Model.Size = new Size(108, 50);
            LabHDD2Model.TabIndex = 1;
            labHDD2Temp.AutoSize = true;
            labHDD2Temp.Font = new Font("Tahoma", 21.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            labHDD2Temp.ForeColor = Color.Green;
            labHDD2Temp.Location = new Point(6, 37);
            labHDD2Temp.Name = "labHDD2Temp";
            labHDD2Temp.Size = new Size(51, 35);
            labHDD2Temp.TabIndex = 0;
            labHDD2Temp.Text = "00";
            groupBox6.Controls.Add(labHDD3UseHours);
            groupBox6.Controls.Add(LabHDD3Model);
            groupBox6.Controls.Add(labHDD3Temp);
            groupBox6.Font = new Font("Times New Roman", 11.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox6.Location = new Point(380, 264);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(177, 98);
            groupBox6.TabIndex = 10;
            groupBox6.TabStop = false;
            groupBox6.Visible = false;
            labHDD3UseHours.AutoSize = true;
            labHDD3UseHours.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            labHDD3UseHours.Location = new Point(63, 74);
            labHDD3UseHours.Name = "labHDD3UseHours";
            labHDD3UseHours.Size = new Size(0, 15);
            labHDD3UseHours.TabIndex = 2;
            LabHDD3Model.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            LabHDD3Model.Location = new Point(63, 21);
            LabHDD3Model.Name = "LabHDD3Model";
            LabHDD3Model.Size = new Size(108, 50);
            LabHDD3Model.TabIndex = 1;
            labHDD3Temp.AutoSize = true;
            labHDD3Temp.Font = new Font("Tahoma", 21.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            labHDD3Temp.ForeColor = Color.Green;
            labHDD3Temp.Location = new Point(6, 37);
            labHDD3Temp.Name = "labHDD3Temp";
            labHDD3Temp.Size = new Size(51, 35);
            labHDD3Temp.TabIndex = 0;
            labHDD3Temp.Text = "00";
            labRecord.AutoSize = true;
            labRecord.Location = new Point(12, 35);
            labRecord.Name = "labRecord";
            labRecord.Size = new Size(0, 13);
            labRecord.TabIndex = 11;
            labLock.AutoSize = true;
            labLock.ForeColor = Color.Red;
            labLock.Location = new Point(109, 35);
            labLock.Name = "labLock";
            labLock.Size = new Size(0, 13);
            labLock.TabIndex = 12;
            groupBox7.Controls.Add(groupBox8);
            groupBox7.Controls.Add(groupBox1);
            groupBox7.Controls.Add(groupBox2);
            groupBox7.Controls.Add(groupBox3);
            groupBox7.Location = new Point(5, 52);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(607, 206);
            groupBox7.TabIndex = 13;
            groupBox7.TabStop = false;
            groupBox8.Controls.Add(labSYSRPM);
            groupBox8.Controls.Add(label11);
            groupBox8.Controls.Add(numericUpDown4);
            groupBox8.Controls.Add(label13);
            groupBox8.Controls.Add(label21);
            groupBox8.Controls.Add(label22);
            groupBox8.Controls.Add(txtSYSFanDuty);
            groupBox8.Controls.Add(labSYSLocal);
            groupBox8.Controls.Add(txtSYSDuty_Hex);
            groupBox8.Controls.Add(labSYSRemote);
            groupBox8.Controls.Add(checkBox4);
            groupBox8.Controls.Add(label25);
            groupBox8.Font = new Font("Times New Roman", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox8.Location = new Point(451, 19);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(141, 178);
            groupBox8.TabIndex = 24;
            groupBox8.TabStop = false;
            groupBox8.Text = "SYS";
            labSYSRPM.AutoSize = true;
            labSYSRPM.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            labSYSRPM.Location = new Point(81, 100);
            labSYSRPM.Name = "labSYSRPM";
            labSYSRPM.Size = new Size(19, 15);
            labSYSRPM.TabIndex = 23;
            labSYSRPM.Text = "---";
            labSYSRPM.Visible = false;
            label11.AutoSize = true;
            label11.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.Location = new Point(9, 100);
            label11.Name = "label11";
            label11.Size = new Size(34, 15);
            label11.TabIndex = 23;
            label11.Text = "RPM";
            label11.Visible = false;
            numericUpDown4.Font = new Font("Times New Roman", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            numericUpDown4.Location = new Point(67, 152);
            numericUpDown4.Name = "numericUpDown4";
            numericUpDown4.Size = new Size(44, 22);
            numericUpDown4.TabIndex = 11;
            label13.AutoSize = true;
            label13.Location = new Point(114, 155);
            label13.Name = "label13";
            label13.Size = new Size(25, 19);
            label13.TabIndex = 20;
            label13.Text = "%";
            label21.AutoSize = true;
            label21.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label21.Location = new Point(86, 69);
            label21.Name = "label21";
            label21.Size = new Size(33, 14);
            label21.TabIndex = 10;
            label21.Text = "Local";
            label22.AutoSize = true;
            label22.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label22.Location = new Point(17, 69);
            label22.Name = "label22";
            label22.Size = new Size(43, 14);
            label22.TabIndex = 8;
            label22.Text = "Remote";
            txtSYSFanDuty.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSYSFanDuty.ForeColor = SystemColors.HotTrack;
            txtSYSFanDuty.Location = new Point(103, 120);
            txtSYSFanDuty.Name = "txtSYSFanDuty";
            txtSYSFanDuty.Size = new Size(34, 20);
            txtSYSFanDuty.TabIndex = 18;
            txtSYSFanDuty.Text = "--";
            labSYSLocal.AutoSize = true;
            labSYSLocal.Font = new Font("Tahoma", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
            labSYSLocal.ForeColor = SystemColors.Highlight;
            labSYSLocal.Location = new Point(78, 28);
            labSYSLocal.Name = "labSYSLocal";
            labSYSLocal.Size = new Size(43, 29);
            labSYSLocal.TabIndex = 9;
            labSYSLocal.Text = "---";
            txtSYSDuty_Hex.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSYSDuty_Hex.ForeColor = SystemColors.HotTrack;
            txtSYSDuty_Hex.Location = new Point(67, 121);
            txtSYSDuty_Hex.Name = "txtSYSDuty_Hex";
            txtSYSDuty_Hex.Size = new Size(26, 20);
            txtSYSDuty_Hex.TabIndex = 17;
            txtSYSDuty_Hex.Text = "--";
            labSYSRemote.AutoSize = true;
            labSYSRemote.Font = new Font("Tahoma", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
            labSYSRemote.ForeColor = SystemColors.HotTrack;
            labSYSRemote.Location = new Point(10, 28);
            labSYSRemote.Name = "labSYSRemote";
            labSYSRemote.Size = new Size(43, 29);
            labSYSRemote.TabIndex = 7;
            labSYSRemote.Text = "---";
            checkBox4.AutoSize = true;
            checkBox4.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            checkBox4.Location = new Point(12, 153);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(51, 19);
            checkBox4.TabIndex = 16;
            checkBox4.Text = "Auto";
            checkBox4.UseVisualStyleBackColor = true;
            label25.AutoSize = true;
            label25.Font = new Font("Times New Roman", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label25.Location = new Point(9, 126);
            label25.Name = "label25";
            label25.Size = new Size(51, 15);
            label25.TabIndex = 15;
            label25.Text = "FanDuty";
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(620, 369);
            Controls.Add(labLock);
            Controls.Add(labRecord);
            Controls.Add(groupBox6);
            Controls.Add(LabECVersion);
            Controls.Add(label20);
            Controls.Add(groupBox5);
            Controls.Add(LabNBModel);
            Controls.Add(groupBox4);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            Controls.Add(groupBox7);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Clevo ECView 5.5";
            Load += Form1_Load;
            FormClosed += Form1_FormClosed;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            numericUpDown1.EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            numericUpDown2.EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            numericUpDown3.EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            numericUpDown4.EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}