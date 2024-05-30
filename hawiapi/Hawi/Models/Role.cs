using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Role
{
    public byte RoleId { get; set; }

    public string? Role1 { get; set; }

    public virtual ICollection<UserProfile> UserProfiles { get; } = new List<UserProfile>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
