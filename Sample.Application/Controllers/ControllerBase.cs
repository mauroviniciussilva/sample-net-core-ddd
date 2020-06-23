using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Application.Response;
using Sample.Domain.Entities;
using Sample.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace Sample.Application.Controllers
{
    [Authorize]
    [Route("Api/[controller]")]
    [Route("{language:regex(^[[a-z]]{{2}}(?:-[[A-Z]]{{2}})?$)}/Api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class ControllerBase<TEntity, TViewModelEdit, TViewModelList> : ControllerBase
        where TEntity : EntityBase
        where TViewModelEdit : class
        where TViewModelList : class
    {
        #region [ Properties ]

        private readonly IServiceBase<TEntity> _service;
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
        /// <response code="200">Returns a list of entities</response>
        /// <response code="400">Returns an error object</response>
        [Produces("application/json")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public virtual ActionResult<IEnumerable<TViewModelList>> Get()
        {
            IEnumerable<TEntity> data = _service.GetAll();

            var vwResult = _mapper.Map<IList<TViewModelList>>(data);

            return Ok(vwResult);
        }

        /// <summary>
        /// A basic get by id API
        /// </summary>
        /// <param name="id">Id of the Entity</param>
        /// <response code="200">Returns the requested entity</response>
        /// <response code="400">Returns an error object</response>
        [Produces("application/json")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public virtual ActionResult<TViewModelEdit> GetById(int id)
        {
            TEntity entity = _service.GetById(id);

            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity).Name} not found");
            }

            var vwResult = _mapper.Map<TViewModelEdit>(entity);

            return Ok(vwResult);
        }

        /// <summary>
        /// A search API that returns paged data.
        /// </summary>
        /// <remarks>
        /// The QueryFilter that is used in this route can be passed in the URI, for example:
        ///     <para><![CDATA[Api/User/Search?Page=1&Limit=10&Active=true]]></para>
        ///     <para><![CDATA[Api/User/Search?Page=1&Limit=10&UserCreationId=1]]></para>
        /// <para>If the Page and the Limit are not informed, they default to 1 and 20, respectively</para>
        /// </remarks>
        /// <param name="filter">An object with the filter parameters</param>
        /// <response code="200">Returns an object that contains a list of entities and the record count</response>
        /// <response code="400">Returns an error object</response>
        [Produces("application/json")]
        [HttpGet("Search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public virtual ActionResult<SearchResultViewModel> Search([ModelBinder] QueryFilter filter)
        {
            var pagedResult = _service.Search(filter);

            var vwResult = _mapper.Map<IList<TViewModelList>>(pagedResult.Result);

            return Ok(new SearchResultViewModel { Result = vwResult, Count = pagedResult.RowCount });
        }

        /// <summary>
        /// A basic post API to add new registries to your database
        /// </summary>
        /// <param name="viewModel">Entity's ViewModel</param>
        /// <response code="200">Returns the entity after created</response>
        /// <response code="400">Returns an error object</response>
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public virtual ActionResult<TViewModelEdit> Post([FromBody] TViewModelEdit viewModel)
        {
            var entity = _mapper.Map<TEntity>(viewModel);

            entity.Validate();

            var result = _service.Add(entity);

            var vwResult = _mapper.Map<TViewModelEdit>(result);

            return Created(this.Request.Scheme, vwResult);
        }

        /// <summary>
        /// A basic put API to edit the registries from your database
        /// </summary>
        /// <param name="id">Entity's Id</param>
        /// <param name="viewModel">Entity's ViewModel</param>
        /// <response code="200">Returns the entity after updated</response>
        /// <response code="400">Returns an error object</response>
        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public virtual ActionResult<TViewModelEdit> Put(int id, [FromBody] TViewModelEdit viewModel)
        {
            TEntity entity = _service.GetById(id);

            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity).Name} not found");
            }

            var updatedEntity = _mapper.Map(viewModel, entity);

            updatedEntity.Validate();

            var result = _service.Update(updatedEntity);

            var vwResult = _mapper.Map<TViewModelEdit>(result);

            return Ok(vwResult);
        }

        /// <summary>
        /// Deletes a entity based on its id
        /// </summary>
        /// <param name="id">Entity's Id</param>
        /// <response code="200">The response is empty</response>
        /// <response code="400">Returns an error object</response>
        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public virtual ActionResult Delete(int id)
        {
            TEntity entity = _service.GetById(id);

            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity).Name} not found");
            }

            _service.DeleteById(id);

            return Ok();
        }

        #endregion

        #region [ Helpers ]

        /// <summary>
        /// Result of the search API
        /// </summary>
        /// <remarks><![CDATA[
        /// This class is being used only so that swagger can read the response type of the request to the Search method
        /// ]]>
        /// </remarks>        
        public class SearchResultViewModel
        {
            /// <summary>
            /// Filtered list of entities
            /// </summary>
            public IEnumerable<TViewModelList> Result { get; set; }
            /// <summary>
            /// Record count
            /// </summary>
            public int Count { get; set; }
        }

        #endregion
    }
}