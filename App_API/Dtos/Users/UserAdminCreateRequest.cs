﻿namespace App_API.Dtos.Users
{
    public class UserAdminCreateRequest
    {
        public int RoleId { get; set; }

        public int? PubId { get; set; }

        public string? Source { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public DateTime? HireDate { get; set; }
        
    }
}
