using log4net;
using System;
using System.IO;

namespace Common
{
    /// <summary>
    /// 异常捕获帮助类
    /// </summary>
    public sealed class ExceptionHelper
    {
        private static ILog Logger => LogManager.GetLogger("DevelopUserFrm");
        /// <summary>
        /// 捕获异常 参数可为方法名称或方法体
        /// 捕获异常并抛出
        /// </summary>
        /// <param name="action">方法名称或方法体</param>
        /// <param name="catchAction">catch 块执行的方法</param>
        /// <param name="finallyAction">finally 块执行的方法</param>
        public static void CatchException(Action action, Action<Exception> catchAction = null, Action finallyAction = null)
        {
            try
            {
                action();
            }
            //catch (DbUpdateConcurrencyException ex) 
            //{
            //    ex.Entries.Single().Reload();
            //}
            catch (Exception ex)
            {
                catchAction?.Invoke(ex);
                LoggerHelper.WriteException(ex);
                throw ex;
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }

        /// <summary>
        /// UI层捕获异常 参数可为方法名称或方法体
        /// 写入日志并在 catchAction 中进行Msg.Show显示异常
        /// </summary>
        /// <param name="action">方法名称或方法体</param>
        /// <param name="catchAction">catch 块执行的方法</param>
        /// <param name="finallyAction">finally 块执行的方法</param>
        public static void CatchUIException(Action action, Action<Exception> catchAction = null, Action finallyAction = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                MsgHelper.ShowError(ex.Message);
                catchAction?.Invoke(ex);
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }

        /// <summary>
        /// 取得当前源码的出错行
        /// </summary>
        private static int GetExceptionLineNum()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            return st.GetFrame(0).GetFileLineNumber();
        }

        /// <summary>
        /// 异常方法名
        /// </summary>
        private static string GetExceptionMethodName()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            return st.GetFrame(0).GetMethod().Name;
        }

        /// <summary>
        /// 取当前源码的源文件名
        /// </summary>
        private static string GetExceptionSourceFileName()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            return st.GetFrame(0).GetFileName();
        }
    }
}