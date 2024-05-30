using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ArticleAdvertisement
{
    public long ArticleAdvertisementId { get; set; }

    public long ArticleId { get; set; }

    public byte PendingStatusId { get; set; }

    public string? AnotherPhoneNumber { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual PendingStatus PendingStatus { get; set; } = null!;
}
