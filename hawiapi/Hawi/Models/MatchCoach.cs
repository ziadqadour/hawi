using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchCoach
{
    public long MatchCoachId { get; set; }

    public long MatchId { get; set; }

    public long? CoachId { get; set; }

    public long? AssistantCoachId { get; set; }

    public long? TeamId { get; set; }

    public long? UserProfileId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual UserProfile? AssistantCoach { get; set; }

    public virtual UserProfile? Coach { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual SportInstitution? Team { get; set; }

    public virtual UserProfile? UserProfile { get; set; }
}
