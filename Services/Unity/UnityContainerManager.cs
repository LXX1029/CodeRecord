using Unity;

namespace Services.Unity
{
    /// <summary>
    /// Unity容器类
    /// </summary>
    public static class UnityContainerManager
    {
        static UnityContainerManager()
        {
            // 注册接口实现
            UnityContainer = new UnityContainer()
                .RegisterType<IUnityUserFacade, UnityUserFacade>()
                .RegisterType<IUnityDevelopPowerFunFacade, UnityDevelopPowerFunFacade>()
                .RegisterType<IUnityDevelopRecordFacade, UnityDevelopRecordFacade>()
                .RegisterType<IUnityDevelopTypeFacade, UnityDevelopTypeFacade>()
                .RegisterType<IUnityStatisticsFacade, UnityStatisticsFacade>()
                .RegisterType<IUnityDevelopFunFacade, UnityDevelopFunFacade>();

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
        public static T GetUnityFacade<T>(string registerName = "")
            where T : class
        {
            return UnityContainer.Resolve<T>(registerName);
        }
        #endregion

        #region 数据接口层
        public static IUnityDevelopFunFacade UnityDevelopFunFacade => GetUnityFacade<UnityDevelopFunFacade>();

        public static IUnityDevelopPowerFunFacade UnityDevelopPowerFunFacade => GetUnityFacade<UnityDevelopPowerFunFacade>();

        public static IUnityDevelopRecordFacade UnityDevelopRecordFacade => GetUnityFacade<UnityDevelopRecordFacade>();

        public static IUnityDevelopTypeFacade UnityDevelopTypeFacade => GetUnityFacade<UnityDevelopTypeFacade>();

        public static IUnityStatisticsFacade UnityStatisticsFacade => GetUnityFacade<UnityStatisticsFacade>();

        public static IUnityUserFacade UnityUserFacade => GetUnityFacade<UnityUserFacade>();

        #endregion

    }
}
