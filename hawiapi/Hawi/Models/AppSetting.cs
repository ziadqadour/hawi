using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class AppSetting
{
    public short Id { get; set; }

    public byte? SeasonId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime CreateDate { get; set; }
}
