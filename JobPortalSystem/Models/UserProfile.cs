using System.Text.Json.Serialization;

namespace JobPortalSystem.Models
{
    public class UserProfile
    {


        public int UserProfileId { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }

        // Relationship - One-to-One with User
        public int UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
    }
}

