using System.ComponentModel.DataAnnotations;
using WebApplication1.Validations;

namespace WebApplication1.Models
{
    public class Game
    {
        public int Id { get; set; }
        [UppercaseLetter]
        [Required]
        public string Name { get; set; }
        [UppercaseLetter]
        [Required]
        public string Platform { get; set; }
        [Required]
        public string Gamma { get; set; }
        [Required]
        public bool Free { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
