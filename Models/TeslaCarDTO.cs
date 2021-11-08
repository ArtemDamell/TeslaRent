using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class TeslaCarDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter car name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter occupancy")]
        public int Occupancy { get; set; }

        [Range(1, 3000, ErrorMessage = "Regular rate must be between 1 and 3000")]
        public double RegularRate { get; set; }
        public string Details { get; set; }
        public string NumberOfSeats { get; set; }
    }
}
