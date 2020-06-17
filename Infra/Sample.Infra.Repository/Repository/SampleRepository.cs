using Sample.Domain.Entities;
using Sample.Domain.Interfaces;
using Sample.Domain.Interfaces.Repositories;
using Sample.Infra.Data.Context;

namespace Sample.Infra.Data.Repositories
{
    /// <summary>
    /// This is a sample repository class, that will receive all the methods from the repository base
    /// </summary>
    public class SampleRepository : RepositoryBase<SampleEntity>, ISampleRepository
    {
        public SampleRepository(SampleContext context) : base(context)
        {
            
        }
    }
}