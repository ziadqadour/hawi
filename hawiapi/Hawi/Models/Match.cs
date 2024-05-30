using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Match
{
    public long MatchId { get; set; }

    public byte? SeasonId { get; set; }

    public byte? MatchTypeId { get; set; }

    public DateTime? MatchDate { get; set; }

    public long? HomeTeamId { get; set; }

    public long? AwayTeamId { get; set; }

    public long UserProfileId { get; set; }

    public bool? IsStart { get; set; }

    public bool? IsEnd { get; set; }

    public byte? HalfTimeBreak { get; set; }

    public byte? NumberOfPlayer { get; set; }

    public long? PlayGroundId { get; set; }

    public string? MatchTime { get; set; }

    public DateTime? MatchStartTime { get; set; }

    public DateTime? SecondHalfStartTime { get; set; }

    public DateTime? FirstHalfEndTime { get; set; }

    public byte? FirstHalfWastedTime { get; set; }

    public byte? SecondHalfWastedTime { get; set; }

    public DateTime? SecondHalfEndTime { get; set; }

    public double? MatchDuration { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual SportInstitution? AwayTeam { get; set; }

    public virtual ICollection<ChampionshipMatch> ChampionshipMatches { get; } = new List<ChampionshipMatch>();

    public virtual SportInstitution? HomeTeam { get; set; }

    public virtual ICollection<MatchCard> MatchCards { get; } = new List<MatchCard>();

    public virtual ICollection<MatchCoach> MatchCoaches { get; } = new List<MatchCoach>();

    public virtual ICollection<MatchMatchRefereeCandidate> MatchMatchRefereeCandidates { get; } = new List<MatchMatchRefereeCandidate>();

    public virtual ICollection<MatchMatchRefereeRequest> MatchMatchRefereeRequests { get; } = new List<MatchMatchRefereeRequest>();

    public virtual ICollection<MatchPlayer> MatchPlayers { get; } = new List<MatchPlayer>();

    public virtual ICollection<MatchReferee> MatchReferees { get; } = new List<MatchReferee>();

    public virtual ICollection<MatchScore> MatchScores { get; } = new List<MatchScore>();

    public virtual ICollection<MatchSubstitution> MatchSubstitutions { get; } = new List<MatchSubstitution>();

    public virtual ICollection<MatchTechnicalTeam> MatchTechnicalTeams { get; } = new List<MatchTechnicalTeam>();

    public virtual MatchMatchType? MatchType { get; set; }

    public virtual ICollection<MatchUniform> MatchUniforms { get; } = new List<MatchUniform>();

    public virtual Playground? PlayGround { get; set; }

    public virtual Season? Season { get; set; }

    public virtual UserProfile UserProfile { get; set; } = null!;
}
