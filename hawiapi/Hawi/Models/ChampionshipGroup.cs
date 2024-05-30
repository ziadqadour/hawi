using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ChampionshipGroup
{
    public long ChampionshipGroupId { get; set; }

    public long ChampionshipId { get; set; }

    public string ChampionshipGroupName { get; set; } = null!;

    public byte ChampionshipGroupNumber { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Championship Championship { get; set; } = null!;

    public virtual ICollection<ChampionshipGroupTeam> ChampionshipGroupTeams { get; } = new List<ChampionshipGroupTeam>();
}
