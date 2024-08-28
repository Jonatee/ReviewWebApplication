namespace Review_Web_App.Models.DTOs
{
    public class BookmarkRequestModel
    {
        public Guid ReviewerId { get; set; }
        public Guid PostId { get; set; }
    }
    public class BookmarkResponseModel
    {
        public Guid Id { get; set; } = default!;
        public Guid ReviewerId { get; set; } = default!;
        public Guid PostId { get; set; } = default!;
        public DateTime DateCreated { get; set; }
    }
}
