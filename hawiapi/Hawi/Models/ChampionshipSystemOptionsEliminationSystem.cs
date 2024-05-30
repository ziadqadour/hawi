using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ChampionshipSystemOptionsEliminationSystem
{
    public long EliminationSystemId { get; set; }

    public bool HasExtraTime { get; set; }

    public long ChampionshipSystemId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ChampionshipSystem ChampionshipSystem { get; set; } = null!;
}
