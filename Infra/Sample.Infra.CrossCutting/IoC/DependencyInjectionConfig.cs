using Sample.Infra.Logging;
using Microsoft.Extensions.Configuration;

namespace Sample.Infra.CrossCutting.IoC
{
    public class DependencyInjectionConfig
    {
        public LoggingLayoutTemplate Template { get; set; }
        public IConfiguration Configuration { get; set; }
    }
}