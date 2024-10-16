using JobPortalSystem.Data;
using JobPortalSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Controllers
{
    [Authorize(Roles = "Users")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Basic check to see if the controller is accessible
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("You have accessed the User controller.");
        }

        // Fetch all users - change route to "api/Users/AllUsers"
        [HttpGet("AllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // Create User and handle job application
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user, int jobId)
        {
            // Add the user to the Users table
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Find the job by ID (this links the application to the job)
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
            {
                return BadRequest("Job not found");
            }

            // Create a new application for the user
            var application = new Application
            {
                UserId = user.UserId, // Link to the new user
                JobId = job.JobId,    // Link to the existing job
                AppliedDate = DateTime.Now,
                Status = ApplicationStatus.Applied // Use the enum instead of string
            };

            // Add the application and save it to the database
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            // Return the newly created user
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }






    }
}
