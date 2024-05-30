using Hawi.Dtos;
using Hawi.Models;
using Hawi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hawi.Controllers
{
    [Route("hawi/Subscription/[action]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {

        private readonly HawiContext _context ;
        private readonly UserFunctions _userFunctions;
        public SubscriptionController(UserFunctions userFunctions , HawiContext context)
        {
            _userFunctions = userFunctions;
            _context = context;
        }

        //target "اللى انت عايز تتابعة" , sourse "انت"

        [HttpGet (Name = "GetAllFollowsOfUser")]
        public IActionResult GetAllFollowsOfUser(long SourceUserProfileId)
        {  //follower
            var SourceUser = _context.UserProfiles.Where(x => x.UserProfileId == SourceUserProfileId).FirstOrDefault();
            if (SourceUser == null)  return NotFound(" لم يتم العثور على المستخدم هذا! ");

            var subscripes = _context.Subscriptions
                .Where(x=>x.TargetUserProfileId == SourceUserProfileId && 
                x.SourceUserProfile.IsActive==true && x.SourceUserProfile.User.IsActive == true)
                .Select(x => new
                {
                    TargetUserProfileId=x.SourceUserProfileId,
                    role=x.SourceUserProfile.Role.RoleId,
                    x.SourceUserProfile.Role.Role1,
                    name= _userFunctions.GetUserName(x.SourceUserProfile.RoleId, x.SourceUserProfile.UserProfileId, x.SourceUserProfile.UserId),
                    imageUrl= _userFunctions.GetUserImage(x.SourceUserProfile.UserProfileId, x.SourceUserProfile.RoleId),
                }).ToList();
            return Ok(subscripes);
        }
        
        [HttpGet(Name = "GetUsersThatUserFollow")]
        public IActionResult GetUsersThatUserFollow(long SourceUserProfileId)
        {//who is follow "يتابع" // المتابعين
            var SourceUser = _context.UserProfiles.Where(x => x.UserProfileId == SourceUserProfileId).FirstOrDefault();
            if (SourceUser == null) return NotFound(" لم يتم العثور على المستخدم هذا! ");
            
            var subscripes = _context.Subscriptions
                .Where(x => x.SourceUserProfileId == SourceUserProfileId 
                && x.TargetUserProfile.IsActive == true && x.TargetUserProfile.User.IsActive == true)
                .Select(x => new
                {
                    TargetUserProfileId = x.TargetUserProfileId,
                    role = x.TargetUserProfile.Role.RoleId,
                    x.TargetUserProfile.Role.Role1,
                    name = _userFunctions.GetUserName(x.TargetUserProfile.RoleId, x.TargetUserProfile.UserProfileId, x.TargetUserProfile.UserId),
                    imageUrl = _userFunctions.GetUserImage(x.TargetUserProfile.UserProfileId, x.TargetUserProfile.RoleId),
                }).ToList();
            return Ok(subscripes);
        }

        [HttpGet(Name = "SuggestionsForFollowUp")]
        public async Task< IActionResult> SuggestionsForFollowUp(long SourceUserProfileId, [FromQuery] OwnerParameters ownerParameters)
        {
            try
            {
                var sourceUserProfile = _context.UserProfiles.FirstOrDefault(up => up.UserProfileId == SourceUserProfileId);
                if (sourceUserProfile == null) return NotFound("!يجب عليك انشاء حساب اولا");
                var sourceUserRole = sourceUserProfile.RoleId;

                // Get a list of user profiles with the same role as the source user profile.
                var sameRoleUserProfiles = _context.UserProfiles
                    .Where(up => up.RoleId == sourceUserRole 
                    && up.UserProfileId != SourceUserProfileId 
                    && up.IsActive==true 
                    && up.User.IsActive==true
                    &&!_context.Subscriptions.Any(s => s.SourceUserProfileId == SourceUserProfileId && s.TargetUserProfileId == up.UserProfileId))
                    .Take(20)
                    .Select( up => new FollowUpSuggestion
                    {
                        UserProfileId = up.UserProfileId,
                        RoleId = up.RoleId,
                        Role = up.Role.Role1 ?? null,
                        UserName = _userFunctions.GetUserName(up.RoleId, up.UserProfileId, up.UserId),
                        UserImage = _userFunctions.GetUserImage(up.UserProfileId, up.RoleId),
                        NumberOfFollowers = up.SubscriptionTargetUserProfiles.Count,
                        
                    })
                  .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize / 2)
                  .Take(ownerParameters.PageSize / 2)
                    .ToList();

                // Find the user profile with the most followers (subscribers).
                var userWithMostFollowers = _context.UserProfiles
                     .Where(up => up.UserProfileId != SourceUserProfileId 
                     && up.SubscriptionTargetUserProfiles.Count != 0 
                     && up.IsActive == true 
                     && up.User.IsActive == true
                     && !_context.Subscriptions.Any(s => s.SourceUserProfileId == SourceUserProfileId && s.TargetUserProfileId == up.UserProfileId))
                    .OrderByDescending(up => up.SubscriptionTargetUserProfiles.Count)
                    .Take(20)
                    .Select(up => new FollowUpSuggestion
                    {
                        UserProfileId = up.UserProfileId,
                        RoleId = up.RoleId,
                        Role = up.Role.Role1 ?? null,
                        UserName = _userFunctions.GetUserName(up.RoleId, up.UserProfileId, up.UserId),
                        UserImage = _userFunctions.GetUserImage(up.UserProfileId, up.RoleId),
                        NumberOfFollowers = up.SubscriptionTargetUserProfiles.Count,

                    })
                  .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize / 2)
                  .Take(ownerParameters.PageSize / 2)
                    .ToList();

                var allSuggestions = sameRoleUserProfiles.Concat(userWithMostFollowers).ToList();
                allSuggestions = allSuggestions.GroupBy(s => s.UserProfileId).Select(g => g.First()).ToList();

                var rng = new Random();
                allSuggestions = allSuggestions.OrderBy(_ => rng.Next()).ToList();

                return Ok(allSuggestions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "Follow")]
        public IActionResult Follow(long SourceUserProfileId, long TargetUserProfileId)
        {
            var SourceUser = _context.UserProfiles.Where(x => x.UserProfileId == SourceUserProfileId).FirstOrDefault();
            if (SourceUser == null) return NotFound(" لم يتم العثور على المستخدم هذا! ");

            var TargetUser = _context.UserProfiles.Where(x => x.UserProfileId == TargetUserProfileId).FirstOrDefault();
            if (TargetUser == null) return NotFound(" لم يتم العثور على المستخدم الذى تود متابعتة! ");

            var isfollow = _context.Subscriptions.Where(x => x.SourceUserProfileId == SourceUserProfileId && x.TargetUserProfileId == TargetUserProfileId).FirstOrDefault();
            if (isfollow != null) return Ok("تمت المتابعة");

            var newSubscripe = new Subscription
            {
                SourceUserProfileId = SourceUserProfileId,
                TargetUserProfileId = TargetUserProfileId,
            };

            _context.Subscriptions.Add(newSubscripe);
            _context.SaveChanges();
            return Ok("تمت المتابعة بنجاح!");
        }
      
        [HttpDelete (Name ="UnFollow")]
        public IActionResult UnFollow (long SourceUserProfileId, long TargetUserProfileId)
        {
            var SourceUser = _context.UserProfiles.Where(x => x.UserProfileId == SourceUserProfileId).FirstOrDefault();
            if (SourceUser == null) return NotFound(" لم يتم العثور على المستخدم هذا! ");
            var TargetUser = _context.UserProfiles.Where(x => x.UserProfileId == TargetUserProfileId).FirstOrDefault();
         
            if (TargetUser == null) return NotFound(" لم يتم العثور على المستخدم الذى تود متابعتة! ");

            var subscription = _context.Subscriptions.Where(x => x.SourceUserProfileId == SourceUserProfileId
                                   && x.TargetUserProfileId == TargetUserProfileId).FirstOrDefault();
            if (subscription == null) return BadRequest("لم تتم المتابعة من قبل !");

            _context.Subscriptions.Remove(subscription);
            _context.SaveChanges();
            return Ok("تم الغاء المتابعة");
        }
    }
}

