namespace App_API.Dtos.Publishers
{
    public class PublisherCreateRequest
    {
        public string PublisherName { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }
    }
}
