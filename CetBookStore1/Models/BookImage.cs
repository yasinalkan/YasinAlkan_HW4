namespace CetBookStore1.Models
{
    public class BookImage
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string FileName { get; set; }
        public bool IsDefaultImage { get; set; }
        public virtual Book Book { get; set; }
    }
}
