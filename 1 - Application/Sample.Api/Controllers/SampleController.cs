using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Api.ViewModel;
using Sample.Domain.Entities;
using Sample.Domain.Interfaces.Services;

namespace Sample.Api.Controllers
{
    /// <summary>
    /// This is a sample controller class, that will receive all the methods from the controller base
    /// </summary>
    [Route("Api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase<SampleEntity, SampleEditViewModel, SampleListViewModel>
    {
        #region [ Constructor ]

        public SampleController(ISampleService service, IMapper mapper) : base(service, mapper)
        {
            
        }

        #endregion

        #region [ Methods ]

        [HttpGet, Route("GetSomething/{id}")]
        public IActionResult GetSomething()
        {
            // Make some logic in here, call application, etc.
            return Ok();
        }

        #endregion

        #region [ Allow Anonymous Methods ]

        [AllowAnonymous]
        [HttpGet, Route("AllowAnonymousMethod")]
        public IActionResult AllowAnonymousMethod()
        {
            // You can make some logic here

            return Ok();
        }

        #endregion
    }
}