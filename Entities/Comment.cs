namespace Review_Web_App.Entities
{
    public class Comment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CommentText { get; set; } = default!;
        public Guid ReviewerId { get; set; } = default!;
        public Reviewer Reviewer { get; set; } = default!;
        public Guid PostId { get; set; } = default!;
        public Post Post { get; set; } = default!;
        public string? FileUrl { get; set; } 
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }

    }
}
