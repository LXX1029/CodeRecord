using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using DataEntitys;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;
using static Common.VerifyHelper;
using static Services.Unity.UnityContainerManager;
namespace DLCodeRecord.DevelopForms
{
    /// <summary>
    /// 版块管理窗体
    /// </summary>
    public partial class DevelopTypeAddFrm : BaseFrm
    {
        #region 构造函数
        public DevelopTypeAddFrm()
        {
            InitializeComponent();

            #region 初始化设置

            this.tlDevelopList.OptionsView.ShowVertLines = true;
            this.Text = "版块管理";
            this.txtNode.Properties.MaxLength = 20;
            this.txtChild.Properties.MaxLength = 20;
            this.txtNode.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
            this.txtChild.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
            this.txtRoot.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
            this.tlDevelopList.OptionsFind.AllowFindPanel = true;
            // this.tlDevelopList.ExpandAll();
            this.tlDevelopList.FocusedNodeChanged -= tlDevelopList_FocusedNodeChanged;
            this.tlDevelopList.FocusedNodeChanged += tlDevelopList_FocusedNodeChanged;
            this.txtRoot.TextChanged -= Txt_TextChanged;
            this.txtRoot.TextChanged += Txt_TextChanged;
            this.txtChild.TextChanged -= Txt_TextChanged;
            this.txtChild.TextChanged += Txt_TextChanged;
            #endregion 初始化设置

            #region 窗体加载/关闭
            this.Load -= DevelopTypeAddFrm_Load;
            this.Load += DevelopTypeAddFrm_Load;
            this.FormClosing -= DevelopTypeAddFrm_FormClosing;
            this.FormClosing += DevelopTypeAddFrm_FormClosing;
            #endregion 窗体加载/关闭
        }
        #endregion

        #region 版块信息文本改变
        private void Txt_TextChanged(object sender, EventArgs e)
        {
            TextEdit te = (TextEdit)sender;
            if (actionState != DevelopActiveState.Adding && !IsEmptyOrNullOrWhiteSpace(te.Text))
                actionState = DevelopActiveState.Adding;
        }
        #endregion

        #region 窗口加载事件

        private async void DevelopTypeAddFrm_Load(object sender, EventArgs e)
        {
            var tuple = UtilityHelper.GetWorkingAreaSize(0.4, 0.5);
            this.Size = tuple.Item3;
            this.Location = tuple.Item4;
            ShowSplashScreenForm(PromptHelper.D_LOADINGDATA);
            await LoadDevelopType();
            CloseSplashScreenForm();
        }

        #endregion 窗口加载事件

        #region 窗口关闭
        private void DevelopTypeAddFrm_FormClosing(object sender, FormClosingEventArgs e) => FormClosingTip(e);
        #endregion

        #region 选择项改变，设置对象到按钮

        private void tlDevelopList_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            TreeListNode node = tlDevelopList.FocusedNode;
            if (tlDevelopList.GetDataRecordByNode(node) is DevelopType focusedType)
            {
                this.txtNode.Text = focusedType.Name;
                this.btnDelete.Tag = focusedType;
                this.btnEdit.Tag = focusedType;
            }
        }

        #endregion 选择项改变，设置对象到按钮

        #region 加载大小板块

        private async Task LoadDevelopType()
        {
            try
            {
                IList<DevelopType> developTypeList = await UnityDevelopTypeFacade.GetEntities();
                this.tlDevelopList.DataSource = developTypeList;
            }
            catch (Exception ex)
            {
                CatchLoadException(this, ex);
            }
        }
        #endregion 加载大小板块

        #region 添加大版块

        private async void btnAddRoot_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtRoot.Text.Trim();
                if (IsEmptyOrNullOrWhiteSpace(name))
                {
                    txtRoot.ErrorText = "请输入大版块名称";
                    txtRoot.Focus();
                    txtRoot.SelectAll();
                    return;
                }
                if (name.Length >= 20)
                {
                    txtRoot.ErrorText = "输入字符长度小于20";
                    txtRoot.Focus();
                    return;
                }

                IList<DevelopType> array = await UnityDevelopTypeFacade.GetDevelopTypeListByFilter(name, 0);
                if (array.Count > 0)
                {
                    txtRoot.ErrorText = "该版块名称已存在";
                    txtRoot.SelectAll();
                    return;
                }
                DevelopType type = new DevelopType()
                {
                    Name = name,
                };
                await UnityDevelopTypeFacade.AddEntity(type);
                this.txtRoot.Text = string.Empty;
                await LoadDevelopType();
                MsgHelper.ShowInfo(PromptHelper.D_ADD_SUCCESS);
                FocusedNode(name);
                actionState = DevelopActiveState.Normal;
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }

        #endregion 添加大版块

        #region 添加子版块

        private async void btnAddChild_Click(object sender, EventArgs e)
        {
            try
            {
                // 判断选中节点
                if (!(tlDevelopList.GetDataRecordByNode(tlDevelopList.FocusedNode) is DevelopType parentType))
                {
                    txtChild.ErrorText = "请选择大版块";
                    tlDevelopList.Focus();
                    return;
                }

                string name = this.txtChild.Text.Trim();
                if (IsEmptyOrNullOrWhiteSpace(name))
                {
                    txtChild.ErrorText = "请输入小版块名称";
                    txtChild.Focus();
                    txtChild.SelectAll();
                    return;
                }

                if (name.Length >= 20)
                {
                    txtRoot.ErrorText = "输入字符长度小于20";
                    txtRoot.Focus();
                    return;
                }

                IList<DevelopType> array = await UnityDevelopTypeFacade.GetDevelopTypeListByFilter(name, parentType.Id);
                if (array.Count > 0)
                {
                    txtChild.ErrorText = "该版块已存在";
                    txtChild.SelectAll();
                    return;
                }
                if (parentType == null)
                    return;
                DevelopType type = new DevelopType()
                {
                    Name = name,
                    ParentId = parentType.Id,
                    CreatedTime = DateTime.Now,
                };
                await UnityDevelopTypeFacade.AddEntity(type);
                txtChild.Text = string.Empty;
                await LoadDevelopType();
                tlDevelopList.FocusedNode.ExpandAll();
                MsgHelper.ShowInfo(PromptHelper.D_ADD_SUCCESS);
                FocusedNode(name);
                actionState = DevelopActiveState.Normal;
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }

        #endregion 添加子版块

        #region 删除节点
        /// <summary>
        /// 删除事件
        /// </summary>
        public event EventHandler<DeleteEventArgs> DeleteNodeHandler;

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is SimpleButton btn)
                {
                    DevelopType type = btn.Tag as DevelopType;
                    if (MsgHelper.ShowConfirm(PromptHelper.D_DELETE_CONFIRM) == DialogResult.OK)
                    {
                        TreeListNode parentNode = tlDevelopList.FocusedNode.ParentNode;
                        // 执行删除
                        int affectedRows = await UnityDevelopTypeFacade.RemoveEntity(type);
                        if (affectedRows > 0)
                        {
                            await LoadDevelopType();
                            txtNode.Text = string.Empty;
                            // 删除事件
                            var handler = DeleteNodeHandler;
                            handler?.Invoke(this, new DeleteEventArgs() { TypeId = type.Id });
                            // 如果存在ParentNode 选中
                            DevelopType parentType = tlDevelopList.GetDataRecordByNode(parentNode) as DevelopType;
                            FocusedNode(parentType?.Name);
                            MsgHelper.ShowInfo(PromptHelper.D_DELETE_SUCCESS);
                        }
                        else
                        {
                            MsgHelper.ShowError(PromptHelper.D_DELETE_FAIL);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }
        #endregion 删除节点

        #region 修改节点
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!chkEnable.Checked)
                {
                    txtNode.ErrorText = "请先点击启用";
                    return;
                }
                if (sender is SimpleButton btn)
                {
                    string name = txtNode.Text.Trim();
                    if (btn.Tag is DevelopType type)
                    {
                        if (type.Name == name) return;
                        if (IsEmptyOrNullOrWhiteSpace(name))
                        {
                            this.txtNode.ErrorText = "名称不能为空";
                            this.txtNode.Focus();
                            return;
                        }
                        //var type1 = await UnityDevelopTypeFacade.GetDevelopTypeById(type.Id);
                        type.Name = name;
                        type.UpdatedTime = DateTime.Now;
                        await UnityDevelopTypeFacade.UpdateEntity(type);
                        await LoadDevelopType();
                        FocusedNode(type.Name);
                        txtNode.Text = string.Empty;
                        this.txtNode.Properties.ReadOnly = true;
                        this.chkEnable.Checked = false;
                        MsgHelper.ShowInfo(PromptHelper.D_UPDATE_SUCCESS);
                        actionState = DevelopActiveState.Normal;
                    }
                }
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }

        #endregion 修改节点

        #region 启动修改功能

        private void chkEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnable.Checked)
            {
                this.txtNode.Properties.ReadOnly = false;
                this.txtNode.Focus();
                this.txtNode.SelectAll();
                actionState = DevelopActiveState.Updating;
            }
            else
            {
                this.txtNode.Properties.ReadOnly = true;
                actionState = DevelopActiveState.Normal;
            }
        }

        #endregion 启动修改功能

        #region 选中节点
        /// <summary>
        /// 选中节点
        /// </summary>
        /// <param name="nameValue">为新增的版块名称</param>
        private void FocusedNode(string nameValue)
        {
            TreeListNode tln = tlDevelopList.FindNodeByFieldValue("Name", nameValue);
            if (tln == null) return;
            tln.Selected = true;
            tln.Checked = true;
            tln.ExpandAll();
        }
        #endregion
    }
}