using Contracts;
using Hawi.Dtos;
using Hawi.Extensions;
using Hawi.Models;
using Hawi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hawi.Controllers
{
    [Route("hawi/SportInstitutions/[action]")]
    [ApiController]
    public class SportInstitutionsController : ControllerBase
    {
        private readonly HawiContext _context;
        private readonly ImageFunctions _ImageFunctions;
        private readonly UserFunctions _UserFunctions;
        private readonly Authentiaction _authentiaction;
        private readonly IConfiguration _configuration;
        private readonly ISMRepository _smsService;
        public SportInstitutionsController(HawiContext context, ImageFunctions ImageFunctions, ISMRepository smsService, UserFunctions userFunctions, Authentiaction authentiaction, IConfiguration configuration)
        {
            _smsService = smsService;
            _UserFunctions = userFunctions;
            _authentiaction = authentiaction;
            _configuration = configuration;
            _context = context;
            _ImageFunctions = ImageFunctions;
        }

        //not start refactor yet 
        [HttpGet(Name = "GetSportInstitutionsType")]
        public IActionResult GetSportInstitutionsType()
        {
            var SportInstitutionsType = _context.SportInstitutionSportInstitutionTypes
                .Select(c => new ShowSportInstitutionsTypeDto
                {
                    SportInstitutionTypeId = c.SportInstitutionTypeId,
                    SportInstitutionType = c.SportInstitutionType,
                    Imagepath = c.ImagePath,

                })
                .ToList();
            if (SportInstitutionsType == null || !SportInstitutionsType.Any()) return Ok();
            return Ok(SportInstitutionsType);
        }

        [HttpGet(Name = "GETSportInstitutionBranchType")]
        public IActionResult GETSportInstitutionBranchType()
        {
            var SportInstitutionBranchTypes = _context.SportInstitutionBranchTypeSportInstitutionBranchTypes
                .Select(c => new SportInstitutionBranchTypeDto
                {
                    SportInstitutionBranchTypeId = c.SportInstitutionBranchTypeId,
                    SportInstitutionBranchType = c.SportInstitutionBranchType
                })
                .ToList();
            if (SportInstitutionBranchTypes == null || !SportInstitutionBranchTypes.Any()) return Ok();
            return Ok(SportInstitutionBranchTypes);
        }

        [HttpGet(Name = "GETSportInstitutionSubscriptionPeriod")]
        public IActionResult GETSportInstitutionSubscriptionPeriod()
        {
            var SportInstitutionSubscriptionPeriods = _context.SportInstitutionAgePriceSubscriptionPeriods
                .Select(c => new SportInstitutionAgePriceSubscriptionPeriodDTO
                {
                    SubscriptionPeriodId = c.SubscriptionPeriodId,
                    SubscriptionPeriod = c.SubscriptionPeriod,
                })
                .ToList();
            if (SportInstitutionSubscriptionPeriods == null || !SportInstitutionSubscriptionPeriods.Any()) return Ok();
            return Ok(SportInstitutionSubscriptionPeriods);
        }

        [HttpGet(Name = "GETSportInstitutionAgePriceAgeCategory")]
        public IActionResult GETSportInstitutionAgePriceAgeCategory(byte SportInstitutionTypeId, long BranchId = 0)
        {
            if (SportInstitutionTypeId == 2 || SportInstitutionTypeId == 4)
                SportInstitutionTypeId = 1;

            if (BranchId != 0)
            {
                var SelectedSportInstitution = _context.SportInstitutionBranches
                    .Where(x => x.SportInstitutionBranchId.Equals(BranchId))
                    .FirstOrDefault();

                if (SelectedSportInstitution == null) return BadRequest("! الفرع الذى ادخلته غير موجود");

                var ageCategoriesExistInThatpRANCH = _context.SportInstitutionAgePrices
                     .Where(belong => belong.SportInstitutionBranchId == BranchId)
                     .Select(item => item.AgeCategoryId)
                     .ToList();

                var SportInstitutionAgePriceAgeCategories = _context.SportInstitutionAgePriceAgeCategories
                    .Where(x => x.SportInstitutionTypeId == SportInstitutionTypeId)
                    .Where(c => !ageCategoriesExistInThatpRANCH.Contains(c.AgeCategoryId))
                    .Select(c => new GETSportInstitutionAgePriceAgeCategoryDto
                    {
                        AgeCategoryId = c.AgeCategoryId,
                        AgeCategory = c.AgeCategory,
                    })
                    .ToList();
                return Ok(SportInstitutionAgePriceAgeCategories);
            }
            var SportInstitutionAgePriceAgeCategories1 = _context.SportInstitutionAgePriceAgeCategories.Where(x => x.SportInstitutionTypeId == SportInstitutionTypeId)
            .Select(x => new
            {
                AgeCategoryId = x.AgeCategoryId,
                AgeCategory = x.AgeCategory,
            }).ToList();

            return Ok(SportInstitutionAgePriceAgeCategories1);
        }

        [HttpGet(Name = "GetSportInstitutions")]
        public IActionResult GetSportInstitutions()
        {
            var SportInstitutions = _context.SportInstitutions
                .Select(x => new
                {
                    x.SportInstitutionId,
                    x.SportInstitutionName,
                    x.LogoUrlfullPath,
                    Description = _context.UserProfiles.FirstOrDefault(n => n.UserProfileId == x.UserProfileId).Description ?? null,
                    x.Gmail
                }).ToList();
            return Ok(SportInstitutions);
        }

        [HttpGet(Name = "GetSportInstitutionData")]
        public IActionResult GetSportInstitutionData(long userProfileId)
        {
            var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == userProfileId).FirstOrDefault();
            if (userprofile == null) return NotFound("!لم يتم العثور على هذا المستخدم");

            SportInstitution? sportInstitution = null;

            sportInstitution = _context.SportInstitutions
                 .Include(si => si.SportInstitutionBranches)
                 .FirstOrDefault(si => si.UserProfileId == userProfileId);

            if (sportInstitution == null) return NotFound("!هذا المستخدم ليس لدية اى منظمة رياضية بعد ");

            //if there is no branches
            var branch = _context.SportInstitutionBranches.Where(x => x.SportInstitutionId == sportInstitution.SportInstitutionId).FirstOrDefault();
            if (branch == null)
            {
                var SportInstitutions = _context.SportInstitutions
                .Where(si => si.UserProfileId == userProfileId)
                .Select(x => new
                {
                    x.SportInstitutionId,
                    image = sportInstitution.LogoUrlfullPath,
                    sportInstitutionDateCreated = x.DateCreated,
                    x.FounderName,
                    x.SportInstitutionName,
                    x.LicenseNumber,
                    Description = userprofile.Description,
                    SportInstitutiontype = _context.SportInstitutionSportInstitutionTypes.FirstOrDefault(x => x.SportInstitutionTypeId == x.SportInstitutionTypeId).SportInstitutionType,
                    SportInstitutionTypeId = x.SportInstitutionTypeId,
                    isbranch = 0,
                    Gmail = x.Gmail
                })
               .FirstOrDefault();
                return Ok(SportInstitutions);
            }

            var branches = sportInstitution.SportInstitutionBranches
                .Where(n => n.SportInstitutionBranchTypeId == 1)
                .Select(z => new
                {
                    z.SportInstitutionId,
                    image = sportInstitution.LogoUrlfullPath,
                    sportInstitutionDateCreated = z.SportInstitution.DateCreated,
                    z.SportInstitution.FounderName,
                    z.SportInstitution.SportInstitutionName,
                    z.SportInstitution.LicenseNumber,
                    Description = userprofile.Description,
                    Gmail = sportInstitution.Gmail,
                    SportInstitutiontype = _context.SportInstitutionSportInstitutionTypes.FirstOrDefault(x => x.SportInstitutionTypeId == z.SportInstitutionBranchTypeId).SportInstitutionType,
                    SportInstitutiontypeId = z.SportInstitution.SportInstitutionTypeId,
                    z.SportInstitutionBranchId,
                    SportInstitutionBranchType = _context.SportInstitutionBranchTypeSportInstitutionBranchTypes.FirstOrDefault(x => x.SportInstitutionBranchTypeId == z.SportInstitutionBranchTypeId).SportInstitutionBranchType,
                    SportInstitutionBranchTypeId = z.SportInstitutionBranchTypeId,
                    sportInstitutionbranchCreated = z.CreateDate,
                    z.Location,
                    z.CityDistricts,
                    countryId = _context.Cities.FirstOrDefault(x => x.CityId == branch.CityId).CityCountryId,
                    country = _context.CityCountries.FirstOrDefault(x => x.CityCountryId == (_context.Cities.FirstOrDefault(x => x.CityId == branch.CityId).CityCountryId)).CountryArabicName,
                    z.CityId,
                    city = _context.Cities.FirstOrDefault(x => x.CityId == z.CityId).CityArabicName,
                    z.BranchPhone,
                    isbranch = 1,
                    AgeCategory = _context.SportInstitutionAgePrices
                             .Where(belong => belong.SportInstitutionBranch.SportInstitutionId == z.SportInstitutionId)
                             .Select(x => new
                             {
                                 x.AgeCategory.AgeCategory,
                                 x.AgeCategory.AgeCategoryId
                             })
                             .Distinct()
                             .ToList(),
                    MaxAgePrice = _context.SportInstitutionAgePrices
                    .Where(a => a.SportInstitutionBranch.SportInstitutionId == z.SportInstitutionId)
                    .Select(agePrice => agePrice.Price)
                    .Max(),
                    MinAgePrice = _context.SportInstitutionAgePrices
                    .Where(a => a.SportInstitutionBranch.SportInstitutionId == z.SportInstitutionId)
                    .Select(agePrice => agePrice.Price)
                    .Min(),

                })
                .FirstOrDefault();

            return Ok(branches);
        }

        [HttpGet(Name = "GetBranchsOfSportInstitution")]
        public IActionResult GetBranchOfSportInstitution(long? userprofileId = null, long? sportInstitutionId = null, long IfLongMainBranch = 0)
        {
            // can get branch by userprofile or with sportInstitutionId
            //IfLongMainBranch if want all branch with main send 1
            long SportInstitutionID = 0;
            SportInstitution SelectedSportInstitution;
            if (userprofileId != null)
            {
                SelectedSportInstitution = _context.SportInstitutions.Where(x => x.UserProfileId == userprofileId).FirstOrDefault();
                if (SelectedSportInstitution == null)
                    return BadRequest(" المؤسسة الرياضية الذى ادخلتها غير موجوده!");
                SportInstitutionID = SelectedSportInstitution.SportInstitutionId;
            }
            else
            {
                SelectedSportInstitution = _context.SportInstitutions.Where(x => x.SportInstitutionId.Equals(sportInstitutionId)).FirstOrDefault();
                if (SelectedSportInstitution == null)
                    return BadRequest(" المؤسسة الرياضية الذى ادخلتها غير موجوده!");
                SportInstitutionID = SelectedSportInstitution.SportInstitutionId;
            }

            if (IfLongMainBranch != 0)
            {
                var branchesWithMain = _context.SportInstitutionBranches
                .Where(branch => branch.SportInstitutionId == SportInstitutionID)
                .Select(branch => new
                {
                    branch.SportInstitutionBranchId,
                    SportInstitutionBranchType = _context.SportInstitutionBranchTypeSportInstitutionBranchTypes
                        .FirstOrDefault(x => x.SportInstitutionBranchTypeId == branch.SportInstitutionBranchTypeId)
                        .SportInstitutionBranchType ?? null,
                    sportInstitutionbranchCreated = branch.DateCreated,
                    branch.CityDistricts,
                    countryId = _context.Cities.FirstOrDefault(x => x.CityId == branch.CityId).CityCountryId,
                    country = _context.CityCountries.FirstOrDefault(x => x.CityCountryId == (_context.Cities.FirstOrDefault(x => x.CityId == branch.CityId).CityCountryId)).CountryArabicName,
                    cityid = branch.CityId,
                    city = _context.Cities.FirstOrDefault(x => x.CityId == branch.CityId).CityArabicName,
                    branch.Location,
                    BranchPhone = branch.BranchPhone ?? null,
                    SportInstitutionBranchName = branch.SportInstitutionBranchName ?? null,
                })
                .ToList();
                return Ok(branchesWithMain);
            }

            var branches = _context.SportInstitutionBranches
                .Where(branch => branch.SportInstitutionId == SportInstitutionID && branch.SportInstitutionBranchTypeId == 2)
                .Select(branch => new
                {
                    branch.SportInstitutionBranchId,
                    //SportInstitutionBranchType = _context.SportInstitutionBranchTypeSportInstitutionBranchTypes
                    //    .FirstOrDefault(x => x.SportInstitutionBranchTypeId == branch.SportInstitutionBranchTypeId) != null
                    //        ? _context.SportInstitutionBranchTypeSportInstitutionBranchTypes
                    //            .FirstOrDefault(x => x.SportInstitutionBranchTypeId == branch.SportInstitutionBranchTypeId).SportInstitutionBranchType
                    //        : null,
                    SportInstitutionBranchType = _context.SportInstitutionBranchTypeSportInstitutionBranchTypes
                                    .FirstOrDefault(x => x.SportInstitutionBranchTypeId == branch.SportInstitutionBranchTypeId)
                                    .SportInstitutionBranchType ?? null,
                    sportInstitutionbranchCreated = branch.DateCreated,
                    branch.CityDistricts,
                    countryId = _context.Cities.FirstOrDefault(x => x.CityId == branch.CityId).CityCountryId,
                    country = _context.CityCountries.FirstOrDefault(x => x.CityCountryId == (_context.Cities.FirstOrDefault(x => x.CityId == branch.CityId).CityCountryId)).CountryArabicName,
                    cityid = branch.CityId,
                    city = _context.Cities.FirstOrDefault(x => x.CityId == branch.CityId).CityArabicName,
                    branch.Location,
                    BranchPhone = branch.BranchPhone ?? null,
                    SportInstitutionBranchName = branch.SportInstitutionBranchName ?? null
                })
                .ToList();

            return Ok(branches);
        }

        [HttpGet(Name = "GetSportInstituationAgeCategory")]
        public IActionResult GetSportInstituationAgeCategory(long SportInstituationID)
        {
            var SelectedSportInstitution = _context.SportInstitutions.Where(x => x.SportInstitutionId.Equals(SportInstituationID)).FirstOrDefault();
            if (SelectedSportInstitution == null) return BadRequest(" المؤسسة الرياضية الذى ادخلتها غير موجوده!");

            var AgeCategory = _context.SportInstitutionAgePrices
                               .Where(belong => belong.SportInstitutionBranch.SportInstitutionId == SportInstituationID)
                               .Select(x => new
                               {
                                   x.AgeCategoryId,
                                   x.AgeCategory.AgeCategory,
                               })
                               .Distinct()
                               .ToList();
            return Ok(AgeCategory);

        }

        [HttpGet(Name = "GetBranchAgeCategory")]
        public IActionResult GetBranchAgeCategory(long BranchId)
        {
            var SelectedSportInstitution = _context.SportInstitutionBranches.Where(x => x.SportInstitutionBranchId.Equals(BranchId)).FirstOrDefault();
            if (SelectedSportInstitution == null) return BadRequest(" المؤسسة الرياضية الذى ادخلتها غير موجوده!");

            var ageCategories = _context.SportInstitutionAgePrices
             .Where(belong => belong.SportInstitutionBranchId == BranchId)
             .Select(x => new
             {
                 x.SportInstitutionAgePriceId,
                 x.SportInstitutionBranchId,
                 x.AgeCategory.AgeCategory,
                 x.AgeCategory.AgeCategoryId,
                 x.Price,
             })
             .ToList().Distinct();
            return Ok(ageCategories);

        }

        [HttpGet(Name = "GetPlayerCoahAdministrativesPendingOfSportInstitutionBranch")]
        public IActionResult GetPlayerCoahAdministrativesPendingOfSportInstitutionBranch(long SportInstitutionBranchId, byte? BelongTypeId = 0, byte? isBelongPending = 0)
        {
            var existingSportInstitutionBranch = _context.SportInstitutionBranches.Where(x => x.SportInstitutionBranchId == SportInstitutionBranchId).FirstOrDefault();
            if (existingSportInstitutionBranch == null) return NotFound("لم يتم العثور على الفرع الذى ادخلتة!");

            var BelongType = _context.SportInstitutionBelongBelongTypes.Where(x => x.BelongTypeId == BelongTypeId).FirstOrDefault();
            if (BelongType == null) return NotFound("ادخل نوع عمل الشخص الذى تبحث عنة بصورة صحيحة!");

            // get pending with filter 1
            if (isBelongPending != 0 && isBelongPending != null)
            {
                var result = _context.SportInstitutionBelongPendings.Where(x => x.SportInstitutionBranchId == SportInstitutionBranchId && x.BelongTypeId == BelongTypeId)
               .Select(x => new
               {
                   x.SportInstitutionBranchId,
                   x.BelongTypeId,
                   BelongType = _context.SportInstitutionBelongBelongTypes.FirstOrDefault(n => n.BelongTypeId == x.BelongTypeId).BelongType,
                   x.BelongUserProfileId,
                   name = _context.UserProfiles.Where(n => n.UserProfileId == x.BelongUserProfileId).Select(z => z.User.Name).FirstOrDefault() ?? null,
                   image = _UserFunctions.GetUserImage(x.BelongUserProfileId, x.BelongUserProfile.RoleId),
                   x.ShortDescription,
                   x.CreateDate

               })
                 .ToList();

                return Ok(result);
            }
            // get belong with filter
            var Belongresult = _context.SportInstitutionBelongs.Where(x => x.SportInstitutionBranchId == SportInstitutionBranchId
                    && x.BelongTypeId == BelongTypeId).Select(x => new
                    {
                        x.SportInstitutionBelongId,
                        x.SportInstitutionBranchId,
                        x.BelongTypeId,
                        BelongType = _context.SportInstitutionBelongBelongTypes.FirstOrDefault(n => n.BelongTypeId == x.BelongTypeId).BelongType,
                        x.BelongUserProfileId,
                        name = _context.UserProfiles.Where(n => n.UserProfileId == x.BelongUserProfileId).Select(z => z.User.Name).FirstOrDefault() ?? null,
                        image = _UserFunctions.GetUserImage(x.BelongUserProfileId, x.BelongUserProfile.RoleId),
                        x.ShortDescription,
                        x.CreateDate
                    })
                .ToList();


            return Ok(Belongresult);

        }

        [HttpGet(Name = "GetSportInstitutionSponser")]
        public IActionResult GetSportInstitutionSponser(long userProfileId)
        {
            var getSelectedSponser = _context.Sponsors.Where(x => x.UserProfileId == userProfileId)
                                           .Select(x => new
                                           {
                                               x.SponsorId,
                                               x.UserProfileId,
                                               x.SponsorUserProfileId,
                                               Logo = _context.Images.FirstOrDefault(n => n.ImageId == x.SponsorLogoId).ImageUrlfullPath ?? null,
                                               x.SponsorName,
                                               x.CreateDate
                                           }).ToList();
            return Ok(getSelectedSponser);
        }

        [HttpGet(Name = "GetSportInstitutionsByID")]
        public async Task<IActionResult> GetSportInstitutionsByID(long EntireUserProfileId)
        {
            var selectResult = await _context.SportInstitutions
               .Where(x => x.UserProfileId == EntireUserProfileId)
               .Select(x => new
               {
                   x.SportInstitutionName,
                   x.SportInstitutionId,
               }).FirstOrDefaultAsync();
            return Ok(selectResult);
        }

        [HttpGet(Name = "GetSportInstitutionsByTypeID")]
        public async Task<IActionResult> GetSportInstitutionsByTypeID(byte SportInstitutionTypeId)
        {
            var selectResult = await _context.SportInstitutions
               .Where(x => x.SportInstitutionTypeId == SportInstitutionTypeId)
               .Select(x => new
               {
                   x.SportInstitutionName,
                   x.SportInstitutionId,
               })
               .ToListAsync();

            return Ok(selectResult);
        }

        [HttpGet(Name = "GetSportInstitutionTechnicalTeams")]
        public async Task<IActionResult> GetSportInstitutionTechnicalTeams(long SportInstituationID)
        {//Coach And Adminstration 

            var SelectedResult = await _context.SportInstitutionBelongs
                .Where(x => x.SportInstitutionBranch.SportInstitutionId == SportInstituationID
                        && x.BelongTypeId != 1)
                .Select(x => new
                {
                    UserProfileId = x.BelongUserProfileId,
                    Name = _UserFunctions.GetUserName(x.BelongUserProfile.RoleId, x.BelongUserProfileId, x.BelongUserProfile.UserId),
                    BelongType = _context.SportInstitutionBelongBelongTypes.FirstOrDefault(n => n.BelongTypeId == x.BelongTypeId).BelongType,
                })
                .ToListAsync();
            return Ok(SelectedResult);
        }

        //
        [HttpPost(Name = "CreateSportInstitutions")]
        public async Task<IActionResult> CreateSportInstitutionsAsync(long Userid, byte SportInstitutionTypeId, [FromForm] CreatesportInstitutionsDto sportInstitution)
        {
            using (var transication = _context.Database.BeginTransaction())
            {
                try
                {

                    #region validation
                    var enteredUser = _context.Users.Where(x => x.UserId.Equals(Userid)).FirstOrDefault();
                    if (enteredUser == null) return BadRequest("لم يتم العثور على هذا المستخدم!");

                    //

                    var SelectedSportInstitution = _context.SportInstitutionSportInstitutionTypes.Where(x => x.SportInstitutionTypeId.Equals(SportInstitutionTypeId)).FirstOrDefault();
                    if (SelectedSportInstitution == null) return BadRequest("نوع مؤسسة رياضية الذى ادخلتها غير موجود!");

                    byte role = 5;
                    if (SportInstitutionTypeId == 1)
                    {
                        role = 5;
                        var checkuserProfileacademyrole = _context.UserProfiles.Where(u => u.UserId == Userid && u.RoleId == 5).FirstOrDefault();
                        if (checkuserProfileacademyrole != null) return BadRequest("لا يمكن لنفس المستخدم ادرارة اكثر من اكاديمية !");
                    }
                    else if (SportInstitutionTypeId == 2)
                    {
                        role = 7;
                        var checkuserProfileacademyrole = _context.UserProfiles.Where(u => u.UserId == Userid && u.RoleId == 7).FirstOrDefault();
                        if (checkuserProfileacademyrole != null) return BadRequest("لا يمكن لنفس المستخدم ادرارة اكثر من نادى هواه !");
                    }
                    else if (SportInstitutionTypeId == 3)
                    {
                        role = 8;
                        var checkuserProfileacademyrole = _context.UserProfiles.Where(u => u.UserId == Userid && u.RoleId == 8).FirstOrDefault();
                        if (checkuserProfileacademyrole != null) return BadRequest("لا يمكن لنفس المستخدم ادرارة اكثر من منشاه تعليمية !");
                    }
                    else if (SportInstitutionTypeId == 4)
                    {
                        role = 11;

                        var checkuserProfileacademyrole = _context.UserProfiles.Where(u => u.UserId == Userid && u.RoleId == 11).FirstOrDefault();
                        if (checkuserProfileacademyrole != null) return BadRequest("لا يمكن لنفس المستخدم ادرارة اكثر من مجموعة تمارين !");
                    }
                    if (!new[] { ".jpg", ".jpeg", ".png" }.Contains(Path.GetExtension(sportInstitution.Logo.FileName).ToLower()))
                        return BadRequest($"يتم قبول صيغ الصور التالية فقط: JPG، JPEG، PNG, {sportInstitution.Logo.FileName}غير معوم!");

                    #endregion

                    #region upload logo 

                    var pathh = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\SportInstitution\{enteredUser.Mobile}\";
                    await _ImageFunctions.CheckDirectoryExist(pathh);

                    var LogoFileName = _ImageFunctions.ChangeImageName(sportInstitution.Logo);
                    var Logopath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\SportInstitution\{enteredUser.Mobile}\" + LogoFileName;

                    using (var stream = new FileStream(Logopath, FileMode.Create))
                    {
                        await sportInstitution.Logo.CopyToAsync(stream);
                    }

                    var Logophysicalpath = $"http://mobile.hawisports.com/image/SportInstitution/{enteredUser.Mobile}/" + LogoFileName;

                    #endregion

                    #region add new role to user 
                    var userprofile = new UserProfile
                    {
                        UserId = enteredUser.UserId,
                        RoleId = role,
                        Description = sportInstitution.Description,
                        IsActive = true,
                        LastUpdate = DateTime.Now,
                    };
                    _context.UserProfiles.Add(userprofile);
                    //enteredUser.LastLoginRoleId = role;
                    await _context.SaveChangesAsync();

                    #endregion

                    #region add sportInstitution
                    var NewsportInstitution = new SportInstitution
                    {
                        UserProfileId = userprofile.UserProfileId,
                        SportInstitutionTypeId = SportInstitutionTypeId,
                        SportInstitutionName = sportInstitution.SportInstitutionName,
                        FounderName = sportInstitution.FounderName,
                        DateCreated = sportInstitution.DateCreated,
                        LogoFileName = LogoFileName,
                        LogoUrlfullPath = Logophysicalpath,
                        Gmail = sportInstitution.Gmail,
                        LastUpdate = DateTime.Now,
                        IsActive = true,
                    };
                    await _context.SportInstitutions.AddAsync(NewsportInstitution);
                    await _context.SaveChangesAsync();

                    transication.Commit();
                    //return created role to create new token
                    return Ok(role);
                    #endregion
                }
                catch (Exception ex)
                {
                    transication.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost(Name = "CreateSportInstitutionBranch")]
        public IActionResult CreateSportInstitutionBranch([FromBody] SportInstitutionBranchDTO branch)
        {

            #region validation
            var SelectedSportInstitution = _context.SportInstitutions.Where(x => x.SportInstitutionId.Equals(branch.SportInstitutionId)).FirstOrDefault();
            if (SelectedSportInstitution == null)
                return BadRequest(" المؤسسة الرياضية الذى ادخلتها غير موجود!");

            var SelectedSportInstitutionBranch = _context.SportInstitutionBranchTypeSportInstitutionBranchTypes.Where(x => x.SportInstitutionBranchTypeId.Equals(branch.SportInstitutionBranchTypeId)).FirstOrDefault();
            if (SelectedSportInstitution == null)
                return BadRequest($"نوع مؤسسة رياضية الذى ادخلتها غير موجود{branch.SportInstitutionBranchTypeId}!");

            //check if he add branch and there is no main branch
            var checkExistingBranch = _context.SportInstitutionBranches.Where(x => x.SportInstitutionId == branch.SportInstitutionId && x.SportInstitutionBranchTypeId == 1).FirstOrDefault();
            if (checkExistingBranch == null && branch.SportInstitutionBranchTypeId == 2)
                return BadRequest("يجب عليك اضافة المركز الرئيسى اولا ! ");

            if (checkExistingBranch != null && branch.SportInstitutionBranchTypeId == 1)
                return BadRequest("لا يمكن انشاء اكثر من فرع رئيسى لنفس المنظمة !");

            var citydistict = _context.Cities.Where(x => x.CityId == branch.CityId).FirstOrDefault();
            if (citydistict == null)
                return BadRequest("المدينة الذى اضفتها غير موجود!");

            #endregion

            #region add sportInstitution
            var newbranch = new SportInstitutionBranch
            {
                SportInstitutionId = branch.SportInstitutionId,
                SportInstitutionBranchTypeId = branch.SportInstitutionBranchTypeId,

                CityId = branch.CityId,
                CityDistricts = branch.CityDistricts,
                Location = branch.Location,
                SportInstitutionBranchName = branch.SportInstitutionBranchName,
                IsActive = true,
                LastUpdate = DateTime.Now,
            };
            if (branch.BranchPhone != null)
                newbranch.BranchPhone = branch.BranchPhone;

            if (branch.DateCreated != null)
                newbranch.DateCreated = branch.DateCreated;

            if (branch.BranchPhone == null && branch.SportInstitutionBranchTypeId == 2)
            {
                var selectedmainbranch = _context.SportInstitutionBranches.Where(x => x.SportInstitutionId == branch.SportInstitutionId
                                                                     && x.SportInstitutionBranchTypeId == 1).FirstOrDefault();
                if (selectedmainbranch != null)
                {
                    newbranch.BranchPhone = selectedmainbranch.BranchPhone;
                }
            }

            _context.SportInstitutionBranches.Add(newbranch);
            _context.SaveChanges();
            #endregion

            return Ok(newbranch.SportInstitutionBranchId);
        }

        [HttpPost(Name = "CreateAgeCategoriesForBranch")]
        public IActionResult CreateAgeCategoriesForBranch([FromBody] SportInstitutionAgecategoryDto agePriceDto)
        {
            try
            {
                #region validation 

                var CheckExistingBranch = _context.SportInstitutionBranches.Where(x => x.SportInstitutionBranchId == agePriceDto.SportInstitutionBranchId).FirstOrDefault();
                if (CheckExistingBranch == null) return BadRequest($" الفرع غير موجود !{CheckExistingBranch.SportInstitutionBranchId}");

                var checkUser = _context.SportInstitutions.Where(x => x.UserProfileId == agePriceDto.UserProfileId &&
                                                        x.SportInstitutionId == CheckExistingBranch.SportInstitutionId).FirstOrDefault();
                if (checkUser == null) return BadRequest("!ليس لديك الصلاحية للقيام بهذا");

                foreach (var checkage in agePriceDto.AgeCategoryId)
                {
                    var checkagecategory = _context.SportInstitutionAgePrices.Where(x => x.SportInstitutionBranchId == agePriceDto.SportInstitutionBranchId && x.AgeCategoryId == checkage).FirstOrDefault();
                    if (checkagecategory != null) return BadRequest("!بعض الاعمار السنية الذى ادخلتها موجودة من قبل ");

                    var AgeCategory = _context.SportInstitutionAgePriceAgeCategories.Where(x => x.AgeCategoryId == checkage).FirstOrDefault();
                    if (AgeCategory == null) return BadRequest("!بعض الاعمار السنية الذى ادخلتها غير موجودة ");
                }
                var SportInstitutionType = _context.SportInstitutionSportInstitutionTypes.Where(x => x.SportInstitutionTypeId == agePriceDto.SportInstitutionTypeID).FirstOrDefault();
                if (SportInstitutionType == null) return NotFound("!يجب ادخال نوع صحيح للمنظمة");

                #endregion

                #region add SportInstitutionAgePrice

                var newSportInstitutionAgePrices = new List<SportInstitutionAgePrice>();
                for (int i = 0; i < agePriceDto.AgeCategoryId?.Count; i++)
                {
                    var newSportInstitutionAgePrice = new SportInstitutionAgePrice
                    {
                        SportInstitutionTypeId = agePriceDto.SportInstitutionTypeID,
                        SportInstitutionBranchId = agePriceDto.SportInstitutionBranchId,
                        AgeCategoryId = (byte)agePriceDto.AgeCategoryId[i],
                        SubscriptionPeriodId = 3,
                        IsActive = true,
                        LastUpdate = DateTime.Now,
                    };
                    newSportInstitutionAgePrices.Add(newSportInstitutionAgePrice);
                }

                _context.SportInstitutionAgePrices.AddRange(newSportInstitutionAgePrices);
                _context.SaveChanges();
                return Ok("تمت الاضافة بنجاح");

                #endregion
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "CreateSportInstitutionSponser")]
        public async Task<IActionResult> CreateSportInstitutionSponser([FromForm] CreateSportInstitutionSponserDto SponserDto)
        {
            using (var transiction = _context.Database.BeginTransaction())
            {
                try
                {
                    var Userprofile = _context.UserProfiles.Where(x => x.UserProfileId.Equals(SponserDto.UserProfileId)).FirstOrDefault();
                    if (Userprofile == null) return NotFound("!لم يتم العثور على المستخدم");

                    var enteredUser = _context.Users.Where(x => x.UserId.Equals(Userprofile.UserId)).FirstOrDefault();
                    if (_UserFunctions.IsSportsorganization(SponserDto.UserProfileId) == false) return BadRequest("!ليس لديك الصلاحية لاضافة راعى ");

                    var checkIfSponserEXIST = _context.Sponsors.Where(x => x.UserProfileId == SponserDto.UserProfileId && x.SponsorName == SponserDto.SponsorName).FirstOrDefault();
                    if (checkIfSponserEXIST != null) return Conflict("تم اضافة هذا الراعى من قبل");

                    var validateImage = _ImageFunctions.GetInvalidImageMessage(SponserDto.SponsorLogo);
                    if (validateImage != null)
                        return BadRequest(validateImage);

                    var generateNewName = _ImageFunctions.ChangeImageName(SponserDto.SponsorLogo);

                    var directory = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\SportInstitution\{enteredUser.Mobile}\";
                    await _ImageFunctions.CheckDirectoryExist(directory);
                    var pathInServer = directory + generateNewName;
                    var PathForImageInDB = $"http://mobile.hawisports.com/image/SportInstitution/{enteredUser.Mobile}/" + generateNewName;

                    var uploadImageToserver = await _ImageFunctions.UploadImageToServerAsync(SponserDto.SponsorLogo, pathInServer);
                    if (uploadImageToserver != null) return BadRequest(uploadImageToserver);

                    var SaveImageInDB = await _ImageFunctions.SaveImageInDBAsync(PathForImageInDB, generateNewName, 14, true);
                    if (!long.TryParse(SaveImageInDB, out long imageId))
                    {
                        return BadRequest(SaveImageInDB);
                    }
                    long imageid = long.Parse(SaveImageInDB);

                    var newSponsor = new Sponsor
                    {
                        UserProfileId = SponserDto.UserProfileId,
                        SponsorName = SponserDto.SponsorName,
                        SponsorLogoId = imageid,
                        SponsorUserProfileId = SponserDto.SponsorUserProfileId,
                        SponsorTypeId = SponserDto.SponsorTypeId,
                    };

                    _context.Sponsors.Add(newSponsor);
                    _context.SaveChanges();

                    transiction.Commit();
                    return Ok("تم الاضافة بنجاح");
                }
                catch (Exception ex)
                {
                    transiction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut(Name = "AddPriceForAgeCategoryForBranch")]
        public IActionResult AddPriceForAgeCategoryForBranch([FromBody] SportInstitutionPriceDto priceDto)
        {
            try
            {
                var CheckExistingBranch = _context.SportInstitutionBranches.Where(x => x.SportInstitutionBranchId == priceDto.SportInstitutionBranchId).FirstOrDefault();
                if (CheckExistingBranch == null) return BadRequest($" غير موجود!{CheckExistingBranch.SportInstitutionBranchId}");

                var checkUser = _context.SportInstitutions.Where(x => x.UserProfileId == priceDto.UserProfileId &&
                                                        x.SportInstitutionId == CheckExistingBranch.SportInstitutionId).FirstOrDefault();
                if (checkUser == null) return BadRequest("!ليس لديك الصلاحية للقيام بهذا");

                var getSportInstitutionAgePriceForBranch = _context.SportInstitutionAgePrices.Where(x => x.SportInstitutionBranchId == priceDto.SportInstitutionBranchId).ToList();
                if (getSportInstitutionAgePriceForBranch.Count == 0 && getSportInstitutionAgePriceForBranch == null)
                    return BadRequest("!لم يتم اضافة اى فئات سنية بعد ");
                else
                {
                    foreach (var x in getSportInstitutionAgePriceForBranch)
                    {
                        x.Price = priceDto.price;
                    }
                    _context.SaveChanges();
                }

                return Ok("تم الاضافة بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(Name = "UpdateSportInstitutions")]
        public async Task<IActionResult> UpdateSportInstitutions(long userProfileid, long SportInstitutioId, [FromBody] UpdatesportInstitutionsDto sportInstitution, long? MainbranchId = null)
        {//updatesportinstituation and it's main branche if exist
            #region validation


            var enteredUser = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId.Equals(userProfileid));
            if (enteredUser == null)
                return BadRequest("لم يتم العثور على هذا المستخدم!");


            var existingSportInstitution = _context.SportInstitutions
                .Where(x => x.SportInstitutionId == SportInstitutioId).FirstOrDefault();
            if (existingSportInstitution == null)
                return NotFound("لم يتم العثور على المؤسسة الرياضية!");

            if (existingSportInstitution.UserProfileId != userProfileid)
                return BadRequest("لا يمكنك التعديل على هذة المؤسسة الرياضية!");


            var existingSportInstitutionBranch = _context.SportInstitutionBranches
              .Where(x => x.SportInstitutionBranchId == MainbranchId).FirstOrDefault();
            if (existingSportInstitutionBranch == null && MainbranchId != null)
                return NotFound("لم يتم العثور على الفرع الذى ادخلتة!");

            var usermopile = _context.Users.FirstOrDefault(x => x.UserId == enteredUser.UserId).Mobile;

            #endregion

            #region update sportInstitution

            existingSportInstitution.SportInstitutionName = sportInstitution.SportInstitutionName;
            existingSportInstitution.FounderName = sportInstitution.FounderName;
            existingSportInstitution.DateCreated = sportInstitution.DateCreated;
            existingSportInstitution.Gmail = sportInstitution.Gmail;
            existingSportInstitution.LastUpdate = DateTime.Now;
            existingSportInstitution.IsActive = true;

            enteredUser.Description = sportInstitution.Description;

            if (MainbranchId != null)
            {
                existingSportInstitutionBranch.BranchPhone = sportInstitution.MainBranchPhone;
                existingSportInstitutionBranch.CityDistricts = sportInstitution.MainBranchCityDistricts;
                existingSportInstitutionBranch.CityId = sportInstitution.MainBranchCCityId;
                existingSportInstitutionBranch.Location = sportInstitution.MainBranchLocation;
                existingSportInstitutionBranch.DateCreated = sportInstitution.MainBranchDateCreated;
                existingSportInstitutionBranch.LastUpdate = DateTime.Now;
                existingSportInstitutionBranch.IsActive = true;

                _context.SportInstitutionBranches.Update(existingSportInstitutionBranch);
            }

            _context.UserProfiles.Update(enteredUser);
            _context.SportInstitutions.Update(existingSportInstitution);
            await _context.SaveChangesAsync();

            return Ok("تم تحديث المؤسسة الرياضية بنجاح.");
            #endregion
        }

        [HttpPut(Name = "UpdateSportInstitutionBranch")]
        public IActionResult UpdateSportInstitutionBranch(long SportInstitutionid, long branchId, [FromBody] UpdateSportInstitutionBranchDTO branch)
        {
            var existingSportInstitution = _context.SportInstitutions
               .Where(x => x.SportInstitutionId == SportInstitutionid).FirstOrDefault();
            if (existingSportInstitution == null)
                return NotFound("لم يتم العثور على المؤسسة الرياضية!");

            var existingSportInstitutionBranch = _context.SportInstitutionBranches
               .Where(x => x.SportInstitutionBranchId == branchId).FirstOrDefault();
            if (existingSportInstitutionBranch == null)
                return NotFound("لم يتم العثور على الفرع الذى ادخلتة!");

            existingSportInstitutionBranch.BranchPhone = branch.BranchPhone;
            existingSportInstitutionBranch.CityId = branch.CityId;
            existingSportInstitutionBranch.CityDistricts = branch.CityDistricts;
            existingSportInstitutionBranch.SportInstitutionBranchName = branch.SportInstitutionBranchName;
            existingSportInstitutionBranch.Location = branch.Location;
            existingSportInstitutionBranch.DateCreated = branch.DateCreated;
            existingSportInstitutionBranch.LastUpdate = DateTime.Now;
            existingSportInstitutionBranch.IsActive = true;

            _context.SportInstitutionBranches.Update(existingSportInstitutionBranch);
            _context.SaveChanges();

            return Ok("تم تحديث الفرع بنجاح.");
        }

        [HttpPut(Name = "UpdateSportInstitutionEmployee")]
        public IActionResult UpdateSportInstitutionEmployee(long SportInstitutionBelongId, long SportInstitutionId, [FromBody] UpdateSportInstitutionBelongDto employee)
        {
            var SportInstitutionBelong = _context.SportInstitutionBelongs.Where(x => x.SportInstitutionBelongId == SportInstitutionBelongId).FirstOrDefault();
            if (SportInstitutionBelong == null) return NotFound("لم يتم العثور على هذا الشخص!");

            var baranch = _context.SportInstitutionBranches.Where(x => x.SportInstitutionBranchId == SportInstitutionBelong.SportInstitutionBranchId).FirstOrDefault();
            if (baranch == null) return NotFound("لم يتم العثور على الفرع الذى ينتمى الية هذا الشخص!");

            if (baranch.SportInstitutionId != SportInstitutionId) return BadRequest("لا يمكنك التعديل على هذا الشخص !");

            var Enteredbaranch = _context.SportInstitutionBranches.Where(x => x.SportInstitutionBranchId == employee.SportInstitutionBranchId).FirstOrDefault();
            if (Enteredbaranch == null) return NotFound("لم يتم العثور على الفرع الذى ادخلتة!");

            var belongtype = _context.SportInstitutionBelongBelongTypes.Where(x => x.BelongTypeId == employee.BelongTypeId).FirstOrDefault();
            if (belongtype == null) return NotFound("ادخل نوع عمل صحيح !");

            //if (employee.BelongTypeId == 2)
            //{
            //    var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == SportInstitutionBelong.BelongUserProfileId).FirstOrDefault();
            //    if (userprofile != null)
            //    {
            //        if (userprofile.RoleId != 4)
            //            return BadRequest("يجب على هذا المستخدم الانتساب كمدرب اولا للانضمام لهذا الدور!");
            //    }
            //}
            SportInstitutionBelong.SportInstitutionBranchId = employee.SportInstitutionBranchId;
            SportInstitutionBelong.ShortDescription = employee.ShortDescription;
            SportInstitutionBelong.BelongTypeId = employee.BelongTypeId;
            SportInstitutionBelong.LastUpdate = DateTime.Now;
            _context.SportInstitutionBelongs.Update(SportInstitutionBelong);
            _context.SaveChanges();

            return Ok("تم التعديل ينجاح");
        }

        [HttpDelete(Name = "DeleteBranche")]
        public IActionResult DeleteBranche(long branchId, long userprofileId)
        {

            var existingSportInstitutionBranch = _context.SportInstitutionBranches
             .Where(x => x.SportInstitutionBranchId == branchId).FirstOrDefault();
            if (existingSportInstitutionBranch == null)
                return NotFound("لم يتم العثور على الفرع الذى ادخلتة!");


            var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == userprofileId).FirstOrDefault();
            if (userprofile == null)
                return BadRequest("لم يتم العثور على هذا المستخدم!");

            var sportinnstituation = _context.SportInstitutions.Where(x => x.SportInstitutionId
                        == existingSportInstitutionBranch.SportInstitutionId).FirstOrDefault();

            if (sportinnstituation.UserProfileId != userprofileId)
                return BadRequest("لا يمكنك اجراء هذا الحذف!");

            _context.SportInstitutionBranches.Remove(existingSportInstitutionBranch);
            _context.SaveChanges();
            return Ok("تم الحذف بنجاح ");
        }

        [HttpDelete(Name = "DeletecAgeCategory")]
        public IActionResult DeletecAgeCategory(byte AgeCategoryId, long brancheId, long userprofileId)
        {
            var existingSportInstitutionBranch = _context.SportInstitutionBranches
           .Where(x => x.SportInstitutionBranchId == brancheId).FirstOrDefault();
            if (existingSportInstitutionBranch == null)
                return NotFound("لم يتم العثور على الفرع !");

            var ageprice = _context.SportInstitutionAgePrices.Where(x => x.SportInstitutionBranchId == brancheId && x.AgeCategoryId == AgeCategoryId).ToList();
            if (ageprice == null)
                return NotFound("لا يوجد اشتراكات  لهذة الفئة السنية!");

            var sportinnstituation = _context.SportInstitutions.Where(x => x.SportInstitutionId
                        == existingSportInstitutionBranch.SportInstitutionId).FirstOrDefault();

            if (sportinnstituation.UserProfileId != userprofileId)
                return BadRequest("لا يمكنك اجراء هذا الحذف!");

            _context.SportInstitutionAgePrices.RemoveRange(ageprice);
            _context.SaveChanges();
            return Ok("تم الحذف بنجاح");
        }

        [HttpDelete(Name = "DeleteSportInstitutionEmployee")]
        public IActionResult DeleteSportInstitutionEmployee(long SportInstitutionBelongId, long SportInstitutionId)
        {
            var SportInstitutionBelong = _context.SportInstitutionBelongs.Where(x => x.SportInstitutionBelongId == SportInstitutionBelongId).FirstOrDefault();
            if (SportInstitutionBelong == null)
                return NotFound("لم يتم العثور على هذا الشخص!");

            var baranch = _context.SportInstitutionBranches.Where(x => x.SportInstitutionBranchId == SportInstitutionBelong.SportInstitutionBranchId).FirstOrDefault();
            if (baranch == null)
                return NotFound("لم يتم العثور على الفرع الذى ينتمى الية هذا الشخص!");

            if (baranch.SportInstitutionId != SportInstitutionId)
                return BadRequest("لا يمكنك حذف هذا الشخص من الاكاديمية!");

            _context.SportInstitutionBelongs.Remove(SportInstitutionBelong);
            _context.SaveChanges();
            return Ok("تم الحذف بنجاح");
        }

        [HttpDelete(Name = "DeleteSportInstitutionSponser")]
        public IActionResult DeleteSportInstitutionSponser(long userProfileId, long SponsorId)
        {
            var checkSponser = _context.Sponsors.Where(x => x.SponsorId == SponsorId && x.UserProfileId == userProfileId).FirstOrDefault();
            if (checkSponser == null) return NotFound("!لم يتم العثور على هذا الراعى");

            var Userprofile = _context.UserProfiles.Where(x => x.UserProfileId == userProfileId).FirstOrDefault();

            var image = _context.Images.Where(x => x.ImageId == checkSponser.SponsorLogoId).FirstOrDefault();

            var ImageBathInServer = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\SportInstitution\{Userprofile}\" + checkSponser.SponsorLogo.ImageFileName;

            if (System.IO.File.Exists(ImageBathInServer))
                System.IO.File.Delete(ImageBathInServer);

            _context.Sponsors.Remove(checkSponser);

            if (image != null)
                _context.Images.Remove(image);

            _context.SaveChanges();
            return Ok("تم الحذف بنجاح");
        }

        ////////////

        //[HttpGet(Name = "folderAdvertisements")]
        //public IActionResult folderAdvertisements()
        //{
        //    //var pathh = @"C:\Inetpub\vhosts\mobile.hawisports.com\httpdocs\wwwroot\image\";
        //    var pathh = @"C:\Inetpub\vhosts\mobile.hawisports.com\httpdocs\wwwroot\image";
        //    var folderName = "Advertisements";
        //    Directory.CreateDirectory(Path.Combine(pathh, folderName));

        //    return Ok("created");
        //}


        //[HttpGet(Name = "folderUser")]
        //public IActionResult folderUser()
        //{
        //    //var pathh = @"C:\Inetpub\vhosts\mobile.hawisports.com\httpdocs\wwwroot\image\";
        //    var pathh = @"C:\Inetpub\vhosts\mobile.hawisports.com\httpdocs\wwwroot\image";
        //    var folderName = "Users";
        //    Directory.CreateDirectory(Path.Combine(pathh, folderName));

        //    return Ok("created");
        //}
        //[HttpGet(Name = "foldercountry")]
        //public IActionResult foldercountry()
        //{
        //    //var pathh = @"C:\Inetpub\vhosts\mobile.hawisports.com\httpdocs\wwwroot\image\";
        //    var pathh = @"C:\Inetpub\vhosts\mobile.hawisports.com\httpdocs\wwwroot\image";
        //    var folderName = "Events";
        //    Directory.CreateDirectory(Path.Combine(pathh, folderName));
        //    return Ok("created");
        //}
        //[HttpGet(Name = "folderEvent")]
        //public IActionResult folderEvent()
        //{
        //    //var pathh = @"C:\Inetpub\vhosts\mobile.hawisports.com\httpdocs\wwwroot\image\";
        //    var pathh = @"C:\Inetpub\vhosts\mobile.hawisports.com\httpdocs\wwwroot\image";
        //    var folderName = "Country";
        //    Directory.CreateDirectory(Path.Combine(pathh, folderName));
        //    return Ok("created");
        //}

        //[HttpGet(Name = "folderSportInstitution")]
        //public IActionResult v()
        //{
        //   // var pathh = @"C:\Inetpub\vhosts\mobile.hawisports.com\httpdocs\wwwroot\image\";
        //    var pathh = @"C:\Inetpub\vhosts\mobile.hawisports.com\httpdocs\wwwroot\image";
        //    var folderName = "SportInstitution";
        //    Directory.CreateDirectory(Path.Combine(pathh, folderName));
        //    return Ok("created");
        //}


        //[HttpGet(Name = "PendingAffiliation")]
        //public IActionResult PendingAffiliation()
        //{
        //   // var pathh = @"C:\Inetpub\vhosts\mobile.hawisports.com\httpdocs\wwwroot\image\";
        //    var pathh = @"C:\Inetpub\vhosts\mobile.hawisports.com\httpdocs\wwwroot\image";
        //    var folderName = "PendingAffiliation";
        //    Directory.CreateDirectory(Path.Combine(pathh, folderName));
        //    return Ok("created");
        //}

        //[HttpGet(Name = "Training")]
        //public IActionResult Training()
        //{
        //   // var pathh = @"C:\Inetpub\vhosts\mobile.hawisports.com\httpdocs\wwwroot\image\";
        //    var pathh = @"C:\Inetpub\vhosts\mobile.hawisports.com\httpdocs\wwwroot\image";
        //    var folderName = "Training";
        //    Directory.CreateDirectory(Path.Combine(pathh, folderName));
        //    return Ok("created");
        //}
    }
}

