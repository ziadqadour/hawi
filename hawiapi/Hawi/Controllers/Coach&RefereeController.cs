using Hawi.Dtos;
using Hawi.Extensions;
using Hawi.Models;
using Hawi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twilio.TwiML.Messaging;
using UnidecodeSharpFork;
using static System.Net.Mime.MediaTypeNames;

namespace Hawi.Controllers
{
    [Route("hawi/CoachReferee/[action]")]
    [ApiController]
    public class Coach_RefereeController : ControllerBase
    {
        private readonly HawiContext _context ;
        private readonly ImageFunctions _ImageFunctions ;
        private readonly UserFunctions _UserFunctions;
        private readonly INotificationService _notificationService;
        private readonly Notification _notification;
        public Coach_RefereeController(UserFunctions userFunctions, INotificationService notificationService, Notification notification, HawiContext context ,ImageFunctions imageFunctions)
        {
            _UserFunctions = userFunctions;
            _notificationService = notificationService;
            _notification = notification;
            _context = context;
            _ImageFunctions = imageFunctions;
        }

       
        [HttpGet(Name = "GetAffiliationType")]
        public IActionResult GetAffiliationType()
        {
            var AffiliationType = _context.PendingAffiliationListPendingAffiliationListTypes
                .Select(x => new
                {
                    x.PendingAffiliationListTypeId,
                    x.PendingAffiliationListType,

                }).ToList();
            return Ok(AffiliationType);
        }

        [HttpGet(Name = "GetAffiliationStatus")]
        public IActionResult GetAffiliationStatus()
        {
            var AffiliationStatus = _context.PendingStatuses
                .Select(x => new
                {
                    x.PendingAffiliationStatusId,
                    PendingAffiliationStatus1=x.PendingAffiliationStatus,

                }).ToList();
            return Ok(AffiliationStatus);
        }

        [HttpGet(Name = "GetPendingCoach")]
        public IActionResult GetPendingCoach()
        {
            var PendingCoach = _context.PendingAffiliationLists
                .Where(x => x.PendingAffiliationListTypeId == 2 && x.PendingAffiliationStatusId==1 )
                .Include(x => x.UserProfile)
                .Include(x => x.PendingAffiliationListType)
                .ToList();
           
            var selectPendingCoach = PendingCoach.Select(x => new
            {
                PendingAffiliationListId = x?.PendingAffiliationListId,
                UserProfileId = x?.UserProfileId,
                x?.PendingAffiliationListTypeId,
                PendingAffiliationListTyp = x?.PendingAffiliationListType?.PendingAffiliationListType,
                name = _UserFunctions.GetUserName(x.UserProfile.RoleId, x.UserProfileId, x.UserProfile.UserId),
                Image = _UserFunctions.GetUserImage(x.UserProfileId, x.UserProfile.RoleId),
                x?.CreateDate,
                x?.CertificateImageUrl,
                PendingAffiliationStatus = _context.PendingStatuses
                    .FirstOrDefault(p => p.PendingAffiliationStatusId == x.PendingAffiliationStatusId)
                    ?.PendingAffiliationStatus ?? null,
                PendingAffiliationStatusReason = x?.PendingAffiliationStatusReason ?? null
            });
            return Ok(selectPendingCoach);
        }

        [HttpGet(Name = "GetPendingReferee")]
        public IActionResult GetPendingReferee()
        {
            try
            {
                var PendingReferee = _context.PendingAffiliationLists
                    .Where(x => x.PendingAffiliationListTypeId == 1 && x.PendingAffiliationStatusId == 1)
                    .Include(x => x.UserProfile)
                    .Include(x => x.PendingAffiliationListType)
                    .ToList();
            
                var selectPendingReferee = PendingReferee.Select(x => new
                {
                    PendingAffiliationListId = x?.PendingAffiliationListId,
                    UserProfileId = x?.UserProfileId,
                    x?.PendingAffiliationListTypeId,
                    PendingAffiliationListTyp = x?.PendingAffiliationListType?.PendingAffiliationListType,
                    name = _UserFunctions.GetUserName(x.UserProfile.RoleId, x.UserProfileId, x.UserProfile.UserId),
                    Image = _UserFunctions.GetUserImage(x.UserProfileId, x.UserProfile.RoleId),
                    x?.CreateDate,
                    x?.CertificateImageUrl,
                    PendingAffiliationStatus = _context.PendingStatuses
                    .FirstOrDefault(p => p.PendingAffiliationStatusId == x.PendingAffiliationStatusId)
                    ?.PendingAffiliationStatus ?? null,
                    PendingAffiliationStatusReason = x?.PendingAffiliationStatusReason ?? null
                }).ToList();
                return Ok(selectPendingReferee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetSpeceficUserPendingAffiliationByID")]
        public IActionResult GetSpeceficUserPendingAffiliationByID(long userprofileId)
        {
            try
            {
                var userprofile = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == userprofileId);
                if (userprofile == null)  return NotFound("!لم يتم العثور على المستخدم!");

                var Pending = _context.PendingAffiliationLists
                    .Where(x => x.UserProfileId == userprofileId)
                    .Include(x => x.UserProfile)
                    .Include(x => x.PendingAffiliationListType)
                    .ToList();

                var selectPending = Pending.Select(x => new
                {
                    PendingAffiliationListId = x?.PendingAffiliationListId,
                    UserProfileId = x?.UserProfileId,
                    x?.PendingAffiliationListTypeId,
                    PendingAffiliationListTyp = x?.PendingAffiliationListType?.PendingAffiliationListType,
                    name =  _UserFunctions.GetUserName(x.UserProfile.RoleId, x.UserProfileId, x.UserProfile.UserId),
                    Image = _UserFunctions.GetUserImage(x.UserProfileId, x.UserProfile.RoleId),
                    x?.CreateDate,
                    x?.CertificateImageUrl,
                    PendingAffiliationStatus = _context.PendingStatuses
                        .FirstOrDefault(p => p.PendingAffiliationStatusId == x.PendingAffiliationStatusId)
                        ?.PendingAffiliationStatus ?? null,
                    PendingAffiliationStatusReason = x?.PendingAffiliationStatusReason ?? null
                }).ToList();
                return Ok(selectPending);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetRefereeRequest")]
        public IActionResult GetRefereeRequest(long? userprofileid = 0)
        {
            //دى بترجع كل طلبات الحكام وكمان بترجع ,لو دخلت باراميتر بترجعلك كل طلبات الاكاديمية دى للحكام 
            //كل الماتشات اللى عايزة حكام
            List<MatchMatchRefereeRequest>? AllRefereeRequests = new List<MatchMatchRefereeRequest>();

            if (userprofileid.HasValue && userprofileid.Value != 0)
            {
                AllRefereeRequests = _context.MatchMatchRefereeRequests
                .Where(x => x.UserProfileId == userprofileid)
                .Select(x => new MatchMatchRefereeRequest
                {
                    MatchRefereeRequestId = x.MatchRefereeRequestId,
                    UserProfileId = x.UserProfileId,
                    MatchDate = x.MatchDate,
                    Place = x.Place,
                    Description = x.Description,
                    EstimatedCost = x.EstimatedCost ?? null,
                    NumberOfReferee = x.NumberOfReferee ?? null,
                    CreateDate = x.CreateDate,
                    IsActive = x.IsActive,
                    MatchId = x.MatchId,
                }).ToList();
            }

            AllRefereeRequests = _context.MatchMatchRefereeRequests
               .Where(x => x.IsActive == true)
               .Select(x => new MatchMatchRefereeRequest
               {
                   MatchRefereeRequestId = x.MatchRefereeRequestId,
                   UserProfileId = x.UserProfileId,
                   MatchDate = x.MatchDate,
                   Place = x.Place,
                   Description = x.Description,
                   EstimatedCost = x.EstimatedCost ?? null,
                   NumberOfReferee = x.NumberOfReferee ?? null,
                   CreateDate = x.CreateDate,

               }).ToList();
            return Ok(AllRefereeRequests);
        }

        [HttpGet(Name = "GetActiveRefereeRequestOfSpeceficSportinstituation")]
        public IActionResult GetActiveRefereeRequestOfSpeceficSportinstituation(long userprofileid)
        {
            var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == userprofileid).FirstOrDefault();
            if (userprofile == null)   return NotFound("!لم يتم العثور على هذه الاكاديمية!");

            var AllRefereeRequests = _context.MatchMatchRefereeRequests
                 .Where(x => x.IsActive == true)
                 .Select(x => new MatchMatchRefereeRequest
                 {
                     MatchRefereeRequestId = x.MatchRefereeRequestId,
                     UserProfileId = x.UserProfileId,
                     MatchDate = x.MatchDate,
                     Place = x.Place,
                     Description = x.Description,
                     EstimatedCost = x.EstimatedCost ?? null,
                     NumberOfReferee = x.NumberOfReferee ?? null,
                     CreateDate = x.CreateDate,

                 }).ToList();
            return Ok(AllRefereeRequests);
        }

        [HttpGet(Name = "GetAllRefereeCandidateForfereeRequest")]
        public IActionResult GetAllRefereeCandidateForfereeRequest(long MatchRefereeRequestId)
        {
            //الحكام اللى مقدمين على ماتش
            var selectedRefereeCandidate = _context.MatchMatchRefereeCandidates.Where(x => x.MatchRefereeRequestsId == MatchRefereeRequestId).FirstOrDefault();
            if (selectedRefereeCandidate == null) return NotFound("!لم يتم العثور على طلبك للتحكيم لهذا الماتش");

            var RefereeCandidates = _context.MatchMatchRefereeCandidates.Where(x => x.MatchRefereeRequestsId == MatchRefereeRequestId).ToList();
            var numOfCandidata = RefereeCandidates.Count();

            var DataOfRefereeCandirate = RefereeCandidates.Select(x => new
            {
                x.UserProfileId,
                x.MatchRefereeRequestsId,
                Image = _UserFunctions.GetUserImage((long)x.UserProfileId, x.UserProfile.RoleId),
                phone = _context.UserProfiles?.Where(x => x.UserProfileId == x.UserProfileId).Select(c => c.User.Mobile),
                x.MatchRefereeCandidateId,
                x.Comment,
                x.CreateDate,

            }).ToList();
            return Ok(DataOfRefereeCandirate);
        }

        [HttpGet(Name = "SearchForRefaree")]
        public async Task <IActionResult> SearchForRefaree(string Key)
        {
            var SelectedResult=await _context.UserProfiles.Where(x=>(x.User.Mobile==Key || x.User.Name== Key) && x.RoleId==3)
                .Select(x => new
                {
                    x.UserProfileId,
                    x.User.Name,
                })
              .FirstOrDefaultAsync();
            return Ok(SelectedResult);
        }
        
        //تقديم طلب انتساب
        [HttpPost (Name = "PendingAffiliationAsCoach_Referee")]
        public async Task<IActionResult> PendingAffiliationAsCoach_RefereeAsync(long UserProfileId, [FromForm] AddPendingAffiliationDto pendingAffiliationDto)
        {
            try
            {
                var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileId).FirstOrDefault();
                if (userprofile == null)  return NotFound("!لم يتم العثور على هذا المستخدم");

                //referee
                if (pendingAffiliationDto.PendingAffiliationListTypeId == 1)//referee
                {
                    var checkIfHaveThatrole = _context.UserProfiles.Where(x => x.UserId == userprofile.UserId && x.RoleId == 3).FirstOrDefault();
                    if (checkIfHaveThatrole != null) return BadRequest("!لديك حساب ل حكم بالفعل");
                }
               if (pendingAffiliationDto.PendingAffiliationListTypeId==2)//coach
                {
                    var checkIfHaveThatrole = _context.UserProfiles.Where(x => x.UserId == userprofile.UserId && x.RoleId == 4).FirstOrDefault();
                    if (checkIfHaveThatrole != null) return BadRequest("!لديك حساب ل مدرب بالفعل");
                }

                var PendingAffiliation = _context.PendingAffiliationLists.Where(x => x.UserProfileId == UserProfileId && x.IsActive==true).FirstOrDefault();
                if (PendingAffiliation != null)  return BadRequest("!قد تم رفع طلب انتساب وفى انتظار الرد علية ");


                var checfile = _ImageFunctions.ValidateFile(pendingAffiliationDto.CertificateImageUrl);
                if (checfile != null) return BadRequest(checfile);

                string englishFileName = _ImageFunctions.ChangeImageName(pendingAffiliationDto.CertificateImageUrl);

                var photopath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\PendingAffiliation\" + englishFileName;
                using (var stream = new FileStream(photopath, FileMode.Create))
                {
                    await pendingAffiliationDto.CertificateImageUrl.CopyToAsync(stream);
                }
                var newPath = $"http://mobile.hawisports.com/image/PendingAffiliation/" + englishFileName;

                var newPendingAffiliationList = new PendingAffiliationList
                {
                    UserProfileId = UserProfileId,
                    PendingAffiliationListTypeId = pendingAffiliationDto.PendingAffiliationListTypeId,
                    CertificateImageUrl = newPath,
                    PendingAffiliationStatusId=1,
                    IsActive = true
                };

                _context.PendingAffiliationLists.Add(newPendingAffiliationList);
                _context.SaveChanges();
                return Ok("تم ارسال الطلب بنجاح");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //accept or reject Affiliation شىي سثىي ىخفهبهؤشفهخى
        [HttpPost(Name = "AffiliationStatusDetermination")]
        public async Task< IActionResult> AffiliationStatusDetermination(long AdminuserProfileId, long PendingAffiliationId, [FromBody] AffiliationStatusDeterminationDTO affiliation)
        { 
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (affiliation.PendingAffiliationStatusId == 1) return BadRequest("!يجب القبول او الرفض على هذا الطلب ");

                    var adminuserprofile = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == AdminuserProfileId);
                    if (adminuserprofile == null)  return NotFound("لم يتم العثور على هذا الادمن!");

                    var PendingAffiliation = _context.PendingAffiliationLists.FirstOrDefault(x => x.PendingAffiliationListId == PendingAffiliationId);
                    if (PendingAffiliation == null)  return NotFound("لم يتم العثور على طلب الانتساب!");

                    long ifAcceptedPendingAffiliation = 0;

                    // Add new role
                    if (affiliation.PendingAffiliationStatusId == 3)
                    {
                        var userid = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == PendingAffiliation.UserProfileId)?.UserId;
                        var roleid = (PendingAffiliation.PendingAffiliationListTypeId == 1) ? 3 : 4;

                        var userprofile = new UserProfile
                        {
                            UserId = (long)userid,
                            RoleId = (byte)roleid,
                            IsActive = true,
                        };

                        _context.UserProfiles.Add(userprofile);
                        _context.SaveChanges();
                        ifAcceptedPendingAffiliation = userprofile.UserProfileId;
                    }

                    // Update status of PendingAffiliation
                    PendingAffiliation.IsActive = false;
                    PendingAffiliation.PendingAffiliationStatusReason = affiliation.PendingAffiliationStatusReason;
                    PendingAffiliation.PendingAffiliationStatusId = affiliation.PendingAffiliationStatusId;
                    _context.PendingAffiliationLists.Update(PendingAffiliation);
                    _context.SaveChanges();

                    // Add notification
                    var UserDataForNotification = await _UserFunctions.GetUserDataForNotificationByProfileId(PendingAffiliation.UserProfileId, AdminuserProfileId);
                   
                    string message = null;
                       var Pendingtype = _context.PendingAffiliationListPendingAffiliationListTypes?
                         .FirstOrDefault(x => x.PendingAffiliationListTypeId == PendingAffiliation.PendingAffiliationListTypeId)?.PendingAffiliationListType;
                       
                        if (affiliation.PendingAffiliationStatusId == 3) message = $" تم قبول طلبك للانتساب ك {Pendingtype} ";
                        else if (affiliation.PendingAffiliationStatusId == 2) message = $" تم رفض طلبك للانتساب ك {Pendingtype} ";


                        var newNotification = new RealTimeNotification
                        {
                            FromUserProfileId = AdminuserProfileId,
                            ToUserProfileId = PendingAffiliation.UserProfileId,
                            TargetTypeId = 3,
                            TargetId = (ifAcceptedPendingAffiliation == 0) ? PendingAffiliation.UserProfileId : ifAcceptedPendingAffiliation,
                            IsRead = false,
                            ContentMessage = message,
                        };

                    _context.RealTimeNotifications.Add(newNotification);

                    //sendNotification
                    var UserProfileThatInvetionenSendToIt = _context.UserProfiles.Where(x => x.UserProfileId == PendingAffiliation.UserProfileId).FirstOrDefault();
                   // var UserDataForNotification = await _UserFunctions.GetUserDataForNotificationByProfileId(PendingAffiliation.UserProfileId, AdminuserProfileId);
                    var newRealTimeNotification =await _notification.CreateRealTimeNotificationModel(UserDataForNotification, message, UserProfileThatInvetionenSendToIt);
                    await _notificationService.SendNotification(newRealTimeNotification);

                    _context.SaveChanges();
                    transaction.Commit();

                    return Ok("تمت العملية بنجاح");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        //طلب حكام
        [HttpPost (Name = "RefereeRequest")]
        public IActionResult RefereeRequest (long UserProfileId, [FromBody] RefereeRequestDTO referee)
        {
            try
            {
                 //صاحب الاكاديمية
                var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileId).FirstOrDefault();
                if (userprofile == null)  return NotFound("لم يتم العثور على هذا المستخدم!");

                if(referee.MatchId!=0) 
                {
                    var match=_context.Matches.Where(x=>x.MatchId==referee.MatchId).FirstOrDefault();
                    if (match == null) return NotFound("لم يتم العثور على الماتش!");
                }

                var checkrefereerequestifExist=_context.MatchMatchRefereeRequests.Where(x=>x.MatchId == referee.MatchId).FirstOrDefault(); 
                if (checkrefereerequestifExist!=null) return BadRequest ("!تم طلب حكام لهذا الماتش من قبل ");

                var newMatchMatchRefereeRequest = new MatchMatchRefereeRequest
                {
                    UserProfileId= UserProfileId,
                    MatchDate=referee.MatchDate,
                    Place=referee.Place,
                    Description=referee.Description,
                    IsActive=true,
                };

                if(referee.EstimatedCost!=0  && referee.EstimatedCost != null)
                    newMatchMatchRefereeRequest.EstimatedCost = referee.EstimatedCost;

                if (referee.NumberOfReferee != 0 && referee.NumberOfReferee != null)
                    newMatchMatchRefereeRequest.NumberOfReferee = referee.NumberOfReferee;

                if(referee.MatchId!=0 && referee.NumberOfReferee!=null)
                    newMatchMatchRefereeRequest.MatchId = referee.MatchId;


                _context.MatchMatchRefereeRequests.Add (newMatchMatchRefereeRequest);
                _context.SaveChanges();


                //send notification to all refree in same city
                //if (newMatchMatchRefereeRequest.MatchId!=null && newMatchMatchRefereeRequest.MatchId != 0)
                //{
                //    var match = _context.Matches.Where(x => x.MatchId == referee.MatchId).FirstOrDefault();
                //    //var cityId = match.MatchCityId;

                //    if (cityId != null)
                //    {
                //        var userProfilesInSameCity = _context.UserProfiles
                //            .Where(profile => profile.RoleId == 3 && profile.User.CityId == cityId)
                //            .ToList();

                //        var message = $"يوجد مباراة جديدة تحتاج لحكم في مدينتك. يُرجى الاطلاع على الطلبات المتاحة واختيار المباراة التي ترغب في تحكيمها.";

                //        foreach (var profile in userProfilesInSameCity)
                //        {
                //            var newNotification = new RealTimeNotification
                //            {
                //                //من الشخص اللى مكريت الماتش بس مش هيكون تارجت
                //                FromUserProfileId = match.UserProfileId,
                //                ToUserProfileId = profile.UserProfileId,
                //                TargetTypeId = 4,
                //                TargetId = referee.MatchId,
                //                IsRead = false,
                //                ContentMessage = message,

                //            };
                //            _context.RealTimeNotifications.Add(newNotification);
                //            _context.SaveChanges();
                //        }
                //    }
                //}
            
                return Ok("that endpoint not finish yet");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
   
        //تقديم الحكام عالماتش
        [HttpPost(Name = "AddRefereeCandidate")]
        public IActionResult AddRefereeCandidate(long MatchId, long UserProfileId, [FromBody] RefereeCandidateDTO candidateDTO)
        {//تقديم طلب انة يحكم الماتش
            try
            {
                var MatchMatchRefereeRequest = _context.MatchMatchRefereeRequests.Where(x => x.MatchId == MatchId).FirstOrDefault();
                if (MatchMatchRefereeRequest == null) return NotFound("لم يتم العثور على طلب حكام لهذا الماتش  !");

                var checkifcandidateornotbefore=_context.MatchMatchRefereeCandidates.Where(x=>x.MatchId==MatchId && x.UserProfileId== UserProfileId).FirstOrDefault();
                if (checkifcandidateornotbefore != null) return Ok("!لقد قمت بتقديم طلب تحكيم لهذا الماتش من قبل");

                var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileId).FirstOrDefault();
                if (userprofile == null) return NotFound("لم يتم العثور على هذا المستخدم!");

                if (userprofile.RoleId != 3) return BadRequest("يجب عليك الانتساب كحكم اولا !");

                var newcandidateDTO = new MatchMatchRefereeCandidate
                {
                    MatchRefereeRequestsId = MatchMatchRefereeRequest.MatchRefereeRequestId,
                    UserProfileId = UserProfileId,
                    Comment = candidateDTO.Comment,
                    MatchId= MatchMatchRefereeRequest.MatchId
                };

                _context.MatchMatchRefereeCandidates.Add(newcandidateDTO);
                _context.SaveChanges();
                return Ok("تم التقديم بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(Name = "UpdateRefereeRequest")]
        public IActionResult UpdateRefereeRequest(long UserProfileId, long MatchRefereeRequestId, [FromBody] RefereeRequestDTO referee)
        {
            try
            {
                var MatchMatchRefereeRequest = _context.MatchMatchRefereeRequests.Where(x => x.MatchRefereeRequestId == MatchRefereeRequestId).FirstOrDefault();
                if (MatchMatchRefereeRequest == null) return NotFound("لم يتم العثور على طلب الحكام لهذا الماتش  !");

                var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileId).FirstOrDefault();
                if (userprofile == null) return NotFound("لم يتم العثور على هذا المستخدم!");

                UserProfileId = UserProfileId;
                MatchMatchRefereeRequest.MatchDate = referee.MatchDate;
                MatchMatchRefereeRequest.Place = referee.Place;
                MatchMatchRefereeRequest.Description = referee.Description;
                MatchMatchRefereeRequest.IsActive = true;

                if (referee.EstimatedCost != 0 && referee.EstimatedCost.HasValue)
                    MatchMatchRefereeRequest.EstimatedCost = referee.EstimatedCost;

                if (referee.NumberOfReferee != 0 && referee.NumberOfReferee.HasValue)
                    MatchMatchRefereeRequest.NumberOfReferee = referee.NumberOfReferee;

                _context.MatchMatchRefereeRequests.Update(MatchMatchRefereeRequest);
                _context.SaveChanges();
                return Ok("تم التعديل بنجاح");
            }
            catch(Exception EX)
            {
                return BadRequest(EX.Message);  
            }
        }

        [HttpPut (Name = "DisActive_ActiveRefereeRequest")]
        public IActionResult DisActive_ActiveRefereeRequest(long userprofileid, long MatchRefereeRequestId)
        {
            try
            {
                var MatchMatchRefereeRequest = _context.MatchMatchRefereeRequests.Where(x => x.MatchRefereeRequestId == MatchRefereeRequestId).FirstOrDefault();
                if (MatchMatchRefereeRequest == null) return NotFound("لم يتم العثور على طلب الحكام لهذا الماتش  !");

                var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == userprofileid).FirstOrDefault();
                if (userprofile == null) return NotFound("لم يتم العثور على هذا المستخدم!");

                if (MatchMatchRefereeRequest.UserProfileId != userprofileid || userprofile.RoleId != 6) return BadRequest("لا يمكن تنفيذ طلبك!");

                if (MatchMatchRefereeRequest.IsActive == false)
                    MatchMatchRefereeRequest.IsActive = true;
                else if(MatchMatchRefereeRequest.IsActive == true)
                    MatchMatchRefereeRequest.IsActive = false;

                _context.MatchMatchRefereeRequests.Update(MatchMatchRefereeRequest);
                _context.SaveChanges();
                return Ok("تم تنفيذ طلبك بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(Name = "DeleteRefereeCandidate")]
        public IActionResult DeleteRefereeCandidate(long MatchRefereeRequestId, long UserProfileId)
        {//الحكم يمسح طلب تقديمة عالماتش
            try
            {
                var selectedRefereeCandidate = _context.MatchMatchRefereeCandidates.Where(x => x.UserProfileId == UserProfileId && x.MatchRefereeRequestsId == MatchRefereeRequestId).FirstOrDefault();
                if (selectedRefereeCandidate == null) return NotFound("!لم يتم العثور على طلبك للتحكيم لهذا الماتش");

                _context.MatchMatchRefereeCandidates.Remove(selectedRefereeCandidate);
                _context.SaveChanges();
                return Ok("تم الحذف بنجاح");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(Name = "DeleteRefereeRequest")]
        public IActionResult DeleteRefereeRequest(long userprofileid, long MatchRefereeRequestId)
        {// يمسح الطلب اللى عايز فية حكام
            try
            {
                var MatchMatchRefereeRequest = _context.MatchMatchRefereeRequests.Where(x => x.MatchRefereeRequestId == MatchRefereeRequestId).FirstOrDefault();
                if (MatchMatchRefereeRequest == null)  return NotFound("!لم يتم العثور على طلب الحكام لهذا الماتش ");

                var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == userprofileid).FirstOrDefault();
                if (userprofile == null)  return NotFound("!لم يتم العثور على هذا المستخدم");

                if (MatchMatchRefereeRequest.UserProfileId != userprofileid || userprofile.RoleId != 6) return BadRequest("!لا يمكن تنفيذ طلبك");

                _context.MatchMatchRefereeRequests.Remove(MatchMatchRefereeRequest);
                _context.SaveChanges();
                return Ok("تم تنفيذ طلبك بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

