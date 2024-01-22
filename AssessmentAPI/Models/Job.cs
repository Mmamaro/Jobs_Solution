namespace AssessmentAPI.Models
{
    public class Job
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastRunTime { get; set; } = default(DateTime);
        public int TimeIntervals { get; set; }
        public string Status { get; set; }
        public string City { get; set; }

    }
}
