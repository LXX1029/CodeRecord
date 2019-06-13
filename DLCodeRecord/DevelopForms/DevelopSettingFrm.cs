using Common;
using static Common.ExceptionHelper;
using DataEntitys;
using log4net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLCodeRecord.DevelopForms
{
    /// <summary>
    /// 其它设置窗体类
    /// </summary>
    public partial class DevelopSettingFrm : BaseFrm
    {

        #region 构造函数
        public DevelopSettingFrm()
        {
            InitializeComponent();

            #region 初始化设置
            this.Text = "设置";

            #endregion 初始化设置
            this.Load -= DevelopSettingFrm_Load;
            this.Load += DevelopSettingFrm_Load;
        }
        #endregion

        #region 窗口加载
        private async void DevelopSettingFrm_Load(object sender, EventArgs e)
        {
            var tuple = UtilityHelper.GetWorkingAreaSize(0.6, 0.6);
            this.Size = tuple.Item3;
            this.Location = tuple.Item4;
            ShowSplashScreenForm(this, PromptHelper.D_LOADINGDATA);
            await Task.Delay(1000);
            CloseSplashScreenForm();
            InitialLayOut();
        }
        #endregion 窗口加载

        #region 初始化整体布局方法
        private void InitialLayOut()
        {
            txtPwd.Properties.PasswordChar = '*';
            txtPwd.Properties.MaxLength = 10;
            txtPwd.ErrorIconAlignment = ErrorIconAlignment.TopRight;
            txtNewPwd.Properties.PasswordChar = '*';
            txtNewPwd.Properties.MaxLength = 10;
            txtNewPwd.ErrorIconAlignment = ErrorIconAlignment.TopRight;
        }

        #endregion 初始化整体布局方法

        #region 修改密码事件

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            string pwd = this.txtPwd.Text.Trim();
            if (string.IsNullOrEmpty(pwd))
            {
                txtPwd.ErrorText = "请输入原始密码";
                txtPwd.Focus();
                return;
            }
            try
            {
                string userId = UtilityHelper.GetConfigurationKeyValue("userId");
                if (string.IsNullOrEmpty(userId))
                {
                    MsgHelper.ShowInfo("配置文件不存在");
                    return;
                }
                pwd = UtilityHelper.MD5Encrypt(pwd.Trim());
                DevelopUser user = await base.UnityUserFacade.GetEntity(Convert.ToInt32(userId));
                if (user.Pwd != pwd)
                {
                    txtPwd.ErrorText = "原始密码输入不正确";
                    txtPwd.ResetText();
                    txtPwd.Focus();
                    return;
                }
                string newPwd = this.txtNewPwd.Text.Trim();
                if (string.IsNullOrEmpty(newPwd))
                {
                    txtNewPwd.ErrorText = "请输入新密码";
                    txtNewPwd.Focus();
                    return;
                }
                string md5Pwd = UtilityHelper.MD5Encrypt(newPwd.Trim());
                if (md5Pwd.Length > 20)
                {
                    txtNewPwd.ErrorText = "输入字符串长度小于20";
                    txtNewPwd.Focus();
                    return;
                }
                user.Pwd = md5Pwd;
                await UnityUserFacade.UpdateEntity(user);
                MsgHelper.ShowInfo("修改成功，下次登录有效。");
                txtPwd.ResetText();
                txtNewPwd.ResetText();
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }

        #endregion 修改密码事件

        #region 重置密码

        private void BtnReset_Click(object sender, EventArgs e)
        {
            this.txtPwd.ResetText();
            this.txtPwd.Focus();
            this.txtNewPwd.ResetText();
        }

        #endregion 重置密码
    }
}