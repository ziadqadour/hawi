using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ChampionshipRound
{
    public byte RoundId { get; set; }

    public string RoundName { get; set; } = null!;

    public virtual ICollection<ChampionshipMatchRound> ChampionshipMatchRounds { get; } = new List<ChampionshipMatchRound>();
}
