using System;
using System.Linq;
using Bogus;
using bookstore.Data;
using bookstore.Data.Models;
using bookstore.GraphQL;
using bookstore.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace bookstore
{
    public static class Extensions
    {
        public static IApplicationBuilder Seed(this IApplicationBuilder builder)
        {
            Randomizer.Seed = new Random(8675309);

            // issue with books orders and authors

            var stock = new Faker<StockInfo>()
               .RuleFor(s => s.Id, opt => opt.IndexGlobal)
               .RuleFor(s => s.NumberInStock, opt => opt.Random.Int(0, 100))
               .RuleFor(s => s.SaleDiscount, opt => opt.Random.Double(0, 20))
               .RuleFor(s => s.UnitPrice, opt => opt.Random.Double(15, 50));

            var books = new Faker<Book>()
              .RuleFor(b => b.Id, opt => opt.IndexGlobal)
              .RuleFor(b => b.PageNumber, opt => opt.Random.Int(0, 500))
              .RuleFor(b => b.Rating, opt => opt.Random.Double(0, 5))
              .RuleFor(b => b.StockInfo, opt => stock.Generate())
              .RuleFor(b => b.Title, opt => opt.Random.Words())
              .RuleFor(b => b.Description, opt => opt.Lorem.Paragraph())
              .Generate(1000);


            var authors = new Faker<Author>()
                .RuleFor(a => a.Id, opt => opt.IndexGlobal)
                .RuleFor(a => a.FirstName, opt => opt.Name.FirstName())
                .RuleFor(a => a.LastName, opt => opt.Name.LastName())
                .RuleFor(a => a.Biography, opt => opt.Lorem.Paragraph())
                .RuleFor(a => a.Books, opt => opt.PickRandom(books, 15).ToHashSet())
                .Generate(20);

            var address = new Faker<Address>()
                .RuleFor(a => a.City, opt => opt.Address.City())
                .RuleFor(a => a.Country, opt => opt.Address.Country())
                .RuleFor(a => a.State, opt => opt.Address.State())
                .RuleFor(a => a.Street, opt => opt.Address.StreetName())
                .RuleFor(a => a.Zip, opt => opt.Address.ZipCode());


            var order = new Faker<Order>()
                .RuleFor(o => o.Id, opt => opt.IndexGlobal)
                .RuleFor(o => o.Address, opt => address.Generate())
                .RuleFor(o => o.Books, opt => opt.PickRandom(books, 10).ToHashSet())
                .RuleFor(o => o.Price, (opt, o) => o.Books.Select(b => b.StockInfo.UnitPrice * b.StockInfo.SaleDiscount).Sum());

            var customers = new Faker<Customer>()
                .RuleFor(c => c.Id, opt => opt.IndexGlobal)
                .RuleFor(c => c.Address, opt => address.Generate())
                .RuleFor(c => c.FirstName, opt => opt.Name.FirstName())
                .RuleFor(c => c.LastName, opt => opt.Name.LastName())
                .RuleFor(c => c.PhoneNumber, opt => opt.Phone.PhoneNumber())
                .RuleFor(c => c.Email, opt => opt.Internet.Email())
                .RuleFor(c => c.Orders, opt => order.Generate(5))
                .Generate(10);

            var scope = builder.ApplicationServices.CreateScope();
            var ctx = scope.ServiceProvider.GetService<BookstoreContext>();

            ctx.Books.AddRange(books);
            ctx.Authors.AddRange(authors);
            ctx.Customers.AddRange(customers);
            ctx.SaveChanges();

            return builder;
        }

        public static IApplicationBuilder UseGraphQL(this IApplicationBuilder builder, Action<GraphQLSettings> config)
        {
            var settings = new GraphQLSettings();
            config(settings);

            builder.UseMiddleware<GraphQLMiddleware>(settings);
            return builder;
        }
    }
}