using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class TrainingBranch
{
    public long TrainingBranchId { get; set; }

    public long TrainingId { get; set; }

    public long SportInstitutionBranchId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual SportInstitutionBranch SportInstitutionBranch { get; set; } = null!;

    public virtual Training Training { get; set; } = null!;
}
