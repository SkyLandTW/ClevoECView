// Type: ECView_CSharp.Input_FanCount
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
    public class Input_FanCount : Form
    {
        private IContainer components;
        private Label label1;
        private Button button1;
        private Button button2;
        private TextBox textBox1;
        private Label label2;
        private string MyFanVal;

        public string FanCount
        {
            get { return MyFanVal; }
            set { MyFanVal = value; }
        }

        public Input_FanCount()
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
            button1 = new Button();
            button2 = new Button();
            textBox1 = new TextBox();
            label2 = new Label();
            SuspendLayout();
            label1.Font = new Font("新細明體", 11.25f, FontStyle.Regular, GraphicsUnit.Point, 136);
            label1.Location = new Point(23, 19);
            label1.Name = "label1";
            label1.Size = new Size(168, 39);
            label1.TabIndex = 0;
            label1.Text = "請輸入設定的風扇數量(PS: 最多4個)";
            button1.Location = new Point(18, 115);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "OK";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            button2.Location = new Point(99, 115);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 2;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            textBox1.Font = new Font("Times New Roman", 11.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(18, 71);
            textBox1.MaxLength = 3;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(141, 25);
            textBox1.TabIndex = 0;
            textBox1.Tag = "0";
            label2.Font = new Font("新細明體", 11.25f, FontStyle.Regular, GraphicsUnit.Point, 136);
            label2.Location = new Point(165, 75);
            label2.Name = "label2";
            label2.Size = new Size(40, 25);
            label2.TabIndex = 6;
            label2.Text = "個";
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(210, 148);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label1);
            Name = "Input_FanCount";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "設定風扇數量";
            ResumeLayout(false);
            PerformLayout();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 274 && (int) m.WParam == 61536)
            {
                if (MessageBox.Show("確定這個視窗關閉", "關閉Fan設定訊息!!", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
                Close();
            }
            base.WndProc(ref m);
        }

        public bool IsNumeric(string strNumber)
        {
            return !new Regex("[^1-4.-]").IsMatch(strNumber);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (IsNumeric(textBox1.Text))
                {
                    FanCount = textBox1.Text;
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    var num1 = (int) MessageBox.Show("請輸入1~4的數值");
                }
            }
            else
            {
                var num2 = (int) MessageBox.Show("請輸入欲設定的風扇或是取消");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}