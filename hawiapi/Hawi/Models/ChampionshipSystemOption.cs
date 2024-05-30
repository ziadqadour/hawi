using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ChampionshipSystemOption
{
    public byte ChampionshipSystemOptionsId { get; set; }

    public string ChampionshipSystemOption1 { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public virtual ICollection<ChampionshipSystem> ChampionshipSystems { get; } = new List<ChampionshipSystem>();
}
