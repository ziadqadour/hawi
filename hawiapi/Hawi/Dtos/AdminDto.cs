using System.ComponentModel.DataAnnotations;

namespace Hawi.Dtos
{
    
    public class AddSeasonDto
    {
        [Required (ErrorMessage ="*")]
        public DateTime StartDate { get; set; }
        
        [Required(ErrorMessage = "*")]
        public DateTime EndDate { get; set; }
    }

    public class UpdateSeasonDto: AddSeasonDto
    { }

}
