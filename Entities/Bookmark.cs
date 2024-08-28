namespace Review_Web_App.Entities
{
    public class Bookmark
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ReviewerId { get; set; } = default!;
        public Reviewer? Reviewer { get; set; }
        public Guid PostId { get; set; } = default!;
        public Post? Post { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
