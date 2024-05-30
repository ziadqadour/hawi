using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SportInstitutionBelongBelongType
{
    public byte BelongTypeId { get; set; }

    public string? BelongType { get; set; }

    public virtual ICollection<SportInstitutionBelongPending> SportInstitutionBelongPendings { get; } = new List<SportInstitutionBelongPending>();
}
