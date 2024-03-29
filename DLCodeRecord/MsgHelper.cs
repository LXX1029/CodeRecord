﻿using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DLCodeRecord.DevelopForms;
using System.Windows.Forms;

namespace DLCodeRecord
{
    /// <summary>
    /// 信息提示帮助类
    /// </summary>
    public static class MsgHelper
    {
        /// <summary>
        /// 警告提示信息，包含一个OK，Cancel按钮
        /// </summary>
        /// <param name="text">提示信息,默认为"处于编辑状态,确定要关闭？"</param>
        /// <returns>DialogResult:OK/Cancel</returns>
        public static DialogResult ShowConfirm(string text = "数据处于编辑状态,确定关闭？")
        {
            return XtraMessageBox.Show(text, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 错误提示信息，包含一个Ok按钮
        /// </summary>
        /// <param name="text">提示信息</param>
        /// <returns>DialogResult:OK</returns>
        public static DialogResult ShowError(string text)
        {
            return XtraMessageBox.Show(text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 一般提示信息，包含一个Ok按钮
        /// </summary>
        /// <param name="text">提示信息</param>
        /// <returns>DialogResult:OK</returns>
        public static DialogResult ShowInfo(string text)
        {
            return XtraMessageBox.Show(text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示SplashScreen进度窗体
        /// </summary>
        /// <param name="mainForm">拥有进度窗口的主窗口</param>
        /// <param name="content">显示的文字内容,默认：正在生成窗口</param>
        public static void ShowSplashScreenForm(XtraForm mainForm, string content = "正在生成窗口")
        {
            SplashScreenManager.ShowForm(mainForm, typeof(SplashScreenFrm), false, false, false);
        }

        /// <summary>
        /// 关闭SplashScreen进度窗体
        /// </summary>
        public static void CloseSplashScreenForm()
        {
            SplashScreenManager.CloseForm(false);
        }

        /// <summary>
        /// 显示WaitForm进度窗体
        /// </summary>
        /// <param name="mainForm">拥有进度窗口的主窗口</param>
        /// <param name="content">显示的文字内容,默认：正在进行后台操作</param>
        public static void ShowWaitingForm(XtraForm mainForm, string content = "正在进行后台操作")
        {
            SplashScreenManager.ShowForm(mainForm, typeof(WaitForm), false, false, false);
            SplashScreenManager.Default.SetWaitFormCaption(content);
            //SplashScreenManager.Default.SetWaitFormDescription();
        }
    }
}