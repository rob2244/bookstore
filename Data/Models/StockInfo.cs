namespace bookstore.Data.Models
{
    public class StockInfo
    {
        public int Id { get; set; }
        public double UnitPrice { get; set; }
        public int NumberInStock { get; set; }
        public double SaleDiscount { get; set; }
    }
}