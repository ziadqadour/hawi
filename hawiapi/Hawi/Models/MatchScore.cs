using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchScore
{
    public long MatchScoreId { get; set; }

    public long MatchId { get; set; }

    public byte? GoalMethodId { get; set; }

    public DateTime? LastUpdate { get; set; }

    public long UserProfileId { get; set; }

    public long? TeamId { get; set; }

    public DateTime CreateDate { get; set; }

    public string? GoalTime { get; set; }

    public long? PlayerUserProfileId { get; set; }

    public long? AssistPlayerUserProfileId { get; set; }

    public virtual UserProfile? AssistPlayerUserProfile { get; set; }

    public virtual MatchScoreGoalMethod? GoalMethod { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual UserProfile? PlayerUserProfile { get; set; }

    public virtual SportInstitution? Team { get; set; }

    public virtual UserProfile UserProfile { get; set; } = null!;
}
