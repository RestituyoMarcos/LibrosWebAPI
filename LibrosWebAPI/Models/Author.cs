using System.ComponentModel.DataAnnotations;

namespace LibrosWebAPI.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        public int IdBook { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
    }
}
