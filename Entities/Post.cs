namespace Review_Web_App.Entities
{
    public class Post
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string PostTitle { get; set; } = default!; 
        public string PostText { get; set; } = default!;
        public Guid ReviewerId { get; set; } = default!;
        public Reviewer? Reviewer { get; set; }
        public Guid CategoryId {  get; set; } = default!;
        public Category? Category { get; set; } 
        public string PostFile { get; set; } = default!;
        public DateTime DateCreated { get; set; } = DateTime.Now; 
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public ICollection<Like> Likes { get; set; } = new HashSet<Like>(); 
        public ICollection<Bookmark> Bookmarks { get; set; } = new HashSet<Bookmark>();
        public bool IsDeleted { get; set; }

    }
}
