using bookstore.Data;
using bookstore.Data.Models;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace bookstore.GraphQL.Types
{
    public class BookType : ObjectGraphType<Book>
    {
        public BookType(BookstoreContext ctx)
        {
            Field(b => b.Id).Description("Book Id");
            Field(b => b.PageNumber).Description("Number of pages in book");
            Field(b => b.Description).Description("Book Description");
            Field(b => b.Rating).Description("Book Rating");
            Field(b => b.Title).Description("Book Title");
            FieldAsync<AuthorType>(name: "Author", resolve: async gqlCtx => (
                await ctx.Books.Include(b => b.Author)
                    .FirstOrDefaultAsync(b => b.Id == gqlCtx.Source.Id)
                )?.Author
            );
            FieldAsync<StockInfoType>(
                name: "StockInfo",
                resolve: async gqlCtx => (
                    await ctx.Books.Include(b => b.StockInfo)
                        .FirstOrDefaultAsync(b => b.Id == gqlCtx.Source.Id)
                        )?.StockInfo,
                description: "Book stock info");
        }
    }
}