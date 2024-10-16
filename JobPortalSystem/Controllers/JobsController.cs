using JobPortalSystem.Data;
using JobPortalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {
            return await _context.Jobs.Include(j => j.Employer).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Job>> CreateJob(Job job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetJobs), new { id = job.JobId }, job);
        }


        [HttpGet("search")] // New route for searching jobs
        public async Task<ActionResult<IEnumerable<Job>>> SearchJobs(
            string jobTitle = null,
            string location = null,
            string category = null,
            int? experienceRequired = null)
        {
            var query = _context.Jobs.AsQueryable();

            if (!string.IsNullOrEmpty(jobTitle))
            {
                query = query.Where(j => j.JobTitle.Contains(jobTitle));
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(j => j.Location.Contains(location));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(j => j.Category.Contains(category));
            }

            if (experienceRequired.HasValue)
            {
                query = query.Where(j => j.ExperienceRequired == experienceRequired.Value);
            }

            var jobs = await query.Include(j => j.Employer).ToListAsync();
            return Ok(jobs);
        }

    }

}
