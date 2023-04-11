using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class CarAccessoryDTO
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2), MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MinLength(2), MaxLength(300)]
        public string Description { get; set; }
        [Required]
        public string Icon { get; set; }
        public virtual ICollection<TeslaCarDTO>? Car { get; set; }
    }
}
