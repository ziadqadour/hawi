using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class UserBasicDataQualificationType
{
    public byte QualificationTypeId { get; set; }

    public string? QualificationType { get; set; }

    public virtual ICollection<UserBasicDatum> UserBasicData { get; } = new List<UserBasicDatum>();
}
