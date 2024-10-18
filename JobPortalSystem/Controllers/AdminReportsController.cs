using JobPortalSystem.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminReportsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public AdminReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("TotalJobs")]
        public async Task<IActionResult> GetTotalJobs()
        {
            var totalJobs = await _context.Jobs.CountAsync();
            return Ok(new { totalJobs });
        }

        [HttpGet("TotalApplications")]
        public async Task<IActionResult> GetTotalApplications()
        {
            var totalApplications = await _context.Applications.CountAsync();
            return Ok(new { totalApplications });
        }

        [HttpGet("TopEmployers")]
        public async Task<IActionResult> GetTopEmployers()
        {
            var topEmployers = await _context.Employers
                                .OrderByDescending(e => e.Jobs.Count)
                                .Select(e => new
                                {
                                    e.EmployerId,
                                    e.CompanyName,
                                    TotalJobs = e.Jobs.Count
                                })
                                .Take(5) // Top 5 employers
                                .ToListAsync();

            return Ok(topEmployers);
        }
    }
}
