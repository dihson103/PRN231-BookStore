using App_API.Dtos.Books;
using App_API.Dtos.Common;
using App_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var books = _bookService.GetBooks();
            return Ok(new SuccessResponse() { Message = "Get all books success!", Data= books });
        }

        [HttpGet("search")]
        [EnableQuery]
        public IActionResult Search()
        {
            var books = _bookService.GetBooks();
            return Ok(books.AsQueryable());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var book = _bookService.GetById(id);
            return Ok(new SuccessResponse() { Message = $"Get book has id: {id} success", Data= book });
        }

        [HttpPost]
        public IActionResult Add([FromBody] BookCreateRequest bookCreateRequest)
        {
            _bookService.Add(bookCreateRequest);
            return Ok(new SuccessResponse() { Message = "Create new book success"});
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] BookUpdateRequest bookUpdateRequest)
        {
            _bookService.Update(id, bookUpdateRequest);
            return Ok(new SuccessResponse() { Message = $"Update book has id:{id} success" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _bookService.Delete(id);
            return Ok(new SuccessResponse() { Message = $"Delete book has id: {id} success" });
        }
    }
}
