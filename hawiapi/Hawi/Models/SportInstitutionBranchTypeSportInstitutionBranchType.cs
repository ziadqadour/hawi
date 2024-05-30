using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SportInstitutionBranchTypeSportInstitutionBranchType
{
    public byte SportInstitutionBranchTypeId { get; set; }

    public string? SportInstitutionBranchType { get; set; }

    public virtual ICollection<SportInstitutionBranch> SportInstitutionBranches { get; } = new List<SportInstitutionBranch>();
}
