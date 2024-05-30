using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ChampionshipTeam
{
    public long ChampionshipTeamId { get; set; }

    public long UserProfileId { get; set; }

    public long ChampionshipId { get; set; }

    public long TeamId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Championship Championship { get; set; } = null!;

    public virtual ICollection<ChampionshipGroupTeam> ChampionshipGroupTeams { get; } = new List<ChampionshipGroupTeam>();

    public virtual ICollection<ChampionshipTeamRanking> ChampionshipTeamRankings { get; } = new List<ChampionshipTeamRanking>();

    public virtual SportInstitution Team { get; set; } = null!;

    public virtual UserProfile UserProfile { get; set; } = null!;
}
