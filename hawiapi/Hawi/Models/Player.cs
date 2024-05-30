using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Player
{
    public long PlayerId { get; set; }

    public long UserProfileId { get; set; }

    public byte? PlayerTypeId { get; set; }

    public byte? MainPlaceId { get; set; }

    public byte? AlternativePlaceId { get; set; }

    public byte? PlayerNumber { get; set; }

    public long? SportInstitutionBranchId { get; set; }

    public byte? SeasonId { get; set; }

    public byte? PlayerFeetId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual PlayerPlayerPlace? AlternativePlace { get; set; }

    public virtual PlayerPlayerPlace? MainPlace { get; set; }

    public virtual ICollection<MatchPlayer> MatchPlayers { get; } = new List<MatchPlayer>();

    public virtual PlayerPlayerFoot? PlayerFeet { get; set; }

    public virtual PlayerPlayerType? PlayerType { get; set; }

    public virtual Season? Season { get; set; }

    public virtual SportInstitutionBranch? SportInstitutionBranch { get; set; }

    public virtual UserProfile UserProfile { get; set; } = null!;
}
