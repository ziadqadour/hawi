using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchUniform
{
    public long MatchUniformId { get; set; }

    public long MatchId { get; set; }

    public long UserProfileId { get; set; }

    public long? TeamId { get; set; }

    public string? PlayerShortsColor { get; set; }

    public string? PlayerSocksColor { get; set; }

    public string? PlayerStshirtColor { get; set; }

    public string? GoalkeeperShortsColor { get; set; }

    public string? GoalkeeperSocksColor { get; set; }

    public string? GoalkeeperTshirtColor { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual SportInstitution? Team { get; set; }

    public virtual UserProfile UserProfile { get; set; } = null!;
}
