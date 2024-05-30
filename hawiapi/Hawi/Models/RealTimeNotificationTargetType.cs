using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class RealTimeNotificationTargetType
{
    public byte RealTimeNotificationTargetTypeId { get; set; }

    public string? RealTimeNotificationTargetType1 { get; set; }

    public virtual ICollection<RealTimeNotification> RealTimeNotifications { get; } = new List<RealTimeNotification>();
}
