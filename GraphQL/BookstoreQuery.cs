using System.Linq;
using bookstore.Data;
using bookstore.Data.Models;
using bookstore.GraphQL.Types;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace bookstore.GraphQL
{
    public class BookstoreQuery : ObjectGraphType
    {
        public BookstoreQuery(BookstoreContext ctx)
        {
            Name = "Query";
            Field<ListGraphType<CustomerType>>(
                "customers",
                resolve: gqlCtx => ctx.Customers.ToListAsync()
            );

            Field<CustomerType>(
                "customer",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "customer id" }),
                resolve: gqlCtx => ctx.FindAsync<Customer>(gqlCtx.GetArgument<int>("id"))
            );

            Field<ListGraphType<AuthorType>>(
                "Authors",
                resolve: gqlCtx => ctx.Authors.ToListAsync()
            );

            Field<AuthorType>(
               "Author",
                 arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "author id" }
               ),
               resolve: gqlCtx => ctx.Books.FirstOrDefaultAsync(b => b.Author.Id == gqlCtx.GetArgument<int>("id", 0))
           );

            Field<ListGraphType<BookType>>(
               "books",
               resolve: gqlCtx => ctx.Books.ToListAsync()
           );

            Field<BookType>(
               "book",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "book id" }
               ),
               resolve: gqlCtx => ctx.Books.FirstOrDefaultAsync(b => b.Id == gqlCtx.GetArgument<int>("id", 0))
           );

            Field<ListGraphType<BookType>>(
               "booksByAuthor",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "author id" }
               ),
               resolve: gqlCtx => ctx.Books.Where(b => b.Author.Id == gqlCtx.GetArgument<int>("id", 0)).ToListAsync()
           );

            Field<BookType>(
                "bookByTitle",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title", Description = "book title" }
                ),
                resolve: gqlCtx => ctx.Books.FirstOrDefaultAsync(b => b.Title.Contains(gqlCtx.GetArgument<string>("title", "")))
            );
        }
    }
}