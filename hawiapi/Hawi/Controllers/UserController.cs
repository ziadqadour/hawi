using Hawi.Dtos;
using Hawi.Extensions;
using Hawi.Models;
using Hawi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hawi.Controllers
{
    [Route("hawi/User/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HawiContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserFunctions _userFunctions;
        private readonly ImageFunctions _ImageFunctions;
        private readonly Authentiaction _Authentiaction;

        public UserController(HawiContext context, ImageFunctions ImageFunctions, Authentiaction Authentiaction, IConfiguration configuration, UserFunctions userFunctions)
        {
            _configuration = configuration;
            _userFunctions = userFunctions;
            _context = context;
            _ImageFunctions = ImageFunctions;
            _Authentiaction = Authentiaction;
        }


        [HttpGet(Name = "GetUserRoles")]
        public IActionResult GetUserRoles(long userId)
        {
            var userExists = _context.Users.Any(u => u.UserId == userId);
            if (!userExists)
                return NotFound($"لم يتم العثور مستخدم بذلك الهوية {userId}");

            var userRoles = _context.UserProfiles
                .Where(x => x.UserId == userId)
                .Select(x => new
                {
                    x.Role.RoleId,
                    x.Role.Role1,
                    NumOfUnReadNotification = _context.RealTimeNotifications
                      .Count(n => n.ToUserProfileId == x.UserProfileId && n.IsRead == false)
                })
                .OrderBy(x => x.RoleId);

            return Ok(userRoles);
        }

        [HttpGet(Name = "Getjointdata")]
        public async Task<IActionResult> Getjointdata(long UserProfileId, long userthatVisitProfile)
        {
            try
            { //data that common for all (user & sportinstituation ) at  top account

                var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileId).FirstOrDefault();
                if (userprofile == null) return NotFound("!لم يتم العثور على هذا المستخدم");

                var subscripe = _context.Subscriptions.Where(x => x.SourceUserProfileId == UserProfileId).FirstOrDefault();
                long follows = (subscripe == null) ? 0 : _context.Subscriptions.Count(x => x.SourceUserProfileId == UserProfileId);
                long follwers = _context.Subscriptions.Count(x => x.TargetUserProfileId == UserProfileId);

                bool isfollow = false;
                var CheckuserthatVisitProfile = _context.UserProfiles.Where(x => x.UserProfileId == userthatVisitProfile).FirstOrDefault();

                if (CheckuserthatVisitProfile != null)
                    isfollow = _context.Subscriptions.Any(x => x.SourceUserProfileId == userthatVisitProfile && x.TargetUserProfileId == UserProfileId);

                var user = _context.Users.Where(x => x.UserId == userprofile.UserId).FirstOrDefault();


                LocationInfo locationInfo = _userFunctions.GetUserLocation(userprofile);
                var rolename = _context.Roles.FirstOrDefault(x => x.RoleId == userprofile.RoleId).Role1;

                var selectedrole = new GetjointdataDto
                {
                    UserProfileId = UserProfileId,
                    userid = user.UserId,
                    Role = rolename,
                    RoleId = userprofile.RoleId,
                    Location = locationInfo.LLocation,
                    LocationUrl = locationInfo.LocationUrl,
                    Name = _userFunctions.GetUserName(userprofile.RoleId, UserProfileId, user.UserId),
                    ImgUrl = _userFunctions.GetUserImage(UserProfileId, userprofile.RoleId) ?? null,
                    Folowers = follwers,
                    follows = follows,
                    IsFollow = isfollow,
                    Description = userprofile.Description,
                    backgroundImage = _userFunctions.GetUserBackgroundImage(UserProfileId, userprofile.RoleId) ?? null
                };

                return Ok(selectedrole);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(long UserProfileId)
        {//by userprofileid
            var user = _context.UserProfiles.Where(u => u.UserProfileId == UserProfileId).FirstOrDefault();
            if (user == null) return BadRequest($"!لا يوجد مستخدم بتلك الهوية");

            var userimage = _userFunctions.GetUserImage(user.UserProfileId, user.RoleId);
            var username = _userFunctions.GetUserName(user.RoleId, user.UserProfileId, user.UserId);
            var selecteduser = _context.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
            var selecteduserrole = _context.Roles.Where(x => x.RoleId == user.RoleId).FirstOrDefault();

            var selectedUser = new UserDtos
            {
                UserId = user.UserId,
                Name = username,
                Mobile = selecteduser.Mobile,
                IsMale = selecteduser.IsMale,
                Password = user.User.Password,
                ImgUrl = userimage,
                UserRole = selecteduserrole.Role1,
                LastLoginDate = selecteduser.LastLoginDate
            };
            return Ok(selectedUser);
        }

        [HttpGet(Name = "GetFavoriteClub")]
        public IActionResult GetFavoriteClub(string? search = null)
        {
            var clubs = _context.Clubs
                .Select(x => new
                {
                    x.ClubId,
                    x.ClubArabicName,
                    x.ClubEnglishName,
                    x.ClubLogoUrlfullPath
                }).
                ToList();

            if (!string.IsNullOrEmpty(search) && search != null)
                clubs = clubs.Where(c => c.ClubArabicName?.Contains(search, StringComparison.OrdinalIgnoreCase) == true || c.ClubEnglishName?.Contains(search, StringComparison.OrdinalIgnoreCase) == true).ToList();

            return Ok(clubs);
        }

        [HttpGet(Name = "GetQualificationType")]
        public IActionResult GetQualificationType()
        {
            var Qualification = _context.UserBasicDataQualificationTypes
                .Select(x => new
                {
                    x.QualificationTypeId,
                    x.QualificationType

                })
                .ToList();
            return Ok(Qualification);
        }

        [HttpGet(Name = "GetPlayerPlace")]
        public IActionResult GetPlayerPlace()
        {
            var places = _context.PlayerPlayerPlaces.Select(x => new
            {
                x.PlayerPlaceId,
                x.PlayerPlace
            })
              .ToList();

            return Ok(places);
        }

        [HttpGet(Name = "GetPlayerFeet")]
        public IActionResult GetPlayerFeet()
        {
            var feets = _context.PlayerPlayerFeet.Select(x => new
            {
                x.PlayerFeetId,
                x.FootName
            })
              .ToList();

            return Ok(feets);
        }

        [HttpGet(Name = "GetFullUserBasicData")]
        public IActionResult GetFullUserBasicData(long userid)
        {
            var User = _context.Users.Where(x => x.UserId == userid).FirstOrDefault();
            if (User == null) return NotFound("لم يتم العثور على المستخدم ");

            var selectedUserBasicData = _userFunctions.GetFullUserBasicdata(userid);
            if (selectedUserBasicData == null) return Ok();

            return Ok(selectedUserBasicData);
        }

        // can delete it if replace with search api"AppSearch"
        [HttpGet(Name = "SearchUserByPhone")]
        public async Task<IActionResult> SearchUserByPhone(string phoneNumber, long UserProfileID, long branchId = 0)
        {
            try
            { //branchId=0 "IF SEARCH IN branch " , else >> if search in all system

                if (string.IsNullOrEmpty(phoneNumber)) return BadRequest("ادخل رقم هاتف!");

                var searchResult = _userFunctions.SearchUserByPhone(phoneNumber, UserProfileID, branchId);
                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet(Name = "GetUserImageByUserProfileID")]
        public IActionResult GetUserImageByUserProfileID([FromQuery] OwnerParameters ownerParameters, long userprofileid, byte isalpom = 1)
        {
            try
            { // isalpom == 1 >> return image as album , 0 >> return all image not in album

                var selectedIteams = _userFunctions.getUserImageByUserProfileID(ownerParameters, userprofileid, isalpom);

                return Ok(selectedIteams);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetAlbumByID")]
        public IActionResult GetAlbumByID([FromQuery] OwnerParameters ownerParameters, long AlbumId, bool Isdefault, long userprofileid)
        {
            try
            { //default mean image that "user didn't add album for it " like "post , training , profile ....." ,
              //if not default mean that user has create that album 
                if (Isdefault == false)
                {
                    var userAlbums = _context.ImageAlbums
                    .Where(album => album.ImageAlbumsId == AlbumId)
                    .SelectMany(album => album.ImageAlbumImages
                    .Select(albumImage => new ImageInfo
                    {
                        ImageId = albumImage.Image.ImageId,
                        ImageUrl = albumImage.Image.ImageUrlfullPath,
                        Date = albumImage.CreateDate,
                        IsVideo = false,
                    })
                    )
                    .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                    .Take(ownerParameters.PageSize)
                    .ToList();
                    return Ok(userAlbums);
                }
                else
                {

                    if (AlbumId == 1)//article
                    {

                        var articleImages = _context.ArticleImages
                         .Where(ai => ai.Article.UserProfileId == userprofileid && ai.Article.IsActive == true)
                         .Select(ai => new ImageInfo
                         {
                             ImageId = ai.ImageId,
                             ImageUrl = ai.Image.ImageUrlfullPath,
                             Date = ai.Image.CreateDate,
                             IsVideo = false,
                             //  alpumId = 1,
                         })
                         .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                         .Take(ownerParameters.PageSize)
                         .ToList();
                        return Ok(articleImages);
                    }
                    if (AlbumId == 2)//sportinstituation
                    {

                        var sportInstitutionlogoImages = _context.SportInstitutions
                       .Where(si => si.UserProfileId == userprofileid)
                       .Select(si => new ImageInfo
                       {
                           ImageUrl = si.LogoUrlfullPath,
                           Date = si.CreateDate,
                           IsVideo = false,
                           // alpumId = 2,
                       })
                       .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                       .Take(ownerParameters.PageSize)
                       .ToList();
                        return Ok(sportInstitutionlogoImages);
                    }
                    if (AlbumId == 3) //training
                    {

                        var trainingImages = _context.Training
                       .Where(t => t.UserProfileId == userprofileid && (t.TrainingImageId != null || t.TrainingImageId != 0))
                       .Select(t => new ImageInfo
                       {
                           ImageId = t.TrainingImage.ImageId,
                           ImageUrl = t.TrainingImage.ImageUrlfullPath,
                           Date = t.CreateDate,
                           IsVideo = false,
                           //  alpumId = 3,
                       })
                       .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                       .Take(ownerParameters.PageSize)
                       .ToList();
                        return Ok(trainingImages);
                    }
                    if (AlbumId == 4) //profile
                    {
                        var userProfileImages = _context.UserProfileImages
                       .Where(upi => upi.UserProfileId == userprofileid && upi.ImageTypeId == 1)
                       .Select(upi => new ImageInfo
                       {
                           ImageId = upi.ImageId,
                           ImageUrl = upi.Image.ImageUrlfullPath,
                           Date = upi.CreateDate,
                           IsVideo = false,
                           // alpumId = 4,
                       })
                       .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                       .Take(ownerParameters.PageSize)
                       .ToList();
                        return Ok(userProfileImages);
                    }
                    if (AlbumId == 5) //uniform
                    {

                        var uniformImages = _context.Uniforms
                       .Where(u => u.UserProfileId == userprofileid)
                       .Select(u => new ImageInfo
                       {
                           ImageId = u.UniformImage.ImageId,
                           ImageUrl = u.UniformImage.ImageUrlfullPath,
                           Date = u.CreateDate,
                           IsVideo = false,
                           //  alpumId = 5,
                       })
                       .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                       .Take(ownerParameters.PageSize)
                       .ToList();
                        return Ok(uniformImages);
                    }
                    if (AlbumId == 6) //article video
                    {
                        var articleViedio = _context.ArticleVideos
                       .Where(ai => ai.Article.UserProfileId == userprofileid && ai.Article.IsActive == true)
                       .Select(ai => new ImageInfo
                       {
                           ImageId = ai.VideoId,
                           ImageUrl = ai.Video.VideoUrl,
                           Date = ai.Video.CreateDate,
                           IsVideo = true,
                           // alpumId = 6,
                       })
                       .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                       .Take(ownerParameters.PageSize)
                       .ToList();
                        return Ok(articleViedio);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        ///////////////////////////////////////////////////////////
        [HttpPost(Name = "ChangeLoginToSpecificRole")]
        public async Task<IActionResult> ChangeLoginToSpecificRole(byte RoleId, long UserId)
        {
            using (var transication = _context.Database.BeginTransaction())
            {
                try
                {
                    var userProfile = _context.UserProfiles.Where(up => up.UserId == UserId && up.RoleId == RoleId).FirstOrDefault();
                    if (userProfile == null) return BadRequest($"!ليس لديك الصلاحية لاستخدام هذه الهوية ");

                    var token = await _Authentiaction.GenerateJwtToken(_configuration, UserId, RoleId, _userFunctions, _context);
                    if (token == null)
                        return BadRequest("!حدث خطا اثناء تسجيل الدخول");
                    transication.Commit();
                    return Ok(token);
                }
                catch (Exception ex)
                {
                    transication.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost(Name = "AddFullUserBasicData")]
        public IActionResult AddFullUserBasicData(long userid, [FromBody] AddFullUserBasicDataDTO user)
        {
            try
            {
                var userprofile = _context.Users.Where(x => x.UserId == userid).FirstOrDefault();
                if (userprofile == null) return NotFound("لم يتم العثور على المستخدم ");

                var selectedUserBasicData = _context.UserBasicData.Where(x => x.UserId == userid).FirstOrDefault();
                if (selectedUserBasicData != null) return BadRequest("لقد قمت باضافة البيانات الاساسية من قبل ");

                var newUserBasicdata = new UserBasicDatum
                {
                    UserId = userid,
                    Idnumber = user.Idnumber,
                    FirstName = user.FirstName ?? null,
                    FatherName = user.FatherName,
                    GrandFatherName = user.GrandFatherName,
                    FamilyName = user.FamilyName,
                    Height = user.Height,
                    Weight = user.Weight,
                    Dob = user.Dob,
                    Pob = user.Pob,
                    PlaceOfResidence = user.PlaceOfResidence,
                    NationalityId = (user.NationalityId != 0) ? user.NationalityId : null,
                    LocalFavoritePlayer = user.LocalFavoritePlayer,
                    InternationalFavoritePlayer = user.InternationalFavoritePlayer,
                    LocalFavoriteClubId = (user.LocalFavoriteClubId != 0) ? user.LocalFavoriteClubId : null,
                    InternationalFavoriteClubId = (user.InternationalFavoriteClubId != 0) ? user.InternationalFavoriteClubId : null,
                    InternationalFavoriteTeamId = (user.InternationalFavoriteTeamId != 0) ? user.InternationalFavoriteTeamId : null,
                    QualificationTypeId = (user.QualificationTypeId != 0) ? user.QualificationTypeId : null,
                    PlayerFeetId = user.PlayerFeetId ?? null,
                    PlayerSecondaryPlaceId = user.PlayerSecondaryPlaceId ?? null,
                    PlayerMainPlaceId = user.PlayerMainPlaceId ?? null,
                };
                _context.UserBasicData.Add(newUserBasicdata);
                _context.SaveChanges();

                return Ok("تم الحفظ بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost(Name = "AddNewAlbumForSpeceficUser")]
        public async Task<IActionResult> AddNewAlbumForSpeceficUser([FromBody] AddAlpum alpum)
        {
            try
            {
                var userProfile = _context.UserProfiles.Where(u => u.UserProfileId == alpum.UserProfileId).FirstOrDefault();
                if (userProfile == null) return NotFound("!لم يتم العثور على المستخدم");

                var checkalpum = _context.ImageAlbums.Where(x => x.AlbumName.ToLower() == alpum.AlpumName.ToLower() && x.UserProfileId == alpum.UserProfileId).FirstOrDefault();
                if (checkalpum != null) return BadRequest("!الالبوم الذى ادخلتة موجود مسبقا");

                var newalbum = new ImageAlbum
                {
                    UserProfileId = alpum.UserProfileId,
                    AlbumName = alpum.AlpumName,
                };
                _context.ImageAlbums.Add(newalbum);
                _context.SaveChanges();
                return Ok(newalbum.ImageAlbumsId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "AddImagesInAlbum")]
        public async Task<IActionResult> AddImagesInAlbum([FromForm] AddImagesInAlpum AlpumImages)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var userProfile = _context.UserProfiles.Where(u => u.UserProfileId == AlpumImages.UserProfileId).FirstOrDefault();
                    if (userProfile == null) return NotFound("!لم يتم العثور على المستخدم");

                    var userphone = _context.Users.FirstOrDefault(x => x.UserId == userProfile.UserId).Mobile;

                    var checkalpum = _context.ImageAlbums.Where(x => x.ImageAlbumsId == AlpumImages.AlbumId).FirstOrDefault();
                    if (checkalpum == null) return BadRequest("!الالبوم الذى ادخلتة غير موجود");

                    foreach (var image in AlpumImages.Images)
                    {
                        string errorMessage = _ImageFunctions.GetInvalidImageMessage(image);
                        if (errorMessage != null) return BadRequest(errorMessage);
                    }

                    foreach (var image in AlpumImages.Images)
                    {
                        try
                        {
                            string englishFileName = _ImageFunctions.ChangeImageName(image);

                            var directory = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{userphone}\";
                            await _ImageFunctions.CheckDirectoryExist(directory);
                            var path = directory + englishFileName;

                            var uploadtoserverresult = await _ImageFunctions.UploadImageToServerAsync(image, path);
                            if (uploadtoserverresult != null)
                                return BadRequest(uploadtoserverresult);

                            var ImageUrlfullPath = $"http://mobile.hawisports.com/image/Users/{userphone}/" + englishFileName;
                            var saveimageinDB = await _ImageFunctions.SaveImageInDBAsync(ImageUrlfullPath, image.FileName, 13, true);
                            if (!long.TryParse(saveimageinDB, out long imageId))
                            {
                                return BadRequest(saveimageinDB);
                            }
                            long imageid = long.Parse(saveimageinDB);

                            var SaveImageAlbumInDB = await _ImageFunctions.AddImageAlbumImageAsync(AlpumImages.AlbumId, imageid);
                            if (SaveImageAlbumInDB != null)
                                return BadRequest($"{SaveImageAlbumInDB}");

                        }
                        catch (Exception ex)
                        {
                            return BadRequest(ex.Message);
                        }
                    }
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
        //refactor
        [HttpPost(Name = "AddUserProfileImage")]
        public async Task<IActionResult> AddUserProfileImageAsync(long userprofileid, byte imageTypeProfileOrBackground, [FromForm] AddImageDto image)
        {
            using (var transication = _context.Database.BeginTransaction())
            {
                try
                {
                    //imageTypeProfileOrBackground (1)? "profile" :background
                    #region validation 
                    var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == userprofileid).FirstOrDefault();
                    if (userprofile == null) return NotFound("لم يتم العثور على المستخدم ");

                    var checkimage = _ImageFunctions.GetInvalidImageMessage(image.image);
                    if (checkimage != null) return BadRequest(checkimage);

                    if (imageTypeProfileOrBackground < 1 && imageTypeProfileOrBackground > 2)
                        return BadRequest("!ادخل نوع صحيح للصورة التى تريد تغيرها");

                    var user = _context.Users.FirstOrDefault(x => x.UserId == userprofile.UserId);
                    var roleid = userprofile.RoleId;
                    string englishFileName = _ImageFunctions.ChangeImageName(image.image);
                    string ReturnedImage = null;
                    #endregion

                    #region SportInstitution image
                    if (roleid == 5 || roleid == 7 || roleid == 8 || roleid == 11)
                    {
                        var existingSportInstitution = _context.SportInstitutions
                            .Where(x => x.UserProfileId == userprofileid).FirstOrDefault();

                        if (imageTypeProfileOrBackground == 1)
                        {
                            if (existingSportInstitution.LogoUrlfullPath != null) return BadRequest(" !يوجد صورة بروفايل بالفعل");
                        }
                        else if (imageTypeProfileOrBackground == 2)
                        {
                            if (existingSportInstitution.BackGroundUrlfullPath != null) return BadRequest("! يوجد صورة لخلفية البروفايل بالفعل");
                        }

                        var directory = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\SportInstitution\{user.Mobile}\";
                        await _ImageFunctions.CheckDirectoryExist(directory);
                        var photopath = directory + englishFileName;

                        using (var stream = new FileStream(photopath, FileMode.Create))
                        {
                            await image.image.CopyToAsync(stream);
                        }

                        var newPath = $"http://mobile.hawisports.com/image/SportInstitution/{user.Mobile}/" + englishFileName;

                        if (imageTypeProfileOrBackground == 1)
                        {
                            existingSportInstitution.LogoFileName = englishFileName;
                            existingSportInstitution.LogoUrlfullPath = newPath;
                        }
                        else
                        {
                            existingSportInstitution.BackGroundFileName = englishFileName;
                            existingSportInstitution.BackGroundUrlfullPath = newPath;
                        }
                        _context.SaveChanges();
                        ReturnedImage = newPath;
                    }
                    #endregion

                    #region user image
                    else
                    {
                        if (imageTypeProfileOrBackground == 1)
                        {
                            var userimage = _context.UserProfileImages.Where(x => x.UserProfileId == userprofileid && x.ImageTypeId == 1).FirstOrDefault();
                            if (userimage != null) return BadRequest("!يوجد صوة بروفايل بالفعل");
                        }

                        if (imageTypeProfileOrBackground == 2)
                        {
                            var userimage = _context.UserProfileImages.Where(x => x.UserProfileId == userprofileid && x.ImageTypeId == 12).FirstOrDefault();
                            if (userimage != null) return BadRequest("!يوجد صوة لخلفية البروفايل بالفعل");
                        }

                        var directory = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{user.Mobile}\";
                        await _ImageFunctions.CheckDirectoryExist(directory);
                        var path = directory + englishFileName;
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await image.image.CopyToAsync(stream);
                        }

                        byte userimagetypeid = 0;
                        if (imageTypeProfileOrBackground == 1)
                            userimagetypeid = 1;
                        else if (imageTypeProfileOrBackground == 2)
                            userimagetypeid = 12;

                        var img = new Models.Image
                        {
                            ImageUrlfullPath = $"http://mobile.hawisports.com/image/Users/{user.Mobile}/" + englishFileName,
                            ImageFileName = englishFileName,
                            ImageTypeId = userimagetypeid,
                            IsActive = true,
                        };
                        _context.Images.Add(img);
                        _context.SaveChanges();

                        var UserProfileImage = new UserProfileImage
                        {
                            UserProfileId = userprofileid,
                            ImageId = img.ImageId,
                            ImageTypeId = userimagetypeid,
                        };
                        _context.UserProfileImages.Add(UserProfileImage);
                        _context.SaveChanges();

                        ReturnedImage = img.ImageUrlfullPath;
                    }
                    transication.Commit();
                    return Ok(ReturnedImage);
                    #endregion
                }
                catch (Exception ex)
                {
                    transication.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        //refactor
        [HttpPut(Name = "EditUserProfileImage")]
        public async Task<IActionResult> EditUserProfileImageAsync(long userprofileid, byte imageTypeProfileOrBackground, [FromForm] AddImageDto image)
        {
            using (var transication = _context.Database.BeginTransaction())
            {
                try
                {
                    #region validation
                    var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == userprofileid).FirstOrDefault();
                    if (userprofile == null) return NotFound("!لم يتم العثور على المستخدم ");

                    var checkimage = _ImageFunctions.GetInvalidImageMessage(image.image);
                    if (checkimage != null) return BadRequest(checkimage);

                    if (imageTypeProfileOrBackground < 1 && imageTypeProfileOrBackground > 2)
                        return BadRequest("!ادخل نوع صحيح للصورة التى تريد تغيرها");

                    var user = _context.Users.FirstOrDefault(x => x.UserId == userprofile.UserId);
                    var roleid = userprofile.RoleId;
                    string englishFileName = _ImageFunctions.ChangeImageName(image.image);


                    string ReturnedImage = null;
                    #endregion

                    #region  SportInstitution image

                    if (roleid == 5 || roleid == 7 || roleid == 8 || roleid == 11)
                    {

                        var existingSportInstitution = _context.SportInstitutions
                            .Where(x => x.UserProfileId == userprofileid).FirstOrDefault();

                        if (imageTypeProfileOrBackground == 1)
                        {
                            if (existingSportInstitution.LogoUrlfullPath == null)
                                return BadRequest("لا يوجد صورة بروفايل !");
                        }
                        else if (imageTypeProfileOrBackground == 2)
                        {
                            if (existingSportInstitution.BackGroundUrlfullPath == null)
                                return BadRequest("لا يوجد صورة لخلفية البروفايل !");
                        }

                        //delete Old one
                        if (imageTypeProfileOrBackground == 1)
                        {
                            if (existingSportInstitution.LogoFileName != null)
                            {
                                var oldLogo = existingSportInstitution.LogoFileName;
                                var oldLogoPath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\SportInstitution\{user.Mobile}\" + oldLogo;
                                if (System.IO.File.Exists(oldLogoPath))
                                {
                                    System.IO.File.Delete(oldLogoPath);
                                }
                            }
                        }
                        else if (imageTypeProfileOrBackground == 2)
                        {
                            if (existingSportInstitution.BackGroundFileName != null)
                            {
                                var oldLogo = existingSportInstitution.BackGroundFileName;
                                var oldLogoPath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\SportInstitution\{user.Mobile}\" + oldLogo;
                                if (System.IO.File.Exists(oldLogoPath))
                                {
                                    System.IO.File.Delete(oldLogoPath);
                                }
                            }
                        }


                        //add newOne
                        var directory = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\SportInstitution\{user.Mobile}\";
                        await _ImageFunctions.CheckDirectoryExist(directory);
                        var photopath = directory + englishFileName;

                        using (var stream = new FileStream(photopath, FileMode.Create))
                        {
                            await image.image.CopyToAsync(stream);
                        }
                        var newPath = $"http://mobile.hawisports.com/image/SportInstitution/{user.Mobile}/" + englishFileName;

                        if (imageTypeProfileOrBackground == 1)
                        {
                            existingSportInstitution.LogoFileName = englishFileName;
                            existingSportInstitution.LogoUrlfullPath = newPath;
                        }
                        else if (imageTypeProfileOrBackground == 2)
                        {
                            existingSportInstitution.BackGroundFileName = englishFileName;
                            existingSportInstitution.BackGroundUrlfullPath = newPath;
                        }
                        _context.SaveChanges();


                        ReturnedImage = newPath;
                    }
                    #endregion

                    #region user image
                    else
                    {
                        byte userimagetypeid = 0;
                        if (imageTypeProfileOrBackground == 1)
                            userimagetypeid = 1;
                        else if (imageTypeProfileOrBackground == 2)
                            userimagetypeid = 12;

                        var userimage = _context.UserProfileImages.Where(x => x.UserProfileId == userprofileid && x.ImageTypeId == userimagetypeid).FirstOrDefault();
                        if (userimage == null) return BadRequest("لا يوجد صورة مضافة!");

                        #region delete pevious image
                        var selectedimage = _context.Images.Where(x => x.ImageId == userimage.ImageId).FirstOrDefault();
                        var imagepath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{user.Mobile}\" + selectedimage.ImageFileName;

                        _context.UserProfileImages.Remove(userimage);
                        _context.Images.Remove(selectedimage);
                        _context.SaveChanges();

                        if (System.IO.File.Exists(imagepath))
                        {
                            System.IO.File.Delete(imagepath);
                        }

                        #endregion

                        #region AddNewImage

                        var directory = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{user.Mobile}\";
                        await _ImageFunctions.CheckDirectoryExist(directory);
                        var path = directory + englishFileName;
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await image.image.CopyToAsync(stream);
                        }

                        var img = new Models.Image
                        {
                            ImageUrlfullPath = $"http://mobile.hawisports.com/image/Users/{user.Mobile}/" + englishFileName,
                            ImageFileName = englishFileName,
                            ImageTypeId = userimagetypeid,
                            IsActive = true,
                        };
                        _context.Images.Add(img);
                        _context.SaveChanges();

                        var UserProfileImage = new UserProfileImage
                        {
                            UserProfileId = userprofileid,
                            ImageId = img.ImageId,
                            ImageTypeId = userimagetypeid,
                        };
                        _context.UserProfileImages.Add(UserProfileImage);
                        _context.SaveChanges();

                        ReturnedImage = img.ImageUrlfullPath;
                        #endregion
                    }

                    transication.Commit();
                    return Ok(ReturnedImage);
                    #endregion
                }
                catch (Exception ex)
                {
                    transication.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut(Name = "Add&UpdateDescription")]
        public IActionResult UpdateDescription(long UserProfileId, [FromBody] DescriptionDto descriptionDto)
        {
            try
            { // add description "pioe"
                var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileId).FirstOrDefault();
                if (userprofile == null) return NotFound("لم يتم العثور على المستخدم ");

                userprofile.Description = descriptionDto.Description;
                _context.UserProfiles.Update(userprofile);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(Name = "EditFullUserBasicData")]
        public IActionResult EditFullUserBasicData(long userid, long UserBasicDataId, [FromBody] AddFullUserBasicDataDTO User)
        {
            try
            {
                var userprofile = _context.Users.Where(x => x.UserId == userid).FirstOrDefault();
                if (userprofile == null) return NotFound("لم يتم العثور على المستخدم ");

                var selecteduser = _context.Users.Where(x => x.UserId == userid).FirstOrDefault();

                var selectedUserBasicData = _context.UserBasicData.Where(x => x.UserBasicDataId == UserBasicDataId).FirstOrDefault();
                if (selectedUserBasicData == null) return NotFound("لم تقم باضافة اى بيانات بعد!");

                if (User.CityCountryId != null && User.CityCountryId.HasValue)
                    selecteduser.CityCountryId = User.CityCountryId;

                if (User.CityId != null && User.CityId.HasValue)
                    selecteduser.CityId = User.CityId;

                if (User.UserNeckName != null)
                    selecteduser.Name = User.UserNeckName;

                selectedUserBasicData.Idnumber = User.Idnumber;
                selectedUserBasicData.FirstName = User.FirstName;
                selectedUserBasicData.FatherName = User.FatherName;
                selectedUserBasicData.GrandFatherName = User.GrandFatherName;
                selectedUserBasicData.FamilyName = User.FamilyName;
                selectedUserBasicData.Height = User.Height;
                selectedUserBasicData.Weight = User.Weight;
                selectedUserBasicData.Dob = User.Dob;
                selectedUserBasicData.Pob = User.Pob;
                selectedUserBasicData.PlaceOfResidence = User.PlaceOfResidence;
                selectedUserBasicData.NationalityId = (User.NationalityId != 0) ? User.NationalityId : null;
                selectedUserBasicData.LocalFavoritePlayer = User.LocalFavoritePlayer;
                selectedUserBasicData.InternationalFavoritePlayer = User.InternationalFavoritePlayer;
                selectedUserBasicData.LocalFavoriteClubId = (User.LocalFavoriteClubId != 0) ? User.LocalFavoriteClubId : null;
                selectedUserBasicData.InternationalFavoriteClubId = (User.InternationalFavoriteClubId != 0) ? User.InternationalFavoriteClubId : null;
                selectedUserBasicData.InternationalFavoriteTeamId = (User.InternationalFavoriteTeamId != 0) ? User.InternationalFavoriteTeamId : null;
                selectedUserBasicData.QualificationTypeId = (User.QualificationTypeId != 0) ? User.QualificationTypeId : null;
                selectedUserBasicData.PlayerFeetId = User.PlayerFeetId ?? null;
                selectedUserBasicData.PlayerMainPlaceId = User.PlayerMainPlaceId ?? null;
                selectedUserBasicData.PlayerSecondaryPlaceId = User.PlayerSecondaryPlaceId ?? null;
                _context.SaveChanges();
                return Ok("تم الحفظ بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(Name = "AddUserBasicDataImage")]
        public async Task<IActionResult> AddUserBasicDataImageAsync([FromForm] AddUserBasicDataImageDTO User)
        {
            try
            {
                var selecteduser = _context.Users.Where(x => x.UserId == User.UserId).FirstOrDefault();
                if (selecteduser == null) return NotFound("لم يتم العثور على المستخدم ");

                var selectedUserBasicData = _context.UserBasicData.Where(x => x.UserBasicDataId == User.UserBasicDataId).FirstOrDefault();
                if (selectedUserBasicData == null) return NotFound("لم تقم باضافة اى بيانات بعد!");

                string userImageDirectory = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{selecteduser.Mobile}";

                //delete Previous Image
                if (selectedUserBasicData.OfficialPhotoForMatches != null)
                {
                    var ExistingImageName = Path.GetFileName(Path.GetDirectoryName(selectedUserBasicData.OfficialPhotoForMatches));
                    var PathOfExistingImageInServer = Path.Combine(userImageDirectory, ExistingImageName);

                    if (System.IO.File.Exists(PathOfExistingImageInServer))
                    {

                        System.IO.File.Delete(PathOfExistingImageInServer);
                    }

                }

                //add new one 
                await _ImageFunctions.CheckDirectoryExist(userImageDirectory);
                string GenerateNewName = _ImageFunctions.ChangeImageName(User.Image);
                string imagePath = Path.Combine(userImageDirectory, GenerateNewName);

                await _ImageFunctions.CheckDirectoryExist(userImageDirectory);
                var uploadImageResult = await _ImageFunctions.UploadImageToServerAsync(User.Image, imagePath);

                if (uploadImageResult != null)
                    return BadRequest(uploadImageResult);

                selectedUserBasicData.OfficialPhotoForMatches = $"http://mobile.hawisports.com/image/Users/{selecteduser.Mobile}/{GenerateNewName}";

                _context.SaveChanges();
                return Ok("تم الحفظ بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(Name = "Deletealpum")]
        public IActionResult Deletealpum(long alpumid, long userprofileid)
        {
            using (var transication = _context.Database.BeginTransaction())
            {
                try
                {
                    var checkalpum = _context.ImageAlbums.Where(x => x.UserProfileId == userprofileid && x.ImageAlbumsId == alpumid).FirstOrDefault();
                    if (checkalpum == null)
                        return NotFound("!لم يتم العثور على الالبوم");

                    var getuserprofile = _context.UserProfiles.Where(x => x.UserProfileId == userprofileid).FirstOrDefault();
                    var userphone = _context.Users.FirstOrDefault(x => x.UserId == getuserprofile.UserId).Mobile;

                    var imagealpum = _context.ImageAlbumImages.Where(x => x.AlbumId == alpumid).ToList();

                    if (imagealpum != null)
                    {
                        foreach (var iteam in imagealpum)
                        {
                            var image = _context.Images.Where(x => x.ImageId.Equals(iteam.ImageId)).FirstOrDefault();
                            var imagePath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{userphone}\" + image.ImageFileName;

                            if (image != null)
                            {
                                _context.ImageAlbumImages.Remove(iteam);
                                _context.Images.Remove(image);
                            }
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                            _context.SaveChanges();
                        }
                    }

                    _context.ImageAlbums.Remove(checkalpum);
                    _context.SaveChanges();
                    transication.Commit();
                    return Ok("!تم الحذف بنجاح");
                }
                catch (Exception ex)
                {
                    transication.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete(Name = "DeleteImageInalpum")]
        public IActionResult DeleteImageInalpum(long alpumid, long imageid, long userprofileid)
        {
            using (var transication = _context.Database.BeginTransaction())
            {
                try
                {
                    var checkImage = _context.ImageAlbumImages.Where(x => x.AlbumId == alpumid && x.ImageId == imageid).FirstOrDefault();
                    if (checkImage == null)
                        return NotFound("!لم يتم العثور على الصورة داخل الالبوم");

                    var checkalpum = _context.ImageAlbums.Where(x => x.UserProfileId == userprofileid && x.ImageAlbumsId == alpumid).FirstOrDefault();
                    if (checkalpum == null)
                        return NotFound("!لا يمكنك الحذف داخل هذا الالبوم ");

                    var getuserprofile = _context.UserProfiles.Where(x => x.UserProfileId == userprofileid).FirstOrDefault();
                    var userphone = _context.Users.FirstOrDefault(x => x.UserId == getuserprofile.UserId).Mobile;

                    var imagealpum = _context.ImageAlbumImages.Where(x => x.AlbumId == alpumid && x.ImageId == imageid).FirstOrDefault();
                    if (imagealpum != null)
                    {
                        var image = _context.Images.Where(x => x.ImageId.Equals(imagealpum.ImageId)).FirstOrDefault();
                        var imagePath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{userphone}\" + image.ImageFileName;

                        if (image != null)
                        {
                            _context.ImageAlbumImages.Remove(imagealpum);
                            _context.Images.Remove(image);
                        }
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        _context.SaveChanges();
                    }
                    transication.Commit();
                    return Ok("!تم الحذف بنجاح");
                }
                catch (Exception ex)
                {
                    transication.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete(Name = "DeleteAllUserAcounts")]
        public IActionResult DeleteAllUserAcounts(long UserId)
        {
            try
            {
                var user = _context.Users.Where(x => x.UserId == UserId && x.IsActive == true).FirstOrDefault();
                if (user == null) return NotFound("!لم يتم العثور على المستخدم الذى ادخلته ");

                var checkDisActivatedUser = _context.UsersUsersActivationArchieves.Where(x => x.UserId == UserId).FirstOrDefault();
                if (checkDisActivatedUser == null)
                {
                    var newDisActivatedUser = new UsersUsersActivationArchieve
                    {
                        UserId = UserId,
                        DateToDelete = DateTime.UtcNow,
                    };
                    _context.UsersUsersActivationArchieves.Add(newDisActivatedUser);
                }
                user.IsActive = false;

                _context.SaveChanges();
                return Ok("!سوف يتم حذف الحساب بعد 60 يوم اذا لم تتم اى عمليه تسجيل دخول فى تلك الفتره");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "AppSearch")]
        public async Task<IActionResult> AppSearch([FromQuery] SearchParameters search)
        {
            try
            {
                string defaultTrainingImage = "http://mobile.hawisports.com/image/coverprofileapp.png";

                var searchTermLower = search.SearchTerm.ToLower();
                var userProfileId = search.UserProfileId;
                //Articles
                if (search.SearchTypeId == 1)
                {
                    var Articlesresults = _context.Articles
                        .Where(x =>
                            (
                            (x.UserProfile.IsActive == true && x.UserProfile.User.IsActive == true)
                            && (x.Title.ToLower().Contains(searchTermLower)
                            || x.ArticleText.ToLower().Contains(searchTermLower)
                            || (search.date != null && x.ArticleCreateDate.Date == search.date.Value.Date))))
                        .Skip((search.PageNumber - 1) * search.PageSize)
                        .Take(search.PageSize)
                        .Select(post => new GetPostsDto
                        {
                            ArticleId = post.ArticleId,
                            ImageCount = _context.ArticleImages.Count(x => x.ArticleId == post.ArticleId),
                            IsActive = post.IsActive,
                            Title = post.Title,
                            ArticleText = post.ArticleText,
                            ArticleCreateDate = post.ArticleCreateDate,
                            // ImageUrlfullPath = _userFunctions.GetUserImage(post.UserProfileId, post.UserProfile.RoleId),
                            ImageUrlfullPath = _context.ArticleImages
                                .Where(x => x.ArticleId == post.ArticleId && (x.ImageTypeId == 2 || x.ImageTypeId == 3))
                                .OrderBy(x => x.ImageTypeId)
                                .Select(x => (_context.Images.FirstOrDefault(i => i.ImageId == x.ImageId) ?? new Image()).ImageUrlfullPath)
                                .FirstOrDefault(),
                            VideoUrlfullPath = _context.ArticleVideos
                                .Where(video => video.ArticleId == post.ArticleId)
                                .Select(video => _context.Videos.FirstOrDefault(v => v.VideoId == video.VideoId).VideoUrl)
                                .FirstOrDefault(),
                            UserId = post.UserProfile.UserId,
                            UserProfileId = post.UserProfileId,
                            UserName = _userFunctions.GetUserName(post.UserProfile.RoleId, post.UserProfileId, post.UserProfile.UserId),
                            UserRole = post.UserProfile.Role.Role1,
                            UserImg = _userFunctions.GetUserImage(post.UserProfileId, post.UserProfile.Role.RoleId),
                            UserLikeReact = _context.ArticleLikes
                                .Any(like => like.LikeUserProfileId == userProfileId && like.ArticleId == post.ArticleId),
                            UserCommentReact = _context.ArticleComments
                                .Any(comment => comment.CommentUserProfileId == userProfileId && comment.ArticleId == post.ArticleId)
                        })
                        .OrderBy(x => x.ArticleCreateDate)
                        .ToList();
                    return Ok(Articlesresults);
                }
                //SportInstitutions
                if (search.SearchTypeId == 2)
                {
                    var sportinstituationsearch = _context.SportInstitutions.Where(x =>
                                                                  (x.UserProfile.IsActive == true && x.UserProfile.User.IsActive == true)
                                                                 && ((search.date != null && x.DateCreated == search.date)
                                                                 || x.SportInstitutionName.ToLower().Contains(searchTermLower)
                                                                 || x.SportInstitutionBranches.Any(branch => branch.City.CityArabicName.ToLower() == searchTermLower)
                                                                 || x.SportInstitutionBranches.Any(branch => branch.CityDistricts.ToLower() == searchTermLower)
                                                                 || x.FounderName.ToLower().Contains(searchTermLower)))
                                                             .Skip((search.PageNumber - 1) * search.PageSize)
                                                            .Take(search.PageSize)
                                                            .Select(x => new
                                                            {
                                                                SportInstitutionId = x.SportInstitutionId,
                                                                UserProfileId = x.UserProfileId,
                                                                SportInstitutionName = x.SportInstitutionName ?? null,
                                                                FounderName = x.FounderName ?? null,
                                                                LogoUrlfullPath = x.LogoUrlfullPath ?? null,
                                                                Description = _context.UserProfiles.FirstOrDefault(n => n.UserProfileId == x.UserProfileId).Description ?? null,
                                                            })
                                                            .ToList();
                    return Ok(sportinstituationsearch);
                }
                //Events
                if (search.SearchTypeId == 3)
                {
                    var eventSearch = _context.Events.Where(x =>
                                                             x.EventTitle.ToLower().Contains(searchTermLower)
                                                             || x.EventText.ToLower().Contains(searchTermLower)
                                                             || (search.date != null && x.EventCreateDate.Date == search.date.Value.Date /*|| x.StratDate.Value.Date == search.date.Value.Date || x.FinishDate.Value.Date == search.date.Value.Date*/)
                                                             || x.EventPlaceLocation.ToLower().Contains(searchTermLower))
                                                     .Skip((search.PageNumber - 1) * search.PageSize)
                                                    .Take(search.PageSize)
                                                     .OrderByDescending(e => e.StratDate)
                                                     .Select(e => new
                                                     {
                                                         e.EventId,
                                                         e.IsActive,
                                                         e.EventPlaceLocation,
                                                         e.FinishDate,
                                                         e.StratDate,
                                                         e.EventText,
                                                         e.EventTitle,
                                                         EventVideoUrlfullPath = e.EventVideos.FirstOrDefault().Video.VideoUrl,
                                                         mainImage = e.EventImages.Any() ? e.EventImages.First().Image.ImageUrlfullPath : null,
                                                         imagescount = e.EventImages.Count(),
                                                         e.LastUpdate,
                                                         UserId = e.UserProfile.UserId
                                                     })
                                                     .ToList();
                    return Ok(eventSearch);
                }
                //Training
                if (search.SearchTypeId == 4)
                {
                    var TrainingSearch = _context.TrainingDetails
                        .Where(t =>
                            (t.Training.UserProfile.User.IsActive == true && t.Training.UserProfile.IsActive == true)
                            && (t.Training.UserProfile.User.Name.ToLower().Contains(searchTermLower)
                                || t.Training.UserProfile.User.Mobile.ToLower().Contains(searchTermLower)
                                || t.Training.TrainingName.ToLower().Contains(searchTermLower)
                                || t.Training.TrainingDetails.Any(tr => tr.TrainingCost.ToLower() == searchTermLower)
                                || t.Training.TrainingDetails.Any(tr => tr.TrainingPlaceLocation.ToLower().Contains(searchTermLower))
                                || (search.date != null && t.CreateDate.Value.Date == search.date.Value.Date)))
                        .Include(t => t.Training)
                        .Skip((search.PageNumber - 1) * search.PageSize)
                        .Take(search.PageSize)
                        .ToList();


                    var resutl = TrainingSearch.Select(x =>
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
                            //IsInTraining = false,
                            IsEnd = (x.TrainingDate.Value.AddDays(1) >= DateTime.Now) ? false : true,
                            IsRepeated = x.Training.IsRepeated ?? null,
                            //IsOpen = false,
                        };
                    }).ToList();

                    return Ok(TrainingSearch);
                }



                ////Training
                //if (search.SearchTypeId == 4)
                //{
                //    var TrainingSearch = _context.Training.Where(t =>
                //                                          (t.UserProfile.User.IsActive == true && t.UserProfile.IsActive == true)

                //                                                   && (t.UserProfile.User.Name.ToLower().Contains(searchTermLower)
                //                                                || t.UserProfile.User.Mobile.ToLower().Contains(searchTermLower)
                //                                                || t.TrainingName.ToLower().Contains(searchTermLower)
                //                                                || t.TrainingDetails.Any(tr => tr.TrainingCost.ToLower() == searchTermLower)
                //                                                || t.TrainingDetails.Any(tr => tr.TrainingPlaceLocation.ToLower().Contains(searchTermLower))
                //                                                || (search.date != null && t.CreateDate.Value.Date == search.date.Value.Date)))
                //                                                .Skip((search.PageNumber - 1) * search.PageSize)
                //                                                .Take(search.PageSize)
                //                                                .Select(t => new
                //                                                {
                //                                                    t.TrainingId,
                //                                                    t.UserProfileId,
                //                                                    t.TrainingName,
                //                                                    TrainingType = t.TrainingType.TrainingType ?? null,
                //                                                    TrainingTypeid = t.TrainingTypeId,
                //                                                    Season = t.Season.SeasonName,
                //                                                    ImageUrlfullPath = (t.TrainingImageId != null) ? t.TrainingImage.ImageUrlfullPath : defaultTrainingImage,
                //                                                    t.StartDate,
                //                                                    t.EndDate,
                //                                                    t.IsRepeated,
                //                                                    t.CreateDate,
                //                                                    SportInstitutionBranchId = _context.TrainingBranches
                //                                                       .Where(x => x.TrainingId == t.TrainingId)
                //                                                       .Select(x => (long?)x.SportInstitutionBranchId)
                //                                                       .FirstOrDefault(),
                //                                                    trainingadmins = _context.TrainingAdmins.Where(x => x.TrainingId == t.TrainingId).Select(n => n.AdminUserProfileId).ToList()
                //                                                }).ToList();
                //    return Ok(TrainingSearch);
                //}

                //user (هاوى لاعب حكم مدرب)
                if (search.SearchTypeId == 6 || search.SearchTypeId == 7 || search.SearchTypeId == 8 || search.SearchTypeId == 9)
                {
                    var roleid = search.SearchTypeId switch
                    {
                        6 => 2,   // player
                        7 => 3,   // referee
                        8 => 4,   // coach
                        9 => 1,   // hawi
                        _ => 0    // default or handle other cases if necessary
                    };

                    var userSearch = _context.UserProfiles.Where(x =>
                                                            (x.User.IsActive == true && x.IsActive == true && x.RoleId == roleid)
                                                          && (x.User.Name.ToLower().Contains(searchTermLower)
                                                          || x.Role.Role1.ToLower() == searchTermLower
                                                          || x.User.Mobile.ToLower().Contains(searchTermLower)
                                                          || x.User.City.CityArabicName.ToLower() == searchTermLower
                                                          || x.User.City.CityEnglishName.ToLower() == searchTermLower
                                                          || x.User.CityCountry.CountryArabicName.ToLower() == searchTermLower
                                                          || x.User.CityCountry.CountryEnglishName.ToLower() == searchTermLower))
                                                      .Select(x => new
                                                      {
                                                          userprofileid = x.UserProfileId,
                                                          Role = x.Role.Role1,
                                                          name = _userFunctions.GetUserName(x.RoleId, x.UserProfileId, x.UserId),
                                                          UserImg = _userFunctions.GetUserImage(x.UserProfileId, x.RoleId),
                                                      }).ToList();
                    return Ok(userSearch);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "searchtype")]
        public IActionResult searchtype()
        {
            var searchtype = _context.SearchTypes.Select(x => new
            {
                x.SearchTypeId,
                x.SearchKay,
            }).ToList();
            return Ok(searchtype);
        }
        //////////////////////////////////////////////////////////// User Details
        [HttpGet(Name = "GetUserDetails")]
        public IActionResult GetUserDetails()
        {
            var SportInstitutions = _context.userdetails
                .Select(x => new
                {
                    x.Id,
                    x.ParentMobil,
                    x.ParentChild,
                }).ToList();
            return Ok(SportInstitutions);
        }
        [HttpGet(Name = "GetUserDetailsByMobile")]
        public async Task<IActionResult> GetUserDetailsByMobile(string Mobile)
        {
            var userdetail = _context.userdetails.Where(u => u.ParentMobil == Mobile).FirstOrDefault();
            if (userdetail == null) return BadRequest($"!لا يوجد مستخدم بتلك الهوية");

            var selectedUser = new UserDetails
            {
                Id = userdetail.Id,
                ParentMobil = userdetail.ParentMobil,
                ParentChild = userdetail.ParentChild,
            };
            return Ok(selectedUser);
        }
        [HttpPost(Name = "AddUserDetails")]
        public IActionResult AddUserDetails([FromBody] UserDetails userDetails)
        {
            try
            {
                if (userDetails == null) return BadRequest();
                else
                {

                    userDetails.ParentMobil = userDetails.ParentMobil;
                    userDetails.ParentChild = userDetails.ParentChild;
                    _context.userdetails.Add(userDetails);
                    _context.SaveChanges();
                }

                return Ok("تم الاضافة بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut(Name = "UpdateUserDetails")]
        public IActionResult UpdateUserDetails(long Id, [FromBody] UserDetails userDetails)
        {
            try
            {

                var checkUserd = _context.userdetails.Where(x => x.Id == Id).FirstOrDefault();
                if (checkUserd == null) return NotFound("لم يتم العثور على المستخدم");
                else
                {
                    checkUserd.ParentMobil = userDetails.ParentMobil;
                    checkUserd.ParentChild = userDetails.ParentChild;
                }
                _context.userdetails.Update(checkUserd);
                _context.SaveChanges();

                return Ok(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpDelete(Name = "DeleteUserDetails")]
        public IActionResult DeleteUserDetails(long Id)
        {
            try
            {

                var checkUserDetails = _context.userdetails.Where(x => x.Id == Id).FirstOrDefault();
                if (checkUserDetails == null) return NotFound("لم يتم العثور على المستخدم");
                else
                {
                    _context.userdetails.Remove(checkUserDetails);
                    _context.SaveChanges(true);
                }
                return Ok(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
//1472
