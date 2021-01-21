using BusinessLogic.Accounts;
using BusinessLogic.Interests;
using BusinessLogic.Notifications;
using BusinessLogic.Trips;
using DataLayer.Accounts;
using DataLayer.CompositionRoot;
using DataLayer.Interests;
using DataLayer.Notifications;
using DataLayer.Trips;
using Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ninject;
using System.Net.Http;

namespace CompositionRoot
{
    public class CompositionRootBackend : ICompositionRoot
    {
        private StandardKernel _kernel;
        private static volatile CompositionRootBackend _instance;
        private static object _syncRoot = new object();

        public static CompositionRootBackend Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new CompositionRootBackend();
                        }
                    }
                }

                return _instance;
            }
        }

        public T GetImplementation<T>(string instanceName = null)
        {
            return _kernel.Get<T>(instanceName);
        }

        public T GetImplementation<T>()
        {
            return _kernel.Get<T>();
        }

        public CompositionRootBackend()
        {
            _kernel = new StandardKernel();

            #region Memory cache

            _kernel.Bind<IOptions<MemoryCacheOptions>>().To<MemoryCacheOptions>();
            _kernel.Bind<IMemoryCache>().To<MemoryCache>()
                .InSingletonScope()
                .WithConstructorArgument(context => context.Kernel.Get<IOptions<MemoryCacheOptions>>());

            #endregion Memory cache

            #region Http Context

            _kernel.Bind<IHttpContextAccessor>().To<HttpContextAccessor>().InSingletonScope();

            #endregion Http Context

            #region HttpClientFactory

            _kernel.Bind<ServiceProvider>().ToMethod(context =>
            {
                var serviceCollection = new ServiceCollection();

                serviceCollection.AddHttpClient();

                return serviceCollection.BuildServiceProvider();
            }).InSingletonScope();

            _kernel.Bind<IHttpClientFactory>().ToMethod(context =>
            {
                var serviceProvider = context.Kernel.Get<ServiceProvider>();
                var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                return httpClientFactory;
            });

            #endregion HttpClientFactory

            #region Accounts

            _kernel.Bind<IUserRepository>().To<UserRepository>();

            _kernel.Bind<UserWorker>().To<UserWorker>()
                .WithConstructorArgument(context => context.Kernel.Get<IUserRepository>());

            #endregion Accounts

            #region Interests

            _kernel.Bind<IUserInterestRepository>().To<UserInterestRepository>();

            _kernel.Bind<IInterestsRepository>().To<InterestsRepository>()
                .WithConstructorArgument(context => context.Kernel.Get<IUserInterestRepository>());

            _kernel.Bind<InterestsWorker>().To<InterestsWorker>()
                .WithConstructorArgument(context => context.Kernel.Get<IInterestsRepository>());

            _kernel.Bind<UserInterestWorker>().To<UserInterestWorker>()
                .WithConstructorArgument(context => context.Kernel.Get<IUserInterestRepository>());

            #endregion Interests

            #region Trips

            _kernel.Bind<ITripMessagesRepository>().To<TripMessagesRepository>();
            _kernel.Bind<ITripsRepository>().To<TripsRepository>();
            _kernel.Bind<ITripsUsersRepository>().To<TripsUsersRepository>();

            _kernel.Bind<TripMessagesWorker>().To<TripMessagesWorker>()
                .WithConstructorArgument(context => context.Kernel.Get<ITripMessagesRepository>());

            _kernel.Bind<TripsWorker>().To<TripsWorker>()
                .WithConstructorArgument(context => context.Kernel.Get<IUserRepository>())
                .WithConstructorArgument(context => context.Kernel.Get<ITripsRepository>())
                .WithConstructorArgument(context => context.Kernel.Get<ITripsUsersRepository>())
                .WithConstructorArgument(context => context.Kernel.Get<IUserInterestRepository>())
                .WithConstructorArgument(context => context.Kernel.Get<INotificationsRepository>());

            _kernel.Bind<TripsUsersWorker>().To<TripsUsersWorker>()
                .WithConstructorArgument(context => context.Kernel.Get<ITripsRepository>())
                .WithConstructorArgument(context => context.Kernel.Get<ITripsUsersRepository>());

            #endregion Trips

            #region Friends

            _kernel.Bind<IFriendsRepository>().To<FriendsRepository>();

            _kernel.Bind<FriendsWorker>().To<FriendsWorker>()
                .WithConstructorArgument(context => context.Kernel.Get<IFriendsRepository>());

            #endregion Friends

            #region Notifications

            _kernel.Bind<INotificationsRepository>().To<NotificationsRepository>();

            _kernel.Bind<NotificationsWorker>().To<NotificationsWorker>()
                .WithConstructorArgument(context => context.Kernel.Get<INotificationsRepository>())
                .WithConstructorArgument(context => context.Kernel.Get<ITripsUsersRepository>())
                .WithConstructorArgument(context => context.Kernel.Get<FriendsWorker>());

            #endregion Notifications
        }
    }
}