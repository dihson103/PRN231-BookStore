namespace App_client.Dtos.Users
{
    public class UserResponse
    {
        public int UserId { get; set; }
        public int? RoleId { get; set; }
        public string? EmailAddress { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateTime? HireDate { get; set; }
        public int? PubId { get; set; }
        public string? Source { get; set; }
    }
}
