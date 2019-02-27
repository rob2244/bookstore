using bookstore.Data.Models;
using GraphQL.Types;

namespace bookstore.GraphQL.Types
{
    public class StockInfoType : ObjectGraphType<StockInfo>
    {
        public StockInfoType()
        {
            Field(s => s.Id).Description("Stock Info Id");
            Field(s => s.NumberInStock).Description("Number of Books in Stock");
            Field(s => s.SaleDiscount).Description("Discount on book");
            Field(s => s.UnitPrice).Description("Unit price per book");
        }
    }
}