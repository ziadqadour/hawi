using AutoMapper;
using Hawi.Dtos;
using Hawi.Extensions;
using Hawi.Models;
using Hawi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnidecodeSharpFork;

namespace Hawi.Controllers
{
    [Route("hawi/Matches/[action]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly HawiContext _context;
        private readonly UserFunctions _userFunctions;
        private readonly ImageFunctions _ImageFunctions;
        private readonly IMapper _mapper;
        private readonly MatchFunctions _MatchFunctions;
        private readonly Notification _Notification;
        private readonly INotificationService _notificationService;
        public MatchesController(INotificationService notificationService, Notification notification, UserFunctions userFunctions, MatchFunctions matchFunctions, IMapper mapper, ImageFunctions ImageFunctions, HawiContext context)
        {
            _mapper = mapper;
            _userFunctions = userFunctions;
            _MatchFunctions = matchFunctions;
            _context = context;
            _ImageFunctions = ImageFunctions;
            _Notification = notification;
            _notificationService = notificationService;
        }



        [HttpGet(Name = "GetMatchCompetitionTypes")]
        public async Task<IActionResult> GetMatchCompetitionTypes()
        {
            var matchCompetitionType = await _context.MatchCompetitionTypes
                .Select(x => new { x.CompetitionTypeId, x.CompetitionType })
                .ToListAsync();

            return Ok(matchCompetitionType);
        }

        [HttpGet(Name = "GetMatchMatchType")]
        public async Task<IActionResult> GetMatchMatchType()
        {
            var matchMatchType = await _context.MatchMatchTypes
            .Select(x => new { x.MatchTypeId, x.MatchType })
            .ToListAsync();

            return Ok(matchMatchType);
        }

        [HttpGet(Name = "GetSeasons")]
        public async Task<IActionResult> GetSeasons()
        {
            var seasons = await _context.Seasons
               .Select(x => new { x.SeasonId, x.SeasonName, x.StartDate, x.EndDate, })
               .ToListAsync();

            return Ok(seasons);
        }

        [HttpGet(Name = "GetPlayerPlaceType")]
        public async Task<IActionResult> GetPlayerPlaceType()
        {
            var PlayerPlaces = await _context.PlayerPlayerPlaces
               .Select(x => new { x.PlayerPlaceId, x.PlayerPlace })
               .ToListAsync();

            return Ok(PlayerPlaces);
        }

        [HttpGet(Name = "GetCardType")]
        public async Task<IActionResult> GetCardType()
        {
            var card = await _context.MatchCardMatchCardTypes
               .Select(x => new { x.MatchCardTypeId, x.MatchCardType })
               .ToListAsync();

            return Ok(card);
        }

        [HttpGet(Name = "GetMatchScoreType")]
        public async Task<IActionResult> GetMatchScoreType()
        {
            var ScoreType = await _context.MatchScoreGoalMethods
               .Select(x => new { x.GoalMethodId, x.GoalMethod })
               .ToListAsync();

            return Ok(ScoreType);
        }

        [HttpGet(Name = "GetUniformType")]
        public async Task<IActionResult> GetUniformType()
        {
            var UniformTypeard = await _context.UniformUniformTypes
                .Select(x => new { x.UniformTypeId, x.UniformType })
                .ToListAsync();

            return Ok(UniformTypeard);
        }


        [HttpGet(Name = "GetMatchById")]
        public async Task<IActionResult> GetMatchById(long id)
        {
            try
            {
                var match = await _context.Matches
                    .Include(m => m.HomeTeam)
                        .ThenInclude(team => team.MatchUniforms)
                    .Include(m => m.AwayTeam)
                        .ThenInclude(team => team.MatchUniforms)
                    .Include(m => m.MatchType)
                    .Include(m => m.MatchScores)
                        .ThenInclude(score => score.GoalMethod)
                    .Include(m => m.Season)
                    .Where(m => m.MatchId == id).FirstOrDefaultAsync();

                if (match == null)
                    return NotFound("!لم يتم العثور على المباراه");

                var SelectedMatch = await _MatchFunctions.GetMatchById(match, id);
                return Ok(SelectedMatch);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetAllMatches")]
        public async Task<IActionResult> GetAllMatches([FromQuery] OwnerParameters ownerParameters, bool IsFinish = true, bool IsYourTMatches = true, bool ISHomeMatches = true, long userProfileId = 0)
        {
            try
            {
                var SelectedMatchs = await _MatchFunctions.GetAllMatch(ownerParameters, IsFinish, IsYourTMatches, ISHomeMatches, userProfileId);
                return Ok(SelectedMatchs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetMatchScore")]
        public async Task<IActionResult> GetMatchScore(long MatchId)
        {
            try
            {
                var MatchGoals = await _MatchFunctions.GetMatchScore(MatchId);
                return Ok(MatchGoals);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet(Name = "GetFrendlyMatchById")]
        public async Task<IActionResult> GetFrendlyMatchByIdAsync(long EntireUserProfileId, long FrendlyMatchid)
        {
            try
            {
                var SelectedMatch = await _MatchFunctions.GetFrendlyMatchById(EntireUserProfileId, FrendlyMatchid);
                return Ok(SelectedMatch);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //not show what is not active
        [HttpGet(Name = "GetAllFrendlyMatchForSportInstituationById_ORGetAllActiveFrendlyMatch")]
        public IActionResult GetAllFrendlyMatchForSportInstituationById_ORGetAllActiveFrendlyMatch(long EntireUserProfileId, long? SportInstituationid = null)
        {
            try
            {
                //EntireUserProfileId >> to check if user has Applicant before to that match or not 

                List<MatchFriendlyMatch> matches = new List<MatchFriendlyMatch>();

                if (SportInstituationid != null)
                {
                    matches = _context.MatchFriendlyMatches
                      .Include(m => m.PlayGround)
                      .Include(m => m.SportInstituation)
                      .Include(m => m.UserProfile)
                      .Include(m => m.MatchFriendlyMatchAgeCategories)
                        .ThenInclude(m => m.AgeCategory)
                      .Where(m => m.SportInstituationId == SportInstituationid && m.MatchDate >= DateTime.Now && m.IsDeleted == false && m.IsActive == true)
                      .OrderBy(m => m.MatchDate)
                      .ToList();

                }
                else
                {
                    matches = _context.MatchFriendlyMatches
                 .Include(m => m.PlayGround)
                 .Include(m => m.SportInstituation)
                 .Include(m => m.UserProfile)
                 .Include(m => m.MatchFriendlyMatchAgeCategories)
                        .ThenInclude(m => m.AgeCategory)
                 .Where(m => m.IsDeleted == false && m.MatchDate >= DateTime.Now && m.IsActive == true)
                  .OrderBy(m => m.MatchDate)
                 .ToList();
                }


                var matchDtos = matches.Select(match =>
                {
                    var playgroundName = match.PlayGround?.PlaygroundName;
                    var playgroundLocation = match.PlayGround?.PlaygroundLocation;
                    var playgroundCityId = match.PlayGround?.CityId;
                    var sportInstitutionName = match.SportInstituation?.SportInstitutionName;

                    var AgeCategoryId = match.MatchFriendlyMatchAgeCategories?.Select(x => x.AgeCategoryId).ToList();
                    var AgeCategory = match.MatchFriendlyMatchAgeCategories?.Select(x => x.AgeCategory.AgeCategory).ToList();

                    var selectedUserProfile = _context.UserProfiles.Where(x => x.UserProfileId == match.UserProfileId).FirstOrDefault();

                    var CheckIfEntireUserApplicantBefore = _context.MatchApplicantsSportInstituationForFriendlyMatches
                        .Where(x => x.FriendlyMatchId == match.FriendlyMatchId && x.SportInstituation.UserProfileId == EntireUserProfileId)
                       .FirstOrDefault();

                    return new GetFriendlyMatchDto
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
                        SportInstitutionImage = (selectedUserProfile != null) ? _userFunctions.GetUserImage(selectedUserProfile.UserProfileId, selectedUserProfile.RoleId) : null,
                        PlaygroundName = playgroundName,
                        PlaygroundLocation = playgroundLocation,
                        PlaygroundCityName = _context.Cities.FirstOrDefault(x => x.CityId == playgroundCityId).CityArabicName,
                        AgeCategoryId = AgeCategoryId,
                        AgeCategory = AgeCategory,
                        RequestForMatchSubmission = (CheckIfEntireUserApplicantBefore != null) ? true : false,
                    };
                }).ToList();

                return Ok(matchDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetApplicantsSportInstituationForFriendlyMatch")]
        public IActionResult GetApplicantsSportInstituationForFriendlyMatch(long FriendlyMatchId)
        {
            try
            {
                var ApplicantsSportInstituation = _context.MatchApplicantsSportInstituationForFriendlyMatches
                  .Where(x => x.FriendlyMatchId == FriendlyMatchId)
                  .Include(m => m.SportInstituation)
                  .ToList();

                var SelectedResult = ApplicantsSportInstituation.Select(match =>
                {
                    var sportInstitutionName = match.SportInstituation?.SportInstitutionName;
                    var sportInstitutionuserProfileId = match.SportInstituation?.UserProfileId;
                    var userprofile = _context.UserProfiles.Where(X => X.UserProfileId == sportInstitutionuserProfileId).FirstOrDefault();

                    return new GetApplicantsSportInstituationForFriendlyMatchDto
                    {
                        ApplicantsSportInstituationId = match.ApplicantsSportInstituationId,
                        CreateDateTime = match.CreateDateTime,
                        FriendlyMatchId = match.FriendlyMatchId,
                        SportInstituationId = match.SportInstituationId,
                        SportInstitutionName = sportInstitutionName,
                        SportInstitutionImage = _userFunctions.GetUserImage(userprofile.UserProfileId, userprofile.RoleId)
                    };
                }).ToList();

                return Ok(SelectedResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //
        [HttpGet(Name = "GetAllListPlayersOfTeamInMatch")]
        public IActionResult GetAllListPlayersOfTeamInMatch(long matchId, long teamId)
        {
            try
            {
                var match = _context.Matches.FirstOrDefault(mp => mp.MatchId == matchId);
                if (match == null)
                {
                    return NotFound("لم يتم العثور على الماتش");
                }

                var matchPlayers = _context.MatchPlayers
                   .Where(mp => mp.MatchId == matchId && mp.TeamId == teamId)
                   .Select(x => new
                   {
                       MatchPlayer = x,
                       UserProfile = _context.UserProfiles
                           // .Where(u => u.UserProfileId == x.Player.UserProfileId)
                           .Where(u => u.UserProfileId == x.UserProfileId)
                           .FirstOrDefault()
                   })
                   .Select(x => new MatchPreparationPlayerDto
                   {
                       PlayerUserProfileId = x.MatchPlayer.Player.UserProfileId,
                       PlayerName = _userFunctions.GetUserName(x.UserProfile.RoleId, x.UserProfile.UserProfileId, x.UserProfile.UserId) ?? null,
                       PlayerImage = _userFunctions.GetUserImage(x.UserProfile.UserProfileId, x.UserProfile.RoleId),
                       IsStart = x.MatchPlayer.IsStart,
                       IsPrepare = x.MatchPlayer.IsPrepare,
                       PlayerNumber = x.MatchPlayer.PlayerNumber,
                       PlaceId = x.MatchPlayer.PlaceId,
                       PlayerId = x.MatchPlayer.PlayerId,
                   })
                   .ToList();


                return Ok(matchPlayers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //(add pagination)
        [HttpGet(Name = "GetAllMatchessNeedReferee")]
        public async Task<IActionResult> GetAllMatchessNeedRefereeAsync()
        {
            try
            {
                var SelectedMatchs = await _MatchFunctions.GetAllMatchessNeedReferee();
                return Ok(SelectedMatchs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //for admin
        [HttpGet(Name = "GetNotFinishedMatch")]
        public IActionResult GetNotFinishedMatch()
        {
            try
            {
                DateTime now = DateTime.Now;
                double hoursThreshold = 24.0;
                var query = _context.Matches
                        .Include(m => m.HomeTeam)
                        .Include(m => m.AwayTeam)
                        .Include(m => m.MatchType)
                        .Include(m => m.MatchScores)
                        .Where(m => m.IsEnd != true)
                        .Select(m => new
                        {
                            matchid = m.MatchId,
                            HomeTeamLogoUrlfullPath = m.HomeTeam.LogoUrlfullPath,
                            HomeTeamSportInstitutionName = m.HomeTeam.SportInstitutionName,
                            HomeTeamSportInstitutionId = m.HomeTeam.SportInstitutionId,
                            AwayTeamLogoUrlfullPath = m.AwayTeam.LogoUrlfullPath,
                            AwayTeamSportInstitutionName = m.AwayTeam.SportInstitutionName,
                            AwayTeamSportInstitutionId = m.AwayTeam.SportInstitutionId,
                            CompetitionType = _context.ChampionshipMatchs //compation name
                            .Where(x => x.MatchId == m.MatchId)
                            .Select(x => x.Championship.ChampionshipsName)
                            .FirstOrDefault() ?? null,
                            MatchType = m.MatchType.MatchType,
                            MatchStartTime = m.MatchDate.HasValue ?
                                m.MatchDate.Value.ToString("MM/dd/yyyy hh:mm tt") : "Unknown",
                            Time = (m.IsStart == true && m.IsEnd == false) ? "المباراة جارية" :
                                           (m.IsStart == false && m.IsEnd == false) ? "المباراة لم تبدأ بعد" : "المباراة انتهت",
                            MinuteTime = _MatchFunctions.GetTimeOfMatchNow(m).Minutes,
                            SecondsTime = _MatchFunctions.GetTimeOfMatchNow(m).Seconds,
                            HomeGoals = m.MatchScores.Where(x => x.TeamId == m.HomeTeamId).Count(),
                            AwayGoals = m.MatchScores.Where(x => x.TeamId == m.AwayTeamId).Count(),
                            m.NumberOfPlayer
                        });
                var matches = query
                    .GroupBy(x => x.CompetitionType)
                    .ToList();

                return Ok(matches);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetUniformsToSpeceficTeam")]
        public async Task<IActionResult> GetUniformsToSpeceficTeam(long teamId)
        { //teamId sportinstituationid

            try
            {
                var uniforms = await _context.Uniforms
                    .Include(u => u.UniformType)
                    .Include(u => u.UniformImage)
                    .Where(u => u.TeamId == teamId)
                    .ToListAsync();

                var uniform = uniforms.Select(x => new
                {
                    UniformId = x.UniformId,
                    TeamName = _context.SportInstitutions.FirstOrDefault(x => x.SportInstitutionId == teamId).SportInstitutionName ?? null,
                    UniformTypeId = x.UniformTypeId,
                    UniformTypeName = x.UniformType.UniformType ?? null,
                    UniformImageId = x.UniformImageId,
                    UniformImageUrl = x.UniformImage?.ImageUrlfullPath ?? null,
                    UserProfileId = x.UserProfileId
                })
                  .ToList();
                return Ok(uniform);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //
        [HttpPost(Name = "GetPlayersOfTeam")]
        public IActionResult GetPlayersOfTeam([FromBody] GetPlayersOfTeamDto PlayersDto)
        {
            try
            {//according spesefic age category
                List<SportInstitutionAgeCategoryBelong> selectedPlayer = null;
                List<long> selectedPlayersUserProfileId = new List<long>();

                if (PlayersDto.AgeCategory != null && PlayersDto.AgeCategory.Count != 0)
                {
                    selectedPlayer = _context.SportInstitutionAgeCategoryBelongs
                           .Where(x => x.SportInstitutionBelong.BelongTypeId == 1
                            && PlayersDto.AgeCategory.Contains(x.AgeCategoryId) &&
                            x.SportInstitutionBelong.SportInstitutionBranch.SportInstitutionId == PlayersDto.SportInstituationId)
                           .ToList();

                }


                //that no filter with branch and no filter with age category
                if (selectedPlayer == null)
                    selectedPlayer = _context.SportInstitutionAgeCategoryBelongs.Where(x => x.SportInstitutionBelong.BelongTypeId == 1 && x.SportInstitutionBelong.SportInstitutionBranch.SportInstitutionId == PlayersDto.SportInstituationId).ToList();

                foreach (var PProfileid in selectedPlayer)
                {
                    var SelectedPProfileid = _context.SportInstitutionBelongs
                        .Where(x => x.SportInstitutionBelongId == PProfileid.SportInstitutionBelongId)
                        .FirstOrDefault();

                    if (SelectedPProfileid != null) selectedPlayersUserProfileId.Add(SelectedPProfileid.BelongUserProfileId);
                }

                var players = _context.Players
                    .Where(x => selectedPlayersUserProfileId.Contains(x.UserProfileId))
                    .ToList();

                //if no result according category then get all player in that sportinstituation
                if (players == null && players.Count == 0)
                    players = _context.Players.
                        Where(x => x.SportInstitutionBranch.SportInstitutionId == PlayersDto.SportInstituationId)
                       .ToList();

                List<sportInstituationPlayerDto> sportInstituationPlayers = new List<sportInstituationPlayerDto>();

                sportInstituationPlayerDto x;
                foreach (var player in players)
                {
                    var ProfileId = _context.UserProfiles.Where(x => x.UserProfileId == player.UserProfileId).FirstOrDefault();

                    x = new sportInstituationPlayerDto
                    {
                        PlayerId = player.PlayerId,
                        UserProfileId = player.UserProfileId,
                        Name = _userFunctions.GetUserName(ProfileId.RoleId, ProfileId.UserProfileId, ProfileId.UserId),
                        PlayerNumber = player.PlayerNumber ?? null,
                        playerPlace = (player.MainPlaceId != 0 && player.MainPlaceId != null)
                                       ? _context.PlayerPlayerPlaces.FirstOrDefault(x => x.PlayerPlaceId == player.MainPlaceId)?.PlayerPlace : null
                    };
                    sportInstituationPlayers.Add(x);
                }
                return Ok(sportInstituationPlayers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //
        [HttpGet(Name = "GetMatchFormationOfTeams")]
        public IActionResult GetMatchFormationOfTeams(long matchId)
        {
            try
            {

                var match = _context.Matches
                    .Include(m => m.MatchPlayers)
                    .ThenInclude(mp => mp.Player)
                    .Include(m => m.MatchPlayers)
                    .ThenInclude(mp => mp.Team)
                    .FirstOrDefault(x => x.MatchId == matchId);

                if (match == null) return NotFound("لم يتم العثور على الماتش!");

                var homeTeamPlayers = _MatchFunctions.GetPlayerInfo(match.MatchPlayers.Where(mp => mp.TeamId == match.HomeTeamId).ToList(), true);
                var homeTeamSubstituatuinsPlayers = _MatchFunctions.GetPlayerInfo(match.MatchPlayers.Where(mp => mp.TeamId == match.HomeTeamId).ToList(), false);

                var awayTeamPlayers = _MatchFunctions.GetPlayerInfo(match.MatchPlayers.Where(mp => mp.TeamId == match.AwayTeamId).ToList(), true);
                var awayTeamSubstituatuinsPlayers = _MatchFunctions.GetPlayerInfo(match.MatchPlayers.Where(mp => mp.TeamId == match.AwayTeamId).ToList(), false);


                var MatchCoach = _context.MatchTechnicalTeams.Where(x => x.MatchId == matchId && x.TeamId == match.HomeTeamId).ToList();
                var MatchCoach2 = _context.MatchTechnicalTeams.Where(x => x.MatchId == matchId && x.TeamId == match.AwayTeamId).ToList();

                var HomeTeamCoach = _MatchFunctions.GetCoachInfo(MatchCoach);
                var AwayTeamCoach = _MatchFunctions.GetCoachInfo(MatchCoach2);


                var formation = new
                {
                    NumberOfPlayer = match.NumberOfPlayer,
                    HomeTeamName = _context.SportInstitutions.FirstOrDefault(x => x.SportInstitutionId == match.HomeTeamId)?.SportInstitutionName ?? null,
                    AwayTeamName = _context.SportInstitutions.FirstOrDefault(x => x.SportInstitutionId == match.AwayTeamId)?.SportInstitutionName ?? null,
                    HomeTeam = new { Players = homeTeamPlayers.GroupBy(x => x.PositionNumber) },
                    AwayTeam = new { Players = awayTeamPlayers.GroupBy(x => x.PositionNumber) },
                    homeTeamSubstituatuins = new { Substituatuins = homeTeamSubstituatuinsPlayers },
                    awayTeamSubstituatuins = new { Substituatuins = awayTeamSubstituatuinsPlayers },
                    HomeTeamCoach = new { HomeTeamCoach = HomeTeamCoach },
                    AwayTeamCoach = new { AwayTeamCoach = AwayTeamCoach },
                };

                return Ok(formation);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //
        [HttpGet(Name = "GetMatchSubstitutions")]
        public IActionResult GetMatchSubstitutions(long matchId, long teamId)
        {
            try
            {
                var matchSubstitutions = _context.MatchSubstitutions
                    .Where(x => x.MatchId == matchId && x.TeamId == teamId)
                    .Select(sub => new
                    {
                        Substitution = sub,
                        PlayerInUserProfile = _context.UserProfiles.FirstOrDefault(pl => pl.UserProfileId == sub.PlayerInUserProfileId),
                        PlayerInMatchPlayer = _context.MatchPlayers.FirstOrDefault(pl => pl.PlayerUserProfileId == sub.PlayerInUserProfileId && pl.MatchId == sub.MatchId && pl.TeamId == sub.TeamId),
                        PlayerOutUserProfile = _context.UserProfiles.FirstOrDefault(pl => pl.UserProfileId == sub.PlayerOutUserProfileId),
                        PlayerOutMatchPlayer = _context.MatchPlayers.FirstOrDefault(pl => pl.PlayerUserProfileId == sub.PlayerOutUserProfileId && pl.MatchId == sub.MatchId && pl.TeamId == sub.TeamId),
                    })
                    .Select(p => new
                    {
                        MatchSubstitutionsId = p.Substitution.MatchSubstitutionsId,
                        Matchid = p.Substitution.MatchId,
                        TeamId = p.Substitution.TeamId,
                        ConfirmedByTheReferee = p.Substitution.ConfirmedByTheReferee,
                        Minute = p.Substitution.Minite, // Corrected spelling
                        PlayerInUserProfileId = p.Substitution.PlayerInUserProfileId,
                        PlayerInName = (p.PlayerInUserProfile != null) ? _userFunctions.GetUserName(p.PlayerInUserProfile.RoleId, p.PlayerInUserProfile.UserProfileId, p.PlayerInUserProfile.UserId) : null,
                        PlayerInImgPath = (p.PlayerInUserProfile != null) ? _userFunctions.GetUserImage(p.PlayerInUserProfile.UserProfileId, p.PlayerInUserProfile.RoleId) : null,
                        PlayerInNumber = (p.PlayerInMatchPlayer != null) ? p.PlayerInMatchPlayer.PlayerNumber : null,
                        PlayerOutUserProfileId = p.Substitution.PlayerOutUserProfileId,
                        PlayerOutName = (p.PlayerOutUserProfile != null) ? _userFunctions.GetUserName(p.PlayerOutUserProfile.RoleId, p.PlayerOutUserProfile.UserProfileId, p.PlayerOutUserProfile.UserId) : null,
                        PlayerOutImgPath = (p.PlayerOutUserProfile != null) ? _userFunctions.GetUserImage(p.PlayerOutUserProfile.UserProfileId, p.PlayerOutUserProfile.RoleId) : null,
                        PlayerOutNumber = (p.PlayerOutMatchPlayer != null) ? p.PlayerOutMatchPlayer.PlayerNumber : null,
                    })
                    .ToList();

                var Substitution = new
                {
                    ConfirmedByTheReferee = matchSubstitutions.Where(x => x.ConfirmedByTheReferee == true).ToList(),
                    NotConfirmedByTheReferee = matchSubstitutions.Where(x => x.ConfirmedByTheReferee == false).ToList(),
                };

                return Ok(Substitution);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //
        [HttpGet(Name = "GetPlayerStatisticOfMatch")]
        public IActionResult GetPlayerStatisticOfMatch(long matchId, long PlayerUserProfileId, long teamId)
        {
            try
            {
                var MatchPlayerId = _context.MatchPlayers
                    .Where(x => x.PlayerUserProfileId == PlayerUserProfileId && x.MatchId == matchId && x.TeamId == teamId)
                    .FirstOrDefault();
                if (MatchPlayerId == null) return NotFound("!لم يتم العثور على اللاعب ");

                //var Player = _context.Players.Where(x => x.PlayerId == playerId).FirstOrDefault();
                //var playerUserProfile = _context.UserProfiles.Where(x => x.UserProfileId == Player.UserProfileId).FirstOrDefault();
                var playerUserProfile = _context.UserProfiles.Where(x => x.UserProfileId == PlayerUserProfileId).FirstOrDefault();

                var result = _MatchFunctions.GetPlayerStatistics(MatchPlayerId, playerUserProfile, matchId, teamId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //
        [HttpGet(Name = "GetMatchEvents")]
        public IActionResult GetMatchEvents(long MatchId)
        {
            var events = _MatchFunctions.GetMatchEvents(MatchId);
            return Ok(events);
        }


        ///////////////////////////////////////////////

        [HttpPost(Name = "AddMatch")]
        public async Task<IActionResult> AddMatch([FromBody] AddMatchDto matchDto, long userProfileId)
        {
            try
            {
                var entereduser = _context.UserProfiles.Where(x => x.UserProfileId.Equals(userProfileId)).FirstOrDefault();
                if (entereduser == null) return BadRequest("المستخدم الذى ادخلتة غير موجود");

                var lastSeason = _context.Seasons.OrderByDescending(s => s.SeasonId).FirstOrDefault();

                var newMatch = new Models.Match
                {
                    UserProfileId = userProfileId,
                    MatchTypeId = matchDto.MatchTypeId,
                    MatchDate = matchDto.MatchDate,
                    MatchTime = matchDto.MatchTime,
                    HomeTeamId = matchDto.HomeTeamId,
                    AwayTeamId = matchDto.AwayTeamId,
                    SeasonId = lastSeason.SeasonId,
                    NumberOfPlayer = matchDto.NumberOfPlayer,
                    PlayGroundId = matchDto.PlayGroundID,
                    IsStart = false,
                    IsEnd = false,
                    HalfTimeBreak = matchDto.HalfTimeBreak,
                    MatchDuration = matchDto.MatchDuration,
                };

                _context.Matches.Add(newMatch);
                _context.SaveChanges();
                long matchid = newMatch.MatchId;
                return Ok(matchid);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost(Name = "AddRequestForFriendlyMatch")]
        public async Task<IActionResult> AddRequestForFriendlyMatch([FromBody] FriendlyMatchDto matchDto)
        {
            using (var transication = _context.Database.BeginTransaction())
            {
                try
                {
                    var enteredPlayground = _context.Playgrounds.Where(x => x.PlaygroundId.Equals(matchDto.PlayGroundId)).FirstOrDefault();
                    if (enteredPlayground == null) return BadRequest("الملعب الذى ادخلتة غير موجود");

                    var CheeckUserProfile = _context.SportInstitutions.Where(x => x.UserProfileId == matchDto.UserProfileId).FirstOrDefault();
                    if (CheeckUserProfile == null)
                        return NotFound("!لم يتم العثور على اى منشاه رياضية لهذا المستخدم");

                    var checkMatchFreindly = _context.MatchFriendlyMatches
                        .Where(x => x.SportInstituationId == CheeckUserProfile.SportInstitutionId && x.IsActive == true && x.IsDeleted == false)
                        .FirstOrDefault();
                    if (checkMatchFreindly != null)
                    {
                        if (checkMatchFreindly.MatchDate < DateTime.Now)
                        {
                            checkMatchFreindly.IsActive = false;
                            checkMatchFreindly.IsDeleted = true;
                        }
                        else
                        {
                            return BadRequest("!يوجد لديك طلب مباراه مضاف يجب حذفة او استكمالة لتتمكن من الاضافة");
                        }
                    }

                    var newFriendlyMatch = _mapper.Map<FriendlyMatchDto, MatchFriendlyMatch>(matchDto);
                    newFriendlyMatch.IsActive = true;
                    newFriendlyMatch.SportInstituationId = CheeckUserProfile.SportInstitutionId;

                    _context.MatchFriendlyMatches.Add(newFriendlyMatch);
                    _context.SaveChanges();

                    if (matchDto.AgeCategoryId != null)
                    {
                        foreach (var x in matchDto.AgeCategoryId)
                        {
                            var newAgeCategory = new MatchFriendlyMatchAgeCategory
                            {
                                AgeCategoryId = x,
                                UserprofileId = matchDto.UserProfileId,
                                FriendlyMatchId = newFriendlyMatch.FriendlyMatchId,
                            };
                            _context.MatchFriendlyMatchAgeCategories.Add(newAgeCategory);
                        }
                    }

                    _context.SaveChanges();
                    transication.Commit();
                    return Ok("تم الاضافة بنجاح");
                }
                catch (Exception ex)
                {
                    transication.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }


        [HttpPost(Name = "CreateApplicantsSportInstituationForFriendlyMatch")]
        public async Task<IActionResult> CreateApplicantsSportInstituationForFriendlyMatch([FromBody] CreateApplicantsSportInstituationForFriendlyMatchDto Applicant)
        {
            try
            {
                var enteredFriendlyMatch = _context.MatchFriendlyMatches
                   .Include(m => m.MatchFriendlyMatchAgeCategories)
                   .Where(x => x.FriendlyMatchId.Equals(Applicant.FriendlyMatchId) && x.IsActive == true && x.IsDeleted == false)
                   .FirstOrDefault();

                if (enteredFriendlyMatch == null) return BadRequest("المباراه الذى ادخلتها غير موجودة");

                var checkIfSportInstituationApplicantBefore = _context.MatchApplicantsSportInstituationForFriendlyMatches
                    .Where(x => x.FriendlyMatchId == Applicant.FriendlyMatchId
                         && x.SportInstituationId == Applicant.SportInstituationId)
                    .FirstOrDefault();
                if (checkIfSportInstituationApplicantBefore != null)
                    return BadRequest("!تم تقديم طلب للعب هذه المباراه من قبل");

                var newApplicant = new MatchApplicantsSportInstituationForFriendlyMatch
                {
                    FriendlyMatchId = Applicant.FriendlyMatchId,
                    SportInstituationId = Applicant.SportInstituationId,
                    IsSelectedToPlayMatch = false,

                };
                _context.MatchApplicantsSportInstituationForFriendlyMatches.Add(newApplicant);
                _context.SaveChanges();

                return Ok("تم الطلب بنجاح");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // عرف الكلاينت سايد ان  notification target typeid =6
        [HttpPost(Name = "AcceptApplicantForFriendlyMatch")]
        public async Task<IActionResult> AcceptApplicantForFriendlyMatch([FromBody] CreateApplicantsSportInstituationForFriendlyMatchDto Applicant)
        {
            using (var transication = _context.Database.BeginTransaction())
            {
                try
                {
                    var enteredFriendlyMatch = await _context.MatchFriendlyMatches
                       .Include(m => m.MatchFriendlyMatchAgeCategories)
                       .Where(x => x.FriendlyMatchId.Equals(Applicant.FriendlyMatchId) && x.IsActive == true && x.IsDeleted == false)
                       .FirstOrDefaultAsync();

                    if (enteredFriendlyMatch == null) return BadRequest("المباراه الذى ادخلتها غير موجودة");

                    var CheckRequestForAppliacantIfExist = await _context.MatchApplicantsSportInstituationForFriendlyMatches
                        .Where(x => x.FriendlyMatchId == Applicant.FriendlyMatchId && x.SportInstituationId == Applicant.SportInstituationId)
                        .FirstOrDefaultAsync();
                    if (CheckRequestForAppliacantIfExist == null)
                        return NotFound("!قد يكون تم حذف طلب لعب المباراه من قبل  يجب تقديم طلب اخر لنفس المنشاه للعب المباراه");

                    //selected Other team TO friendlyMatch
                    CheckRequestForAppliacantIfExist.IsSelectedToPlayMatch = true;

                    //disactive friendlyMatch
                    enteredFriendlyMatch.IsActive = false;


                    var lastSeason = await _context.Seasons
                        .OrderByDescending(s => s.SeasonId)
                        .FirstOrDefaultAsync();

                    var SelectedPlayground = await _context.Playgrounds
                        .Where(x => x.PlaygroundId == enteredFriendlyMatch.PlayGroundId)
                        .FirstOrDefaultAsync();

                    var HomeTeam = await _context.SportInstitutions
                       .Where(x => x.SportInstitutionId == enteredFriendlyMatch.SportInstituationId)
                       .FirstOrDefaultAsync();

                    SportInstitution? AwayTeam = await _context.SportInstitutions
                      .Where(x => x.SportInstitutionId == CheckRequestForAppliacantIfExist.SportInstituationId)
                      .FirstOrDefaultAsync();

                    var UserDataForNotification = await _userFunctions.GetUserDataForNotificationByProfileId(AwayTeam.UserProfileId, HomeTeam.UserProfileId);

                    var AwayTeamProfileId = await _context.UserProfiles
                        .Where(x => x.UserProfileId == AwayTeam.UserProfileId)
                        .FirstOrDefaultAsync();
                    if (AwayTeamProfileId == null) return NotFound();


                    //CreateMatch
                    var NewMatch = new Models.Match
                    {
                        HomeTeamId = enteredFriendlyMatch.SportInstituationId,
                        AwayTeamId = CheckRequestForAppliacantIfExist.SportInstituationId,
                        MatchTypeId = 2,
                        MatchDate = enteredFriendlyMatch.MatchDate,
                        SeasonId = lastSeason.SeasonId,
                        UserProfileId = (long)enteredFriendlyMatch.UserProfileId,
                        IsEnd = false,
                        IsStart = false,
                        PlayGroundId = enteredFriendlyMatch.PlayGroundId,
                        MatchTime = enteredFriendlyMatch.MatchTime,
                        MatchDuration = enteredFriendlyMatch.MatchDuration,
                    };
                    _context.Matches.Add(NewMatch);
                    await _context.SaveChangesAsync();


                    //add referee request for match
                    if (enteredFriendlyMatch.IsRefereeRequest == true)
                    {
                        var newRefereeRequest = new Models.MatchMatchRefereeRequest
                        {
                            UserProfileId = (long)enteredFriendlyMatch.UserProfileId,
                            MatchDate = enteredFriendlyMatch.MatchDate,
                            Place = (SelectedPlayground != null) ? SelectedPlayground.PlaygroundLocation : null,
                            MatchId = NewMatch.MatchId,
                            IsActive = true,
                            NumberOfReferee = (enteredFriendlyMatch.NumberOfReferee != null) ? enteredFriendlyMatch.NumberOfReferee : null,
                            EstimatedCost = (enteredFriendlyMatch.Price != null) ? (byte)enteredFriendlyMatch.Price : null,
                        };
                        _context.MatchMatchRefereeRequests.Add(newRefereeRequest);
                        await _context.SaveChangesAsync();
                    }


                    //sendNotification
                    var message = $"تم قبول مشاركتك في المباراة بنجاح! ستقام المباراة وفريقك ملتزم بالمشاركة مع فريق[{HomeTeam.SportInstitutionName}].";
                    var _newDbObject = await _Notification.CreateDBObjectForNotification(HomeTeam.UserProfileId, AwayTeam.UserProfileId, message, NewMatch.MatchId, NewMatch.MatchId, 6);
                    await _context.RealTimeNotifications.AddAsync(_newDbObject);
                    await _context.SaveChangesAsync();

                    //var UserDataForNotification = await _userFunctions.GetUserDataForNotificationByProfileId(AwayTeam.UserProfileId, HomeTeam.UserProfileId);
                    var newRealTimeNotification = await _Notification.CreateRealTimeNotificationModel(UserDataForNotification, message, AwayTeamProfileId);
                    await _notificationService.SendNotification(newRealTimeNotification);

                    await _context.SaveChangesAsync();
                    transication.Commit();
                    return Ok("تم القبول بنجاح");

                }
                catch (Exception ex)
                {
                    transication.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }


        [HttpPost(Name = "AddMatchReferee")]
        public async Task<IActionResult> AddMatchReferee([FromBody] MatchRefereeDto refereeDto)
        {//على كلام دكتور ايمن المقيم والمراقب ممكن ميبقوش حكام ويكونو مستخدمين عاديين

            var ValidateMatchRefaree = await _MatchFunctions.ValidateMatchRefaree(refereeDto);
            if (ValidateMatchRefaree != null) return BadRequest($"{ValidateMatchRefaree}");

            //_mapper.Map<CreateChampionshipDTO, Championship>(championship);
            var newMatchReferee = _mapper.Map<MatchRefereeDto, MatchReferee>(refereeDto);

            await _context.MatchReferees.AddAsync(newMatchReferee);
            await _context.SaveChangesAsync();

            return Ok(newMatchReferee.MatchRefereeId);
        }


        [HttpPost(Name = "AddMatchFormationOfTeam")]
        public async Task<IActionResult> AddMatchFormationOfTeam([FromBody] AddMatchFormationOfTeamsDto playerDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var match = await _context.Matches.Where(x => x.MatchId == playerDTO.MatchId).FirstOrDefaultAsync();
                    if (match == null) return NotFound("لم يتم العثور على الماتش!");

                    var team = await _context.SportInstitutions.Where(x => x.SportInstitutionId == playerDTO.SportInstituationId).FirstOrDefaultAsync();
                    if (team == null) return NotFound("لم يتم العثور على هذا الفريق!");

                    var check = await _context.MatchPlayers.Where(x => x.MatchId == playerDTO.MatchId && x.TeamId == playerDTO.SportInstituationId).FirstOrDefaultAsync();
                    if (check != null) return BadRequest("تم اضافة تشكيل المباراه لهذا الفريق من قبل !");

                    // Check for duplicate PlayerIds in the list
                    var duplicatePlayerIds = playerDTO.matchFormationDtos
                        .GroupBy(x => x.PlayerUserProfileId)
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key)
                        .ToList();

                    if (duplicatePlayerIds.Any())
                        return BadRequest($"!لا يمكن تكرار لاعب فى تشكيلة الفريق");


                    var matchPlayers = new List<MatchPlayer>();
                    for (int i = 0; i < playerDTO.matchFormationDtos.Count; i++)
                    {
                        var player = new MatchPlayer
                        {
                            UserProfileId = playerDTO.UserProfileId,
                            MatchId = playerDTO.MatchId,
                            TeamId = playerDTO.SportInstituationId,
                            //PlayerId = playerDTO.matchFormationDtos[i].PlayerId,
                            PlayerUserProfileId = playerDTO.matchFormationDtos[i].PlayerUserProfileId,
                            PlaceId = playerDTO.matchFormationDtos[i].PlaceId,
                            PlayerNumber = playerDTO.matchFormationDtos[i].PlayerNumber,
                            IsStart = playerDTO.matchFormationDtos[i].IsStart
                        };
                        matchPlayers.Add(player);
                    }
                    await _context.MatchPlayers.AddRangeAsync(matchPlayers);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    return Ok("تم الاضافة بنجاح");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }


        [HttpPost(Name = "CreateMatchTechnicalTeams")]
        public async Task<IActionResult> CreateMatchTechnicalTeams([FromBody] List<MatchTechnicalTeamDto> teamDtos)
        {
            using (var transiction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (teamDtos == null || teamDtos.Count == 0)
                        return BadRequest();

                    var duplicateUserIds = teamDtos
                        .GroupBy(x => x.UserProfileId)
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key)
                        .ToList();

                    if (duplicateUserIds.Any())
                        return BadRequest($"!لا يمكن تكرار احد من الجهاز الفنى ");


                    foreach (var technical in teamDtos)
                    {
                        var CheckIfAddedBefore = await _context.MatchTechnicalTeams
                            .Where(x => x.MatchId == technical.MatchId
                              && x.UserProfileId == technical.UserProfileId
                              && x.MatchTechnicalTeamTypeId == technical.MatchTechnicalTeamTypeId)
                            .FirstOrDefaultAsync();

                        if (CheckIfAddedBefore != null)
                            return Conflict($"تم اضافة {CheckIfAddedBefore.MatchTechnicalTeamType.TechnicalTeamType} ,من قبل لنفس الشخص");

                    }

                    var entities = _mapper.Map<List<MatchTechnicalTeam>>(teamDtos);
                    await _context.MatchTechnicalTeams.AddRangeAsync(entities);
                    await _context.SaveChangesAsync();
                    transiction.Commit();
                    return Ok("تم الاضافة بنجاح");
                }
                catch (Exception ex)
                {
                    transiction.Rollback();
                    return StatusCode(500, ex.Message);
                }
            }
        }


        [HttpPost(Name = "CreateMatchUniform")]
        public async Task<IActionResult> CreateMatchUniform([FromBody] AddMatchUniformDto matchUniformDTO)
        {
            try
            {
                var match = await _context.Matches.Where(x => x.MatchId == matchUniformDTO.MatchId).FirstOrDefaultAsync();
                if (match == null)
                    return NotFound("لم يتم العثور على الماتش!");

                var userprofile = await _context.UserProfiles.Where(x => x.UserProfileId == matchUniformDTO.UserProfileId).FirstOrDefaultAsync();
                if (userprofile == null)
                    return NotFound("لم يتم العثور على هذا المستخدم!");

                var team = await _context.SportInstitutions.Where(x => x.SportInstitutionId == matchUniformDTO.TeamId).FirstOrDefaultAsync();
                if (team == null)
                    return NotFound($"لم يتم العثور على الفريق: {matchUniformDTO.TeamId}");

                var existingMatchUniform = await _context.MatchUniforms.Where(x =>
                    x.MatchId == matchUniformDTO.MatchId && x.TeamId == matchUniformDTO.TeamId).FirstOrDefaultAsync();
                if (existingMatchUniform != null)
                {
                    var matchUniformUpdated = _mapper.Map<MatchUniform>(matchUniformDTO);
                    _context.MatchUniforms.Update(matchUniformUpdated);
                    await _context.SaveChangesAsync();

                    return Ok("تمت التعديل الأطقم بنجاح");
                }

                var matchUniform = _mapper.Map<MatchUniform>(matchUniformDTO);
                await _context.MatchUniforms.AddAsync(matchUniform);
                await _context.SaveChangesAsync();
                return Ok("تمت إضافة الأطقم بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest($"حدث خطأ: {ex.Message}");
            }
        }


        [HttpPost(Name = "AddUniformToSpeceficTeam")]
        public async Task<IActionResult> AddUniformToSpeceficTeamAsync(long SportInstitutionId, long userprofileId, [FromForm] uniformDto uniform)
        {
            try
            {

                var team = _context.SportInstitutions.Where(x => x.SportInstitutionId == SportInstitutionId).FirstOrDefault();
                if (team == null)
                    return NotFound("لم يتم العثور على تلك المؤسسة الرياضية!");

                var userProfile = _context.UserProfiles.Where(x => x.UserProfileId == userprofileId).FirstOrDefault();
                if (userProfile == null)
                    return NotFound("لم يتم العثور على هذا المستخدم!");

                var user = _context.Users.Where(x => x.UserId == userProfile.UserId).FirstOrDefault();

                var uniformtype = _context.UniformUniformTypes.Where(x => x.UniformTypeId == uniform.UniformTypeId).FirstOrDefault();
                if (uniformtype == null)
                    return NotFound("لم يتم العثور على نوع الطقم الذى ادخلتة!");

                var checkuniform = _context.Uniforms.Where(x => x.TeamId == SportInstitutionId && x.UniformTypeId == uniform.UniformTypeId).FirstOrDefault();
                if (checkuniform != null)
                {
                    var selectedUnifoem = _context.UniformUniformTypes.FirstOrDefault(x => x.UniformTypeId == uniform.UniformTypeId).UniformType;
                    return Conflict($"تم اختيار {selectedUnifoem} من قبل ");
                }

                string englishFileName = uniform.Uniformimage.FileName.Unidecode();
                var photopath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\SportInstitution\{user.Mobile}\" + englishFileName;

                using (var stream = new FileStream(photopath, FileMode.Create))
                {
                    await uniform.Uniformimage.CopyToAsync(stream);
                }
                var newPath = $"http://mobile.hawisports.com/image/SportInstitution/{user.Mobile}/" + englishFileName;


                var img = new Models.Image
                {
                    ImageUrlfullPath = $"http://mobile.hawisports.com/image/SportInstitution/{user.Mobile}/" + englishFileName,
                    ImageFileName = englishFileName,
                    ImageTypeId = (uniform.UniformTypeId == 1) ? (byte?)6 : (uniform.UniformTypeId == 2) ? (byte?)7 : (byte?)8,
                    IsActive = true,
                };
                _context.Images.Add(img);
                _context.SaveChanges();
                var newuniform = new Uniform
                {
                    TeamId = SportInstitutionId,
                    UserProfileId = userprofileId,
                    UniformTypeId = uniform.UniformTypeId,
                    UniformImageId = img.ImageId,
                };
                _context.Uniforms.Add(newuniform);
                _context.SaveChanges();

                return Ok("تم اضافة الزى بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost(Name = "AddMatchScore")]
        public IActionResult AddMatchScore(long MatchId, long UserProfileId, [FromBody] AddMatchScore matchScore)
        {
            try
            {
                var match = _context.Matches.Where(x => x.MatchId == MatchId).FirstOrDefault();
                if (match == null)
                    return NotFound("لم يتم العثور على الماتش !");

                var userProfile = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileId).FirstOrDefault();
                if (userProfile == null)
                    return NotFound("لم يتم العثور على هذا المستخدم!");

                //var player = _context.MatchPlayers.Where(x => x.PlayerId == matchScore.PlayerUserProfileId && x.TeamId == matchScore.TeamId).FirstOrDefault();
                //return BadRequest("هذا اللاعب ليس فى تشكيلة هذا الفريق ! ");

                var newScore = new MatchScore
                {
                    UserProfileId = UserProfileId,
                    MatchId = MatchId,
                    PlayerUserProfileId = matchScore.PlayerUserProfileId,
                    GoalMethodId = matchScore.GoalMethodId,
                    GoalTime = matchScore.GoalTime,
                    TeamId = matchScore.TeamId,
                    //AssistPlayerUserProfileId=matchScore.AssistPlayerUserProfileId
                };
                _context.MatchScores.Add(newScore);
                _context.SaveChanges();
                return Ok("تمت الاضافة بنجاح");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpPost(Name = "AddMatchScoreAssist")]
        public IActionResult AddMatchScoreAssist(long MatchId, long AssistPlayerUserProfileId, long MatchScoreId)
        {
            try
            {
                var match = _context.Matches.Where(x => x.MatchId == MatchId).FirstOrDefault();
                if (match == null)
                    return NotFound("!لم يتم العثور على الماتش");

                var userProfile = _context.UserProfiles.Where(x => x.UserProfileId == AssistPlayerUserProfileId).FirstOrDefault();
                if (userProfile == null)
                    return NotFound("!لم يتم العثور على هذا اللاعب");

                var SelectedGoal = _context.MatchScores.Where(x => x.MatchScoreId == MatchScoreId).FirstOrDefault();
                if (SelectedGoal == null)
                    return NotFound("!لم يتم العثور على الهدف");

                //var player = _context.MatchPlayers.Where(x => x.PlayerId == matchScore.PlayerUserProfileId && x.TeamId == matchScore.TeamId).FirstOrDefault();
                //return BadRequest("هذا اللاعب ليس فى تشكيلة هذا الفريق ! ");

                SelectedGoal.AssistPlayerUserProfileId = AssistPlayerUserProfileId;

                _context.SaveChanges();
                return Ok("تمت الاضافة بنجاح");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpPost(Name = "AddCard")]
        public IActionResult AddCard(long MatchId, long userProfileidWhoEnterData, [FromBody] AddCardDto cardDto)
        {
            var match = _context.Matches.Where(x => x.MatchId == MatchId).FirstOrDefault();
            if (match == null)
                return NotFound("لم يتم العثور على الماتش!");

            var dataentryUser = _context.UserProfiles.Where(x => x.UserProfileId == userProfileidWhoEnterData).FirstOrDefault();
            if (dataentryUser == null)
                return NotFound("لم يتم العثور على مدخل البيانات  !");

            var player = _context.UserProfiles.Where(x => x.UserProfileId == cardDto.PlayerUserProfileId).FirstOrDefault();
            if (player == null)
                return NotFound("لم يتم العثور على اللاعب!");

            var CheckMatchCardCount = _context.MatchCards
                .Where(x => x.MatchId == MatchId && x.MatchCardTypeId == cardDto.MatchCardTypeId
                        && x.PlayerUserProfileId == cardDto.PlayerUserProfileId).Count();
            if (CheckMatchCardCount == 2)
                return BadRequest("!تم اضافة انذارين لهذا اللاعب وتم طردة ");


            var newCard = new MatchCard
            {
                PlayerUserProfileId = cardDto.PlayerUserProfileId,
                MatchCardTypeId = cardDto.MatchCardTypeId,
                MatchId = MatchId,
                Minte = cardDto.Minte,
                Reason = cardDto.Reason,
            };

            if (CheckMatchCardCount == 1 && cardDto.MatchCardTypeId == 1)
            {
                var newCard2 = new MatchCard
                {
                    PlayerUserProfileId = cardDto.PlayerUserProfileId,
                    MatchCardTypeId = 2,
                    MatchId = MatchId,
                    Minte = cardDto.Minte,
                    Reason = cardDto.Reason,
                };
            }
            _context.MatchCards.Add(newCard);
            _context.SaveChanges();
            return Ok("تم الاضافة بنجاح");
        }


        //تبديل
        [HttpPost(Name = "AddSubstitution")]
        public IActionResult AddSubstitution(long MatchId, long userProfileidWhoEnterData, [FromBody] MatchSubstitutionDto substitutionDto, long? MatchSubstitutionsId = null)
        {
            var match = _context.Matches.Where(x => x.MatchId == MatchId).FirstOrDefault();
            if (match == null)
                return NotFound("لم يتم العثور على الماتش!");

            var dataentryUser = _context.UserProfiles.Where(x => x.UserProfileId == userProfileidWhoEnterData).FirstOrDefault();
            if (dataentryUser == null)
                return NotFound("لم يتم العثور على مدخل البيانات  !");

            var playerIN = _context.MatchPlayers
                .Where(x => x.PlayerUserProfileId == substitutionDto.PlayerInUserProfileId
                  && x.MatchId == MatchId && x.TeamId == substitutionDto.TeamId)
                .FirstOrDefault();

            if (playerIN == null)
                return NotFound("لم يتم العثور على اللاعب البديل!");

            var playerOut = _context.MatchPlayers
                .Where(x => x.PlayerUserProfileId == substitutionDto.PlayerOutUserProfileId
                   && x.MatchId == MatchId && x.TeamId == substitutionDto.TeamId)
                .FirstOrDefault();

            if (playerOut == null)
                return NotFound(" لم يتم العثور على اللاعب المستبدل!");

            var CheckMatchSubstitutions = _context.MatchSubstitutions.Where(x => x.MatchSubstitutionsId == MatchSubstitutionsId).FirstOrDefault();

            if (CheckMatchSubstitutions != null)
            {//بياكد التبديل بواسطة الحكم

                if (CheckMatchSubstitutions.PlayerInUserProfileId == substitutionDto.PlayerInUserProfileId
                            && CheckMatchSubstitutions.ConfirmedByTheReferee == true)
                    return BadRequest("!تم اجراء تبديل لهذا اللاعب البديل من قبل");

                if (CheckMatchSubstitutions.PlayerOutUserProfileId == substitutionDto.PlayerOutUserProfileId
                        && CheckMatchSubstitutions.ConfirmedByTheReferee == true)
                    return BadRequest("!تم اجراء تبديل لهذا اللاعب المستبدل من قبل");

                CheckMatchSubstitutions.ConfirmedByTheReferee = true;
                CheckMatchSubstitutions.Minite = substitutionDto.Minite;

                //ليدل على انهما تم استبدالهما فى المباراه وتم تاكيد التبديل بواسطة الحكم
                playerIN.IsRefereeSubstituted = true;
                playerOut.IsRefereeSubstituted = true;
            }
            else
            {//بيضيف تبديل جديد


                if (playerIN.IsCoachSubstituted != null && playerIN.IsCoachSubstituted == true)
                    return BadRequest("!تم اجراء تبديل لهذا اللاعب البديل من قبل");

                if (playerOut.IsCoachSubstituted != null && playerOut.IsCoachSubstituted == true)
                    return BadRequest("!تم اجراء تبديل لهذا اللاعب المستبدل من قبل");


                var newsubstitution = new MatchSubstitution
                {
                    MatchId = MatchId,
                    //اللاعب اللى داخل الملعب جديد
                    PlayerInUserProfileId = substitutionDto.PlayerInUserProfileId,
                    // اللاعب اللى هيخرج من الملعب
                    PlayerOutUserProfileId = substitutionDto.PlayerOutUserProfileId,
                    Minite = substitutionDto.Minite,
                    TeamId = substitutionDto.TeamId,
                    Reason = substitutionDto.Reason
                };
                playerIN.IsCoachSubstituted = true;
                playerOut.IsCoachSubstituted = true;
                _context.MatchSubstitutions.Add(newsubstitution);
            }

            _context.SaveChanges();
            return Ok("تمت الاضافة بنجاح");
        }


        [HttpPost(Name = "AddPlayerDataBySportinstituation")]
        public IActionResult AddPlayerDataBySportinstituation(long sportinstiuationid, long userprofileId, [FromBody] AddPlayerDataBySportinstituationDto PlayerData)
        {
            //userprofileId how enter data "Sportinstituation how add it "
            var selectSportinstituation = _context.SportInstitutions.Where(x => x.SportInstitutionId == sportinstiuationid).FirstOrDefault();
            if (selectSportinstituation == null)
                return NotFound("لم يتم العثور على الاكاديمية الذى ادخلتها!");

            var selectSportinstituationbranch = _context.SportInstitutionBranches.Where(x => x.SportInstitutionBranchId == PlayerData.SportinstituationbranchId).FirstOrDefault();
            if (selectSportinstituationbranch == null)
                return NotFound("لم يتم العثور على الفرع الذى ادخلته!");

            if (selectSportinstituation.UserProfileId == userprofileId)
                return BadRequest("لا يمكنك اضافة هذة البيانات!");

            var enteredUser = _context.UserProfiles.Where(x => x.UserProfileId == PlayerData.UserProfileId && x.RoleId == 2).FirstOrDefault();
            if (enteredUser == null)
                return NotFound("لم يتم العثور على هذا اللاعب!");

            var checkExistingPlayer = _context.Players.Where(x => x.UserProfileId == PlayerData.UserProfileId && x.SeasonId == PlayerData.SeasonId && x.SportInstitutionBranchId == PlayerData.SportinstituationbranchId).FirstOrDefault();
            if (checkExistingPlayer != null)
                return Ok("تم اضافة بيانات هذا اللاعب لنفس الموسم مسبقا!");

            var newPlayer = new Player
            {
                UserProfileId = PlayerData.UserProfileId,
                SeasonId = PlayerData.SeasonId,
                PlayerTypeId = PlayerData.PlayerTypeId,
                MainPlaceId = PlayerData.MainPlaceId,
                PlayerNumber = PlayerData.PlayerNumber,
                PlayerFeetId = PlayerData.PlayerFeetId,
                SportInstitutionBranchId = PlayerData.SportinstituationbranchId
            };
            if (PlayerData.AlternativePlaceId != null)
                newPlayer.AlternativePlaceId = PlayerData.AlternativePlaceId;

            _context.Players.Add(newPlayer);
            _context.SaveChanges();

            return Ok("تم الاضافة بنجاح");
        }


        //////////////////////////////////////////////////////////////////

        [HttpPut(Name = "UpdateRequestForFriendlyMatch")]
        public async Task<IActionResult> UpdateRequestForFriendlyMatch(long FriendlyMatchId, [FromBody] FriendlyMatchDto matchDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var enteredFriendlyMatch = _context.MatchFriendlyMatches
                    .Include(m => m.MatchFriendlyMatchAgeCategories)
                    .Where(x => x.FriendlyMatchId.Equals(FriendlyMatchId))
                    .FirstOrDefault();
                    if (enteredFriendlyMatch == null) return BadRequest("المباراه الذى ادخلتها غير موجودة");

                    var enteredPlayground = _context.Playgrounds.Where(x => x.PlaygroundId.Equals(matchDto.PlayGroundId)).FirstOrDefault();
                    if (enteredPlayground == null) return BadRequest("الملعب الذى ادخلتة غير موجود");

                    _mapper.Map(matchDto, enteredFriendlyMatch);
                    _context.MatchFriendlyMatches.Update(enteredFriendlyMatch);


                    _context.MatchFriendlyMatchAgeCategories.RemoveRange(enteredFriendlyMatch.MatchFriendlyMatchAgeCategories);
                    if (matchDto.AgeCategoryId != null)
                    {
                        foreach (var x in matchDto.AgeCategoryId)
                        {
                            var newAgeCategory = new MatchFriendlyMatchAgeCategory
                            {
                                AgeCategoryId = x,
                                UserprofileId = matchDto.UserProfileId,
                                FriendlyMatchId = FriendlyMatchId,
                            };
                            _context.MatchFriendlyMatchAgeCategories.Add(newAgeCategory);
                        }
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                    return Ok("تم الاضافة بنجاح");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        // update to PlayerUserProfileId not to be payerid (done)
        [HttpPut(Name = "prepareTeamPlayer")]
        public async Task<IActionResult> prepareTeamPlayer(long matchId, long teamId, [FromBody] MatchPreparationPlayer updateDtos)
        {
            using (var Transiction = _context.Database.BeginTransaction())
            {
                try
                {
                    var match = await _context.Matches.FirstOrDefaultAsync(mp => mp.MatchId == matchId);
                    if (match == null) return NotFound("لم يتم العثور على الماتش");

                    foreach (var updateDto in updateDtos.MatchPlayersprepare)
                    {
                        var matchPlayer = await _context.MatchPlayers
                            .FirstOrDefaultAsync(mp => mp.MatchId == matchId && mp.TeamId == teamId && mp.PlayerUserProfileId == updateDto.PlayerUserProfileId);

                        if (matchPlayer != null)
                        {
                            matchPlayer.IsPrepare = updateDto.IsPrepare;
                        }
                        _context.Update(matchPlayer);
                    }

                    await _context.SaveChangesAsync();
                    Transiction.Commit();

                    return Ok("تم التحضير بنجاح ");
                }
                catch (Exception ex)
                {
                    Transiction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }


        //start and end all halves by select (1 or 2 or 3 or 4)
        [HttpPut(Name = "UpdateMatchHalves")]
        public IActionResult UpdateMatchHalves(long matchId, byte Determine1234ForUpdate)
        {
            //start first half"start match"  1
            //end first half    2
            //start second half 3
            //end seconf half "endmatch" 4
            try
            {
                var match = _context.Matches.Where(mp => mp.MatchId == matchId).FirstOrDefault();
                if (match == null) return NotFound("لم يتم العثور على الماتش");

                //start Match
                if (Determine1234ForUpdate == 1)
                {
                    match.IsStart = true;
                    match.MatchStartTime = DateTime.Now;
                }
                if (Determine1234ForUpdate == 2)
                {
                    match.FirstHalfEndTime = DateTime.Now;
                }
                if (Determine1234ForUpdate == 3)
                {
                    match.SecondHalfStartTime = DateTime.Now;
                }
                if (Determine1234ForUpdate == 4)
                {
                    match.IsEnd = true;
                    match.SecondHalfEndTime = DateTime.Now;
                }

                _context.Matches.Update(match);
                _context.SaveChanges();
                return Ok("تم التحديث بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        //wastedTime for 2 halves by select 1 for first halv  and 2 for second
        [HttpPut(Name = "Determine12HaveForUpdateLostTime")]
        public IActionResult Determine12HaveForUpdateLostTime(long matchId, int Determine12HaveForUpdateLostTime, int LostTime)
        {
            //FIRST have wasted time  1
            //second have wasted time  2
            try
            {
                var match = _context.Matches.Where(mp => mp.MatchId == matchId).FirstOrDefault();
                if (match == null) return NotFound("لم يتم العثور على الماتش");

                //start Match
                if (Determine12HaveForUpdateLostTime == 1)
                {

                    match.FirstHalfWastedTime = (byte?)LostTime;
                }
                if (Determine12HaveForUpdateLostTime == 2)
                {
                    match.SecondHalfWastedTime = (byte?)LostTime;
                }

                _context.Matches.Update(match);
                _context.SaveChanges();

                return Ok("تم التحديث بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPut("UpdateMatch")]
        public IActionResult UpdateMatch(long matchId, long userProfileId, [FromBody] AddMatchDto matchDto)
        {
            try
            {
                #region UpdateMatch
                var existingMatch = _context.Matches.Where(x => x.MatchId == matchId).FirstOrDefault();
                if (existingMatch == null)
                    return NotFound("المباراة التي تريد تحديثها غير موجودة");

                var entereduser = _context.UserProfiles.Where(x => x.UserProfileId.Equals(userProfileId)).FirstOrDefault();
                if (entereduser == null)
                    return BadRequest("المستخدم الذى ادخلتة غير موجود!");

                existingMatch.MatchTypeId = matchDto.MatchTypeId;
                existingMatch.MatchDate = matchDto.MatchDate;
                existingMatch.HomeTeamId = matchDto.HomeTeamId;
                existingMatch.AwayTeamId = matchDto.AwayTeamId;
                existingMatch.NumberOfPlayer = matchDto.NumberOfPlayer;
                existingMatch.HalfTimeBreak = matchDto.HalfTimeBreak;
                existingMatch.MatchTime = matchDto.MatchTime;
                existingMatch.UserProfileId = userProfileId;
                _context.Matches.Update(existingMatch);
                _context.SaveChanges();
                return Ok("تم تحديث المباراة بنجاح");
                #endregion
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut(Name = "UpdateCard")]
        public IActionResult UpdateCard(long CardId, long userProfileidWhoEnterData, [FromBody] AddCardDto cardDto)
        {
            var card = _context.MatchCards.Where(x => x.MatchCardId == CardId).FirstOrDefault();
            if (card == null)
                return NotFound("لم يتم العثور على هذا الكارت !");

            var dataentryUser = _context.UserProfiles.Where(x => x.UserProfileId == userProfileidWhoEnterData).FirstOrDefault();
            if (dataentryUser == null)
                return NotFound("لم يتم العثور على مدخل البيانات  !");

            var player = _context.UserProfiles.Where(x => x.UserProfileId == cardDto.PlayerUserProfileId).FirstOrDefault();
            if (player == null)
                return NotFound("لم يتم العثور على اللاعب!");


            card.PlayerUserProfileId = cardDto.PlayerUserProfileId;
            card.MatchCardTypeId = cardDto.MatchCardTypeId;
            card.Minte = cardDto.Minte;
            card.Reason = cardDto.Reason;

            _context.MatchCards.Update(card);
            _context.SaveChanges();
            return Ok("تم التعديل بنجاح");
        }


        [HttpPut(Name = "UpdateSubstitution")]
        public IActionResult UpdateSubstitution(long SubstitutionId, long userProfileidWhoEnterData, [FromBody] MatchSubstitutionDto substitutionDto)
        {

            var Substitution = _context.MatchSubstitutions.Where(x => x.MatchSubstitutionsId == SubstitutionId).FirstOrDefault();
            if (Substitution == null)
                return NotFound("لم يتم العثور على هذا التبديل!");

            var dataentryUser = _context.UserProfiles.Where(x => x.UserProfileId == userProfileidWhoEnterData).FirstOrDefault();
            if (dataentryUser == null)
                return NotFound("لم يتم العثور على مدخل البيانات  !");

            var playerIN = _context.UserProfiles.Where(x => x.UserProfileId == substitutionDto.PlayerInUserProfileId).FirstOrDefault();
            if (playerIN == null)
                return NotFound("لم يتم العثور على اللاعب البديل!");

            var playerOut = _context.UserProfiles.Where(x => x.UserProfileId == substitutionDto.PlayerOutUserProfileId).FirstOrDefault();
            if (playerOut == null)
                return NotFound(" لم يتم العثور على اللاعب المستبدل!");



            Substitution.PlayerInUserProfileId = substitutionDto.PlayerInUserProfileId;
            Substitution.PlayerOutUserProfileId = substitutionDto.PlayerOutUserProfileId;
            Substitution.Minite = substitutionDto.Minite;
            Substitution.Reason = substitutionDto.Reason;

            _context.MatchSubstitutions.Update(Substitution);
            _context.SaveChanges();
            return Ok("تمت التعديل بنجاح");
        }


        [HttpPut(Name = "UpdateMatchScore")]
        public IActionResult UpdateMatchScore(long MatchScoreId, long UserProfileId, [FromBody] AddMatchScore matchScore)
        {
            var matchScoree = _context.MatchScores.Where(x => x.MatchScoreId == MatchScoreId).FirstOrDefault();
            if (matchScoree == null)
                return NotFound("لم يتم العثور على الهدف !");

            var userProfile = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileId).FirstOrDefault();
            if (userProfile == null)
                return NotFound("لم يتم العثور على هذا المستخدم!");

            var player = _context.MatchPlayers.Where(x => x.PlayerId == matchScore.PlayerUserProfileId && x.TeamId == matchScore.TeamId).FirstOrDefault();
            return BadRequest("هذا اللاعب ليس فى تشكيلة هذا الفريق ! ");

            matchScoree.GoalMethodId = matchScore.GoalMethodId;
            matchScore.PlayerUserProfileId = matchScore.PlayerUserProfileId;
            matchScore.TeamId = matchScore.TeamId;
            matchScore.GoalTime = matchScore.GoalTime;
            matchScoree.LastUpdate = DateTime.Now;

            _context.MatchScores.Update(matchScoree);
            _context.SaveChanges();
            return Ok("تمت التحديث بنجاح");
        }


        [HttpPut(Name = "UpdateMatchUniform")]
        public async Task<IActionResult> UpdateMatchUniform(long MatchUniformId, [FromBody] AddMatchUniformDto matchUniformDTO)
        {
            try
            {
                var matchUniform = await _context.MatchUniforms.Where(x => x.MatchUniformId == MatchUniformId).FirstOrDefaultAsync();
                if (matchUniform == null)
                    return NotFound("لم يتم العثور على الزى  !");

                var userprofile = await _context.UserProfiles.FindAsync(matchUniformDTO.UserProfileId);
                if (userprofile == null)
                    return NotFound("لم يتم العثور على هذا المستخدم!");

                var team = await _context.SportInstitutions.Where(x => x.SportInstitutionId == matchUniformDTO.TeamId).FirstOrDefaultAsync();
                if (team == null)
                    return NotFound($"لم يتم العثور على الفريق: {matchUniformDTO.TeamId}");

                var MapTomodel = _mapper.Map(matchUniformDTO, matchUniform);
                //_context.MatchUniforms.Update.(matchUniform);
                await _context.SaveChangesAsync();
                return Ok("تم تحديث الاطقم بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut(Name = "UpdateUniformToSpeceficTeam")]
        public async Task<IActionResult> UpdateUniformToSpeceficTeam(long Uniformid, long userprofileId, [FromForm] uniformDto uniform)
        {
            try
            {
                var existingUniform = await _context.Uniforms.FindAsync(Uniformid);
                if (existingUniform == null)
                    return NotFound("لم يتم العثور على الزى الرياضى!");

                var userProfile = _context.UserProfiles.Where(x => x.UserProfileId == userprofileId).FirstOrDefault();
                if (userProfile == null)
                    return NotFound("لم يتم العثور على هذا المستخدم!");

                if (userprofileId != existingUniform.UserProfileId)
                    return BadRequest("لا يمكنك التعديل !");

                var uniformtype = await _context.UniformUniformTypes.FindAsync(uniform.UniformTypeId);
                if (uniformtype == null)
                    return NotFound("لم يتم العثور على نوع الطقم الذى ادخلتة!");

                var checkuniform = await _context.Uniforms.FirstOrDefaultAsync(x => x.TeamId == existingUniform.TeamId && x.UniformTypeId == uniform.UniformTypeId && x.UniformId != Uniformid);
                if (checkuniform != null)
                {
                    var selectedUnifoem = _context.UniformUniformTypes.FirstOrDefault(x => x.UniformTypeId == uniform.UniformTypeId).UniformType;
                    return Conflict($"تم اختيار {selectedUnifoem} من قبل ");
                }

                if (uniform.Uniformimage != null)
                {
                    string selectoldPathImage = null;
                    var imageName = _context.Images.Where(x => x.ImageId == existingUniform.UniformImageId).FirstOrDefault();
                    if (imageName != null)
                        selectoldPathImage = imageName.ImageFileName;

                    var user = _context.Users.Where(x => x.UserId == userProfile.UserId).FirstOrDefault();

                    var oldpath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\SportInstitution\{user.Mobile}\" + selectoldPathImage;

                    System.IO.File.Delete(oldpath);


                    var englishFileName = uniform.Uniformimage.FileName.Unidecode();
                    var photopath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\SportInstitution\{existingUniform.UserProfile.User.Mobile}\" + englishFileName;
                    using (var stream = new FileStream(photopath, FileMode.Create))
                    {
                        await uniform.Uniformimage.CopyToAsync(stream);
                    }
                    var newPath = $"http://mobile.hawisports.com/image/SportInstitution/{existingUniform.UserProfile.User.Mobile}/" + englishFileName;

                    var existingImage = await _context.Images.FindAsync(existingUniform.UniformImageId);
                    if (existingImage == null)
                        throw new Exception("حدث خطأ أثناء تحديث الزى الرياضى!");

                    existingImage.ImageUrlfullPath = newPath;
                    existingImage.ImageFileName = englishFileName;
                    existingImage.ImageTypeId = (uniform.UniformTypeId == 1) ? (byte?)6 : (uniform.UniformTypeId == 2) ? (byte?)7 : (byte?)8;
                    _context.SaveChanges();
                }

                existingUniform.UniformTypeId = uniform.UniformTypeId;
                await _context.SaveChangesAsync();

                return Ok("تم تحديث الزى الرياضى بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut(Name = "UpdateMatchReferee")]
        public IActionResult UpdateMatchReferee(long matchId, [FromBody] MatchRefereeDto refereeDto)
        {
            var match = _context.Matches.FirstOrDefault(x => x.MatchId == matchId);
            if (match == null)
                return NotFound("لم يتم العثور على الماتش!");

            var existingMatchReferee = _context.MatchReferees.FirstOrDefault(x => x.MatchId == matchId);
            if (existingMatchReferee == null)
                return NotFound("لم يتم تعيين حكام لهذا الماتش!");

            var mainReferee = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == refereeDto.MainRefereeId && x.RoleId == 3);
            if (mainReferee == null)
                return NotFound("لم يتم العثور على الحكم الأول للماتش!");

            if (refereeDto.Assistant1RefereeId != null)
            {
                var assistant1Referee = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == refereeDto.Assistant1RefereeId && x.RoleId == 3);
                if (assistant1Referee == null)
                    return NotFound("لم يتم العثور على الحكم المساعد الأول للماتش!");
            }

            if (refereeDto.Assistant2RefereeId != null)
            {
                var assistant2Referee = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == refereeDto.Assistant2RefereeId && x.RoleId == 3);
                if (assistant2Referee == null)
                    return NotFound("لم يتم العثور على الحكم المساعد التاني للماتش!");
            }

            if (refereeDto.FourthRefereeId != null)
            {
                var fourthReferee = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == refereeDto.FourthRefereeId && x.RoleId == 3);
                if (fourthReferee == null)
                    return NotFound("لم يتم العثور على الحكم الرابع للماتش!");
            }

            existingMatchReferee.MainRefereeId = refereeDto.MainRefereeId;
            existingMatchReferee.Assistant1RefereeId = refereeDto.Assistant1RefereeId;
            existingMatchReferee.Assistant2RefereeId = refereeDto.Assistant2RefereeId;
            existingMatchReferee.FourthRefereeId = refereeDto.FourthRefereeId;
            existingMatchReferee.ResidentRefereeId = refereeDto.ResidentRefereeId;
            existingMatchReferee.SupervisingRefereeId = refereeDto.SupervisingRefereeId;

            _context.SaveChanges();

            return Ok("تم التحديث بنجاح");
        }


        [HttpPut(Name = "UpdatePlayerDataBySportinstituation")]
        public IActionResult UpdatePlayerDataBySportinstituation(long sportinstiuationid, long SportinstituationuserprofileId, long playerId, [FromBody] AddPlayerDataBySportinstituationDto PlayerData)
        {
            //userprofileId how enter data "Sportinstituation how add it "
            var selectSportinstituation = _context.SportInstitutions.Where(x => x.SportInstitutionId == sportinstiuationid).FirstOrDefault();
            if (selectSportinstituation == null)
                return NotFound("لم يتم العثور على الاكاديمية الذى ادخلتها!");

            var selectSportinstituationbranch = _context.SportInstitutionBranches.Where(x => x.SportInstitutionBranchId == PlayerData.SportinstituationbranchId).FirstOrDefault();
            if (selectSportinstituationbranch == null)
                return NotFound("لم يتم العثور على الفرع الذى ادخلته!");

            if (selectSportinstituation.UserProfileId == SportinstituationuserprofileId)
                return BadRequest("لا يمكنك التعديل على هذة البيانات!");

            var enteredUser = _context.UserProfiles.Where(x => x.UserProfileId == PlayerData.UserProfileId && x.RoleId == 2).FirstOrDefault();
            if (enteredUser == null)
                return NotFound("لم يتم العثور على هذا اللاعب!");


            var checkExistingPlayer = _context.Players.Where(x => x.PlayerId == playerId).FirstOrDefault();
            if (checkExistingPlayer != null)
                return Ok("لم يتم اضافة اى بيانات لهذا اللاعب مسبقا");


            checkExistingPlayer.UserProfileId = PlayerData.UserProfileId;
            checkExistingPlayer.SeasonId = PlayerData.SeasonId;
            checkExistingPlayer.PlayerTypeId = PlayerData.PlayerTypeId;
            checkExistingPlayer.MainPlaceId = PlayerData.MainPlaceId;
            checkExistingPlayer.PlayerNumber = PlayerData.PlayerNumber;
            checkExistingPlayer.PlayerFeetId = PlayerData.PlayerFeetId;
            checkExistingPlayer.AlternativePlaceId = PlayerData.AlternativePlaceId;
            checkExistingPlayer.SportInstitutionBranchId = PlayerData.SportinstituationbranchId;

            _context.Players.Update(checkExistingPlayer);
            _context.SaveChanges();

            return Ok("تم التعديل بنجاح");
        }

        /////////////////////////////////////

        [HttpDelete(Name = "DeleteMatch")]
        public IActionResult DeleteMatch(long matchId, long UserProfileId)
        {
            var match = _context.Matches.Where(x => x.MatchId == matchId).FirstOrDefault();
            if (match == null)
                return NotFound("لم يتم العثور على الماتش!");

            var userprofile = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileId).FirstOrDefault();
            if (userprofile == null)
                return NotFound("لم يتم العثور على المستخدم!");
            _context.Matches.Remove(match);
            _context.SaveChanges();
            return Ok("تم الحذف بنجاح");
        }

        [HttpDelete(Name = "DeleteFriendlyMatch")]
        public IActionResult DeleteFriendlyMatch(long FriendlyMatchId, long UserProfileId)
        {
            var enteredFriendlyMatch = _context.MatchFriendlyMatches
                   .Where(x => x.FriendlyMatchId.Equals(FriendlyMatchId))
                   .FirstOrDefault();

            if (enteredFriendlyMatch == null) return BadRequest("المباراه الذى ادخلتها غير موجودة");
            if (enteredFriendlyMatch.UserProfileId != UserProfileId) return Unauthorized("ليس لديك الصلاحية لحذف  هذة المباراه!");

            enteredFriendlyMatch.IsDeleted = true;
            enteredFriendlyMatch.IsActive = false;
            _context.SaveChanges();
            return Ok("تم الحذف بنجاح");
        }

        [HttpDelete(Name = "DeleteApplicantsSportInstituationForFriendlyMatch")]
        public async Task<IActionResult> DeleteApplicantsSportInstituationForFriendlyMatch(long ApplicantsSportInstituationId, long SportInstituationId)
        {
            try
            {
                var selectedApplicant = _context.MatchApplicantsSportInstituationForFriendlyMatches
                   .Where(x => x.ApplicantsSportInstituationId.Equals(ApplicantsSportInstituationId))
                   .FirstOrDefault();

                if (selectedApplicant == null) return BadRequest("!لم يتم العثور على هذا الطلب");
                //if (selectedApplicant.SportInstituationId != SportInstituationId  ) return Unauthorized("ليس لديك الصلاحية لهذا الحذف");

                _context.MatchApplicantsSportInstituationForFriendlyMatches.Remove(selectedApplicant);
                _context.SaveChanges();

                return Ok("تم الحذف بنجاح");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete(Name = "DeleteUniformToSpeceficTeam")]
        public async Task<IActionResult> DeleteUniformToSpeceficTeam(long Uniformid, long userprofileId)
        {
            try
            {
                var existingUniform = await _context.Uniforms.FindAsync(Uniformid);
                if (existingUniform == null)
                    return NotFound("لم يتم العثور على الزى الرياضى!");

                var userProfile = _context.UserProfiles.Where(x => x.UserProfileId == userprofileId).FirstOrDefault();
                if (userProfile == null)
                    return NotFound("لم يتم العثور على هذا المستخدم!");

                if (userprofileId != existingUniform.UserProfileId)
                    return BadRequest("لا يمكنك الحذف !");

                var existingImage = await _context.Images.FindAsync(existingUniform.UniformImageId);
                if (existingImage == null)
                    throw new Exception("حدث خطأ أثناء حذف الزى الرياضى!");


                string selectoldPathImage = null;
                var imageName = _context.Images.Where(x => x.ImageId == existingUniform.UniformImageId).FirstOrDefault();
                if (imageName != null)
                    selectoldPathImage = imageName.ImageFileName;

                var user = _context.Users.Where(x => x.UserId == userProfile.UserId).FirstOrDefault();

                var imagePath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\SportInstitution\{user.Mobile}\" + selectoldPathImage;
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);


                _context.Uniforms.Remove(existingUniform);
                _context.SaveChanges();
                _context.Images.Remove(existingImage);
                _context.SaveChanges();



                return Ok("تم حذف الزى الرياضى بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest("لا يمكن حذف هذا الزى لانه مستخدم كزى للفريق فى ماتش !");
            }
        }

        [HttpDelete(Name = "DeleteMatchUniform")]
        public IActionResult DeleteMatchUniform(long matchId, long UserProfileId)
        {
            var match = _context.Matches.Where(x => x.MatchId == matchId).FirstOrDefault();
            if (match == null)
                return NotFound("لم يتم العثور على الماتش !");

            var uniform = _context.MatchUniforms.Where(x => x.MatchId == matchId).ToList();
            if (uniform == null)
                return NotFound("لم يتم العثور على اى اطقم لهذة المبارة! ");
            _context.MatchUniforms.RemoveRange(uniform);
            _context.SaveChanges();
            return Ok("تم الحذف بنجاح");
        }

        [HttpDelete(Name = "DeleteMatchReferee")]
        public IActionResult DeleteMatchReferee(long matchId, long userprofileId)
        {
            var match = _context.Matches.Where(x => x.MatchId == matchId).FirstOrDefault();
            if (match == null)
                return NotFound("لم يتم العثور على الماتش !");

            var matchReferees = _context.MatchReferees.Where(x => x.MatchId == matchId).FirstOrDefault();
            if (matchReferees == null)
                return NotFound("لم يتم تعيين حكام لهذا الماتش !");

            _context.MatchReferees.Remove(matchReferees);
            _context.SaveChanges();
            return Ok("تم الحذف بنجاح");
        }

        //[HttpDelete(Name = "DeleteMatchCoach")]
        //public IActionResult DeleteMatchCoach(long MatchId)
        //{
        //    var match = _context.Matches.Where(x => x.MatchId == MatchId).FirstOrDefault();
        //    if (match == null)
        //        return NotFound("لم يتم العثور على الماتش !");

        //    var MatchCoach = _context.MatchCoaches.Where(x => x.MatchId == MatchId).FirstOrDefault();
        //    if (MatchCoach == null)
        //        return NotFound("لم يتم تعيين مدربين لهذا الماتش !");



        //    _context.MatchCoaches.Remove(MatchCoach);
        //    _context.SaveChanges();
        //    return Ok("تم الحذف بنجاح");
        //}

        [HttpDelete(Name = "DeleteMatchScore")]
        public IActionResult DeleteMatchScore(long MatchScoreId, long UserProfileId)
        {
            var matchScoree = _context.MatchScores.Where(x => x.MatchScoreId == MatchScoreId).FirstOrDefault();
            if (matchScoree == null)
                return NotFound("لم يتم العثور على الهدف !");

            var userProfile = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileId).FirstOrDefault();
            if (userProfile == null)
                return NotFound("لم يتم العثور على هذا المستخدم!");

            _context.MatchScores.Remove(matchScoree);
            _context.SaveChanges();
            return Ok("تمت الحذف بنجاح");
        }

        [HttpDelete(Name = "DeleteMatchFormationOfTeams")]
        public IActionResult DeleteMatchFormationOfTeams(long MatchId, long SportInstitutionId1, long? SportInstitutionId2 = null)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var matchPlayers1 = _context.MatchPlayers.Where(x => x.MatchId == MatchId && (x.TeamId == SportInstitutionId1)).ToList();
                    if (matchPlayers1 == null || matchPlayers1.Count == 0)
                        return NotFound("لم يتم العثور على تشكيل المباراة للفريق الاول!");

                    _context.MatchPlayers.RemoveRange(matchPlayers1);

                    if (SportInstitutionId2 != null)
                    {
                        var matchPlayers2 = _context.MatchPlayers.Where(x => x.MatchId == MatchId && (x.TeamId == SportInstitutionId2)).ToList();
                        if (matchPlayers2 == null || matchPlayers2.Count == 0)
                            return NotFound("لم يتم العثور على تشكيل المباراة للفريق الثانى!");

                        _context.MatchPlayers.RemoveRange(matchPlayers2);
                    }

                    _context.SaveChanges();

                    transaction.Commit();

                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest("فشل حذف تشكيل المباراة. يرجى التحقق من البيانات المدخلة.");
                }
            }
        }

        [HttpDelete(Name = "DeleteCard")]
        public IActionResult DeleteCard(long CardId, long userProfileidWhoEnterData)
        {
            var card = _context.MatchCards.Where(x => x.MatchCardId == CardId).FirstOrDefault();
            if (card == null)
                return NotFound("لم يتم العثور على هذا الكارت !");

            var dataentryUser = _context.UserProfiles.Where(x => x.UserProfileId == userProfileidWhoEnterData).FirstOrDefault();
            if (dataentryUser == null)
                return NotFound("لم يتم العثور على مدخل البيانات  !");

            _context.MatchCards.Remove(card);
            _context.SaveChanges();
            return Ok("تم الحذف بنجاح");
        }

        [HttpDelete(Name = "DeleteSubstitution")]
        public IActionResult DeleteSubstitution(long SubstitutionId, long userProfileidWhoEnterData)
        {

            var Substitution = _context.MatchSubstitutions.Where(x => x.MatchSubstitutionsId == SubstitutionId).FirstOrDefault();
            if (Substitution == null)
                return NotFound("لم يتم العثور على هذا التبديل!");

            var dataentryUser = _context.UserProfiles.Where(x => x.UserProfileId == userProfileidWhoEnterData).FirstOrDefault();
            if (dataentryUser == null)
                return NotFound("لم يتم العثور على مدخل البيانات  !");

            // الغاء التبديل فى matchplayer  بواسطة الحكم والمدرب
            //player In
            var MatchPlayerInSubstitution = _context.MatchPlayers
                .Where(x => x.MatchId == Substitution.MatchId && x.TeamId == Substitution.TeamId &&
                x.PlayerUserProfileId == Substitution.PlayerInUserProfileId)
                .FirstOrDefault();

            if (MatchPlayerInSubstitution != null)
            {
                MatchPlayerInSubstitution.IsRefereeSubstituted = false;
                MatchPlayerInSubstitution.IsCoachSubstituted = false;
            }

            //player out
            var MatchPlayerOutSubstitution = _context.MatchPlayers
               .Where(x => x.MatchId == Substitution.MatchId && x.TeamId == Substitution.TeamId &&
               x.PlayerUserProfileId == Substitution.PlayerOutUserProfileId)
               .FirstOrDefault();

            if (MatchPlayerOutSubstitution != null)
            {
                MatchPlayerOutSubstitution.IsRefereeSubstituted = false;
                MatchPlayerOutSubstitution.IsCoachSubstituted = false;
            }
            _context.MatchSubstitutions.Remove(Substitution);
            _context.SaveChanges();
            return Ok("تمت الحذف بنجاح");
        }
    }
}

