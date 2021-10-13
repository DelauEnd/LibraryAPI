using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class Author
    {
        public int AuthorId { get; set; }

        [Required]
        public string Surname { get; set; }

        [JsonIgnore]
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
