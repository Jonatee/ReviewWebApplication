namespace Review_Web_App.Entities
{
    public class Reviewer
    {
        public Guid Id { get; set; } = new Guid();
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; } = default!;
        public string? ProfilePicture { get; set; }
        public ICollection<Post> Posts { get; set; }= new HashSet<Post>();
        public ICollection<Comment> Comments { get; set; }= new HashSet<Comment>();
        public ICollection<Like> Likes { get; set; } = new HashSet<Like>();
        public ICollection<Bookmark> Bookmarks { get; set; } = new HashSet<Bookmark>();
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public User? User { get; set; }
        public Guid UserId { get; set; }


    }
}
