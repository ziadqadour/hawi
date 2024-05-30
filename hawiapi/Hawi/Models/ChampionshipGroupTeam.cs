using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ChampionshipGroupTeam
{
    public long ChampionshipGroupTeamId { get; set; }

    public long ChampionshipGroupId { get; set; }

    public long ChampionshipTeamId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ChampionshipGroup ChampionshipGroup { get; set; } = null!;

    public virtual ChampionshipTeam ChampionshipTeam { get; set; } = null!;
}
