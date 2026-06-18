using System.ComponentModel.DataAnnotations;

namespace ECommerce_WebAPi.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
