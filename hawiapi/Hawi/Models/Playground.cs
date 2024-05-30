using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Playground
{
    public long PlaygroundId { get; set; }

    public string PlaygroundName { get; set; } = null!;

    public long CityId { get; set; }

    public string PlaygroundLocation { get; set; } = null!;

    public byte? PlaygroundFloorId { get; set; }

    public byte? PlaygroundSizeId { get; set; }

    public string? ImagePath { get; set; }

    public long? UserProfileId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<ChampionshipsPlayGround> ChampionshipsPlayGrounds { get; } = new List<ChampionshipsPlayGround>();

    public virtual City City { get; set; } = null!;

    public virtual ICollection<MatchFriendlyMatch> MatchFriendlyMatches { get; } = new List<MatchFriendlyMatch>();

    public virtual ICollection<Match> Matches { get; } = new List<Match>();

    public virtual TrainingDetailsPlaygroundFloor? PlaygroundFloor { get; set; }

    public virtual TrainingDetailsPlaygroundSize? PlaygroundSize { get; set; }

    public virtual UserProfile? UserProfile { get; set; }
}
