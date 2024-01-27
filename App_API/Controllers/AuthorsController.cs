using App_API.Dtos.Authors;
using App_API.Dtos.Common;
using App_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var authors = _authorService.GetAll();
            return Ok(new SuccessResponse() { Message = "Get all authors success!", Data= authors });
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            var author = _authorService.GetById(id);
            return Ok(new SuccessResponse() { Message = $"Get author has id: {id} success!", Data= author });
        }

        [HttpPost]
        public IActionResult Add([FromBody] AuthorCreateRequest authorCreateRequest)
        {
            _authorService.Add(authorCreateRequest);
            return Ok(new SuccessResponse() { Message = "Create new author success!"});
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] AuthorUpdateRequest authorUpdateRequest)
        {
            _authorService.Update(id, authorUpdateRequest);
            return Ok(new SuccessResponse() { Message = $"Update author has id: {id} success!" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _authorService.Delete(id);
            return Ok(new SuccessResponse() { Message = $"Delete author has id: {id} success!" });
        }
    }
}
