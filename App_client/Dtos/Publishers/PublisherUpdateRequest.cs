namespace App_client.Dtos.Publishers
{
    public class PublisherUpdateRequest
    {
        public int PubId { get; set; }

        public string PublisherName { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }
    }
}
