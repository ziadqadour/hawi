using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ChampionshipSystem
{
    public long ChampionshipSystemId { get; set; }

    public long ChampionshipId { get; set; }

    public double? MatchDuration { get; set; }

    public double? BreakTime { get; set; }

    public byte? Substitutions { get; set; }

    public byte ChampionshipSystemOptionsId { get; set; }

    public long? UserProfileId { get; set; }

    public byte? NumberOfFormation { get; set; }

    public byte? NumberOfPlayersOnList { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Championship Championship { get; set; } = null!;

    public virtual ChampionshipSystemOption ChampionshipSystemOptions { get; set; } = null!;

    public virtual ICollection<ChampionshipSystemOptionsEliminationSystem> ChampionshipSystemOptionsEliminationSystems { get; } = new List<ChampionshipSystemOptionsEliminationSystem>();

    public virtual ICollection<ChampionshipSystemOptionsLeagueSystem> ChampionshipSystemOptionsLeagueSystems { get; } = new List<ChampionshipSystemOptionsLeagueSystem>();

    public virtual ICollection<ChampionshipSystemOptionsMixedLeagueSystem> ChampionshipSystemOptionsMixedLeagueSystems { get; } = new List<ChampionshipSystemOptionsMixedLeagueSystem>();

    public virtual ICollection<ChampionshipTeamRanking> ChampionshipTeamRankings { get; } = new List<ChampionshipTeamRanking>();

    public virtual UserProfile? UserProfile { get; set; }
}
