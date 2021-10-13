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
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryContext db;

        public AuthorsController(LibraryContext context)
        {
            db = context;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            if (!db.Authors.Any())
                return StatusCode(StatusCodes.Status204NoContent, new { Status = "Success", Message = "Author table is empty" });
            return await db.Authors.AsNoTracking().ToListAsync();
        }
        
        // GET: api/Authors/Books/5
        [HttpGet]
        [Route("Books/{id}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetAuthorBooks(int id)
        {
            if (!AuthorExists(id))
                return StatusCode(StatusCodes.Status404NotFound, new { Status = "Error", Message = "Author not found" });

            var books = await db.Books.AsNoTracking().Where(book => book.Authors.Any(author => author.AuthorId == id)).Select(x=>x.Title).ToListAsync();
            if (!books.Any())
                return StatusCode(StatusCodes.Status204NoContent, new { Status = "Success", Message = "Author has no books" });

            return books;
        }

        // POST: api/Authors
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            if (author == null)
                return StatusCode(StatusCodes.Status400BadRequest, new { Status = "Error", Message = "Incorrect author" });
            if (author.Surname.Length > 30)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Length of the surname is more than allowed" });
            db.Authors.Add(author);
            await db.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { Status = "Success", Message = "Author added successfully" });
        }

        // POST: api/Authors/5/AddBook
        [HttpPost]
        [Route("AddBook")]
        public async Task<ActionResult<Author>> AddBook(int authorId, int bookId)
        {
            if (!db.Books.Any(e => e.BookId == bookId))
                return StatusCode(StatusCodes.Status404NotFound, new { Status = "Error", Message = "Book not found" });
            var book = await db.Books.FindAsync(bookId);

            if (!AuthorExists(authorId))
                return StatusCode(StatusCodes.Status404NotFound, new { Status = "Error", Message = "Author not found" });
            var author = await db.Authors.Include(x => x.Books).FirstOrDefaultAsync(author => author.AuthorId == authorId);

            if (author.Books.Contains(book))
                return StatusCode(StatusCodes.Status400BadRequest, new { Status = "Error", Message = "Book already added" });

            author.Books.Add(book);
            await db.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { Status = "Success", Message = "Book added successfully" });
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Author>> DeleteAuthor(int id)
        {
             if (!AuthorExists(id))
                return StatusCode(StatusCodes.Status404NotFound, new { Status = "Error", Message = "Author not found" });

            var author = await db.Authors.FindAsync(id);
            db.Authors.Remove(author);
            await db.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { Status = "Success", Message = "Deleted successfully" });
        }

        private bool AuthorExists(int id)
        {
            return db.Authors.Any(e => e.AuthorId == id);
        }
    }
}
