using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Hawi.Models;

public partial class HawiContext : DbContext
{
    public HawiContext()
    {
    }

    public HawiContext(DbContextOptions<HawiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<Advertisement> Advertisements { get; set; }

    public virtual DbSet<AdvertisementAgeRange> AdvertisementAgeRanges { get; set; }

    public virtual DbSet<AdvertisementAgeRangeAgeRange> AdvertisementAgeRangeAgeRanges { get; set; }

    public virtual DbSet<AdvertisementCity> AdvertisementCities { get; set; }

    public virtual DbSet<AdvertisementSeen> AdvertisementSeens { get; set; }

    public virtual DbSet<AdvertisementVisit> AdvertisementVisits { get; set; }

    public virtual DbSet<AppSetting> AppSettings { get; set; }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<ArticleAdvertisement> ArticleAdvertisements { get; set; }

    public virtual DbSet<ArticleAdvertisementAgeRange> ArticleAdvertisementAgeRanges { get; set; }

    public virtual DbSet<ArticleAdvertisementCity> ArticleAdvertisementCities { get; set; }

    public virtual DbSet<ArticleComment> ArticleComments { get; set; }

    public virtual DbSet<ArticleCommentLike> ArticleCommentLikes { get; set; }

    public virtual DbSet<ArticleCommentNotification> ArticleCommentNotifications { get; set; }

    public virtual DbSet<ArticleHide> ArticleHides { get; set; }

    public virtual DbSet<ArticleImage> ArticleImages { get; set; }

    public virtual DbSet<ArticleLike> ArticleLikes { get; set; }

    public virtual DbSet<ArticleNotification> ArticleNotifications { get; set; }

    public virtual DbSet<ArticleNotificationNotificationReason> ArticleNotificationNotificationReasons { get; set; }

    public virtual DbSet<ArticleUsersSaved> ArticleUsersSaveds { get; set; }

    public virtual DbSet<ArticleVideo> ArticleVideos { get; set; }

    public virtual DbSet<Championship> Championships { get; set; }

    public virtual DbSet<ChampionshipAgeCategory> ChampionshipAgeCategories { get; set; }

    public virtual DbSet<ChampionshipGroup> ChampionshipGroups { get; set; }

    public virtual DbSet<ChampionshipGroupTeam> ChampionshipGroupTeams { get; set; }

    public virtual DbSet<ChampionshipMatch> ChampionshipMatchs { get; set; }

    public virtual DbSet<ChampionshipMatchRound> ChampionshipMatchRounds { get; set; }

    public virtual DbSet<ChampionshipReferee> ChampionshipReferees { get; set; }

    public virtual DbSet<ChampionshipRound> ChampionshipRounds { get; set; }

    public virtual DbSet<ChampionshipSystem> ChampionshipSystems { get; set; }

    public virtual DbSet<ChampionshipSystemOption> ChampionshipSystemOptions { get; set; }

    public virtual DbSet<ChampionshipSystemOptionsEliminationSystem> ChampionshipSystemOptionsEliminationSystems { get; set; }

    public virtual DbSet<ChampionshipSystemOptionsLeagueSystem> ChampionshipSystemOptionsLeagueSystems { get; set; }

    public virtual DbSet<ChampionshipSystemOptionsMixedLeagueSystem> ChampionshipSystemOptionsMixedLeagueSystems { get; set; }

    public virtual DbSet<ChampionshipTeam> ChampionshipTeams { get; set; }

    public virtual DbSet<ChampionshipTeamRanking> ChampionshipTeamRankings { get; set; }

    public virtual DbSet<ChampionshipsPlayGround> ChampionshipsPlayGrounds { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CityCountry> CityCountries { get; set; }

    public virtual DbSet<CityDistrict> CityDistricts { get; set; }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Coach> Coaches { get; set; }

    public virtual DbSet<CoachCoachType> CoachCoachTypes { get; set; }

    public virtual DbSet<Day> Days { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventImage> EventImages { get; set; }

    public virtual DbSet<EventVideo> EventVideos { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<ImageAlbum> ImageAlbums { get; set; }

    public virtual DbSet<ImageAlbumDefaultAlbumType> ImageAlbumDefaultAlbumTypes { get; set; }

    public virtual DbSet<ImageAlbumImage> ImageAlbumImages { get; set; }

    public virtual DbSet<ImageImageType> ImageImageTypes { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<MatchApplicantsSportInstituationForFriendlyMatch> MatchApplicantsSportInstituationForFriendlyMatches { get; set; }

    public virtual DbSet<MatchCard> MatchCards { get; set; }

    public virtual DbSet<MatchCardMatchCardType> MatchCardMatchCardTypes { get; set; }

    public virtual DbSet<MatchCoach> MatchCoaches { get; set; }

    public virtual DbSet<MatchCompetitionType> MatchCompetitionTypes { get; set; }

    public virtual DbSet<MatchFriendlyMatch> MatchFriendlyMatches { get; set; }

    public virtual DbSet<MatchFriendlyMatchAgeCategory> MatchFriendlyMatchAgeCategories { get; set; }

    public virtual DbSet<MatchMatchRefereeCandidate> MatchMatchRefereeCandidates { get; set; }

    public virtual DbSet<MatchMatchRefereeRequest> MatchMatchRefereeRequests { get; set; }

    public virtual DbSet<MatchMatchType> MatchMatchTypes { get; set; }

    public virtual DbSet<MatchNumberOfPlayer> MatchNumberOfPlayers { get; set; }

    public virtual DbSet<MatchPlayer> MatchPlayers { get; set; }

    public virtual DbSet<MatchReferee> MatchReferees { get; set; }

    public virtual DbSet<MatchScore> MatchScores { get; set; }

    public virtual DbSet<MatchScoreGoalMethod> MatchScoreGoalMethods { get; set; }

    public virtual DbSet<MatchSubstitution> MatchSubstitutions { get; set; }

    public virtual DbSet<MatchTechnicalTeam> MatchTechnicalTeams { get; set; }

    public virtual DbSet<MatchTechnicalTeamType> MatchTechnicalTeamTypes { get; set; }

    public virtual DbSet<MatchUniform> MatchUniforms { get; set; }

    public virtual DbSet<PendingAffiliationList> PendingAffiliationLists { get; set; }

    public virtual DbSet<PendingAffiliationListPendingAffiliationListType> PendingAffiliationListPendingAffiliationListTypes { get; set; }

    public virtual DbSet<PendingStatus> PendingStatuses { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayerPlayerFoot> PlayerPlayerFeet { get; set; }

    public virtual DbSet<PlayerPlayerPlace> PlayerPlayerPlaces { get; set; }

    public virtual DbSet<PlayerPlayerType> PlayerPlayerTypes { get; set; }

    public virtual DbSet<Playground> Playgrounds { get; set; }

    public virtual DbSet<RealTimeNotification> RealTimeNotifications { get; set; }

    public virtual DbSet<RealTimeNotificationTargetType> RealTimeNotificationTargetTypes { get; set; }

    public virtual DbSet<Referee> Referees { get; set; }

    public virtual DbSet<RefereeRefereeType> RefereeRefereeTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SearchType> SearchTypes { get; set; }

    public virtual DbSet<Season> Seasons { get; set; }

    public virtual DbSet<SocialMediatype> SocialMediatypes { get; set; }

    public virtual DbSet<SocialMedium> SocialMedia { get; set; }

    public virtual DbSet<Sponsor> Sponsors { get; set; }

    public virtual DbSet<SponsorSponsorType> SponsorSponsorTypes { get; set; }

    public virtual DbSet<SportInstitution> SportInstitutions { get; set; }

    public virtual DbSet<SportInstitutionAgeCategoryBelong> SportInstitutionAgeCategoryBelongs { get; set; }

    public virtual DbSet<SportInstitutionAgePrice> SportInstitutionAgePrices { get; set; }

    public virtual DbSet<SportInstitutionAgePriceAgeCategory> SportInstitutionAgePriceAgeCategories { get; set; }

    public virtual DbSet<SportInstitutionAgePriceSubscriptionPeriod> SportInstitutionAgePriceSubscriptionPeriods { get; set; }

    public virtual DbSet<SportInstitutionBelong> SportInstitutionBelongs { get; set; }

    public virtual DbSet<SportInstitutionBelongBelongType> SportInstitutionBelongBelongTypes { get; set; }

    public virtual DbSet<SportInstitutionBelongPending> SportInstitutionBelongPendings { get; set; }

    public virtual DbSet<SportInstitutionBelongPendingAgeCategory> SportInstitutionBelongPendingAgeCategories { get; set; }

    public virtual DbSet<SportInstitutionBranch> SportInstitutionBranches { get; set; }

    public virtual DbSet<SportInstitutionBranchTypeSportInstitutionBranchType> SportInstitutionBranchTypeSportInstitutionBranchTypes { get; set; }

    public virtual DbSet<SportInstitutionEmployeeJobTypeXxx> SportInstitutionEmployeeJobTypeXxxes { get; set; }

    public virtual DbSet<SportInstitutionEmployeeXxx> SportInstitutionEmployeeXxxes { get; set; }

    public virtual DbSet<SportInstitutionSportInstitutionType> SportInstitutionSportInstitutionTypes { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Training> Training { get; set; }

    public virtual DbSet<TrainingAdmin> TrainingAdmins { get; set; }

    public virtual DbSet<TrainingAttendance> TrainingAttendances { get; set; }

    public virtual DbSet<TrainingBranch> TrainingBranches { get; set; }

    public virtual DbSet<TrainingDetail> TrainingDetails { get; set; }

    public virtual DbSet<TrainingDetailsPlaygroundFloor> TrainingDetailsPlaygroundFloors { get; set; }

    public virtual DbSet<TrainingDetailsPlaygroundSize> TrainingDetailsPlaygroundSizes { get; set; }

    public virtual DbSet<TrainingEvaluation> TrainingEvaluations { get; set; }

    public virtual DbSet<TrainingInjury> TrainingInjuries { get; set; }

    public virtual DbSet<TrainingInjuryInjuryPosition> TrainingInjuryInjuryPositions { get; set; }

    public virtual DbSet<TrainingInvitationPending> TrainingInvitationPendings { get; set; }

    public virtual DbSet<TrainingPlayerXxx> TrainingPlayerXxxes { get; set; }

    public virtual DbSet<TrainingSponsor> TrainingSponsors { get; set; }

    public virtual DbSet<TrainingTeam> TrainingTeams { get; set; }

    public virtual DbSet<TrainingTeamPlayer> TrainingTeamPlayers { get; set; }

    public virtual DbSet<TrainingTrainingType> TrainingTrainingTypes { get; set; }

    public virtual DbSet<Uniform> Uniforms { get; set; }

    public virtual DbSet<UniformUniformType> UniformUniformTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserDetails> userdetails { get; set; }

    public virtual DbSet<UserBasicDataQualificationType> UserBasicDataQualificationTypes { get; set; }

    public virtual DbSet<UserBasicDatum> UserBasicData { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<UserProfileImage> UserProfileImages { get; set; }

    public virtual DbSet<UserUserStatus> UserUserStatuses { get; set; }

    public virtual DbSet<UsersUsersActivationArchieve> UsersUsersActivationArchieves { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<VideoVideoType> VideoVideoTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=SQL8003.site4now.net;Initial Catalog=db_a874b7_hawi;User Id=db_a874b7_hawi_admin;Password=HawiDB@2023;MultipleActiveResultSets=True;Encrypt=true;TrustServerCertificate=true;Connection Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.AchievementId).HasName("PK_Achievements");

            entity.ToTable("Achievement");

            entity.Property(e => e.AchievementId).HasColumnName("AchievementID");
            entity.Property(e => e.AchievementDate).HasColumnType("smalldatetime");
            entity.Property(e => e.AchievementTitle).HasMaxLength(200);
            entity.Property(e => e.AchievementUserProfileId).HasColumnName("AchievementUserProfileID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.SeasonId).HasColumnName("SeasonID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Season).WithMany(p => p.Achievements)
                .HasForeignKey(d => d.SeasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Achievement_Season");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.Achievements)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Achievement_UserProfile");
        });

        modelBuilder.Entity<Advertisement>(entity =>
        {
            entity.ToTable("Advertisement");

            entity.Property(e => e.AdvertisementId).HasColumnName("AdvertisementID");
            entity.Property(e => e.AdvertisementFileName)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.AdvertisementTitle).HasMaxLength(200);
            entity.Property(e => e.AdvertisementUrlfullPath)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EndDate).HasColumnType("smalldatetime");
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
            entity.Property(e => e.TargetSite).HasMaxLength(500);
            entity.Property(e => e.TargetUserLogoFileName)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.TargetUserLogoUrlfullPath)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.TargetUserProfileId).HasColumnName("TargetUserProfileID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.TargetUserProfile).WithMany(p => p.AdvertisementTargetUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.TargetUserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Advertisement_TargetUserProfile");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.AdvertisementUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Advertisement_UserProfile");
        });

        modelBuilder.Entity<AdvertisementAgeRange>(entity =>
        {
            entity.ToTable("AdvertisementAgeRange");

            entity.Property(e => e.AdvertisementAgeRangeId).HasColumnName("AdvertisementAgeRangeID");
            entity.Property(e => e.AdvertisementId).HasColumnName("AdvertisementID");
            entity.Property(e => e.AgeRangeId).HasColumnName("AgeRangeID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.Advertisement).WithMany(p => p.AdvertisementAgeRanges)
                .HasForeignKey(d => d.AdvertisementId)
                .HasConstraintName("FK_AdvertisementAgeRange_AdvertisementAgeRange_Advertisement");

            entity.HasOne(d => d.AgeRange).WithMany(p => p.AdvertisementAgeRanges)
                .HasForeignKey(d => d.AgeRangeId)
                .HasConstraintName("FK_AdvertisementAgeRange_AdvertisementAgeRange_AgeRange");
        });

        modelBuilder.Entity<AdvertisementAgeRangeAgeRange>(entity =>
        {
            entity.HasKey(e => e.AgeRangeId).HasName("PK_Advertisement.AgeRange");

            entity.ToTable("AdvertisementAgeRange.AgeRange");

            entity.Property(e => e.AgeRangeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("AgeRangeID");
            entity.Property(e => e.AgeRange).HasMaxLength(50);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<AdvertisementCity>(entity =>
        {
            entity.ToTable("AdvertisementCity");

            entity.Property(e => e.AdvertisementCityId).HasColumnName("AdvertisementCityID");
            entity.Property(e => e.AdvertisementId).HasColumnName("AdvertisementID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.Advertisement).WithMany(p => p.AdvertisementCities)
                .HasForeignKey(d => d.AdvertisementId)
                .HasConstraintName("FK_AdvertisementCity_AdvertisementCity_Advertisement");

            entity.HasOne(d => d.City).WithMany(p => p.AdvertisementCities)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdvertisementCity_AdvertisementCity_City");
        });

        modelBuilder.Entity<AdvertisementSeen>(entity =>
        {
            entity.ToTable("AdvertisementSeen");

            entity.Property(e => e.AdvertisementSeenId).HasColumnName("AdvertisementSeenID");
            entity.Property(e => e.AdvertisementId).HasColumnName("AdvertisementID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Advertisement).WithMany(p => p.AdvertisementSeens)
                .HasForeignKey(d => d.AdvertisementId)
                .HasConstraintName("FK_AdvertisementSeen_AdvertisementSeen_Advertisement");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.AdvertisementSeens)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdvertisementSeen_AdvertisementSeen_UserProfile");
        });

        modelBuilder.Entity<AdvertisementVisit>(entity =>
        {
            entity.ToTable("AdvertisementVisit");

            entity.Property(e => e.AdvertisementVisitId).HasColumnName("AdvertisementVisitID");
            entity.Property(e => e.AdvertisementId).HasColumnName("AdvertisementID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Advertisement).WithMany(p => p.AdvertisementVisits)
                .HasForeignKey(d => d.AdvertisementId)
                .HasConstraintName("FK_AdvertisementVisit_Advertisement_Advertisement");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.AdvertisementVisits)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdvertisementVisit_AdvertisementVisit_Profile");
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.ArticleId).HasName("PK_News");

            entity.ToTable("Article");

            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.ArticleCreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.Articles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Article_ArticleUserProfile");
        });

        modelBuilder.Entity<ArticleAdvertisement>(entity =>
        {
            entity.ToTable("ArticleAdvertisement");

            entity.Property(e => e.ArticleAdvertisementId).HasColumnName("ArticleAdvertisementID");
            entity.Property(e => e.AnotherPhoneNumber).HasMaxLength(39);
            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.PendingStatusId).HasColumnName("PendingStatusID");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleAdvertisements)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleAdvertisement_Article1");

            entity.HasOne(d => d.PendingStatus).WithMany(p => p.ArticleAdvertisements)
                .HasForeignKey(d => d.PendingStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleAdvertisement_Status");
        });

        modelBuilder.Entity<ArticleAdvertisementAgeRange>(entity =>
        {
            entity.ToTable("ArticleAdvertisementAgeRange");

            entity.Property(e => e.ArticleAdvertisementAgeRangeId).HasColumnName("ArticleAdvertisementAgeRangeID");
            entity.Property(e => e.AgeRangeId).HasColumnName("AgeRangeID");
            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.AgeRange).WithMany(p => p.ArticleAdvertisementAgeRanges)
                .HasForeignKey(d => d.AgeRangeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleAdvertisementAgeRange_AgeRange");
        });

        modelBuilder.Entity<ArticleAdvertisementCity>(entity =>
        {
            entity.ToTable("ArticleAdvertisementCity");

            entity.Property(e => e.ArticleAdvertisementCityId).HasColumnName("ArticleAdvertisementCityID");
            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleAdvertisementCities)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleAdvertisementCity_Article");

            entity.HasOne(d => d.City).WithMany(p => p.ArticleAdvertisementCities)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleAdvertisementCity_City");
        });

        modelBuilder.Entity<ArticleComment>(entity =>
        {
            entity.HasKey(e => e.CommentId);

            entity.ToTable("ArticleComment");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.Comment).HasMaxLength(500);
            entity.Property(e => e.CommentUserProfileId).HasColumnName("CommentUserProfileID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleComments)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_ArticleComment_Article");

            entity.HasOne(d => d.CommentUserProfile).WithMany(p => p.ArticleComments)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.CommentUserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleComment_UserProfile");
        });

        modelBuilder.Entity<ArticleCommentLike>(entity =>
        {
            entity.ToTable("ArticleCommentLike");

            entity.Property(e => e.ArticleCommentLikeId).HasColumnName("ArticleCommentLikeID");
            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Comment).WithMany(p => p.ArticleCommentLikes)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("FK_ArticleCommentLike_ArticleCommentLike_Comment");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.ArticleCommentLikes)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleCommentLike_ArticleCommentLike_profile");
        });

        modelBuilder.Entity<ArticleCommentNotification>(entity =>
        {
            entity.ToTable("ArticleCommentNotification");

            entity.Property(e => e.ArticleCommentNotificationId).HasColumnName("ArticleCommentNotificationID");
            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.CommentNotificationMemo).HasMaxLength(200);
            entity.Property(e => e.CommentNotificationReasonId).HasColumnName("CommentNotificationReasonID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Comment).WithMany(p => p.ArticleCommentNotifications)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("FK_ArticleCommentNotification_ArticleCommentNotification_Comment");

            entity.HasOne(d => d.CommentNotificationReason).WithMany(p => p.ArticleCommentNotifications)
                .HasForeignKey(d => d.CommentNotificationReasonId)
                .HasConstraintName("FK_ArticleCommentNotification_ArticleCommentNotification_Reson");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.ArticleCommentNotifications)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleCommentNotification_ArticleCommentNotification_ProfileID");
        });

        modelBuilder.Entity<ArticleHide>(entity =>
        {
            entity.HasKey(e => e.ArticleHideId).HasName("PK_HideArticle");

            entity.ToTable("ArticleHide");

            entity.Property(e => e.ArticleHideId).HasColumnName("ArticleHideID");
            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleHides)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_HideArticle_HideArticle_Aritcle");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.ArticleHides)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HideArticle_HideArticle_UserProfile");
        });

        modelBuilder.Entity<ArticleImage>(entity =>
        {
            entity.ToTable("ArticleImage");

            entity.Property(e => e.ArticleImageId).HasColumnName("ArticleImageID");
            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.ImageTypeId).HasColumnName("ImageTypeID");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleImages)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_ArticleImage_Article");

            entity.HasOne(d => d.Image).WithMany(p => p.ArticleImages)
                .HasForeignKey(d => d.ImageId)
                .HasConstraintName("FK_ArticleImage_Image");

            entity.HasOne(d => d.ImageType).WithMany(p => p.ArticleImages)
                .HasForeignKey(d => d.ImageTypeId)
                .HasConstraintName("FK_ArticleImage_ImageType");
        });

        modelBuilder.Entity<ArticleLike>(entity =>
        {
            entity.HasKey(e => e.LikeId).HasName("PK_Likes");

            entity.ToTable("ArticleLike");

            entity.Property(e => e.LikeId).HasColumnName("LikeID");
            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.LikeCreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LikeUserProfileId).HasColumnName("LikeUserProfileID");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleLikes)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_ArticleLike_Article");

            entity.HasOne(d => d.LikeUserProfile).WithMany(p => p.ArticleLikes)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.LikeUserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleLike_UserProfile");
        });

        modelBuilder.Entity<ArticleNotification>(entity =>
        {
            entity.ToTable("ArticleNotification");

            entity.Property(e => e.ArticleNotificationId).HasColumnName("ArticleNotificationID");
            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.NotificationMemo).HasMaxLength(200);
            entity.Property(e => e.NotificationReasonId).HasColumnName("NotificationReasonID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleNotifications)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_ArticleNotification_Article");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.ArticleNotifications)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleNotification_ArticleNotification_profile");
        });

        modelBuilder.Entity<ArticleNotificationNotificationReason>(entity =>
        {
            entity.HasKey(e => e.NotificationReasonId);

            entity.ToTable("ArticleNotification.NotificationReason");

            entity.Property(e => e.NotificationReasonId)
                .ValueGeneratedOnAdd()
                .HasColumnName("NotificationReasonID");
            entity.Property(e => e.NotificationReason).HasMaxLength(50);
        });

        modelBuilder.Entity<ArticleUsersSaved>(entity =>
        {
            entity.HasKey(e => e.ArticleUsersSavedId).HasName("PK_articleArchived");

            entity.ToTable("ArticleUsersSaved");

            entity.Property(e => e.ArticleUsersSavedId).HasColumnName("ArticleUsersSavedID");
            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleUsersSaveds)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_articleArchived_Article");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.ArticleUsersSaveds)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_articleArchived_UserProfile1");
        });

        modelBuilder.Entity<ArticleVideo>(entity =>
        {
            entity.ToTable("ArticleVideo");

            entity.Property(e => e.ArticleVideoId).HasColumnName("ArticleVideoID");
            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");
            entity.Property(e => e.VideoTypeId).HasColumnName("VideoTypeID");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleVideos)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_ArticleVideo_Article");

            entity.HasOne(d => d.Video).WithMany(p => p.ArticleVideos)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleVideo_Video");

            entity.HasOne(d => d.VideoType).WithMany(p => p.ArticleVideos)
                .HasForeignKey(d => d.VideoTypeId)
                .HasConstraintName("FK_ArticleVideo_ArticleVideoType");
        });

        modelBuilder.Entity<Championship>(entity =>
        {
            entity.HasKey(e => e.ChampionshipsId);

            entity.Property(e => e.ChampionshipsId).HasColumnName("ChampionshipsID");
            entity.Property(e => e.ChampionshipsName).HasMaxLength(50);
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EndDate).HasColumnType("smalldatetime");
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
            entity.Property(e => e.TargetedCategoriesId).HasColumnName("TargetedCategoriesID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.City).WithMany(p => p.Championships)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Championships_City");

            entity.HasOne(d => d.TargetedCategories).WithMany(p => p.Championships)
                .HasForeignKey(d => d.TargetedCategoriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Championships_ChampionshipTargetedCategorie");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.Championships)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .HasConstraintName("FK_Championships_UserProfile");
        });

        modelBuilder.Entity<ChampionshipAgeCategory>(entity =>
        {
            entity.ToTable("ChampionshipAgeCategory");

            entity.Property(e => e.ChampionshipAgeCategoryId).HasColumnName("ChampionshipAgeCategoryID");
            entity.Property(e => e.AgeCategoryId).HasColumnName("AgeCategoryID");
            entity.Property(e => e.ChampionshipId).HasColumnName("ChampionshipID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.AgeCategory).WithMany(p => p.ChampionshipAgeCategories)
                .HasForeignKey(d => d.AgeCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipAgeCategory_AgeCategory");

            entity.HasOne(d => d.Championship).WithMany(p => p.ChampionshipAgeCategories)
                .HasForeignKey(d => d.ChampionshipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipAgeCategory_Championship");
        });

        modelBuilder.Entity<ChampionshipGroup>(entity =>
        {
            entity.ToTable("ChampionshipGroup");

            entity.Property(e => e.ChampionshipGroupId).HasColumnName("ChampionshipGroupID");
            entity.Property(e => e.ChampionshipGroupName).HasMaxLength(50);
            entity.Property(e => e.ChampionshipId).HasColumnName("ChampionshipID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.Championship).WithMany(p => p.ChampionshipGroups)
                .HasForeignKey(d => d.ChampionshipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipGroup_Championship");
        });

        modelBuilder.Entity<ChampionshipGroupTeam>(entity =>
        {
            entity.ToTable("ChampionshipGroupTeam");

            entity.Property(e => e.ChampionshipGroupTeamId).HasColumnName("ChampionshipGroupTeamID");
            entity.Property(e => e.ChampionshipGroupId).HasColumnName("ChampionshipGroupID");
            entity.Property(e => e.ChampionshipTeamId).HasColumnName("ChampionshipTeamID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.ChampionshipGroup).WithMany(p => p.ChampionshipGroupTeams)
                .HasForeignKey(d => d.ChampionshipGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipGroupTeam_ChampionshipGroup");

            entity.HasOne(d => d.ChampionshipTeam).WithMany(p => p.ChampionshipGroupTeams)
                .HasForeignKey(d => d.ChampionshipTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipGroupTeam_ChampionshipTeam");
        });

        modelBuilder.Entity<ChampionshipMatch>(entity =>
        {
            entity.Property(e => e.ChampionshipMatchId).HasColumnName("ChampionshipMatchID");
            entity.Property(e => e.ChampionshipId).HasColumnName("ChampionshipID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Championship).WithMany(p => p.ChampionshipMatches)
                .HasForeignKey(d => d.ChampionshipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipMatchs_Championship");

            entity.HasOne(d => d.Match).WithMany(p => p.ChampionshipMatches)
                .HasForeignKey(d => d.MatchId)
                .HasConstraintName("FK_ChampionshipMatchs_Match");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.ChampionshipMatches)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipMatchs_UserProfile");
        });

        modelBuilder.Entity<ChampionshipMatchRound>(entity =>
        {
            entity.ToTable("ChampionshipMatchRound");

            entity.Property(e => e.ChampionshipMatchRoundId).HasColumnName("ChampionshipMatchRoundID");
            entity.Property(e => e.ChampionshipMatchId).HasColumnName("ChampionshipMatchID");
            entity.Property(e => e.ChampionshipRoundId).HasColumnName("ChampionshipRoundID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.ChampionshipMatch).WithMany(p => p.ChampionshipMatchRounds)
                .HasForeignKey(d => d.ChampionshipMatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipMatchRound_ChampionshipMatch");

            entity.HasOne(d => d.ChampionshipRound).WithMany(p => p.ChampionshipMatchRounds)
                .HasForeignKey(d => d.ChampionshipRoundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipMatchRound_Round");
        });

        modelBuilder.Entity<ChampionshipReferee>(entity =>
        {
            entity.ToTable("ChampionshipReferee");

            entity.Property(e => e.ChampionshipRefereeId).HasColumnName("ChampionshipRefereeID");
            entity.Property(e => e.ChampionshipId).HasColumnName("ChampionshipID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.MatchRefereeId).HasColumnName("MatchRefereeID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Championship).WithMany(p => p.ChampionshipReferees)
                .HasForeignKey(d => d.ChampionshipId)
                .HasConstraintName("FK_ChampionshipReferee_Championship");

            entity.HasOne(d => d.MatchReferee).WithMany(p => p.ChampionshipReferees)
                .HasForeignKey(d => d.MatchRefereeId)
                .HasConstraintName("FK_ChampionshipReferee_MatchReferee");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.ChampionshipReferees)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipReferee_UserProfile");
        });

        modelBuilder.Entity<ChampionshipRound>(entity =>
        {
            entity.HasKey(e => e.RoundId);

            entity.ToTable("ChampionshipRound");

            entity.Property(e => e.RoundId)
                .ValueGeneratedOnAdd()
                .HasColumnName("RoundID");
            entity.Property(e => e.RoundName).HasMaxLength(50);
        });

        modelBuilder.Entity<ChampionshipSystem>(entity =>
        {
            entity.ToTable("ChampionshipSystem");

            entity.Property(e => e.ChampionshipSystemId).HasColumnName("ChampionshipSystemID");
            entity.Property(e => e.ChampionshipId).HasColumnName("ChampionshipID");
            entity.Property(e => e.ChampionshipSystemOptionsId).HasColumnName("ChampionshipSystemOptionsID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Championship).WithMany(p => p.ChampionshipSystems)
                .HasForeignKey(d => d.ChampionshipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipSystem_Championship");

            entity.HasOne(d => d.ChampionshipSystemOptions).WithMany(p => p.ChampionshipSystems)
                .HasForeignKey(d => d.ChampionshipSystemOptionsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipSystem_ChampionshipSystemOptions");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.ChampionshipSystems)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .HasConstraintName("FK_ChampionshipSystem_UserProfileID");
        });

        modelBuilder.Entity<ChampionshipSystemOption>(entity =>
        {
            entity.HasKey(e => e.ChampionshipSystemOptionsId);

            entity.Property(e => e.ChampionshipSystemOptionsId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ChampionshipSystemOptionsID");
            entity.Property(e => e.ChampionshipSystemOption1)
                .HasMaxLength(50)
                .HasColumnName("ChampionshipSystemOption");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<ChampionshipSystemOptionsEliminationSystem>(entity =>
        {
            entity.HasKey(e => e.EliminationSystemId);

            entity.ToTable("ChampionshipSystemOptions.EliminationSystem");

            entity.Property(e => e.EliminationSystemId).HasColumnName("EliminationSystemID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.ChampionshipSystem).WithMany(p => p.ChampionshipSystemOptionsEliminationSystems)
                .HasForeignKey(d => d.ChampionshipSystemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipSystemOptions.EliminationSystem_ChampionshipSystemOptions");
        });

        modelBuilder.Entity<ChampionshipSystemOptionsLeagueSystem>(entity =>
        {
            entity.HasKey(e => e.LeagueSystemId);

            entity.ToTable("ChampionshipSystemOptions.LeagueSystem");

            entity.Property(e => e.LeagueSystemId).HasColumnName("LeagueSystemID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.ChampionshipSystem).WithMany(p => p.ChampionshipSystemOptionsLeagueSystems)
                .HasForeignKey(d => d.ChampionshipSystemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipSystemOptions.LeagueSystem_ChampionshipSystemOptions");
        });

        modelBuilder.Entity<ChampionshipSystemOptionsMixedLeagueSystem>(entity =>
        {
            entity.HasKey(e => e.MixedLeagueSystemId);

            entity.ToTable("ChampionshipSystemOptions.MixedLeagueSystem");

            entity.Property(e => e.MixedLeagueSystemId).HasColumnName("MixedLeagueSystemID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.ChampionshipSystem).WithMany(p => p.ChampionshipSystemOptionsMixedLeagueSystems)
                .HasForeignKey(d => d.ChampionshipSystemId)
                .HasConstraintName("FK_ChampionshipSystemOptions.MixedLeagueSystem_ChampionshipSystemOptions");
        });

        modelBuilder.Entity<ChampionshipTeam>(entity =>
        {
            entity.Property(e => e.ChampionshipTeamId).HasColumnName("ChampionshipTeamID");
            entity.Property(e => e.ChampionshipId).HasColumnName("ChampionshipID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Championship).WithMany(p => p.ChampionshipTeams)
                .HasForeignKey(d => d.ChampionshipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipTeams_Championship");

            entity.HasOne(d => d.Team).WithMany(p => p.ChampionshipTeams)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipTeams_Team");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.ChampionshipTeams)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipTeams_UserProfile");
        });

        modelBuilder.Entity<ChampionshipTeamRanking>(entity =>
        {
            entity.HasKey(e => e.TeamsRankingId);

            entity.ToTable("ChampionshipTeamRanking");

            entity.Property(e => e.TeamsRankingId).HasColumnName("TeamsRankingID");
            entity.Property(e => e.ChampionshipId).HasColumnName("ChampionshipID");
            entity.Property(e => e.ChampionshipSystemId).HasColumnName("ChampionshipSystemID");
            entity.Property(e => e.ChampionshipTeamId).HasColumnName("ChampionshipTeamID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.Championship).WithMany(p => p.ChampionshipTeamRankings)
                .HasForeignKey(d => d.ChampionshipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipTeamRanking_Championship");

            entity.HasOne(d => d.ChampionshipSystem).WithMany(p => p.ChampionshipTeamRankings)
                .HasForeignKey(d => d.ChampionshipSystemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipTeamRanking_ChampionshipSystem");

            entity.HasOne(d => d.ChampionshipTeam).WithMany(p => p.ChampionshipTeamRankings)
                .HasForeignKey(d => d.ChampionshipTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipTeamRanking_ChampionshipTeam");
        });

        modelBuilder.Entity<ChampionshipsPlayGround>(entity =>
        {
            entity.HasKey(e => e.ChampionshipPlayGroundId);

            entity.Property(e => e.ChampionshipPlayGroundId).HasColumnName("ChampionshipPlayGroundID");
            entity.Property(e => e.ChampionshipId).HasColumnName("ChampionshipID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.PlayGroundId).HasColumnName("PlayGroundID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Championship).WithMany(p => p.ChampionshipsPlayGrounds)
                .HasForeignKey(d => d.ChampionshipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipsPlayGrounds_Championships");

            entity.HasOne(d => d.PlayGround).WithMany(p => p.ChampionshipsPlayGrounds)
                .HasForeignKey(d => d.PlayGroundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChampionshipsPlayGrounds_PlayGrounds");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.ChampionshipsPlayGrounds)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .HasConstraintName("FK_ChampionshipsPlayGrounds_UserProfile");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK_Cities");

            entity.ToTable("City");

            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CityArabicName).HasMaxLength(100);
            entity.Property(e => e.CityCountryId).HasColumnName("CityCountryID");
            entity.Property(e => e.CityEnglishName).HasMaxLength(100);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");

            entity.HasOne(d => d.CityCountry).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CityCountryId)
                .HasConstraintName("FK_Cities_CityCountry");
        });

        modelBuilder.Entity<CityCountry>(entity =>
        {
            entity.ToTable("CityCountry");

            entity.Property(e => e.CityCountryId)
                .ValueGeneratedOnAdd()
                .HasColumnName("CityCountryID");
            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CountryArabicName).HasMaxLength(150);
            entity.Property(e => e.CountryEnglishName).HasMaxLength(150);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ImageUrl)
                .IsUnicode(false)
                .HasColumnName("ImageURL");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("regionCode");
        });

        modelBuilder.Entity<CityDistrict>(entity =>
        {
            entity.HasKey(e => e.CityDistrictsId);

            entity.Property(e => e.CityDistrictsId)
                .HasMaxLength(50)
                .HasColumnName("CityDistrictsID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.DistractArabicName).HasMaxLength(100);
            entity.Property(e => e.DistractEnglishName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");

            entity.HasOne(d => d.City).WithMany(p => p.CityDistricts)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CityDistricts_City");
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.ToTable("Club");

            entity.Property(e => e.ClubId).HasColumnName("ClubID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.ClubArabicName).HasMaxLength(100);
            entity.Property(e => e.ClubEnglishName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ClubLogoFileName)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.ClubLogoUrlfullPath)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.City).WithMany(p => p.Clubs)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Club_City");
        });

        modelBuilder.Entity<Coach>(entity =>
        {
            entity.ToTable("Coach");

            entity.Property(e => e.CoachId).HasColumnName("CoachID");
            entity.Property(e => e.CoachTypeId).HasColumnName("CoachTypeID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.SeasonId).HasColumnName("SeasonID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.CoachType).WithMany(p => p.Coaches)
                .HasForeignKey(d => d.CoachTypeId)
                .HasConstraintName("FK_Coach_Coach.CoachType");

            entity.HasOne(d => d.Season).WithMany(p => p.Coaches)
                .HasForeignKey(d => d.SeasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Coach_Season");
        });

        modelBuilder.Entity<CoachCoachType>(entity =>
        {
            entity.HasKey(e => e.CoachTypeId);

            entity.ToTable("Coach.CoachType");

            entity.Property(e => e.CoachTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("CoachTypeID");
            entity.Property(e => e.CoachType).HasMaxLength(50);
        });

        modelBuilder.Entity<Day>(entity =>
        {
            entity.Property(e => e.DayId)
                .ValueGeneratedOnAdd()
                .HasColumnName("DayID");
            entity.Property(e => e.DayInArabic).HasMaxLength(15);
            entity.Property(e => e.DayInEnglish).HasMaxLength(15);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("Event");

            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.EventCreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EventTitle).HasMaxLength(500);
            entity.Property(e => e.FinishDate).HasColumnType("smalldatetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");
            entity.Property(e => e.StratDate).HasColumnType("smalldatetime");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.Events)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Event_EventUserProfile");
        });

        modelBuilder.Entity<EventImage>(entity =>
        {
            entity.ToTable("EventImage");

            entity.Property(e => e.EventImageId).HasColumnName("EventImageID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.ImageTypeId).HasColumnName("ImageTypeID");

            entity.HasOne(d => d.Event).WithMany(p => p.EventImages)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventImage_Event");

            entity.HasOne(d => d.Image).WithMany(p => p.EventImages)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventImage_Image");

            entity.HasOne(d => d.ImageType).WithMany(p => p.EventImages)
                .HasForeignKey(d => d.ImageTypeId)
                .HasConstraintName("FK_EventImage_ImageType");
        });

        modelBuilder.Entity<EventVideo>(entity =>
        {
            entity.ToTable("EventVideo");

            entity.Property(e => e.EventVideoId).HasColumnName("EventVideoID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");
            entity.Property(e => e.VideoTypeId).HasColumnName("VideoTypeID");

            entity.HasOne(d => d.Event).WithMany(p => p.EventVideos)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventVideo_EventID");

            entity.HasOne(d => d.Video).WithMany(p => p.EventVideos)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventVideo_Video");

            entity.HasOne(d => d.VideoType).WithMany(p => p.EventVideos)
                .HasForeignKey(d => d.VideoTypeId)
                .HasConstraintName("FK_EventVideo_VideoType");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK_Images");

            entity.ToTable("Image");

            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ImageFileName)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.ImageTypeId).HasColumnName("ImageTypeID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrlfullPath)
                .HasMaxLength(256)
                .IsUnicode(false);

            entity.HasOne(d => d.ImageType).WithMany(p => p.Images)
                .HasForeignKey(d => d.ImageTypeId)
                .HasConstraintName("FK_Image_Type");
        });

        modelBuilder.Entity<ImageAlbum>(entity =>
        {
            entity.HasKey(e => e.ImageAlbumsId).HasName("PK_Image.ImageAlbum");

            entity.ToTable("ImageAlbum");

            entity.Property(e => e.ImageAlbumsId).HasColumnName("ImageAlbumsID");
            entity.Property(e => e.AlbumName).HasMaxLength(50);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.ImageAlbums)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .HasConstraintName("FK_ImageAlbum_UserProfile");
        });

        modelBuilder.Entity<ImageAlbumDefaultAlbumType>(entity =>
        {
            entity.HasKey(e => e.DefaultAlbumTypeId);

            entity.ToTable("ImageAlbum.DefaultAlbumType");

            entity.Property(e => e.DefaultAlbumTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("DefaultAlbumTypeID");
            entity.Property(e => e.DefaultAlbumName).HasMaxLength(50);
        });

        modelBuilder.Entity<ImageAlbumImage>(entity =>
        {
            entity.HasKey(e => e.ImageAlbumImagesId);

            entity.ToTable("ImageAlbum.Images");

            entity.Property(e => e.ImageAlbumImagesId).HasColumnName("ImageAlbumImagesID");
            entity.Property(e => e.AlbumId).HasColumnName("AlbumID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ImageId).HasColumnName("ImageID");

            entity.HasOne(d => d.Album).WithMany(p => p.ImageAlbumImages)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImageAlbum.Images_Album");

            entity.HasOne(d => d.Image).WithMany(p => p.ImageAlbumImages)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImageAlbum.Images_Image");
        });

        modelBuilder.Entity<ImageImageType>(entity =>
        {
            entity.HasKey(e => e.ImageTypeId).HasName("PK_ImageImageTypes");

            entity.ToTable("Image.ImageType");

            entity.Property(e => e.ImageTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ImageTypeID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ImageType).HasMaxLength(50);
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.ToTable("Match");

            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.AwayTeamId).HasColumnName("AwayTeamID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.FirstHalfEndTime).HasColumnType("smalldatetime");
            entity.Property(e => e.HomeTeamId).HasColumnName("HomeTeamID");
            entity.Property(e => e.MatchDate).HasColumnType("smalldatetime");
            entity.Property(e => e.MatchStartTime).HasColumnType("smalldatetime");
            entity.Property(e => e.MatchTime).HasMaxLength(20);
            entity.Property(e => e.MatchTypeId).HasColumnName("MatchTypeID");
            entity.Property(e => e.PlayGroundId).HasColumnName("PlayGroundID");
            entity.Property(e => e.SeasonId).HasColumnName("SeasonID");
            entity.Property(e => e.SecondHalfEndTime).HasColumnType("smalldatetime");
            entity.Property(e => e.SecondHalfStartTime).HasColumnType("smalldatetime");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.AwayTeam).WithMany(p => p.MatchAwayTeams)
                .HasForeignKey(d => d.AwayTeamId)
                .HasConstraintName("FK_Match_Match_Awayteam");

            entity.HasOne(d => d.HomeTeam).WithMany(p => p.MatchHomeTeams)
                .HasForeignKey(d => d.HomeTeamId)
                .HasConstraintName("FK_Match_Match_HomeTeam");

            entity.HasOne(d => d.MatchType).WithMany(p => p.Matches)
                .HasForeignKey(d => d.MatchTypeId)
                .HasConstraintName("FK_Match_MatchMatchType");

            entity.HasOne(d => d.PlayGround).WithMany(p => p.Matches)
                .HasForeignKey(d => d.PlayGroundId)
                .HasConstraintName("FK_Match_MatchPlayground");

            entity.HasOne(d => d.Season).WithMany(p => p.Matches)
                .HasForeignKey(d => d.SeasonId)
                .HasConstraintName("FK_Match_Season");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.Matches)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match_Match_USERpROFILE");
        });

        modelBuilder.Entity<MatchApplicantsSportInstituationForFriendlyMatch>(entity =>
        {
            entity.HasKey(e => e.ApplicantsSportInstituationId);

            entity.ToTable("Match.ApplicantsSportInstituationForFriendlyMatch");

            entity.Property(e => e.ApplicantsSportInstituationId).HasColumnName("ApplicantsSportInstituationID");
            entity.Property(e => e.CreateDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.FriendlyMatchId).HasColumnName("friendlyMatchID");

            entity.HasOne(d => d.FriendlyMatch).WithMany(p => p.MatchApplicantsSportInstituationForFriendlyMatches)
                .HasForeignKey(d => d.FriendlyMatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match.ApplicantsSportInstituationForFriendlyMatch_friendlyMatch");

            entity.HasOne(d => d.SportInstituation).WithMany(p => p.MatchApplicantsSportInstituationForFriendlyMatches)
                .HasForeignKey(d => d.SportInstituationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match.ApplicantsSportInstituationForFriendlyMatch_SportInstituation");
        });

        modelBuilder.Entity<MatchCard>(entity =>
        {
            entity.ToTable("MatchCard");

            entity.Property(e => e.MatchCardId).HasColumnName("MatchCardID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.MatchCardTypeId).HasColumnName("MatchCardTypeID");
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.Minte)
                .HasMaxLength(20)
                .HasColumnName("minte");
            entity.Property(e => e.PlayerUserProfileId).HasColumnName("PlayerUserProfileID");
            entity.Property(e => e.Reason)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("reason");

            entity.HasOne(d => d.MatchCardType).WithMany(p => p.MatchCards)
                .HasForeignKey(d => d.MatchCardTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchCard_MatchCardType");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchCards)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchCard_Match");

            entity.HasOne(d => d.PlayerUserProfile).WithMany(p => p.MatchCards)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.PlayerUserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchCard_PlayerUserProfileid");
        });

        modelBuilder.Entity<MatchCardMatchCardType>(entity =>
        {
            entity.HasKey(e => e.MatchCardTypeId).HasName("PK_MatchCardType");

            entity.ToTable("MatchCard.MatchCardType");

            entity.Property(e => e.MatchCardTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("MatchCardTypeID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.MatchCardType).HasMaxLength(20);
        });

        modelBuilder.Entity<MatchCoach>(entity =>
        {
            entity.ToTable("MatchCoach");

            entity.Property(e => e.MatchCoachId).HasColumnName("MatchCoachID");
            entity.Property(e => e.AssistantCoachId).HasColumnName("AssistantCoachID");
            entity.Property(e => e.CoachId).HasColumnName("CoachID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.AssistantCoach).WithMany(p => p.MatchCoachAssistantCoaches)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.AssistantCoachId)
                .HasConstraintName("FK_MatchCoach_AssistantCoach");

            entity.HasOne(d => d.Coach).WithMany(p => p.MatchCoachCoaches)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.CoachId)
                .HasConstraintName("FK_MatchCoach_Coach");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchCoaches)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchCoach_Match");

            entity.HasOne(d => d.Team).WithMany(p => p.MatchCoaches)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_MatchCoach_Team");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.MatchCoachUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .HasConstraintName("FK_MatchCoach_UserProfile");
        });

        modelBuilder.Entity<MatchCompetitionType>(entity =>
        {
            entity.HasKey(e => e.CompetitionTypeId);

            entity.ToTable("Match.CompetitionType");

            entity.Property(e => e.CompetitionTypeId).HasColumnName("CompetitionTypeID");
            entity.Property(e => e.CompetitionType).HasMaxLength(200);
        });

        modelBuilder.Entity<MatchFriendlyMatch>(entity =>
        {
            entity.HasKey(e => e.FriendlyMatchId);

            entity.ToTable("Match.FriendlyMatch");

            entity.Property(e => e.FriendlyMatchId).HasColumnName("friendlyMatchID");
            entity.Property(e => e.CreateDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.MatchDate).HasColumnType("smalldatetime");
            entity.Property(e => e.MatchTime).HasMaxLength(50);
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.PlayGround).WithMany(p => p.MatchFriendlyMatches)
                .HasForeignKey(d => d.PlayGroundId)
                .HasConstraintName("FK_Match.FriendlyMatch_PlayGround");

            entity.HasOne(d => d.SportInstituation).WithMany(p => p.MatchFriendlyMatches)
                .HasForeignKey(d => d.SportInstituationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match.FriendlyMatch_SportInstituation");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.MatchFriendlyMatches)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .HasConstraintName("FK_Match.FriendlyMatch_UserProfileID");
        });

        modelBuilder.Entity<MatchFriendlyMatchAgeCategory>(entity =>
        {
            entity.HasKey(e => e.FriendlyMatchAgeCategoryId);

            entity.ToTable("Match.FriendlyMatchAgeCategories");

            entity.Property(e => e.FriendlyMatchAgeCategoryId).HasColumnName("FriendlyMatchAgeCategoryID");
            entity.Property(e => e.AgeCategoryId).HasColumnName("AgeCategoryID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.FriendlyMatchId).HasColumnName("friendlyMatchID");
            entity.Property(e => e.UserprofileId).HasColumnName("UserprofileID");

            entity.HasOne(d => d.AgeCategory).WithMany(p => p.MatchFriendlyMatchAgeCategories)
                .HasForeignKey(d => d.AgeCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match.FriendlyMatchAgeCategories_AgeCategories");

            entity.HasOne(d => d.FriendlyMatch).WithMany(p => p.MatchFriendlyMatchAgeCategories)
                .HasForeignKey(d => d.FriendlyMatchId)
                .HasConstraintName("FK_Match.FriendlyMatchAgeCategories_FriendlyMatch");

            entity.HasOne(d => d.Userprofile).WithMany(p => p.MatchFriendlyMatchAgeCategories)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserprofileId)
                .HasConstraintName("FK_Match.FriendlyMatchAgeCategories_UserProfile");
        });

        modelBuilder.Entity<MatchMatchRefereeCandidate>(entity =>
        {
            entity.HasKey(e => e.MatchRefereeCandidateId);

            entity.ToTable("Match.MatchRefereeCandidate");

            entity.Property(e => e.MatchRefereeCandidateId).HasColumnName("MatchRefereeCandidateID");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ExpectedCost).HasColumnName("expectedCost");
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.MatchRefereeRequestsId).HasColumnName("MatchRefereeRequestsID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchMatchRefereeCandidates)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Match.MatchRefereeCandidate_Match");

            entity.HasOne(d => d.MatchRefereeRequests).WithMany(p => p.MatchMatchRefereeCandidates)
                .HasForeignKey(d => d.MatchRefereeRequestsId)
                .HasConstraintName("FK_Match.MatchRefereeCandidate_MatchRefereeRequests");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.MatchMatchRefereeCandidates)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .HasConstraintName("[FK_Match.MatchRefereeCandidate_UserProfile");
        });

        modelBuilder.Entity<MatchMatchRefereeRequest>(entity =>
        {
            entity.HasKey(e => e.MatchRefereeRequestId);

            entity.ToTable("Match.MatchRefereeRequests");

            entity.Property(e => e.MatchRefereeRequestId).HasColumnName("MatchRefereeRequestID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.MatchDate).HasColumnType("smalldatetime");
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.Place)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchMatchRefereeRequests)
                .HasForeignKey(d => d.MatchId)
                .HasConstraintName("FK_Match.MatchRefereeRequests_Match");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.MatchMatchRefereeRequests)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match.MatchRefereeRequests_UserProfile");
        });

        modelBuilder.Entity<MatchMatchType>(entity =>
        {
            entity.HasKey(e => e.MatchTypeId);

            entity.ToTable("Match.MatchType");

            entity.Property(e => e.MatchTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("MatchTypeID");
            entity.Property(e => e.MatchType).HasMaxLength(50);
        });

        modelBuilder.Entity<MatchNumberOfPlayer>(entity =>
        {
            entity.HasKey(e => e.NumberOfPlayerId);

            entity.ToTable("Match.NumberOfPlayer");

            entity.Property(e => e.NumberOfPlayerId).HasColumnName("NumberOfPlayerID");
            entity.Property(e => e.NumberOfPlayer).HasMaxLength(50);
        });

        modelBuilder.Entity<MatchPlayer>(entity =>
        {
            entity.ToTable("MatchPlayer");

            entity.Property(e => e.MatchPlayerId).HasColumnName("MatchPlayerID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.IsCoachSubstituted).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsPrepare).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsRefereeSubstituted).HasDefaultValueSql("((0))");
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.PlaceId).HasColumnName("PlaceID");
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchPlayers)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_MatchPlayer_Match");

            entity.HasOne(d => d.Place).WithMany(p => p.MatchPlayers)
                .HasForeignKey(d => d.PlaceId)
                .HasConstraintName("FK_MatchPlayer_Player.PlayerPlace");

            entity.HasOne(d => d.Player).WithMany(p => p.MatchPlayers)
                .HasForeignKey(d => d.PlayerId)
                .HasConstraintName("FK_MatchPlayer_Player");

            entity.HasOne(d => d.PlayerUserProfile).WithMany(p => p.MatchPlayerPlayerUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.PlayerUserProfileId)
                .HasConstraintName("FK_MatchPlayer_PlayerUserProfile");

            entity.HasOne(d => d.Team).WithMany(p => p.MatchPlayers)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_MatchPlayer_SportInstitution");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.MatchPlayerUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchPlayer_UserProfile");
        });

        modelBuilder.Entity<MatchReferee>(entity =>
        {
            entity.ToTable("MatchReferee");

            entity.Property(e => e.MatchRefereeId).HasColumnName("MatchRefereeID");
            entity.Property(e => e.Assistant1RefereeId).HasColumnName("Assistant1RefereeID");
            entity.Property(e => e.Assistant2RefereeId).HasColumnName("Assistant2RefereeID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.FourthRefereeId).HasColumnName("FourthRefereeID");
            entity.Property(e => e.MainRefereeId).HasColumnName("MainRefereeID");
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.ResidentRefereeId).HasColumnName("ResidentRefereeID");
            entity.Property(e => e.SupervisingRefereeId).HasColumnName("SupervisingRefereeID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Assistant1Referee).WithMany(p => p.MatchRefereeAssistant1Referees)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.Assistant1RefereeId)
                .HasConstraintName("FK_MatchReferee_UserProfile1");

            entity.HasOne(d => d.Assistant2Referee).WithMany(p => p.MatchRefereeAssistant2Referees)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.Assistant2RefereeId)
                .HasConstraintName("FK_MatchReferee_UserProfile2");

            entity.HasOne(d => d.FourthReferee).WithMany(p => p.MatchRefereeFourthReferees)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.FourthRefereeId)
                .HasConstraintName("FK_MatchReferee_UserProfile3");

            entity.HasOne(d => d.MainReferee).WithMany(p => p.MatchRefereeMainReferees)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.MainRefereeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchReferee_UserProfile");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchReferees)
                .HasForeignKey(d => d.MatchId)
                .HasConstraintName("FK_MatchReferee_Match");

            entity.HasOne(d => d.ResidentReferee).WithMany(p => p.MatchRefereeResidentReferees)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.ResidentRefereeId)
                .HasConstraintName("FK_MatchReferee_Residentreferee");

            entity.HasOne(d => d.SupervisingReferee).WithMany(p => p.MatchRefereeSupervisingReferees)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.SupervisingRefereeId)
                .HasConstraintName("FK_MatchReferee_Supervisingreferee");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.MatchRefereeUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .HasConstraintName("FK_MatchReferee_DataentryUserProfile");
        });

        modelBuilder.Entity<MatchScore>(entity =>
        {
            entity.ToTable("MatchScore");

            entity.Property(e => e.MatchScoreId).HasColumnName("MatchScoreID");
            entity.Property(e => e.AssistPlayerUserProfileId).HasColumnName("AssistPlayerUserProfileID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.GoalMethodId).HasColumnName("GoalMethodID");
            entity.Property(e => e.GoalTime).HasMaxLength(50);
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.PlayerUserProfileId).HasColumnName("PlayerUserProfileID");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.AssistPlayerUserProfile).WithMany(p => p.MatchScoreAssistPlayerUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.AssistPlayerUserProfileId)
                .HasConstraintName("FK_MatchScore_AssistPlayer");

            entity.HasOne(d => d.GoalMethod).WithMany(p => p.MatchScores)
                .HasForeignKey(d => d.GoalMethodId)
                .HasConstraintName("FK_MatchScore_MatchScore_GoalMethod");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchScores)
                .HasForeignKey(d => d.MatchId)
                .HasConstraintName("FK_MatchScore_MatchScore_mATCH");

            entity.HasOne(d => d.PlayerUserProfile).WithMany(p => p.MatchScorePlayerUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.PlayerUserProfileId)
                .HasConstraintName("FK_MatchScore_PlayerUserProfile");

            entity.HasOne(d => d.Team).WithMany(p => p.MatchScores)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_MatchScore_Team");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.MatchScoreUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchScore_MatchScore_Profile");
        });

        modelBuilder.Entity<MatchScoreGoalMethod>(entity =>
        {
            entity.HasKey(e => e.GoalMethodId);

            entity.ToTable("MatchScore.GoalMethod");

            entity.Property(e => e.GoalMethodId)
                .ValueGeneratedOnAdd()
                .HasColumnName("GoalMethodID");
            entity.Property(e => e.GoalMethod).HasMaxLength(50);
        });

        modelBuilder.Entity<MatchSubstitution>(entity =>
        {
            entity.HasKey(e => e.MatchSubstitutionsId);

            entity.Property(e => e.MatchSubstitutionsId).HasColumnName("MatchSubstitutionsID");
            entity.Property(e => e.ConfirmedByTheReferee).HasDefaultValueSql("((0))");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.Minite)
                .HasMaxLength(20)
                .HasColumnName("minite");
            entity.Property(e => e.PlayerInUserProfileId).HasColumnName("PlayerInUserProfileID");
            entity.Property(e => e.PlayerOutUserProfileId).HasColumnName("PlayerOutUserProfileID");
            entity.Property(e => e.Reason).HasMaxLength(50);

            entity.HasOne(d => d.Match).WithMany(p => p.MatchSubstitutions)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchSubstitutions_Match");

            entity.HasOne(d => d.PlayerInUserProfile).WithMany(p => p.MatchSubstitutionPlayerInUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.PlayerInUserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchSubstitutions_PlayerInUserProfile");

            entity.HasOne(d => d.PlayerOutUserProfile).WithMany(p => p.MatchSubstitutionPlayerOutUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.PlayerOutUserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchSubstitutions_PlayerOutUserProfile");

            entity.HasOne(d => d.Team).WithMany(p => p.MatchSubstitutions)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_MatchSubstitutions_SportInstituationId");
        });

        modelBuilder.Entity<MatchTechnicalTeam>(entity =>
        {
            entity.ToTable("MatchTechnicalTeam");

            entity.Property(e => e.MatchTechnicalTeamId).HasColumnName("MatchTechnicalTeamID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.MatchTechnicalTeamTypeId).HasColumnName("MatchTechnicalTeamTypeID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchTechnicalTeams)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchTechnicalTeam_Match");

            entity.HasOne(d => d.MatchTechnicalTeamType).WithMany(p => p.MatchTechnicalTeams)
                .HasForeignKey(d => d.MatchTechnicalTeamTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchTechnicalTeam_MatchTechnicalTeamType");

            entity.HasOne(d => d.Team).WithMany(p => p.MatchTechnicalTeams)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_MatchTechnicalTeam_Teamid");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.MatchTechnicalTeams)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchTechnicalTeam_Userprofile");
        });

        modelBuilder.Entity<MatchTechnicalTeamType>(entity =>
        {
            entity.ToTable("MatchTechnicalTeamType");

            entity.Property(e => e.MatchTechnicalTeamTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("MatchTechnicalTeamTypeID");
            entity.Property(e => e.TechnicalTeamType).HasMaxLength(50);
        });

        modelBuilder.Entity<MatchUniform>(entity =>
        {
            entity.ToTable("MatchUniform");

            entity.Property(e => e.MatchUniformId).HasColumnName("MatchUniformID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.GoalkeeperShortsColor).HasMaxLength(50);
            entity.Property(e => e.GoalkeeperSocksColor).HasMaxLength(50);
            entity.Property(e => e.GoalkeeperTshirtColor).HasMaxLength(50);
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.PlayerShortsColor).HasMaxLength(50);
            entity.Property(e => e.PlayerSocksColor).HasMaxLength(50);
            entity.Property(e => e.PlayerStshirtColor)
                .HasMaxLength(50)
                .HasColumnName("PlayerSTshirtColor");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchUniforms)
                .HasForeignKey(d => d.MatchId)
                .HasConstraintName("FK_MatchUniform_Match");

            entity.HasOne(d => d.Team).WithMany(p => p.MatchUniforms)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_MatchUniform_Team");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.MatchUniforms)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchUniform_UserProfile");
        });

        modelBuilder.Entity<PendingAffiliationList>(entity =>
        {
            entity.ToTable("PendingAffiliationList");

            entity.Property(e => e.PendingAffiliationListId).HasColumnName("PendingAffiliationListID");
            entity.Property(e => e.CertificateImageUrl).HasMaxLength(200);
            entity.Property(e => e.Comment).HasMaxLength(500);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.PendingAffiliationStatusId).HasColumnName("PendingAffiliationStatusID");
            entity.Property(e => e.PendingAffiliationStatusReason).HasMaxLength(50);

            entity.HasOne(d => d.PendingAffiliationListType).WithMany(p => p.PendingAffiliationLists)
                .HasForeignKey(d => d.PendingAffiliationListTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PendingAffiliationList_PendingAffiliationListType");

            entity.HasOne(d => d.PendingAffiliationStatus).WithMany(p => p.PendingAffiliationLists)
                .HasForeignKey(d => d.PendingAffiliationStatusId)
                .HasConstraintName("FK_PendingAffiliationList_PendingAffiliationStatus");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.PendingAffiliationLists)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PendingAffiliationList_Userprofile");
        });

        modelBuilder.Entity<PendingAffiliationListPendingAffiliationListType>(entity =>
        {
            entity.HasKey(e => e.PendingAffiliationListTypeId);

            entity.ToTable("PendingAffiliationList.PendingAffiliationListType");

            entity.Property(e => e.PendingAffiliationListTypeId).ValueGeneratedOnAdd();
            entity.Property(e => e.PendingAffiliationListType).HasMaxLength(50);
        });

        modelBuilder.Entity<PendingStatus>(entity =>
        {
            entity.HasKey(e => e.PendingAffiliationStatusId).HasName("PK_PendingAffiliationStatus");

            entity.ToTable("PendingStatus");

            entity.Property(e => e.PendingAffiliationStatusId)
                .ValueGeneratedOnAdd()
                .HasColumnName("PendingAffiliationStatusID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.PendingAffiliationStatus).HasMaxLength(20);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PK_Players");

            entity.ToTable("Player");

            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.AlternativePlaceId).HasColumnName("AlternativePlaceID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.MainPlaceId).HasColumnName("MainPlaceID");
            entity.Property(e => e.PlayerFeetId).HasColumnName("PlayerFeetID");
            entity.Property(e => e.PlayerTypeId).HasColumnName("PlayerTypeID");
            entity.Property(e => e.SeasonId).HasColumnName("SeasonID");
            entity.Property(e => e.SportInstitutionBranchId).HasColumnName("SportInstitutionBranchID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.AlternativePlace).WithMany(p => p.PlayerAlternativePlaces)
                .HasForeignKey(d => d.AlternativePlaceId)
                .HasConstraintName("FK_Player_Player.PlayerPlace_Alterntive");

            entity.HasOne(d => d.MainPlace).WithMany(p => p.PlayerMainPlaces)
                .HasForeignKey(d => d.MainPlaceId)
                .HasConstraintName("FK_Player_Player.PlayerPlace_Main");

            entity.HasOne(d => d.PlayerFeet).WithMany(p => p.Players)
                .HasForeignKey(d => d.PlayerFeetId)
                .HasConstraintName("FK_Player_PlayerFeet");

            entity.HasOne(d => d.PlayerType).WithMany(p => p.Players)
                .HasForeignKey(d => d.PlayerTypeId)
                .HasConstraintName("FK_Player_Player.PlayerType");

            entity.HasOne(d => d.Season).WithMany(p => p.Players)
                .HasForeignKey(d => d.SeasonId)
                .HasConstraintName("FK_Player_Season");

            entity.HasOne(d => d.SportInstitutionBranch).WithMany(p => p.Players)
                .HasForeignKey(d => d.SportInstitutionBranchId)
                .HasConstraintName("FK_Player_SportInstitutionBranchID");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.Players)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Player_UserProfile");
        });

        modelBuilder.Entity<PlayerPlayerFoot>(entity =>
        {
            entity.HasKey(e => e.PlayerFeetId).HasName("PK_PlayerFeet");

            entity.ToTable("Player.PlayerFeet");

            entity.Property(e => e.PlayerFeetId)
                .ValueGeneratedOnAdd()
                .HasColumnName("PlayerFeetID");
            entity.Property(e => e.FootName).HasMaxLength(50);
        });

        modelBuilder.Entity<PlayerPlayerPlace>(entity =>
        {
            entity.HasKey(e => e.PlayerPlaceId).HasName("PK_PlayerStatisticsPlayerPlaces");

            entity.ToTable("Player.PlayerPlace");

            entity.Property(e => e.PlayerPlaceId)
                .ValueGeneratedOnAdd()
                .HasColumnName("PlayerPlaceID");
            entity.Property(e => e.PlayerPlace).HasMaxLength(50);
        });

        modelBuilder.Entity<PlayerPlayerType>(entity =>
        {
            entity.HasKey(e => e.PlayerTypeId);

            entity.ToTable("Player.PlayerType");

            entity.Property(e => e.PlayerTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("PlayerTypeID");
            entity.Property(e => e.PlayerType).HasMaxLength(50);
        });

        modelBuilder.Entity<Playground>(entity =>
        {
            entity.ToTable("Playground");

            entity.Property(e => e.PlaygroundId).HasColumnName("PlaygroundID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ImagePath).HasMaxLength(200);
            entity.Property(e => e.PlaygroundFloorId).HasColumnName("PlaygroundFloorID");
            entity.Property(e => e.PlaygroundLocation).HasMaxLength(100);
            entity.Property(e => e.PlaygroundName).HasMaxLength(50);
            entity.Property(e => e.PlaygroundSizeId).HasColumnName("PlaygroundSizeID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.City).WithMany(p => p.Playgrounds)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Playground_PlaygroundCity");

            entity.HasOne(d => d.PlaygroundFloor).WithMany(p => p.Playgrounds)
                .HasForeignKey(d => d.PlaygroundFloorId)
                .HasConstraintName("FK_Playground_Floor");

            entity.HasOne(d => d.PlaygroundSize).WithMany(p => p.Playgrounds)
                .HasForeignKey(d => d.PlaygroundSizeId)
                .HasConstraintName("FK_Playground_PlaygroundSize");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.Playgrounds)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .HasConstraintName("FK_Playground_UserProfileID");
        });

        modelBuilder.Entity<RealTimeNotification>(entity =>
        {
            entity.HasKey(e => e.RealTimeNotificationId).HasName("PK_RealTimeNotificationn");

            entity.ToTable("RealTimeNotification");

            entity.Property(e => e.RealTimeNotificationId).HasColumnName("RealTimeNotificationID");
            entity.Property(e => e.ContentMessage).HasMaxLength(200);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.InvitationId).HasColumnName("InvitationID");
            entity.Property(e => e.TargetId).HasColumnName("TargetID");

            entity.HasOne(d => d.FromUserProfile).WithMany(p => p.RealTimeNotificationFromUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.FromUserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RealTimeNotification_FromUserProfil");

            entity.HasOne(d => d.TargetType).WithMany(p => p.RealTimeNotifications)
                .HasForeignKey(d => d.TargetTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RealTimeNotification_TargetType");

            entity.HasOne(d => d.ToUserProfile).WithMany(p => p.RealTimeNotificationToUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.ToUserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RealTimeNotification_ToUserProfile");
        });

        modelBuilder.Entity<RealTimeNotificationTargetType>(entity =>
        {
            entity.ToTable("RealTimeNotificationTargetType");

            entity.Property(e => e.RealTimeNotificationTargetTypeId).ValueGeneratedOnAdd();
            entity.Property(e => e.RealTimeNotificationTargetType1)
                .HasMaxLength(50)
                .HasColumnName("RealTimeNotificationTargetType");
        });

        modelBuilder.Entity<Referee>(entity =>
        {
            entity.ToTable("Referee");

            entity.Property(e => e.RefereeId).HasColumnName("RefereeID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.RefereeTypeId).HasColumnName("RefereeTypeID");
            entity.Property(e => e.SeasonId).HasColumnName("SeasonID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.RefereeType).WithMany(p => p.Referees)
                .HasForeignKey(d => d.RefereeTypeId)
                .HasConstraintName("FK_Referee_Referee.RefereeType");

            entity.HasOne(d => d.Season).WithMany(p => p.Referees)
                .HasForeignKey(d => d.SeasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Referee_Season");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.Referees)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Referee_UserProfile");
        });

        modelBuilder.Entity<RefereeRefereeType>(entity =>
        {
            entity.HasKey(e => e.RefereeTypeId);

            entity.ToTable("Referee.RefereeType");

            entity.Property(e => e.RefereeTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("RefereeTypeID");
            entity.Property(e => e.RefereeType).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK_User.Role");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedOnAdd()
                .HasColumnName("RoleID");
            entity.Property(e => e.Role1)
                .HasMaxLength(50)
                .HasColumnName("Role");
        });

        modelBuilder.Entity<SearchType>(entity =>
        {
            entity.ToTable("SearchType");

            entity.Property(e => e.SearchTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("SearchTypeID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.SearchKay).HasMaxLength(20);
        });

        modelBuilder.Entity<Season>(entity =>
        {
            entity.HasKey(e => e.SeasonId).HasName("PK_Seasons");

            entity.ToTable("Season");

            entity.Property(e => e.SeasonId)
                .ValueGeneratedOnAdd()
                .HasColumnName("SeasonID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EndDate).HasColumnType("smalldatetime");
            entity.Property(e => e.SeasonName).HasMaxLength(100);
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Sponsor>(entity =>
        {
            entity.ToTable("Sponsor");

            entity.Property(e => e.SponsorId).HasColumnName("SponsorID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.SponsorLogoId).HasColumnName("SponsorLogoID");
            entity.Property(e => e.SponsorName).HasMaxLength(200);
            entity.Property(e => e.SponsorTypeId).HasColumnName("SponsorTypeID");
            entity.Property(e => e.SponsorUserProfileId).HasColumnName("SponsorUserProfileID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.SponsorLogo).WithMany(p => p.Sponsors)
                .HasForeignKey(d => d.SponsorLogoId)
                .HasConstraintName("FK_Sponsor_Image");

            entity.HasOne(d => d.SponsorType).WithMany(p => p.Sponsors)
                .HasForeignKey(d => d.SponsorTypeId)
                .HasConstraintName("FK_Sponsor_Sponsor.SponsorType");

            entity.HasOne(d => d.SponsorUserProfile).WithMany(p => p.SponsorSponsorUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.SponsorUserProfileId)
                .HasConstraintName("FK_Sponsor_UserProfile1");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.SponsorUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sponsor_UserProfile");
        });

        modelBuilder.Entity<SponsorSponsorType>(entity =>
        {
            entity.HasKey(e => e.SponsorTypeId);

            entity.ToTable("Sponsor.SponsorType");

            entity.Property(e => e.SponsorTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("SponsorTypeID");
            entity.Property(e => e.SponsorType).HasMaxLength(50);
        });

        modelBuilder.Entity<SportInstitution>(entity =>
        {
            entity.HasKey(e => e.SportInstitutionId).HasName("PK_Academies");

            entity.ToTable("SportInstitution");

            entity.Property(e => e.SportInstitutionId).HasColumnName("SportInstitutionID");
            entity.Property(e => e.BackGroundFileName)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.BackGroundUrlfullPath)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.DateCreated).HasColumnType("smalldatetime");
            entity.Property(e => e.FounderName).HasMaxLength(100);
            entity.Property(e => e.Gmail).HasMaxLength(50);
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");
            entity.Property(e => e.LicenseNumber).HasMaxLength(100);
            entity.Property(e => e.LogoFileName)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.LogoUrlfullPath)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.SportInstitutionName).HasMaxLength(200);
            entity.Property(e => e.SportInstitutionTypeId).HasColumnName("SportInstitutionTypeID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.SportInstitutionType).WithMany(p => p.SportInstitutions)
                .HasForeignKey(d => d.SportInstitutionTypeId)
                .HasConstraintName("FK_SportInstitution_SportInstitutionType");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.SportInstitutions)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SportInstitution_UserProfile");
        });

        modelBuilder.Entity<SportInstitutionAgeCategoryBelong>(entity =>
        {
            entity.HasKey(e => e.AgeCategoryBelongId).HasName("PK_AgeCategoryBelongID");

            entity.ToTable("SportInstitutionAgeCategoryBelong");

            entity.Property(e => e.AgeCategoryBelongId).HasColumnName("AgeCategoryBelongID");
            entity.Property(e => e.AgeCategoryId).HasColumnName("AgeCategoryID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.SportInstitutionBelongId).HasColumnName("SportInstitutionBelongID");

            entity.HasOne(d => d.AgeCategory).WithMany(p => p.SportInstitutionAgeCategoryBelongs)
                .HasForeignKey(d => d.AgeCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgeCategoryBelongID_SportInstitutionAgePrice.AgeCategory");

            entity.HasOne(d => d.SportInstitutionBelong).WithMany(p => p.SportInstitutionAgeCategoryBelongs)
                .HasForeignKey(d => d.SportInstitutionBelongId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgeCategoryBelongID_SportInstitutionBelong");
        });

        modelBuilder.Entity<SportInstitutionAgePrice>(entity =>
        {
            entity.ToTable("SportInstitutionAgePrice");

            entity.Property(e => e.SportInstitutionAgePriceId).HasColumnName("SportInstitutionAgePriceID");
            entity.Property(e => e.AgeCategoryId).HasColumnName("AgeCategoryID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");
            entity.Property(e => e.SportInstitutionBranchId).HasColumnName("SportInstitutionBranchID");
            entity.Property(e => e.SportInstitutionTypeId).HasColumnName("SportInstitutionTypeID");
            entity.Property(e => e.SubscriptionPeriodId).HasColumnName("SubscriptionPeriodID");

            entity.HasOne(d => d.AgeCategory).WithMany(p => p.SportInstitutionAgePrices)
                .HasForeignKey(d => d.AgeCategoryId)
                .HasConstraintName("FK_SportInstitutionAgePrice_AgeCat");

            entity.HasOne(d => d.SportInstitutionBranch).WithMany(p => p.SportInstitutionAgePrices)
                .HasForeignKey(d => d.SportInstitutionBranchId)
                .HasConstraintName("FK_SportInstitutionAgePrice_Branch");

            entity.HasOne(d => d.SportInstitutionType).WithMany(p => p.SportInstitutionAgePrices)
                .HasForeignKey(d => d.SportInstitutionTypeId)
                .HasConstraintName("FK_SportInstitutionAgePrice_SportInstitutionType");

            entity.HasOne(d => d.SubscriptionPeriod).WithMany(p => p.SportInstitutionAgePrices)
                .HasForeignKey(d => d.SubscriptionPeriodId)
                .HasConstraintName("FK_SportInstitutionAgePrice_SubsciptionPeriod");
        });

        modelBuilder.Entity<SportInstitutionAgePriceAgeCategory>(entity =>
        {
            entity.HasKey(e => e.AgeCategoryId).HasName("PK_AgeCategory");

            entity.ToTable("SportInstitutionAgePrice.AgeCategory");

            entity.Property(e => e.AgeCategoryId)
                .ValueGeneratedOnAdd()
                .HasColumnName("AgeCategoryID");
            entity.Property(e => e.AgeCategory).HasMaxLength(50);
            entity.Property(e => e.SportInstitutionTypeId).HasColumnName("SportInstitutionTypeID");

            entity.HasOne(d => d.SportInstitutionType).WithMany(p => p.SportInstitutionAgePriceAgeCategories)
                .HasForeignKey(d => d.SportInstitutionTypeId)
                .HasConstraintName("FK_SportInstitutionAgePrice.AgeCategory_SportInstitutionType");
        });

        modelBuilder.Entity<SportInstitutionAgePriceSubscriptionPeriod>(entity =>
        {
            entity.HasKey(e => e.SubscriptionPeriodId).HasName("PK_SubscriptionPeriod");

            entity.ToTable("SportInstitutionAgePrice.SubscriptionPeriod");

            entity.Property(e => e.SubscriptionPeriodId)
                .ValueGeneratedOnAdd()
                .HasColumnName("SubscriptionPeriodID");
            entity.Property(e => e.SubscriptionPeriod).HasMaxLength(50);
        });

        modelBuilder.Entity<SportInstitutionBelong>(entity =>
        {
            entity.ToTable("SportInstitutionBelong");

            entity.Property(e => e.SportInstitutionBelongId).HasColumnName("SportInstitutionBelongID");
            entity.Property(e => e.BelongTypeId).HasColumnName("BelongTypeID");
            entity.Property(e => e.BelongUserProfileId).HasColumnName("BelongUserProfileID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");
            entity.Property(e => e.SeasonId).HasColumnName("SeasonID");
            entity.Property(e => e.ShortDescription).HasMaxLength(200);
            entity.Property(e => e.SportInstitutionBranchId).HasColumnName("SportInstitutionBranchID");

            entity.HasOne(d => d.BelongUserProfile).WithMany(p => p.SportInstitutionBelongs)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.BelongUserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SportInstitutionBelong_Userprofile");

            entity.HasOne(d => d.Season).WithMany(p => p.SportInstitutionBelongs)
                .HasForeignKey(d => d.SeasonId)
                .HasConstraintName("FK_SportInstitutionBelong_Season");

            entity.HasOne(d => d.SportInstitutionBranch).WithMany(p => p.SportInstitutionBelongs)
                .HasForeignKey(d => d.SportInstitutionBranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SportInstitutionBelong_Branch");
        });

        modelBuilder.Entity<SportInstitutionBelongBelongType>(entity =>
        {
            entity.HasKey(e => e.BelongTypeId);

            entity.ToTable("SportInstitutionBelong.BelongType");

            entity.Property(e => e.BelongTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("BelongTypeID");
            entity.Property(e => e.BelongType).HasMaxLength(50);
        });

        modelBuilder.Entity<SportInstitutionBelongPending>(entity =>
        {
            entity.ToTable("SportInstitutionBelongPending");

            entity.Property(e => e.SportInstitutionBelongPendingId).HasColumnName("SportInstitutionBelongPendingID");
            entity.Property(e => e.BelongTypeId).HasColumnName("BelongTypeID");
            entity.Property(e => e.BelongUserProfileId).HasColumnName("BelongUserProfileID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.SeasonId).HasColumnName("SeasonID");
            entity.Property(e => e.ShortDescription).HasMaxLength(200);
            entity.Property(e => e.SportInstitutionBranchId).HasColumnName("SportInstitutionBranchID");

            entity.HasOne(d => d.BelongType).WithMany(p => p.SportInstitutionBelongPendings)
                .HasForeignKey(d => d.BelongTypeId)
                .HasConstraintName("FK_SportInstitutionBelongPending_SportInstitutionBelong.BelongType");

            entity.HasOne(d => d.BelongUserProfile).WithMany(p => p.SportInstitutionBelongPendings)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.BelongUserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SportInstitutionBelongPending_UserProfile");

            entity.HasOne(d => d.Season).WithMany(p => p.SportInstitutionBelongPendings)
                .HasForeignKey(d => d.SeasonId)
                .HasConstraintName("FK_SportInstitutionBelongPending_Season");
        });

        modelBuilder.Entity<SportInstitutionBelongPendingAgeCategory>(entity =>
        {
            entity.ToTable("SportInstitutionBelongPending.AgeCategories");

            entity.Property(e => e.SportInstitutionBelongPendingAgeCategoryId).HasColumnName("SportInstitutionBelongPendingAgeCategoryID");
            entity.Property(e => e.AgeCategoryId).HasColumnName("AgeCategoryID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.SportInstitutionBelongPendingId).HasColumnName("SportInstitutionBelongPendingID");

            entity.HasOne(d => d.AgeCategory).WithMany(p => p.SportInstitutionBelongPendingAgeCategories)
                .HasForeignKey(d => d.AgeCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SportInstitutionBelongPending.AgeCategories_SportInstitutionBelongPendingAgeCategories-AgeCategory");

            entity.HasOne(d => d.SportInstitutionBelongPending).WithMany(p => p.SportInstitutionBelongPendingAgeCategories)
                .HasForeignKey(d => d.SportInstitutionBelongPendingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SportInstitutionBelongPendingAgeCategories_SportInstitutionBelongPendingAgeCategories-SportInstitutionBelongPending");
        });

        modelBuilder.Entity<SportInstitutionBranch>(entity =>
        {
            entity.ToTable("SportInstitutionBranch");

            entity.Property(e => e.SportInstitutionBranchId).HasColumnName("SportInstitutionBranchID");
            entity.Property(e => e.BranchPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CityDistricts).HasMaxLength(50);
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.DateCreated).HasColumnType("smalldatetime");
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");
            entity.Property(e => e.Location).HasMaxLength(500);
            entity.Property(e => e.SportInstitutionBranchName).HasMaxLength(50);
            entity.Property(e => e.SportInstitutionBranchTypeId).HasColumnName("SportInstitutionBranchTypeID");
            entity.Property(e => e.SportInstitutionId).HasColumnName("SportInstitutionID");

            entity.HasOne(d => d.City).WithMany(p => p.SportInstitutionBranches)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_SportInstitutionBranch_City");

            entity.HasOne(d => d.SportInstitutionBranchType).WithMany(p => p.SportInstitutionBranches)
                .HasForeignKey(d => d.SportInstitutionBranchTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SportInstitutionBranch_SportInstitutionBranchType");

            entity.HasOne(d => d.SportInstitution).WithMany(p => p.SportInstitutionBranches)
                .HasForeignKey(d => d.SportInstitutionId)
                .HasConstraintName("FK_SportInstitution_SportInstitution");
        });

        modelBuilder.Entity<SportInstitutionBranchTypeSportInstitutionBranchType>(entity =>
        {
            entity.HasKey(e => e.SportInstitutionBranchTypeId);

            entity.ToTable("SportInstitutionBranchType.SportInstitutionBranchType");

            entity.Property(e => e.SportInstitutionBranchTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("SportInstitutionBranchTypeID");
            entity.Property(e => e.SportInstitutionBranchType).HasMaxLength(50);
        });

        modelBuilder.Entity<SportInstitutionEmployeeJobTypeXxx>(entity =>
        {
            entity.HasKey(e => e.JobTypeId).HasName("PK_JobType");

            entity.ToTable("SportInstitutionEmployee.JobTypeXXX");

            entity.Property(e => e.JobTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("JobTypeID");
            entity.Property(e => e.JobType).HasMaxLength(50);
        });

        modelBuilder.Entity<SportInstitutionEmployeeXxx>(entity =>
        {
            entity.HasKey(e => e.SportInstitutionEmployeeId).HasName("PK_SportInstitutionEmployee");

            entity.ToTable("SportInstitutionEmployeeXXX");

            entity.Property(e => e.SportInstitutionEmployeeId).HasColumnName("SportInstitutionEmployeeID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EmployeeFullName).HasMaxLength(200);
            entity.Property(e => e.EmployeeemPhotoFileName)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeemPhotoUrlfullPath)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.JobPosition).HasMaxLength(200);
            entity.Property(e => e.JobTypeId).HasColumnName("JobTypeID");
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");
            entity.Property(e => e.SportInstitutionBranchId).HasColumnName("SportInstitutionBranchID");

            entity.HasOne(d => d.JobType).WithMany(p => p.SportInstitutionEmployeeXxxes)
                .HasForeignKey(d => d.JobTypeId)
                .HasConstraintName("FK_SportInstitutionEmployee_JobType");

            entity.HasOne(d => d.SportInstitutionBranch).WithMany(p => p.SportInstitutionEmployeeXxxes)
                .HasForeignKey(d => d.SportInstitutionBranchId)
                .HasConstraintName("FK_SportInstitutionEmployee_SportInstitutionBranch");
        });

        modelBuilder.Entity<SportInstitutionSportInstitutionType>(entity =>
        {
            entity.HasKey(e => e.SportInstitutionTypeId);

            entity.ToTable("SportInstitution.SportInstitutionType");

            entity.Property(e => e.SportInstitutionTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("SportInstitutionTypeID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ImagePath).HasMaxLength(100);
            entity.Property(e => e.SportInstitutionType).HasMaxLength(50);
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("PK_Subscriptions");

            entity.ToTable("Subscription");

            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.SourceUserProfileId).HasColumnName("SourceUserProfileID");
            entity.Property(e => e.TargetUserProfileId).HasColumnName("TargetUserProfileID");

            entity.HasOne(d => d.SourceUserProfile).WithMany(p => p.SubscriptionSourceUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.SourceUserProfileId)
                .HasConstraintName("FK_Subscription_Subscription_Source");

            entity.HasOne(d => d.TargetUserProfile).WithMany(p => p.SubscriptionTargetUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.TargetUserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subscription_UserProfile_Target");
        });

        modelBuilder.Entity<Training>(entity =>
        {
            entity.Property(e => e.TrainingId).HasColumnName("TrainingID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EndDate).HasColumnType("smalldatetime");
            entity.Property(e => e.SeasonId).HasColumnName("SeasonID");
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
            entity.Property(e => e.TrainingImageId).HasColumnName("TrainingImageID");
            entity.Property(e => e.TrainingName).HasMaxLength(200);
            entity.Property(e => e.TrainingTypeId).HasColumnName("TrainingTypeID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Season).WithMany(p => p.Training)
                .HasForeignKey(d => d.SeasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Training_Season");

            entity.HasOne(d => d.TrainingImage).WithMany(p => p.Training)
                .HasForeignKey(d => d.TrainingImageId)
                .HasConstraintName("FK_Training_rainingImage");

            entity.HasOne(d => d.TrainingType).WithMany(p => p.Training)
                .HasForeignKey(d => d.TrainingTypeId)
                .HasConstraintName("FK_Training_Training.TrainingType");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.Training)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Training_UserProfile");
        });

        modelBuilder.Entity<TrainingAdmin>(entity =>
        {
            entity.ToTable("TrainingAdmin");

            entity.Property(e => e.TrainingAdminId).HasColumnName("TrainingAdminID");
            entity.Property(e => e.AdminUserProfileId).HasColumnName("AdminUserProfileID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");
            entity.Property(e => e.TrainingId).HasColumnName("TrainingID");

            entity.HasOne(d => d.AdminUserProfile).WithMany(p => p.TrainingAdmins)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.AdminUserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingAdmin_UserProfile");

            entity.HasOne(d => d.Training).WithMany(p => p.TrainingAdmins)
                .HasForeignKey(d => d.TrainingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingAdmin_Training");
        });

        modelBuilder.Entity<TrainingAttendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK_TrainingPreparation");

            entity.ToTable("TrainingAttendance");

            entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");
            entity.Property(e => e.AttendanceTypeId).HasColumnName("AttendanceTypeID");
            entity.Property(e => e.AttendanceUserProfileId).HasColumnName("AttendanceUserProfileID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.TrainingDetailsId).HasColumnName("TrainingDetailsID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.AttendanceUserProfile).WithMany(p => p.TrainingAttendanceAttendanceUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.AttendanceUserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingAttendance_UserProfile1");

            entity.HasOne(d => d.TrainingDetails).WithMany(p => p.TrainingAttendances)
                .HasForeignKey(d => d.TrainingDetailsId)
                .HasConstraintName("FK_TrainingPreparation_TrainingDetails");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.TrainingAttendanceUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingAttendance_UserProfile");
        });

        modelBuilder.Entity<TrainingBranch>(entity =>
        {
            entity.HasKey(e => e.TrainingBranchId).HasName("PK_TrainingCoachAndBranch");

            entity.ToTable("TrainingBranch");

            entity.Property(e => e.TrainingBranchId).HasColumnName("TrainingBranchID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.SportInstitutionBranchId).HasColumnName("SportInstitutionBranchID");
            entity.Property(e => e.TrainingId).HasColumnName("TrainingID");

            entity.HasOne(d => d.SportInstitutionBranch).WithMany(p => p.TrainingBranches)
                .HasForeignKey(d => d.SportInstitutionBranchId)
                .HasConstraintName("FK_TrainingCoachAndBranch_SportInstitutionBranch");

            entity.HasOne(d => d.Training).WithMany(p => p.TrainingBranches)
                .HasForeignKey(d => d.TrainingId)
                .HasConstraintName("FK_TrainingCoachAndBranch_Training");
        });

        modelBuilder.Entity<TrainingDetail>(entity =>
        {
            entity.HasKey(e => e.TrainingDetailsId);

            entity.Property(e => e.TrainingDetailsId).HasColumnName("TrainingDetailsID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.PlaygroundFloorId).HasColumnName("PlaygroundFloorID");
            entity.Property(e => e.PlaygroundSizeId).HasColumnName("PlaygroundSizeID");
            entity.Property(e => e.TrainingCost).HasMaxLength(100);
            entity.Property(e => e.TrainingDate).HasColumnType("smalldatetime");
            entity.Property(e => e.TrainingId).HasColumnName("TrainingID");

            entity.HasOne(d => d.PlaygroundFloor).WithMany(p => p.TrainingDetails)
                .HasForeignKey(d => d.PlaygroundFloorId)
                .HasConstraintName("FK_TrainingDetails_TrainingDetails.PlaygroundFloor");

            entity.HasOne(d => d.PlaygroundSize).WithMany(p => p.TrainingDetails)
                .HasForeignKey(d => d.PlaygroundSizeId)
                .HasConstraintName("FK_TrainingDetails_TrainingDetails.PlaygroundSize");

            entity.HasOne(d => d.Training).WithMany(p => p.TrainingDetails)
                .HasForeignKey(d => d.TrainingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingDetails_Training");
        });

        modelBuilder.Entity<TrainingDetailsPlaygroundFloor>(entity =>
        {
            entity.HasKey(e => e.PlaygroundFloorId).HasName("PK_Trining.PlaygroundFloor");

            entity.ToTable("TrainingDetails.PlaygroundFloor");

            entity.Property(e => e.PlaygroundFloorId)
                .ValueGeneratedOnAdd()
                .HasColumnName("PlaygroundFloorID");
            entity.Property(e => e.PlaygroundFloor).HasMaxLength(50);
        });

        modelBuilder.Entity<TrainingDetailsPlaygroundSize>(entity =>
        {
            entity.HasKey(e => e.PlaygroundSizeId).HasName("PK_Training.PlaygroundSize");

            entity.ToTable("TrainingDetails.PlaygroundSize");

            entity.Property(e => e.PlaygroundSizeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("PlaygroundSizeID");
            entity.Property(e => e.PlaygroundSize).HasMaxLength(50);
        });

        modelBuilder.Entity<TrainingEvaluation>(entity =>
        {
            entity.HasKey(e => e.EvaluationId);

            entity.ToTable("TrainingEvaluation");

            entity.Property(e => e.EvaluationId).HasColumnName("EvaluationID");
            entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EvaluationComment).HasMaxLength(200);
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Attendance).WithMany(p => p.TrainingEvaluations)
                .HasForeignKey(d => d.AttendanceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingEvaluation_TrainingAttendance");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.TrainingEvaluations)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingEvaluation_UserProfile");
        });

        modelBuilder.Entity<TrainingInjury>(entity =>
        {
            entity.HasKey(e => e.InjuryId);

            entity.ToTable("TrainingInjury");

            entity.Property(e => e.InjuryId).HasColumnName("InjuryID");
            entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.InjuryComment).HasMaxLength(500);
            entity.Property(e => e.InjuryPositionId).HasColumnName("InjuryPositionID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Attendance).WithMany(p => p.TrainingInjuries)
                .HasForeignKey(d => d.AttendanceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingInjury_TrainingAttendance");

            entity.HasOne(d => d.InjuryPosition).WithMany(p => p.TrainingInjuries)
                .HasForeignKey(d => d.InjuryPositionId)
                .HasConstraintName("FK_TrainingInjury_InjuryPostion");

            entity.HasOne(d => d.TrainingDetails).WithMany(p => p.TrainingInjuries)
                .HasForeignKey(d => d.TrainingDetailsId)
                .HasConstraintName("FK_TrainingInjury_TrainingDetails");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.TrainingInjuries)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingInjury_UserProfile");
        });

        modelBuilder.Entity<TrainingInjuryInjuryPosition>(entity =>
        {
            entity.HasKey(e => e.InjuryPositionId);

            entity.ToTable("TrainingInjury.InjuryPosition");

            entity.Property(e => e.InjuryPositionId)
                .ValueGeneratedOnAdd()
                .HasColumnName("InjuryPositionID");
            entity.Property(e => e.InjuryPositionInArabic).HasMaxLength(50);
            entity.Property(e => e.InjuryPositionInEnglish)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TrainingInvitationPending>(entity =>
        {
            entity.ToTable("TrainingInvitationPending");

            entity.Property(e => e.TrainingInvitationPendingId).HasColumnName("TrainingInvitationPendingID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.IsInvited).HasDefaultValueSql("((0))");
            entity.Property(e => e.PendingUserProfileId).HasColumnName("PendingUserProfileID");
            entity.Property(e => e.TrainingId).HasColumnName("TrainingID");
            entity.Property(e => e.TrainingOwnerUserProfileId).HasColumnName("TrainingOwnerUserProfileID");

            entity.HasOne(d => d.PendingUserProfile).WithMany(p => p.TrainingInvitationPendingPendingUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.PendingUserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingInvitationPending_PendingUserProfileID");

            entity.HasOne(d => d.Training).WithMany(p => p.TrainingInvitationPendings)
                .HasForeignKey(d => d.TrainingId)
                .HasConstraintName("FK_TrainingInvitationPending_Training");

            entity.HasOne(d => d.TrainingOwnerUserProfile).WithMany(p => p.TrainingInvitationPendingTrainingOwnerUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.TrainingOwnerUserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingInvitationPending_OwnerUserProfileID");
        });

        modelBuilder.Entity<TrainingPlayerXxx>(entity =>
        {
            entity.HasKey(e => e.TrainingPlayerId).HasName("PK_TrainingPlayer");

            entity.ToTable("TrainingPlayerXXX");

            entity.Property(e => e.TrainingPlayerId).HasColumnName("TrainingPlayerID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.PlayerUserProfileId).HasColumnName("PlayerUserProfileID");
            entity.Property(e => e.TrainingDetailsId).HasColumnName("TrainingDetailsID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.PlayerUserProfile).WithMany(p => p.TrainingPlayerXxxPlayerUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.PlayerUserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingPlayer_UserProfile1");

            entity.HasOne(d => d.TrainingDetails).WithMany(p => p.TrainingPlayerXxxes)
                .HasForeignKey(d => d.TrainingDetailsId)
                .HasConstraintName("FK_TrainingPlayer_TrainingDetails");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.TrainingPlayerXxxUserProfiles)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingPlayer_UserProfile");
        });

        modelBuilder.Entity<TrainingSponsor>(entity =>
        {
            entity.ToTable("TrainingSponsor");

            entity.Property(e => e.TrainingSponsorId).HasColumnName("TrainingSponsorID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.SponsorId).HasColumnName("SponsorID");
            entity.Property(e => e.TrainingId).HasColumnName("TrainingID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Sponsor).WithMany(p => p.TrainingSponsors)
                .HasForeignKey(d => d.SponsorId)
                .HasConstraintName("FK_TrainingSponsor_Sponsor");

            entity.HasOne(d => d.Training).WithMany(p => p.TrainingSponsors)
                .HasForeignKey(d => d.TrainingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingSponsor_Training");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.TrainingSponsors)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingSponsor_UserProfile");
        });

        modelBuilder.Entity<TrainingTeam>(entity =>
        {
            entity.ToTable("TrainingTeam");

            entity.Property(e => e.TrainingTeamId).HasColumnName("TrainingTeamID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.TrainingDetailsId).HasColumnName("TrainingDetailsID");
            entity.Property(e => e.TrainingTeamName).HasMaxLength(50);
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.TrainingDetails).WithMany(p => p.TrainingTeams)
                .HasForeignKey(d => d.TrainingDetailsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingTeam_TrainingDetails");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.TrainingTeams)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingTeam_UserProfile");
        });

        modelBuilder.Entity<TrainingTeamPlayer>(entity =>
        {
            entity.ToTable("TrainingTeamPlayer");

            entity.Property(e => e.TrainingTeamPlayerId).HasColumnName("TrainingTeamPlayerID");
            entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.TrainingTeamId).HasColumnName("TrainingTeamID");

            entity.HasOne(d => d.Attendance).WithMany(p => p.TrainingTeamPlayers)
                .HasForeignKey(d => d.AttendanceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingTeamPlayer_TrainingAttendance");

            entity.HasOne(d => d.TrainingTeam).WithMany(p => p.TrainingTeamPlayers)
                .HasForeignKey(d => d.TrainingTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TrainingTeamPlayer_TrainingTeam");
        });

        modelBuilder.Entity<TrainingTrainingType>(entity =>
        {
            entity.HasKey(e => e.TrainingTypeId);

            entity.ToTable("Training.TrainingType");

            entity.Property(e => e.TrainingTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("TrainingTypeID");
            entity.Property(e => e.TrainingType).HasMaxLength(50);
        });

        modelBuilder.Entity<Uniform>(entity =>
        {
            entity.ToTable("Uniform");

            entity.Property(e => e.UniformId).HasColumnName("UniformID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.UniformImageId).HasColumnName("UniformImageID");
            entity.Property(e => e.UniformTypeId).HasColumnName("UniformTypeID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Team).WithMany(p => p.Uniforms)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_Uniform_SportInstitution");

            entity.HasOne(d => d.UniformImage).WithMany(p => p.Uniforms)
                .HasForeignKey(d => d.UniformImageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Uniform_Image");

            entity.HasOne(d => d.UniformType).WithMany(p => p.Uniforms)
                .HasForeignKey(d => d.UniformTypeId)
                .HasConstraintName("FK_Uniform_Uniform.UniformType");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.Uniforms)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Uniform_UserProfile");
        });

        modelBuilder.Entity<UniformUniformType>(entity =>
        {
            entity.HasKey(e => e.UniformTypeId);

            entity.ToTable("Uniform.UniformType");

            entity.Property(e => e.UniformTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("UniformTypeID");
            entity.Property(e => e.UniformType).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_UserInfos");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CityCountryId).HasColumnName("CityCountryID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.LastLoginDate).HasColumnType("smalldatetime");
            entity.Property(e => e.LastLoginRoleId).HasColumnName("LastLoginRoleID");
            entity.Property(e => e.LastSentVerifiedCodeTime).HasColumnType("smalldatetime");
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");
            entity.Property(e => e.Mobile).HasMaxLength(20).IsUnicode(false);

            entity.Property(e => e.ParentChild).HasMaxLength(20).IsUnicode(false);

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Password)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UserCreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.UserStatusId).HasColumnName("UserStatusID");

            entity.HasOne(d => d.CityCountry).WithMany(p => p.Users)
                .HasForeignKey(d => d.CityCountryId)
                .HasConstraintName("FK_User_CityCountry");

            entity.HasOne(d => d.City).WithMany(p => p.Users)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_User_User_City");

            entity.HasOne(d => d.LastLoginRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.LastLoginRoleId)
                .HasConstraintName("FK_User_UserLastRole");

            entity.HasOne(d => d.UserStatus).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserStatusId)
                .HasConstraintName("FK_User_Status");
        });

        modelBuilder.Entity<UserBasicDataQualificationType>(entity =>
        {
            entity.HasKey(e => e.QualificationTypeId);

            entity.ToTable("UserBasicData.QualificationType");

            entity.Property(e => e.QualificationTypeId).HasColumnName("QualificationTypeID");
            entity.Property(e => e.QualificationType).HasMaxLength(50);
        });

        modelBuilder.Entity<UserBasicDatum>(entity =>
        {
            entity.HasKey(e => e.UserBasicDataId).HasName("PK_BasicData");

            entity.Property(e => e.UserBasicDataId).HasColumnName("UserBasicDataID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.Dob)
                .HasColumnType("smalldatetime")
                .HasColumnName("DOB");
            entity.Property(e => e.FamilyName).HasMaxLength(20);
            entity.Property(e => e.FatherName).HasMaxLength(20);
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.GrandFatherName).HasMaxLength(20);
            entity.Property(e => e.Idnumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IDNumber");
            entity.Property(e => e.InternationalFavoriteClubId).HasColumnName("InternationalFavoriteClubID");
            entity.Property(e => e.InternationalFavoritePlayer).HasMaxLength(50);
            entity.Property(e => e.InternationalFavoriteTeamId).HasColumnName("InternationalFavoriteTeamID");
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");
            entity.Property(e => e.LocalFavoriteClubId).HasColumnName("LocalFavoriteClubID");
            entity.Property(e => e.LocalFavoritePlayer).HasMaxLength(50);
            entity.Property(e => e.NationalityId).HasColumnName("NationalityID");
            entity.Property(e => e.PlaceOfResidence).HasMaxLength(100);
            entity.Property(e => e.PlayerFeetId).HasColumnName("PlayerFeetID");
            entity.Property(e => e.PlayerMainPlaceId).HasColumnName("PlayerMainPlaceID");
            entity.Property(e => e.PlayerSecondaryPlaceId).HasColumnName("PlayerSecondaryPlaceID");
            entity.Property(e => e.Pob)
                .HasMaxLength(100)
                .HasColumnName("POB");
            entity.Property(e => e.QualificationTypeId)
                .HasDefaultValueSql("((255))")
                .HasColumnName("QualificationTypeID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.InternationalFavoriteClub).WithMany(p => p.UserBasicDatumInternationalFavoriteClubs)
                .HasForeignKey(d => d.InternationalFavoriteClubId)
                .HasConstraintName("FK_UserBasicData_InternationalClub");

            entity.HasOne(d => d.InternationalFavoriteTeam).WithMany(p => p.UserBasicDatumInternationalFavoriteTeams)
                .HasForeignKey(d => d.InternationalFavoriteTeamId)
                .HasConstraintName("FK_UserBasicData_InternationalTeam");

            entity.HasOne(d => d.LocalFavoriteClub).WithMany(p => p.UserBasicDatumLocalFavoriteClubs)
                .HasForeignKey(d => d.LocalFavoriteClubId)
                .HasConstraintName("FK_UserBasicData_LocalClub");

            entity.HasOne(d => d.Nationality).WithMany(p => p.UserBasicDatumNationalities)
                .HasForeignKey(d => d.NationalityId)
                .HasConstraintName("FK_UserBasicData_Nationlaty");

            entity.HasOne(d => d.PlayerFeet).WithMany(p => p.UserBasicData)
                .HasForeignKey(d => d.PlayerFeetId)
                .HasConstraintName("FK_UserBasicData_Feet");

            entity.HasOne(d => d.PlayerMainPlace).WithMany(p => p.UserBasicDatumPlayerMainPlaces)
                .HasForeignKey(d => d.PlayerMainPlaceId)
                .HasConstraintName("FK_UserBasicData_MainPlace");

            entity.HasOne(d => d.PlayerSecondaryPlace).WithMany(p => p.UserBasicDatumPlayerSecondaryPlaces)
                .HasForeignKey(d => d.PlayerSecondaryPlaceId)
                .HasConstraintName("FK_UserBasicData_SecondaryPlace");

            entity.HasOne(d => d.QualificationType).WithMany(p => p.UserBasicData)
                .HasForeignKey(d => d.QualificationTypeId)
                .HasConstraintName("FK_UserBasicData_UserBasicData.QualificationType");

            entity.HasOne(d => d.User).WithMany(p => p.UserBasicData)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserBasicData_UserBasicData_Profile");
        });
        modelBuilder.Entity<UserDetails>(entity =>
        {
            entity.Property(e => e.ParentMobil).HasMaxLength(50);
            entity.Property(e => e.ParentChild).HasMaxLength(50);
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("PK_UserProfile_1");

            entity.ToTable("UserProfile");

            entity.HasIndex(e => e.UserProfileId, "IX_UserProfile_1").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");
            entity.Property(e => e.UserProfileId)
                .ValueGeneratedOnAdd()
                .HasColumnName("UserProfileID");

            entity.HasOne(d => d.Role).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserProfile_Role");

            entity.HasOne(d => d.User).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserProfile_User");
        });

        modelBuilder.Entity<UserProfileImage>(entity =>
        {
            entity.HasKey(e => e.UserProfileImageId).HasName("PK_UserImage");

            entity.ToTable("UserProfileImage");

            entity.Property(e => e.UserProfileImageId).HasColumnName("UserProfileImageID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.ImageTypeId).HasColumnName("ImageTypeID");
            entity.Property(e => e.UserProfileId).HasColumnName("UserProfileID");

            entity.HasOne(d => d.Image).WithMany(p => p.UserProfileImages)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserImage_Image");

            entity.HasOne(d => d.ImageType).WithMany(p => p.UserProfileImages)
                .HasForeignKey(d => d.ImageTypeId)
                .HasConstraintName("FK_UserImage_ImageType");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.UserProfileImages)
                .HasPrincipalKey(p => p.UserProfileId)
                .HasForeignKey(d => d.UserProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserProfileImage_UserProfile");
        });

        modelBuilder.Entity<UserUserStatus>(entity =>
        {
            entity.HasKey(e => e.UserStatusId).HasName("PK_UserUserStatuses");

            entity.ToTable("User.UserStatus");

            entity.Property(e => e.UserStatusId)
                .ValueGeneratedOnAdd()
                .HasColumnName("UserStatusID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");
            entity.Property(e => e.UserStatus).HasMaxLength(50);
        });

        modelBuilder.Entity<UsersUsersActivationArchieve>(entity =>
        {
            entity.HasKey(e => e.DisActivatedUserId);

            entity.ToTable("Users. Users.ActivationArchieve");

            entity.Property(e => e.DisActivatedUserId).HasColumnName("DisActivatedUserID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.UsersUsersActivationArchieves)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users. Users.ActivationArchieve_User");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.Property(e => e.VideoId).HasColumnName("VideoID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.VideoFileName).HasMaxLength(200);
            entity.Property(e => e.VideoShortDescription).HasMaxLength(200);
            entity.Property(e => e.VideoTypeId).HasColumnName("VideoTypeID");

            entity.HasOne(d => d.VideoType).WithMany(p => p.Videos)
                .HasForeignKey(d => d.VideoTypeId)
                .HasConstraintName("FK_Videos_Videos_Type");
        });

        modelBuilder.Entity<VideoVideoType>(entity =>
        {
            entity.HasKey(e => e.VideoTypeId);

            entity.ToTable("Video.VideoType");

            entity.Property(e => e.VideoTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("VideoTypeID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.VideoType).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
