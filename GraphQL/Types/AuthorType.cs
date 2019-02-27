using bookstore.Data;
using bookstore.Data.Models;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace bookstore.GraphQL.Types
{
    public class AuthorType : ObjectGraphType<Author>
    {
        public AuthorType(BookstoreContext ctx)
        {
            Field(a => a.Id).Description("Author Id");
            Field(a => a.FirstName).Description("Authors First Name");
            Field(a => a.LastName).Description("Authors Last Name");
            Field(a => a.Biography).Description("Authors Biography");
            FieldAsync<ListGraphType<BookType>>(
                name: "Books",
                resolve: async gqlCtx => (
                    await ctx.Authors.Include(a => a.Books)
                    .FirstOrDefaultAsync(a => a.Id == gqlCtx.Source.Id)
                    )?.Books,
                    description: "Books Author has written"
                );
        }
    }
}