using Contracts;
using Hawi.Dtos;
using Hawi.Extensions;
using Hawi.Models;
using Hawi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Hawi.Controllers
{
    [Route("hawi/Authentiaction/[action]")]
    [ApiController]
    public class AuthentiactionController : ControllerBase
    {
        private readonly HawiContext _context;
        private readonly IConfiguration _configuration;
        private readonly ISMRepository _smsService;
        private readonly UserFunctions _userFunctions;
        private readonly ImageFunctions _ImageFunctions;
        private readonly Authentiaction _Authentiaction;
        private readonly AddRecordsInDB _AddRecordsInDB;
        string uidsa = "";
        string uideg = "";
        public AuthentiactionController(ISMRepository smsService, IConfiguration configuration, UserFunctions userFunctions, Authentiaction authentiaction, ImageFunctions imageFunctions, HawiContext Context, AddRecordsInDB addRecordsInDB)
        {
            _configuration = configuration;
            _smsService = smsService;
            _userFunctions = userFunctions;
            _Authentiaction = authentiaction;
            _ImageFunctions = imageFunctions;
            _context = Context;
            _AddRecordsInDB = addRecordsInDB;
        }


        [HttpPost(Name = "Register")]
        public async Task<IActionResult> RegisterAsync([FromForm] UserForRegistrationDto user)
        {
            using (var transication = _context.Database.BeginTransaction())
            {
                try
                {

                    #region validation for phone Number
                    //يرجى مراعاة أنه يجب عليك انتظار 60 يومًا بعد حذف حسابك لاستخدام نفس الرقم لإنشاء حساب جديد
                    if (user.IsParent == true)
                    {
                        var entereduser = _context.Users.Where(x => x.Mobile.Equals(user.Mobile)).FirstOrDefault();
                        if (entereduser != null) return BadRequest("!الرقم الذى ادخلتة موجود مسبقا");

                        if (entereduser != null)
                        {
                            if (entereduser.IsActive == false)
                            {
                                return BadRequest("!يرجى مراعاة أنه يجب عليك انتظار 60 يومًا بعد حذف حسابك لاستخدام نفس الرقم لإنشاء حساب جديد");
                            }
                        }
                        var countrycode = _context.CityCountries.Where(x => x.CityCountryId.Equals(user.CountryCodeId)).FirstOrDefault();
                        if (countrycode == null) return BadRequest("!كود الدولة الذى ادخلتة غير موجود");

                        var cityid = _context.Cities.Where(x => x.CityId.Equals(user.CityId)).FirstOrDefault();
                        if (cityid == null) return BadRequest("!كود المدينة الذى ادخلتة غير موجود");

                        var checkPhoneNumber = await _userFunctions.CheckPhoneNumber(user.Mobile, countrycode);
                        if (checkPhoneNumber != null) return BadRequest($"{checkPhoneNumber}");

                        if (user.Image != null)
                        {
                            var GetInvalidImageMessage = _ImageFunctions.GetInvalidImageMessage(user.Image);
                            if (GetInvalidImageMessage != null) return BadRequest(GetInvalidImageMessage);
                        }
                    }
                    #endregion

                    #region add user and userprofile

                    var addUserEntity = await _userFunctions.CreateUserEntity(user);
                    // Gnreat Mobil Nmber For Country 
                    if (addUserEntity != null && user.IsParent == false)
                    {
                        var countrie = _context.CityCountries.Where(x => x.CityCountryId == addUserEntity.CityCountryId).FirstOrDefault();
                        if (countrie != null)
                        {
                            if (countrie.CityCountryId == 14)
                            {
                                Random rand = new Random();
                                uidsa = "002" + rand.Next(1000000);//.ToString().Substring(1);
                                addUserEntity.Mobile = uidsa;
                                addUserEntity.ParentChild = user.Mobile;
                            }
                            else
                            {
                                Random rand = new Random();
                                uideg = "001" + rand.Next(1000000000).ToString().Substring(1);
                                addUserEntity.Mobile = uideg;
                                addUserEntity.ParentChild = user.Mobile;
                            }
                            var details = new UserDetails();
                            details.ParentMobil = user.Mobile;
                            details.ParentChild = addUserEntity.Mobile;
                            _context.userdetails.Add(details);
                            _context.SaveChanges();
                        }
                    }
                    var addUserProfile = await _userFunctions.CreateUserProfile(addUserEntity.UserId, 1);

                    var pathh = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\";
                    var folderName = user.Mobile;
                    await _ImageFunctions.AddFolderInServer(pathh, folderName);

                    #endregion

                    #region userimage

                    if (user.Image != null)
                    {
                        var NewFileName = _ImageFunctions.ChangeImageName(user.Image);
                        var path = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{user.Mobile}\" + NewFileName;
                        var imagepathinDB = $"http://mobile.hawisports.com/image/Users/{user.Mobile}/" + NewFileName;

                        var uploadImageToserver = await _ImageFunctions.UploadImageToServerAsync(user.Image, path);
                        if (uploadImageToserver != null) return BadRequest("!حدث خطا اثناء رفع الصورة");

                        var AddImageInDB = await _ImageFunctions.AddImageInDB(imagepathinDB, NewFileName, 1);
                        var AddUserProfileImage = await _ImageFunctions.AddUserProfileImage(addUserProfile.UserProfileId, AddImageInDB.ImageId, 1);
                    }

                    transication.Commit();
                    return Ok("تم انشاء الحساب بنجاح");

                    #endregion
                }
                catch (Exception ex)
                {
                    transication.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost(Name = "login")]
        public async Task<ActionResult<string>> login([FromBody] UserForLoginDto user)
        {
            using (var transiction = _context.Database.BeginTransaction())
            {
                try
                {
                    var SelectedUser = _context.Users.Where(x => x.Mobile.Equals(user.Mobile)).FirstOrDefault();
                    if (SelectedUser == null)
                        return BadRequest("!رقم الهاتف أو كلمة المرور التي أدخلتها غير صحيحة");

                    if (user.Password != SelectedUser.Password)
                        return BadRequest("!رقم الهاتف أو كلمة المرور التي أدخلتها غير صحيحة");

                    var lastloginrole = SelectedUser.LastLoginRoleId;

                    var token = await _Authentiaction.GenerateJwtToken(_configuration, SelectedUser.UserId,(byte)lastloginrole, _userFunctions, _context);
                    if (token == null) return BadRequest("!حدث خطا اثناء تسجيل الدخول");

                    //فى حالة انة كان مسح الاكونت هيتعملة  وفى حالة دخل علاكونت قبل 60 يوم "disactivate "هيتمسح 
                    if (SelectedUser.IsActive == false)
                    {
                        SelectedUser.IsActive = true;

                        var checkDisActivatedUser = _context.UsersUsersActivationArchieves.Where(x => x.UserId == SelectedUser.UserId).FirstOrDefault();
                        if (checkDisActivatedUser != null)
                            _context.UsersUsersActivationArchieves.Remove(checkDisActivatedUser);
                    }
                    transiction.Commit();
                    _context.SaveChanges();
                    return Ok(token);

                }
                catch (Exception ex)
                {
                    transiction.Rollback();
                    return BadRequest(ex);
                }
            }
        }

        // replase with real company to send msg 
        [HttpPost(Name = "ForgetPassword")]
        public IActionResult ForgetPassword(string Mobile)
        {
            try
            {
                var user = _context.Users.Where(x => x.Mobile.Equals(Mobile)).FirstOrDefault();
                if (user == null) return BadRequest("!لم يتم العثور على هذا المستخدم");

                var RandomCode = _smsService.CreateVerifyCode();
                var result = _smsService.Send("+201212058735", $"{RandomCode}");

                if (!string.IsNullOrEmpty(result.ErrorMessage)) return BadRequest(result.ErrorMessage);

                user.VerifyCode = RandomCode;
                user.LastSentVerifiedCodeTime = DateTime.Now.AddMinutes(3);

                _context.SaveChanges();

                return Ok(user.UserId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "VerifiedCode")]
        public IActionResult VerifiedCode(short VerifyCode, long UserId)
        {
            try
            {
                var user = _context.Users.Where(x => x.UserId.Equals(UserId)).FirstOrDefault();
                if (user == null) return BadRequest($"لم يتم العثور على هذا المستخدم {UserId}");

                if (user.LastSentVerifiedCodeTime < DateTime.Now)
                {
                    user.VerifyCode = null;
                    _context.SaveChanges();
                    return BadRequest("!الرمز الذي أدخلته منتهي الصلاحية، يرجى محاولة إرسال الرمز مرة أخرى");
                }

                if (user.VerifyCode != (VerifyCode)) return BadRequest("!الرمز الذي أدخلته غير صحيح، يرجى محاولة إرسال الرمز مرة أخرى");
                if (user.VerifyCode == (VerifyCode)) return Ok("نجحت العملية، لقد تم التحقق من هويتك");

                return BadRequest("!الرمز الذي أدخلته غير صحيح، يرجى محاولة إرسال الرمز مرة أخرى");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost(Name = "ResetPassword")]
        public IActionResult ResetPassword(long UserId, [FromBody] ResetPasswordDto pass)
        {
            try
            {
                var user = _context.Users.Where(x => x.UserId.Equals(UserId)).FirstOrDefault();
                if (user == null) return BadRequest($"لم يتم العثور على المستخدم {UserId}");
                else
                {
                    user.Password = pass.Password;

                    if (user.IsActive == false)
                    {
                        user.IsActive = true;
                        var checkDisActivatedUser = _context.UsersUsersActivationArchieves.Where(x => x.UserId == UserId).FirstOrDefault();
                        if (checkDisActivatedUser != null)
                            _context.UsersUsersActivationArchieves.Remove(checkDisActivatedUser);
                    }
                    _context.SaveChanges();
                    return Ok("تم تحديث كلمة المرور بنجاح");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(Name = "ChangePassword")]
        public IActionResult ChangePassword(long UserId, [FromBody] ChangePasswordDTO password)
        {
            try
            {
                var user = _context.Users.Where(x => x.UserId == UserId && x.IsActive == true).FirstOrDefault();
                if (user == null) return NotFound("!لم يتم العثور على هذا المستخدم");

                if (user.Password != password.CurrentPassword)
                    return Conflict("!كلمة المرور القديمة غير صحيحة");

                user.Password = password.NewPassword;
                _context.SaveChanges();

                return Ok("تم تغيير الرقم السرى بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "CrateCountryCode")]
        public async Task<IActionResult> CrateCountryCodeAsync([FromForm] CreateCountryCodeDto CountryDto)
        {
            using (var transication = _context.Database.BeginTransaction())
            {
                try
                {
                    var country = _context.CityCountries.Where(x => x.Code.Equals(CountryDto.Code)).FirstOrDefault();

                    if (country != null)
                        return BadRequest("!البلد الذي ادخلته موجود بالفعل");
                    else
                    {
                        string englishFileName = _ImageFunctions.ChangeImageName(CountryDto.Image);
                        var directory = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Country\";
                        var path = directory + englishFileName;

                        await _ImageFunctions.CheckDirectoryExist(directory);
                        var uploadImageToserver = await _ImageFunctions.UploadImageToServerAsync(CountryDto.Image, path);
                        if (uploadImageToserver != null) return BadRequest("!حدث خطا اثناء رفع الصورة");

                        await _AddRecordsInDB.CreateCountryEntity(CountryDto, englishFileName);
                        transication.Commit();
                        return Ok("تم الاضافة بنجاح");
                    }
                }
                catch (Exception ex)
                {
                    transication.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet(Name = "GetAllContry")]
        public IActionResult GetAllContry()
        {
            try
            {
                var countries = _context.CityCountries
                   .Select(c =>
                       new GetCountryCodeDto
                       {
                           CityCountryId = c.CityCountryId,
                           CountryArabicName = c.CountryArabicName,
                           CountryEnglishName = c.CountryEnglishName,
                           ImageUrl = c.ImageUrl,
                           Length = c.Length,
                           Code = c.Code,
                           RegionCode = c.RegionCode
                       })
                 .ToList();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet(Name = "GetCitiesByCountryId")]
        public IActionResult GetCitiesByCountryId(byte countryId = 0, string? startsWith = null)
        {
            try
            {
                if (countryId == 0)
                {
                    var cities = _context.Cities
                    .Where(c => (startsWith == null
                       || c.CityArabicName.StartsWith(startsWith)))
                    .Select(c =>
                    new
                    {
                        c.CityId,
                        c.CityArabicName
                    })
                    .ToList();
                    return Ok(cities);
                }
                var country = _context.CityCountries.Where(x => x.CityCountryId == countryId).FirstOrDefault();
                if (country == null) return NotFound("لم يتم العثور على هذة الدولة ");

                var citiesCountry = _context.Cities
                    .Where(c => c.CityCountryId == countryId && (startsWith == null || c.CityArabicName.StartsWith(startsWith)))
                    .Select(c => new
                    {
                        c.CityId,
                        c.CityArabicName
                    })
                    .ToList();

                return Ok(citiesCountry);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetAllRole")]
        public IActionResult GetAllRole()
        {
            try
            {
                var roles = _context.Roles
                .Select(x => new
                {
                    x.RoleId,
                    x.Role1
                }).ToList();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(Name = "GetBYMobile")]
        public IActionResult GetBYMobile(string Mobile)
        {
            try
            {
                var users = _context.Users.Where(x => x.ParentChild == Mobile)
                     .Select(x => new
                     {
                         x.UserId,
                         x.Name,
                         x.Mobile,
                         x.Password,
                     })
                     .ToList();
                return Ok(users);
                //var tables = new ViewUser
                //{
                //    users = _context.Users.Where(x => x.ParentChild == Mobile).ToList(),
                //    //userdetails = _context.userdetails.Where(x => x.ParentMobil == Mobile).ToList(),
                //};
                //return Ok(tables);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
