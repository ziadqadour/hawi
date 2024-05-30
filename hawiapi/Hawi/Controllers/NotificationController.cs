using Contracts;
using Hawi.Dtos;
using Hawi.Extensions;
using Hawi.Models;
using Hawi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Hawi.Controllers
{
    [Route("hawi/Notification/[action]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly HawiContext _context;
        private readonly ISMRepository _smsService;
        private readonly INotificationService _notificationService;
        private readonly UserFunctions _userFunctions;
        private readonly Notification _notification;

        public NotificationController(ISMRepository smsService, INotificationService notificationService, UserFunctions userFunctions, Notification notification, HawiContext context)
        {
            _smsService = smsService;
            _notificationService = notificationService;
            _userFunctions = userFunctions;
            _notification = notification;
            _context = context;
        }


        [HttpPost(Name = "sendNumOfNotification")]
        public async Task<IActionResult> sendNumOfNotification(NotificationModel notificationModel)
        {
            var result = await _notificationService.SendNotification(notificationModel);
            return Ok("send");
        }

        [HttpGet(Name = "SportInstitutionBelongBelongType")]
        public IActionResult SportInstitutionBelongBelongType()
        {
            try
            {
                var BelongBelongType = _context.SportInstitutionBelongBelongTypes
                    .Select(x => new
                    {
                        x.BelongTypeId,
                        x.BelongType,
                    })
                    .ToList();
                return Ok(BelongBelongType);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = ("SportInstitutionSearchUserByPhoneEmployee"))]
        public IActionResult SportInstitutionSearchUserByPhoneEmployee(string phone, byte roleid)
        {
            try
            {
                if (string.IsNullOrEmpty(phone)) return BadRequest("ادخل رقم هاتف!");

                var user = _context.Users.Where(x => x.Mobile == phone && x.IsActive == true).FirstOrDefault();
                if (user == null) return Ok();

                bool? RoleexistOrNot = true;
                byte? BelongTypeRoleMatchingToUserProfileRole = 0;

                // لاعب ادارى مدرب
                if (roleid == 1) BelongTypeRoleMatchingToUserProfileRole = 2;
                else if (roleid == 2) BelongTypeRoleMatchingToUserProfileRole = 4;
                else if (roleid == 3) BelongTypeRoleMatchingToUserProfileRole = 9;
                else
                    BelongTypeRoleMatchingToUserProfileRole = 1;


                var userprofile = _context.UserProfiles.Where(x => x.UserId == user.UserId && x.RoleId == BelongTypeRoleMatchingToUserProfileRole && x.IsActive == true).FirstOrDefault();

                if (userprofile == null)
                {
                    userprofile = _context.UserProfiles.Where(x => x.UserId == user.UserId && x.RoleId == 1 && x.IsActive == true).FirstOrDefault();
                    RoleexistOrNot = false;
                }

                var role = _context.Roles.FirstOrDefault(x => x.RoleId == userprofile.RoleId).Role1;
                var UserProfileImage = _userFunctions.GetUserImage(userprofile.UserProfileId, userprofile.RoleId);

                var resultOfSearch = new SearchUserDto
                {
                    Userprofileid = userprofile.UserProfileId,
                    Userrole = role,
                    RoleexistOrNot = RoleexistOrNot,
                    userimage = UserProfileImage,
                    UserName = user.Name,
                };
                return Ok(resultOfSearch);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetTargetType")]
        public IActionResult GetTargetType()
        {
            var target = _context.RealTimeNotificationTargetTypes
            .Select(x => new
            {
                x.RealTimeNotificationTargetTypeId,
                x.RealTimeNotificationTargetType1
            }).ToList();
            return Ok(target);
        }

        [HttpGet(Name = "GetSpeceficUserNotification")]
        public IActionResult GetSpeceficUserNotification(long UserProfileId, byte roleid)
        {
            var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileId).FirstOrDefault();
            if (userprofile == null) return NotFound("لم يتم العثور على هذا المستخدم !");

            var notification = _context.RealTimeNotifications.Where(x => x.ToUserProfileId == UserProfileId)
               .Select(x => new
               {
                   x.RealTimeNotificationId,
                   x.ToUserProfileId,
                   x.FromUserProfileId,
                   x.TargetId,
                   x.CreateDate,
                   x.ContentMessage,
                   x.TargetTypeId,
                   roleid = x.FromUserProfile.RoleId,
                   x.IsRead,
                   Image = _userFunctions.GetUserImage(x.FromUserProfileId, x.FromUserProfile.RoleId),
                   InvitationID = x.InvitationId,

               }).OrderByDescending(x => x.CreateDate)
               .ToList();
            return Ok(notification);
        }

        // دعوة للانضمام للتطبيق "لسة مفيش شركة نبعت من خلالاهاreal sms "
        [HttpPost(Name = "SendJoinSmsInvetiationTOSportInstitution")]
        public IActionResult SendJoinSmsInvetiationTOSportInstitution([FromBody] SendSmsInvetationDto Invetatio)
        {

            var msg1 = $"نحن نود أن يكون لديك دور في {Invetatio.SportInstituationName} كـ {Invetatio.JobTypeName}. انضم إلى تطبيق هاوى  واستفد من عالم  فرص الرياضة!";
            var msg2 = "قم بتنزيل التطبيق من هذا الرابط {http://mobile.hawisports.com/swagger/index.html}";
            var msg3 = "او قم قم بتسجيل الدخول من الويب {http://mobile.hawisports.com/swagger/index.html}.";

            var RandomCode = _smsService.CreateVerifyCode();
            var result = _smsService.Send("+201212058735", $"{msg1}");

            if (!string.IsNullOrEmpty(result.ErrorMessage))
                return BadRequest(result.ErrorMessage);

            return Ok("تم ارسال الدعوة بنجاح");
        }

        //دعوة انضمام شخص لاكاديمية
        [HttpPost(Name = "InvetionPendingToEmployee2")]
        public async Task<IActionResult> InvetionPendingToEmployee2(long pendingUserProfileId, long userProfileId, [FromBody] AddBendingEmployeeDto bendingEmployee)
        {
            try
            {

                #region  validation

                #region Validation IF Player

                var checkpendingUserProfileId = _context.UserProfiles.Where(x => x.UserProfileId == pendingUserProfileId).FirstOrDefault();
                if (checkpendingUserProfileId == null) return NotFound("لم يتم العثور على المستخدم المرسل الية الدعوة!");


                var branche = _context.SportInstitutionBranches.Where(x => x.SportInstitutionBranchId == bendingEmployee.SportInstitutionBranchId).FirstOrDefault();
                if (branche == null) return NotFound("لم يتم العثور على هذا الفرع!");

                var SType = _context.SportInstitutions.FirstOrDefault(x => x.SportInstitutionId == branche.SportInstitutionId).SportInstitutionTypeId;

                if (bendingEmployee.AgeCategories.Count == 0 && SType != 4) return BadRequest("!يجب ادخال فئات سنية ");

                if (bendingEmployee.BelongTypeId == 1 && SType != 4)
                {

                    if (bendingEmployee.AgeCategories.Count != 1) return BadRequest("!يجب اختيار فئه سنية واحدة فقط للاعب على الاقل");

                    var ChackDateOfBarth = _context.UserBasicData.Where(x => x.UserId == checkpendingUserProfileId.UserId).FirstOrDefault();
                    if (ChackDateOfBarth == null || ChackDateOfBarth.Dob == null) return NotFound("!يجب اولا على المستخدم الذى تود اضافتة ان يكمل بياناتة وادخال تاريخ ميلادة");

                    var checkAgeCategory = _context.SportInstitutionAgePriceAgeCategories.Where(x => x.AgeCategoryId == bendingEmployee.AgeCategories[0]).FirstOrDefault();
                    if (checkAgeCategory == null) return BadRequest("!لم يتم العثور على الفئه السنية الذى ادخلتها");

                    //لو اختار انة يكون فى الفريق الاساسى مش هيكون علية قيود للعمر19
                    if (bendingEmployee.AgeCategories[0] != 19)
                    {
                        if (SType == 1 || SType == 2)
                        {

                            var dateOfBirth = ChackDateOfBarth.Dob;
                            var currentDate = DateTime.Now;
                            var age = currentDate.Year - dateOfBirth.Value.Year;

                            // Check if the birthdate hasn't occurred this year yet
                            if (currentDate < dateOfBirth.Value.AddYears(age))
                                age--;
                            //  اللاعب ينفع ينضاف لمرحلة اعلى من سنة انما اقل من سنة لا
                            if (age > checkAgeCategory.Age)
                                return BadRequest("!الاعب الذى تود اضافتة اكبر من الفئة السنية التى تنوى اضافتة فيها");
                        }
                    }
                }

                #endregion

                var sendinguserprofile = _context.UserProfiles.Where(x => x.UserProfileId == userProfileId).FirstOrDefault();
                if (sendinguserprofile == null) return NotFound("لم يتم العثور على الراسل!");

                var invetationBelonging = _context.SportInstitutionBelongs.Where(x => x.BelongUserProfileId == pendingUserProfileId
                            && x.BelongTypeId == bendingEmployee.BelongTypeId && x.SportInstitutionBranchId == bendingEmployee.SportInstitutionBranchId).FirstOrDefault();
                if (invetationBelonging != null) return BadRequest("!هذا المستخدم موجود بالفعل ");

                #endregion

                #region if user already in pending in database "send notification only"

                var invetitationpending = _context.SportInstitutionBelongPendings.Where(x => x.BelongUserProfileId == pendingUserProfileId
                      && (x.BelongTypeId == bendingEmployee.BelongTypeId) && x.SportInstitutionBranchId == bendingEmployee.SportInstitutionBranchId).FirstOrDefault();

                var UserDataForNotification = await _userFunctions.GetUserDataForNotificationByProfileId(pendingUserProfileId, userProfileId);

                if (invetitationpending != null)
                {
                    var _BelongType = _context.SportInstitutionBelongBelongTypes.FirstOrDefault(x => x.BelongTypeId == bendingEmployee.BelongTypeId).BelongType;
                    var _SportInstitutionName = _context.SportInstitutions.FirstOrDefault(x => x.UserProfileId == userProfileId).SportInstitutionName;


                    var _message = $"تم دعوتك من {_SportInstitutionName} للانضمام كـ {_BelongType}";

                    var newDbObject = await _notification.CreateDBObject(userProfileId, pendingUserProfileId, _message, invetitationpending.SportInstitutionBelongPendingId);
                    _context.RealTimeNotifications.Add(newDbObject);
                    _context.SaveChanges();

                    var _newRealTimeNotification = await _notification.CreateRealTimeNotificationModel(UserDataForNotification, _message, checkpendingUserProfileId);
                    await _notificationService.SendNotification(_newRealTimeNotification);

                    return Ok("تم ارسال الدعوة بنجاح ");
                }
                #endregion

                #region add pending to database

                //add to pending

                var selectBelongType = _context.SportInstitutionBelongBelongTypes.Where(x => x.BelongTypeId == bendingEmployee.BelongTypeId).FirstOrDefault();
                if (selectBelongType == null) return NotFound("يجب ادخال نوع صحيح للشخص الذى تود اضافتة");

                var lastSeason = _context.Seasons.OrderByDescending(s => s.CreateDate).FirstOrDefault()?.SeasonId ?? null;

                var newBending = new SportInstitutionBelongPending
                {
                    BelongUserProfileId = pendingUserProfileId,
                    ShortDescription = bendingEmployee.ShortDescription,
                    BelongTypeId = bendingEmployee.BelongTypeId,
                    SportInstitutionBranchId = bendingEmployee.SportInstitutionBranchId,
                    SeasonId = lastSeason
                };
                _context.SportInstitutionBelongPendings.Add(newBending);
                _context.SaveChanges();

                //add agecategory that that person responsible for it or belong to it 
                foreach (var age in bendingEmployee.AgeCategories)
                {
                    var newSportInstitutionBelongPendingAgeCategory = new SportInstitutionBelongPendingAgeCategory
                    {
                        SportInstitutionBelongPendingId = newBending.SportInstitutionBelongPendingId,
                        AgeCategoryId = age
                    };

                    _context.SportInstitutionBelongPendingAgeCategories.Add(newSportInstitutionBelongPendingAgeCategory);
                }

                _context.SaveChanges();
                #endregion

                #region send notification

                var BelongType = selectBelongType.BelongType;
                var SportInstitutionName = _context.SportInstitutions.FirstOrDefault(x => x.UserProfileId == userProfileId).SportInstitutionName ?? null;

                var message = $"تم دعوتك من {SportInstitutionName} للانضمام كـ {BelongType}";
                //var newNotification = new RealTimeNotification
                //{
                //    FromUserProfileId = userProfileId,
                //    ToUserProfileId = pendingUserProfileId,
                //    TargetTypeId = 1,
                //    // it will use it in accept and reject invetation
                //    InvitationId = newBending.SportInstitutionBelongPendingId,
                //    TargetId = userProfileId,
                //    IsRead = false,
                //    ContentMessage = message,
                //};
                var _newDbObject = await _notification.CreateDBObject(userProfileId, pendingUserProfileId, message, newBending.SportInstitutionBelongPendingId);
                await _context.RealTimeNotifications.AddAsync(_newDbObject);
                await _context.SaveChangesAsync();


                //notification
                //var newNotificationModel = new NotificationModel
                //{
                //    UserProfileToken = checkpendingUserProfileId.Token,
                //    Title = UserDataForNotification.NameOfUserThatSendNotification,
                //    Body = message,
                //    UserProfileid = UserDataForNotification.UserProfileId,
                //    userid = UserDataForNotification.UserId,
                //    roleid = UserDataForNotification.RoleId,
                //    NumOfUnReadNotificationForAllAcount = UserDataForNotification.NumOfUnReadNotificationForAllAcount,
                //    NumOfUnReadNotificationForUserProfile = UserDataForNotification.NumOfUnReadNotificationForUserProfile,
                //    ImageOfUserThatSendNotification = UserDataForNotification.ImageOfUserThatSendNotification,
                //};

                // await _notificationService.SendNotification(newNotificationModel);

                var newRealTimeNotification = await _notification.CreateRealTimeNotificationModel(UserDataForNotification, message, checkpendingUserProfileId);
                await _notificationService.SendNotification(newRealTimeNotification);

                return Ok("تم ارسال الدعوة بنجاح");
                #endregion
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //قبول الدعوة للانضمام للاكاديمية
        [HttpPost(Name = "AcceptSportInstitutionInvetionPending")]
        public async Task<IActionResult> AcceptSportInstitutionInvetionPending(long pendingUserProfileId, byte roleid, long invetitationpendingId)
        {
            try
            {
                #region validation
                var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == pendingUserProfileId).FirstOrDefault();
                if (userprofile == null) return NotFound("لم يتم العثور على هذا المستخدم!");

                if (roleid != userprofile.RoleId) return BadRequest("ليس لديك صلاحية الدور الذى ادخلتة!");

                var invetitation = _context.SportInstitutionBelongPendings.Where(x => x.SportInstitutionBelongPendingId == invetitationpendingId).FirstOrDefault();
                if (invetitation == null) return NotFound("حدث خطا! قد يكون تم حذف هذة الدعوة ");
                if (invetitation.BelongUserProfileId != pendingUserProfileId) return BadRequest("!حدث خطا! قد يكون تم حذف هذة الدعوة");

                var lastSeason = _context.Seasons.OrderByDescending(s => s.CreateDate).FirstOrDefault()?.SeasonId ?? null;

                #endregion

                #region if hawi "didn't have the specefic role" add new role of invitation to user

                var userprofileid = invetitation.BelongUserProfileId;
                //it's role IS hawi we will add new user profile and change id to new user profile
                if (roleid == 1 && invetitation.BelongTypeId != 2)
                {
                    byte BelongtypeidConverterdToMatchRoleId = 0;

                    if (invetitation.BelongTypeId == 1)
                        BelongtypeidConverterdToMatchRoleId = 2;
                    else if (invetitation.BelongTypeId == 3)
                        BelongtypeidConverterdToMatchRoleId = 9;

                    else if (invetitation.BelongTypeId == 2 && roleid != 4)
                        return BadRequest("يجب عليك الانتساب كمدرب اولا للانضمام لهذة المؤسسة الرياضية!");

                    var newuserprofile = new UserProfile
                    {
                        UserId = userprofile.UserId,
                        RoleId = (byte)BelongtypeidConverterdToMatchRoleId,
                        IsActive = true,
                        LastUpdate = DateTime.Now,
                    };
                    _context.UserProfiles.Add(newuserprofile);
                    _context.SaveChanges();

                    //change user profile
                    userprofileid = newuserprofile.UserProfileId;
                }
                if (roleid == 1 && invetitation.BelongTypeId == 2)
                    return BadRequest("يجب عليك الانتساب كمدرب اولا للانضمام لهذة المؤسسة الرياضية!");

                #endregion

                #region add user to sportInstituation and add age category to that user 
                var newemployee = new SportInstitutionBelong
                {
                    SportInstitutionBranchId = (long)invetitation.SportInstitutionBranchId,
                    BelongUserProfileId = userprofileid,
                    ShortDescription = invetitation.ShortDescription,
                    BelongTypeId = invetitation.BelongTypeId,
                    IsActive = true,
                    LastUpdate = DateTime.Now,
                    SeasonId = lastSeason
                };

                _context.SportInstitutionBelongs.Add(newemployee);
                _context.SaveChanges();

                //add age category to that user 
                var BelongPendingAgeCategories = _context.SportInstitutionBelongPendingAgeCategories.Where(x => x.SportInstitutionBelongPendingId == invetitation.SportInstitutionBelongPendingId).ToList();

                foreach (var age in BelongPendingAgeCategories)
                {
                    var newSportInstitutionAgeCategoryBelong = new SportInstitutionAgeCategoryBelong
                    {
                        SportInstitutionBelongId = newemployee.SportInstitutionBelongId,
                        AgeCategoryId = age.AgeCategoryId
                    };

                    _context.SportInstitutionAgeCategoryBelongs.Add(newSportInstitutionAgeCategoryBelong);
                }
                _context.SaveChanges();

                #endregion

                #region add data to table of player if player  and remove pending

                if (invetitation.BelongTypeId == 1)
                {
                    var newplayer = new Player
                    {
                        UserProfileId = userprofileid,
                        SportInstitutionBranchId = invetitation.SportInstitutionBranchId,
                        SeasonId = lastSeason,
                    };
                    _context.Players.Add(newplayer);
                    _context.SaveChanges();
                }
                _context.SportInstitutionBelongPendingAgeCategories.RemoveRange(BelongPendingAgeCategories);
                _context.SportInstitutionBelongPendings.Remove(invetitation);

                var SelectedNotification = _context.RealTimeNotifications.Where(x => x.InvitationId == invetitationpendingId).FirstOrDefault();
                if (SelectedNotification != null) _context.RealTimeNotifications.Remove(SelectedNotification);
                _context.SaveChanges();


                #endregion

                #region send notification to sportInstituation that user accept invetation

                var BelongType = _context.SportInstitutionBelongBelongTypes.FirstOrDefault(x => x.BelongTypeId == invetitation.BelongTypeId).BelongType;
                var personAcceptedName = _context.Users.FirstOrDefault(x => x.UserId == userprofile.UserId).Name;
                var branch = _context.SportInstitutionBranches.Where(x => x.SportInstitutionBranchId == invetitation.SportInstitutionBranchId).FirstOrDefault();
                long sportinstituationid = 0;
                if (branch != null)
                    sportinstituationid = branch.SportInstitutionId;
                var touserprofileid = _context.SportInstitutions.FirstOrDefault(x => x.SportInstitutionId == sportinstituationid).UserProfileId;

                var UserProfileThatSendNotifyToIt = _context.UserProfiles.Where(x => x.UserProfileId == touserprofileid).FirstOrDefault();

                var message = $" تم قبول الدعوه من قبل {personAcceptedName}, للانضمام ك {BelongType}";

                var newNotification = new RealTimeNotification
                {
                    FromUserProfileId = pendingUserProfileId,
                    ToUserProfileId = touserprofileid,
                    TargetTypeId = 2,
                    TargetId = pendingUserProfileId,
                    IsRead = false,
                    ContentMessage = message,

                };

                _context.RealTimeNotifications.Add(newNotification);
                _context.SaveChanges();

                var UserDataForNotification = await _userFunctions.GetUserDataForNotificationByProfileId(touserprofileid, pendingUserProfileId);
                var newRealTimeNotification = await _notification.CreateRealTimeNotificationModel(UserDataForNotification, message, UserProfileThatSendNotifyToIt);
                await _notificationService.SendNotification(newRealTimeNotification);

                return Ok("تم الانضمام بنجاح");
                #endregion

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // make all unread notification for user as "readed"
        [HttpPut(Name = "ReadNotification")]
        public IActionResult ReadNotification(long UserProfileid)
        {
            var selectNotification = _context.RealTimeNotifications.Where(x => x.ToUserProfileId == UserProfileid && x.IsRead == false).ToList();
            selectNotification.ForEach(x => x.IsRead = true);

            _context.RealTimeNotifications.UpdateRange(selectNotification);
            _context.SaveChanges();
            return Ok("تم التحديث بنجاح");
        }

        [HttpDelete(Name = "rejectSportInstitutionInvetionPending")]
        public IActionResult rejectSportInstitutionInvetionPending(long pendingUserProfileId, long invetitationpendingId)
        {
            try
            {
                var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == pendingUserProfileId).FirstOrDefault();
                if (userprofile == null) return NotFound("لم يتم العثور على هذا المستخدم!");

                var invetitation = _context.SportInstitutionBelongPendings.Where(x => x.SportInstitutionBelongPendingId == invetitationpendingId).FirstOrDefault();
                if (invetitation == null) return NotFound("قد يكون قد تم حذف هذة الدعوة!");

                if (invetitation.BelongUserProfileId != pendingUserProfileId) return BadRequest("لا يمكنك حذف هذة الدعوة");

                var BelongPendingAgeCategories = _context.SportInstitutionBelongPendingAgeCategories.Where(x => x.SportInstitutionBelongPendingId == invetitation.SportInstitutionBelongPendingId).ToList();
                _context.SportInstitutionBelongPendingAgeCategories.RemoveRange(BelongPendingAgeCategories);
                _context.SportInstitutionBelongPendings.Remove(invetitation);

                var SelectedNotification = _context.RealTimeNotifications.Where(x => x.InvitationId == invetitationpendingId).FirstOrDefault();
                if (SelectedNotification != null) _context.RealTimeNotifications.Remove(SelectedNotification);

                _context.SaveChanges();
                return Ok("تم حذف الدعوة بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete(Name = "DeleteSpeceficUserNotification")]
        public IActionResult DeleteSpeceficUserNotification(long RealTimeNotificationId, long userprofileid)
        {
            var selectNotification = _context.RealTimeNotifications.Where(x => x.RealTimeNotificationId == RealTimeNotificationId).FirstOrDefault();
            if (selectNotification == null) return NotFound("لم يتم العثور على هذا الاخطار");

            if (selectNotification.ToUserProfileId != userprofileid) return BadRequest("لا يمكنك حذف هذا الاخطار!");

            _context.RealTimeNotifications.Remove(selectNotification);
            _context.SaveChanges();
            return Ok("تم الحذف بنجاح");
        }
    }
}

