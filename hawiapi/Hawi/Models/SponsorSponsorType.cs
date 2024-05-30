using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SponsorSponsorType
{
    public byte SponsorTypeId { get; set; }

    public string? SponsorType { get; set; }

    public virtual ICollection<Sponsor> Sponsors { get; } = new List<Sponsor>();
}
