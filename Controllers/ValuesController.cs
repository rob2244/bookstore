using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bookstore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly BookstoreContext ctx;

        public ValuesController(BookstoreContext ctx)
        {
            this.ctx = ctx;
        }


        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var books = await ctx.Books.Include(b => b.Author).ToListAsync();
            return Ok(books);
        }
    }
}
