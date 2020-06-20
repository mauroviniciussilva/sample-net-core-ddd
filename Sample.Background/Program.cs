using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using Sample.Background.Jobs;
using Sample.Domain.Interfaces;
using Sample.Infra.CrossCutting.IoC;
using Sample.Infra.Logging;
using System;
using Topshelf;

namespace Sample.Background
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sample.Background!");

            IConfiguration configuration = new ConfigurationBuilder()
                                    .SetBasePath(AppContext.BaseDirectory)
                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                    .AddEnvironmentVariables()
                                    .Build();

            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<IUserHelper>(us =>
            {
                return new UserHelperNull();
            });

            services.AddSingleton<IJobFactory>(pr =>
            {
                var jobFactory = new JobFactory(pr);
                return jobFactory;
            });

            services.AddSingleton<SampleJob>();

            CoreIoC.Register(services, () => new DependencyInjectionConfig
            {
                Configuration = configuration,
                Template = LoggingLayoutTemplate.WebApiLogTemplate
            });

            var serviceProvider = services.BuildServiceProvider();

            HostFactory.Run(configurator =>
            {
                configurator.SetServiceName("Sample.Background");
                configurator.SetDisplayName("Sample.Background");
                configurator.SetDescription("Servico de processamento de tarefas em background do Sample");

                configurator.RunAsLocalSystem();

                configurator.Service<SampleService>(serviceConfigurator =>
                {
                    var jobFactory = serviceProvider.GetRequiredService<IJobFactory>();

                    serviceConfigurator.ConstructUsing(() => new SampleService(jobFactory));

                    serviceConfigurator.WhenStarted((service, hostControl) =>
                    {
                        service.OnStart();
                        return true;
                    });
                    serviceConfigurator.WhenStopped((service, hostControl) =>
                    {
                        service.OnStop();
                        return true;
                    });
                });
            });
        }
    }
}