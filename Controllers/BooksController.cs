using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Models;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext db;

        public BooksController(LibraryContext context)
        {
            db = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            if (!db.Books.Any())
                return StatusCode(StatusCodes.Status204NoContent, new { Status = "Error", Message = "Book table is empty" });
            return await db.Books.AsNoTracking().ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            if (!BookExists(id))
                return StatusCode(StatusCodes.Status404NotFound, new { Status = "Error", Message = "Book not found" });

            //TODO: Переделать под вывод только фамилий авторов
            var book = await db.Books.AsNoTracking().Include(book => book.Authors).FirstOrDefaultAsync(book => book.BookId == id);

            return book;
        }
 
        // POST: api/Books
         [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            if (book == null)
                return StatusCode(StatusCodes.Status400BadRequest, new { Status = "Error", Message = "Incorrect book" });
            db.Books.Add(book);
            await db.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { Status = "Success", Message = "Book added successfully" });
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            if (!BookExists(id))
                return StatusCode(StatusCodes.Status404NotFound, new { Status = "Error", Message = "Book not found" });

            var book = await db.Books.FindAsync(id);
            db.Books.Remove(book);
            await db.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { Status = "Success", Message = "Deleted successfully" });
        }

        private bool BookExists(int id)
        {
            return db.Books.Any(e => e.BookId == id);
        }
    }
}
