using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ChampionshipSystemOptionsMixedLeagueSystem
{
    public long MixedLeagueSystemId { get; set; }

    public byte? NumOfGroups { get; set; }

    public byte? TeamsPerGroup { get; set; }

    public byte? QualifiersNumberFromEachGroup { get; set; }

    public long? ChampionshipSystemId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ChampionshipSystem? ChampionshipSystem { get; set; }
}
