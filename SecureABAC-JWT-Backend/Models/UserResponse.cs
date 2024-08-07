﻿using System.ComponentModel.DataAnnotations;

namespace ABAC.Models
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
