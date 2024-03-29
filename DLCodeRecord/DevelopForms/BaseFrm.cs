﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Common;
using DevExpress.XtraEditors;
using log4net;
using static Common.ExceptionHelper;
using static Common.UtilityHelper;
namespace DLCodeRecord.DevelopForms
{
    /// <summary>
    /// 基础窗口
    /// 提供一些基本设置
    /// </summary>
    public partial class BaseFrm : XtraForm
    {
        #region Protected Fields
        /// <summary>
        /// 默认窗口活动状态为Normal
        /// </summary>
        protected DevelopActiveState actionState = DevelopActiveState.Normal;
        protected static ILog Logger => LogManager.GetLogger("BaseFrm");
        #endregion Protected Fields

        #region Public Constructors

        public BaseFrm()
        {
            InitializeComponent();
            try
            {
                this.MaximizeBox = true;
                this.MinimizeBox = true;
                this.LookAndFeel.UseDefaultLookAndFeel = true;
                this.Size = UtilityHelper.GetWorkingAreaSize().Item5;
                string iconPath = AppLaunchPath + "DevelopForms\\Bug.ico";
                if (System.IO.File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath, new Size(48, 48));
                    this.ShowIcon = true;
                }
                // 禁止改变窗体大小
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
            }
            catch (Exception ex)
            {
                MsgHelper.ShowError(ex.Message);
            }
        }

        #endregion Public Constructors

        #region 窗口即将关闭时提示
        /// <summary>
        /// 窗口即将关闭时提示
        /// 派生类窗口在FormClosing事件中使用
        /// </summary>
        /// <param name="e">FormClosingEventArgs</param>
        /// <param name="actionOk">确定关闭之后的操作</param>
        /// <param name="actionCancel">取消关闭之后的操作</param>
        protected void FormClosingTip(FormClosingEventArgs e, Action actionOk = null, Action actionCancel = null)
        {
            // 判断窗体数据状态
            if (actionState != DevelopActiveState.Normal)
            {
                if (MsgHelper.ShowConfirm() == DialogResult.Cancel)
                {
                    actionCancel?.Invoke();
                    e.Cancel = true;
                    return;
                }
                // 确定关闭设置操作状态为Normal
                actionState = DevelopActiveState.Normal;
                actionOk?.Invoke();
            }
        }
        #endregion

        #region 窗口正在打开时进度提示
        /// <summary>
        /// 窗口即正在打开时进度提示
        /// </summary>
        /// <param name="form">显示进度提示条的窗体</param>
        /// <param name="action">在窗体加载中执行的委托方法</param>
        /// <param name="content">提示进度条的内容</param>
        protected void FormLoadingTipAsync(XtraForm form, Action action, string content = "正在生成窗口")
        {
            MsgHelper.ShowSplashScreenForm(this, content);
            CatchUIException(
                () =>
            {
                ExecuteAction(action, form);
            }, (ex) =>
            {
            }, () =>
            {
                BeginInvoke(new Action(() =>
                {
                    MsgHelper.CloseSplashScreenForm();
                }));
            });
        }

        /// <summary>
        /// 显示进度提示
        /// </summary>
        protected void ShowSplashScreenForm(string content = "")
        {
            MsgHelper.ShowSplashScreenForm(this, content);
        }
        /// <summary>
        /// 关闭进度提示
        /// </summary>
        protected void CloseSplashScreenForm()
        {
            MsgHelper.CloseSplashScreenForm();
        }
        /// <summary>
        /// 窗口加载捕获异常
        /// </summary>
        /// <param name="form">出现异常的窗体</param>
        protected void CatchLoadException(XtraForm form, Exception ex)
        {
            form.Hide();
            MsgHelper.CloseSplashScreenForm();
            MsgHelper.ShowError($"加载错误");
            Logger.Error(form.GetType().FullName, ex);
            form.Close();
        }
        /// <summary>
        /// 捕获异常
        /// </summary>
        /// <param name="ex">ex</param>
        protected void CatchException(Exception ex)
        {
            if (actionState != DevelopActiveState.Normal)
                actionState = DevelopActiveState.Normal;
            Logger.Error("BaseFrm", ex);
            if (ex.GetType() != typeof(OperationCanceledException))
            {
                MsgHelper.ShowError("操作出错");
            }
        }

        /// <summary>
        /// 执行一个委托方法
        /// </summary>
        /// <param name="action">匿名委托方法</param>
        /// <param name="form">执行Action的窗体</param>
        /// <returns>bool</returns>
        protected bool ExecuteAction(Action action, XtraForm form = null)
        {
            var result = false;
            CatchUIException(() =>
            {
                Invoke(new Action(() => { action?.Invoke(); }));
                result = true;
            }, (ex) =>
            {
                if (form != null)
                {
                    form.BeginInvoke(new Action(() =>
                    {
                        form.Hide();
                        MsgHelper.CloseSplashScreenForm();
                        form.Close();
                    }));
                }
            });
            return result;
        }
        #endregion

        #region  自定义事件
        protected void AttatchedEvent(EventHandler origionEvent, EventHandler targetEvent)
        {
            if (targetEvent == null) return;
            if (origionEvent.GetType() != targetEvent.GetType()) return;
            origionEvent -= targetEvent;
            origionEvent += targetEvent;
        }
        #endregion
    }
}