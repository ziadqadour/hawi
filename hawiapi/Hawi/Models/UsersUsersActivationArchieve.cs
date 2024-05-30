using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class UsersUsersActivationArchieve
{
    public long DisActivatedUserId { get; set; }

    public long UserId { get; set; }

    public DateTimeOffset? DateToDelete { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual User User { get; set; } = null!;
}
