using System.ComponentModel.DataAnnotations;

namespace Hawi.Dtos
{
    public class UserForRegistrationDto
    {

        [Required(ErrorMessage = "يجب ادخال اسم المستخدم !")]
        public string? Name { get; init; }

        [Required(ErrorMessage = "يجب ادخال رقم هاتف !")]
        public string? Mobile { get; init; }

        [Required(ErrorMessage = "يجب ادخال كلمة مرور !")]
        [MinLength(6, ErrorMessage = "يجب الا تقل كلمة المرور عن 6  ارقام")]
        public string? Password { get; init; }

        [Compare("Password", ErrorMessage = "تأكيد كلمة المرور غير مطابق ، اكتب مرة أخرى!")]
        public string? ConfirmPassword { get; init; }

        [Required(ErrorMessage = "يجب ادخال كود الدولة !")]
        public byte? CountryCodeId { get; init; }
        [Required(ErrorMessage = "يجب ادخال نوع المستخدم!")]
        public bool? IsMale { get; set; }
        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "يجب ادخال المدينة!")]
        public long? CityId { get; set; }

        public bool? IsParent { get; set; } = false;
    }

    public class UserForLoginDto
    {

        [Required(ErrorMessage = "يجب ادخال رقم هاتف !")]
        public string? Mobile { get; init; }

        [Required(ErrorMessage = "يجب ادخال كلمة مرور !")]
        public string? Password { get; init; }
    }

    public class CreateNewFireBaseTokenDto
    {
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public string FirebaseToken { get; set; } = null!;
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public long UserId { get; set; }
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public string? DeviceToken { get; set; }
    }
    public class ResetPasswordDto
    {

        [Required(ErrorMessage = "!يجب ادخال كلمة مرور")]
        [MinLength(6)]
        public string? Password { get; init; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [Compare("Password", ErrorMessage = "كلمة المرور الجديدة وتأكيد كلمة المرور غير متطابقين.")]
        public string? ConfirmPassword { get; init; }
    }

    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "كلمة المرور الحالية مطلوبة")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور الجديدة وتأكيد كلمة المرور غير متطابقين.")]
        public string ConfirmPassword { get; set; }
    }
    public class UserDtos
    {
        public long UserId { get; set; }
        public string? Mobile { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public bool? IsMale { get; set; }
        public string? ImgUrl { get; set; }
        public string UserRole { get; set; }
        public string? LastLoginRoleId { get; set; }
        public DateTime? LastLoginDate { get; set; }
    };

    public class AddImageDto
    {
        [Required(ErrorMessage = "يجب اختيار صورة !")]
        public IFormFile image { get; set; }
    }

    public class SearchUserByPhoneDto
    {
        public string? UserName { get; set; }
        public string? userimage { get; set; }
        public string? Userrole { get; set; }
        public long? Userprofileid { get; set; }
    }

    public class ChangeLoginToSpecificRoleDTO
    {
        public long UserProfileId { get; set; }
        public long userid { get; set; }
        public byte RoleId { get; set; }
        public string? Role { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? ImgUrl { get; set; }
        public long Folowers { get; set; }
        public long unreadNotificationCount { get; set; }

    }

    public class GetjointdataDto : ChangeLoginToSpecificRoleDTO
    {
        public string? LocationUrl { get; set; }
        public bool IsFollow { get; set; }
        public string? Description { get; set; }
        public long? follows { get; set; }
        public string? backgroundImage { get; set; }
    }

    public class AddFullUserBasicDataDTO
    {
        public string? UserNeckName { get; set; }
        public byte? CityCountryId { get; set; }
        public long? CityId { get; set; }
        public string? Idnumber { get; set; }
        [Required(ErrorMessage = "!يجب ادخال الاسم")]
        public string? FirstName { get; set; }
        public string? FatherName { get; set; }
        public string? GrandFatherName { get; set; }
        public string? FamilyName { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        [Required(ErrorMessage = "!يجب ادخال تاريخ الميلاد")]
        public DateTime? Dob { get; set; }
        public string? Pob { get; set; }
        public string? PlaceOfResidence { get; set; }
        [Required(ErrorMessage = "!يجب ادخال الجنسية")]
        public byte? NationalityId { get; set; }
        public string? LocalFavoritePlayer { get; set; }
        public string? InternationalFavoritePlayer { get; set; }
        public int? LocalFavoriteClubId { get; set; }
        public int? InternationalFavoriteClubId { get; set; }
        public byte? InternationalFavoriteTeamId { get; set; }
        public byte? QualificationTypeId { get; set; }

        public byte? PlayerMainPlaceId { get; set; }
        public byte? PlayerSecondaryPlaceId { get; set; }
        public byte? PlayerFeetId { get; set; }

    }

    public class DescriptionDto
    {
        public string Description { get; set; }
    }
    public class reportsDto
    {
        public long users { get; set; }
        public long posts { get; set; }
        public long Advertisement { get; set; }
        public long SportInstitution { get; set; }
        public long events { get; set; }
        public long matches { get; set; }
        public long userprofile { get; set; }
        public long training { get; set; }

    }

    public class ImageInfo
    {
        //public long alpumId { get; set; } = 0;
        public long? ImageId { get; set; } = 0;
        public string? ImageUrl { get; set; }
        public DateTime? Date { get; set; }
        public bool IsVideo { get; set; } = false;
        // public bool IsDefault { get; set; } = true;
    }

    public class resultt
    {
        public long alpumId { get; set; } = 0;
        public string Name { get; set; }
        public bool IsDefault { get; set; } = true;
        public List<ImageInfo> Image { get; set; }

    }

    public class SearchParameters : OwnerParameters
    {
        [Required(ErrorMessage = "ادخل المستخدم")]
        public long UserProfileId { get; set; }

        [Required(ErrorMessage = "ادخل البحث الذى تردية")]
        public string SearchTerm { get; set; }

        [Required(ErrorMessage = "ادخل نوع البحث الذى تردية")]
        public byte SearchTypeId { get; set; }
        public DateTime? date { get; set; }
    }

    public class AddAlpum
    {
        [Required(ErrorMessage = "!يجب ادخال اسم الالبوم")]
        public string AlpumName { get; set; }

        [Required(ErrorMessage = "!يجب ادخال المستخدم")]
        public long UserProfileId { get; set; }
    }

    public class AddImagesInAlpum
    {
        [Required(ErrorMessage = "!يجب ادخال على الاقل صورة ")]
        public List<IFormFile> Images { get; set; }

        [Required(ErrorMessage = "!يجب اختيار الالبوم")]
        public long AlbumId { get; set; }

        [Required(ErrorMessage = "!يجب ادخال المستخدم")]
        public long UserProfileId { get; set; }
    }

    public class LocationInfo
    {
        public string? LLocation { get; set; }
        public string? LocationUrl { get; set; }
    }

    public class AddUserBasicDataImageDTO
    {
        [Required(ErrorMessage = "!هذا الحقل مطلوب ")]
        public long UserBasicDataId { get; set; }

        [Required(ErrorMessage = "!هذا الحقل مطلوب ")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "!هذا الحقل مطلوب ")]
        public IFormFile Image { get; set; }


    }

    public class UserDataForNotificationDto
    {
        public long UserProfileId { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public int NumOfUnReadNotificationForUserProfile { get; set; }
        public int NumOfUnReadNotificationForAllAcount { get; set; }
        public string? NameOfUserThatSendNotification { get; set; }
        public string? ImageOfUserThatSendNotification { get; set; }
    }
    public class NumOfNotificationDto
    {
        public int NumOfUnReadNotificationForUserProfile { get; set; }
        public int NumOfUnReadNotificationForAllAcount { get; set; }
    }


}


