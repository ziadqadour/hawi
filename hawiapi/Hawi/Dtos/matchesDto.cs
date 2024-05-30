using System.ComponentModel.DataAnnotations;

namespace Hawi.Dtos
{
    public class AddMatchDto
    {
        [Required(ErrorMessage = "يجب تحديد نوع الماتش")]
        public byte? MatchTypeId { get; set; }

        [Required(ErrorMessage = "يجب تحديد تاريخ الماتش")]
        public DateTime? MatchDate { get; set; }

        [Required(ErrorMessage = "يجب تحديد موعد الماتش")]
        public string? MatchTime { get; set; }

        [Required(ErrorMessage = "يجب تحديد الفريق صاحب الارض")]
        public long? HomeTeamId { get; set; }

        [Required(ErrorMessage = "يجب تحديد  الفريق الضيف ")]
        public long? AwayTeamId { get; set; }

        public long PlayGroundID { get; set; }

        public byte? NumberOfPlayer { get; set; }

        public byte? HalfTimeBreak { get; set; }
        public double? MatchDuration { get; set; }
    }

    public class FriendlyMatchDto
    {
        //[Required (ErrorMessage = "SportInstituationId IS Required")]
        //public long? SportInstituationId { get; set; } = null;

        [Required(ErrorMessage = "MatchDate IS Required")]
        public DateTime MatchDate { get; set; }

        [Required(ErrorMessage = "MatchTime IS Required")]
        public string MatchTime { get; set; } = null!;

        [Required(ErrorMessage = "MatchDuration IS Required")]
        public byte? MatchDuration { get; set; }

        [Required(ErrorMessage = "PlayGroundId IS Required")]
        public long? PlayGroundId { get; set; }

        [Required(ErrorMessage = "IsRefereeRequest IS Required")]
        public bool? IsRefereeRequest { get; set; }

        public byte? NumberOfReferee { get; set; }

        public double? Price { get; set; }

        [Required(ErrorMessage = "UserProfileId IS Required")]
        public long? UserProfileId { get; set; }

        public List<byte> AgeCategoryId { get; set; }
    }
    public class GetFriendlyMatchDto : FriendlyMatchDto
    {
        public long SportInstituationId { get; set; }
        public long FriendlyMatchId { get; set; }
        public bool IsActive { get; set; }
        public string? SportInstitutionName { get; set; }
        public string? SportInstitutionImage { get; set; }
        public string? PlaygroundLocation { get; set; }
        public string? PlaygroundName { get; set; }
        public string? PlaygroundCityName { get; set; }
        public bool? RequestForMatchSubmission { get; set; }

        public List<string>? AgeCategory { get; set; }

    }

    public class CreateApplicantsSportInstituationForFriendlyMatchDto
    {
        public long SportInstituationId { get; set; }

        public long FriendlyMatchId { get; set; }

    }

    public class GetApplicantsSportInstituationForFriendlyMatchDto : CreateApplicantsSportInstituationForFriendlyMatchDto
    {
        public long ApplicantsSportInstituationId { get; set; }
        public string? SportInstitutionName { get; set; }
        public string? SportInstitutionImage { get; set; }

        public DateTime? CreateDateTime { get; set; }
    }


    public class AddMatchScore
    {
        [Required(ErrorMessage = "يجب ادخال الاعب صاحب الهدف !")]
        public long? PlayerUserProfileId { get; set; }

        [Required(ErrorMessage = "يجب ادخال الوقت الذى تم احراز الهدف فية !")]
        public string? GoalTime { get; set; }

        [Required(ErrorMessage = "يجب ادخال الفريق الذى احرز الهدف ! !")]
        public long? TeamId { get; set; }

        [Required(ErrorMessage = "يجب ادخال باى طريقة تم احراز الهدف! !")]
        public byte? GoalMethodId { get; set; }
    }

    public class MatchGoalsDto
    {
        public string HomeTeamName { get; set; }
        public long HomeTeamGoals { get; set; }
        public string AwayTeamName { get; set; }
        public long AwayTeamGoals { get; set; }
    }

    public class uniformDto
    {
        [Required(ErrorMessage = "يجب ادخال صورة الطقم الرياضى !")]
        public IFormFile? Uniformimage { get; set; }
        [Required(ErrorMessage = "يجب ادخال نوع الطقم الرياضى !")]
        public byte? UniformTypeId { get; set; }
    }


    public class AddMatchUniformDto
    {
        [Required(ErrorMessage = "*")]
        public long UserProfileId { get; set; }
        [Required(ErrorMessage = "*")]
        public long? TeamId { get; set; }
        [Required(ErrorMessage = "*")]
        public long MatchId { get; set; }
        [Required(ErrorMessage = "*")]
        public string? PlayerShortsColor { get; set; }
        [Required(ErrorMessage = "*")]
        public string? PlayerSocksColor { get; set; }
        [Required(ErrorMessage = "*")]
        public string? PlayerStshirtColor { get; set; }
        [Required(ErrorMessage = "*")]
        public string? GoalkeeperShortsColor { get; set; }
        [Required(ErrorMessage = "*")]
        public string? GoalkeeperSocksColor { get; set; }
        [Required(ErrorMessage = "*")]
        public string? GoalkeeperTshirtColor { get; set; }
    }
    public class MatchRefereeDto
    {
        [Required(ErrorMessage = "*")]
        public long userProfileId { get; set; }
        [Required(ErrorMessage = "*")]
        public long MatchId { get; set; }
        [Required(ErrorMessage = "*")]
        public long MainRefereeId { get; set; }
        public long? Assistant1RefereeId { get; set; }
        public long? Assistant2RefereeId { get; set; }
        public long? FourthRefereeId { get; set; }
        public long? ResidentRefereeId { get; set; }
        public long? SupervisingRefereeId { get; set; }
    }

    public class MainRefereeInfoDto
    {
        public long? userprofileid { get; set; }
        public string Name { get; set; }
        public string ImgPath { get; set; }
    }

    public class MatchCoachDto
    {
        [Required(ErrorMessage = "*")]
        public long? CoachId { get; set; }

        public long? AssistantCoachId { get; set; }

        [Required(ErrorMessage = "*")]
        public long? TeamId { get; set; }
    }

    public class CoachInfoDto
    {
        public long? userprofileid { get; set; }
        public string? Name { get; set; }
        public string? ImgPath { get; set; }
        public string? Type { get; set; }
    }
    //MatchFormationOfTeams
    public class AddMatchFormationOfTeamsDto
    {
        [Required(ErrorMessage = "required")]
        public long UserProfileId { get; set; }
        [Required(ErrorMessage = "required")]
        public long MatchId { get; set; }
        [Required(ErrorMessage = "required")]
        public long SportInstituationId { get; set; }
        [Required(ErrorMessage = "required")]
        public List<MatchFormationDto> matchFormationDtos { get; set; }

    }

    public class MatchFormationDto
    {
        ////[Required(ErrorMessage = "required")]
        ////public long PlayerId { get; set; }
        [Required(ErrorMessage = "PlayerUserProfileId is required")]
        public long PlayerUserProfileId { get; set; }

        [Required(ErrorMessage = "required")]
        public byte PlaceId { get; set; }

        [Required(ErrorMessage = "required")]
        public byte PlayerNumber { get; set; }

        [Required(ErrorMessage = "required")]
        public bool IsStart { get; set; }
    }


    public class UpdateMatchFormationOfTeamsDto
    {
        [Required(ErrorMessage = "required")]
        public List<long> PlayerId { get; set; }
        [Required(ErrorMessage = "required")]
        public List<byte> PlaceId { get; set; }
        public List<byte>? PlayerNumber { get; set; } = null;
        [Required(ErrorMessage = "required")]
        public List<bool> IsStart { get; set; }
    }

    public class sportInstituationPlayerDto
    {
        public long PlayerId { get; set; }
        public long UserProfileId { get; set; }
        public string? Name { get; set; }
        public byte? PlayerNumber { get; set; }
        public string? playerPlace { get; set; }

    }

    //AddPlayerDataBySportinstituation
    public class AddPlayerDataBySportinstituationDto
    {
        [Required(ErrorMessage = "required")]
        public long UserProfileId { get; set; }

        [Required(ErrorMessage = "required")]
        public long SportinstituationbranchId { get; set; }

        [Required(ErrorMessage = "required")]
        public byte SeasonId { get; set; }

        [Required(ErrorMessage = "required")]
        public byte PlayerTypeId { get; set; }

        [Required(ErrorMessage = "required")]
        public byte MainPlaceId { get; set; }

        public byte? AlternativePlaceId { get; set; }

        [Required(ErrorMessage = "required")]
        public byte PlayerNumber { get; set; }

        [Required(ErrorMessage = "required")]
        public byte PlayerFeetId { get; set; }
    }

    public class AddCardDto
    {
        [Required(ErrorMessage = "required")]
        public byte MatchCardTypeId { get; set; }
        [Required(ErrorMessage = "required")]
        public long PlayerUserProfileId { get; set; }
        [Required(ErrorMessage = "required")]
        public string Minte { get; set; }

        public string? Reason { get; set; }
    }

    public class MatchSubstitutionDto
    {
        [Required(ErrorMessage = "required")]
        public long PlayerInUserProfileId { get; set; }
        [Required(ErrorMessage = "required")]
        public long PlayerOutUserProfileId { get; set; }
        [Required(ErrorMessage = "required")]
        public string? Minite { get; set; }

        [Required(ErrorMessage = "required")]
        public long? TeamId { get; set; }

        public string? Reason { get; set; }
    }

    public class GetPlayersOfTeamDto
    {
        public long SportInstituationId { get; set; }
        public List<byte>? AgeCategory { get; set; } = null;
    }

    public class PlayerInfoDto
    {
        public long PlayerId { get; set; }
        public long PlayerUserProfileId { get; set; }
        public long? UserProfileId { get; set; }
        public int? PlayerNumber { get; set; }
        public long? PlayerPlaceId { get; set; }
        public string PlayerPlace { get; set; }
        public int? PositionNumber { get; set; }
        public string Name { get; set; }
        public bool IsStart { get; set; }
        public string ImgPath { get; set; }
        public bool IsRefereeSubstituted { get; set; }
        public bool IsCoachSubstituted { get; set; }
        //public string postionNumber { get; set; }
    }

    public class MatchTechnicalTeamDto
    {
        public long UserProfileId { get; set; }
        public byte MatchTechnicalTeamTypeId { get; set; }
        public long MatchId { get; set; }
        public long? TeamId { get; set; }
    }

    //prepare player of match 
    public class MatchPreparationPlayer
    {
        public List<MatchPlayerprepareDto>? MatchPlayersprepare { get; set; }
    }
    public class MatchPlayerprepareDto
    {
        //  public long PlayerID { get; set; }
        public long PlayerUserProfileId { get; set; }
        public bool IsPrepare { get; set; }
    }

    public class MatchPreparationPlayerDto
    {
        public long PlayerUserProfileId { get; set; }
        public string? PlayerName { get; set; }
        public string? PlayerImage { get; set; }
        public bool? IsStart { get; set; }
        public bool? IsPrepare { get; set; }
        public int? PlayerNumber { get; set; }
        public int? PlaceId { get; set; }
        public long? PlayerId { get; set; }
    }

    public class MatchNeedRefereeDto
    {
        public long matchid { get; set; }
        public string HomeTeamLogoUrlfullPath { get; set; }
        public string HomeTeamSportInstitutionName { get; set; }
        public string AwayTeamLogoUrlfullPath { get; set; }
        public string AwayTeamSportInstitutionName { get; set; }
        public string CompetitionType { get; set; }
        public string MatchType { get; set; }
        public string MatchStartTime { get; set; }
        public string Time { get; set; }
        public string MinuteTime { get; set; }
        public int HalfTimeBreak { get; set; }
        public string MatchTime { get; set; }
        public int NumberOfPlayer { get; set; }
        public decimal EstimatedCost { get; set; }
        public string Description { get; set; }
        public string MatchPlase { get; set; }
        public string PlaygroundName { get; set; }
        public int NumberOfReferee { get; set; }
        public int NumberOfCandidateReferee { get; set; }
    }

    public class MatchEvent
    {
        public long MatchEventId { get; set; }
        public long MatchId { get; set; }
        public long? playerOneUserProfileId { get; set; }
        public string? playerOneName { get; set; }
        public long? playerTwoNameUserProfileId { get; set; }
        public string? playerTwoName { get; set; }
        public string EventTime { get; set; }
        public EventType Type { get; set; }

        // Properties specific to Goal
        public string? GoalMethodId { get; set; }
        public long? TeamId { get; set; }


        // Properties specific to Substitution
        //public string? SubstitutionMinute { get; set; }
        public string? SubstitutionReason { get; set; }

        // Properties specific to MatchCard
        public string MatchCardTypeId { get; set; }
        public string? MatchCardReason { get; set; }

    }

    public enum EventType
    {
        Goal,
        Substitution,
        MatchCard
    }


    // for player statistic in match
    public class PlayerStatisticsDto
    {
        public MatchCardDetails RedCard { get; set; }
        public List<MatchCardDetails> YellowCard { get; set; }
        public List<GoalDto> Goals { get; set; }
        public List<AssistDto> Assists { get; set; }
        public string PlayerPlace { get; set; }
        public string PlayerName { get; set; }
        public string PlayerImage { get; set; }
    }
    public class GoalDto
    {
        public string GoalTime { get; set; }
        public string HowIsGoal { get; set; }
    }
    public class AssistDto
    {
        public string GoalTime { get; set; }
    }

    public class MatchCardDetails
    {
        public string Minute { get; set; }
        public string Reason { get; set; }
    }

    public class MatchTime
    {
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }


}
