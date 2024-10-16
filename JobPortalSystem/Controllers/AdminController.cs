using JobPortalSystem.Data;
using JobPortalSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("You have accessed the Admin controller.");
        }


        // Get total jobs, total applications, and user activity
        [HttpGet("stats")]
        public async Task<IActionResult> GetSystemStats()
        {
            var totalJobs = await _context.Jobs.CountAsync();
            var totalApplications = await _context.Applications.CountAsync();
            var totalUsers = await _context.Users.CountAsync();

            return Ok(new
            {
                TotalJobs = totalJobs,
                TotalApplications = totalApplications,
                TotalUsers = totalUsers
            });
        }

        [HttpPost("employer")]
        public async Task<IActionResult> AddEmployer(Employer employer)
        {
            _context.Employers.Add(employer);
            await _context.SaveChangesAsync();
            return Ok(employer);
        }

        [HttpPut("employer/{id}")]
        public async Task<IActionResult> UpdateEmployer(int id, Employer updatedEmployer)
        {
            var employer = await _context.Employers.FindAsync(id);
            if (employer == null)
            {
                return NotFound();
            }

            employer.CompanyName = updatedEmployer.CompanyName;
            employer.Location = updatedEmployer.Location;
            // Update other properties

            await _context.SaveChangesAsync();
            return Ok(employer);
        }

        [HttpDelete("employer/{id}")]
        public async Task<IActionResult> DeleteEmployer(int id)
        {
            var employer = await _context.Employers.FindAsync(id);
            if (employer == null)
            {
                return NotFound();
            }

            _context.Employers.Remove(employer);
            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}
