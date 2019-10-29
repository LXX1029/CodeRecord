using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using DataEntitys;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using static Common.MsgHelper;
using static Services.Unity.UnityContainerManager;
namespace DLCodeRecord.DevelopForms
{
    /// <summary>
    /// 用户权限设置窗体类
    /// </summary>
    public partial class DevelopUserFrm : BaseFrm
    {
        #region 属性

        /// <summary>
        /// 数据管理类
        /// </summary>
        private DataManage _dataManage = null;

        /// <summary>
        /// 拼接 设置权限内容
        /// </summary>
        private StringBuilder _powerBuilder = new StringBuilder();

        /// <summary>
        /// 当前选择的User
        /// </summary>
        public DevelopUser selectedUser { get; set; }

        #endregion 属性

        #region 构造函数
        public DevelopUserFrm()
        {
            InitializeComponent();
            #region 初始化设置
            this.Text = "用户权限管理";
            _dataManage = DataManage.Instance;
            InitialGridControl();
            InitialTreeList();
            #endregion 初始化设置
            this.Load -= DevelopUserFrm_Load;
            this.Load += DevelopUserFrm_Load;
            this.FormClosing -= DevelopUserFrm_FormClosing;
            this.FormClosing += DevelopUserFrm_FormClosing;
        }
        #endregion

        #region 窗口关闭
        private void DevelopUserFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClosingTip(e, () =>
            {
                _dataManage?.DevelopUserList.Clear();
                _dataManage?.DevelopFunList.Clear();
            });
        }
        #endregion

        #region 窗口加载
        private async void DevelopUserFrm_Load(object sender, EventArgs e)
        {
            var tuple = UtilityHelper.GetWorkingAreaSize(0.6, 0.6);
            this.Size = tuple.Item3;
            this.Location = tuple.Item4;
            await ReLoadDevelopUser();
        }

        #endregion 窗口加载

        #region 初始化gridControl

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitialGridControl()
        {
            // 用户数据源绑定
            this.gcUser.DataBindings.Add(new Binding("DataSource", _dataManage, "DevelopUserList", true, DataSourceUpdateMode.OnPropertyChanged));
            this.gcUser.BeginInit();
            // Id 列
            GridColumn idCol = new GridColumn
            {
                VisibleIndex = -1,
                Caption = "编号",
                FieldName = "UserId",
            };

            // 姓名列
            GridColumn nameCol = new GridColumn
            {
                VisibleIndex = 0,
                Caption = "姓名",
                MinWidth = 100,
            };
            nameCol.OptionsColumn.AllowMove = false;
            nameCol.FieldName = "Name";
            // 性别列
            GridColumn sexCol = new GridColumn
            {
                VisibleIndex = 1,
                Caption = "性别",
                FieldName = "Sex",
                MinWidth = 100,
            };
            RepositoryItemComboBox repositoryComboBox = new RepositoryItemComboBox
            {
                NullValuePrompt = "请选择",
                TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor,
            };
            repositoryComboBox.Items.AddRange(new string[] { "男", "女" });
            sexCol.ColumnEdit = repositoryComboBox;
            // this.gcUser.RepositoryItems.Add(repositoryComboBox);
            // 工作年限列
            GridColumn developAgeCol = new GridColumn
            {
                VisibleIndex = 2,
                Caption = "工作年限",
                FieldName = "DevelopAge",
                MinWidth = 100,
            };
            // 添加到列集合
            this.gvUser.Columns.AddRange(new GridColumn[] { idCol, nameCol, sexCol, developAgeCol });
            this.gvUser.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvUser.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.gvUser.NewItemRowText = "点击请输入";
            // 不显示分组面板
            this.gvUser.OptionsView.ShowGroupPanel = false;
            // 整行选中样式
            this.gvUser.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            // 整行选中
            this.gvUser.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
            this.gvUser.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowForFocusedRow;
            this.gvUser.ValidateRow -= new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(GvUser_ValidateRow);
            this.gvUser.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(GvUser_ValidateRow);
            this.gvUser.InvalidRowException -= new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(GvUser_InvalidRowException);
            this.gvUser.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(GvUser_InvalidRowException);
            this.gvUser.KeyUp -= new KeyEventHandler(GvUser_KeyUp);
            this.gvUser.KeyUp += new KeyEventHandler(GvUser_KeyUp);
            this.gvUser.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(GvUser_FocusedRowChanged);
            this.gvUser.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(GvUser_FocusedRowChanged);
            this.gcUser.EndInit();
        }

        /// <summary>
        /// 选择行改变事件 显示对应权限
        /// </summary>
        private void GvUser_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                selectedUser = gvUser.GetFocusedRow() as DevelopUser;
                if (selectedUser == null) return;
                SetUserFun(selectedUser);
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }

        /// <summary>
        /// 设置验证错误提示方式
        /// </summary>
        private void GvUser_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            // 不弹出提示框
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }
        /// <summary>
        /// 按键事件
        /// </summary>
        private async void GvUser_KeyUp(object sender, KeyEventArgs e)
        {
            // 非回车键返回
            if (e.KeyCode != Keys.Enter) return;
            // 添加到数据库
            try
            {
                object obj = gvUser.GetFocusedRow();
                // 触发行数据验证事件，未验证成功，不执行修改操作
                if (gvUser.UpdateCurrentRow() == false) return;
                selectedUser = gvUser.GetFocusedRow() as DevelopUser;
                if (selectedUser?.Id > 0)
                {
                    await UnityUserFacade.UpdateEntity(selectedUser);
                    ShowInfo("修改成功");
                }
                actionState = DevelopActiveState.Normal;
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
            finally
            {
                await ReLoadDevelopUser();
            }
        }

        /// <summary>
        /// 验证行 
        /// 在添加新行/编辑行时触发
        /// </summary>
        private async void GvUser_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView view = sender as GridView;
            // 名字不能为空
            selectedUser = e.Row as DevelopUser;
            if (string.IsNullOrEmpty(selectedUser.Name))
            {
                e.Valid = false;
                // view.SetColumnError(view.Columns[0], "用户名不能为空");
                MsgHelper.ShowError("姓名不能为空");
                return;
            }
            // 名称是否存在
            if (_dataManage.DevelopUserList.Count(u => u.Name == selectedUser.Name) == 2)
            {
                MsgHelper.ShowError("该名称已存在");
                e.Valid = false;
                return;
            }
            if (!VerifyHelper.IsNumberic(selectedUser.DevelopAge.ToString()))
            {
                e.Valid = false;
                // view.SetColumnError(view.Columns[0], "格式输入不正确");
                MsgHelper.ShowError("年龄格式输入不正确");
                return;
            }
            if (selectedUser.Id == 0)
            {
                selectedUser.Pwd = UtilityHelper.MD5Encrypt("111111");
                var result = await UnityUserFacade.AddEntity(selectedUser);
                if (result.Id > 0)
                {
                    MsgHelper.ShowInfo("添加成功");
                    await ReLoadDevelopUser();
                    gvUser.RefreshData();
                    this.gvUser.FocusedRowHandle = this.gvUser.RowCount - 1;
                }
            }
        }

        /// <summary>
        /// 从数据库重新读取用户数据
        /// </summary>
        private async Task ReLoadDevelopUser()
        {
            try
            {
                ShowSplashScreenForm(PromptHelper.D_LOADINGDATA);
                IList<DevelopUser> userArray = await UnityUserFacade.GetDevelopUsers(m => m.Id != 1);
                IList<DevelopFun> funList = await UnityDevelopFunFacade.GetEntities();
                _dataManage.DevelopUserList.Clear();
                foreach (DevelopUser user in userArray)
                    _dataManage?.DevelopUserList.Add(user);
                if (funList.Count() == 0) return;
                _dataManage?.DevelopFunList.Clear();
                foreach (DevelopFun fun in funList)
                    _dataManage?.DevelopFunList.Add(fun);
                funList.Clear();
                CloseSplashScreenForm();
            }
            catch (Exception ex)
            {
                CatchLoadException(this, ex);
            }
            finally
            {
                CloseSplashScreenForm();
            }
        }

        #endregion 初始化gridControl

        #region 初始化TreeList
        /// <summary>
        /// 初始化TreeList
        /// </summary>
        private void InitialTreeList()
        {
            // 用户数据源绑定
            tlUserPower.DataBindings.Add("DataSource", _dataManage, "DevelopFunList", true, DataSourceUpdateMode.OnPropertyChanged, "暂无数据");
            tlUserPower.BeginInit();
            TreeListColumn idCol = new TreeListColumn()
            {
                VisibleIndex = -1,
                FieldName = "FunId",
            };
            // 功能名称列
            TreeListColumn nameCol = new TreeListColumn()
            {
                VisibleIndex = 0,
                FieldName = "Name",
                Caption = "功能名称",
            };
            nameCol.OptionsColumn.AllowEdit = false;

            TreeListColumn parentIdCol = new TreeListColumn()
            {
                VisibleIndex = -1,
                FieldName = "Id",
            };
            this.tlUserPower.Columns.AddRange(new TreeListColumn[] { idCol, nameCol, parentIdCol });
            this.tlUserPower.ParentFieldName = "ParentID";
            this.tlUserPower.KeyFieldName = "Id";
            this.tlUserPower.OptionsView.ShowCheckBoxes = true;
            this.tlUserPower.OptionsSelection.MultiSelect = true;
            this.tlUserPower.OptionsView.ShowHorzLines = true;
            this.tlUserPower.AfterCheckNode -= new DevExpress.XtraTreeList.NodeEventHandler(TlUserPower_AfterCheckNode);
            this.tlUserPower.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(TlUserPower_AfterCheckNode);
            tlUserPower.EndInit();
        }

        /// <summary>
        /// 递归设置节点状态
        /// </summary>
        /// <param name="treeListNode">节点</param>
        /// <param name="isChecked">状态</param>
        private void SetTreeListNodeCheck(TreeListNode treeListNode, bool isChecked)
        {
            treeListNode.Checked = isChecked;
            foreach (TreeListNode node in treeListNode.Nodes)
            {
                node.Checked = isChecked;
                if (node != null && node.HasChildren)
                    SetTreeListNodeCheck(node, isChecked);
            }
        }

        /// <summary>
        /// 根据选择的用户，设置对应权限选中
        /// </summary>
        /// <param name="user">要设置权限的用户</param>
        private void SetUserFun(DevelopUser user)
        {
            foreach (TreeListNode node in this.tlUserPower.Nodes)
                SetTreeListNodeCheck(node, false);
            // 根据用户Id 获取对应权限
            List<DevelopPowerFun> developPowerFuns = user.DevelopPowerFuns.ToList();
            _dataManage?.DevelopPowerFunList.Clear();
            foreach (DevelopPowerFun powerFun in developPowerFuns)
            {
                if (powerFun == null) continue;
                TreeListNode node = tlUserPower.FindNodeByKeyID(powerFun.FunId);
                if (node == null) continue;
                node.Checked = powerFun.IsEnabled;
                // 添加到集合
                _dataManage?.DevelopPowerFunList.Add(powerFun);
            }
            this.tlUserPower.ExpandAll();
        }

        /// <summary>
        ///  节点CheckBox事件
        /// </summary>
        private void TlUserPower_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            if (actionState != DevelopActiveState.Updating)
                actionState = DevelopActiveState.Updating;

            // 选中自身并选中孩子节点
            if (e.Node.Checked && e.Node.HasChildren)
            {
                e.Node.CheckAll();
            }
            else if (e.Node.Checked && e.Node.HasChildren == false)
            {
                // 不存在子节点，存在父节点，选中父节点
                e.Node.Checked = true;
                if (e.Node.ParentNode != null)
                {
                    e.Node.ParentNode.Checked = true;
                }
            }
            else
            {
                // 存在父节点，根据选择的节点数量判定父节点是否也选中
                if (e.Node.ParentNode != null)
                {
                    int unCheckedCount = 0;
                    foreach (TreeListNode node in e.Node.ParentNode.Nodes)
                    {
                        if (node.Checked)
                        {
                            e.Node.ParentNode.Checked = true;
                            break;
                        }
                        else
                        {
                            unCheckedCount++;
                        }
                    }
                    if (unCheckedCount == e.Node.ParentNode.Nodes.Count)
                        e.Node.ParentNode.Checked = false;
                }
                e.Node.UncheckAll();
            }
        }

        #endregion 初始化TreeList

        #region 权限设置保存
        /// <summary>
        /// 权限设置保存
        /// </summary>
        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (selectedUser == null)
            {
                MsgHelper.ShowInfo("请选择要设置权限的用户");
                return;
            }
            try
            {
                TreeListNodes nodeList = this.tlUserPower.Nodes;
                if (nodeList.Count == 0) return;
                foreach (TreeListNode node in nodeList)
                    await SaveTreeListNode(node);
                MsgHelper.ShowInfo(PromptHelper.D_SETTING_SUCCESS);
                await ReLoadDevelopUser();
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
            finally
            {
                actionState = DevelopActiveState.Normal;
            }
        }

        /// <summary>
        /// 递归根据选择的权限保存到数据库
        /// </summary>
        /// <param name="treeListNode">权限节点</param>
        private async Task SaveTreeListNode(TreeListNode treeListNode)
        {
            bool isChecked = treeListNode.Checked;
            if (tlUserPower.GetDataRecordByNode(treeListNode) is DevelopFun powerFun && selectedUser != null)
            {
                DevelopPowerFun developPowerFun = await UnityDevelopPowerFunFacade.SetDevelopPowerFun(new DevelopPowerFun
                {
                    FunId = powerFun.Id,
                    UserId = selectedUser.Id,
                    IsEnabled = treeListNode.Checked,
                });
                if (developPowerFun != null)
                {
                    if (isChecked)
                        Logger.InfoFormat($"用户名为：{selectedUser.Name}，添加权限：{powerFun.Name}");
                    else
                        Logger.InfoFormat($"用户名为：{selectedUser.Name}，移除权限：{powerFun.Name}");
                }
                foreach (TreeListNode node in treeListNode.Nodes)
                {
                    // 获取节点包含的数据对象
                    if (tlUserPower.GetDataRecordByNode(node) is DevelopFun powerFunData)
                    {
                        developPowerFun = await UnityDevelopPowerFunFacade.SetDevelopPowerFun(new DevelopPowerFun
                        {
                            FunId = powerFunData.Id,
                            UserId = selectedUser.Id,
                            IsEnabled = node.Checked,
                        });
                        if (developPowerFun != null)
                        {
                            if (isChecked)
                                Logger.InfoFormat($"用户名为：{selectedUser.Name}，添加权限：{powerFun.Name}-{powerFunData.Name}");
                            else
                                Logger.InfoFormat($"用户名为：{selectedUser.Name}，移除权限：{powerFun.Name}-{powerFunData.Name}");
                        }
                        if (node != null && node.HasChildren)
                            await SaveTreeListNode(node);
                    }
                }
            }
        }
        #endregion 权限设置保存

        #region 删除
        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedUser == null)
                {
                    MsgHelper.ShowInfo(PromptHelper.D_SELECT_DATAROW);
                    return;
                }
                if (MsgHelper.ShowConfirm(PromptHelper.D_DELETE_CONFIRM) == DialogResult.OK)
                {
                    int affectedRows = await UnityUserFacade.RemoveEntity(selectedUser);
                    if (affectedRows > 0)
                    {
                        await ReLoadDevelopUser();
                        gvUser.RefreshData();
                        ShowInfo(PromptHelper.D_DELETE_SUCCESS);
                    }
                    else
                    {
                        ShowInfo(PromptHelper.D_DELETE_FAIL);
                    }
                }
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }
        #endregion
    }
}