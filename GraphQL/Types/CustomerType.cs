using System.Linq;
using bookstore.Data;
using bookstore.Data.Models;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace bookstore.GraphQL.Types
{
    public class CustomerType : ObjectGraphType<Customer>
    {
        public CustomerType(BookstoreContext ctx)
        {
            Field(c => c.Id).Description("The customers Id");
            Field(c => c.FirstName).Description("The customers first name");
            Field(c => c.LastName).Description("The customers last name");
            Field(c => c.Email).Description("The customers email");
            Field(c => c.PhoneNumber).Description("The customers Phone Number");
            Field<AddressType>(
                name: "Address",
                resolve: gqlCtx => gqlCtx.Source.Address,
                description: "The customers address");
            FieldAsync<ListGraphType<OrderType>>(
                name: "Orders",
                resolve: async gqlCtx => (
                    await ctx.Customers.Include(c => c.Orders)
                    .FirstOrDefaultAsync(c => c.Id == gqlCtx.Source.Id))?.Orders,
                description: "The customers Orders");
        }

    }
}