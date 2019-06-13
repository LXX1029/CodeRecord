using Common;
using DataEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.Interception.PolicyInjection.Pipeline;
namespace Services.Unity.UnityControl
{
    /// <summary>
    /// 异常调用处理
    /// </summary>
    public class ExceptionCallHandler : ICallHandler
    {
        public int Order { get; set; }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var message = getNext()(input, getNext);
            if (message.Exception != null)
            {
                MethodInfo obj = (MethodInfo)input.MethodBase;
                Type CurrentType = obj.ReturnType;
                if (CurrentType.IsValueType)
                {
                    message.ReturnValue = 0;
                }
                // 记录异常
                Exception exception = message.Exception;
                LoggerHelper.WriteException(exception);
            }
            return message;
        }
    }

    /// <summary>
    /// 操作日志调用处理
    /// 记录
    /// </summary>
    public class OperatingLogCallHandler : ICallHandler
    {
        public int Order { get; set; }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var message = getNext()(input, getNext);
            if (input.Arguments != null)
            {
                MethodInfo info = (MethodInfo)input.MethodBase;
                if (input.Arguments[0].GetType() == typeof(DevelopUser) && info.Name == "AddUser")
                {
                    DevelopUser user = (DevelopUser)input.Arguments[0];
                    string operationMsg = $"用户{UtilityHelper.GetConfigurationKeyValue("userName")}    添加了用户，名称为：{user.Name}";
                    LoggerHelper.WriteOperation(operationMsg);
                }

            }
            return message;
        }
    }
}
