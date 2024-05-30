using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class RefereeRefereeType
{
    public byte RefereeTypeId { get; set; }

    public string? RefereeType { get; set; }

    public virtual ICollection<Referee> Referees { get; } = new List<Referee>();
}
