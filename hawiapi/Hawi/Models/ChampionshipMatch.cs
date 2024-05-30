using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ChampionshipMatch
{
    public long ChampionshipMatchId { get; set; }

    public long ChampionshipId { get; set; }

    public long UserProfileId { get; set; }

    public long? MatchId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Championship Championship { get; set; } = null!;

    public virtual ICollection<ChampionshipMatchRound> ChampionshipMatchRounds { get; } = new List<ChampionshipMatchRound>();

    public virtual Match? Match { get; set; }

    public virtual UserProfile UserProfile { get; set; } = null!;
}
