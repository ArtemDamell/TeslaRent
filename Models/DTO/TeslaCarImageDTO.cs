namespace Models
{
    // 54. Создаём DTO модель для изображений
    public class TeslaCarImageDTO
    {
        public int Id { get; set; }
        public string? CarImageUrl { get; set; }
        public int CarId { get; set; }
    }
}
