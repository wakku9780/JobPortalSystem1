using JobPortalSystem.Data;
using JobPortalSystem.DTOs;
using JobPortalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserProfilesController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        public async Task<ActionResult<UserProfile>> CreateUserProfile(UserProfileDto userProfileDto)
        {
            // Map DTO to UserProfile
            var userProfile = new UserProfile
            {
                Bio = userProfileDto.Bio,
                ProfilePictureUrl = userProfileDto.ProfilePictureUrl,
                ContactNumber = userProfileDto.ContactNumber,
                Address = userProfileDto.Address,
                UserId = userProfileDto.UserId
            };

            _context.UserProfiles.Add(userProfile);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserProfileByUserId), new { userId = userProfile.UserId }, userProfile);
        }


        [HttpGet("user/{userId}")]
        public async Task<ActionResult<UserProfile>> GetUserProfileByUserId(int userId)
        {
            var profile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserId == userId); // Change UserProfiles to Profiles
            if (profile == null)
            {
                return NotFound();
            }
            return profile;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserProfile(int id, UserProfile updatedProfile)
        {
            if (id != updatedProfile.UserProfileId)
            {
                return BadRequest();
            }

            _context.Entry(updatedProfile).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.UserProfiles.Any(up => up.UserProfileId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpGet("{userId}/applications")]
        public async Task<ActionResult<IEnumerable<Application>>> GetUserApplications(int userId)
        {
            var applications = await _context.Applications
                                             .Where(a => a.UserId == userId)
                                             .Include(a => a.Job)
                                             .ToListAsync();
            return applications;
        }



    }
}
