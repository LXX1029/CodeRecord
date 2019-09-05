using Common;
using DevExpress.XtraEditors;
using DataEntitys;
using System;
using System.Drawing;
using System.Windows.Forms;
using static Services.Unity.UnityContainerManager;
namespace DLCodeRecord.DevelopForms
{
    /// <summary>
    /// 登陆窗体类
    /// </summary>
    public partial class DevelopLoginFrm : BaseFrm
    {
        #region 构造函数
        /// <summary>
        /// 静态函数
        /// 验证参数配置
        /// </summary>
        static DevelopLoginFrm()
        {
            string readCount = UtilityHelper.GetConfigurationKeyValue("perReadCount");
            if (!int.TryParse(readCount, out int stepCount))
            {
                MsgHelper.ShowError("配置文件存在错误,请修复后重新启动程序。");
                LoggerHelper.WriteException("加载量参数格式配置错误");
                System.Environment.Exit(System.Environment.ExitCode);
                Application.Exit();
            }
        }


        public DevelopLoginFrm()
        {
            InitializeComponent();

            #region 汉化

            // 汉化
            //GridLocalizer.Active = new GridControlLocalizer();
            //Localizer.Active = new EditorsLocalizer();
            //BarControlLocalizer.Active = new BarControlLocalizer();
            // RichEditControlLocalizer.Active = new RichEditControlLocalizer();

            #endregion 汉化

            #region 初始化设置
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "登陆";
            CancelButton = this.btnExit;
            AcceptButton = this.btnLogin;
            this.txtPwd.Properties.CharacterCasing = CharacterCasing.Lower;
            this.txtPwd.Properties.PasswordChar = '*';
            this.txtPwd.TextChanged -= new EventHandler(txtPwd_TextChanged);
            this.txtName.TextChanged -= new EventHandler(txtPwd_TextChanged);
            this.btnExit.Click -= new EventHandler(btnExit_Click);
            this.Load -= new EventHandler(DevelopLoginFrm_Load);
            this.txtPwd.TextChanged += new EventHandler(txtPwd_TextChanged);
            this.txtName.TextChanged += new EventHandler(txtPwd_TextChanged);
            this.btnExit.Click += new EventHandler(btnExit_Click);
            this.Load += new EventHandler(DevelopLoginFrm_Load);

            #endregion 初始化设置
        }

        #endregion 构造函数

        #region 密码输入改变事件

        /// <summary>
        /// 密码输入项改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPwd_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextEdit te && te?.Name == "txtPwd")
            {
                if (this.chkRemeberPwd.Checked)
                {
                    UtilityHelper.SetConfigurationKeyValue("pwd", "");
                    this.chkRemeberPwd.Checked = false;
                }
            }
            else
            {
                txtPwd.ResetText();
                chkRemeberPwd.Checked = false;
            }
            lcMessage.ResetText();
        }

        #endregion 密码输入改变事件

        #region 窗体加载

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void DevelopLoginFrm_Load(object sender, EventArgs e)
        {
            var tuple = UtilityHelper.GetWorkingAreaSize(0.2, 0.2);
            this.Size = tuple.Item3;
            this.Location = tuple.Item4;
            this.txtName.Properties.MaxLength = 15;
            this.txtName.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
            this.txtPwd.Properties.MaxLength = 50;
            this.txtPwd.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;

            string name = UtilityHelper.GetConfigurationKeyValue("userName");
            this.txtName.Text = name;

            if (name == "")
                this.txtName.Text = "admin";
            string pwd = UtilityHelper.GetConfigurationKeyValue("pwd");

            if (!VerifyHelper.IsEmptyOrNullOrWhiteSpace(pwd))
            {
                // 根据md5Key 解密
                this.txtPwd.Text = pwd;
                this.btnLogin.Focus();
                this.chkRemeberPwd.Checked = true;
            }
            // 选中密码
            if (!VerifyHelper.IsEmptyOrNullOrWhiteSpace(name))
            {
                this.txtName.TabStop = false;
                this.txtPwd.Focus();
            }
            if (VerifyHelper.IsEmptyOrNullOrWhiteSpace(name) && VerifyHelper.IsEmptyOrNullOrWhiteSpace(pwd))
                txtName.Focus();
        }

        #endregion 窗体加载

        #region 退出

        /// <summary>
        /// 退出
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion 退出

        #region 登陆事件

        /// <summary>
        /// 主窗口
        /// </summary>
        private DevelopManageFrm manageFrm;

        /// <summary>
        /// 登陆
        /// </summary>
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtName.Text.Trim();
                if (VerifyHelper.IsEmptyOrNullOrWhiteSpace(name))
                {
                    txtName.ErrorText = "用户名不能为空";
                    txtName.Focus();
                    return;
                }

                string pwd = txtPwd.Text.Trim();
                if (VerifyHelper.IsEmptyOrNullOrWhiteSpace(pwd))
                {
                    txtPwd.ErrorText = "密码不能为空";
                    txtPwd.Focus();
                    return;
                }
                else
                {
                    string pwdConfig = UtilityHelper.GetConfigurationKeyValue("pwd");
                    if (VerifyHelper.IsEmptyOrNullOrWhiteSpace(pwdConfig))
                    {
                        pwd = UtilityHelper.MD5Encrypt(pwd);
                    }
                }
                DevelopUser user = await UnityUserFacade.GetDevelopUser(name, pwd);
                if (user == null)
                {
                    lcMessage.Text = "用户名或密码错误";
                    txtName.Focus();
                    txtName.SelectAll();
                    return;
                }
                DataManage.LoginUser = user;
                // 保存用户名
                UtilityHelper.SetConfigurationKeyValue("userName", name);
                //记住密码
                if (chkRemeberPwd.Checked)
                    UtilityHelper.SetConfigurationKeyValue("pwd", pwd);
                else
                    UtilityHelper.SetConfigurationKeyValue("pwd", "");
                UtilityHelper.SetConfigurationKeyValue("userId", user.Id.ToString());
                // 显示主窗口
                manageFrm = new DevelopManageFrm();
                manageFrm.AttatchedHandler(new Action(() => { this.Hide(); }));
                manageFrm.Show();
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }

        #endregion 登陆事件
    }
}