using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using DataEntitys;
using DevExpress.Office.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Services;
using log4net;
using static Common.LoggerHelper;
using static Common.UtilityHelper;
using static Services.Unity.UnityContainerManager;
using namespaceDraw = System.Drawing;
using namespaceFrm = System.Windows.Forms;
using namespaceWin = System.Windows;

namespace DLCodeRecord.DevelopForms
{
    /// <summary>
    /// 新增，修改窗体类
    /// </summary>
    public partial class DevelopFrm : BaseFrm
    {
        #region 属性
        /// <summary>
        /// 截屏类
        /// </summary>
        private readonly RisCaptureLib.ScreenCaputre _screenCaputre = new RisCaptureLib.ScreenCaputre();

        /// <summary>
        /// 自定义记录类
        /// </summary>
        private DevelopRecordEntity DevelopRecordEntity { get; set; }

        /// <summary>
        /// 本地DevelopRecord
        /// </summary>
        private DevelopRecord LocalDevelopRecord { get; set; }

        /// <summary>
        /// 最终尺寸
        /// </summary>
        private namespaceWin.Size? _lastSize;

        /// <summary>
        /// 绑定实体类
        /// </summary>
        private BindingSource _bindingSource = new BindingSource();
        #endregion 属性

        #region 构造函数

        public DevelopFrm()
        {
            InitializeComponent();
            DevelopRecordEntity = new DevelopRecordEntity(DataManage.LoginUser.Id);
            this.Text = "操作窗口";
            _screenCaputre.ScreenCaputred -= OnScreenCaputred;
            _screenCaputre.ScreenCaputreCancelled -= OnScreenCaputreCancelled;
            _screenCaputre.ScreenCaputred -= OnScreenCaputred;
            _screenCaputre.ScreenCaputred += OnScreenCaputred;
            _screenCaputre.ScreenCaputreCancelled -= OnScreenCaputreCancelled;
            _screenCaputre.ScreenCaputreCancelled += OnScreenCaputreCancelled;
            codeEditor.Scintilla.TextChanged -= Scintilla_TextChanged;
            codeEditor.Scintilla.TextChanged += Scintilla_TextChanged;
            this.Load -= new EventHandler(DevelopAddFrm_Load);
            this.Load += new EventHandler(DevelopAddFrm_Load);
            this.FormClosing -= DevelopFrm_FormClosing;
            this.FormClosing += DevelopFrm_FormClosing;
            this.rdioGChildType.SelectedIndexChanged -= RdioGChildType_EditValueChanged;
            this.rdioGChildType.SelectedIndexChanged += RdioGChildType_EditValueChanged;
            this.rgParentType.SelectedIndexChanged -= RgParentType_EditValueChanged;
            this.rgParentType.SelectedIndexChanged += RgParentType_EditValueChanged;
            this.picImg.Properties.PictureStoreMode = PictureStoreMode.ByteArray;
            this.picImg.CausesValidation = false;
            // 原始尺寸大小模式
            this.picImg.Properties.SizeMode = PictureSizeMode.Squeeze;
            this.picImg.Properties.ShowZoomSubMenu = DevExpress.Utils.DefaultBoolean.True;
            this.picImg.Properties.NullText = "暂无图像";
            this.btnZipPath.Properties.ReadOnly = true;

            this.txtTitle.Properties.MaxLength = 100;
            this.txtTitle.ErrorIconAlignment = ErrorIconAlignment.MiddleLeft;
            this.rgParentType.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
            this.rdioGChildType.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
            this.btnSelectImg.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
            this.btnZipPath.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
        }

        #region 代码编辑改变
        private void Scintilla_TextChanged(object sender, EventArgs e)
        {
            DevelopRecordEntity.Desc = codeEditor.Scintilla.Text.Trim();
        }
        #endregion

        /// <summary>
        /// 修改操作构造函数
        /// </summary>
        /// <param name="entity">传过来的对象</param>
        public DevelopFrm(DevelopRecordEntity entity) : this()
        {
            try
            {
                if (entity == null)
                {
                    this.lciContinue.Visibility = LayoutVisibility.Always;
                    this.chkContinue.Visible = true;
                    actionState = DevelopActiveState.Adding;
                }
                else
                {
                    this.lciContinue.Visibility = LayoutVisibility.Never;
                    this.chkContinue.Visible = false;
                    actionState = DevelopActiveState.Updating;
                    // 保存 传过来的对象
                    DevelopRecordEntity = entity;
                    Task.Run(async () =>
                    {
                        LocalDevelopRecord = await UnityDevelopRecordFacade.GetEntity(entity.Id);
                    });
                }
            }
            catch (Exception ex)
            {
                CatchLoadException(this, ex);
            }
        }
        #endregion 构造函数

        #region 初始化数据

        /// <summary>
        /// 初始化数据
        /// 使用codeEditor
        /// </summary>
        private async Task InitialData()
        {
            try
            {
                this.txtTitle.Focus();
                ShowSplashScreenForm(PromptHelper.D_LOADINGDATA);
                // 加载大类
                IList<DevelopType> parentTypes = await UnityDevelopTypeFacade.GetDevelopTypesByParentId(0);
                if (parentTypes.Count() > 0)
                {
                    foreach (var f in parentTypes)
                    {
                        rgParentType.Properties.Items.Add(new RadioGroupItem()
                        {
                            Value = f.Id,
                            Description = f.Name,
                        });
                    }
                }
                rgParentType.SelectedIndex = 0;
                #region 绑定developRecordEntity数据到控件
                // this.rgParentType.DataBindings.Add("EditValue", DevelopRecordEntity, "ParentId", true, DataSourceUpdateMode.OnPropertyChanged);
                // this.rdioGChildType.DataBindings.Add("EditValue", DevelopRecordEntity, "SubTypeId", true);
                // this.codeEditor.scintilla.DataBindings.Add("Text", DevelopRecordEntity, "Desc", true, DataSourceUpdateMode.OnPropertyChanged);
                this.rgParentType.EditValue = DevelopRecordEntity.ParentId;
                this.rdioGChildType.EditValue = DevelopRecordEntity.SubTypeId;
                this.codeEditor.Scintilla.Text = DevelopRecordEntity.Desc;

                this.txtTitle.DataBindings.Add(new Binding("EditValue", DevelopRecordEntity, "Title", true, DataSourceUpdateMode.OnPropertyChanged));
                this.picImg.DataBindings.Add("EditValue", DevelopRecordEntity, "Picture", true, DataSourceUpdateMode.OnPropertyChanged);
                if (actionState == DevelopActiveState.Adding && parentTypes.Count() > 0)
                {
                    int parentTypeId = (int)parentTypes.FirstOrDefault().Id;
                    DevelopRecordEntity.ParentId = parentTypeId;
                    rgParentType.SelectedIndex = 0;
                    rdioGChildType.SelectedIndex = 0;
                }
                #endregion
                CloseSplashScreenForm();
            }
            catch (Exception ex)
            {
                CatchLoadException(this, ex);
            }
        }

        #endregion 初始化数据

        #region 选择大/小版块
        /// <summary>
        /// 选择大版块
        /// </summary>
        private async void RgParentType_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                int parentId = Convert.ToInt32(rgParentType.EditValue.ToString());
                IEnumerable<DevelopType> childTypes = await UnityDevelopTypeFacade.GetDevelopTypesByParentId(parentId);
                this.rdioGChildType.Properties.Items.Clear();
                if (childTypes.Count() > 0)
                {
                    childTypes.OrderBy(p => p.Name).ToList().ForEach(f =>
                    {
                        this.rdioGChildType.Properties.Items.Add(new RadioGroupItem()
                        {
                            Value = f.Id,
                            Description = f.Name,
                        });
                    });
                    if (actionState == DevelopActiveState.Adding)
                    {
                        rdioGChildType.SelectedIndex = 0;
                        DevelopRecordEntity.SubTypeId = (int)rdioGChildType.Properties.Items[0].Value;
                    }
                    if (actionState == DevelopActiveState.Updating)
                    {
                        DevelopRecordEntity.ParentId = parentId;
                    }
                }
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }

        /// <summary>
        /// 选择小版块
        /// </summary>
        private void RdioGChildType_EditValueChanged(object sender, EventArgs e)
        {
            DevelopRecordEntity.SubTypeId = Convert.ToInt32(rdioGChildType.EditValue.ToString());
            SetDescValue();
        }

        /// <summary>
        /// 重置描述内容
        /// </summary>
        private void SetDescValue()
        {
            // developRecordEntity 表示新增，显示内容格式
            if (actionState == DevelopActiveState.Adding)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("/*");
                sb.AppendLine("1.000 属性");
                sb.AppendLine("*/");
                sb.AppendLine("//1.001 ");
                sb.AppendLine();
                sb.AppendLine("/*");
                sb.AppendLine("2.000 方法");
                sb.AppendLine("*/");
                sb.AppendLine("#region 2.001 ");
                sb.AppendLine("#endregion");
                sb.AppendLine();
                sb.AppendLine("/*");
                sb.AppendLine("3.000 事件");
                sb.AppendLine("*/");
                sb.AppendLine("#region 3.001 ");
                sb.AppendLine("#endregion");
                sb.AppendLine();
                sb.AppendLine("/*");
                sb.AppendLine("4.000 示例");
                sb.AppendLine("*/");
                sb.AppendLine("#region 4.001 ");
                sb.AppendLine("#endregion");
                codeEditor.Scintilla.Text = sb.ToString().Trim();
            }
        }

        #endregion 选择大/小版块

        #region 窗体加载
        /// <summary>
        /// 窗体加载
        /// </summary>
        private async void DevelopAddFrm_Load(object sender, EventArgs e)
        {
            var tuple = UtilityHelper.GetWorkingAreaSize();
            this.Size = tuple.Item3;
            this.Location = tuple.Item4;
            await InitialData();
        }

        #endregion 窗体加载

        #region 新增，修改保存

        /// <summary>
        /// 保存
        /// </summary>
        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (DevelopRecordEntity.ParentId < 0)
            {
                rgParentType.ErrorText = "请选择大版块";
                return;
            }

            if (DevelopRecordEntity.SubTypeId < 0)
            {
                rdioGChildType.ErrorText = "请选择小版块";
                return;
            }

            if (VerifyHelper.IsEmptyOrNullOrWhiteSpace(DevelopRecordEntity.Title))
            {
                txtTitle.ErrorText = "标题不能为空";
                txtTitle.Focus();
                return;
            }
            if (DevelopRecordEntity.Title.Length > 100)
            {
                txtTitle.ErrorText = "输入字符长度小于50";
                txtTitle.Focus();
                return;
            }
            try
            {
                string imgPath = this.btnSelectImg.Text.Trim();
                string zipPath = this.btnZipPath.Text;
                DateTime editTime = DateTime.Now;
                if (this.LocalDevelopRecord == null)
                    this.LocalDevelopRecord = new DevelopRecord();
                this.LocalDevelopRecord.Title = DevelopRecordEntity.Title;
                this.LocalDevelopRecord.Desc = DevelopRecordEntity.Desc;
                this.LocalDevelopRecord.TypeId = DevelopRecordEntity.SubTypeId;
                this.LocalDevelopRecord.UpdatedTime = editTime;
                this.LocalDevelopRecord.Picture = ConvertImgToByte(picImg.Image);
                this.LocalDevelopRecord.UserId = DataManage.LoginUser.Id;
                if (rgParentType.SelectedIndex == -1)
                    rgParentType.SelectedIndex = 0;
                if (rdioGChildType.SelectedIndex == -1)
                    rdioGChildType.SelectedIndex = 0;
                DevelopRecordEntity.SubTypeName = rdioGChildType.Properties.Items[rdioGChildType.SelectedIndex].Description;
                DevelopRecordEntity.ParentTypeName = rgParentType.Properties.Items[rgParentType.SelectedIndex].Description;
                DevelopRecordEntity.UpdatedTime = editTime;
                DevelopRecordEntity.Picture = this.LocalDevelopRecord.Picture;
                if (actionState == DevelopActiveState.Updating)
                {
                    // 判断是否修改压缩包
                    if (!VerifyHelper.IsEmptyOrNullOrWhiteSpace(zipPath))
                    {
                        this.LocalDevelopRecord.Zip = ConvertFileToByte(zipPath);
                    }
                    await UnityDevelopRecordFacade.UpdateEntity(this.LocalDevelopRecord);
                    MsgHelper.ShowInfo(PromptHelper.D_UPDATE_SUCCESS);
                    CloseNormal();
                }
                else if (actionState == DevelopActiveState.Adding)
                {
                    // 新增
                    DevelopRecordEntity.UserName = DataManage.LoginUser.Name;
                    DevelopRecordEntity.CreatedTime = editTime;
                    this.LocalDevelopRecord.Zip = ConvertFileToByte(this.btnZipPath.Text.Trim());
                    this.LocalDevelopRecord.CreatedTime = editTime;
                    DevelopRecord record = await UnityDevelopRecordFacade.AddEntity(this.LocalDevelopRecord);
                    DevelopRecordEntity.Id = record.Id;
                    // 添加到集合首行
                    DataManage.Instance.DevelopRecordEntityList.Insert(0, new DevelopRecordEntity
                    {
                        Id = DevelopRecordEntity.Id,
                        Title = DevelopRecordEntity.Title,
                        Desc = DevelopRecordEntity.Desc,
                        CreatedTime = DevelopRecordEntity.CreatedTime,
                        UpdatedTime = DevelopRecordEntity.UpdatedTime,
                        Picture = DevelopRecordEntity.Picture,
                        BitMap = DevelopRecordEntity.BitMap,
                        ParentId = DevelopRecordEntity.ParentId,
                        ParentTypeName = DevelopRecordEntity.ParentTypeName,
                        SubTypeId = DevelopRecordEntity.SubTypeId,
                        SubTypeName = DevelopRecordEntity.SubTypeName,
                        UserName = DevelopRecordEntity.UserName,
                        UserId = DevelopRecordEntity.UserId,
                    });
                    MsgHelper.ShowInfo(PromptHelper.D_ADD_SUCCESS);
                    // 是否为继续新增
                    if (chkContinue.Checked)
                    {
                        DevelopRecordEntity.Id = 0;
                        DevelopRecordEntity.ParentId = 0;
                        DevelopRecordEntity.Title = string.Empty;
                        DevelopRecordEntity.Desc = string.Empty;
                        DevelopRecordEntity.Picture = null;
                        rgParentType.Focus();
                        rgParentType.SelectedIndex = 0;
                        rdioGChildType.SelectedIndex = 0;
                        this.btnSelectImg.Text = string.Empty;
                        this.txtTitle.DataBindings[0].ReadValue();
                        this.picImg.DataBindings[0].ReadValue();
                        SetDescValue();
                        actionState = DevelopActiveState.Adding;
                    }
                    else
                    {
                        CloseNormal();
                    }
                }
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }

        #endregion 新增，修改保存

        #region 关闭
        /// <summary>
        /// 设置DevelopActionState 状态为Normal,并关闭窗口
        /// </summary>
        private void CloseNormal()
        {
            actionState = DevelopActiveState.Normal;
            this.Close();
        }
        #endregion

        #region 选择图片，下载图片
        /// <summary>
        /// 关闭按钮事件
        /// </summary>
        private void BtnClose_Click(object sender, EventArgs e) => this.Close();

        /// <summary>
        /// 下载图片
        /// </summary>
        private async void BtnDownLoad_Click(object sender, EventArgs e)
        {
            string imgPath = this.btnSelectImg.Text.Trim();
            if (string.IsNullOrEmpty(imgPath) || !File.Exists(imgPath))
            {
                this.btnSelectImg.ErrorText = "图片路径不合法";
                return;
            }
            try
            {
                ShowSplashScreenForm(PromptHelper.D_LOADINGIMG);
                using (var http = HttpClientFactory.Create())
                {
                    var bytes = await http.GetByteArrayAsync(imgPath).ConfigureAwait(false);
                    this.picImg.Invoke(new Action(() =>
                    {
                        this.picImg.Image = ConvertByteToImg(bytes);
                    }));
                }
            }
            catch (Exception ex)
            {
                CloseSplashScreenForm();
                CatchException(ex);
                Logger.Error(ex);
            }
            finally
            {
                CloseSplashScreenForm();
            }
        }

        /// <summary>
        /// 选择图片
        /// </summary>
        private void BtnSelectImg_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            // 根据选择图片 显示图片
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "PNG|*.png|GIF|*.gif|JPG|*.jpg|JPEG|*.jpeg",
            };
            if (dialog.ShowDialog() == namespaceFrm.DialogResult.OK)
            {
                btnSelectImg.Text = dialog.FileName;
                namespaceDraw.Bitmap btp = new namespaceDraw.Bitmap(dialog.FileName);
                this.picImg.Image = btp;
            }
        }

        #endregion 选择图片，下载图片,关闭提示

        #region 截屏事件

        /// <summary>
        /// 截屏
        /// </summary>
        private void BtnCut_Click(object sender, EventArgs e)
        {
            if (this.Owner != null)
                this.Owner.Visible = false;
            this.Visible = false;
            _screenCaputre.StartCaputre(30, _lastSize);
        }

        /// <summary>
        /// 取消截屏
        /// </summary>
        private void OnScreenCaputreCancelled(object sender, System.EventArgs e)
        {
            if (this.Text != string.Empty)
            {
                if (this.Owner != null && !this.Owner.Visible)
                    this.Owner.Visible = true;
                Show();
                Focus();
            }
        }

        private void OnScreenCaputred(object sender, RisCaptureLib.ScreenCaputredEventArgs e)
        {
            // 设置最终尺寸
            _lastSize = new namespaceWin.Size(e.Bmp.Width, e.Bmp.Height);
            this.StartPosition = FormStartPosition.Manual;
            namespaceDraw.Rectangle screenArea = namespaceFrm.Screen.GetWorkingArea(this);
            this.Location = new namespaceDraw.Point((screenArea.Width - this.Width) / 2, 0);
            Show();
            if (this.Owner != null)
                this.Owner.Show();
            // 转换图片
            namespaceWin.Media.Imaging.BitmapImage bitmapImage = BitMapSoruceToBitMapImage(e.Bmp);
            if (bitmapImage == null)
                return;
            // this.picImg.Image = ConvertByteToImg(BitMapImageToByteArray(bitmapImage));
            this.picImg.EditValue = BitMapImageToByteArray(bitmapImage);
        }

        #endregion 截屏事件

        #region 上传压缩包

        private void BtnUpLoadZip_Click(object sender, ButtonPressedEventArgs e)
        {
            // 根据选择图片 显示图片
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "RAR|*.rar|ZIP|*.zip",
            };
            if (dialog.ShowDialog() == namespaceFrm.DialogResult.OK)
            {
                // 判断压缩包大小不能超过5M
                string fileName = dialog.FileName;
                FileInfo fi = new FileInfo(fileName);
                double fileSize = System.Math.Ceiling(fi.Length / 1024.0 / 1024.0);
                if (fileSize > 5)
                    this.btnZipPath.ErrorText = "压缩包文件大小不能超过5MB";
                else
                    this.btnZipPath.Text = dialog.FileName;
            }
        }

        #endregion 上传压缩包

        #region 窗口关闭

        private void DevelopFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Visible == false)
                return;
            FormClosingTip(e);
        }

        #endregion

        #region 初始化数据-- 弃用
        /// <summary>
        /// 初始化数据
        /// RichEditControl
        /// </summary>
        //private bool InitialData1()
        //{
        //    this.Invoke(new Action(() =>
        //    {
        //        this.picImg.Properties.SizeMode = PictureSizeMode.Squeeze;
        //        this.picImg.Properties.ShowZoomSubMenu = DevExpress.Utils.DefaultBoolean.True;
        //        this.picImg.Properties.NullText = "暂无图像";
        //        this.txtTitle.Focus();
        //        if (!this.recDesc.InvokeRequired)
        //        {
        //            InitialRichEditControl();
        //            this.recDesc.DragEnter += new namespaceFrm.DragEventHandler(RecDesc_DragEnter);
        //            this.recDesc.InitializeDocument += new EventHandler(RecDesc_InitializeDocument);
        //            if (DevelopRecordEntity == null)
        //            {
        //                recDesc.Text = "";
        //                recDesc.Options.DocumentSaveOptions.CurrentFileName = "a.cs";
        //                recDesc.ReplaceService<ISyntaxHighlightService>(new CustomOtherSyntaxHighlightService(recDesc));
        //            }
        //            if (DevelopRecordEntity != null)
        //            {
        //                if (DevelopRecordEntity.ParentTypeName == "Sqlserver")
        //                {
        //                    recDesc.Options.DocumentSaveOptions.CurrentFileName = "a.cs";
        //                    recDesc.ReplaceService<ISyntaxHighlightService>(new CustomSqlSyntaxHighlightService(recDesc.Document));
        //                }
        //                if (DevelopRecordEntity.ParentTypeName == "WPF")
        //                {
        //                    recDesc.Options.DocumentSaveOptions.CurrentFileName = "a.xaml";
        //                    recDesc.ReplaceService<ISyntaxHighlightService>(new CustomOtherSyntaxHighlightService(recDesc));
        //                }
        //                if (DevelopRecordEntity.ParentTypeName == "C#" || DevelopRecordEntity.ParentTypeName == "Devexpress" || DevelopRecordEntity.ParentTypeName == "EF" || DevelopRecordEntity.ParentTypeName == "WCF")
        //                {
        //                    recDesc.Options.DocumentSaveOptions.CurrentFileName = "a.cs";
        //                    recDesc.ReplaceService<ISyntaxHighlightService>(new CustomOtherSyntaxHighlightService(recDesc));
        //                }
        //            }
        //        }
        //    }));
        //    return true;
        //}
        #endregion

        #region 初始化 RichEditControl--弃用

        //private void InitialRichEditControl()
        //{
        //    try
        //    {
        //        //
        //        // recDesc
        //        //
        //        this.recDesc.Name = "recDesc";
        //        this.recDesc.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Draft;  // 简单视图模式
        //        this.recDesc.Dock = System.Windows.Forms.DockStyle.Fill;
        //        this.recDesc.LayoutUnit = DevExpress.XtraRichEdit.DocumentLayoutUnit.Pixel;
        //        this.recDesc.Location = new System.Drawing.Point(2, 28);
        //        this.recDesc.Options.AutoCorrect.DetectUrls = true;
        //        this.recDesc.Options.AutoCorrect.ReplaceTextAsYouType = false;
        //        this.recDesc.Options.Behavior.PasteLineBreakSubstitution = DevExpress.XtraRichEdit.LineBreakSubstitute.Paragraph;
        //        this.recDesc.Options.DocumentCapabilities.Bookmarks = DevExpress.XtraRichEdit.DocumentCapability.Hidden;
        //        this.recDesc.Options.DocumentCapabilities.CharacterStyle = DevExpress.XtraRichEdit.DocumentCapability.Hidden;
        //        this.recDesc.Options.DocumentCapabilities.HeadersFooters = DevExpress.XtraRichEdit.DocumentCapability.Enabled;
        //        this.recDesc.Options.DocumentCapabilities.Hyperlinks = DevExpress.XtraRichEdit.DocumentCapability.Enabled;
        //        this.recDesc.Options.DocumentCapabilities.InlinePictures = DevExpress.XtraRichEdit.DocumentCapability.Enabled;
        //        this.recDesc.Options.DocumentCapabilities.Numbering.Bulleted = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
        //        this.recDesc.Options.DocumentCapabilities.Numbering.MultiLevel = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
        //        this.recDesc.Options.DocumentCapabilities.Numbering.Simple = DevExpress.XtraRichEdit.DocumentCapability.Enabled;
        //        this.recDesc.Options.DocumentCapabilities.ParagraphFormatting = DevExpress.XtraRichEdit.DocumentCapability.Enabled;
        //        this.recDesc.Options.DocumentCapabilities.Paragraphs = DevExpress.XtraRichEdit.DocumentCapability.Enabled;
        //        this.recDesc.Options.DocumentCapabilities.ParagraphStyle = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
        //        this.recDesc.Options.DocumentCapabilities.Sections = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
        //        this.recDesc.Options.DocumentCapabilities.Tables = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
        //        this.recDesc.Options.DocumentCapabilities.TableStyle = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
        //        this.recDesc.Options.DocumentCapabilities.ParagraphFormatting = DocumentCapability.Enabled;
        //        this.recDesc.Options.DocumentCapabilities.Comments = DocumentCapability.Hidden;
        //        this.recDesc.Options.DocumentCapabilities.EndNotes = DocumentCapability.Hidden;
        //        this.recDesc.Options.Bookmarks.Visibility = RichEditBookmarkVisibility.Hidden;
        //        this.recDesc.Options.HorizontalRuler.Visibility = DevExpress.XtraRichEdit.RichEditRulerVisibility.Visible;
        //        this.recDesc.Views.DraftView.AllowDisplayLineNumbers = true;
        //        // 显示行号 宽度
        //        this.recDesc.Views.DraftView.Padding = new System.Windows.Forms.Padding(80, 4, 0, 0);
        //        //this.recDesc.Views.SimpleView.Padding = new System.Windows.Forms.Padding(50, 4, 4, 0);
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteException(ex.Message);
        //        MsgHelper.ShowError(ex.Message);
        //    }
        //}

        /// <summary>
        /// 初始化文档
        /// </summary>
        //private void RecDesc_InitializeDocument(object sender, EventArgs e)
        //{
        //    DevExpress.XtraRichEdit.API.Native.Document document = recDesc.Document;
        //    document.BeginUpdate();
        //    try
        //    {
        //        document.DefaultCharacterProperties.FontName = "Courier New";
        //        document.DefaultCharacterProperties.FontSize = 14;
        //        document.Sections[0].Page.Width = Units.InchesToDocumentsF(100);
        //        document.Sections[0].LineNumbering.CountBy = 1;
        //        document.Sections[0].LineNumbering.RestartType = LineNumberingRestart.NewSection;

        //        namespaceDraw.SizeF tabSize = recDesc.MeasureSingleLineString("       ", document.DefaultCharacterProperties);
        //        TabInfoCollection tabs = document.Paragraphs[0].BeginUpdateTabs(true);
        //        try
        //        {
        //            for (int i = 1; i <= 30; i++)
        //            {
        //                DevExpress.XtraRichEdit.API.Native.TabInfo tab = new DevExpress.XtraRichEdit.API.Native.TabInfo
        //                {
        //                    Position = i * tabSize.Width
        //                };
        //                tabs.Add(tab);
        //            }
        //        }
        //        finally
        //        {
        //            document.Paragraphs[0].EndUpdateTabs(tabs);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteException(ex.Message);
        //        MsgHelper.ShowError(ex.Message);
        //    }
        //    finally
        //    {
        //        document.EndUpdate();
        //    }
        //}
        #endregion 初始化 RichEditControl--舍弃

        #region 拖放文件到RichEditor -- 弃用

        /// <summary>
        /// 拖放文件到 控件
        /// </summary>
        //private void RecDesc_DragEnter(object sender, namespaceFrm.DragEventArgs e)
        //{
        //    try
        //    {
        //        this.recDesc.Text = "";
        //        Array aryFiles = ((System.Array)e.Data.GetData(namespaceFrm.DataFormats.FileDrop));

        //        StringBuilder builder = new StringBuilder();
        //        for (int i = 0; i < aryFiles.Length; i++)
        //        {
        //            // 设置当前拖放文件格式
        //            string ext = System.IO.Path.GetExtension(aryFiles.GetValue(i).ToString());
        //            if (ext == ".xaml")
        //                recDesc.Options.DocumentSaveOptions.CurrentFileName = "a.xaml";
        //            if (ext == ".cs")
        //                recDesc.Options.DocumentSaveOptions.CurrentFileName = "a.cs";
        //            if (ext == ".txt")
        //                recDesc.Options.DocumentSaveOptions.CurrentFileName = "a.txt";

        //            string[] valuesArray = File.ReadAllLines(aryFiles.GetValue(i).ToString(), Encoding.UTF8);
        //            if (valuesArray.Length > 0)
        //            {
        //                valuesArray.ToList().ForEach(p =>
        //                {
        //                    if (p != "")
        //                        builder.Append(p + "\r\n");
        //                });
        //            }
        //        }
        //        this.recDesc.Text = builder.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgHelper.ShowError(ex.Message);
        //        WriteException(ex.Message);
        //    }
        //}
        #endregion 拖放文件到RichEditor -- 舍弃
    }
}