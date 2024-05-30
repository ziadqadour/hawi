using Hawi.Dtos;
using Hawi.Extensions;
using Hawi.Models;
using Hawi.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnidecodeSharpFork;

namespace Hawi.Controllers
{
    [Route("hawi/Advertisement/[action]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {

        private readonly HawiContext _context;
        private readonly ImageFunctions _ImageFunctions;
        private readonly UserFunctions _UserFunctions;
        public AdvertisementController(UserFunctions userFunctions , HawiContext context , ImageFunctions ImageFunctions)
        {
            _UserFunctions = userFunctions;
            _context = context;
            _ImageFunctions = ImageFunctions;
        }

        [HttpGet(Name = "GetAdvertisements")]
        public IActionResult GetAdvertisements([FromQuery] OwnerParameters ownerParameters)
        {
            try
            {
                var advertisements = _context.Advertisements
                    .Where(a => a.IsActive == true)
                    .OrderBy(x => x.StartDate)
                    .Select(a => new
                    {   
                        userimagePath = _UserFunctions.GetUserImage(a.TargetUserProfileId, a.TargetUserProfile.RoleId),
                        Userprofileid = a.TargetUserProfileId,
                        username = _UserFunctions.GetUserName(a.TargetUserProfile.RoleId, a.TargetUserProfileId, a.TargetUserProfile.UserId),
                        userphone = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == a.TargetUserProfileId).User.Mobile ?? null,
                        AdvertisemenId = a.AdvertisementId,
                        AdvertisementTitle = a.AdvertisementTitle,
                        AdvertisementText = a.AdvertisementText,
                        TargetSite = a.TargetSite,
                        AdvertisementUrlfullPath = a.AdvertisementUrlfullPath,
                        TargetUserLogoUrlfullPath = a.TargetUserLogoUrlfullPath,
                        EndDate = a.EndDate,
                        StartDate = a.StartDate,
                        AdvertisemenSeen = _context.AdvertisementSeens.Where(x => x.AdvertisementId == a.AdvertisementId).Count(),
                        AdvertisemenVisit = _context.AdvertisementVisits.Where(x => x.AdvertisementId == a.AdvertisementId).Count(),
                    })
                .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                .Take(ownerParameters.PageSize)
                .ToList();

                return Ok(advertisements);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetAdvertisementVisitsAndSeen")]
        public IActionResult GetAdvertisementVisitsAndSeen(long advertisementId)
        {
            try
            {
                var visits = _context.AdvertisementVisits
                    .Where(av => av.AdvertisementId == advertisementId)
                    .Join(_context.AdvertisementVisits, av => av.UserProfileId, avn => avn.UserProfileId, (av, avn) => new { av.UserProfileId, avn.CreateDate })
                    .ToList();

                var seen = _context.AdvertisementSeens
                    .Where(asn => asn.AdvertisementId == advertisementId)
                    .Join(_context.AdvertisementVisits, asn => asn.UserProfileId, avn => avn.UserProfileId, (asn, avn) => new { asn.UserProfileId, avn.CreateDate })
                    .ToList();

                var visitUserIds = visits
                    .OrderByDescending(v => v.CreateDate)
                    .Select(v => v.UserProfileId)
                    .Distinct()
                    .ToList();

                var seenUserIds = seen
                    .OrderByDescending(s => s.CreateDate)
                    .Select(s => s.UserProfileId)
                    .Distinct()
                    .ToList();

                return Ok(new
                {
                    VisitCount = visits.Count,
                    SeenCount = seen.Count,
                    VisitUserIds = visitUserIds,
                    SeenUserIds = seenUserIds
                });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetAllAdvertisementAgeRange")]
        public IActionResult GetAllAdvertisementAgeRange()
        {
            var AdvertisementAgeRanges = _context.AdvertisementAgeRangeAgeRanges
               .Select(x => new {
                   x.AgeRangeId,
                   x.AgeRange
               }).ToList();

            return Ok(AdvertisementAgeRanges);
        }

        [HttpPost (Name = "AddAdvertisement")]
        public async Task<IActionResult> AddAdvertisement(long userprofileid ,  [FromForm] AddAdvertisementDTO AdvertisementDTO)
        {
            using (var Transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // TargetUserProfileId"ouner of advertisement"
                    #region validation
                    var user = _UserFunctions.CheckUserProfileExists(userprofileid);
                    if (user == null)
                        return BadRequest("لم يتم العثور على المستخم الذى ادخلتة !");

                    var Targetuser = _context.Users.Where(x => x.Mobile == AdvertisementDTO.TargetUserPhone).FirstOrDefault();
                    if (Targetuser == null)
                        return BadRequest("لم يتم العثور على صاحب الاعلان الذى ادخلتة !");

                    var TargetUserProfileid = _context.UserProfiles.Where(x => x.UserId == Targetuser.UserId && x.RoleId == AdvertisementDTO.TargetUserRoleId).FirstOrDefault();
                    if (TargetUserProfileid == null)
                        return BadRequest("هذا المستخدم لم يقم بانشاء حساب بهذة الصلاحية ! ");


                    #endregion

                    #region add advertisement
                    var newAdvertisement = new Advertisement
                    {
                        AdvertisementTitle = AdvertisementDTO.AdvertisementTitle,
                        AdvertisementText = AdvertisementDTO.AdvertisementText,
                        UserProfileId = userprofileid,
                        StartDate = AdvertisementDTO.StartDate,
                        EndDate = AdvertisementDTO.EndDate,
                        TargetSite = AdvertisementDTO.TargetSite,
                        IsMale = AdvertisementDTO.IsMale,
                        IsActive = AdvertisementDTO.IsActive,
                        TargetUserProfileId = TargetUserProfileid.UserProfileId
                    };

                    #endregion
                  
                    #region Image 
                    string? newimagename = null;
                    if (AdvertisementDTO.AdvertisementImage != null)
                    {
                        newimagename = _ImageFunctions.ChangeImageName(AdvertisementDTO.AdvertisementImage);
                        var path = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Advertisements\" + newimagename;
                        //$@"h:\root\home\atmbank-001\www\site1\wwwroot\"

                        var uploadImageToserver = _ImageFunctions.UploadImageToServerAsync(AdvertisementDTO.AdvertisementImage, path);
                        if (uploadImageToserver == null) return BadRequest("!حدث خطا اثناء رفع الصورة");

                        newAdvertisement.AdvertisementFileName = newimagename;
                        newAdvertisement.AdvertisementUrlfullPath = $"http://mobile.hawisports.com/image/Advertisements/{newimagename}";
                    }

                    if (AdvertisementDTO.TargetUserLogo != null)
                    {
                        newimagename = _ImageFunctions.ChangeImageName(AdvertisementDTO.TargetUserLogo);
                        var path = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Advertisements\" + newimagename;

                        var uploadLogoToserver = _ImageFunctions.UploadImageToServerAsync(AdvertisementDTO.TargetUserLogo, path);
                        if (uploadLogoToserver == null) return BadRequest("!حدث خطا اثناء رفع الشعار");

                        newAdvertisement.TargetUserLogoFileName = newimagename;
                        newAdvertisement.TargetUserLogoUrlfullPath = $"http://mobile.hawisports.com/image/Advertisements/{newimagename}";
                    }
                    _context.Advertisements.Add(newAdvertisement);
                    _context.SaveChanges();
                    #endregion
                   
                    #region add ageRange & City 
                    if (AdvertisementDTO.AgeRangeId != null && AdvertisementDTO.AgeRangeId.Any())
                    {
                        foreach (var ageRangeId in AdvertisementDTO.AgeRangeId)
                        {
                            var newAdvertisementAgeRange = new AdvertisementAgeRange
                            {
                                AdvertisementId = newAdvertisement.AdvertisementId,
                                AgeRangeId = ageRangeId,
                            };

                            _context.AdvertisementAgeRanges.Add(newAdvertisementAgeRange);
                        }

                        _context.SaveChanges();
                    }

                    if (AdvertisementDTO.CityId != null && AdvertisementDTO.CityId.Any())
                    {
                        foreach (var CityId in AdvertisementDTO.CityId)
                        {
                            var newAdvertisementCity = new AdvertisementCity
                            {
                                AdvertisementId = newAdvertisement.AdvertisementId,
                                CityId = CityId,

                            };

                            _context.AdvertisementCities.Add(newAdvertisementCity);
                        }
                        _context.SaveChanges(); 
                    }
                    Transaction.Commit();
                    return Ok("تم اضافة الاعلان بنجاح");
                    #endregion
                }
                catch (Exception ex)
                {
                    Transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost(Name = "AdvertisementSeen")]
        public IActionResult AdvertisementSeen(long AdvertisementId,long userprofileid)
        {
            var selectAdvertisement = _context.Advertisements.Where(x => x.AdvertisementId == AdvertisementId).FirstOrDefault();
            if (selectAdvertisement == null)    return NotFound("لم يتم العثور على الاعلان!");

            var newAdvertisementSeen = new AdvertisementSeen
            {
                AdvertisementId = AdvertisementId,
                UserProfileId = userprofileid
            };
            _context.AdvertisementSeens.Add(newAdvertisementSeen);
            _context.SaveChanges();
            return Ok("تمت اضافة المشاهدة للاعلان ");
        }

        [HttpPost(Name = "AdvertisementVisit")]
        public IActionResult AdvertisementVisit(long AdvertisementId, long userprofileid)
        {
            try
            {
                var selectAdvertisement = _context.Advertisements.Where(x => x.AdvertisementId == AdvertisementId).FirstOrDefault();
                if (selectAdvertisement == null) return NotFound("لم يتم العثور على الاعلان!");

                var newAdvertisementVisit = new AdvertisementVisit
                {
                    AdvertisementId = AdvertisementId,
                    UserProfileId = userprofileid
                };
                _context.AdvertisementVisits.Add(newAdvertisementVisit);
                _context.SaveChanges();
                return Ok("تمت اضافة الدخول على الاعلان ");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(Name = "ActivateAdvertisement")]
        public IActionResult ActivateAdvertisement(long AdvertisementId, long userprofileid)
        {
            try
            {
                var selectAdvertisement = _context.Advertisements.Where(x => x.AdvertisementId == AdvertisementId).FirstOrDefault();
                if (selectAdvertisement == null) return NotFound("لم يتم العثور على الاعلان!");

                if (selectAdvertisement.IsActive == true)
                    selectAdvertisement.IsActive = false;
                else
                    selectAdvertisement.IsActive = true;

                selectAdvertisement.UserProfileId = userprofileid;
                _context.Advertisements.Update(selectAdvertisement);
                _context.SaveChanges();
                return Ok("تم التحديث بنجاح بنجاح");
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
         
        [HttpPut(Name = "UpdateAdvertisement")]
        public async Task<IActionResult> UpdateAdvertisement(long advertisementId, [FromForm] UpdateAdvertisementDTO updateAdvertisementDTO)
        {
            try
            {
                #region validation
                var advertisement = _context.Advertisements
                    .Include(a => a.AdvertisementAgeRanges)
                    .Include(a => a.AdvertisementCities)
                    .FirstOrDefault(a => a.AdvertisementId == advertisementId);
               
                if (advertisement == null)  return NotFound("لم يتم العثور على الاعلان!");
                #endregion

                #region Image
            
                string? newimagename = null;
                if (updateAdvertisementDTO.AdvertisementImage != null)
                {
                    var oldpath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Advertisements\" + advertisement.AdvertisementFileName;
                    if (System.IO.File.Exists(oldpath))
                         System.IO.File.Delete(oldpath);
              
                    newimagename = _ImageFunctions.ChangeImageName(updateAdvertisementDTO.AdvertisementImage);
                    var path = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Advertisements\" + advertisement.AdvertisementFileName;
                   
                    var uploadImageToserver = _ImageFunctions.UploadImageToServerAsync(updateAdvertisementDTO.AdvertisementImage, path);
                    if (uploadImageToserver != null) return BadRequest("!حدث خطا اثناء رفع الصورة");

                    advertisement.AdvertisementFileName = newimagename;
                    advertisement.AdvertisementUrlfullPath = $"http://mobile.hawisports.com/image/Advertisements/{newimagename}";
                }
                #endregion

                #region update advertisement and age,sity range
                advertisement.AdvertisementTitle = updateAdvertisementDTO.AdvertisementTitle;
                advertisement.AdvertisementText = updateAdvertisementDTO.AdvertisementText;
                advertisement.StartDate = updateAdvertisementDTO.StartDate.Value;
                advertisement.EndDate = updateAdvertisementDTO.EndDate.Value;
                advertisement.TargetSite = updateAdvertisementDTO.TargetSite;
                advertisement.IsMale = updateAdvertisementDTO.IsMale;
                advertisement.IsActive = updateAdvertisementDTO.IsActive;

                _context.AdvertisementAgeRanges.RemoveRange(advertisement.AdvertisementAgeRanges);
                _context.AdvertisementCities.RemoveRange(advertisement.AdvertisementCities);

                if (updateAdvertisementDTO.AgeRangeId != null && updateAdvertisementDTO.AgeRangeId.Any())
                {
                    foreach (var ageRangeId in updateAdvertisementDTO.AgeRangeId)
                    {
                        var newAdvertisementAgeRange = new AdvertisementAgeRange
                        {
                            AdvertisementId = advertisement.AdvertisementId,
                            AgeRangeId = ageRangeId,
                        };

                        _context.AdvertisementAgeRanges.Add(newAdvertisementAgeRange);
                    }
                }

                if (updateAdvertisementDTO.CityId != null && updateAdvertisementDTO.CityId.Any())
                {
                    foreach (var cityId in updateAdvertisementDTO.CityId)
                    {
                        var newAdvertisementCity = new AdvertisementCity
                        {
                            AdvertisementId = advertisement.AdvertisementId,
                            CityId = cityId,
                        };

                        _context.AdvertisementCities.Add(newAdvertisementCity);
                    }
                }

                _context.SaveChanges();

                return Ok("تم تحديث الإعلان بنجاح");
                #endregion
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(Name = "DeleteAdvertisement")]
        public IActionResult DeleteAdvertisement(long AdvertisementId)
        {
            try
            {
                var selectAdvertisement = _context.Advertisements.Where(x => x.AdvertisementId == AdvertisementId).FirstOrDefault();
                if (selectAdvertisement == null)
                    return NotFound("!لم يتم العثور على الاعلان");

                var imagePath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Advertisements\" + selectAdvertisement.AdvertisementFileName;
                var targetuserlogo = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Advertisements\" + selectAdvertisement.TargetUserLogoFileName;
                _context.Advertisements.Remove(selectAdvertisement);
               
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
                
                if (System.IO.File.Exists(targetuserlogo))
                    System.IO.File.Delete(targetuserlogo);
                _context.SaveChanges();
                return Ok("تم حذف الاعلان بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

