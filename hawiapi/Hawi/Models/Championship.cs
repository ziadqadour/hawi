using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Championship
{
    public long ChampionshipsId { get; set; }

    public string ChampionshipsName { get; set; } = null!;

    public long CityId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public byte NumTeams { get; set; }

    public byte TargetedCategoriesId { get; set; }

    public bool IsMale { get; set; }

    public long? UserProfileId { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<ChampionshipAgeCategory> ChampionshipAgeCategories { get; } = new List<ChampionshipAgeCategory>();

    public virtual ICollection<ChampionshipGroup> ChampionshipGroups { get; } = new List<ChampionshipGroup>();

    public virtual ICollection<ChampionshipMatch> ChampionshipMatches { get; } = new List<ChampionshipMatch>();

    public virtual ICollection<ChampionshipReferee> ChampionshipReferees { get; } = new List<ChampionshipReferee>();

    public virtual ICollection<ChampionshipSystem> ChampionshipSystems { get; } = new List<ChampionshipSystem>();

    public virtual ICollection<ChampionshipTeamRanking> ChampionshipTeamRankings { get; } = new List<ChampionshipTeamRanking>();

    public virtual ICollection<ChampionshipTeam> ChampionshipTeams { get; } = new List<ChampionshipTeam>();

    public virtual ICollection<ChampionshipsPlayGround> ChampionshipsPlayGrounds { get; } = new List<ChampionshipsPlayGround>();

    public virtual City City { get; set; } = null!;

    public virtual SportInstitutionSportInstitutionType TargetedCategories { get; set; } = null!;

    public virtual UserProfile? UserProfile { get; set; }
}
