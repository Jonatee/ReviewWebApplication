namespace Review_Web_App.Models.DTOs
{
    public class LikeRequestModel
    {
        public Guid ReviewerId { get; set; }
        public Guid PostId { get; set; }
    }
    public class LikeResponseModel
    {
        public Guid Id { get; set; } = default!;
        public Guid ReviewerId { get; set; } = default!;
        public Guid PostId { get; set; } = default!;        
        public DateTime DateCreated { get; set; }
    }
        

}
