using System.ComponentModel.DataAnnotations;

namespace WebAppMvc.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string Pass { get; set; } = "";
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Age { get; set; }
    }
}
