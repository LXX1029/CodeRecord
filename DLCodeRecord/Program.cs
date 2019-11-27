using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Common;
using DLCodeRecord.DevelopForms;
using Services.EFCodeFirst;

namespace DLCodeRecord
{
    internal class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            //using (RecordContext recordContext = new RecordContext())
            //{
            //    if (!recordContext.Database.Exists())
            //    {
            //        recordContext.Database.Create();
            //    }
            //    recordContext.Database.Initialize(true);
            //}
            log4net.Config.XmlConfigurator.Configure();

            // 创建新的地方化culture
            // zh-Hans 文件夹在Debug或者Release文件夹下
            CultureInfo culture = CultureInfo.CreateSpecificCulture("zh-Hans");
            // 地方化当前用户界面显示
            Thread.CurrentThread.CurrentUICulture = culture;
            // 地方化当前的数据格式，如数字，日期等等
            Thread.CurrentThread.CurrentCulture = culture;

            //获取欲启动进程名
            string strProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            //检查进程是否已经启动，已经启动则显示报错信息退出程序。
            if (System.Diagnostics.Process.GetProcessesByName(strProcessName).Length > 1)
            {
                MsgHelper.ShowError("程序已运行");
                UtilityHelper.RaiseOtherProcess();
                Application.Exit();
                return;
            }

            // 启用Dev 皮肤
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.UseDefaultLookAndFeel = false;
            DevExpress.LookAndFeel.UserLookAndFeel.Default.UseWindowsXPTheme = false;
            string skinName = ConfigurationManager.AppSettings["skin"];
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = skinName;

            Application.ThreadException -= new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.ApplicationExit -= Application_ApplicationExit;
            Application.ApplicationExit += Application_ApplicationExit;
            Application.Run(new DevelopLoginFrm());
            //Application.Run(new DevelopFrm());
            //Application.Run(new DevelopUserFrm());
            //Application.Run(new DevelopReportFrm());
            //Application.Run(new DevelopTypeAddFrm());
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            GC.Collect();
        }

        #region 捕获线程异常

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MsgHelper.CloseSplashScreenForm();
            MsgHelper.ShowError("程序遇到不可修复的异常被迫关闭，参见日志文件修复错误!");
            LoggerHelper.WriteException(e.Exception);
            Environment.Exit(Environment.ExitCode);
            Application.Exit();
        }

        #endregion 捕获线程异常
    }
}