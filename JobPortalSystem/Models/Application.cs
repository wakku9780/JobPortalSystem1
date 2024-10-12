using System.Text.Json.Serialization;

namespace JobPortalSystem.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public DateTime AppliedDate { get; set; }
        public string Status { get; set; }

        // Many-to-One with User
        [JsonIgnore]
        public int UserId { get; set; }
        public User? User { get; set; }

        // Many-to-One with Job
        [JsonIgnore]
        public int JobId { get; set; }
        public Job? Job { get; set; }
    }

}
