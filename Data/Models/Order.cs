using System.Collections.Generic;

namespace bookstore.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public double Price { get; set; }
        public Address Address { get; set; }
        public virtual ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}