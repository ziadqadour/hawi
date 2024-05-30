using System.ComponentModel.DataAnnotations;

namespace Hawi.Dtos
{
    public class UserDetailsDto
    {
        public long UserId { get; set; }
        [Required(ErrorMessage = "يجب ادخال رقم هاتف !")]
        public string? ParentMobil { get; set; }
        [Required(ErrorMessage = "يجب ادخال رقم هاتف !")]
        public string? ParentChild { get; set; }
    }
}
