using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalSystem.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackId { get; set; }

        public int UserId { get; set; } // Assuming you have UserId to link feedback to a user

        [Required]
        [MaxLength(500)]
        public string Comments { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
