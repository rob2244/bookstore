using GraphQL;
using GraphQL.Types;

namespace bookstore.GraphQL
{
    public class BookstoreSchema : Schema
    {
        public BookstoreSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            Query = resolver.Resolve<BookstoreQuery>();
        }
    }
}