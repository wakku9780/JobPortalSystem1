using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.Models
{
    public class Employer
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployerId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }

        // One-to-Many relationship with Jobs
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }

}
