using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchMatchType
{
    public byte MatchTypeId { get; set; }

    public string? MatchType { get; set; }

    public virtual ICollection<Match> Matches { get; } = new List<Match>();
}
