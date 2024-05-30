using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class TrainingAdmin
{
    public long TrainingAdminId { get; set; }

    public long TrainingId { get; set; }

    public long AdminUserProfileId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual UserProfile AdminUserProfile { get; set; } = null!;

    public virtual Training Training { get; set; } = null!;
}
