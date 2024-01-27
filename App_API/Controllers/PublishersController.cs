using App_API.Dtos.Common;
using App_API.Dtos.Publishers;
using App_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService _publisherService;
        public PublishersController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var publishers = _publisherService.GetAll();
            return Ok(new SuccessResponse() { Message = "Get all publishers success!", Data = publishers });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var publisher = _publisherService.GetById(id);
            return Ok(new SuccessResponse() { Message = $"Get publisher has id: {id} success", Data = publisher });
        }

        [HttpPost]
        public IActionResult AddPublisher([FromBody] PublisherCreateRequest publisherCreateRequest)
        {
            _publisherService.Add(publisherCreateRequest);
            return Ok(new SuccessResponse() { Message = "Create new publisher success!"} );
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePublisher([FromRoute] int id, [FromBody] PublisherUpdateRequest publisherUpdateRequest)
        {
            _publisherService.Update(id, publisherUpdateRequest);
            return Ok(new SuccessResponse() { Message = $"Update publisher has id: {id} success!" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePublisher([FromRoute] int id)
        {
            _publisherService.Delete(id);
            return Ok(new SuccessResponse() { Message = $"Delete publisher has id: {id} success!" });
        }
    }
}
