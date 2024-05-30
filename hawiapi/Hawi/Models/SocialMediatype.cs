using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SocialMediatype
{
    public int Id { get; set; }

    public string? SocialMediaTypeName { get; set; }

    public bool? IsActive { get; set; }
}
