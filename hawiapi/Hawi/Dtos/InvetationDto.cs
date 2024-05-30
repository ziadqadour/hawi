using System.ComponentModel.DataAnnotations;

namespace Hawi.Dtos
{
    public class SearchUserDto
    {
        public long Userprofileid { get; set; }
        public string? Userrole { get; set; }
        public string? UserName { get; set; }
        public string? userimage { get; set; }
        public bool? RoleexistOrNot { get; set; }
    }

    public class AddBendingEmployeeDto
    {
        //[Required(ErrorMessage = "*")]
        public string? ShortDescription { get; set; }
        [Required(ErrorMessage = "*")]
        public byte? BelongTypeId { get; set; }
        [Required(ErrorMessage = "*")]
        public long? SportInstitutionBranchId { get; set; }

       // [Required(ErrorMessage = "*")]
        public List<byte> AgeCategories { get; set; }
    }


    public class SendSmsInvetationDto
    {
        [Required(ErrorMessage = "يجب ادخال رقم الهاتف!")]
        public string? Mobile { get; set; }
        [Required(ErrorMessage = "يجب ادخال اسم المؤسسة الرياضية!")]
        public string? SportInstituationName { get; set; }
        [Required(ErrorMessage = "يجب ادخال نوع المهنة!")]
        public string? JobTypeName { get; set; }
    }

    public class GetSpeceficUserNotificationDto
    {
        public long RealTimeNotificationId { get; set; }
        public long ToUserProfileId { get; set; }
        public long FromUserProfileId { get; set; }
        public long TargetId { get; set; }
        public DateTime CreateDate { get; set; }
        public string ContentMessage { get; set; }
        public long TargetTypeId { get; set; }
        public bool IsRead { get; set; }
        public string Image { get; set; }
    }
}

