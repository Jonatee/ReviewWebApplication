namespace Review_Web_App.Entities
{
    public class Role
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = default!;
        public ICollection<User> Users { get; set; } = new HashSet<User>();
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }

    }
}
