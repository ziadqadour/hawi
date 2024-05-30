using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class PendingStatus
{
    public byte PendingAffiliationStatusId { get; set; }

    public string PendingAffiliationStatus { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public virtual ICollection<ArticleAdvertisement> ArticleAdvertisements { get; } = new List<ArticleAdvertisement>();

    public virtual ICollection<PendingAffiliationList> PendingAffiliationLists { get; } = new List<PendingAffiliationList>();
}
