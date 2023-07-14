using Microsoft.AspNetCore.Identity;
using RestfulGamesApi.DataAccessLayer.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace RestfulGamesApi.DataAccessLayer.Models
{
    public class User: BaseModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Role Role { get; set; }
        public string? PasswordHash { get; set; }
        public string? ResetPasswordHash { get; set; }
        public DateTime PasswordResetExpiryDate { get; set; }
    }
}
