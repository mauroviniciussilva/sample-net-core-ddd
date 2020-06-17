using Sample.Domain.Entities;
using Sample.Domain.Interfaces;
using Sample.Domain.Interfaces.Repositories;
using Sample.Domain.Interfaces.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Domain.Service
{
    /// <summary>
    /// This is a sample service class, that will receive all the methods from the service base, and where you can create new ones
    /// </summary>
    public class SampleService : ServiceBase<SampleEntity>, ISampleService
    {
        #region [ Properties ]

        private readonly IOptions<SampleSettings> _settings;
        private readonly ISampleRepository _sampleRepository;

        #endregion

        #region [ Constructor ]

        public SampleService(ISampleRepository repository,
                             IUserHelper userHelper,
                             IOptions<SampleSettings> settings)
                             : base(repository, userHelper)
        {
            _sampleRepository = repository;
            _settings = settings;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Searches a list of entities based on a query filter
        /// </summary>
        /// <param name="filter">Query Filter</param>
        /// <returns>List of Entities</returns>
        public override PagedResult<SampleEntity> Search(QueryFilter filter)
        {
            #region [ Pagination ]

            var page = 1;
            var limit = 0;

            filter.Filters.TryGetValue("Page", out string stringPage);
            filter.Filters.TryGetValue("Limit", out string stringLimit);

            if (!string.IsNullOrEmpty(stringPage)) page = int.Parse(stringPage);
            if (!string.IsNullOrEmpty(stringLimit)) limit = int.Parse(stringLimit);

            #endregion

            #region [ Filter ]

            IQueryable<SampleEntity> query = _sampleRepository.GetAll();


            filter.Filters.TryGetValue("id", out string stringId);
            filter.Filters.TryGetValue("creationDate", out string stringCreationDate);
            int.TryParse(stringId, out int id);
            DateTime.TryParse(stringCreationDate, out DateTime creationDate);


            if (id > 0) query = query.Where(x => x.Id == id).Distinct();
            if (!string.IsNullOrEmpty(stringCreationDate)) query = query.Where(x => x.CreationDate == creationDate);

            #endregion

            var result = new PagedResult<SampleEntity>
            {
                CurrentPage = page,
                PageSize = limit,
                RowCount = query.Count()
            };

            if (limit > 0)
            {
                var pageCount = (double)result.RowCount / limit;
                result.PageCount = (int)Math.Ceiling(pageCount);

                var skip = (page - 1) * limit;
                result.Result = query.Skip(skip)
                                     .Take(limit)
                                     .OrderBy(x => x.Id)
                                     .ToList();
            }
            else
            {
                result.Result = query.ToList();
            }

            return result;
        }

        #endregion
    }
}