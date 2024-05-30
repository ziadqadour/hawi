using Hawi.Dtos;
using Hawi.Models;
using Hawi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hawi.Extensions
{
    public class MatchFunctions
    {

        HawiContext _context = new HawiContext();
        private readonly UserFunctions _userFunctions;
        public MatchFunctions(UserFunctions userFunctions)
        {
            _userFunctions = userFunctions;
        }

        public async Task<object> GetMatchById(Models.Match match, long id)
        {
            try
            {
                var referee = _context.MatchReferees.FirstOrDefault(x => x.MatchId == id);

                var mainReferee = GetMainRefereeInfo(referee?.MainRefereeId);
                var assistant1Referee = GetMainRefereeInfo(referee?.Assistant1RefereeId);
                var assistant2Referee = GetMainRefereeInfo(referee?.Assistant2RefereeId);
                var fourthReferee = GetMainRefereeInfo(referee?.FourthRefereeId);
                var ResidentRefereeId = GetMainRefereeInfo(referee?.ResidentRefereeId);
                var SupervisingRefereeId = GetMainRefereeInfo(referee?.SupervisingRefereeId);

                //ChampionSipIfExist
                var ChambionchipID = _context.ChampionshipMatchs.Where(x => x.MatchId == match.MatchId).FirstOrDefault();
                var Chambionchipname = ChambionchipID != null ?
                    _context.Championships.FirstOrDefault(x => x.ChampionshipsId == ChambionchipID.ChampionshipId).ChampionshipsName :
                    null;

                //checkPreparation of the 2 team 
                var team1Checkprepare = _context.MatchPlayers
                            .Where(mp => mp.MatchId == match.MatchId
                              && mp.TeamId == match.HomeTeamId && mp.IsPrepare == true)
                            .FirstOrDefault();

                var team2Checkprepare = _context.MatchPlayers
                           .Where(mp => mp.MatchId == match.MatchId
                             && mp.TeamId == match.AwayTeamId && mp.IsPrepare == true)
                           .FirstOrDefault();

                var Playground = _context.Playgrounds.Where(x => x.PlaygroundId == match.PlayGroundId).FirstOrDefault();
                MatchTime realTime = GetTimeOfMatchNow(match);
                var IsRefereeRequest = _context.MatchMatchRefereeRequests.Where(x => x.MatchId == match.MatchId && x.IsActive == true).FirstOrDefault();
                var matchDto = new
                {
                    match.MatchId,
                    MatchType = match.MatchType?.MatchType ?? string.Empty,
                    IsRefereesRequest = (IsRefereeRequest == null) ? false : true,
                    NumberOfReferees = (IsRefereeRequest == null) ? 0 : IsRefereeRequest.NumberOfReferee,
                    IsStart = match.IsStart ?? false,
                    IsEnd = match.IsEnd ?? false,
                    HalfTimeBreak = match.HalfTimeBreak ?? 0,
                    MatchTime = match.MatchTime ?? null,
                    NumberOfPlayerId = match.NumberOfPlayer ?? 0,
                    season = _context.Seasons.FirstOrDefault(x => x.SeasonId == match.SeasonId)?.SeasonName ?? null,
                    HomeTeam = GetTeamInfo(match.HomeTeam, id, match),
                    AwayTeam = GetTeamInfo(match.AwayTeam, id, match),
                    Referee = new
                    {
                        MainReferee = mainReferee,
                        Assistant1Referee = assistant1Referee,
                        Assistant2Referee = assistant2Referee,
                        FourthReferee = fourthReferee,
                        SupervisingRefereeId = SupervisingRefereeId,
                        ResidentRefereeId = ResidentRefereeId,
                    },
                    CompetitionID = (ChambionchipID != null) ? (ChambionchipID?.ChampionshipId) : null,
                    CompetitionName = (Chambionchipname != null) ? (Chambionchipname) : null,
                    MatchStartTime = (match.MatchTime == null) ? null : match.MatchTime,
                    MatchStatus = (match.IsStart == true && match.IsEnd == false) ? "المباراة جارية" :
                                           (match.IsStart == false && match.IsEnd == false) ? "المباراة لم تبدأ بعد" : "المباراة انتهت",
                    MatchDate = match.MatchDate?.ToString("MM/dd/yyyy"),
                    MatchDuration = match.MatchDuration ?? null,
                    RealTimeMinuteTime = realTime.Minutes,
                    RealTimeSecondTime = realTime.Seconds,
                    FirstHalfWastedTime = match.FirstHalfWastedTime ?? null,
                    SecondHalfWastedTime = match.SecondHalfWastedTime ?? null,
                    HomeGoalsCount = match.MatchScores.Where(x => x.TeamId == match.HomeTeamId).Count(),
                    AwayGoalsCount = match.MatchScores.Where(x => x.TeamId == match.AwayTeamId).Count(),
                    // is team 1 preoaresd or not yet
                    HomeTeamprepare = (team1Checkprepare == null) ? false : true,
                    AwayTeamprepare = (team2Checkprepare == null) ? false : true,
                    DetermineAnyPartOfMatchStart = (match.SecondHalfEndTime != null) ? 4
                        : (match.SecondHalfStartTime != null) ? 3 : (match.FirstHalfEndTime != null) ? 2
                        : (match.MatchStartTime != null) ? 1 : 0,
                    MatchPlase = (Playground != null) ? Playground.PlaygroundLocation : null,
                    PlaygroundName = (Playground != null) ? Playground.PlaygroundName : null,
                };
                return matchDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<object> GetAllMatch([FromQuery] OwnerParameters ownerParameters, bool IsFinish = true, bool IsYourTMatches = true, bool ISHomeMatches = true, long userProfileId = 0)
        {
            try
            {
                List<Models.Match> selectedMatches = null;
                List<long> allMatchesID = null;

                if (ISHomeMatches == true)
                {

                    ownerParameters.PageNumber = 0;
                    ownerParameters.PageSize = 2;
                    selectedMatches = await _context.Matches
                    .Include(m => m.HomeTeam)
                    .Include(m => m.AwayTeam)
                    .Include(m => m.MatchType)
                    .Include(m => m.MatchScores)
                    .Include(m => m.MatchMatchRefereeRequests)
                    //اللى مخلصتش واللى معداش عليها 2 ايام 
                    .Where(x => x.IsEnd == false && x.MatchDate >= DateTime.UtcNow.AddDays(-2))
                    .OrderBy(x => (x.IsEnd == false && x.IsStart == true) ? 0 : 1)
                    .ThenBy(x => (x.IsEnd == false && x.IsStart == true) ? 0 : 1)
                    .ThenBy(x => (x.IsEnd == false && x.IsStart == false) ? 0 : 1)
                    .Skip(0)
                    .Take(2)
                    .ToListAsync();

                }
                else
                {

                    var matchTechnicalTeam = await _context.MatchTechnicalTeams
                      .Where(x => x.UserProfileId == userProfileId)
                      .Select(x => x.MatchId)
                      .Distinct()
                      .ToListAsync();

                    var matchPlayer = await _context.MatchPlayers
                        .Where(x => x.Player.UserProfileId == userProfileId)
                        .Select(x => x.MatchId)
                        .Where(x => x.HasValue)
                        .Select(x => x.Value)
                        .Distinct()
                        .ToListAsync();

                    var matchReferee = await _context.MatchReferees
                        .Where(x =>
                            x.MainRefereeId == userProfileId ||
                            x.Assistant1RefereeId == userProfileId ||
                            x.Assistant2RefereeId == userProfileId ||
                            x.FourthRefereeId == userProfileId ||
                            x.ResidentRefereeId == userProfileId ||
                            x.SupervisingRefereeId == userProfileId ||
                            x.UserProfileId == userProfileId)
                        .Select(x => x.MatchId)
                        .Distinct()
                        .ToListAsync();

                    var matchSportInstitution = await _context.Matches
                        .Where(x => x.AwayTeam.UserProfileId == userProfileId || x.HomeTeam.UserProfileId == userProfileId)
                        .Select(x => x.MatchId)
                        .Distinct()
                        .ToListAsync();

                    var addedChampionshipMatch = await _context.ChampionshipMatchs
                        .Where(x => x.UserProfileId == userProfileId)
                        .Select(x => x.MatchId)
                        .Where(x => x.HasValue)
                        .Select(x => x.Value)
                        .Distinct()
                        .ToListAsync();

                    allMatchesID = matchTechnicalTeam
                       .Concat(matchPlayer)
                       .Concat(matchReferee)
                       .Concat(matchSportInstitution)
                       .Concat(addedChampionshipMatch)
                       .Distinct()
                       .ToList();

                    if (IsYourTMatches == true)
                    {
                        selectedMatches = await _context.Matches
                       .Include(m => m.HomeTeam)
                       .Include(m => m.AwayTeam)
                       .Include(m => m.MatchType)
                       .Include(m => m.MatchScores)
                       .Include(m => m.MatchMatchRefereeRequests)
                       .Include(m => m.ChampionshipMatches)
                      .Where(x => allMatchesID != null && allMatchesID.Contains(x.MatchId))
                      .Where(x => (IsFinish == false && x.IsEnd == false)
                                 || (IsFinish == true && x.IsEnd == true))
                      .OrderBy(m => m.MatchDate)
                      .ThenBy(m => m.MatchStartTime)
                      .ToListAsync();
                    }

                    else if (IsYourTMatches == false)
                    {
                        selectedMatches = await _context.Matches
                       .Include(m => m.HomeTeam)
                       .Include(m => m.AwayTeam)
                       .Include(m => m.MatchType)
                       .Include(m => m.MatchScores)
                       .Include(m => m.MatchMatchRefereeRequests)
                       .Include(m => m.ChampionshipMatches)
                      .Where(x => allMatchesID != null && !allMatchesID.Contains(x.MatchId))
                      .Where(x => (IsFinish == false && x.IsEnd == false)
                                 || (IsFinish == true && x.IsEnd == true))
                      .OrderBy(m => m.MatchDate)
                      .ThenBy(m => m.MatchStartTime)
                      .ToListAsync();
                    }


                }

                var Matches = selectedMatches.Select(m =>
                {
                    var ChambionchipID = _context.ChampionshipMatchs.Where(x => x.MatchId == m.MatchId).FirstOrDefault();
                    var Chambionchipname = ChambionchipID != null ?
                        _context.Championships.FirstOrDefault(x => x.ChampionshipsId == ChambionchipID.ChampionshipId).ChampionshipsName :
                        null;

                    var CheckIFFromTechnicalTeam = _context.MatchTechnicalTeams
                    .Where(x => x.MatchId == m.MatchId && x.UserProfileId == userProfileId).FirstOrDefault();
                    MatchTime realTime = GetTimeOfMatchNow(m);
                    return new
                    {
                        matchid = m.MatchId,
                        HomeTeamLogoUrlfullPath = m.HomeTeam.LogoUrlfullPath,
                        HomeTeamSportInstitutionName = m.HomeTeam.SportInstitutionName,
                        AwayTeamLogoUrlfullPath = m.AwayTeam.LogoUrlfullPath,
                        AwayTeamSportInstitutionName = m.AwayTeam.SportInstitutionName,
                        CompetitionID = (ChambionchipID != null) ? (ChambionchipID?.ChampionshipId) : null,
                        CompetitionName = (Chambionchipname != null) ? (Chambionchipname) : null,
                        MatchStartTime = (m.MatchTime == null) ? null : m.MatchTime,
                        MatchStatus = (m.IsStart == true && m.IsEnd == false) ? "المباراة جارية" :
                                           (m.IsStart == false && m.IsEnd == false) ? "المباراة لم تبدأ بعد" : "المباراة انتهت",
                        MatchDate = m.MatchDate,
                        MatchDuration = m.MatchDuration ?? null,
                        //RealTimeMinuteTime = _MatchFunctions.GetminiteOfMatchNow(m),
                        RealTimeMinuteTime = realTime.Minutes,
                        RealTimeSecondsTime = realTime.Seconds,
                        HomeGoals = m.MatchScores.Where(x => x.TeamId == m.HomeTeamId).Count(),
                        AwayGoals = m.MatchScores.Where(x => x.TeamId == m.AwayTeamId).Count(),
                        TeamIDifPersonIsCoach = (CheckIFFromTechnicalTeam != null) ? CheckIFFromTechnicalTeam.TeamId : null,
                    };
                })
                 .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                 .Take(ownerParameters.PageSize)
                 .ToList();

                if (ISHomeMatches == true)
                    return Matches;

                var groupedMatches = Matches
                    .GroupBy(m => m.MatchDate)
                    .Select(group => group.OrderBy(m => m.MatchDate).ThenBy(m => m.MatchStartTime).ToList())
                    .ToList();

                return groupedMatches;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<object> GetAllMatchessNeedReferee()
        {
            try
            {
                var query = await _context.MatchMatchRefereeRequests
                    .Where(x => x.IsActive == true && x.Match.IsStart == false && x.Match.IsEnd == false)
                    .Select(m => new
                    {
                        matchid = m.MatchId,
                        HomeTeamLogoUrlfullPath = m.Match.HomeTeam.LogoUrlfullPath ?? null,
                        HomeTeamSportInstitutionName = m.Match.HomeTeam.SportInstitutionName ?? null,
                        AwayTeamLogoUrlfullPath = m.Match.AwayTeam.LogoUrlfullPath ?? null,
                        AwayTeamSportInstitutionName = m.Match.AwayTeam.SportInstitutionName ?? null,
                        CompetitionType = _context.ChampionshipMatchs//compation name
                    .Where(x => x.MatchId == m.MatchId)
                    .Select(x => x.Championship.ChampionshipsName)
                    .FirstOrDefault() ?? null,
                        MatchType = m.Match.MatchType.MatchType ?? null,
                        MatchStartTime = m.Match.MatchDate.HasValue ? m.Match.MatchDate.Value.ToString("MM/dd/yyyy hh:mm tt") : "Unknown",
                        Time = (m.Match.IsStart == true && m.Match.IsEnd == false) ? "المباراة جارية" :
                           (m.Match.IsStart == false && m.Match.IsEnd == false) ? "المباراة لم تبدأ بعد" : "المباراة انتهت",
                        //MinuteTime = GetTimeOfMatchNow(m.Match).Minutes,
                        //SecondsTime = GetTimeOfMatchNow(m.Match).Seconds,
                        HalfTimeBreak = m.Match.HalfTimeBreak ?? 0,
                        MatchTime = m.Match.MatchTime ?? null,
                        NumberOfPlayer = m.Match.NumberOfPlayer ?? 0,
                        EstimatedCost = m.EstimatedCost ?? 0,
                        Description = m.Description ?? "N/A",
                        MatchPlase = _context.Playgrounds
                        .Where(p => p.PlaygroundId == m.Match.PlayGroundId)
                        .Select(p => p.PlaygroundLocation)
                        .FirstOrDefault(),
                        PlaygroundName = _context.Playgrounds
                        .Where(p => p.PlaygroundId == m.Match.PlayGroundId)
                        .Select(p => p.PlaygroundLocation)
                        .FirstOrDefault(),
                        NumberOfReferee = m.NumberOfReferee ?? 0,
                        NumberOfCandidateReferee = _context.MatchMatchRefereeCandidates.Count(x => x.MatchId == m.MatchId)
                    })
                    .GroupBy(m => m.CompetitionType)
                    .ToListAsync();

                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Object> GetMatchScore(long id)
        {
            try
            {
                var match = await _context.Matches
                    .Include(m => m.HomeTeam)
                    .Include(m => m.AwayTeam)
                    .Include(m => m.MatchScores)
                        .ThenInclude(score => score.GoalMethod)
                    .Where(m => m.MatchId == id).FirstOrDefaultAsync();


                var homeTeamGoals = match.MatchScores
                   .Where(score => score.TeamId == match.HomeTeamId)
                   .Select(score => new
                   {
                       GoalId = score.MatchScoreId,
                       GoalTime = score.GoalTime,
                       GoalMethod = (score.GoalMethodId != null) ? _context.MatchScoreGoalMethods.FirstOrDefault(x => x.GoalMethodId == score.GoalMethodId).GoalMethod : null,
                       GoalPlayerName = (score.PlayerUserProfileId != null)
                           ? _userFunctions.GetUserName(_context.UserProfiles.FirstOrDefault(x => x.UserProfileId == score.PlayerUserProfileId).RoleId, (long)score.PlayerUserProfileId, _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == score.PlayerUserProfileId).UserId)
                           : null,
                       GoalPlayerNameUserProfileID = (score.PlayerUserProfileId != null) ? score.PlayerUserProfileId : null,
                       AssistPlayerName = (score.AssistPlayerUserProfileId != null) ? _userFunctions.GetUserName(_context.UserProfiles.FirstOrDefault(x => x.UserProfileId == score.AssistPlayerUserProfileId).RoleId, (long)score.AssistPlayerUserProfileId, _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == score.AssistPlayerUserProfileId).UserId) : null,
                       AssistPlayerUserProfileID = (score.AssistPlayerUserProfileId != null) ? score.AssistPlayerUserProfileId : null,
                   })
                   .ToList();

                var awayTeamGoals = match.MatchScores
                  .Where(score => score.TeamId == match.AwayTeamId)
                  .Select(score => new
                  {
                      GoalId = score.MatchScoreId,
                      GoalTime = score.GoalTime,
                      GoalMethod = (score.GoalMethodId != null) ? _context.MatchScoreGoalMethods.FirstOrDefault(x => x.GoalMethodId == score.GoalMethodId).GoalMethod : null,
                      GoalPlayerName = (score.PlayerUserProfileId != null)
                           ? _userFunctions.GetUserName(_context.UserProfiles.FirstOrDefault(x => x.UserProfileId == score.PlayerUserProfileId).RoleId, (long)score.PlayerUserProfileId, _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == score.PlayerUserProfileId).UserId)
                          : null,
                      GoalPlayerNameUserProfileID = (score.PlayerUserProfileId != null) ? score.PlayerUserProfileId : null,
                      AssistPlayerName = (score.AssistPlayerUserProfileId != null) ? _userFunctions.GetUserName(_context.UserProfiles.FirstOrDefault(x => x.UserProfileId == score.AssistPlayerUserProfileId).RoleId, (long)score.AssistPlayerUserProfileId, _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == score.AssistPlayerUserProfileId).UserId) : null,
                      AssistPlayerUserProfileID = (score.AssistPlayerUserProfileId != null) ? score.AssistPlayerUserProfileId : null,
                  })
                  .ToList();

                var TeamsGoal = new
                {
                    homeTeamGoals = homeTeamGoals,
                    homeTeamGoalsCount = homeTeamGoals.Count,
                    awayTeamGoals = awayTeamGoals,
                    awayTeamGoalsCount = awayTeamGoals.Count,
                };


                return TeamsGoal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GetFriendlyMatchDto> GetFrendlyMatchById(long EntireUserProfileId, long FrendlyMatchid)
        {
            try
            {
                var match = await _context.MatchFriendlyMatches
                    .Include(m => m.PlayGround)
                    .Include(m => m.SportInstituation)
                    .Include(m => m.UserProfile)
                    .Include(m => m.MatchFriendlyMatchAgeCategories)
                        .ThenInclude(m => m.AgeCategory)
                    .Where(m => m.FriendlyMatchId == FrendlyMatchid && m.IsDeleted == false && m.IsActive == true)
                    .FirstOrDefaultAsync();

                if (match == null)
                    throw new Exception("!لم يتم العثور على طلب المباراه");
                var playgroundCityId = match.PlayGround?.CityId;
                var playgroundName = match.PlayGround?.PlaygroundName;
                var playgroundLocation = match.PlayGround?.PlaygroundLocation;
                var sportInstitutionName = match.SportInstituation?.SportInstitutionName;
                var AgeCategoryId = match.MatchFriendlyMatchAgeCategories?.Select(x => x.AgeCategoryId).ToList();
                var AgeCategory = match.MatchFriendlyMatchAgeCategories?.Select(x => x.AgeCategory.AgeCategory).ToList();
                var selectedUserProfile = _context.UserProfiles.Where(x => x.UserProfileId == match.UserProfileId).FirstOrDefault();

                var CheckIfEntireUserApplicantBefore = _context.MatchApplicantsSportInstituationForFriendlyMatches
                       .Where(x => x.FriendlyMatchId == match.FriendlyMatchId && x.SportInstituation.UserProfileId == EntireUserProfileId)
                       .FirstOrDefault();

                var matchDto = new GetFriendlyMatchDto
                {
                    FriendlyMatchId = match.FriendlyMatchId,
                    SportInstituationId = match.SportInstituationId,
                    IsActive = match.IsActive,
                    MatchDate = match.MatchDate,
                    MatchTime = match.MatchTime,
                    MatchDuration = match.MatchDuration,
                    PlayGroundId = match.PlayGroundId,
                    IsRefereeRequest = match.IsRefereeRequest,
                    NumberOfReferee = match.NumberOfReferee,
                    Price = match.Price,
                    UserProfileId = match.UserProfileId,
                    SportInstitutionName = sportInstitutionName,
                    PlaygroundName = playgroundName,
                    PlaygroundLocation = playgroundLocation,
                    SportInstitutionImage = (selectedUserProfile != null) ? _userFunctions.GetUserImage(selectedUserProfile.UserProfileId, selectedUserProfile.RoleId) : null,
                    PlaygroundCityName = _context.Cities.FirstOrDefault(x => x.CityId == playgroundCityId).CityArabicName,
                    AgeCategoryId = AgeCategoryId,
                    AgeCategory = AgeCategory,
                    RequestForMatchSubmission = (CheckIfEntireUserApplicantBefore != null) ? true : false,
                };

                return matchDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<PlayerInfoDto> GetPlayerInfo(List<MatchPlayer> matchPlayers, bool isStart)
        {
            var playerInfoList = matchPlayers
             .Where(mp =>
             //دى التشكيلة اللى فى الملعب بما فى ذلك اللى تم استبدالة بخروج لاعب ونزول لاعب
             (isStart == true && ((mp.IsStart == true && mp.IsRefereeSubstituted == false) || (mp.IsStart == false && mp.IsRefereeSubstituted == true)))
             ||
             // اللى قاعدين بدلا بما فى ذلك اللى خرجوا من الملعب 
             (isStart == false && ((mp.IsStart == true && mp.IsRefereeSubstituted == true) || (mp.IsStart == false && mp.IsRefereeSubstituted == false))))
            .Select(mp => new PlayerInfoDto
            {
                //PlayerId = mp.PlayerId ?? 0,
                // UserProfileId = _context.Players.FirstOrDefault(x => x.PlayerId == mp.PlayerId)?.UserProfileId ?? null,
                PlayerUserProfileId = mp.PlayerUserProfileId ?? 0,
                PlayerNumber = mp.PlayerNumber ?? null /*_context.Players?.FirstOrDefault(x => x.PlayerId == mp.PlayerId)?.PlayerNumber*/ /*?? null*/,
                PlayerPlaceId = mp.PlaceId ?? null,
                PlayerPlace = _context.PlayerPlayerPlaces?.FirstOrDefault(x => x.PlayerPlaceId == mp.PlaceId)?.PlayerPlace ?? null,
                PositionNumber = _context.PlayerPlayerPlaces?.FirstOrDefault(x => x.PlayerPlaceId == mp.PlaceId)?.PlayerPositionNumber ?? null,
                Name = _userFunctions.GetUserName(_context.UserProfiles.FirstOrDefault(x => x.UserProfileId == mp.Player.UserProfileId).RoleId, (byte)mp.Player.UserProfileId, _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == mp.Player.UserProfileId).UserId),
                IsStart = (bool)mp.IsStart,
                ImgPath = _userFunctions.GetUserImage(mp.Player.UserProfileId, _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == mp.Player.UserProfileId).RoleId),
                IsRefereeSubstituted = mp.IsRefereeSubstituted ?? false,
                IsCoachSubstituted = mp.IsCoachSubstituted ?? false,
            })
             .ToList();
            return playerInfoList;
        }

        public List<CoachInfoDto> GetCoachInfo(List<MatchTechnicalTeam> matchCoach)
        {
            var playerInfoList = matchCoach
                .Select(mc =>
                {
                    var userProfile = _context.UserProfiles
                        .Where(x => x.UserProfileId == mc.UserProfileId)
                        .FirstOrDefault();

                    return new CoachInfoDto
                    {
                        userprofileid = mc.UserProfileId,
                        Name = _userFunctions.GetUserName(userProfile.RoleId, userProfile.UserProfileId, userProfile.UserId),
                        ImgPath = _userFunctions.GetUserImage(userProfile.UserProfileId, userProfile.RoleId),
                        Type = _context.MatchTechnicalTeamTypes.FirstOrDefault(x => x.MatchTechnicalTeamTypeId == mc.MatchTechnicalTeamTypeId).TechnicalTeamType ?? null
                    };
                })
                .ToList();

            return playerInfoList;
        }

        public async Task<String> ValidateMatchRefaree(MatchRefereeDto refereeDto)
        {
            var match = await _context.Matches.FirstOrDefaultAsync(x => x.MatchId == refereeDto.MatchId);
            if (match == null)
            {
                return ("لم يتم العثور على الماتش !");
            }

            var existingMatchReferee = await _context.MatchReferees.FirstOrDefaultAsync(x => x.MatchId == refereeDto.MatchId);
            if (existingMatchReferee != null)
            {
                return ("تم تعيين حكام لهذا الماتش من قبل!");
            }

            var MainReferee = await _context.UserProfiles.Where(x => x.UserProfileId == refereeDto.MainRefereeId && x.RoleId == 3).FirstOrDefaultAsync();
            if (MainReferee == null)
                return ("لم يتم العثور على الحكم لاول للماتش !");

            if (refereeDto.Assistant1RefereeId != null && refereeDto.Assistant1RefereeId != 0)
            {
                var Assistant1Referee = await _context.UserProfiles.Where(x => x.UserProfileId == refereeDto.Assistant1RefereeId && x.RoleId == 3).FirstOrDefaultAsync();
                if (Assistant1Referee == null)
                    return ("لم يتم العثور على الحكم المساعد الاول للماتش !");
            }

            if (refereeDto.Assistant2RefereeId != null && refereeDto.Assistant2RefereeId != 0)
            {
                var Assistant2Referee = await _context.UserProfiles.Where(x => x.UserProfileId == refereeDto.Assistant2RefereeId && x.RoleId == 3).FirstOrDefaultAsync();
                if (Assistant2Referee == null)
                    return ("لم يتم العثور على الحكم المساعد التانى للماتش !");
            }

            if (refereeDto.FourthRefereeId != null && refereeDto.FourthRefereeId != 0)
            {
                var FourthReferee = await _context.UserProfiles.Where(x => x.UserProfileId == refereeDto.FourthRefereeId && x.RoleId == 3).FirstOrDefaultAsync();
                if (FourthReferee == null)
                    return ("لم يتم العثور على الحكم الرابع للماتش !");
            }

            return null;
        }

        public MatchTime GetTimeOfMatchNow(Models.Match m)
        {
            DateTime currentTime = DateTime.UtcNow;

            double durationTimeOfSecondHalf = (m.MatchDuration == null) ? 0 : Math.Floor((double)(m.MatchDuration / 2));

            if (m.IsStart == true)
            {
                TimeSpan timeElapsed;
                int minutesElapsed;
                int secondsElapsed;

                if (m.SecondHalfStartTime != null && m.FirstHalfEndTime != null)
                {
                    timeElapsed = currentTime - m.SecondHalfStartTime.Value;
                    minutesElapsed = (int)timeElapsed.TotalMinutes;
                    secondsElapsed = timeElapsed.Seconds;

                    // Time of first half full time and time in the second half
                    int totalMinutes = (int)(minutesElapsed + durationTimeOfSecondHalf);

                    return new MatchTime
                    {
                        Minutes = totalMinutes,
                        Seconds = secondsElapsed
                    };
                }
                else if (m.MatchStartTime != null)
                {
                    timeElapsed = currentTime - m.MatchStartTime.Value;
                    minutesElapsed = (int)timeElapsed.TotalMinutes;
                    secondsElapsed = timeElapsed.Seconds;

                    return new MatchTime
                    {
                        Minutes = minutesElapsed,
                        Seconds = secondsElapsed
                    };
                }
            }
            else
            {
                // The match hasn't started yet
                return new MatchTime { Minutes = 0, Seconds = 0 };
            }
            return new MatchTime { Minutes = 0, Seconds = 0 };
        }

        public MainRefereeInfoDto GetMainRefereeInfo(long? refereeId)
        {

            if (refereeId != null && refereeId != 0)
            {
                var userprofile = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == refereeId);
                return new MainRefereeInfoDto
                {
                    userprofileid = refereeId,
                    Name = _context.Users.FirstOrDefault(x => x.UserId == (_context.UserProfiles.FirstOrDefault(c => c.UserProfileId == refereeId).UserId))?.Name,
                    ImgPath = _userFunctions.GetUserImage((long)refereeId, userprofile.RoleId),
                };
            }

            return null;
        }

        public object GetTeamInfo(SportInstitution team, long matchId, Models.Match match)
        {
            if (team == null)
                return null;

            return new
            {
                TeamId = team.SportInstitutionId,
                LogoUrlfullPath = team.LogoUrlfullPath ?? string.Empty,
                SportInstitutionName = team.SportInstitutionName ?? string.Empty,
                Uniform = team.MatchUniforms
                    .Where(uniform => uniform.MatchId == matchId && uniform.TeamId == team.SportInstitutionId)
                    .Select(uniform => new
                    {
                        MatchUniformId = uniform.MatchUniformId,
                        PlayerShortsColor = uniform.PlayerShortsColor,
                        PlayerSocksColor = uniform.PlayerSocksColor,
                        PlayerStshirtColor = uniform.PlayerStshirtColor,
                        GoalkeeperShortsColor = uniform.GoalkeeperShortsColor,
                        GoalkeeperTshirtColor = uniform.GoalkeeperTshirtColor,
                        GoalkeeperSocksColor = uniform.GoalkeeperSocksColor,
                    })
                    .FirstOrDefault(),
                MatchScore = match.MatchScores
                    .Where(score => score.MatchId == matchId && score.TeamId == team.SportInstitutionId)
                    .Select(score => new
                    {
                        PlayerName = _context.Users.FirstOrDefault(c => c.UserId == (_context.UserProfiles.FirstOrDefault(n => n.UserProfileId == score.PlayerUserProfileId)).UserId).Name ?? string.Empty,
                        GoalTime = score.GoalTime,
                    })
                    .ToList(),
            };
        }

        public List<MatchEvent> GetMatchEvents(long matchId)
        {

            var goalEvents = _context.MatchScores
                .Where(ms => ms.MatchId == matchId)
                .Select(ms => new
                {
                    ms,
                    PlayerGoal = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == ms.PlayerUserProfileId),
                    PlayerAssist = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == ms.AssistPlayerUserProfileId) ?? null
                })
                .Select(x => new MatchEvent
                {
                    MatchEventId = x.ms.MatchScoreId,
                    MatchId = x.ms.MatchId,
                    EventTime = x.ms.GoalTime ?? null,
                    Type = EventType.Goal,
                    GoalMethodId = (x.ms.GoalMethodId != null) ?
                        _context.MatchScoreGoalMethods.FirstOrDefault(gm => gm.GoalMethodId == x.ms.GoalMethodId).GoalMethod
                        : null,
                    playerOneUserProfileId = x.PlayerGoal.UserProfileId,
                    playerOneName = (x.PlayerGoal != null) ?
                        _userFunctions.GetUserName(x.PlayerGoal.RoleId, x.PlayerGoal.UserProfileId, x.PlayerGoal.UserId)
                        : null,
                    playerTwoNameUserProfileId = (x.PlayerAssist != null) ? x.PlayerAssist.UserProfileId
                        : null,
                    playerTwoName = (x.PlayerAssist != null) ?
                        _userFunctions.GetUserName(x.PlayerAssist.RoleId, x.PlayerAssist.UserProfileId, x.PlayerAssist.UserId)
                        : null,
                    TeamId = x.ms.TeamId,
                })
                .ToList();



            var substitutionEvents = _context.MatchSubstitutions
                .Where(ms => ms.MatchId == matchId)
                .Select(ms => new
                {
                    ms,
                    Playerin = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == ms.PlayerInUserProfileId),
                    //playerone
                    Playerout = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == ms.PlayerOutUserProfileId),
                    teamid = _context.MatchPlayers.Where(n => n.PlayerUserProfileId == ms.PlayerInUserProfileId && n.MatchId == ms.MatchId).FirstOrDefault(),
                })
                .Select(x => new MatchEvent
                {
                    MatchEventId = x.ms.MatchSubstitutionsId,
                    MatchId = x.ms.MatchId,
                    EventTime = x.ms.Minite ?? null,
                    Type = EventType.Substitution,
                    playerOneUserProfileId = x.ms.PlayerOutUserProfileId,
                    playerOneName = (x.Playerout != null) ?
                        _userFunctions.GetUserName(x.Playerout.RoleId, x.Playerout.UserProfileId, x.Playerout.UserId)
                        : null,
                    playerTwoNameUserProfileId = (x.Playerin != null) ? x.ms.PlayerInUserProfileId
                        : null,
                    playerTwoName = (x.Playerin != null) ?
                        _userFunctions.GetUserName(x.Playerin.RoleId, x.Playerin.UserProfileId, x.Playerin.UserId)
                        : null,
                    SubstitutionReason = x.ms.Reason,
                    TeamId = (x.teamid != null) ? x.teamid.TeamId : null,

                }).ToList();

            var cardEvents = _context.MatchCards
                .Where(mc => mc.MatchId == matchId)
                .Select(ms => new
                {
                    ms,
                    Player = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == ms.PlayerUserProfileId),
                    teamid = _context.MatchPlayers.Where(n => n.PlayerUserProfileId == ms.PlayerUserProfileId && n.MatchId == ms.MatchId).FirstOrDefault(),
                })
                .Select(x => new MatchEvent
                {
                    MatchEventId = x.ms.MatchCardId,
                    MatchId = x.ms.MatchId,
                    EventTime = x.ms.Minte ?? null,
                    Type = EventType.MatchCard,
                    MatchCardTypeId = (x.ms.MatchCardTypeId != null)
                        ? _context.MatchCardMatchCardTypes.FirstOrDefault(n => n.MatchCardTypeId == x.ms.MatchCardTypeId).MatchCardType
                        : null,
                    playerOneName = (x.Player != null) ?
                        _userFunctions.GetUserName(x.Player.RoleId, x.Player.UserProfileId, x.Player.UserId)
                        : null,
                    playerOneUserProfileId = (x.Player != null) ? x.ms.PlayerUserProfileId
                        : null,
                    MatchCardReason = x.ms.Reason,
                    TeamId = (x.teamid != null) ? x.teamid.TeamId : null,
                }).ToList();

            var allEvents = goalEvents.Concat(substitutionEvents).Concat(cardEvents).OrderBy(e => e.EventTime).ToList();
            return allEvents;
        }

        public PlayerStatisticsDto GetPlayerStatistics(MatchPlayer MatchPlayer, UserProfile playerUserProfile, long matchId, long teamId)
        {


            var redCardDetails = _context.MatchCards
           .Where(x => x.PlayerUserProfileId == playerUserProfile.UserProfileId && x.MatchId == matchId && x.MatchCardTypeId == 2)
           .Select(x => new MatchCardDetails { Minute = x.Minte, Reason = x.Reason })
           .FirstOrDefault();

            var yellowCardDetails = _context.MatchCards
            .Where(x => x.PlayerUserProfileId == playerUserProfile.UserProfileId && x.MatchId == matchId && x.MatchCardTypeId == 1)
            .Select(x => new MatchCardDetails { Minute = x.Minte, Reason = x.Reason })
            .ToList();

            var Goals = _context.MatchScores.Where(x => x.PlayerUserProfileId == playerUserProfile.UserProfileId
                 && x.MatchId == matchId && x.TeamId == teamId)
              .Select(x => new
              {
                  x.GoalTime,
                  HowIsGoal = _context.MatchScoreGoalMethods.FirstOrDefault(n => n.GoalMethodId == x.GoalMethodId).GoalMethod ?? null
              })
              .ToList();

            var assists = _context.MatchScores.Where(x => x.AssistPlayerUserProfileId == playerUserProfile.UserProfileId
                 && x.MatchId == matchId && x.TeamId == teamId)
            .Select(x => new
            {
                x.GoalTime,
            })
            .ToList();

            var playerPlase = _context.PlayerPlayerPlaces.FirstOrDefault(x => x.PlayerPlaceId == MatchPlayer.PlaceId).PlayerPlace;
            var PlayerName = _userFunctions.GetUserName(playerUserProfile.RoleId, playerUserProfile.UserProfileId, playerUserProfile.UserId);
            var PlayerImage = _userFunctions.GetUserImage(playerUserProfile.UserProfileId, playerUserProfile.RoleId);

            var playerStatisticsDto = new PlayerStatisticsDto
            {
                RedCard = redCardDetails,
                YellowCard = yellowCardDetails,
                Goals = Goals.Select(g => new GoalDto { GoalTime = g.GoalTime, HowIsGoal = g.HowIsGoal }).ToList(),
                Assists = assists.Select(a => new AssistDto { GoalTime = a.GoalTime }).ToList(),
                PlayerPlace = playerPlase,
                PlayerName = PlayerName,
                PlayerImage = PlayerImage
            };

            return playerStatisticsDto;
        }
    }
}
