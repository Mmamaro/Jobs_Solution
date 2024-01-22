namespace AssessmentAPI.Models
{
    public class Weather
    {
        public int Id { get; set; }
        public string Temperature { get; set; }
        public string WindSpeed { get; set; }
        public string Description { get; set; }
        public Guid JobId { get; set; }
    }
}
