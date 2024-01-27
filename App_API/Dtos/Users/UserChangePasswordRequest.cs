namespace App_API.Dtos.Users
{
    public class UserChangePasswordRequest
    {
        public int UserId { get; set; }
        public string Password { get; set; }
    }
}
