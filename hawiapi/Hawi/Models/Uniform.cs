using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Uniform
{
    public long UniformId { get; set; }

    public long? TeamId { get; set; }

    public long UniformImageId { get; set; }

    public byte? UniformTypeId { get; set; }

    public long UserProfileId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual SportInstitution? Team { get; set; }

    public virtual Image UniformImage { get; set; } = null!;

    public virtual UniformUniformType? UniformType { get; set; }

    public virtual UserProfile UserProfile { get; set; } = null!;
}
