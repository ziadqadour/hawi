using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SportInstitutionEmployeeJobTypeXxx
{
    public byte JobTypeId { get; set; }

    public string? JobType { get; set; }

    public virtual ICollection<SportInstitutionEmployeeXxx> SportInstitutionEmployeeXxxes { get; } = new List<SportInstitutionEmployeeXxx>();
}
