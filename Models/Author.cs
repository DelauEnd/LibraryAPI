using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Models
{
    public class Author
    {
        public int AuthorId { get; set; }

        [Required]
        public string Surname { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();
    }
}
