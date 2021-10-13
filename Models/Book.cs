using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class Book
    {
        public int BookId { get; set; }

        [Required]
        [MaxLength(30)]
        public string Title { get; set; }

        [DefaultValue("DateTime.Now")]
        public DateTime PublicationDate { get; set; }

        [JsonIgnore]
        public List<Author> Authors { get; set; } = new List<Author>();
    }
}
