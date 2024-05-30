using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchReferee
{
    public long MatchRefereeId { get; set; }

    public long MatchId { get; set; }

    public long MainRefereeId { get; set; }

    public long? Assistant1RefereeId { get; set; }

    public long? Assistant2RefereeId { get; set; }

    public long? FourthRefereeId { get; set; }

    public long? ResidentRefereeId { get; set; }

    public long? SupervisingRefereeId { get; set; }

    public long? UserProfileId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual UserProfile? Assistant1Referee { get; set; }

    public virtual UserProfile? Assistant2Referee { get; set; }

    public virtual ICollection<ChampionshipReferee> ChampionshipReferees { get; } = new List<ChampionshipReferee>();

    public virtual UserProfile? FourthReferee { get; set; }

    public virtual UserProfile MainReferee { get; set; } = null!;

    public virtual Match Match { get; set; } = null!;

    public virtual UserProfile? ResidentReferee { get; set; }

    public virtual UserProfile? SupervisingReferee { get; set; }

    public virtual UserProfile? UserProfile { get; set; }
}
