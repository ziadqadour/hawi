using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchCardMatchCardType
{
    public byte MatchCardTypeId { get; set; }

    public string MatchCardType { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public virtual ICollection<MatchCard> MatchCards { get; } = new List<MatchCard>();
}
