using System.ComponentModel.DataAnnotations;

namespace Hawi.Dtos
{
   public class AddNewCoachstatisticsSeasonDto
    {

        [Required (ErrorMessage ="*")]
        public byte SeasonId { get; set; }

        [Required (ErrorMessage ="*")]
        public byte? CoachTypeId { get; set; }

        public short? NumberOfMatches { get; set; }
       
        public short? Win { get; set; }
       
        public short? Lose { get; set; }
        
        public short? Draw { get; set; }
   
        public short? GoalScored { get; set; }
    
        public short? GoalConceded { get; set; }
    }

    public class UpdateCoachstatisticsSeasonDto: AddNewCoachstatisticsSeasonDto
    {
    }
    public class getcoachDto: AddNewCoachstatisticsSeasonDto
    {
        public long CoachId { get; set; }
        public long UserProfileId { get; set; }

    }

    public class getRefereeDto
    {
        public long RefereeId { get; set; }

        public long UserProfileId { get; set; }
        public short? NumberOfMatches { get; set; }

        public short? Fouls { get; set; }

        public short? Penalties { get; set; }

        public short? YellowCards { get; set; }

        public short? RedCards { get; set; }

        public short? OffsideCases { get; set; }

        public short? FourthReferee { get; set; }

        public short? LineReferee { get; set; }
    }

    public class AddAchievementDto
    {
        [Required(ErrorMessage ="required")]
        public DateTime AchievementDate { get; set; }
        [Required(ErrorMessage = "required")]
        public string AchievementTitle { get; set; }
        [Required(ErrorMessage = "required")]
        public byte SeasonId { get; set; }
    }
    public class GetAchievementDto
    {
       public long AchievementId { get; set; }
        public DateTime? AchievementDate { get; set; }
        public string? AchievementTitle { get; set; }
        public string Season { get; set; }
    }
}
 