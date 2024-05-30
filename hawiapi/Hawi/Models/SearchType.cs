using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SearchType
{
    public byte SearchTypeId { get; set; }

    public string? SearchKay { get; set; }

    public DateTime? CreateDate { get; set; }
}
