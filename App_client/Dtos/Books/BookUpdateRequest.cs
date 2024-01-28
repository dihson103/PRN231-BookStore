namespace App_client.Dtos.Books
{
    public class BookUpdateRequest
    {
        public int BookId { get; set; }

        public int? PubId { get; set; }

        public string? Title { get; set; }

        public string? Advance { get; set; }

        public double? Royalty { get; set; }

        public double? YtdSales { get; set; }

        public string? Notes { get; set; }

        public DateTime? PublishedDate { get; set; }
    }
}
