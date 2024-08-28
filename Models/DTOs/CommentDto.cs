using Review_Web_App.Entities;

namespace Review_Web_App.Models.DTOs
{
    public class CommentRequestModel
    {
        public string CommentText { get; set; } = default!;
        public Guid ReviewerId { get; set; } = default!;
        public Guid PostId { get; set; } = default!;
        public IFormFile? FileUrl { get; set; }
    }
    public class CommentResponseModel
    {
        public Guid Id { get; set; }
        public string CommentText { get; set; } = default!;
        public Guid ReviewerId { get; set; } = default!;
        public string ReviewerUserName { get; set; } = default!;
        public Guid PostId { get; set; } = default!;
        public bool IsCommentByUser { get; set; }
        public string? FileUrl { get; set; }
        public DateTime DateCreated { get; set; }
      
    }
}
