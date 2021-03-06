﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Sample.Domain.Interfaces.Repositories;
using Sample.Domain.Interfaces.Services;
using Sample.Service;
using Sample.Infra.Data.Context;
using Sample.Infra.Data.Interceptors;
using Sample.Infra.Data.Repositories;
using Sample.Infra.Data.Repository;
using Sample.Infra.Logging;
using System;
using Sample.Infra.Data.Utils;

namespace Sample.Infra.CrossCutting.IoC
{
    public static class CoreIoC
    {
        public static void Register(IServiceCollection services, Func<DependencyInjectionConfig> dependencyInjectionConfig)
        {
            var config = dependencyInjectionConfig();

            services.AddSingleton<ILogger>(x =>
            {
                LogManager.Configuration = LogConfigurationFactory.CreateConfiguration(config.Template);
                return LogManager.GetCurrentClassLogger();
            });

            services.AddDbContextPool<CoreContext>(options => options.UseMySQL(config.Configuration.GetConnectionString("DefaultConnection")).AddInterceptors(new DbNoLockInterceptor()));

            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));

            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IUserService), typeof(UserService));
        }
    }
}