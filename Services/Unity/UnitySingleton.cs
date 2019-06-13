using Unity;
using Unity.Interception;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;

namespace Services.Unity
{
    /// <summary>
    /// Unity容器类
    /// </summary>
    public static class UnitySingleton
    {
        static UnitySingleton()
        {
            // 注册接口实现
            UnityContainer = new UnityContainer().AddNewExtension<Interception>()
                .RegisterType<IUnityUserFacade, UnityUserFacade>()
                .RegisterType<IUnityDevelopPowerFunFacade, UnityDevelopPowerFunFacade>()
                .RegisterType<IUnityDevelopRecordFacade, UnityDevelopRecordFacade>()
                .RegisterType<IUnityDevelopTypeFacade, UnityDevelopTypeFacade>()
                .RegisterType<IUnityStatisticsFacade, UnityStatisticsFacade>()
                .RegisterType<IUnityDevelopFunFacade, UnityDevelopFunFacade>();

            // 拦截接口
            UnityContainer.Configure<Interception>().SetInterceptorFor<IUnityUserFacade>(new InterfaceInterceptor());
            UnityContainer.Configure<Interception>().SetInterceptorFor<IUnityDevelopPowerFunFacade>(new InterfaceInterceptor());
            UnityContainer.Configure<Interception>().SetInterceptorFor<IUnityDevelopRecordFacade>(new InterfaceInterceptor());
            UnityContainer.Configure<Interception>().SetInterceptorFor<IUnityDevelopTypeFacade>(new InterfaceInterceptor());
            UnityContainer.Configure<Interception>().SetInterceptorFor<IUnityStatisticsFacade>(new InterfaceInterceptor());
            UnityContainer.Configure<Interception>().SetInterceptorFor<IUnityDevelopFunFacade>(new InterfaceInterceptor());
        }
        /// <summary>
        /// Unity容器
        /// </summary>
        private static IUnityContainer UnityContainer { get; set; }
        #region 数据获取接口

        /// <summary>
        /// 获取数据访问接口
        /// </summary>
        /// <typeparam name="T">数据接口类名称</typeparam>
        /// <param name="registerName">实现类继承自同一个接口时进行RegisterType命名的别名</param>
        /// <returns>数据接口</returns>
        public static T GetUnityFacade<T>(string registerName = "") where T : class
        {
            return UnityContainer.Resolve<T>(registerName);
        }
        #endregion


        #region 数据接口层
        public static IUnityDevelopFunFacade UnityDevelopFunFacade => GetUnityFacade<IUnityDevelopFunFacade>();

        public static IUnityDevelopPowerFunFacade UnityDevelopPowerFunFacade => GetUnityFacade<IUnityDevelopPowerFunFacade>();

        public static IUnityDevelopRecordFacade UnityDevelopRecordFacade => GetUnityFacade<IUnityDevelopRecordFacade>();

        public static IUnityDevelopTypeFacade UnityDevelopTypeFacade => GetUnityFacade<IUnityDevelopTypeFacade>();

        public static IUnityStatisticsFacade UnityStatisticsFacade => GetUnityFacade<IUnityStatisticsFacade>();

        public static IUnityUserFacade UnityUserFacade => GetUnityFacade<IUnityUserFacade>();
        #endregion

    }
}
