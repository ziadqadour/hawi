using Hawi.Dtos;
using Hawi.Extensions;
using Hawi.Models;
using Hawi.Repository;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using UnidecodeSharpFork;

namespace Hawi.Controllers
{
    [Route("hawi/Admin/[action]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly HawiContext _context ;
        private readonly UserFunctions _userFunctions;
        private readonly AddRecordsInDB _record ;
 
        public AdminController(UserFunctions userFunctions , HawiContext context , AddRecordsInDB record)
        {
            _userFunctions = userFunctions;
            _context = context;
            _record = record;
        }

        //refactor code 
        [HttpGet(Name = "GetAdmins")]
        public IActionResult GetAdmins(long userprofileid)
        {
            try
            {
                var admins = _context.UserProfiles
                    .Where(x => x.RoleId == 6 
                       && x.IsActive == true)
                     .Select(x => new
                     {
                       userprofileid = x.UserProfileId,
                       name = x.User.Name,
                       createDate = x.CreateDate,
                       image = _userFunctions.GetUserImage(x.UserProfileId,x.RoleId),
                      })
                     .ToList();
                return Ok(admins);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpPost(Name = "AddSeason")]
        public IActionResult AddSeason(long userprofileid, [FromBody] AddSeasonDto seasonDto)
        {
            try
            {
                var newSeason = _record.AddSeason(seasonDto.StartDate, seasonDto.EndDate);
                if (newSeason != null) return BadRequest(newSeason);

                return Ok("تم الاضافة بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "AddAdmin")]
        public IActionResult AddAdmin(long userprofileid, string MobilePhone)
        {
            try
            {
                var CheckSelectedNewAdmin = _context.Users.Where(x => x.Mobile == MobilePhone ).FirstOrDefault();
                if (CheckSelectedNewAdmin == null)  return NotFound("!يجب على هذا المستخدم انشاء حساب اولا ");

                var CheckIfHaveAcountAsAdmin=_context.UserProfiles.Where(x=>x.UserId== CheckSelectedNewAdmin.UserId &&x.RoleId==6).FirstOrDefault();
                if(CheckIfHaveAcountAsAdmin!=null)
                 if (CheckIfHaveAcountAsAdmin.RoleId == 6 && CheckIfHaveAcountAsAdmin.IsActive==true) return BadRequest("! المستخدم لدية حساب ك ادمن ");
               
                if (CheckIfHaveAcountAsAdmin != null)
                {
                    if (CheckIfHaveAcountAsAdmin.IsActive == false)
                    {
                        CheckIfHaveAcountAsAdmin.IsActive = true;
                        _context.SaveChanges();
                        return Ok("تم الاضافة بنجاح");
                    }
                }
                var newUserProfile = _record.AddNewUserProfile(CheckSelectedNewAdmin.UserId, 6);
                if (newUserProfile != null) return BadRequest(newUserProfile);
                return Ok("تم الاضافة بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpPut(Name = "EditSeason")]
        public IActionResult EditSeason(long userprofileid, byte SeasonId, [FromBody] UpdateSeasonDto seasonDto)
        {
            try
            {     
                var selectedseason = _context.Seasons.Where(x => x.SeasonId == SeasonId).FirstOrDefault();
                if (selectedseason == null) return NotFound("!لم يتم العثور على الموسم");

                selectedseason.StartDate = seasonDto.StartDate;
                selectedseason.EndDate = seasonDto.EndDate;
                _context.SaveChanges();

                return Ok("تم التعديل بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(Name = "DeleteSeason")]
        public IActionResult DeleteSeason(long userprofileid, byte SeasonId)
        {
            try
            {
                var selectedseason = _context.Seasons.Where(x => x.SeasonId == SeasonId).FirstOrDefault();
                if (selectedseason == null) return NotFound("لم يتم العثور على الموسم!");

                _context.Seasons.Remove(selectedseason);
                _context.SaveChanges();

                return Ok("تم الحذف بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(Name = "Deletedmin")]
        public IActionResult Deletedmin(long userprofileid, long DeletedAdminUserProfileId)
        {
            try
            {
                var CheckSelectedNewAdmin = _context.UserProfiles.Where(x => x.UserProfileId == DeletedAdminUserProfileId && x.IsActive == true).FirstOrDefault();
                if (CheckSelectedNewAdmin == null) return NotFound("لم يتم العثور عل الادمن الذى تود حذفة");

                CheckSelectedNewAdmin.IsActive = false;
                _context.SaveChanges();

                return Ok("تم الحذف بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
