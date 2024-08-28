using System.ComponentModel.DataAnnotations;

namespace Review_Web_App.Models.DTOs
{
    public class ReviewerRequestModel
    {
        [Required(ErrorMessage = "FirstName is Required")]
        public string FirstName { get; set; } = default!;
        [Required(ErrorMessage = "LastName is Required")]
        public string LastName { get; set; } = default!;
        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; } = default!;
        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = default!;
        [Required]
        public string VerificationCode { get; set; } = default!;
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W_]).+$", ErrorMessage = "Password must contain at least one letter, one number, and one special character")]
        public string Password { get; set; } = default!;
        [Required]
        [Compare("Password", ErrorMessage = "PassWord does not Match")]
        public string ConfirmPassWord { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; } = default!;
        public IFormFile? ProfilePicture { get; set; }
    }
    public class ReviewerUpdateModel
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public IFormFile? ProfilePicture { get; set; }
    }
    public class ReviewerResponseModel
    {
        public Guid Id { get; set; } = default!;
        public Guid UserId { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; } = default!;
        public string? ProfilePicture { get; set; }
        public ICollection<CommentResponseModel> Comments { get; set; } = new HashSet<CommentResponseModel>();
        public ICollection<LikeResponseModel> Likes { get; set; } = new HashSet<LikeResponseModel>();
        public ICollection<PostResponseModel> Posts { get; set; } = new HashSet<PostResponseModel>();
        public ICollection<BookmarkResponseModel> Bookmarks { get; set; } = new HashSet<BookmarkResponseModel>();
    }
}
