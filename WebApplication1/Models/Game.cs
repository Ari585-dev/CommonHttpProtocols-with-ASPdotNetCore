namespace WebApplication1.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Platform { get; set; }
        public string Gamma { get; set; }
        public bool Free { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
