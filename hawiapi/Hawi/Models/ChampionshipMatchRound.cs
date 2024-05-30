using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ChampionshipMatchRound
{
    public long ChampionshipMatchRoundId { get; set; }

    public byte ChampionshipRoundId { get; set; }

    public long ChampionshipMatchId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ChampionshipMatch ChampionshipMatch { get; set; } = null!;

    public virtual ChampionshipRound ChampionshipRound { get; set; } = null!;
}
