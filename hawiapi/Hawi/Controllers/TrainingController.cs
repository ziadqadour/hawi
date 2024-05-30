using Hawi.Contracts;
using Hawi.Dtos;
using Hawi.Extensions;
using Hawi.Models;
using Hawi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hawi.Controllers
{
    [Route("hawi/Training/[action]")]
    [ApiController]
    public class TrainingController : Controller
    {

        private readonly HawiContext _context;
        private readonly ImageFunctions _ImageFunctions;
        private readonly IDateTimeBetweenRange _dateTimeRepository;
        private readonly UserFunctions _userFunctions;
        private readonly Notification _notification;
        private readonly INotificationService _notificationService;
        public TrainingController(IDateTimeBetweenRange dateTimeRepository, UserFunctions userFunctions, Notification notification, INotificationService notificationService, ImageFunctions imageFunctions, HawiContext Context)
        {
            _dateTimeRepository = dateTimeRepository;
            _userFunctions = userFunctions;
            _notification = notification;
            _notificationService = notificationService;
            _ImageFunctions = imageFunctions;
            _context = Context;
        }


        [HttpGet(Name = "GetTrainingType")]
        public IActionResult GetTrainingType()
        {
            try
            {
                var types = _context.TrainingTrainingTypes
                 .Select(x => new
                 {
                     x.TrainingTypeId,
                     x.TrainingType,

                 }).ToList();
                return Ok(types);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetPlaygroundFloorType")]
        public IActionResult GetPlaygroundFloorType()
        {
            try
            {
                var PlaygroundFloors = _context.TrainingDetailsPlaygroundFloors
                  .Select(x => new
                  {
                      x.PlaygroundFloorId,
                      x.PlaygroundFloor,
                  }).ToList();

                return Ok(PlaygroundFloors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetPlaygroundSizeType")]
        public IActionResult GetPlaygroundSizeType()
        {
            try
            {
                var PlaygroundSizeTypes = _context.TrainingDetailsPlaygroundSizes.Select(x => new
                {
                    x.PlaygroundSizeId,
                    x.PlaygroundSize,
                }).ToList();

                return Ok(PlaygroundSizeTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetInjuryPositions")]
        public IActionResult GetInjuryPositions()
        {
            try
            {
                var InjuryPositions = _context.TrainingInjuryInjuryPositions.Select(x => new
                {
                    x.InjuryPositionId,
                    x.InjuryPositionInEnglish,
                    x.InjuryPositionInArabic
                }).ToList();

                return Ok(InjuryPositions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetDays")]
        public IActionResult GetDays()
        {
            try
            {
                var days = _context.Days.Select(x => new
                {
                    x.DayId,
                    x.DayInArabic,
                    x.DayInEnglish
                }).ToList();
                return Ok(days);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetSportInstitutionBranchAgeCategories")]
        public IActionResult GetSportInstitutionBranchAgeCategories(long SportInstitutionBranchId)
        {
            try
            {
                var ageCategories = _context.SportInstitutionAgeCategoryBelongs
                .Where(belong => belong.SportInstitutionBelong.SportInstitutionBranchId == SportInstitutionBranchId)
                .Select(x => new
                {
                    x.AgeCategory.AgeCategory,
                    x.AgeCategory.AgeCategoryId,
                })
                .ToList();
                return Ok(ageCategories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "GetCoachesOfBracnchByAgeCategories")]
        public IActionResult GetCoachesOfBracnchByAgeCategories([FromBody] GetCoachOfTraininByListOfAgeCategoryDto coach)
        {
            try
            {
                var userProfileIds = _context.SportInstitutionBelongs
                                .Where(x => x.SportInstitutionBranchId == coach.BranchId && x.BelongTypeId == 2)
                                .Where(x => x.SportInstitutionAgeCategoryBelongs
                                 .Any(y => coach.AgecategoryId.Contains(y.AgeCategoryId)))
                                .Select(x => x.BelongUserProfileId)
                                .ToList();

                var coaches = _context.UserProfiles
                                .Where(x => userProfileIds.Contains(x.UserProfileId))
                                .Select(x => new
                                {
                                    x.UserProfileId,
                                    x.User.Name
                                })
                               .ToList();
                return Ok(coaches);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //
        [HttpGet(Name = "GetTraining")]
        public async Task<IActionResult> GetTraining(long userprofileId, [FromQuery] OwnerParameters ownerParameters, bool IsFinish = true, bool IsYourTraining = true)
        {
            try
            {
                string defaultTrainingImage = "http://mobile.hawisports.com/image/coverprofileapp.png";

                var currentDate = DateTime.UtcNow;
                var IsInTraining = false;
                var IsOpen = false;

                List<TrainingDetail> trainingDetails = null;

                if (IsYourTraining == true)
                {
                    IsInTraining = true;
                    IsOpen = false;

                    trainingDetails = await _context.TrainingDetails
                       .Where(td => td.TrainingAttendances.Any(a => a.AttendanceUserProfileId == userprofileId))
                       .Where(td => (IsFinish == false && td.TrainingDate >= currentDate)
                                || (IsFinish == true && td.TrainingDate < currentDate))
                      .Distinct()
                      .ToListAsync();

                }
                else if (IsYourTraining == false)
                {//open and "userprofileId" not in trainig 

                    IsInTraining = false;
                    IsOpen = true;
                    List<long> trainingDetailsForOpen = null;

                    trainingDetailsForOpen = await _context.TrainingDetails
                       .Where(td => td.TrainingAttendances.Any(a => a.AttendanceUserProfileId == userprofileId))
                       .Select(td => td.TrainingDetailsId)
                       .Distinct()
                       .ToListAsync();

                    trainingDetails = await _context.TrainingDetails
                        .Where(td => !trainingDetailsForOpen.Contains(td.TrainingDetailsId) && td.Training.TrainingTypeId == 1)
                        .Where(td => (IsFinish == false && td.TrainingDate >= currentDate)
                                || (IsFinish == true && td.TrainingDate < currentDate))
                        .ToListAsync();
                }

                trainingDetails = trainingDetails
                    .Where(x => x.IsActive == true)
                       .OrderBy(td => IsFinish ? null : td.TrainingDate) // Order by TrainingDate if IsFinish is false
                       .ThenByDescending(td => IsFinish ? td.TrainingDate : null) // Order by TrainingDate if IsFinish is false
                       .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                       .Take(ownerParameters.PageSize).ToList();



                var result = trainingDetails.Select(x =>
                {
                    var training = _context.Training.FirstOrDefault(t => t.TrainingId == x.TrainingId);
                    var playgroundFloor = _context.TrainingDetailsPlaygroundFloors.Where(pf => pf.PlaygroundFloorId == x.PlaygroundFloorId)?.FirstOrDefault();
                    var playgroundSize = _context.TrainingDetailsPlaygroundSizes.Where(ps => ps.PlaygroundSizeId == x.PlaygroundSizeId)?.FirstOrDefault();
                    var trainingImageId = training?.TrainingImageId;
                    var ImageUrlfullPath = (trainingImageId != null) ? _context.Images.FirstOrDefault(i => i.ImageId == trainingImageId)?.ImageUrlfullPath : defaultTrainingImage;
                    var trainingName = training?.TrainingName ?? null;
                    var trainingType = training?.TrainingTypeId ?? null;
                    var seasonName = _context.Seasons.FirstOrDefault(s => s.SeasonId == x.Training.SeasonId)?.SeasonName ?? null;
                    var SportInstitutionBranchId = _context.TrainingBranches
                           .Where(x => x.TrainingId == training.TrainingId)
                           .Select(x => (long?)x.SportInstitutionBranchId)
                           .FirstOrDefault() ?? null;
                    var userprofileoftraining = _context.UserProfiles.Where(z => z.UserProfileId == x.Training.UserProfileId).FirstOrDefault();
                    var trainingadmins = _context.TrainingAdmins.Where(t => t.TrainingId == x.TrainingId).Select(x => x.AdminUserProfileId).ToList();

                    return new
                    {
                        OwnerOfTrainingImage = _userFunctions.GetUserImage(userprofileoftraining.UserProfileId, userprofileoftraining.RoleId),
                        OwnerOfTrainName = _userFunctions.GetUserName(userprofileoftraining.RoleId, userprofileoftraining.UserProfileId, userprofileoftraining.UserId),
                        x.TrainingId,
                        x.TrainingDetailsId,
                        TrainingDate = x.TrainingDate ?? null,
                        TrainingTime = x.TrainingTime ?? null,
                        DurationInMinutes = x.DurationInMinutes ?? null,
                        TrainingCost = x.TrainingCost ?? null,
                        PlaygroundFloor = (playgroundFloor != null) ? playgroundFloor.PlaygroundFloor : null,
                        PlaygroundSize = (playgroundSize != null) ? playgroundSize.PlaygroundSize : null,
                        TrainingPlaceLocation = x.TrainingPlaceLocation ?? null,
                        ImageUrlfullPath = ImageUrlfullPath ?? null,
                        TrainingName = trainingName ?? null,
                        Trainingtype = trainingType ?? null,
                        SeasonName = seasonName ?? null,
                        SportInstitutionBranchId = SportInstitutionBranchId ?? null,
                        numberofAttendance = _context.TrainingAttendances.Where(n => n.TrainingDetailsId == x.TrainingDetailsId).Count(),
                        OwnerOfTraining = training.UserProfileId,
                        trainingadmins = trainingadmins,
                        IsInTraining = IsInTraining,
                        IsEnd = (x.TrainingDate.Value.AddDays(1) >= DateTime.Now) ? false : true,
                        IsRepeated = x.Training.IsRepeated ?? null,
                        IsOpen = IsOpen,
                    };
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetAttendanceOfTraining")]
        public IActionResult GetAttendanceOfTraining(long TrainingDetailsId, byte isPresent)
        {
            try
            {
                //if isPresent=1 >>give present only, if =0 >>all attendance

                var TrainingDetails = _context.TrainingDetails.Where(x => x.TrainingDetailsId == TrainingDetailsId).FirstOrDefault();
                if (TrainingDetails == null) return NotFound("لم يتم العثور على التمرين");

                var trainingid = TrainingDetails.TrainingId;

                var trainingadmins = _context.TrainingAdmins.Where(x => x.TrainingId == trainingid && x.IsActive == true).ToList();
                var isAdminLookup = trainingadmins.ToLookup(admin => admin.AdminUserProfileId);

                List<ShowTrainingAttendanceDto> attendance = null;

                if (isPresent == 1)
                {
                    attendance = _context.TrainingAttendances
                        .Where(x => x.IsPresent && x.TrainingDetailsId == TrainingDetailsId)
                        .Select(x => new ShowTrainingAttendanceDto
                        {
                            AttindanceId = x.AttendanceId,
                            UserProfileId = x.AttendanceUserProfileId,
                            IsPresent = x.IsPresent,
                            Name = _userFunctions.GetUserName(x.AttendanceUserProfile.RoleId, x.AttendanceUserProfileId, x.AttendanceUserProfile.UserId) ?? null,
                            type = _context.SportInstitutionBelongBelongTypes.FirstOrDefault(z => z.BelongTypeId == x.AttendanceTypeId).BelongType ?? null,
                            Image = _userFunctions.GetUserImage(x.AttendanceUserProfileId, x.AttendanceUserProfile.RoleId),
                            IsAdmin = isAdminLookup.Contains(x.AttendanceUserProfileId)

                        })
                        .ToList();
                }
                else
                {
                    attendance = _context.TrainingAttendances
                        .Where(x => x.TrainingDetailsId == TrainingDetailsId)
                      .Select(x => new ShowTrainingAttendanceDto
                      {
                          AttindanceId = x.AttendanceId,
                          UserProfileId = x.AttendanceUserProfileId,
                          IsPresent = x.IsPresent,
                          Name = _userFunctions.GetUserName(x.AttendanceUserProfile.RoleId, x.AttendanceUserProfileId, x.AttendanceUserProfile.UserId) ?? null,
                          type = _context.SportInstitutionBelongBelongTypes.FirstOrDefault(z => z.BelongTypeId == x.AttendanceTypeId).BelongType ?? null,
                          Image = _userFunctions.GetUserImage(x.AttendanceUserProfileId, x.AttendanceUserProfile.RoleId),
                          IsAdmin = isAdminLookup.Contains(x.AttendanceUserProfileId)

                      })
                      .ToList();
                }

                var getattendance = attendance.GroupBy(x => x.type);

                return Ok(getattendance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetTrainingEvaluation")]
        public IActionResult GetTrainingEvaluation(long TrainingDetailsId)
        {
            try
            {
                var trainingDetails = _context.TrainingDetails.FirstOrDefault(x => x.TrainingDetailsId == TrainingDetailsId);
                if (trainingDetails == null) return NotFound("لم يتم العثور على التمرين");

                var attendanceList = _context.TrainingAttendances
                    .Where(a => a.TrainingDetailsId == TrainingDetailsId)
                    .Select(a => new
                    {
                        a.AttendanceId,
                        a.AttendanceUserProfileId,
                        a.IsPresent,
                        a.AttendanceTypeId,
                        Evaluation = a.TrainingEvaluations.FirstOrDefault(),
                        userid = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == a.AttendanceUserProfileId).UserId
                    })
                    .ToList();

                var evaluations = attendanceList.Select(a =>
                {
                    var selectedUserProfile = _context.UserProfiles.Where(x => x.UserProfileId == a.AttendanceUserProfileId).FirstOrDefault();
                    return new
                    {
                        a.Evaluation?.EvaluationId,
                        a.AttendanceId,
                        a.AttendanceUserProfileId,
                        name = _userFunctions.GetUserName(selectedUserProfile.RoleId, a.AttendanceUserProfileId, selectedUserProfile.UserId) ?? null,
                        Image = _userFunctions.GetUserImage(a.AttendanceUserProfileId, selectedUserProfile.RoleId),
                        EvaluationComment = a.Evaluation?.EvaluationComment,
                        OutOfFive = a.Evaluation?.OutOfFive,
                        ispresent = a.IsPresent,
                        isevaluate = (a.Evaluation?.EvaluationId == null) ? false : true,
                        type = a.AttendanceTypeId,
                    };
                }).ToList().GroupBy(a => a.ispresent).Reverse();

                var groupedEvaluations = evaluations.Select(group => group.Select(innerGroup => innerGroup).ToList()).ToList();
                return Ok(groupedEvaluations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetTrainingAdmins")]
        public IActionResult GetTrainingAdmins(long TrainingId)
        {
            try
            {
                var admins = _context.TrainingAdmins.Where(x => x.TrainingId == TrainingId && x.IsActive == true)
                                                   .Select(x => new
                                                   {
                                                       x.TrainingAdminId,
                                                       x.AdminUserProfileId,
                                                       Name = _userFunctions.GetUserName(x.AdminUserProfile.RoleId, x.AdminUserProfileId, x.AdminUserProfile.UserId) ?? null,
                                                       role = _context.UserProfiles.FirstOrDefault(z => z.UserProfileId == x.AdminUserProfileId).Role.Role1 ?? null,
                                                       image = _userFunctions.GetUserImage(x.AdminUserProfileId, x.AdminUserProfile.RoleId),
                                                       x.CreateDate
                                                   }).ToList();

                return Ok(admins);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetTrainingInjuries")]
        public IActionResult GetTrainingInjuries(long AttendanceID, long TrainingDetailsId)
        {
            try
            {
                var attendanc = _context.TrainingAttendances
                    .Where(x => x.AttendanceId == AttendanceID && x.TrainingDetailsId == TrainingDetailsId)
                    .FirstOrDefault();

                if (attendanc == null)
                    return NotFound("!لم يتم العثور على هذا اللاعب");

                var userprofile = _context.UserProfiles
                    .Where(x => x.UserProfileId == attendanc.AttendanceUserProfileId)
                    .FirstOrDefault();

                var injuries = _context.TrainingInjuries
                    .Where(x => x.AttendanceId == AttendanceID && x.TrainingDetailsId == TrainingDetailsId)
                    .Select(x => new
                    {
                        x.InjuryId,
                        x.AttendanceId,
                        TrainingDetailsId = x.TrainingDetailsId,
                        x.Attendance.AttendanceUserProfileId,
                        name = _userFunctions.GetUserName(userprofile.RoleId, userprofile.UserProfileId, userprofile.UserId),
                        image = _userFunctions.GetUserImage(userprofile.UserProfileId, userprofile.RoleId),
                        InjuryPosition = x.InjuryPosition.InjuryPositionInArabic,
                        InjuryPositionInEnglishname = x.InjuryPosition.InjuryPositionInEnglish,
                        x.InjuryPositionId,
                        x.InjuryComment,
                        x.CreateDate
                    })
                    .ToList();

                var updateTrainingInjuryDto = new UPDATETrainingInjuryDto();

                // Get the properties of the UPDATETrainingInjuryDto model using reflection
                var properties = typeof(UPDATETrainingInjuryDto).GetProperties();

                bool checkifexistInjery = (injuries != null && injuries.Count != 0);

                if (checkifexistInjery == true)
                {
                    foreach (var propertyInfo in properties)
                    {
                        // Check if the property name matches the InjuryPositionInEnglishname
                        var propertyName = propertyInfo.Name;
                        var isMatching = injuries.Any(x => x.InjuryPositionInEnglishname == propertyName);

                        // Set the property to true or false based on the match
                        propertyInfo.SetValue(updateTrainingInjuryDto, isMatching);
                    }
                }
                var returndata = new
                {
                    updateTrainingInjuryDto,
                    TrainingDetailsId,
                    AttendanceID,
                    UserProfileId = userprofile.UserProfileId,
                    name = _userFunctions.GetUserName(userprofile.RoleId, userprofile.UserProfileId, userprofile.UserId) ?? null,
                    image = _userFunctions.GetUserImage(userprofile.UserProfileId, userprofile.RoleId) ?? null,
                    InjuryComment = (checkifexistInjery == true) ? injuries.FirstOrDefault(x => x.AttendanceId == AttendanceID && x.TrainingDetailsId == TrainingDetailsId).InjuryComment : null,
                    CreateDate = (checkifexistInjery == true) ? injuries.FirstOrDefault(x => x.AttendanceId == AttendanceID && x.TrainingDetailsId == TrainingDetailsId).CreateDate : null
                };

                // return Ok(updateTrainingInjuryDto);
                return Ok(returndata);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "AddTraining")]
        public async Task<IActionResult> AddTraining(long UserProfileId, [FromForm] AddTrainingDto trainingDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    //#region to check validate if user add training in same time of one training  

                    //var v = _context.Training
                    //   .Where(x => x.UserProfileId == UserProfileId && x.StartDate >= trainingDto.StartDate)
                    //   .Include(x => x.TrainingDetails)
                    //   .ToList();


                    //if (trainingDto.IsRepeated == true)
                    //{

                    //    List<string> newdays = new List<string>();
                    //    for (int i = 0; i < trainingDto.Days.Count; i++)
                    //    {
                    //        newdays.Add(_context.Days.FirstOrDefault(x => x.DayId == trainingDto.Days[i]).DayInEnglish);
                    //    }

                    //    for (int i = 0; i < newdays.Count; i++)
                    //    {
                    //        List<DateTime> occurrences = _dateTimeRepository.GetDateTimeOccurrences(newdays[i], trainingDto.StartDate, trainingDto.EndDate);
                    //        for (int x = 0; x < occurrences.Count; x++)
                    //        {
                    //            var CheckTrainingDate = _userFunctions.AddTimeToDateInTraining(trainingDto.TrainingTime[i], occurrences[x]);


                    //            var CheckIfTrainingDateInSameTime = v.Where(x=>x.TrainingDetails.Where(n=>n.TrainingDate==))
                    //        }
                    //    }

                    //}
                    //else
                    //{
                    //    var CheckTrainingDate = _userFunctions.AddTimeToDateInTraining(trainingDto.TrainingTime[0], trainingDto.StartDate);
                    //}


                    //#endregion

                    #region validation

                    if (trainingDto.TrainingImage != null)
                    {
                        var checkimage = _ImageFunctions.GetInvalidImageMessage(trainingDto.TrainingImage);
                        if (checkimage != null) return BadRequest(checkimage);
                    }
                    var CheckUserProfile = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileId).FirstOrDefault();
                    if (CheckUserProfile == null) return NotFound("لم يتم العثور على هذا المستخدم!");

                    if (trainingDto.IsRepeated == true)
                        if (trainingDto.StartDate == trainingDto.EndDate)
                            return BadRequest("فى حالة تكرار التمرين لا يجب ان يكون وقت بداية التمرين تساوى نهاية التمرين");

                    if (trainingDto.IsRepeated == true)
                        if (trainingDto.StartDate > trainingDto.EndDate)
                            return BadRequest("لا يجب ان يكون وقت بداية التمرين اكبر من وقت بداية التمرين!");


                    if (trainingDto.IsRepeated == false)
                        if (trainingDto.StartDate != trainingDto.EndDate)
                            return BadRequest("فى حالة عدم تكرار التمرين  يجب ان يكون وقت بداية التمرين يساوى نهاية التمرين");


                    if (trainingDto.TrainingTime.Count != trainingDto.Days.Count && trainingDto.TrainingTime.Count != trainingDto.DurationInMinutes.Count)
                        return BadRequest("يجب اختيار المواعيد بشكل صحيح !");

                    var lastSeason = _context.Seasons.OrderByDescending(s => s.SeasonId).FirstOrDefault();
                    if (trainingDto.StartDate < lastSeason.StartDate)
                        return BadRequest($"يجب ان يكون التمرين ما بين {lastSeason.StartDate}:{lastSeason.EndDate}");

                    if (trainingDto.EndDate > lastSeason.EndDate)
                        return BadRequest($"يجب ان يكون التمرين ما بين {lastSeason.StartDate}:{lastSeason.EndDate}");

                    #endregion

                    #region training

                    #region add training image

                    long imageid = 0;
                    if (trainingDto.TrainingImage != null)
                    {
                        string englishFileName = _ImageFunctions.ChangeImageName(trainingDto.TrainingImage);
                        var directory = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Training\";
                        await _ImageFunctions.CheckDirectoryExist(directory);
                        var path = directory + englishFileName;
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await trainingDto.TrainingImage.CopyToAsync(stream);
                        }

                        var img = new Models.Image
                        {
                            ImageUrlfullPath = $"http://mobile.hawisports.com/image/Training/" + englishFileName,
                            ImageFileName = englishFileName,
                            ImageTypeId = 9,
                            IsActive = true,
                        };

                        _context.Images.Add(img);
                        await _context.SaveChangesAsync();
                        imageid = img.ImageId;
                    }
                    #endregion


                    var NewTraining = new Training
                    {
                        UserProfileId = UserProfileId,
                        TrainingName = trainingDto.TrainingName,
                        TrainingTypeId = trainingDto.TrainingTypeId,
                        StartDate = trainingDto.StartDate,
                        EndDate = trainingDto.EndDate,
                        SeasonId = lastSeason.SeasonId,
                        IsRepeated = trainingDto.IsRepeated ?? false,
                        IsActive = true,
                    };
                    if (trainingDto.TrainingImage != null)
                        NewTraining.TrainingImageId = imageid;

                    _context.Training.Add(NewTraining);
                    _context.SaveChanges();

                    #endregion

                    #region TrainingBranch

                    var SelectTrainingPlaceLocation = trainingDto.TrainingPlaceLocation;
                    if (trainingDto.BranchId != null && trainingDto.BranchId != 0)
                    {
                        var checkPranch = _context.SportInstitutionBranches.Where(x => x.SportInstitutionBranchId == trainingDto.BranchId).FirstOrDefault();
                        if (checkPranch == null) return NotFound("!لم يتم العثور على الفرع الذى ادخلتة");

                        var TrainingBranch = new TrainingBranch
                        {
                            TrainingId = NewTraining.TrainingId,
                            SportInstitutionBranchId = (long)trainingDto.BranchId,
                        };
                        _context.TrainingBranches.Add(TrainingBranch);
                        _context.SaveChanges();
                        SelectTrainingPlaceLocation = checkPranch.Location ?? null;
                    }

                    #endregion

                    #region Traininig Details


                    if (trainingDto.IsRepeated == true)
                    {

                        List<string> newdays = new List<string>();
                        for (int i = 0; i < trainingDto.Days.Count; i++)
                        {
                            newdays.Add(_context.Days.FirstOrDefault(x => x.DayId == trainingDto.Days[i]).DayInEnglish);
                        }

                        for (int i = 0; i < newdays.Count; i++)
                        {
                            List<DateTime> occurrences = _dateTimeRepository.GetDateTimeOccurrences(newdays[i], trainingDto.StartDate, trainingDto.EndDate);
                            for (int x = 0; x < occurrences.Count; x++)
                            {
                                var newTrainingDetails = new TrainingDetail
                                {
                                    TrainingId = NewTraining.TrainingId,
                                    TrainingPlaceLocation = SelectTrainingPlaceLocation,
                                    TrainingCost = trainingDto.TrainingCost ?? null,
                                    DurationInMinutes = trainingDto.DurationInMinutes[i],
                                    TrainingTime = trainingDto.TrainingTime[i],
                                    TrainingDate = _userFunctions.AddTimeToDateInTraining(trainingDto.TrainingTime[i], occurrences[x])
                                };
                                if (trainingDto.PlaygroundFloorId != 0) newTrainingDetails.PlaygroundFloorId = trainingDto.PlaygroundFloorId;
                                if (trainingDto.PlaygroundSizeId != 0) newTrainingDetails.PlaygroundSizeId = trainingDto.PlaygroundSizeId;
                                _context.TrainingDetails.Add(newTrainingDetails);
                                //  _context.SaveChanges();
                            }
                        }
                        _context.SaveChanges();

                    }
                    else
                    {
                        var newTrainingDetails = new TrainingDetail
                        {
                            TrainingId = NewTraining.TrainingId,
                            PlaygroundFloorId = trainingDto.PlaygroundFloorId ?? null,
                            PlaygroundSizeId = trainingDto.PlaygroundSizeId ?? null,
                            TrainingPlaceLocation = SelectTrainingPlaceLocation,
                            TrainingCost = trainingDto.TrainingCost ?? null,
                            DurationInMinutes = trainingDto.DurationInMinutes[0],
                            TrainingTime = trainingDto.TrainingTime[0],
                            // TrainingDate = trainingDto.StartDate,
                            TrainingDate = _userFunctions.AddTimeToDateInTraining(trainingDto.TrainingTime[0], trainingDto.StartDate),
                        };
                        _context.TrainingDetails.Add(newTrainingDetails);

                        _context.SaveChanges();
                    }

                    //var lastTrainingDetailsTime = _context.TrainingDetails.Where(x => x.TrainingId == NewTraining.TrainingId)
                    //        .OrderByDescending(x => x.TrainingDate)
                    //        .FirstOrDefault();

                    //NewTraining.EndDate = lastTrainingDetailsTime.TrainingDate;
                    #endregion

                    #region AddItAdmin

                    var admin = new TrainingAdmin
                    {
                        TrainingId = NewTraining.TrainingId,
                        AdminUserProfileId = UserProfileId,
                        IsActive = true,
                    };
                    _context.TrainingAdmins.Add(admin);
                    _context.SaveChanges();

                    #endregion

                    #region AddIt in training attendance

                    var futureTraining = _context.TrainingDetails.Where(a => a.TrainingId == NewTraining.TrainingId).ToList();
                    foreach (var t in futureTraining)
                    {
                        var newtrainingattendance = new TrainingAttendance
                        {
                            UserProfileId = UserProfileId,
                            TrainingDetailsId = t.TrainingDetailsId,
                            AttendanceUserProfileId = UserProfileId,
                            IsPresent = false,
                            AttendanceTypeId = CheckUserProfile.RoleId,
                        };
                        _context.TrainingAttendances.Add(newtrainingattendance);
                    }
                    _context.SaveChanges();

                    #endregion

                    transaction.Commit();
                    return Ok(NewTraining.TrainingId);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        // that select all player in that SportInstituation according age categories and adding it to training attendance
        [HttpPost(Name = "TrainingAttendancesForSportInstituation")]
        public async Task<IActionResult> TrainingAttendancesForSportInstituation(long UserProfileId, long TrainingId, [FromBody] TrainingAttendanceDto trainingAttendance)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region validation

                    var CheckUserProfile = _context.TrainingAdmins.Where(x => x.AdminUserProfileId == UserProfileId && x.TrainingId == TrainingId && x.IsActive == true).FirstOrDefault();
                    if (CheckUserProfile == null)
                        return NotFound("لا يمكنك القيام بذلك المهام!");

                    var Training = _context.Training.Where(x => x.TrainingId == TrainingId).FirstOrDefault();
                    if (Training == null)
                        return NotFound("لم يتم العثور على التمرين");

                    var trainingBranchid = _context.TrainingBranches
                     .Where(x => x.TrainingId == TrainingId)
                     .Select(x => x.SportInstitutionBranchId)
                     .FirstOrDefault();

                    #endregion

                    #region attendance
                    //player of all age category that select 
                    List<byte> ageCategories = trainingAttendance.AgeCategoryOfPlayer;
                    List<long> playersbelongUserProfileIds = _context.SportInstitutionAgeCategoryBelongs
                       .Where(siba => ageCategories.Contains(siba.AgeCategoryId)
                                    && siba.SportInstitutionBelong.BelongTypeId == 1
                                    && siba.SportInstitutionBelong.SportInstitutionBranchId == trainingBranchid)
                       .Select(siba => siba.SportInstitutionBelong.BelongUserProfileId)
                       .Distinct()
                       .ToList();


                    List<long> trainingDetails = _context.TrainingDetails
                   .Where(td => td.TrainingId == TrainingId)
                   .Select(td => td.TrainingDetailsId)
                   .ToList();

                    void AddAttendance(List<long> userProfileIds, byte AttendanceTypeId)
                    {
                        foreach (var trainingDetailsId in trainingDetails)
                        {
                            foreach (var attendance in userProfileIds)
                            {
                                var checkattendance = _context.TrainingAttendances.Where(x => x.TrainingDetailsId == trainingDetailsId && x.AttendanceUserProfileId == attendance).FirstOrDefault();
                                if (checkattendance == null)
                                {
                                    var newTrainingAttendance = new TrainingAttendance
                                    {
                                        UserProfileId = UserProfileId,
                                        TrainingDetailsId = trainingDetailsId,
                                        AttendanceUserProfileId = attendance,
                                        IsPresent = false,
                                        AttendanceTypeId = AttendanceTypeId
                                    };

                                    _context.TrainingAttendances.Add(newTrainingAttendance);
                                }
                            }
                        }
                    }
                    //player
                    AddAttendance(playersbelongUserProfileIds, 1);
                    //coach
                    AddAttendance(trainingAttendance.CoachUserProfileId, 2);
                    //addministration
                    AddAttendance(trainingAttendance.AdministrativeUserProfileId, 3);
                    _context.SaveChanges();
                    #endregion
                    transaction.Commit();
                    return Ok("تمت الاضافة  بنجاح");
                }
                catch (Exception ex)
                {
                    #region delete training
                    //delete  training & image from server
                    transaction.Rollback();
                    var Training = _context.Training.Where(x => x.TrainingId == TrainingId).FirstOrDefault();
                    _context.Training.Remove(Training);

                    var imagename = _context.Images.Where(x => x.ImageId == Training.TrainingImageId).FirstOrDefault();
                    if (imagename != null)
                    {
                        var path = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Training\" + imagename.ImageFileName;
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                    _context.SaveChanges();
                    #endregion
                    return BadRequest(ex.Message);
                }
            }
        }
        // add one person to attendance of training for SportInstituation 
        [HttpPost(Name = "OneTrainingAttendanceForSportInstituation")]
        public IActionResult OneTrainingAttendanceForSportInstituation(long UserProfileId, long TrainingId, long NewPersonUserProfileId)
        {
            try
            {
                //AttendanceTypeId "player trainer administration"1 2 3""
                var CheckUserProfile = _context.TrainingAdmins.Where(x => x.AdminUserProfileId == UserProfileId && x.TrainingId == TrainingId && x.IsActive == true).FirstOrDefault();
                if (CheckUserProfile == null)
                    return NotFound("لا يمكنك القيام بذلك المهام!");

                var Training = _context.Training.Where(x => x.TrainingId == TrainingId).FirstOrDefault();
                if (Training == null)
                    return NotFound("لم يتم العثور على التمرين");

                var checkbranch = _context.TrainingBranches.Where(x => x.TrainingId == TrainingId).FirstOrDefault();
                if (checkbranch == null)
                    return NotFound("لم يتم اضافة فرع مع هذا التدريب !");

                var checkNewPersonUserProfileId = _context.SportInstitutionBelongs.Where(x => x.BelongUserProfileId == NewPersonUserProfileId && x.SportInstitutionBranchId == checkbranch.SportInstitutionBranchId).FirstOrDefault();
                if (checkNewPersonUserProfileId == null)
                    return BadRequest("هذا الشخص غير منتسب للفرع المضاف للتمرين!");


                var newuserprofile = _context.UserProfiles.Where(x => x.UserProfileId == NewPersonUserProfileId).FirstOrDefault();
                var futureTraining = _context.TrainingDetails
                   .Where(a => a.TrainingId == TrainingId &&
                             (a.TrainingDate >= (DateTime.Now)))
                   .ToList();
                foreach (var t in futureTraining)
                {
                    var checkAttendance = _context.TrainingAttendances.Where(x => x.TrainingDetailsId == t.TrainingDetailsId && x.AttendanceUserProfileId == NewPersonUserProfileId).FirstOrDefault();
                    if (checkAttendance == null)
                    {
                        var newtrainingattendance = new TrainingAttendance
                        {
                            UserProfileId = UserProfileId,
                            TrainingDetailsId = t.TrainingDetailsId,
                            AttendanceUserProfileId = NewPersonUserProfileId,
                            IsPresent = false,
                            AttendanceTypeId = newuserprofile.RoleId,
                        };
                        _context.TrainingAttendances.Add(newtrainingattendance);
                    }
                }
                _context.SaveChanges();
                return Ok("تم الاضافة بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //close
        //notification
        // IsInvited = true
        [HttpPost(Name = "InvetationTrainingAttendanceForperson")]
        public async Task<IActionResult> InvetationTrainingAttendanceForperson(long UserProfileId, long? TrainingId, [FromBody] TrainingAttendanceForPersonsDto trainingAttendance)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var CheckUserProfile = _context.TrainingAdmins.Where(x => x.AdminUserProfileId == UserProfileId && x.TrainingId == TrainingId && x.IsActive == true).FirstOrDefault();
                    if (CheckUserProfile == null)
                        return NotFound("لا يمكنك القيام بذلك المهام!");


                    var Training = _context.Training.Where(x => x.TrainingId == TrainingId && x.IsActive == true).FirstOrDefault();
                    if (Training == null)
                        return NotFound("لم يتم العثور على التمرين");

                    var owneruserprofile = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileId).FirstOrDefault();
                    var username = _context.Users.FirstOrDefault(x => x.UserId == owneruserprofile.UserId).Name;

                    //if send notifybefore don't send again 
                    var x = 0;

                    foreach (var trainee in trainingAttendance.PendingUserProfileId)
                    {
                        var UserDataForNotification = await _userFunctions.GetUserDataForNotificationByProfileId(trainee, UserProfileId);

                        var checkifexist = _context.TrainingInvitationPendings
                             .Where(x => x.TrainingOwnerUserProfileId == UserProfileId
                                 && x.PendingUserProfileId == trainee && x.TrainingId == TrainingId).FirstOrDefault();

                        if (checkifexist != null)

                            x++;
                        else
                        {

                            //add in pending
                            var newTrainingInvitationPending = new TrainingInvitationPending
                            {
                                TrainingOwnerUserProfileId = UserProfileId,
                                PendingUserProfileId = trainee,
                                TrainingId = TrainingId,
                                IsInvited = true,
                            };
                            _context.TrainingInvitationPendings.Add(newTrainingInvitationPending);
                            _context.SaveChanges();


                            var message = $" تم دعوتك من قبل {username} للانضمام للتمرين الخاص به";
                            var newNotification = new RealTimeNotification
                            {
                                FromUserProfileId = UserProfileId,
                                ToUserProfileId = newTrainingInvitationPending.PendingUserProfileId,
                                TargetTypeId = 5,
                                InvitationId = newTrainingInvitationPending.TrainingInvitationPendingId,
                                TargetId = (long)TrainingId,
                                IsRead = false,
                                ContentMessage = message,
                            };

                            _context.RealTimeNotifications.Add(newNotification);
                            _context.SaveChanges();

                            //sendNotification
                            var TraininUserProfile = _context.UserProfiles.Where(x => x.UserProfileId == newTrainingInvitationPending.PendingUserProfileId).FirstOrDefault();
                            var newRealTimeNotification = await _notification.CreateRealTimeNotificationModel(UserDataForNotification, message, TraininUserProfile);
                            await _notificationService.SendNotification(newRealTimeNotification);

                        }
                    }
                    _context.SaveChanges();


                    transaction.Commit();
                    if (trainingAttendance.PendingUserProfileId.Count > 1 && x > 0)
                        return Ok("تم الارسال بنجاح بنجاح , يوجد بعض الاشخاص قد تم ارسال دعوة لهم من قبل ");

                    if (trainingAttendance.PendingUserProfileId.Count == 1 && x > 0)
                        return Ok("تم ارسال دعوة الانضمام من قبل");

                    return Ok("تم الارسال بنجاح");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        //notification
        [HttpPost(Name = "AcceptClosedTrainingAttendanceForPersons")]
        public async Task<IActionResult> AcceptTrainingAttendanceForPersons(long UserProfileId, long TrainingInvitationPendingId, long RealTimeNotificationid = 0)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var TrainingInvitationPending = _context.TrainingInvitationPendings
                            .Where(x => x.TrainingInvitationPendingId == TrainingInvitationPendingId).FirstOrDefault();
                    if (TrainingInvitationPending == null) return NotFound("حدث خطا ,قد يكون قد تم حذف هذة الدعوة !");

                    var Training = _context.Training.Where(x => x.TrainingId == TrainingInvitationPending.TrainingId && x.IsActive == true).FirstOrDefault();
                    if (Training == null) return NotFound("حدث خطا ,قد يكون قد تم حذف هذا التدريب  !");
                    var trainingTypeid = Training.TrainingTypeId;

                    var checkpendinguserprofile = _context.UserProfiles.Where(x => x.UserProfileId == TrainingInvitationPending.PendingUserProfileId).FirstOrDefault();
                    if (checkpendinguserprofile == null) return NotFound("لم يتم العثور على الشخص الموجة الية الدعوة!");

                    var pendinguserprofilename = _context.Users.FirstOrDefault(x => x.UserId == checkpendinguserprofile.UserId).Name;

                    string message = null;
                    long newNotificationToUserProfileId = 0;

                    if (RealTimeNotificationid == 0)//open
                    {
                        var trainingadmin = _context.TrainingAdmins.Where(x => x.TrainingId == TrainingInvitationPending.TrainingId
                                                 && x.AdminUserProfileId == UserProfileId).FirstOrDefault();
                        if (trainingadmin == null) return Unauthorized("ليس لديك هذة الصلاحية");

                        message = $" تم قبول طلبك للانضمام لتدريب {Training.TrainingName}";
                        newNotificationToUserProfileId = TrainingInvitationPending.PendingUserProfileId;
                    }
                    else//close
                    {
                        if (TrainingInvitationPending.PendingUserProfileId != UserProfileId) return BadRequest("هذه الدعوة غير موجه اليك!");
                        message = $" تم قبول الدعوة للانضمام للتمرين من قبل {pendinguserprofilename}";
                        newNotificationToUserProfileId = TrainingInvitationPending.TrainingOwnerUserProfileId;

                    }

                    var UserDataForNotification = await _userFunctions.GetUserDataForNotificationByProfileId(newNotificationToUserProfileId, UserProfileId);
                    var trainingdetails = _context.TrainingDetails.Where(x => x.TrainingId == TrainingInvitationPending.TrainingId && x.TrainingDate >= DateTime.Now).ToList();

                    foreach (var trainingdetail in trainingdetails)
                    {
                        var newtrainingattendance = new TrainingAttendance
                        {
                            UserProfileId = TrainingInvitationPending.TrainingOwnerUserProfileId,
                            TrainingDetailsId = trainingdetail.TrainingDetailsId,
                            AttendanceUserProfileId = TrainingInvitationPending.PendingUserProfileId,
                            IsPresent = false,
                            AttendanceTypeId = checkpendinguserprofile.RoleId,
                        };
                        _context.TrainingAttendances.Add(newtrainingattendance);
                    }
                    _context.SaveChanges();

                    //close
                    var newNotification = new RealTimeNotification
                    {
                        FromUserProfileId = UserProfileId,
                        ToUserProfileId = newNotificationToUserProfileId,
                        TargetTypeId = 5,
                        TargetId = (long)Training.TrainingId,
                        IsRead = false,
                        ContentMessage = message,
                    };
                    _context.RealTimeNotifications.Add(newNotification);
                    _context.SaveChanges();

                    var SelectedNotification = _context.RealTimeNotifications.Where(x => x.InvitationId == TrainingInvitationPending.TrainingInvitationPendingId).FirstOrDefault();
                    if (SelectedNotification != null) _context.RealTimeNotifications.Remove(SelectedNotification);

                    if (TrainingInvitationPending != null)
                        _context.TrainingInvitationPendings.Remove(TrainingInvitationPending);

                    _context.SaveChanges();


                    var newRealTimeNotification = await _notification.CreateRealTimeNotificationModel(UserDataForNotification, message, checkpendinguserprofile);
                    await _notificationService.SendNotification(newRealTimeNotification);


                    transaction.Commit();
                    return Ok("تمت قبول الدعوة بنجاح");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        //notification
        [HttpPost(Name = "RequestToJoinopenTraining")]
        public async Task<IActionResult> RequestToJoinopenTraining(long userprofileid, long trainingid)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var training = _context.Training.FirstOrDefault(x => x.TrainingId == trainingid && x.IsActive == true);
                    if (training == null)
                        return NotFound("حدث خطأ، قد يكون قد تم حذف هذا التدريب!");

                    if (training.TrainingTypeId == 2)
                        return BadRequest("!يمكن اضافتك فقط عن طريق صاحب التدريب");

                    var userProfile = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == userprofileid);
                    if (userProfile == null)
                        return NotFound("لم يتم العثور على هذا المستخدم!");

                    var existingInvitation = _context.TrainingInvitationPendings
                        .FirstOrDefault(x => x.TrainingId == trainingid && x.PendingUserProfileId == userprofileid);
                    if (existingInvitation != null)
                        return Ok("تم تقديم طلب قيد الانتظار للانضمام لهذا التدريب من قبل!");

                    if (training.EndDate < DateTime.Now)
                        return Ok("تم الانتهاء من هذا التدريب!");

                    var username = _context.Users.FirstOrDefault(x => x.UserId == userProfile.UserId)?.Name ?? null;

                    var newInvitation = new TrainingInvitationPending
                    {
                        TrainingOwnerUserProfileId = training.UserProfileId,
                        PendingUserProfileId = userprofileid,
                        TrainingId = trainingid,
                    };
                    _context.TrainingInvitationPendings.Add(newInvitation);
                    _context.SaveChanges();
                    //for notification
                    var UserDataForNotification = await _userFunctions.GetUserDataForNotificationByProfileId(newInvitation.TrainingOwnerUserProfileId, userprofileid);

                    var message = $"تم تقديم طلب من قبل {username} للانضمام للتدريب {training.TrainingName}";
                    var newNotification = new RealTimeNotification
                    {
                        FromUserProfileId = userprofileid,
                        ToUserProfileId = newInvitation.TrainingOwnerUserProfileId,
                        TargetTypeId = 5,
                        TargetId = (long)training.TrainingId,
                        InvitationId = newInvitation.TrainingInvitationPendingId,
                        IsRead = false,
                        ContentMessage = message,
                    };
                    _context.RealTimeNotifications.Add(newNotification);
                    _context.SaveChanges();

                    if (existingInvitation != null)
                        _context.TrainingInvitationPendings.Remove(existingInvitation);

                    _context.SaveChanges();

                    //sendNotification
                    var TraininUserProfile = _context.UserProfiles.Where(x => x.UserProfileId == newInvitation.TrainingOwnerUserProfileId).FirstOrDefault();
                    //var UserDataForNotification = await _userFunctions.GetUserDataForNotificationByProfileId(newInvitation.TrainingOwnerUserProfileId, userprofileid);
                    var newRealTimeNotification = await _notification.CreateRealTimeNotificationModel(UserDataForNotification, message, TraininUserProfile);
                    await _notificationService.SendNotification(newRealTimeNotification);

                    transaction.Commit();
                    return Ok("تم ارسال طلب الانضمام للتدريب بنجاح");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }


        [HttpDelete(Name = "RejectRequestToJoinTraining")]
        public async Task<IActionResult> RejectRequestToJoinTraining(long userprofileid, long TrainingInvitationPendingId, bool isopen = true)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var TrainingInvitationPending = _context.TrainingInvitationPendings
                                .Where(x => x.TrainingInvitationPendingId == TrainingInvitationPendingId).FirstOrDefault();
                    if (TrainingInvitationPending == null)
                        return NotFound("حدث خطا ,قد يكون قد تم حذف هذا الطلب !");


                    var Training = _context.Training.Where(x => x.TrainingId == TrainingInvitationPending.TrainingId && x.IsActive == true).FirstOrDefault();
                    if (Training == null)
                        return NotFound("حدث خطا ,قد يكون قد تم حذف هذا التدريب  !");


                    if (isopen == true)
                    {
                        //الللى مقدم الطلب ممكن يسمسح طلبة او الادمن

                        var trainingadmin = _context.TrainingAdmins.Where(x => x.TrainingId == TrainingInvitationPending.TrainingId
                                                     && x.AdminUserProfileId == userprofileid).FirstOrDefault();

                        if (trainingadmin != null || TrainingInvitationPending.PendingUserProfileId == userprofileid)
                            _context.TrainingInvitationPendings.Remove(TrainingInvitationPending);

                        else
                            return Unauthorized("ليس لديك هذه الصلاحية");
                    }
                    else
                    {
                        if (TrainingInvitationPending.PendingUserProfileId != userprofileid)
                            return Unauthorized("هذه الدعوة غير موجه اليك!");

                        _context.TrainingInvitationPendings.Remove(TrainingInvitationPending);
                    }

                    var SelectedNotification = _context.RealTimeNotifications.Where(x => x.InvitationId == TrainingInvitationPending.TrainingInvitationPendingId).FirstOrDefault();
                    if (SelectedNotification != null) _context.RealTimeNotifications.Remove(SelectedNotification);

                    _context.SaveChanges();
                    transaction.Commit();
                    return Ok("تم الالغاء بنجاح");

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        //updated
        [HttpGet(Name = "GetRequestsToJoinopenTraining")]
        public async Task<IActionResult> GetRequestsToJoinopenTraining(long userprofileId, long trainingId)
        {
            try
            {
                var defaulimage = "http://mobile.hawisports.com/image/defultprofile.png";

                var training = _context.Training.Where(x => x.TrainingId == trainingId && x.IsActive == true)
                    .FirstOrDefault();

                if (training == null)
                    return NotFound("حدث خطأ، قد يكون قد تم حذف هذا التدريب!");

                var trainingadmins = _context.TrainingAdmins
                    .Where(x => x.TrainingId == trainingId && x.AdminUserProfileId == userprofileId)
                    .FirstOrDefault();

                var checkRequestOfUserForTraining = _context.TrainingInvitationPendings
                    .Where(x => x.TrainingId == trainingId
                    && x.PendingUserProfileId == userprofileId
                    //
                    && x.IsInvited == false)
                    .FirstOrDefault();

                List<TrainingInvitationPending> requests = new List<TrainingInvitationPending>();

                if (trainingadmins != null)
                    requests = _context.TrainingInvitationPendings
                        .Where(x => x.TrainingId == trainingId
                         && x.IsInvited == false)
                        .ToList();

                else if (checkRequestOfUserForTraining != null)
                    requests = _context.TrainingInvitationPendings
                        .Where(x => x.TrainingId == trainingId
                            && x.PendingUserProfileId == userprofileId
                            && x.IsInvited == false)
                        .ToList();
                else
                    requests = requests.ToList();

                List<GetRequestsToJoinTrainingDto> users = new List<GetRequestsToJoinTrainingDto>();

                foreach (var x in requests)
                {
                    var UserProfile = _context.UserProfiles.Where(n => n.UserProfileId == x.PendingUserProfileId).FirstOrDefault();
                    var newusers = new GetRequestsToJoinTrainingDto
                    {
                        TrainingInvitationPendingId = x.TrainingInvitationPendingId,
                        UserProfileId = x.PendingUserProfileId,
                        Name = _context.Users.FirstOrDefault(z => z.UserId == UserProfile.UserId).Name ?? null,
                        Image = _context.UserProfileImages
                            .Where(n => n.UserProfileId == x.PendingUserProfileId && n.ImageTypeId == 1)
                            .Select(c => c.Image.ImageUrlfullPath)
                            .FirstOrDefault() ?? defaulimage,
                        type = _context.Roles.FirstOrDefault(n => n.RoleId == UserProfile.RoleId).Role1,
                        trainingtypeId = x.Training.TrainingTypeId,

                    };
                    users.Add(newusers);
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "Trainingpreparetation")]
        public async Task<IActionResult> preparetation(long UserProfileId, long TrainingDetailsId, [FromBody] TrainingpreparetationDto trainingpr)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var TrainingDetails = _context.TrainingDetails.Where(x => x.TrainingDetailsId == TrainingDetailsId).FirstOrDefault();
                    if (TrainingDetails == null)
                        return NotFound("لم يتم العثور على التمرين");

                    var CheckUserProfile = _context.TrainingAdmins.Where(x => x.AdminUserProfileId == UserProfileId && x.TrainingId == TrainingDetails.TrainingId && x.IsActive == true).FirstOrDefault();
                    if (CheckUserProfile == null)
                        return NotFound("لا يمكنك القيام بذلك المهام!");

                    foreach (var AttendanceId in trainingpr.AttendanceId)
                    {
                        var existingAttendance = await _context.TrainingAttendances
                            .FirstOrDefaultAsync(a => a.AttendanceId == AttendanceId && a.TrainingDetailsId == TrainingDetailsId);

                        if (existingAttendance != null)
                        {
                            if (!existingAttendance.IsPresent == false)
                            {
                                var trainingEvaluation = _context.TrainingEvaluations.Where(x => x.AttendanceId == existingAttendance.AttendanceId && x.Attendance.TrainingDetailsId == existingAttendance.TrainingDetailsId).FirstOrDefault();
                                if (trainingEvaluation != null)
                                {
                                    _context.TrainingEvaluations.Remove(trainingEvaluation);
                                    _context.SaveChanges();
                                }
                            }
                            existingAttendance.IsPresent = !existingAttendance.IsPresent;
                        }
                        else
                            return NotFound("يجب ان يكون كل الاشخاص الذى تقوم بتحضيرهم داخل التمرين !");
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                    return Ok("تم التحضير بنجاح");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost(Name = "TrainingEvaluation")]
        public async Task<IActionResult> TrainingEvaluation(long UserProfileId, long TrainingDetailsId, [FromBody] TrainingEvaluationDto training)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var TrainingDetails = _context.TrainingDetails.Where(x => x.TrainingDetailsId == TrainingDetailsId).FirstOrDefault();
                    if (TrainingDetails == null)
                        return NotFound("لم يتم العثور على التمرين");

                    var CheckUserProfile = _context.TrainingAdmins.Where(x => x.AdminUserProfileId == UserProfileId && x.TrainingId == TrainingDetails.TrainingId && x.IsActive == true).FirstOrDefault();
                    if (CheckUserProfile == null)
                        return NotFound("لا يمكنك القيام بذلك المهام!");

                    foreach (var evaluationDto in training.User)
                    {
                        var attendance = _context.TrainingAttendances.Where(x => x.AttendanceId == evaluationDto.AttendanceId && x.IsPresent == true).FirstOrDefault();
                        if (attendance == null)
                            return NotFound($"لم يتم العثور على حضور التدريب برقم الحضور {evaluationDto.AttendanceId}");

                        // var checkTrainingEvaluation=_context.TrainingEvaluations.Where(x=>x.AttendanceId == evaluationDto.AttendanceId && x.)
                        var trainingEvaluation = new TrainingEvaluation
                        {
                            UserProfileId = UserProfileId,
                            AttendanceId = evaluationDto.AttendanceId,
                            OutOfFive = evaluationDto.OutOfFive ?? 0,
                            EvaluationComment = evaluationDto.EvaluationComment,
                        };

                        _context.TrainingEvaluations.Add(trainingEvaluation);
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return Ok("تم اضافة التقييم بنجاح");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost(Name = "TrainingAdmins")]
        public async Task<IActionResult> TrainingAdmins(long UserProfileId, long TrainingId, [FromBody] TrainingAdminDto admins)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var Training = _context.Training.Where(x => x.TrainingId == TrainingId).FirstOrDefault();
                    if (Training == null)
                        return NotFound("لم يتم العثور على التمرين");

                    if (Training.UserProfileId != UserProfileId)
                        return Unauthorized("ليس لديك صلاحية هذا الدور !");

                    foreach (var admin in admins.AdminUserProfileId)
                    {
                        var checkAdmin = _context.TrainingAdmins.Where(x => x.AdminUserProfileId == admin && x.TrainingId == TrainingId && x.IsActive == true).FirstOrDefault();
                        if (checkAdmin == null)
                        {

                            var checkactivate = _context.TrainingAdmins.Where(x => x.AdminUserProfileId == admin && x.TrainingId == TrainingId && x.IsActive == false).FirstOrDefault();
                            if (checkactivate != null)
                                checkactivate.IsActive = true;
                            else
                            {
                                var newAdmin = new TrainingAdmin
                                {
                                    TrainingId = TrainingId,
                                    AdminUserProfileId = admin,
                                    IsActive = true
                                };
                                _context.TrainingAdmins.Add(newAdmin);
                            }
                        }
                    }

                    _context.SaveChanges();
                    transaction.Commit();
                    return Ok("تم الاضافة بنجاح");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost(Name = "TrainingInjury")]
        public async Task<IActionResult> TrainingInjury(long UserProfileId, long TrainingDetailsId, [FromBody] TrainingInjuryDto injery)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var TrainingDetails = _context.TrainingDetails.Where(x => x.TrainingDetailsId == TrainingDetailsId).FirstOrDefault();
                    if (TrainingDetails == null) return NotFound("لم يتم العثور على التمرين");

                    var CheckUserProfile = _context.TrainingAdmins.Where(x => x.AdminUserProfileId == UserProfileId && x.TrainingId == TrainingDetails.TrainingId && x.IsActive == true).FirstOrDefault();
                    if (CheckUserProfile == null) return NotFound("لا يمكنك القيام بذلك المهام!");


                    var attendance = _context.TrainingAttendances.Where(x => x.AttendanceId == injery.AttendanceId && x.IsPresent == true).FirstOrDefault();
                    if (attendance == null) return NotFound("لم يتم التحضير لهذا الشخص!");

                    var checkinjery = _context.TrainingInjuries.Where(x => x.TrainingDetailsId == TrainingDetailsId && x.AttendanceId == injery.AttendanceId).FirstOrDefault();
                    if (checkinjery != null) return BadRequest("!تم اضافة اصابة لهذا الشخص فى نفس التمرين");

                    foreach (var x in injery.injuryDtos.GetType().GetProperties())
                    {
                        // Check if the property value is true
                        if (x.GetValue(injery.injuryDtos) is bool propertyValue && propertyValue)
                        {
                            var checkInjuryPositionId = _context.TrainingInjuryInjuryPositions.Where(n => n.InjuryPositionInEnglish == x.Name).FirstOrDefault();
                            if (checkInjuryPositionId != null)
                            {
                                var TrainingInjury = new TrainingInjury
                                {
                                    UserProfileId = UserProfileId,
                                    AttendanceId = injery.AttendanceId,
                                    InjuryPositionId = checkInjuryPositionId.InjuryPositionId,
                                    InjuryComment = injery.InjuryComment ?? null,
                                    TrainingDetailsId = TrainingDetailsId,
                                };
                                _context.TrainingInjuries.Add(TrainingInjury);
                            }
                        }
                    }
                    _context.SaveChanges();
                    transaction.Commit();

                    return Ok("تم اضافة الاصابة بنجاح");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut(Name = "UpdateTrainingDetails")]
        public async Task<IActionResult> UpdateTrainingDetails(long TrainingDetailsId, long userprofileId, [FromBody] UpdateTrainingDetailsDto trainingDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var TrainingDetails = _context.TrainingDetails.Where(x => x.TrainingDetailsId == TrainingDetailsId).FirstOrDefault();
                    if (TrainingDetails == null)
                        return NotFound("لم يتم العثور على التمرين");

                    var CheckUserProfile = _context.TrainingAdmins.Where(x => x.AdminUserProfileId == userprofileId && x.TrainingId == TrainingDetails.TrainingId && x.IsActive == true).FirstOrDefault();
                    if (CheckUserProfile == null)
                        return NotFound("لا يمكنك القيام بذلك المهام!");

                    TrainingDetails.TrainingDate = trainingDto.TrainingDate;
                    TrainingDetails.DurationInMinutes = trainingDto.DurationInMinutes;
                    TrainingDetails.TrainingTime = trainingDto.TrainingTime;
                    TrainingDetails.TrainingCost = trainingDto.TrainingCost;
                    TrainingDetails.PlaygroundSizeId = trainingDto.PlaygroundSizeId;
                    TrainingDetails.PlaygroundFloorId = trainingDto.PlaygroundFloorId;
                    TrainingDetails.TrainingPlaceLocation = trainingDto.TrainingPlaceLocation;

                    _context.SaveChanges();
                    transaction.Commit();
                    return Ok("تم التعديل بنجاح");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut(Name = "UpdateTrainingEvaluation")]
        public async Task<IActionResult> UpdateTrainingEvaluation(long TrainingDetailsId, long EvaluationId, long userprofileId, [FromBody] UpdateEvaluationDto trainingDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var TrainingEvaluation = _context.TrainingEvaluations.Where(x => x.EvaluationId == EvaluationId).FirstOrDefault();
                    if (TrainingEvaluation == null)
                        return NotFound("لم يتم العثور على التقييم");

                    var TrainingDetails = _context.TrainingDetails.Where(x => x.TrainingDetailsId == TrainingDetailsId).FirstOrDefault();
                    if (TrainingDetails == null)
                        return NotFound("لم يتم العثور على التمرين");

                    var CheckUserProfile = _context.TrainingAdmins.Where(x => x.AdminUserProfileId == userprofileId && x.TrainingId == TrainingDetails.TrainingId && x.IsActive == true).FirstOrDefault();
                    if (CheckUserProfile == null)
                        return NotFound("لا يمكنك القيام بذلك المهام!");

                    TrainingEvaluation.UserProfileId = userprofileId;
                    TrainingEvaluation.OutOfFive = trainingDto.OutOfFive;
                    TrainingEvaluation.EvaluationComment = trainingDto.EvaluationComment;

                    _context.SaveChanges();
                    transaction.Commit();
                    return Ok("تم التعديل بنجاح");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut(Name = "UpdateTrainingInjury")]
        public async Task<IActionResult> UpdateTrainingInjury(long TrainingDetailsId, long userprofileId, [FromBody] TrainingInjuryDto injery)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var TrainingDetails = _context.TrainingDetails.Where(x => x.TrainingDetailsId == TrainingDetailsId).FirstOrDefault();
                    if (TrainingDetails == null) return NotFound("!لم يتم العثور على التمرين");

                    var CheckUserProfile = _context.TrainingAdmins.Where(x => x.AdminUserProfileId == userprofileId && x.TrainingId == TrainingDetails.TrainingId && x.IsActive == true).FirstOrDefault();
                    if (CheckUserProfile == null) return NotFound("!لا يمكنك القيام بذلك المهام");

                    //var checkinjery = _context.TrainingInjuries.Where(x => x.TrainingDetailsId == TrainingDetailsId && x.AttendanceId == injery.AttendanceId).FirstOrDefault();
                    //if (checkinjery == null) return BadRequest("!لم يم اضافة اصابة لهذا الشخص");

                    foreach (var x in injery.injuryDtos.GetType().GetProperties())
                    {
                        var checkinjeryposition = _context.TrainingInjuries.Where(n => n.TrainingDetailsId == TrainingDetailsId && n.AttendanceId == injery.AttendanceId && n.InjuryPosition.InjuryPositionInEnglish == x.Name).FirstOrDefault();
                        // Check if the property value is true
                        if (x.GetValue(injery.injuryDtos) is bool propertyValue && propertyValue)
                        {
                            var checkInjuryPositionId = _context.TrainingInjuryInjuryPositions.Where(n => n.InjuryPositionInEnglish == x.Name).FirstOrDefault();
                            if (checkInjuryPositionId != null)
                            {

                                if (checkinjeryposition == null)
                                {
                                    var TrainingInjury = new TrainingInjury
                                    {
                                        UserProfileId = userprofileId,
                                        AttendanceId = injery.AttendanceId,
                                        InjuryPositionId = checkInjuryPositionId.InjuryPositionId,
                                        InjuryComment = injery.InjuryComment ?? null,
                                        TrainingDetailsId = TrainingDetailsId,
                                    };
                                    _context.TrainingInjuries.Add(TrainingInjury);
                                }
                            }
                        }
                        else
                        {
                            if (checkinjeryposition != null)
                            {
                                _context.TrainingInjuries.Remove(checkinjeryposition);
                            }
                        }
                    }

                    _context.SaveChanges();
                    transaction.Commit();
                    return Ok("تم التعديل بنجاح");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete(Name = "DeleteTraining")]
        public async Task<IActionResult> DeleteTraining(long TrainingId, long TrainingDetailsId, long UserProfileId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var CheckTraining = _context.Training.Where(x => x.TrainingId == TrainingId).FirstOrDefault();
                    if (CheckTraining == null)
                        return NotFound("!لم يتم العثور على التدريب الاساسى لهذا التدريب");

                    var CheckTrainingDetails = _context.TrainingDetails.Where(x => x.TrainingDetailsId == TrainingDetailsId && x.IsActive == true).FirstOrDefault();
                    if (CheckTrainingDetails == null)
                        return NotFound("لم يتم العثور على هذا التدريب!");

                    if (CheckTraining.UserProfileId != UserProfileId)
                        return BadRequest("لا يمكنك حذف هذا التدريب !");

                    CheckTrainingDetails.IsActive = false;

                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    return Ok("تم حذف التمرين بنجاح");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete(Name = "DeleteAdmin")]
        public async Task<IActionResult> DeleteAdmin(long TrainingId, long UserProfileId, long AdminUserProfileId)
        {
            try
            {
                var CheckTraining = _context.Training.Where(x => x.TrainingId == TrainingId).FirstOrDefault();
                if (CheckTraining == null)
                    return NotFound("لم يتم العثور على هذا التمرين!");

                var checkadmin = _context.TrainingAdmins.Where(x => x.AdminUserProfileId == AdminUserProfileId && x.TrainingId == TrainingId).FirstOrDefault();
                if (checkadmin == null)
                    return BadRequest("هذا الشخص ليس من ضمن الادمن لهذا التمرين");

                if (CheckTraining.UserProfileId == AdminUserProfileId)
                    return BadRequest("!لا يمكنك اذالة صاحب التمرين");

                var CheckUserProfile = _context.TrainingAdmins.Where(x => x.AdminUserProfileId == UserProfileId && x.TrainingId == TrainingId && x.IsActive == true).FirstOrDefault();
                if (CheckUserProfile == null)
                    return NotFound("لا يمكنك القيام بذلك المهام!");


                checkadmin.IsActive = false;
                _context.SaveChanges();
                return Ok("تم الاذالة بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //delete only from new 
        [HttpDelete(Name = "DeleteTrainingAttendance")]
        public async Task<IActionResult> DeleteTrainingAttendance(long TrainingDetailsId, long UserProfileId, long AttendanceUserProfileID)
        {
            try
            {

                var currentTime = DateTime.Now;

                var CheckTraining = _context.TrainingDetails
                    .Include(td => td.Training)
                    .Where(x => x.TrainingDetailsId == TrainingDetailsId)
                    .FirstOrDefault();

                if (CheckTraining == null)
                    return NotFound("لم يتم العثور على هذا التمرين!");


                var CheckUserProfile = _context.TrainingAdmins.Where(x => x.AdminUserProfileId == UserProfileId && x.TrainingId == CheckTraining.TrainingId && x.IsActive == true).FirstOrDefault();
                if (CheckUserProfile == null)
                    return NotFound("لا يمكنك القيام بذلك المهام!");

                var trainingid = CheckTraining.TrainingId;

                var attendanceToRemove = _context.TrainingAttendances
                    .Where(a =>
                        a.AttendanceUserProfileId == AttendanceUserProfileID &&
                        a.TrainingDetails.TrainingId == trainingid &&
                        (a.TrainingDetails.TrainingDate >= currentTime /*|| a.TrainingDetails.TrainingTime > currentTime.TimeOfDay*/)
                    )
                    .ToList();

                if (attendanceToRemove.Count == 0)
                    return NotFound("لا يمكنك الحذف من التدريبات السابقة!");

                var checkAdmin = _context.TrainingAdmins.Where(x => x.TrainingId == CheckTraining.TrainingId && x.AdminUserProfileId == AttendanceUserProfileID).FirstOrDefault();
                if (checkAdmin != null)
                {
                    _context.TrainingAdmins.Remove(checkAdmin);
                    _context.SaveChanges();
                }
                _context.TrainingAttendances.RemoveRange(attendanceToRemove);
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
//1716
