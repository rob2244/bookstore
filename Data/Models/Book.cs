namespace bookstore.Data.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public int PageNumber { get; set; }
        public virtual Author Author { get; set; }
        public virtual StockInfo StockInfo { get; set; }
    }
}