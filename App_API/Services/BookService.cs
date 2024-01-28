using App_API.Dtos.Books;
using App_API.Exceptions;
using App_API.Models;
using App_API.Repositories;
using AutoMapper;
using System.Net;

namespace App_API.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;
        private readonly IPublisherService _publisherService;
        private readonly IMapper _mapper;
        public BookService(IBookRepository repository, IPublisherService publisherService, IMapper mapper)
        {
            _repository = repository;
            _publisherService = publisherService;
            _mapper = mapper;
        }

        public void Add(BookCreateRequest bookCreateRequest)
        {
            if(bookCreateRequest== null)
            {
                throw new MyException((int)HttpStatusCode.BadRequest, "Please input all field!");
            }

            if(bookCreateRequest.PubId != null)
            {
                _publisherService.CheckPublisherExist(bookCreateRequest.PubId);
            }

            var book = _mapper.Map<Book>(bookCreateRequest);
            _repository.Add(book);
        }

        public void Delete(int id)
        {
            ChekcBookExist(id);

            var book = _repository.GetByIdWithBookAuthor(id);
            if(book.BookAuthors.Count > 0)
            {
                throw new MyException((int)HttpStatusCode.BadRequest, "This book is used!");
            }

            _repository.DeleteById(id);
        }

        public List<BookResponse> GetBooks()
        {
            var books = _repository.GetBooks();

            if(books == null || books.Count == 0)
            {
                throw new MyException((int)HttpStatusCode.NotFound, "Can not find any book!");
            }

            var bookResponses = _mapper.Map<List<BookResponse>>(books);
            return bookResponses;
        }

        public BookResponse GetById(int id)
        {
            ChekcBookExist(id);

            var book = _repository.GetById(id);
            var bookResponse = _mapper.Map<BookResponse>(book);

            return bookResponse;
        }

        public void Update(int id, BookUpdateRequest bookUpdateRequest)
        {
            if(bookUpdateRequest== null)
            {
                throw new MyException((int)HttpStatusCode.BadRequest, "Please input all field");
            }

            if(id != bookUpdateRequest.BookId)
            {
                throw new MyException((int)HttpStatusCode.Conflict, "There is something wrong with bookId");
            }

            ChekcBookExist(id);

            if(bookUpdateRequest.PubId != null)
            {
                _publisherService.CheckPublisherExist(bookUpdateRequest.PubId);
            }

            var book = _repository.GetById(id);

            book.Royalty = bookUpdateRequest.Royalty;
            book.Advance = bookUpdateRequest.Advance;
            book.PubId = bookUpdateRequest.PubId;
            book.Title= bookUpdateRequest.Title;
            book.YtdSales= bookUpdateRequest.YtdSales;
            book.Notes= bookUpdateRequest.Notes;
            book.PublishedDate = bookUpdateRequest.PublishedDate;

            _repository.Update(book);
        }

        private void ChekcBookExist(int id)
        {
            var isBookExist = _repository.IsBookExist(id);
            if (!isBookExist)
            {
                throw new MyException((int)HttpStatusCode.NotFound, "Can not find book has id: " + id);
            }
        }
    }
}
