using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class PlayerPlayerType
{
    public byte PlayerTypeId { get; set; }

    public string? PlayerType { get; set; }

    public virtual ICollection<Player> Players { get; } = new List<Player>();
}
