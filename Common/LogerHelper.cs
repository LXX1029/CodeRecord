using System;
using System.IO;

namespace Common
{
    /// <summary>
    /// 日志记录帮助类
    /// </summary>
    public sealed class LoggerHelper
    {
        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="exception">异常信息</param>
        public static void WriteException(string exception)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "log.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine($"*****************************{DateTime.Now}*****************************");
                writer.WriteLine(exception);
            }
        }

        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="exception">异常信息</param>
        public static void WriteException(Exception exception)
        {
            string path = UtilityHelper.AppLaunchPath + "log.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine($"*****************************{DateTime.Now}*****************************");
                writer.WriteLine($"异常程序集： {exception?.Source}");
                writer.WriteLine($"异常类名称： {exception?.TargetSite.DeclaringType}");
                writer.WriteLine($"异常对象类型： {exception?.TargetSite.MemberType}  异常对象名称： {exception?.TargetSite}");
                writer.WriteLine($"异常:    {exception?.Message}");
                writer.WriteLine($"内部异常:{exception?.InnerException?.Message}");
                writer.WriteLine($"栈跟踪信息:{exception.StackTrace}");
            }
        }

        /// <summary>
        /// 操作日志记录
        /// </summary>
        /// <param name="operation">操作记录</param>
        public static void WriteOperation(string operation)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "peration.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format("--------------------------------------{0}------------------------------", DateTime.Now));
                writer.WriteLine(operation);
            }
        }
    }
}