using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Domain.Entities;
using Sample.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace Sample.Application.Controllers
{
    [Authorize]
    [Route("Api/[controller]")]
    [Route("{language:regex(^[[a-z]]{{2}}(?:-[[A-Z]]{{2}})?$)}/api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ControllerBase<TEntity, TViewModelEdit, TViewModelList> : ControllerBase
        where TEntity : EntityBase
        where TViewModelEdit : class
        where TViewModelList : class
    {
        #region [ Properties ]

        protected readonly IServiceBase<TEntity> _service;
        private readonly IMapper _mapper;

        #endregion

        #region [ Constructor ]

        public ControllerBase(IServiceBase<TEntity> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// A basic get that returns all the registries from your database
        /// </summary>
        /// <remarks><![CDATA[
        /// I highly recommend you to take this method out of your code if you're going to deal with a large amount of registries
        /// ]]>
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult<IEnumerable<string>> Get()
        {
            IEnumerable<TEntity> data = _service.GetAll();

            var vwResult = _mapper.Map<IList<TViewModelList>>(data);

            return Ok(vwResult);
        }

        /// <summary>
        /// A basic get by id API
        /// </summary>
        /// <param name="id">Id of the Entity</param>
        /// <returns>The ViewModel of the Entity</returns>
        [HttpGet("{id}")]
        public virtual ActionResult GetById(int id)
        {
            TEntity data = _service.GetById(id);

            var vwResult = _mapper.Map<TViewModelEdit>(data);

            return Ok(vwResult);
        }

        /// <summary>
        /// A search API that returns paged data.
        /// </summary>
        /// <remarks><![CDATA[
        /// The QueryFilter that is used in this route can be passed in the URI:
        /// For example:
        ///     Api/User/Search?Page=1&Limit=10&Active=true
        ///     Api/User/Search?Page=1&Limit=10&UserCreationId=1
        /// If the Page and the Limit are not informed, they default to 1 and 20, respectively
        /// ]]>
        /// </remarks>
        /// <param name="filter">An object with the filter parameters</param>
        /// <returns>Object that contains a list of entities and the record count</returns>
        [HttpGet("Search")]
        public virtual ActionResult<IEnumerable<string>> Search([ModelBinder]QueryFilter filter)
        {
            var pagedResult = _service.Search(filter);

            var vwResult = _mapper.Map<IList<TViewModelList>>(pagedResult.Result);

            return Ok(new { Result = vwResult, Count = pagedResult.RowCount });
        }

        /// <summary>
        /// A basic post API to add new registries to your database
        /// </summary>
        /// <param name="viewModel">Entity's ViewModel</param>
        /// <returns>Entity's ViewModel after created</returns>
        [HttpPost]
        public virtual ActionResult Post([FromBody] TViewModelEdit viewModel)
        {
            var entity = _mapper.Map<TEntity>(viewModel);

            if (!entity.IsValid())
                return BadRequest(entity.GetErros());

            var result = _service.Add(entity);

            var vwResult = _mapper.Map<TViewModelEdit>(result);

            return Created(this.Request.Scheme, vwResult);
        }

        /// <summary>
        /// A basic put API to edit the registries from your database
        /// </summary>
        /// <param name="id">Entity's Id</param>
        /// <param name="viewModel">Entity's ViewModel</param>
        /// <returns>Entity's ViewModel after updated</returns>
        [HttpPut("{id}")]
        public virtual ActionResult Put(int id, [FromBody] TViewModelEdit viewModel)
        {
            TEntity entity = _service.GetById(id);

            if (entity == null)
            {
                return NotFound();
            }

            var updatedEntity = _mapper.Map(viewModel, entity);

            if (!updatedEntity.IsValid())
                return BadRequest(entity.GetErros());

            var result = _service.Update(updatedEntity);

            var vwResult = _mapper.Map<TViewModelEdit>(result);

            return Ok(vwResult);
        }

        /// <summary>
        /// Deletes a entity based on its id
        /// </summary>
        /// <param name="id">Entity's Id</param>
        [HttpDelete("{id}")]
        public virtual ActionResult Delete(int id)
        {
            TEntity entity = _service.GetById(id);

            if (entity == null)
            {
                return NotFound();
            }

            _service.DeleteById(id);

            return Ok();
        }

        #endregion
    }
}