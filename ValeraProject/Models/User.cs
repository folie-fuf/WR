using System.ComponentModel.DataAnnotations;

namespace ValeraProject.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [MinLength(3)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public string Role { get; set; } = "User";
        
        // один пользователь может иметь много Валер?
        public ICollection<Valera> Valeras { get; set; } = new List<Valera>();
    }
}