using Sample.Domain.Entities;
using Sample.Domain.Interfaces.Application;
using Sample.Domain.Interfaces.Services;

namespace Sample.Application
{
    /// <summary>
    /// This is a sample application class, that will receive all the methods from the application base
    /// </summary>
    public class SampleApplication : ApplicationBase<SampleEntity>, ISampleApplication
    {
        #region [ Properties ]

        private readonly new ISampleService _service;

        #endregion

        #region [ Constructor ]

        public SampleApplication(ISampleService service) : base(service)
        {
            _service = service;
        }

        #endregion
    }
}