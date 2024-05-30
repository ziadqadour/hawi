using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ChampionshipSystemOptionsLeagueSystem
{
    public long LeagueSystemId { get; set; }

    public bool IsLeague { get; set; }

    public long ChampionshipSystemId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ChampionshipSystem ChampionshipSystem { get; set; } = null!;
}
