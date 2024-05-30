using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class UserUserStatus
{
    public byte UserStatusId { get; set; }

    public string? UserStatus { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}
