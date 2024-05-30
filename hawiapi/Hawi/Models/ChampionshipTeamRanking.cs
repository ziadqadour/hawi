using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ChampionshipTeamRanking
{
    public long TeamsRankingId { get; set; }

    public long ChampionshipId { get; set; }

    public long ChampionshipSystemId { get; set; }

    public long ChampionshipTeamId { get; set; }

    public byte MatchesPlayed { get; set; }

    public byte Wins { get; set; }

    public byte Draws { get; set; }

    public byte Losses { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Championship Championship { get; set; } = null!;

    public virtual ChampionshipSystem ChampionshipSystem { get; set; } = null!;

    public virtual ChampionshipTeam ChampionshipTeam { get; set; } = null!;
}
