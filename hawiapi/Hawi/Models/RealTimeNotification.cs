using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class RealTimeNotification
{
    public long RealTimeNotificationId { get; set; }

    public long FromUserProfileId { get; set; }

    public long ToUserProfileId { get; set; }

    public byte TargetTypeId { get; set; }

    public long TargetId { get; set; }

    public string? ContentMessage { get; set; }

    public bool? IsRead { get; set; }

    public long? InvitationId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual UserProfile FromUserProfile { get; set; } = null!;

    public virtual RealTimeNotificationTargetType TargetType { get; set; } = null!;

    public virtual UserProfile ToUserProfile { get; set; } = null!;
}
