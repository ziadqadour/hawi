using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class PendingAffiliationListPendingAffiliationListType
{
    public byte PendingAffiliationListTypeId { get; set; }

    public string? PendingAffiliationListType { get; set; }

    public virtual ICollection<PendingAffiliationList> PendingAffiliationLists { get; } = new List<PendingAffiliationList>();
}
