using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class TrainingTeam
{
    public long TrainingTeamId { get; set; }

    public string? TrainingTeamName { get; set; }

    public long TrainingDetailsId { get; set; }

    public long UserProfileId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual TrainingDetail TrainingDetails { get; set; } = null!;

    public virtual ICollection<TrainingTeamPlayer> TrainingTeamPlayers { get; } = new List<TrainingTeamPlayer>();

    public virtual UserProfile UserProfile { get; set; } = null!;
}
