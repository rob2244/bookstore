using System.Linq;
using bookstore.Data;
using bookstore.Data.Models;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace bookstore.GraphQL.Types
{
    public class OrderType : ObjectGraphType<Order>
    {
        public OrderType(BookstoreContext ctx)
        {
            Field(o => o.Id).Description("Order Id");
            Field(o => o.Price).Description("Total order price");
            Field<AddressType>(
                name: "Address",
                resolve: gqlCtx => gqlCtx.Source.Address,
                description: "Shipping Address");
            FieldAsync<ListGraphType<BookType>>(
                name: "Books",
                resolve: async gqlCtx => (
                    await ctx.Orders.Include(o => o.Books)
                    .FirstOrDefaultAsync(o => o.Id == gqlCtx.Source.Id))?.Books,
                description: "Books on order");
        }
    }
}