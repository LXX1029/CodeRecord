using DevExpress.XtraWaitForm;
using System;

namespace Common
{
    /// <summary>
    /// 等待窗口
    /// </summary>
    public partial class WaitFrm : DevExpress.XtraWaitForm.WaitForm
    {
        public WaitFrm()
        {
            InitializeComponent();
            this.progressPanel.AutoHeight = true;
            this.ShowOnTopMode = ShowFormOnTopMode.AboveParent; // 显示位置
        }
        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        /// <summary>
        /// 设置显示标题
        /// </summary>
        /// <param name="caption">标题内容</param>
        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel.Caption = caption;
        }

        /// <summary>
        /// 设置描述
        /// </summary>
        /// <param name="description">描述内容</param>
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel.Description = description;
        }
        #endregion Overrides
    }
}