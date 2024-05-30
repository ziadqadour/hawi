using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Referee
{
    public long RefereeId { get; set; }

    public long UserProfileId { get; set; }

    public byte? RefereeTypeId { get; set; }

    public byte SeasonId { get; set; }

    public short? NumberOfMatches { get; set; }

    public short? Fouls { get; set; }

    public short? Penalties { get; set; }

    public short? YellowCards { get; set; }

    public short? RedCards { get; set; }

    public short? OffsideCases { get; set; }

    public short? FourthReferee { get; set; }

    public short? LineReferee { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual RefereeRefereeType? RefereeType { get; set; }

    public virtual Season Season { get; set; } = null!;

    public virtual UserProfile UserProfile { get; set; } = null!;
}
