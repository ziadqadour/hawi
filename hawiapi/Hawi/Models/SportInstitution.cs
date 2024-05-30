using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SportInstitution
{
    public long SportInstitutionId { get; set; }

    public long UserProfileId { get; set; }

    public byte? SportInstitutionTypeId { get; set; }

    public string? LicenseNumber { get; set; }

    public string? SportInstitutionName { get; set; }

    public string? FounderName { get; set; }

    public DateTime? DateCreated { get; set; }

    public string? BackGroundFileName { get; set; }

    public string? BackGroundUrlfullPath { get; set; }

    public string? LogoFileName { get; set; }

    public string? LogoUrlfullPath { get; set; }

    public string? Gmail { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<ChampionshipTeam> ChampionshipTeams { get; } = new List<ChampionshipTeam>();

    public virtual ICollection<MatchApplicantsSportInstituationForFriendlyMatch> MatchApplicantsSportInstituationForFriendlyMatches { get; } = new List<MatchApplicantsSportInstituationForFriendlyMatch>();

    public virtual ICollection<Match> MatchAwayTeams { get; } = new List<Match>();

    public virtual ICollection<MatchCoach> MatchCoaches { get; } = new List<MatchCoach>();

    public virtual ICollection<MatchFriendlyMatch> MatchFriendlyMatches { get; } = new List<MatchFriendlyMatch>();

    public virtual ICollection<Match> MatchHomeTeams { get; } = new List<Match>();

    public virtual ICollection<MatchPlayer> MatchPlayers { get; } = new List<MatchPlayer>();

    public virtual ICollection<MatchScore> MatchScores { get; } = new List<MatchScore>();

    public virtual ICollection<MatchSubstitution> MatchSubstitutions { get; } = new List<MatchSubstitution>();

    public virtual ICollection<MatchTechnicalTeam> MatchTechnicalTeams { get; } = new List<MatchTechnicalTeam>();

    public virtual ICollection<MatchUniform> MatchUniforms { get; } = new List<MatchUniform>();

    public virtual ICollection<SportInstitutionBranch> SportInstitutionBranches { get; } = new List<SportInstitutionBranch>();

    public virtual SportInstitutionSportInstitutionType? SportInstitutionType { get; set; }

    public virtual ICollection<Uniform> Uniforms { get; } = new List<Uniform>();

    public virtual UserProfile UserProfile { get; set; } = null!;
}
