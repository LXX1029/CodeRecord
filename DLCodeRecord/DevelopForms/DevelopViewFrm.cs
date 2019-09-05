using Common;
using DataEntitys;
using log4net;
using System;
using System.Threading.Tasks;

namespace DLCodeRecord.DevelopForms
{
    /// <summary>
    /// 查看详细窗体类
    /// </summary>
    public partial class DevelopViewFrm : BaseFrm
    {
        #region Private Fields

        /// <summary>
        /// 记录类
        /// </summary>
        private DevelopRecordEntity developRecordEntity = null;
        private ILog Log => LogManager.GetLogger("DevelopViewFrm");

        #endregion Private Fields

        #region Public Constructors

        public DevelopViewFrm()
        {
            InitializeComponent();
        }

        public DevelopViewFrm(DevelopRecordEntity entity)
        {
            InitializeComponent();
            this.Text = "查看";
            this.picImg.Properties.NullText = "暂无图片";
            this.Load -= new EventHandler(DevelopViewFrm_Load);
            this.Load += new EventHandler(DevelopViewFrm_Load);
            developRecordEntity = entity;
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void ShowHandler();

        #endregion Public Delegates

        #region Private Methods

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void DevelopViewFrm_Load(object sender, EventArgs e)
        {
            var tuple = UtilityHelper.GetWorkingAreaSize();
            this.Size = tuple.Item3;
            this.Location = tuple.Item4;
            FormLoadingTipAsync(this, LoadDate);
        }

        private void InitialRichEditControl()
        {
            //
            // recDesc
            //
            //this.recDesc.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Draft;
            //this.recDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.recDesc.LayoutUnit = DevExpress.XtraRichEdit.DocumentLayoutUnit.Pixel;
            //this.recDesc.Location = new System.Drawing.Point(2, 28);
            //this.recDesc.Name = "recDesc";
            //this.recDesc.Options.AutoCorrect.DetectUrls = false;
            //this.recDesc.Options.AutoCorrect.ReplaceTextAsYouType = false;
            //this.recDesc.Options.Behavior.PasteLineBreakSubstitution = DevExpress.XtraRichEdit.LineBreakSubstitute.Paragraph;
            //this.recDesc.Options.DocumentCapabilities.Bookmarks = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            //this.recDesc.Options.DocumentCapabilities.CharacterStyle = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            //this.recDesc.Options.DocumentCapabilities.HeadersFooters = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            //this.recDesc.Options.DocumentCapabilities.Hyperlinks = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            //this.recDesc.Options.DocumentCapabilities.InlinePictures = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            //this.recDesc.Options.DocumentCapabilities.Numbering.Bulleted = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            //this.recDesc.Options.DocumentCapabilities.Numbering.MultiLevel = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            //this.recDesc.Options.DocumentCapabilities.Numbering.Simple = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            //this.recDesc.Options.DocumentCapabilities.ParagraphFormatting = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            //this.recDesc.Options.DocumentCapabilities.Paragraphs = DevExpress.XtraRichEdit.DocumentCapability.Enabled;
            //this.recDesc.Options.DocumentCapabilities.ParagraphStyle = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            //this.recDesc.Options.DocumentCapabilities.Sections = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            //this.recDesc.Options.DocumentCapabilities.Tables = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            //this.recDesc.Options.DocumentCapabilities.TableStyle = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            //this.recDesc.Options.HorizontalRuler.Visibility = DevExpress.XtraRichEdit.RichEditRulerVisibility.Hidden;

            //this.recDesc.Views.DraftView.AllowDisplayLineNumbers = true;
            //this.recDesc.Options.Behavior.ShowPopupMenu = DevExpress.XtraRichEdit.DocumentCapability.Disabled;  //右键不可用
            //this.recDesc.ReadOnly = true; // 文档只读
            //// 显示行号 宽度
            //this.recDesc.Views.DraftView.Padding = new System.Windows.Forms.Padding(80, 4, 0, 0);
            ////this.recDesc.Views.SimpleView.Padding = new System.Windows.Forms.Padding(50, 4, 4, 0);
        }

        /// <summary>
        /// 加载数据方法
        /// </summary>
        private void LoadDate()
        {
            this.picImg.Properties.ShowMenu = true;  // 图片 不显示右键菜单
            this.lcParentName.Text = string.Format("大板块：{0}", developRecordEntity.ParentTypeName);
            this.lcChildrenName.Text = string.Format("小板块：{0}", developRecordEntity.SubTypeName);
            this.codeEditor.scintilla.Text = developRecordEntity.Desc;
            this.codeEditor.scintilla.ReadOnly = true;
            this.picImg.Image = UtilityHelper.ConvertByteToImg(developRecordEntity.Picture);
            this.picImg.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.lcTitle.Text = developRecordEntity.Title;
        }

        /// <summary>
        /// 初始化文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecDesc_InitializeDocument(object sender, EventArgs e)
        {
            //Document document = recDesc.Document;
            //document.BeginUpdate();
            //try
            //{
            //    document.DefaultCharacterProperties.FontName = "Courier New";
            //    document.DefaultCharacterProperties.FontSize = 10;
            //    document.Sections[0].Page.Width = Units.InchesToDocumentsF(100);
            //    document.Sections[0].LineNumbering.CountBy = 1;

            //    SizeF tabSize = recDesc.MeasureSingleLineString("    ", document.DefaultCharacterProperties);
            //    TabInfoCollection tabs = document.Paragraphs[0].BeginUpdateTabs(true);
            //    try
            //    {
            //        for (int i = 1; i <= 30; i++)
            //        {
            //            DevExpress.XtraRichEdit.API.Native.TabInfo tab = new DevExpress.XtraRichEdit.API.Native.TabInfo();
            //            tab.Position = i * tabSize.Width;
            //            tabs.Add(tab);
            //        }
            //    }
            //    finally
            //    {
            //        document.Paragraphs[0].EndUpdateTabs(tabs);
            //    }
            //}
            //finally
            //{
            //    document.EndUpdate();
            //}
        }

        #endregion Private Methods
    }
}