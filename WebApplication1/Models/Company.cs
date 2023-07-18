using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Validations;

namespace WebApplication1.Models
{
    public class Company
    {
       
        public int Id { get; set; }
        [Required(ErrorMessage ="This field is required")]
        [UppercaseLetter]
        public string Name { get; set; }
        public List<Game> Games { get; set; }
    }
}
