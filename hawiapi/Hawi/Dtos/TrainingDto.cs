using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hawi.Dtos
{
    public class AddTrainingDto
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string? TrainingName { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public byte? TrainingTypeId { get; set; }

        public IFormFile? TrainingImage { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public bool? IsRepeated { get; set; }

        //details

       public List<byte>? Days { get; set; } 

        
        public long? BranchId { get; set; } 

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public List<double> TrainingTime { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public List<double> DurationInMinutes { get; set; } 


        public string? TrainingPlaceLocation { get; set; }

        public string? TrainingCost { get; set; }

        public byte? PlaygroundSizeId { get; set; } 

        public byte? PlaygroundFloorId { get; set; } 

    }
   
    public class UpdateTrainingDetailsDto
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public float? TrainingTime { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public float? DurationInMinutes { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string? TrainingPlaceLocation { get; set; }

        public string? TrainingCost { get; set; }
        
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public byte? PlaygroundSizeId { get; set; }
       
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public byte? PlaygroundFloorId { get; set; }
        
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public DateTime? TrainingDate { get; set; }
    }

    public class GetCoachOfTraininByListOfAgeCategoryDto
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public List<byte> AgecategoryId { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public long BranchId { get; set; }
    }

    public class TrainingAttendanceDto
    {
        public List<byte> AgeCategoryOfPlayer { get; set; }
        public List<long> CoachUserProfileId { get; set; }
        public List<long> AdministrativeUserProfileId { get; set; }

    }

    public class TrainingAttendanceForPersonsDto
    {
        public List<long> PendingUserProfileId { get; set; }
        public byte? TrainingType { get; set; }
    }
  
    public class TrainingpreparetationDto
    {
        public List<long> AttendanceId { get; set; }
    }

    public class TrainingEvaluationDto
    {
        public List<EvaluationDto> User { get; set; }
    }

    public class EvaluationDto
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public long AttendanceId { get; set; }
       
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public double? OutOfFive { get; set; }
        public string? EvaluationComment { get; set; }
    }

    public class UpdateEvaluationDto
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public double? OutOfFive { get; set; }
        public string? EvaluationComment { get; set; }
    }

    public class TrainingAdminDto
    {
        public List<long> AdminUserProfileId { get; set; }
    }

    public class TrainingInjuryDto
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public long AttendanceId { get; set; }
        public string? InjuryComment { get; set; }
        public UPDATETrainingInjuryDto injuryDtos { get; set; }
    }

    public class UPDATETrainingInjuryDto
    {
        public bool head { get; set; }
        public bool neck { get; set; }
        public bool leftShoulder { get; set; }
        public bool leftUpperArm { get; set; }
        public bool leftElbow { get; set; }
        public bool leftLowerArm { get; set; }
        public bool leftHand { get; set; }
        public bool rightShoulder { get; set; }
        public bool rightUpperArm { get; set; }
        public bool rightElbow { get; set; }
        public bool rightLowerArm { get; set; }
        public bool rightHand { get; set; }
        public bool upperBody { get; set; }
        public bool lowerBody { get; set; }
        public bool leftUpperLeg { get; set; }
        public bool leftKnee { get; set; }
        public bool leftLowerLeg { get; set; }
        public bool leftFoot { get; set; }
        public bool rightUpperLeg { get; set; }
        public bool rightKnee { get; set; }
        public bool rightLowerLeg { get; set; }
        public bool rightFoot { get; set; }
        public bool abdomen { get; set; }
    }


    public class ShowTrainingAttendanceDto
    {
        public long UserProfileId { get; set; }
        public long AttindanceId { get; set; }
        public string Name { get; set; } 
        public bool IsPresent { get; set; }
        public string type { get; set; }
        public string Image { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class GetRequestsToJoinTrainingDto
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public long UserProfileId { get; set; }
        public string? type { get; set; }
        public byte? trainingtypeId { get; set; }

        public long TrainingInvitationPendingId { get; set; }
    }


    public class TrainingTeamsPlayersDto
    {
        public List <TrainingTeamPlayerDto> TrainingTeamPlayer { get; set; }
    }
    public class TrainingTeamPlayerDto
    {
        public string TrainingTeamName { get; set; }
        public List<long> AttendanceId { get; set; }
    }

    public class MyModel
    {
        public bool Head { get; set; }
        public bool Neck { get; set; }
        public bool LeftShoulder { get; set; }
        public bool LeftUpperArm { get; set; }
        public bool LeftElbow { get; set; }
        public bool LeftLowerArm { get; set; }
        public bool LeftHand { get; set; }
        public bool RightShoulder { get; set; }
        public bool RightUpperArm { get; set; }
        public bool RightElbow { get; set; }
        public bool RightLowerArm { get; set; }
        public bool RightHand { get; set; }
        public bool UpperBody { get; set; }
        public bool LowerBody { get; set; }
        public bool LeftUpperLeg { get; set; }
        public bool LeftKnee { get; set; }
        public bool LeftLowerLeg { get; set; }
        public bool LeftFoot { get; set; }
        public bool RightUpperLeg { get; set; }
        public bool RightKnee { get; set; }
        public bool RightLowerLeg { get; set; }
        public bool RightFoot { get; set; }
        public bool Abdomen { get; set; }
        public bool Vestibular { get; set; }
    }

}
