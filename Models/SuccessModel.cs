namespace Models
{
    // 194. В проект Models добавить модель SuccessModel
    public class SuccessModel
    {
        public string Title { get; set; }
        public int StatusCode { get; set; }
        public string SuccessMessage { get; set; }
        public object Data { get; set; }
    }
}
