using App_API.Exceptions;
using App_API.Repositories;
using System.Net;

namespace App_API.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository;
        public PublisherService(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        public void CheckPublisherExist(int? id)
        {
            if (id == null) return;

            var isPublisherExist = _publisherRepository.IsPublisherExist((int)id);
            if (!isPublisherExist) 
                throw new MyException((int) HttpStatusCode.BadRequest, $"PublisherService:: Can not found publisher has id: {id}");
        }
    }
}
