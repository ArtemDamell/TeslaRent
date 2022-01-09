using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
