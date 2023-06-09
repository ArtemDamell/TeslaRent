﻿using System.ComponentModel.DataAnnotations;

namespace DataAccess.Data
{
    public class CarAccessory
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
        public virtual ICollection<TeslaCar>? Car { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
