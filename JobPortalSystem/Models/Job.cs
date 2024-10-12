using System.Text.Json.Serialization;

namespace JobPortalSystem.Models
{
    public class Job
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public int ExperienceRequired { get; set; }

        // Many-to-One with Employer
        public int EmployerId { get; set; }

        [JsonIgnore]
        public Employer? Employer { get; set; }

        // One-to-Many relationship with Applications

        [JsonIgnore]
        // One-to-Many relationship with Applications
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }

}
