using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using DataEntitys;
using DevExpress.Skins;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using DLCodeRecord.Reports;
using log4net;
using static Common.ExceptionHelper;
using static Services.Unity.UnityContainerManager;
namespace DLCodeRecord.DevelopForms
{
    /// <summary>
    /// 主窗体类
    /// </summary>
    public partial class DevelopManageFrm : BaseFrm
    {
        #region 变量

        /// <summary>
        /// 数据管理类
        /// </summary>
        private DataManage _dataManage = null;

        /// <summary>
        /// 计时器
        /// </summary>
        private System.Windows.Forms.Timer _timer;

        private DevelopRecordEntity SelectedDevelopRecordEntity { get; set; }

        /// <summary>
        /// 总数据量
        /// </summary>
        private static int TotalCount { get; set; }
        /// <summary>
        /// 每次读取量
        /// </summary>
        private static int StepCount { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        private int PagerCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        private int TotalPagerCount { get; set; }

        #endregion 变量

        #region 构造函数
        public DevelopManageFrm()
        {
            InitializeComponent();
            #region 初始化设置
            this.Text = "Bug/Code记录";
            this.TopMost = false;
            this.Load -= new EventHandler(DevelopManageFrm_Load);
            this.Load += new EventHandler(DevelopManageFrm_Load);
            this.FormClosing -= new FormClosingEventHandler(DevelopManageFrm_FormClosing);
            this.FormClosing += new FormClosingEventHandler(DevelopManageFrm_FormClosing);
            this.FormClosed -= DevelopManageFrm_FormClosed;
            this.FormClosed += DevelopManageFrm_FormClosed;
            #endregion 初始化设置
        }
        #endregion 构造函数

        #region 窗体加载
        private async void DevelopManageFrm_Load(object sender, EventArgs e)
        {
            if (int.TryParse(UtilityHelper.GetConfigurationKeyValue("perReadCount"), out int perReadCount))
            {
                StepCount = perReadCount;
            }
            else
            {
                this.Close();
                return;
            }
            _dataManage = DataManage.Instance;
            var tuple = UtilityHelper.GetWorkingAreaSize();
            this.Size = tuple.Item1;
            this.Location = tuple.Item2;

            #region 构建显示columns
            // 大版块列
            GridColumn colParentType = new GridColumn()
            {
                Caption = "大版块",
                FieldName = "ParentTypeName",
                VisibleIndex = 0,
                Width = 150,
            };
            colParentType.OptionsColumn.AllowFocus = false;
            colParentType.OptionsColumn.AllowSize = false;
            // 小版块列
            GridColumn colType = new GridColumn()
            {
                Caption = "小版块",
                FieldName = "SubTypeName",
                VisibleIndex = 1,
                Width = 150,
            };
            colType.OptionsColumn.AllowFocus = false;

            // 标题列
            GridColumn colTitle = new GridColumn()
            {
                VisibleIndex = 2,
                Caption = "标题",
                FieldName = "Title",
                MinWidth = 400,

            };
            colTitle.OptionsColumn.AllowFocus = false;
            colTitle.OptionsColumn.AllowSize = false;
            // 描述列
            GridColumn colDesc = new GridColumn()
            {
                VisibleIndex = 3,
                Caption = "描述",
                FieldName = "Desc",
                MinWidth = 400,
            };
            colDesc.OptionsColumn.ReadOnly = true;
            colDesc.OptionsColumn.AllowSize = true;
            RepositoryItemMemoExEdit repositoryItemMemoExEdit = new RepositoryItemMemoExEdit()
            {
                NullText = "暂无描述",
                AutoHeight = true,
                AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True,
                PopupFormMinSize = new Size(800, 500),
                ScrollBars = ScrollBars.Both,
            };
            colDesc.ColumnEdit = repositoryItemMemoExEdit;
            // 图片列
            GridColumn colImagePath = new GridColumn()
            {
                VisibleIndex = 4,
                Caption = "效果图",
                FieldName = "BitMap",
                MinWidth = 100,
            };
            colImagePath.OptionsColumn.ReadOnly = true;
            colImagePath.OptionsColumn.AllowSize = false;
            RepositoryItemImageEdit repositoryItemImageEdit = new RepositoryItemImageEdit()
            {
                NullText = "暂无效果图",
                Name = "edit1",
                AutoHeight = true,
                PictureAlignment = ContentAlignment.MiddleCenter,
                SizeMode = PictureSizeMode.Squeeze,
                PopupFormMinSize = new Size(800, 500),
                AllowNullInput = DevExpress.Utils.DefaultBoolean.True,
                NullValuePrompt = "暂无图片",
            };
            colImagePath.ColumnEdit = repositoryItemImageEdit;
            // 创建人列
            GridColumn colUserName = new GridColumn()
            {
                VisibleIndex = 5,
                Caption = "创建人",
                FieldName = "UserName",
                Width = 60,
            };
            colUserName.OptionsColumn.ReadOnly = true;
            colUserName.OptionsColumn.AllowFocus = false;
            colUserName.OptionsColumn.AllowSize = false;
            // 创建时间列
            GridColumn colCreatedTime = new GridColumn()
            {
                VisibleIndex = 6,
                Caption = "创建时间",
                FieldName = "CreatedTime",
                MinWidth = 150,
                MaxWidth = 150,

            };
            colCreatedTime.OptionsColumn.AllowFocus = false;
            colCreatedTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            colCreatedTime.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            // 更新时间列
            GridColumn colUpdatedTime = new GridColumn()
            {
                VisibleIndex = 7,
                Caption = "更新时间",
                FieldName = "UpdatedTime",
                MinWidth = 150,
                MaxWidth = 150,
            };
            colUpdatedTime.OptionsColumn.AllowFocus = false;
            // colUpdatedTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            colUpdatedTime.DisplayFormat.FormatType = FormatType.Custom;
            colUpdatedTime.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            // 操作列
            GridColumn colView = new GridColumn()
            {
                Name = "Operation",
                VisibleIndex = 8,
                Caption = "操作",
                MinWidth = 120,
            };
            RepositoryItemButtonEdit repositoryItemButtonEdit = new RepositoryItemButtonEdit()
            {
                Name = "btn",
                ButtonsStyle = BorderStyles.Default,
                TextEditStyle = TextEditStyles.HideTextEditor,
            };
            repositoryItemButtonEdit.Buttons[0].Visible = false; // 默认不显示第一个按钮

            EditorButton ebView = new EditorButton()
            {
                Caption = "查看",
                Kind = ButtonPredefines.Glyph, // 自定义按钮bitMap
                Image = gvImageCollection.Images[1], // 按钮图片索引
                ImageLocation = ImageLocation.MiddleRight, // 按钮图片位置
            };
            EditorButton ebDownLoad = new EditorButton()
            {
                Caption = "获取压缩包",
                Kind = ButtonPredefines.Glyph,
                Image = gvImageCollection.Images[0],
                ImageLocation = ImageLocation.MiddleRight,
            };
            // 添加按钮到RepositoryItemButtonEdit
            repositoryItemButtonEdit.Buttons.AddRange(new EditorButton[] { ebView, ebDownLoad });
            colView.ColumnEdit = repositoryItemButtonEdit;
            #region 按钮列操作事件
            // 按钮列操作事件
            repositoryItemButtonEdit.ButtonClick += RepositoryItemButtonEdit_ButtonClick;

            #endregion

            this.gvDevelop.BeginInit();
            // 添加列到gvDevelop
            this.gvDevelop.Columns.AddRange(new GridColumn[] { colParentType, colType, colTitle, colDesc, colCreatedTime, colUpdatedTime, colImagePath, colView, colUserName });
            /*gcDevelop.RepositoryItems.Add(repositoryItemImageEdit);
            gcDevelop.RepositoryItems.Add(repositoryItemMemoExEdit);
            this.gvDevelop.Columns["Desc"].ColumnEdit = repositoryItemMemoExEdit;
            this.gvDevelop.Columns["BitMap"].ColumnEdit = repositoryItemImageEdit;*/
            // 添加统计
            this.gvDevelop.Columns["SubTypeName"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "SubTypeName", "共计：{0}");
            this.gvDevelop.OptionsView.ShowFooter = true;
            this.gvDevelop.OptionsFilter.AllowFilterEditor = false;
            // 是否启用列过滤
            this.gvDevelop.OptionsCustomization.AllowFilter = true;
            // 禁用菜单
            this.gvDevelop.OptionsMenu.EnableColumnMenu = true;
            this.gvDevelop.OptionsMenu.EnableFooterMenu = false;
            // 空数据显示提示
            this.gvDevelop.CustomDrawEmptyForeground += new DevExpress.XtraGrid.Views.Base.CustomDrawEventHandler(GvDevelop_CustomDrawEmptyForeground);
            // 设置过滤模式
            this.gvDevelop.OptionsFind.FindMode = FindMode.FindClick;
            // 设置过滤字段
            this.gvDevelop.OptionsFind.FindFilterColumns = "ParentTypeName;Title;SubTypeName;Desc";
            // 控制bar是否可以关闭
            this.barStatus.OptionsBar.DisableClose = false;
            this.barStatus.OptionsBar.DisableCustomization = false;
            this.barStatus.Manager.AllowCustomization = false;
            // 控制bar是否显示右键菜单
            this.barStatus.Manager.AllowShowToolbarsPopup = false;
            this.gvDevelop.EndInit();
            this.gvDevelop.FocusedRowChanged -= GvDevelop_FocusedRowChanged;
            this.gvDevelop.FocusedRowChanged += GvDevelop_FocusedRowChanged;
            this.gcDevelop.DataBindings.Add("DataSource", _dataManage, "DevelopRecordEntityList", true, DataSourceUpdateMode.OnPropertyChanged, "暂无数据");

            _dataManage.DevelopRecordEntityList.ListChanged -= DevelopRecordEntityList_ListChanged;
            _dataManage.DevelopRecordEntityList.ListChanged += DevelopRecordEntityList_ListChanged;
            #endregion 构建columns

            #region 构建菜单项,显示当前用户

            DevelopUser loginUser = DataManage.LoginUser;
            try
            {
                await InitialFunMenu(loginUser);
            }
            catch (Exception ex)
            {
                CatchException(ex);
                Application.Exit();
            }

            bsiUser.Caption = "当前用户:" + DataManage.LoginUser?.Name;
            #endregion 构建菜单项,显示当前用户

            #region Timer
            _timer = new System.Windows.Forms.Timer();
            _timer.Tick -= new EventHandler(Timer_Tick);
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Interval = 1000;
            #endregion Timer
            OnCloseLoginFrmHandler();
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            await LoadDevelopRecord(CancellationToken.None);
            sw.Stop();
            Console.WriteLine($"loaded time:{sw.ElapsedMilliseconds}");
            _timer.Start();
        }

        /// <summary>
        /// 集合改变
        /// </summary>
        private void DevelopRecordEntityList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                gcDevelop.RefreshDataSource();
                gvDevelop.FocusedRowHandle = this.SelectedRowIndex;
            }
            else if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                gcDevelop.RefreshDataSource();
                gvDevelop.FocusedRowHandle = 0;
                this.Activate();
            }
        }

        /// <summary>
        /// 操作列事件
        /// </summary>
        private async void RepositoryItemButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                await DealData(e);
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }

        #endregion 窗体加载

        #region 查看，获取压缩包操作
        private async Task DealData(ButtonPressedEventArgs e)
        {
            string caption = e.Button.Caption;
            if (caption == "查看")
            {
                if (gvDevelop.GetFocusedRow() is DevelopRecordEntity entity)
                {
                    // 更新数据库
                    bool result = await UnityDevelopRecordFacade.UpdateDevelopRecordClickCount(entity.Id);
                    DevelopViewFrm developViewFrm = new DevelopViewFrm(entity)
                    {
                        Owner = this,
                    };
                    developViewFrm.ShowDialog();
                    this.Activate();
                }
            }
            // 下载上传的压缩包
            if (caption == "获取压缩包")
            {
                if (gvDevelop.GetFocusedRow() is DevelopRecordEntity entity)
                {
                    DevelopRecord record = await UnityDevelopRecordFacade.GetEntity(entity.Id);
                    if (record.Zip == null)
                    {
                        MsgHelper.ShowInfo(PromptHelper.D_NOPACKAGE);
                        return;
                    }
                    MsgHelper.ShowWaitingForm(this, PromptHelper.D_DOWNLOADINGPACKAGE);
                    await UtilityHelper.ConvertByteToZip(record.Zip, record.Title);
                    this.Invoke(new Action(async () =>
                    {
                        // 设置完毕提示
                        SplashScreenManager.Default.SetWaitFormCaption(PromptHelper.D_DOWNLOADING_SUCCESS);
                        await Task.Delay(500);
                        MsgHelper.CloseSplashScreenForm();
                        // 下载完成，最小化
                        this.WindowState = FormWindowState.Minimized;
                    }));
                }
            }
        }
        #endregion

        #region 选择行改变事件
        private void GvDevelop_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            object obj = this.gvDevelop.GetFocusedRow();
            if (obj == null)
            {
                return;
            }
            SelectedDevelopRecordEntity = obj as DevelopRecordEntity;
        }
        #endregion

        #region 窗口关闭

        /// <summary>
        /// 窗口关闭，清理资源
        /// </summary>
        private void DevelopManageFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_timer.Enabled)
                _timer.Stop();
            this.Hide();
            Application.Exit();
        }
        /// <summary>
        /// 窗口即将关闭时
        /// </summary>
        private void DevelopManageFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ExitConfirm() == DialogResult.Cancel)
                e.Cancel = true;
        }

        /// <summary>
        /// 退出提示
        /// </summary>
        private DialogResult ExitConfirm() => MsgHelper.ShowConfirm(PromptHelper.D_EXIST_CONFIRM);
        #endregion 窗口关闭

        #region 计时器事件

        private void Timer_Tick(object sender, EventArgs e) => barStaticItem1.Caption = $"😄{DateTime.Now.ToString()}";

        #endregion 计时器事件

        #region 自定义绘制 空数据显示文字

        /// <summary>
        /// 自定义绘制 空数据显示文字
        /// </summary>
        private void GvDevelop_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            GridView gv = sender as GridView;
            int rowCount = gv.DataRowCount;
            if (rowCount == 0)
            {
                Graphics graphics = e.Graphics;
                Rectangle r = new Rectangle(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5);
                graphics.DrawString("暂无记录", new System.Drawing.Font("宋体", 12, FontStyle.Bold), Brushes.Red, new PointF(e.Bounds.X + 10, e.Bounds.Y + 10));
            }
        }

        #endregion 自定义绘制 空数据显示文字

        #region 触发隐藏登陆窗口事件
        public void AttatchedHandler(Action handler)
        {
            if (handler == null) return;
            this._closeLoginFrmHandler -= handler;
            this._closeLoginFrmHandler += handler;
        }

        /// <summary>
        /// 隐藏登陆窗口事件
        /// </summary>
        private Action _closeLoginFrmHandler;

        private void OnCloseLoginFrmHandler()
        {
            _closeLoginFrmHandler?.Invoke();
        }

        #endregion 触发隐藏登陆窗口事件

        #region 根据用户Id,生成所有功能项菜单
        /// <summary>
        /// 初始化菜单
        /// </summary>
        /// <param name="developUser">用户</param>
        private async Task InitialFunMenu(DevelopUser developUser)
        {
            ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            ribbonControl1.AllowMinimizeRibbon = false;  // 不允许最小化Ribbon
            #region 权限判断创建菜单
            // 管理员
            if (developUser.Name == "admin")
            {
                IList<DevelopFun> funList = await UnityDevelopFunFacade.GetEntities();
                if (funList != null)
                {
                    // 遍历父节点
                    foreach (DevelopFun parentFun in funList.Where(f => f.ParentID == 0))
                    {
                        bool hasChildItem = funList.Any(a => a.ParentID == parentFun.Id);
                        RibbonPage parentPage = new RibbonPage(parentFun.Name);
                        // 主菜单  存在子菜单添加，不存在添加相同的主菜单
                        if (parentFun.ParentID == 0 && hasChildItem)
                        {
                            IEnumerable<DevelopFun> listChild = funList.Where(w => w.ParentID == parentFun.Id);
                            // 遍历子项
                            foreach (DevelopFun childrenFun in listChild)
                            {
                                RibbonPageGroup rpg = new RibbonPageGroup(childrenFun.Name);
                                BarButtonItem buttonItem = ribbonControl1.Items.CreateButton(childrenFun.Name);
                                buttonItem.RibbonStyle = RibbonItemStyles.Large;
                                buttonItem.LargeWidth = 70;
                                buttonItem.Id = childrenFun.Id;
                                SuperToolTip tip = new SuperToolTip();
                                // 提示内容
                                tip.Items.Add(new ToolTipItem() { Text = $"#{childrenFun.Name}#" });
                                buttonItem.SuperTip = tip;
                                rpg.ItemLinks.Add(buttonItem);
                                buttonItem.ImageIndex = (int)childrenFun.ImageIndex;
                                buttonItem.ItemClick += new ItemClickEventHandler(Item_ItemClick);
                                parentPage.Groups.Add(rpg);
                                ribbonControl1.Pages.Add(parentPage);
                            }
                        }
                    }
                }
            }
            else
            {
                // 获取当前用户对应的菜单权限关系
                IEnumerable<DevelopPowerFun> funList = DataManage.LoginUser.DevelopPowerFuns.OrderBy(o => o?.Id);
                if (funList != null && funList.Any(m => m.DevelopFun != null))
                {
                    // 遍历父节点
                    foreach (DevelopPowerFun parentFun in funList.Where(f => f.DevelopFun.ParentID == 0 && f.IsEnabled == true))
                    {
                        bool hasChildItem = funList.Any(a => a.DevelopFun?.ParentID == parentFun.FunId);
                        RibbonPage parentPage = new RibbonPage(parentFun.DevelopFun.Name);
                        // 主菜单
                        if (parentFun.DevelopFun.ParentID == 0 && hasChildItem)
                        {
                            IEnumerable<DevelopPowerFun> listChild = funList.Where(w => w.DevelopFun.ParentID == parentFun.FunId && w.IsEnabled == true);
                            // 遍历子项
                            foreach (DevelopPowerFun childrenFun in listChild)
                            {
                                RibbonPageGroup rpg = new RibbonPageGroup(childrenFun.DevelopFun.Name);
                                BarButtonItem buttonItem = ribbonControl1.Items.CreateButton(childrenFun.DevelopFun.Name);
                                buttonItem.RibbonStyle = RibbonItemStyles.Large;
                                buttonItem.LargeWidth = 70;
                                buttonItem.Id = childrenFun.FunId;
                                rpg.ItemLinks.Add(buttonItem);
                                buttonItem.ImageIndex = (int)childrenFun.DevelopFun.ImageIndex;
                                buttonItem.ItemClick -= new ItemClickEventHandler(Item_ItemClick);
                                buttonItem.ItemClick += new ItemClickEventHandler(Item_ItemClick);
                                parentPage.Groups.Add(rpg);
                                ribbonControl1.Pages.Add(parentPage);
                            }
                        }
                    }
                }
            }
            #endregion

            #region 自定义的其它操作  ButtonItem的Id 从500 开始
            RibbonPage otherPage = new RibbonPage("其它操作");
            #region 退出 Id = 500
            RibbonPageGroup existrrpg = new RibbonPageGroup("退出");
            BarButtonItem existButtonItem = ribbonControl1.Items.CreateButton("退出");
            existButtonItem.RibbonStyle = RibbonItemStyles.All;
            existButtonItem.ImageIndex = 1;
            existButtonItem.LargeWidth = 50;
            existButtonItem.Id = 500;
            existButtonItem.ItemClick += new ItemClickEventHandler(Item_ItemClick);
            existrrpg.ItemLinks.Add(existButtonItem);
            #endregion

            #region 刷新数据 Id = 501
            RibbonPageGroup refreshrrpg = new RibbonPageGroup("重载数据")
            {
                Name = "refreshrrpg",
            };
            BarButtonItem refreshButtonItem = ribbonControl1.Items.CreateButton("重载数据");
            refreshButtonItem.Name = "refreshButtonItem";
            refreshButtonItem.Id = 501;
            // 刷新数据
            refreshButtonItem.RibbonStyle = RibbonItemStyles.Large;
            refreshButtonItem.ImageIndex = 5;
            refreshButtonItem.LargeWidth = 80;
            refreshButtonItem.Caption = "重载数据";
            refreshrrpg.ItemLinks.Add(refreshButtonItem);
            refreshButtonItem.ItemClick += new ItemClickEventHandler(Item_ItemClick);
            #endregion

            #region 皮肤设定 Id = 502

            SkinRibbonGalleryBarItem srgbri = new SkinRibbonGalleryBarItem
            {
                Caption = "皮肤",
                Border = BorderStyles.Flat,
                Id = 502,
            };
            srgbri.Gallery.ShowItemText = true;
            srgbri.Gallery.ImageSize = new Size(32, 32);
            srgbri.Gallery.ItemImageLayout = DevExpress.Utils.Drawing.ImageLayoutMode.ZoomInside;
            srgbri.Gallery.AllowHoverImages = true; // 允许显示 悬浮图片
            GalleryItemGroup group1 = new GalleryItemGroup
            {
                Tag = 1,
            };
            string applicationSource = AppDomain.CurrentDomain.BaseDirectory + "SkinImg\\";
            foreach (SkinContainer skin in SkinManager.Default.Skins)
            {
                string imgPath = string.Format(applicationSource + skin.SkinName + ".png");
                if (System.IO.File.Exists(imgPath) == false) continue;
                string hoverImgPath = string.Format(applicationSource + skin.SkinName + "_48x48.png");
                if (System.IO.File.Exists(hoverImgPath) == false) continue;
                Image image = Image.FromFile(imgPath);
                Image hoverImage = Image.FromFile(hoverImgPath);
                GalleryItem item = new GalleryItem(image, hoverImage, skin.SkinName, string.Empty, 1, 1, null, string.Empty);
                group1.Items.Add(item);
            }
            srgbri.Gallery.Groups.Add(group1);
            // 设置皮肤
            srgbri.Gallery.ItemClick += (sender, e) =>
            {
                int tag = Convert.ToInt32(e.Item.GalleryGroup.Tag.ToString());
                if (tag == 1)
                {
                    string caption = e.Item.Caption;
                    DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = caption;
                    // 写入配置文件
                    bool result = UtilityHelper.SetConfigurationKeyValue("skin", caption);
                    if (result == false)
                    {
                        MsgHelper.ShowError("设置失败,请检查是否存在配置文件!");
                    }
                }
            };

            RibbonPageGroup skinsrrpg = new RibbonPageGroup("皮肤设定");
            skinsrrpg.ItemLinks.Add(srgbri);
            ribbonControl1.Items.Add(srgbri);
            #endregion 皮肤设定

            #region 打印数据  Id = 503
            /*RibbonPageGroup reportrrpg = new RibbonPageGroup("打印");
            BarButtonItem reportButtonItem = ribbonControl1.Items.CreateButton("打印");
            reportButtonItem.RibbonStyle = RibbonItemStyles.Large;
            reportButtonItem.ImageIndex = 10;
            reportButtonItem.LargeWidth = 80;
            reportButtonItem.Id = 503;
            reportButtonItem.ItemClick += new ItemClickEventHandler(item_ItemClick);
            reportrrpg.ItemLinks.Add(reportButtonItem);*/
            #endregion

            otherPage.Groups.Add(refreshrrpg);
            otherPage.Groups.Add(skinsrrpg);
            // otherPage.Groups.Add(reportrrpg);
            otherPage.Groups.Add(existrrpg);
            ribbonControl1.Pages.Add(otherPage);
            foreach (RibbonPage page in ribbonControl1.Pages)
            {
                foreach (RibbonPageGroup group in page.Groups)
                    group.ShowCaptionButton = false;  // 不显示右下角箭头
            }
            #endregion
            #region 设置ApplicationButton菜单
            // 设置ApplicationButton菜单
            this.popupMenu.AddItems(new BarItem[]
            {
                refreshButtonItem,
                existButtonItem,
            });
            ribbonControl1.ApplicationButtonDropDownControl = this.popupMenu;
            #endregion
        }

        /// <summary>
        /// 菜单事件
        /// </summary>
        private async void Item_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                string caption = e.Item.Caption;
                int id = e.Item.Id;
                if (id == (int)DevelopFunCaptions.DevelopAdd)
                {
                    DevelopFrm addFrm = new DevelopFrm(null)
                    {
                        Owner = this,
                        StartPosition = FormStartPosition.CenterParent,
                    };
                    addFrm.ShowDialog();
                }
                if (id == (int)DevelopFunCaptions.DevelopTypeAdd)
                {
                    DevelopTypeAddFrm frm = new DevelopTypeAddFrm();
                    if (!IsExistForm(frm))
                    {
                        // 重新加载数据
                        frm.DeleteNodeHandler += null;
                        frm.DeleteNodeHandler += (m, n) =>
                        {
                            // 获取包含删除小版块的Record记录
                            var records = _dataManage.DevelopRecordEntityList.Where(w => w.SubTypeId == n.TypeId).ToList();
                            for (int i = 0; i < records.Count; i++)
                                _dataManage.DevelopRecordEntityList.Remove(records[i]);
                        };
                        ShowForm(frm);
                    }
                }
                if (id == (int)DevelopFunCaptions.DevelopReport)
                {
                    DevelopReportFrm frm = new DevelopReportFrm();
                    if (!IsExistForm(frm))
                        ShowForm(frm);
                }

                if (id == (int)DevelopFunCaptions.DevelopUser)
                {
                    DevelopUserFrm frm = new DevelopUserFrm();
                    if (!IsExistForm(frm))
                        ShowForm(frm);
                }
                if (id == (int)DevelopFunCaptions.DevelopSetting)
                {
                    var frm = new DevelopSettingFrm();
                    if (!IsExistForm(frm))
                        ShowForm(frm);
                }
                if (id == (int)DevelopFunCaptions.DevelopUpdate)
                    UpdateItem();
                if (id == (int)DevelopFunCaptions.DevelopDelete)
                    await Delete();
                if (id == (int)DevelopFunCaptions.ReLoadData)
                    await ReLoadData();
                if (id == (int)DevelopFunCaptions.Exist)
                    this.Close();
                if (id == (int)DevelopFunCaptions.Print)
                {
                    if (SelectedDevelopRecordEntity == null)
                    {
                        MsgHelper.ShowInfo(PromptHelper.D_SELECT_PRINTEDDATAROW);
                        return;
                    }
                    var report = new RecordReport(SelectedDevelopRecordEntity);
                    ReportFrm frm = new ReportFrm(report);
                    if (!IsExistForm(frm))
                        ShowForm(frm);
                }
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }

        #endregion 根据用户Id,生成所有功能项菜单

        #region 数据加载方法
        // 加载数据方法
        private async Task LoadDevelopRecord(CancellationToken cts)
        {
            _dataManage.DevelopRecordEntityList.Clear();
            using (cts.Register(() =>
             {
                 Console.WriteLine("##cancel");
             }));
            WaitingFrm frm = null;
            this.Invoke(new Action(() =>
            {
                frm = new WaitingFrm(TotalPagerCount)
                {
                    Owner = this,
                };
                if (frm.Visible == false)
                    frm.Show();
            }));
            try
            {
                await Task.Run(async () =>
                {
                    TotalCount = await UnityDevelopRecordFacade.GetDevelopRecordListCount().ConfigureAwait(false);
                    PagerCount = TotalCount / StepCount;
                    TotalPagerCount = (TotalCount % StepCount) > 0 ? PagerCount + 1 : PagerCount;

                    for (int i = 0; i < TotalPagerCount; i++)
                    {
                        if (cts.IsCancellationRequested)
                        {
                            break;
                        }
                        var result = await UnityDevelopRecordFacade.GetDevelopRecordListByPager(i, StepCount).ConfigureAwait(false);
                        int pageIndex = i;
                        Console.WriteLine($"PageIndex:{pageIndex}");
                        Console.WriteLine($"Result:{result.Count}");
                        this.Invoke(new Action(() =>
                        {
                            frm.Percent = pageIndex + 1;
                            Console.WriteLine($"frm.Percent:{frm.Percent}");
                            for (int j = 0; j < result.Count; j++)
                            {
                                if (cts.IsCancellationRequested)
                                {
                                    break;
                                }
                                _dataManage.DevelopRecordEntityList.Add(result[j]);
                            }
                            Console.WriteLine($"DevelopRecordEntityList:{_dataManage.DevelopRecordEntityList.Count}");
                        }));
                    }
                }, cts);
                this.Invoke(new Action(() =>
                {
                    frm?.Close();
                }));

            }
            catch (Exception ex)
            {
                CatchException(ex);
                frm?.Close();
                _dataManage?.DevelopRecordEntityList.Clear();
            }
        }
        #endregion 数据加载方法 async await

        #region 重新加载
        private CancellationTokenSource _source = new CancellationTokenSource();
        /// <summary>
        /// 重新加载数据
        /// </summary>
        private async Task ReLoadData()
        {
            this.gvDevelop.FindFilterText = string.Empty;
            Interlocked.Exchange(ref _source, new CancellationTokenSource()).Cancel();
            await LoadDevelopRecord(_source.Token);
        }

        #endregion 重新加载

        #region 删除事件

        /// <summary>
        /// 删除
        /// </summary>
        private async Task Delete()
        {
            if (HandleVerify() == false) return;
            try
            {
                object obj = this.gvDevelop.GetFocusedRow();
                if (MsgHelper.ShowConfirm(PromptHelper.D_DELETE_CONFIRM) == DialogResult.OK)
                {
                    DevelopRecordEntity entity = obj as DevelopRecordEntity;
                    int affectedRows = await UnityDevelopRecordFacade.RemoveEntity(entity.Id);
                    if (affectedRows > 0)
                    {
                        _dataManage.DevelopRecordEntityList.Remove(entity);
                        MsgHelper.ShowInfo(PromptHelper.D_DELETE_SUCCESS);

                    }
                    else
                    {
                        MsgHelper.ShowInfo(PromptHelper.D_DELETE_FAIL);
                    }
                }
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }
        #endregion 删除事件

        #region 修改事件
        /// <summary>
        /// 当前选中行索引
        /// </summary>
        private int SelectedRowIndex { get; set; }
        /// <summary>
        /// 修改
        /// </summary>
        private void UpdateItem()
        {
            if (HandleVerify() == false) return;
            if (this.gvDevelop.GetFocusedRow() is DevelopRecordEntity entity)
            {
                this.SelectedRowIndex = this.gvDevelop.FocusedRowHandle;
                var frm = new DevelopFrm(entity) { Owner = this };
                frm.ShowDialog();
                this.Activate();
            }
        }

        #endregion 修改事件

        #region 右键菜单显示事件

        /// <summary>
        /// 右键菜单
        /// </summary>
        private void GcDevelop_MouseUp(object sender, MouseEventArgs e)
        {
            /*if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                object obj = this.gvDevelop.GetFocusedRow();
                if (obj == null)
                {
                    MsgHelper.ShowInfo("请选择要操作的行");
                    return;
                }
                DevelopRecordEntity entity = obj as DevelopRecordEntity;
                if (entity.Id != DataManage.CurrentUser.Id)
                {
                    MsgHelper.ShowInfo("该数据由其它人员创建，不可进行操作。");
                    return;
                }
                //popupMenu1.ShowPopup(Control.MousePosition);
            }*/
        }

        #endregion 右键菜单显示事件

        #region 操作权限判断
        /// <summary>
        /// 操作提示
        /// </summary>
        private bool HandleVerify()
        {
            if (this.gvDevelop.GetFocusedRow() is DevelopRecordEntity entity)
            {
                if (entity.Id != DataManage.LoginUser.Id && DataManage.LoginUser.Id != 1)
                {
                    MsgHelper.ShowInfo(PromptHelper.D_UnAuthority);
                    return false;
                }
            }
            else
            {
                MsgHelper.ShowInfo(PromptHelper.D_SELECT_DATAROW);
                return false;
            }
            return true;
        }

        #endregion 操作判断

        #region 窗口只显示一次
        /// <summary>
        /// 判断窗体是否已打开
        /// </summary>
        private bool IsExistForm(XtraForm frm)
        {
            if (frm == null) return false;
            foreach (Form childFrm in Application.OpenForms)
            {
                if (childFrm.Text == frm.Text)
                {
                    childFrm.Activate();
                    childFrm.BringToFront();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 打开窗口
        /// </summary>
        private void ShowForm(XtraForm frm)
        {
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
        }
        #endregion 窗口只显示一次
    }
}