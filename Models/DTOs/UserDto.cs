using Review_Web_App.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Review_Web_App.Models.DTOs
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public Role Role { get; set; } = default!;
    }
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; } = default!;
        [Required]
        public string Password { get; set; } = default!;
    }
    public class UpdateUserRequest
    {

    }

    }
