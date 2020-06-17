using System;

namespace Sample.Infra.CrossCutting.IoC
{
    public class ServiceLocator
    {
        private static IServiceProvider _provider;
        public static void Init(IServiceProvider provider) => _provider = provider;
        public static T Resolve<T>() => (T)_provider.GetService(typeof(T));
    }
}