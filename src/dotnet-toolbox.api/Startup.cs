using Autofac;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Autofac.Extensions.DependencyInjection;
using System;
using Microsoft.AspNet.StaticFiles;
using StackExchange.Redis;
using System.IO;
using dotnet_toolbox.api.Env;
using dotnet_toolbox.api.Nuget;

namespace dotnet_toolbox.api
{
    public class Startup
    {
        private const string JavaScriptExtension = ".js";
        private const int PACKAGES_DB = 1;

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var builder = new ContainerBuilder();
            builder.Register(_ => EnvironmentReader.FromEnvironment());
            builder.Register(BuildRabbitConnectionFactory).SingleInstance();
            builder.Register(BuildRabbitConnection).InstancePerLifetimeScope();
            builder.Register(BuildRabbitModel).InstancePerLifetimeScope();
            builder.RegisterType<Nuget.NugetApi>().As<Nuget.INugetApi>().InstancePerLifetimeScope();
            builder.RegisterType<PackageCrawlerJobQueue>().As<IPackageCrawlerJobQueue>();
            builder.Register(BuildConnectionMultiplexer).As<ConnectionMultiplexer>().SingleInstance();
            builder.Register(componentContext => componentContext.Resolve<ConnectionMultiplexer>().GetDatabase(PACKAGES_DB)).As<IDatabase>().InstancePerLifetimeScope();
            builder.Populate(services);
            var container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }

        private RabbitMQ.Client.IModel BuildRabbitModel(IComponentContext context)
        {
            return context.Resolve<RabbitMQ.Client.IConnection>()
                .CreateModel();
        }

        private static RabbitMQ.Client.IConnection BuildRabbitConnection(IComponentContext context)
        {
            return context.Resolve<RabbitMQ.Client.ConnectionFactory>()
                            .CreateConnection();
        }

        private RabbitMQ.Client.ConnectionFactory BuildRabbitConnectionFactory(IComponentContext context)
        {
            var environment = context.Resolve<EnvironmentReader>();
            var connectionFactory = new RabbitMQ.Client.ConnectionFactory();
            connectionFactory.Uri = environment.RabbitMQConnectionString;
            connectionFactory.SocketFactory = (addressFamily) =>
                     new System.Net.Sockets.TcpClient(System.Net.Sockets.AddressFamily.InterNetwork);
            return connectionFactory;
        }

        private static ConnectionMultiplexer BuildConnectionMultiplexer(IComponentContext context)
        {
            var environment = context.Resolve<EnvironmentReader>();
            return ConnectionMultiplexer.Connect(environment.RedisConnectionString);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseDefaultFiles();
            var provider = new FileExtensionContentTypeProvider();

            ForceUtf8EncodingForJavaScript(provider);

            // Serve static files.
            app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = provider });

            app.UseMvc();
        }

        private static void ForceUtf8EncodingForJavaScript(FileExtensionContentTypeProvider provider)
        {
            if (provider.Mappings.ContainsKey(JavaScriptExtension))
            {
                provider.Mappings.Remove(JavaScriptExtension);
            }
            provider.Mappings.Add(JavaScriptExtension, "application/javascript; charset=utf-8");
        }


        // Entry point for the application.
        public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
    }
}
