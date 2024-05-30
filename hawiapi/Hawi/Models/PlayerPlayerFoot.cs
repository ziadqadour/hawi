using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class PlayerPlayerFoot
{
    public byte PlayerFeetId { get; set; }

    public string? FootName { get; set; }

    public virtual ICollection<Player> Players { get; } = new List<Player>();

    public virtual ICollection<UserBasicDatum> UserBasicData { get; } = new List<UserBasicDatum>();
}
