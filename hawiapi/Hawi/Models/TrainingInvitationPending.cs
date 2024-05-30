using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class TrainingInvitationPending
{
    public long TrainingInvitationPendingId { get; set; }

    public long TrainingOwnerUserProfileId { get; set; }

    public long PendingUserProfileId { get; set; }

    public long? TrainingId { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool? IsInvited { get; set; }

    public virtual UserProfile PendingUserProfile { get; set; } = null!;

    public virtual Training? Training { get; set; }

    public virtual UserProfile TrainingOwnerUserProfile { get; set; } = null!;
}
