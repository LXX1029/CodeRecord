using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Interception.PolicyInjection.Pipeline;
using Unity.Interception.PolicyInjection.Policies;

namespace Services.Unity.UnityControl
{
    /// <summary>
    /// 异常特性
    /// </summary>
    public class UnityExceptionAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new ExceptionCallHandler();
        }
    }

    /// <summary>
    /// 操作日志特性
    /// </summary>
    public class OperatingLogAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new OperatingLogCallHandler();
        }
    }
}
