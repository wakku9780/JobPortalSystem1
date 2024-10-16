using JobPortalSystem.Data;
using JobPortalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly NotificationHub _notificationHub;
        private readonly EmailService _emailService;

        public ApplicationsController(ApplicationDbContext context, NotificationHub notificationHub, EmailService emailService)
        {
            _context = context;
            _notificationHub = notificationHub;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplications()
        {
            return await _context.Applications.Include(a => a.User).Include(a => a.Job).ToListAsync();
        }



        [HttpPost]
        public async Task<ActionResult<Application>> CreateApplication([FromBody] Application application)
        {
            // Save the application
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            // Fetch the User details from the database using UserId
            var user = await _context.Users.FindAsync(application.UserId);

            if (user == null)
            {
                return BadRequest("Invalid User ID");
            }

            // After saving the application, send notifications and email
            var message = "You have successfully applied for the job.";
            await _notificationHub.SendApplicationStatusUpdate(application.UserId.ToString(), message);
            await _emailService.SendEmailAsync(user.Email, "Job Application Submitted", message);

            return CreatedAtAction(nameof(GetApplications), new { id = application.ApplicationId }, application);
        }


        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateApplicationStatus(int id, [FromBody] ApplicationStatus newStatus)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            application.Status = newStatus;
            await _context.SaveChangesAsync();

            // Send notification/email about status update
            var user = await _context.Users.FindAsync(application.UserId);
            var message = $"Your application status has been updated to: {newStatus}";
            await _notificationHub.SendApplicationStatusUpdate(application.UserId.ToString(), message);
            await _emailService.SendEmailAsync(user.Email, "Job Application Status Update", message);

            return NoContent();
        }


        [HttpDelete("{id}/withdraw")]
        public async Task<IActionResult> WithdrawApplication(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            // Check if the application can be withdrawn (only if the status is "Applied")
            if (application.Status == ApplicationStatus.Applied) // Use the enum here
            {
                _context.Applications.Remove(application);
                await _context.SaveChangesAsync();

                var user = await _context.Users.FindAsync(application.UserId);
                var message = "You have withdrawn your job application.";
                await _notificationHub.SendApplicationStatusUpdate(application.UserId.ToString(), message);
                await _emailService.SendEmailAsync(user.Email, "Job Application Withdrawn", message);

                return NoContent();
            }

            return BadRequest("Cannot withdraw the application at its current status.");
        }





    }

}
