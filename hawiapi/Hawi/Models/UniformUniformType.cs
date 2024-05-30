using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class UniformUniformType
{
    public byte UniformTypeId { get; set; }

    public string? UniformType { get; set; }

    public virtual ICollection<Uniform> Uniforms { get; } = new List<Uniform>();
}
