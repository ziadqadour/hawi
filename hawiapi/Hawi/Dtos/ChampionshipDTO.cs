using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hawi.Dtos
{
  
    public class CreateChampionshipDTO
    {
        //shared "User Will not enter them i add value to them during step of code to able to use automapper  "
        public long? ChampionshipsId { get; set; } = null;
        public long? ChampionshipSystemId { get; set; } = null;

        //Championships
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public long? UserProfileId { get; set; }
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public string ChampionshipsName { get; set; } = null!;
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public long CityId { get; set; }   
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public byte NumTeams { get; set; }
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public byte TargetedCategoriesId { get; set; }
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public bool IsMale { get; set; }


        // age category 
        public List <byte>? AgeCategoryId { get; set; }


        //ChampionshipSystem 
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public double? MatchDuration { get; set; }
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public double? BreakTime { get; set; }
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public byte ChampionshipSystemOptionsId { get; set; }
        public byte? Substitutions { get; set; }
       
        public byte? NumberOfFormation { get; set; }

        public byte? NumberOfPlayersOnList { get; set; }


        //ChampionshipSystemOptionsLeagueSystem
        public bool IsLeague { get; set; } = false;


        //ChampionshipSystemOptionsEliminationSystem
        public bool HasExtraTime { get; set; }=false;


        //ChampionshipSystemOptionsMixedLeagueSystem
        public byte? NumOfGroups { get; set; }

        public byte? TeamsPerGroup { get; set; }

        public byte? QualifiersNumberFromEachGroup { get; set; }

        
        //ChampionshipsPlayGround
        public List<long>? PlayGroundId { get; set; }
       
        
        //public List<long>? TeamId { get; set; }
        // public bool? IsActive { get; set; }=true;
    }

    public class CreatePlayGroundDTO
    {

        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public long? UserProfileId { get; set; }
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public string PlaygroundName { get; set; } = null!;
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public long CityId { get; set; }
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public string PlaygroundLocation { get; set; } = null!;
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public byte? PlaygroundFloorId { get; set; }
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public byte? PlaygroundSizeId { get; set; }

        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public IFormFile Image { get; set; }

        public string? ImagePath { get; set; }

    }

    public class CreateChampionshipsTeamsDTO
    {
        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public List<long>? TeamId { get; set; }
        
        public bool? IsActive { get; set; } = true;

        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public long? ChampionshipsId { get; set; } = null;

        [Required(ErrorMessage = "!يرجى إدخال قيمة لهذا الحقل")]
        public long? UserProfileId { get; set; }
    }

    public class CreateChampionshipsMatchDTO
    {
        public long ChampionshipId { get; set; }
        public long UserProfileId { get; set; }
        public long? MatchId { get; set; }
    }

    // for ChampionshipDetailsDashboardDto
    public class DashBoardChampionshipDTO
    {
        public long ChampionshipsId { get; set; }
        public string IsMale { get; set; }
        public string ChampionshipsName { get; set; }
        public DateTime StartDate { get; set; }
        public List<string> AgeCategories { get; set; }
        public string CityArabicName { get; set; }
        public string StatusOfChampionship { get; set; }
        public long OwnerChampionshipUserProfileID { get; set; }
        public string OwnerChampionshipName { get; set; }
        public string OwnerChampionshipImage { get; set; }
        public long TargetCategoryId { get; set; }
    }

    public class ChampionshipDetailsDashboardDto
    {
        public long ChampionshipId { get; set; }
        public string? ChampionshipName { get; set; }
        public string? DateRange { get; set; }
        public long? CityId { get; set; }
        public string? City { get; set; }
        public string? Gender { get; set; }
        public List<AgeCategoryDto>? AgeCategories { get; set; }
        public int NumTeams { get; set; }
        public string? TargetedCategory { get; set; }
        public string? MatchDuration { get; set; }
        public string? BreakBetweenHalves { get; set; }
        public byte? Substitutions { get; set; }
        public byte? NumberOfFormation { get; set; }
        public byte? NumberOfPlayersOnList { get; set; }
        public string? systemType { get; set; }
        public List<string> Playgrounds { get; set; }
        public List<TeamDto>? Teams { get; set; }
        public List<MatchDto>? Matches { get; set; }
    }
    public class AgeCategoryDto
    {
        public byte AgeCategoryID { get; set; }
        public string Name { get; set; }
    }
    public class TeamDto
    {
        public long TeamID { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
    }
    public class MatchDto
    {
        public long? MatchID { get; set; }
        public long? HomeTeamID { get; set; }
        public string? HomeTeam { get; set; }
        public string? HomeTeamLogo { get; set; }
        public long? AwayTeamID { get; set; }
        public string? AwayTeam { get; set; }
        public string? AwayTeamLogo { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        
    }
    //

    public class AddAddChampionshipMathcRefereeDto
    {
        [Required (ErrorMessage ="*")]
        public long UserProfileId { get; set; }
       
        [Required(ErrorMessage = "*")]
        public long? ChampionshipId { get; set; }

        [Required(ErrorMessage = "*")]
        public long? MatchRefereeId { get; set; }
    }

}
