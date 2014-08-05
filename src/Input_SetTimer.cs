// Type: ECView_CSharp.Input_SetTimer
// Assembly: ClevoECView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 466451CB-A6EA-4E8C-846C-A99227909B6B
// Assembly location: C:\ECView\ClevoECView.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ECView_CSharp
{
    public class Input_SetTimer : Form
    {
        private IContainer components;
        private Label label1;
        private TextBox textBox1;
        private Label label2;
        private Button button1;
        private Button button2;
        private string MyTimerVal;

        public string SetTimer
        {
            get { return MyTimerVal; }
            set { MyTimerVal = value; }
        }

        public Input_SetTimer()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            label1 = new Label();
            textBox1 = new TextBox();
            label2 = new Label();
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            label1.Font = new Font("PMingLiU", 11.25f, FontStyle.Regular, GraphicsUnit.Point, 136);
            label1.Location = new Point(30, 24);
            label1.Name = "label1";
            label1.Size = new Size(139, 39);
            label1.TabIndex = 1;
            label1.Text = "請輸入設定的秒數(PS: 1~5秒)";
            textBox1.Font = new Font("Times New Roman", 11.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(33, 74);
            textBox1.MaxLength = 3;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(126, 25);
            textBox1.TabIndex = 4;
            label2.Font = new Font("PMingLiU", 11.25f, FontStyle.Regular, GraphicsUnit.Point, 136);
            label2.Location = new Point(165, 78);
            label2.Name = "label2";
            label2.Size = new Size(40, 25);
            label2.TabIndex = 5;
            label2.Text = "秒";
            button1.Location = new Point(31, 113);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 6;
            button1.Text = "OK";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            button2.Location = new Point(112, 113);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 7;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(210, 148);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Name = "Input_SetTimer";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "計時器";
            ResumeLayout(false);
            PerformLayout();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 274 && (int) m.WParam == 61536)
            {
                if (MessageBox.Show("確定這個視窗關閉", "關閉Timer設定訊息!!", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
                Close();
            }
            base.WndProc(ref m);
        }

        public bool IsNumeric(string strNumber)
        {
            return !new Regex("[^1-5.-]").IsMatch(strNumber);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (IsNumeric(textBox1.Text))
                {
                    SetTimer = textBox1.Text;
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    var num1 = (int) MessageBox.Show("請輸入1~5的秒數");
                }
            }
            else
            {
                var num2 = (int) MessageBox.Show("請輸入欲設定的計時秒數或是取消");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}