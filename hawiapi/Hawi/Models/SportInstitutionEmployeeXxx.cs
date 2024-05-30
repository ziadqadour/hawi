using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SportInstitutionEmployeeXxx
{
    public long SportInstitutionEmployeeId { get; set; }

    public long SportInstitutionBranchId { get; set; }

    public string? EmployeeFullName { get; set; }

    public string? JobPosition { get; set; }

    public byte? JobTypeId { get; set; }

    public string? EmployeeemPhotoFileName { get; set; }

    public string? EmployeeemPhotoUrlfullPath { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual SportInstitutionEmployeeJobTypeXxx? JobType { get; set; }

    public virtual SportInstitutionBranch SportInstitutionBranch { get; set; } = null!;
}
