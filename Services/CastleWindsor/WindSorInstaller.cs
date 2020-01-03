using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Services.CastleWindsor
{
    public class WindSorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<TypedFactoryFacility>();
            container.Register(Component.For<IRepository, IUserService>().ImplementedBy<DevelopUserService>(),
                Component.For<IRepository, IStatisticsService>().ImplementedBy<DevelopStatisticsService>(),
                Component.For<IRepository, IDevelopRecordService>().ImplementedBy<DevelopRecordService>().Named("DevelopRecordService"),
                Component.For<IServiceFactory>().AsFactory());

            // 泛型注册
            // container.Register(Component.For(typeof(IRepository<>)).ImplementedBy(typeof(Repository<>)));

        }
    }
    public interface IServiceFactory
    {
        IRepository Create<T>() where T : IRepository;

        IRepository GetDevelopRecordService();
    }
}
