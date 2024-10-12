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

        public ApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplications()
        {
            return await _context.Applications.Include(a => a.User).Include(a => a.Job).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Application>> CreateApplication(Application application)
        {
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetApplications), new { id = application.ApplicationId }, application);
        }
    }

}
