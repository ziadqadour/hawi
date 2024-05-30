using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class UserProfile
{
    public long UserProfileId { get; set; }

    public long UserId { get; set; }

    public byte RoleId { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastUpdate { get; set; }

    public string? Token { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<Achievement> Achievements { get; } = new List<Achievement>();

    public virtual ICollection<AdvertisementSeen> AdvertisementSeens { get; } = new List<AdvertisementSeen>();

    public virtual ICollection<Advertisement> AdvertisementTargetUserProfiles { get; } = new List<Advertisement>();

    public virtual ICollection<Advertisement> AdvertisementUserProfiles { get; } = new List<Advertisement>();

    public virtual ICollection<AdvertisementVisit> AdvertisementVisits { get; } = new List<AdvertisementVisit>();

    public virtual ICollection<ArticleCommentLike> ArticleCommentLikes { get; } = new List<ArticleCommentLike>();

    public virtual ICollection<ArticleCommentNotification> ArticleCommentNotifications { get; } = new List<ArticleCommentNotification>();

    public virtual ICollection<ArticleComment> ArticleComments { get; } = new List<ArticleComment>();

    public virtual ICollection<ArticleHide> ArticleHides { get; } = new List<ArticleHide>();

    public virtual ICollection<ArticleLike> ArticleLikes { get; } = new List<ArticleLike>();

    public virtual ICollection<ArticleNotification> ArticleNotifications { get; } = new List<ArticleNotification>();

    public virtual ICollection<ArticleUsersSaved> ArticleUsersSaveds { get; } = new List<ArticleUsersSaved>();

    public virtual ICollection<Article> Articles { get; } = new List<Article>();

    public virtual ICollection<ChampionshipMatch> ChampionshipMatches { get; } = new List<ChampionshipMatch>();

    public virtual ICollection<ChampionshipReferee> ChampionshipReferees { get; } = new List<ChampionshipReferee>();

    public virtual ICollection<ChampionshipSystem> ChampionshipSystems { get; } = new List<ChampionshipSystem>();

    public virtual ICollection<ChampionshipTeam> ChampionshipTeams { get; } = new List<ChampionshipTeam>();

    public virtual ICollection<Championship> Championships { get; } = new List<Championship>();

    public virtual ICollection<ChampionshipsPlayGround> ChampionshipsPlayGrounds { get; } = new List<ChampionshipsPlayGround>();

    public virtual ICollection<Event> Events { get; } = new List<Event>();

    public virtual ICollection<ImageAlbum> ImageAlbums { get; } = new List<ImageAlbum>();

    public virtual ICollection<MatchCard> MatchCards { get; } = new List<MatchCard>();

    public virtual ICollection<MatchCoach> MatchCoachAssistantCoaches { get; } = new List<MatchCoach>();

    public virtual ICollection<MatchCoach> MatchCoachCoaches { get; } = new List<MatchCoach>();

    public virtual ICollection<MatchCoach> MatchCoachUserProfiles { get; } = new List<MatchCoach>();

    public virtual ICollection<MatchFriendlyMatchAgeCategory> MatchFriendlyMatchAgeCategories { get; } = new List<MatchFriendlyMatchAgeCategory>();

    public virtual ICollection<MatchFriendlyMatch> MatchFriendlyMatches { get; } = new List<MatchFriendlyMatch>();

    public virtual ICollection<MatchMatchRefereeCandidate> MatchMatchRefereeCandidates { get; } = new List<MatchMatchRefereeCandidate>();

    public virtual ICollection<MatchMatchRefereeRequest> MatchMatchRefereeRequests { get; } = new List<MatchMatchRefereeRequest>();

    public virtual ICollection<MatchPlayer> MatchPlayerPlayerUserProfiles { get; } = new List<MatchPlayer>();

    public virtual ICollection<MatchPlayer> MatchPlayerUserProfiles { get; } = new List<MatchPlayer>();

    public virtual ICollection<MatchReferee> MatchRefereeAssistant1Referees { get; } = new List<MatchReferee>();

    public virtual ICollection<MatchReferee> MatchRefereeAssistant2Referees { get; } = new List<MatchReferee>();

    public virtual ICollection<MatchReferee> MatchRefereeFourthReferees { get; } = new List<MatchReferee>();

    public virtual ICollection<MatchReferee> MatchRefereeMainReferees { get; } = new List<MatchReferee>();

    public virtual ICollection<MatchReferee> MatchRefereeResidentReferees { get; } = new List<MatchReferee>();

    public virtual ICollection<MatchReferee> MatchRefereeSupervisingReferees { get; } = new List<MatchReferee>();

    public virtual ICollection<MatchReferee> MatchRefereeUserProfiles { get; } = new List<MatchReferee>();

    public virtual ICollection<MatchScore> MatchScoreAssistPlayerUserProfiles { get; } = new List<MatchScore>();

    public virtual ICollection<MatchScore> MatchScorePlayerUserProfiles { get; } = new List<MatchScore>();

    public virtual ICollection<MatchScore> MatchScoreUserProfiles { get; } = new List<MatchScore>();

    public virtual ICollection<MatchSubstitution> MatchSubstitutionPlayerInUserProfiles { get; } = new List<MatchSubstitution>();

    public virtual ICollection<MatchSubstitution> MatchSubstitutionPlayerOutUserProfiles { get; } = new List<MatchSubstitution>();

    public virtual ICollection<MatchTechnicalTeam> MatchTechnicalTeams { get; } = new List<MatchTechnicalTeam>();

    public virtual ICollection<MatchUniform> MatchUniforms { get; } = new List<MatchUniform>();

    public virtual ICollection<Match> Matches { get; } = new List<Match>();

    public virtual ICollection<PendingAffiliationList> PendingAffiliationLists { get; } = new List<PendingAffiliationList>();

    public virtual ICollection<Player> Players { get; } = new List<Player>();

    public virtual ICollection<Playground> Playgrounds { get; } = new List<Playground>();

    public virtual ICollection<RealTimeNotification> RealTimeNotificationFromUserProfiles { get; } = new List<RealTimeNotification>();

    public virtual ICollection<RealTimeNotification> RealTimeNotificationToUserProfiles { get; } = new List<RealTimeNotification>();

    public virtual ICollection<Referee> Referees { get; } = new List<Referee>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Sponsor> SponsorSponsorUserProfiles { get; } = new List<Sponsor>();

    public virtual ICollection<Sponsor> SponsorUserProfiles { get; } = new List<Sponsor>();

    public virtual ICollection<SportInstitutionBelongPending> SportInstitutionBelongPendings { get; } = new List<SportInstitutionBelongPending>();

    public virtual ICollection<SportInstitutionBelong> SportInstitutionBelongs { get; } = new List<SportInstitutionBelong>();

    public virtual ICollection<SportInstitution> SportInstitutions { get; } = new List<SportInstitution>();

    public virtual ICollection<Subscription> SubscriptionSourceUserProfiles { get; } = new List<Subscription>();

    public virtual ICollection<Subscription> SubscriptionTargetUserProfiles { get; } = new List<Subscription>();

    public virtual ICollection<Training> Training { get; } = new List<Training>();

    public virtual ICollection<TrainingAdmin> TrainingAdmins { get; } = new List<TrainingAdmin>();

    public virtual ICollection<TrainingAttendance> TrainingAttendanceAttendanceUserProfiles { get; } = new List<TrainingAttendance>();

    public virtual ICollection<TrainingAttendance> TrainingAttendanceUserProfiles { get; } = new List<TrainingAttendance>();

    public virtual ICollection<TrainingEvaluation> TrainingEvaluations { get; } = new List<TrainingEvaluation>();

    public virtual ICollection<TrainingInjury> TrainingInjuries { get; } = new List<TrainingInjury>();

    public virtual ICollection<TrainingInvitationPending> TrainingInvitationPendingPendingUserProfiles { get; } = new List<TrainingInvitationPending>();

    public virtual ICollection<TrainingInvitationPending> TrainingInvitationPendingTrainingOwnerUserProfiles { get; } = new List<TrainingInvitationPending>();

    public virtual ICollection<TrainingPlayerXxx> TrainingPlayerXxxPlayerUserProfiles { get; } = new List<TrainingPlayerXxx>();

    public virtual ICollection<TrainingPlayerXxx> TrainingPlayerXxxUserProfiles { get; } = new List<TrainingPlayerXxx>();

    public virtual ICollection<TrainingSponsor> TrainingSponsors { get; } = new List<TrainingSponsor>();

    public virtual ICollection<TrainingTeam> TrainingTeams { get; } = new List<TrainingTeam>();

    public virtual ICollection<Uniform> Uniforms { get; } = new List<Uniform>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserProfileImage> UserProfileImages { get; } = new List<UserProfileImage>();
}
