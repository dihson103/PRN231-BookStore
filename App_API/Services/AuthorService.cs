using App_API.Dtos.Authors;
using App_API.Exceptions;
using App_API.Models;
using App_API.Repositories;
using AutoMapper;
using System.Net;

namespace App_API.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public void Add(AuthorCreateRequest authorCreateRequest)
        {
            checkEmailExist(authorCreateRequest.EmailAddress);

            var author = _mapper.Map<Author>(authorCreateRequest);
            _authorRepository.Add(author);
        }

        public void Delete(int id)
        {
            checkAuthorExist(id);

            var author = _authorRepository.GetById(id);

            _authorRepository.Delete(author);
        }

        public List<AuthorResponse> GetAll()
        {
            var authors = _authorRepository.GetAuthors();

            if(authors == null || authors.Count == 0)
            {
                throw new MyException((int)HttpStatusCode.NotFound, "AuthorService:: Can not find any author!");
            }

            var authorResponses = _mapper.Map<List<AuthorResponse>>(authors);   
            return authorResponses;
        }

        public AuthorResponse GetById(int id)
        {
            checkAuthorExist(id);

            var author = _authorRepository.GetById(id);
            var authorResponse = _mapper.Map<AuthorResponse>(author);

            return authorResponse;
        }

        public void Update(int id, AuthorUpdateRequest authorUpdateRequest)
        {
            if(id != authorUpdateRequest.AuthorId)
            {
                throw new MyException((int)HttpStatusCode.Conflict, "AuthorService:: There is something wrong with authorId");
            }

            checkAuthorExist(id);

            var author = _authorRepository.GetById(id) as Author;

            if(author.EmailAddress!= authorUpdateRequest.EmailAddress)
            {
                checkEmailExist(authorUpdateRequest.EmailAddress);
            }

            author.Phone = authorUpdateRequest.Phone;
            author.LastName= authorUpdateRequest.LastName;
            author.FirstName= authorUpdateRequest.FirstName;
            author.Address = authorUpdateRequest.Address;
            author.City= authorUpdateRequest.City;
            author.State= authorUpdateRequest.State;
            author.Zip = authorUpdateRequest.Zip;
            author.EmailAddress= authorUpdateRequest.EmailAddress;
        }

        private void checkAuthorExist(int id)
        {
            var isAuthorExist = _authorRepository.IsAuthorExist(id);
            if (!isAuthorExist)
            {
                throw new MyException((int)HttpStatusCode.NotFound, $"AuthorService:: Can not find author has id: {id}");
            }
        }

        private void checkEmailExist(string email)
        {
            var isEmailExist = _authorRepository.IsEmailExist(email);
            if(isEmailExist)
            {
                throw new MyException((int)HttpStatusCode.BadRequest, $"AuthorService:: Email is already exist");
            }
        }
    }
}
