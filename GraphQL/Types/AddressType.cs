using bookstore.Data.Models;
using GraphQL.Types;

namespace bookstore.GraphQL.Types
{
    public class AddressType : ObjectGraphType<Address>
    {
        public AddressType()
        {
            Field(a => a.Street);
            Field(a => a.City);
            Field(a => a.State);
            Field(a => a.Zip);
            Field(a => a.Country);
        }
    }
}