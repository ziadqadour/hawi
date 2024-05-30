using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SportInstitutionBranch
{
    public long SportInstitutionBranchId { get; set; }

    public long SportInstitutionId { get; set; }

    public byte SportInstitutionBranchTypeId { get; set; }

    public DateTime? DateCreated { get; set; }

    public string? CityDistricts { get; set; }

    public long? CityId { get; set; }

    public string? Location { get; set; }

    public string? BranchPhone { get; set; }

    public string? SportInstitutionBranchName { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual City? City { get; set; }

    public virtual ICollection<Player> Players { get; } = new List<Player>();

    public virtual SportInstitution SportInstitution { get; set; } = null!;

    public virtual ICollection<SportInstitutionAgePrice> SportInstitutionAgePrices { get; } = new List<SportInstitutionAgePrice>();

    public virtual ICollection<SportInstitutionBelong> SportInstitutionBelongs { get; } = new List<SportInstitutionBelong>();

    public virtual SportInstitutionBranchTypeSportInstitutionBranchType SportInstitutionBranchType { get; set; } = null!;

    public virtual ICollection<SportInstitutionEmployeeXxx> SportInstitutionEmployeeXxxes { get; } = new List<SportInstitutionEmployeeXxx>();

    public virtual ICollection<TrainingBranch> TrainingBranches { get; } = new List<TrainingBranch>();
}
