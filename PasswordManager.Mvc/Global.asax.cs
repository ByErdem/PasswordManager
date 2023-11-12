using Autofac.Integration.Mvc;
using Autofac;
using AutoMapper;
using PasswordManager.Services.Abstract;
using PasswordManager.Services.Concrete;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PasswordManager.Mvc.Models.Profiles;
using System.Data.Entity;

namespace PasswordManager.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Mapper.Initialize(opt => opt.AddProfile<MappingProfile>());

            var builder = new ContainerBuilder();

            builder.RegisterInstance(ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["redisConnectionString"]))
                   .As<IConnectionMultiplexer>()
                   .SingleInstance();

            builder.Register(c => c.Resolve<ConnectionMultiplexer>().GetDatabase())
                   .As<IDatabase>();



            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            var mapper = config.CreateMapper();
            builder.RegisterInstance(mapper).As<IMapper>().SingleInstance();
            builder.RegisterType<ConfigurationService>().As<IConfigurationService>().AsSelf().SingleInstance();
            builder.RegisterType<RabbitMQManager>().As<IRabbitMQService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<EncryptionManager>().As<IEncryptionService>().AsSelf().SingleInstance();
            builder.RegisterType<UserManager>().As<IUserService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CategoryManager>().As<ICategoryService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<DashboardManager>().As<IDashboardService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<TokenManager>().As<ITokenService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<RedisCacheManager>().As<IRedisCacheService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<DbContextEntity>().As<IDbContextEntity>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<HttpManager>().As<IHttpService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<MyPasswordsManager>().As<IMyPasswordsService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<SessionManager>().As<ISessionService>().SingleInstance();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                   .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic)
                   .As<Profile>();

            builder.Register(c => new MapperConfiguration(cfg =>
            {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve)).As<IMapper>().InstancePerLifetimeScope();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            var redisService = DependencyResolver.Current.GetService<IRedisCacheService>();
            var tokenService = DependencyResolver.Current.GetService<ITokenService>();
            var sessionService = DependencyResolver.Current.GetService<ISessionService>();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters, redisService, tokenService, sessionService);
        }
    }
}
