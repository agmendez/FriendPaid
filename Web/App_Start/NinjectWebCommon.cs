using Repository;
using Repository.Implementations;
using Repository.Interfaces;
using Services;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Web.App_Start.NinjectWebCommon), "Stop")]

namespace Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static IKernel Kernel { get { return bootstrapper.Kernel; } }
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application..
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IHibernateSessionFactory>().To<HibernateSessionFactory>().InSingletonScope();
            kernel.Bind<IUserRepository>().To<UserRepository>().InSingletonScope().WithConstructorArgument("userRepository", kernel.GetService(typeof(IUserRepository)));
            kernel.Bind<IGroupRepository>().To<GroupRepository>().InSingletonScope().WithConstructorArgument("groupRepository", kernel.GetService(typeof(IGroupRepository)));
            kernel.Bind<IUserService>().To<UserService>().InSingletonScope().WithConstructorArgument("userService", kernel.GetService(typeof(IUserService)));
            kernel.Bind<IGroupService>().To<GroupService>().InSingletonScope().WithConstructorArgument("groupService", kernel.GetService(typeof(IGroupService)));
        }        
    }
}
