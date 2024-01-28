using App_API.Dtos.Publishers;
using App_API.Exceptions;
using App_API.Models;
using App_API.Repositories;
using AutoMapper;
using System.Net;

namespace App_API.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository;
        private readonly IMapper _mapper;
        public PublisherService(IPublisherRepository publisherRepository, IMapper mapper)
        {
            _publisherRepository = publisherRepository;
            _mapper = mapper;
        }

        public void Add(PublisherCreateRequest publisherCreateRequest)
        {
            var publisher = _mapper.Map<Publisher>(publisherCreateRequest);
            _publisherRepository.Add(publisher);
        }

        public void CheckPublisherExist(int? id)
        {
            if (id == 0) return;

            var isPublisherExist = _publisherRepository.IsPublisherExist((int)id);
            if (!isPublisherExist) 
                throw new MyException((int) HttpStatusCode.BadRequest, $"PublisherService:: Can not found publisher has id: {id}");
        }

        public void Delete(int id)
        {
            CheckPublisherExist(id);

            //check publisher is used or not
            var publisher = _publisherRepository.GetPublisherWithUsersAndBook(id);
            if(publisher.Users.Count> 0 || publisher.Books.Count > 0)
            {
                throw new MyException((int)HttpStatusCode.Conflict, "This publisher is used!");
            }

            _publisherRepository.Delete(id);
        }

        public List<PublisherResponse> GetAll()
        {
            var publishers = _publisherRepository.GetAll();

            if(publishers == null || publishers.Count == 0)
            {
                throw new MyException((int)HttpStatusCode.NotFound, "Can not find any publisher");
            }

            var publisherResponses = _mapper.Map<List<PublisherResponse>>(publishers);
            return publisherResponses;
        }

        public PublisherResponse GetById(int id)
        {
            CheckPublisherExist(id);

            var publisher = _publisherRepository.GetById(id);
            var publisherResponse = _mapper.Map<PublisherResponse>(publisher);
            
            return publisherResponse;
        }

        public void Update(int id, PublisherUpdateRequest publisherUpdateRequest)
        {
            if(id != publisherUpdateRequest.PubId)
            {
                throw new MyException((int)HttpStatusCode.Conflict, "There is something wrong with publisherId");
            }

            CheckPublisherExist(id);

            var publisher = _publisherRepository.GetById(id);

            publisher.PublisherName = publisherUpdateRequest.PublisherName;
            publisher.State= publisherUpdateRequest.State;
            publisher.City= publisherUpdateRequest.City;
            publisher.Country= publisherUpdateRequest.Country;

            _publisherRepository.Update(publisher);
        }
    }
}
