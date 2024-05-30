using Hawi.Dtos;
using Hawi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hawi.Controllers
{
    [Route("hawi/Dashboard/[action]")]
    [ApiController]
    public class DashboardController : Controller
    {

        private readonly HawiContext _context;
        public DashboardController(HawiContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "reports")]
        public IActionResult reports()
        {
            var users = _context.Users.Count();
            var posts = _context.Articles.Count();
            var Advertisement = _context.Advertisements.Where(x => x.IsActive == true).Count();
            var SportInstitution = _context.SportInstitutions.Count();
            var events = _context.Events.Count();
            var matches = _context.Matches.Count();
            var userprofile = _context.UserProfiles.Count();
            var training = _context.Training.Count();
            var newreport = new reportsDto
            {
                users = users,
                posts = posts,
                Advertisement = Advertisement,
                SportInstitution = SportInstitution,
                training = training,
                matches = matches,
                events = events,
                userprofile = userprofile,

            };
            return Ok(newreport);
        }

    }
}
