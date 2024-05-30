using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Day
{
    public byte DayId { get; set; }

    public string? DayInEnglish { get; set; }

    public string? DayInArabic { get; set; }
}
