using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Subscription
{
    public long SubscriptionId { get; set; }

    public long TargetUserProfileId { get; set; }

    public long? SourceUserProfileId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual UserProfile? SourceUserProfile { get; set; }

    public virtual UserProfile TargetUserProfile { get; set; } = null!;
}
