using DevExpress.XtraEditors;
using DevExpress.XtraWaitForm;
using System;
using System.Windows.Forms;

namespace DLCodeRecord.DevelopForms
{
    /// <summary>
    /// 等待窗口
    /// </summary>
    public partial class WaitingFrm : DevExpress.XtraWaitForm.WaitForm
    {
        private int _percent = 0;

        public WaitingFrm(int maxiMum)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            //this.BackColor = Color.Transparent;
            Size = new System.Drawing.Size(393, 40);
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            this.ShowOnTopMode = ShowFormOnTopMode.AboveAll;
            //progressBarControl1.Properties.Step = 10;
            // 设定显示 百分比到 进度条上
            progressBarControl1.Properties.PercentView = true;
            this.progressBarControl1.Properties.ShowTitle = true;

            progressBarControl1.Properties.Minimum = 0;
            progressBarControl1.Properties.Maximum = maxiMum;

            MaxCount = maxiMum;
            progressBarControl1.Dock = DockStyle.Fill;

            this.labelControl1.Margin = new Padding(10, 0, 0, 0);
            this.labelControl1.Dock = DockStyle.Bottom;
            this.labelControl1.Text = "";
            this.labelControl1.Visible = false;

            GroupControl groupControl = new GroupControl
            {
                ShowCaption = false,
                Dock = DockStyle.Fill
            };
            groupControl.Controls.AddRange(new Control[] { progressBarControl1, labelControl1 });
            this.Controls.Add(groupControl);
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public int MaxCount { get; set; } = 0;

        public int Percent
        {
            get { return _percent; }
            set
            {
                _percent = value;
                progressBarControl1.Text = value.ToString();
                progressBarControl1.PerformStep();
                progressBarControl1.Update();
            }
        }
    }
}