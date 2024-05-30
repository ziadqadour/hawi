using AutoMapper;
using Hawi.Dtos;
using Hawi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Newtonsoft.Json.Linq;

namespace Hawi.Controllers
{

    [Route("hawi/Rolesstatistics/[action]")]
    [ApiController]
    public class RolesstatisticsController : ControllerBase
    {
        private readonly HawiContext _context ;
      
        public RolesstatisticsController(HawiContext context) 
        {
            _context = context;
        }

        [HttpGet(Name = "GetAllSeasons")]
        public async Task<IActionResult> GetAllSeasons()
        {
            try
            {
                var seasons = await _context.Seasons
                    .Select(s => new { s.SeasonId, s.SeasonName })
                    .ToListAsync();

                return Ok(seasons);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllCoachTypes")]
        public async Task<IActionResult> GetAllCoachTypes()
        {
            try
            {
                var coachTypes = await _context.CoachCoachTypes
                    .Select(ct => new { ct.CoachTypeId, ct.CoachType })
                    .ToListAsync();

                return Ok(coachTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost(Name = "AddNewCoachstatisticsSeason")]
        public IActionResult AddNewCoachstatisticsSeason(long UserProfileId, [FromBody] AddNewCoachstatisticsSeasonDto Coach)
        {
            try
            {
                var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileId).FirstOrDefault();
                if (userprofile == null)
                    return NotFound("لم يتم العثور على هذا المدرب ");

                if (userprofile.RoleId != 4)
                    return BadRequest("يجب تسجيل دخول كمدرب اولا!");

                var checkIfExistForSameSeason = _context.Coaches.Where(x => x.UserProfileId == UserProfileId && x.SeasonId == Coach.SeasonId).FirstOrDefault();
                return BadRequest("لقد قمت من قبل بانشاء احصائيات لهذا المدرب لنفس الموسم!");

                 var newCoach = new Coach
                {
                    UserProfileId = UserProfileId,
                    CoachTypeId = Coach.CoachTypeId,
                    SeasonId = Coach.SeasonId,
                    Win = Coach.Win,
                    Lose = Coach.Lose,
                    Draw = Coach.Draw,
                    GoalScored = Coach.GoalScored,
                    GoalConceded = Coach.GoalConceded,
                    NumberOfMatches = Coach.NumberOfMatches,
                };
                _context.Coaches.Add(newCoach);
                _context.SaveChanges();
                return Ok("تم الاضافة بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut(Name = "UpdateCoachstatisticsSeason")]
        public IActionResult UpdateCoachstatisticsSeason(long CoachId, [FromBody] UpdateCoachstatisticsSeasonDto Coach)
        {
            try
            {

                var Selectcoach = _context.Coaches.Where(x => x.CoachId == CoachId).FirstOrDefault();
                if (Selectcoach == null)
                    return NotFound("لم يتم انشاء احصائيات لهذا المدرب!");


                Selectcoach.NumberOfMatches = Coach.NumberOfMatches;
                Selectcoach.Draw= Coach.Draw;
                Selectcoach.Win= Coach.Win;
                Selectcoach.Lose= Coach.Lose;
                Selectcoach.NumberOfMatches= Coach.NumberOfMatches;
                Selectcoach.GoalScored= Coach.GoalScored;
                Selectcoach.GoalConceded= Coach.GoalConceded;

                _context.Coaches.Update(Selectcoach);
                _context.SaveChanges();
                return Ok("تم التعديل بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

      
        [HttpGet (Name = "GetCoachstatistics")]
        public IActionResult GetCoachstatistics(long userprofileid , byte? seasonId = null)
        {
            try
            {

                var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == userprofileid).FirstOrDefault();
                if (userprofile == null)
                    return NotFound("لم يتم العثور على هذا المدرب ");

                if (userprofile.RoleId != 4)
                    return BadRequest("يجب تسجيل دخول كمدرب اولا!");

                if (seasonId != null)
                {
                    var coach=_context.Coaches.Where(x=> x.UserProfileId == userprofileid && x.SeasonId== seasonId).FirstOrDefault();
                    getcoachDto results = new getcoachDto();
                    results.CoachId = coach.CoachId;   
                    results.UserProfileId = userprofileid;
                    results.NumberOfMatches = coach.NumberOfMatches;
                    results.Win = coach.Win;
                    results.Lose = coach.Lose;
                    results.Draw = coach.Draw;
                    results.GoalScored = coach.GoalScored;
                    results.GoalConceded = coach.GoalConceded;
                    return Ok(results);
                }
                else
                {
                    getcoachDto results = new getcoachDto();
                    var coach = _context.Coaches.Where(x => x.UserProfileId == userprofileid).ToList();
                 
                    results.UserProfileId = userprofileid;
                    results.NumberOfMatches = (short?)coach.Sum(c => c.NumberOfMatches ?? 0);
                    results.Win = (short?)coach.Sum(c => c.Win ?? 0);
                    results.Lose = (short?)coach.Sum(c => c.Lose ?? 0);
                    results.Draw = (short?)coach.Sum(c => c.Draw ?? 0);
                    results.GoalScored = (short?)coach.Sum(c => c.GoalScored ?? 0);
                    results.GoalConceded = (short?)coach.Sum(c => c.GoalConceded ?? 0);
                  
                    return Ok(results);

                }
                return Ok();
            }
            catch (Exception ex)
            {
                 return BadRequest(ex.Message);
            }
        }

        //////////////////////////// referees
        
        [HttpGet(Name = "GetRefereestatistics")]
        public IActionResult GetRefereestatistics(long userprofileid, byte? seasonId = null)
        {
            try
            {
                var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == userprofileid).FirstOrDefault();
                if (userprofile == null)
                    return NotFound("لم يتم العثور على هذا المستخدم! ");

                if (userprofile.RoleId != 3)
                    return BadRequest("يجب تسجيل دخول كحكم اولا!");


                if (seasonId != null)
                {
                    var Referee = _context.Referees.Where(x => x.UserProfileId == userprofileid && x.SeasonId == seasonId).FirstOrDefault();
                    getRefereeDto results = new getRefereeDto();
                    results.RefereeId = Referee.RefereeId;
                    results.UserProfileId = userprofileid;
                    results.NumberOfMatches = Referee.NumberOfMatches;
                    results.Fouls = Referee.Fouls;
                    results.Penalties = Referee.Penalties;
                    results.YellowCards = Referee.YellowCards;
                    results.RedCards = Referee.RedCards;
                    results.OffsideCases = Referee.OffsideCases;
                    results.FourthReferee = Referee.FourthReferee;
                    results.LineReferee = Referee.LineReferee;
                    return Ok(results);
                }
                else
                {
                    getRefereeDto results = new getRefereeDto();
                    var Referee = _context.Referees.Where(x => x.UserProfileId == userprofileid).ToList();

                    results.UserProfileId = userprofileid;
                    results.NumberOfMatches = (short?)Referee.Sum(c => c.NumberOfMatches ?? 0);
                    results.Fouls = (short?)Referee.Sum(c => c.Fouls ?? 0);
                    results.Penalties = (short?)Referee.Sum(c => c.Penalties ?? 0);
                    results.YellowCards = (short?)Referee.Sum(c => c.YellowCards ?? 0);
                    results.RedCards = (short?)Referee.Sum(c => c.RedCards ?? 0);
                    results.OffsideCases = (short?)Referee.Sum(c => c.OffsideCases ?? 0);
                    results.FourthReferee = (short?)Referee.Sum(c => c.FourthReferee ?? 0);
                    results.LineReferee = (short?)Referee.Sum(c => c.LineReferee ?? 0);

                    return Ok(results);

                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        //////////////////////////////////////achievement
        
        ////////
        [HttpPost(Name = "AddUserAchievement")]
        public IActionResult AddUserAchievement([FromBody] AddAchievementDto Achievement,long AchievementUserProfileIId , long UserProfileIId)
        {
            try
            {
                //owner of Achievement
                var AchievementUserProfile = _context.UserProfiles.Where(x => x.UserProfileId == AchievementUserProfileIId).FirstOrDefault();
                if (AchievementUserProfile == null)
                    return BadRequest("لم يتم العثور على صاحب الانجاز!");
                var UserProfile = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileIId).FirstOrDefault();
                if (UserProfile == null)
                    return BadRequest("لم يتم العثور على مدخل الانجاز");


                var newAchievement = new Achievement
                {
                    AchievementUserProfileId = AchievementUserProfileIId,
                    AchievementId = UserProfileIId,
                    AchievementDate = Achievement.AchievementDate,
                    AchievementTitle = Achievement.AchievementTitle,
                    SeasonId = Achievement.SeasonId,
                    UserProfileId = UserProfileIId,
                };
                _context.Achievements.Add(newAchievement);
                _context.SaveChanges();
                return Ok("تم الاضافة بنجاح");
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPut (Name = "EditAchievement")]
        public IActionResult EditAchievement(long AchievementUserProfileId, long UserProfileId, long AchievementId, [FromBody] AddAchievementDto Achievementdto)
        {

            var AchievementUserProfile = _context.UserProfiles.Where(x => x.UserProfileId == AchievementUserProfileId).FirstOrDefault();
            if (AchievementUserProfile == null)
                return BadRequest("لم يتم العثور على صاحب الانجاز!");
            var UserProfile = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileId).FirstOrDefault();
            if (UserProfile == null)
                return BadRequest("لم يتم العثور على مدخل الانجاز");

            var achievement = _context.Achievements.Where(x => x.AchievementId == AchievementId).FirstOrDefault();
            if (achievement == null)
                return BadRequest("لم يتم العثور على هذا الانجاز!");

            achievement.UserProfileId= UserProfileId;
            achievement.AchievementUserProfileId = AchievementUserProfileId;
            achievement.AchievementDate = Achievementdto.AchievementDate;
            achievement.AchievementTitle= Achievementdto.AchievementTitle;
            achievement.SeasonId= Achievementdto.SeasonId;  
            _context.Achievements.Update(achievement);
            _context.SaveChanges();
            return Ok("تم التعديل بنجاح ");
        }

        [HttpDelete(Name = "DeleteAchievement")]
        public IActionResult DeleteAchievement (long AchievementUserProfileId, long UserProfileId, long AchievementId)
        {

            var achievement = _context.Achievements.Where(x => x.AchievementId == AchievementId).FirstOrDefault();
            if (achievement == null)
                return BadRequest("لم يتم العثور على هذا الانجاز!");

            if (achievement.UserProfileId != UserProfileId || achievement.AchievementUserProfileId != AchievementUserProfileId)
                return BadRequest("لا يمكنك حذف هذا لانجاز!");

            _context.Achievements.Remove(achievement);
            _context.SaveChanges();
            return Ok("تم الحذف بنجاح ");
        }
       
        
        [HttpGet(Name = "GetspecieficUserAchievement")]
        
        public IActionResult GetspecieficUserAchievement(long AchievementUserProfileId, byte? SeasonId=null)
        {
            var AchievementUserProfile = _context.UserProfiles.Where(x => x.UserProfileId == AchievementUserProfileId).FirstOrDefault();
            if (AchievementUserProfile == null)
                return BadRequest("لم يتم العثور على هذا المستخدم!");

            List<GetAchievementDto> achievement = null;

            if (SeasonId == null)
            {
                achievement = _context.Achievements
                    .Where(x => x.AchievementUserProfileId == AchievementUserProfileId)
                    .Select(x => new GetAchievementDto
                    {
                        AchievementId= x.AchievementId,
                        AchievementDate = x.AchievementDate,
                        AchievementTitle = x.AchievementTitle,
                        Season = (_context.Seasons.FirstOrDefault(s => s.SeasonId == x.SeasonId).SeasonName)
                    })
                    .ToList();
            }
            else
            {
                achievement = _context.Achievements
                    .Where(x => x.AchievementUserProfileId == AchievementUserProfileId && x.SeasonId == SeasonId)
                    .Select(x => new GetAchievementDto
                    {
                        AchievementId = x.AchievementId,
                        AchievementDate = x.AchievementDate,
                        AchievementTitle = x.AchievementTitle,
                        Season =(_context.Seasons.FirstOrDefault(s => s.SeasonId == x.SeasonId).SeasonName) 
                    })
                    .ToList();
            }
            return Ok(achievement);
        }
    }
}
