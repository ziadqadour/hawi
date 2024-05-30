using AutoMapper;
using Hawi.Dtos;
using Hawi.Models;
using Microsoft.EntityFrameworkCore;

namespace Hawi.Extensions
{
    public class ChampionshipFunctions
    {
        //DbA874b7HawiContext _context = new DbA874b7HawiContext();
        private readonly IMapper _mapper;
        private readonly HawiContext _context ;

        public ChampionshipFunctions(IMapper mapper, HawiContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public  async Task AddChampionshipSystemAsync(CreateChampionshipDTO championship) 
        {
            if(championship.ChampionshipSystemOptionsId==1)
            {
                var newMixedLeague = new ChampionshipSystemOptionsMixedLeagueSystem
                {
                    NumOfGroups = championship.NumOfGroups,
                    TeamsPerGroup = championship.TeamsPerGroup,
                    QualifiersNumberFromEachGroup = championship.QualifiersNumberFromEachGroup,
                    ChampionshipSystemId = championship.ChampionshipSystemId,
                };

                await _context.ChampionshipSystemOptionsMixedLeagueSystems.AddAsync(newMixedLeague);
            }

            else if(championship.ChampionshipSystemOptionsId == 2)
            {
                var newEliminationSystem = new ChampionshipSystemOptionsEliminationSystem
                {
                    HasExtraTime=championship.HasExtraTime, 
                   ChampionshipSystemId = (long)championship.ChampionshipSystemId,
                };

                await _context.ChampionshipSystemOptionsEliminationSystems.AddAsync(newEliminationSystem);
            }

            else if (championship.ChampionshipSystemOptionsId == 3)
            {
                var newLeagueSystem = new ChampionshipSystemOptionsLeagueSystem
                {
                     IsLeague =championship.IsLeague,
                     ChampionshipSystemId = (long)championship.ChampionshipSystemId,
                };

               await _context.ChampionshipSystemOptionsLeagueSystems.AddAsync(newLeagueSystem);
            }

             
        }

        public string CheckChampionshipStatus(Championship championship, DateTime currentDate)
        {
            if (championship.ChampionshipTeams.Count() < championship.NumTeams ||
                championship.ChampionshipReferees.Count() == 0 ||
                championship.ChampionshipMatches.Count() != championship.NumTeams / 2)
            {
                return "غير مكتمل";
            }
            else if (championship.StartDate > currentDate)
            {
                return "مدخل";
            }
            return "جارية";
        }

        public async Task<Championship> GetChampionshipAsync(long championshipId)
        {
            return await _context.Championships
                .Include(c => c.ChampionshipSystems)
                .Include(c => c.ChampionshipAgeCategories)
                .Include(c => c.ChampionshipTeams)
                .Include(c => c.ChampionshipMatches)
                .Include(c => c.ChampionshipsPlayGrounds)
                .FirstOrDefaultAsync(c => c.ChampionshipsId == championshipId);
        }

        public async Task<List<AgeCategoryDto>> GetAgeCategoriesAsync(Championship championship)
        {
            var ChampionshipAgeCategories=  championship.ChampionshipAgeCategories.Select(ac => new AgeCategoryDto
            {
                Name = _context.SportInstitutionAgePriceAgeCategories.FirstOrDefault(x => x.AgeCategoryId == ac.AgeCategoryId).AgeCategory ?? null,
                AgeCategoryID = ac.AgeCategoryId,
            }).ToList();
            return ChampionshipAgeCategories;
        }

        public async Task<List<TeamDto>> GetTeamsAsync(Championship championship)
        {
            var ChampionshipTeams=  championship.ChampionshipTeams.Select(t => new TeamDto
            {
                TeamID = t.TeamId,
                Name = _context.SportInstitutions.FirstOrDefault(x => x.SportInstitutionId == t.TeamId).SportInstitutionName ?? null,
                Logo = _context.SportInstitutions.FirstOrDefault(x => x.SportInstitutionId == t.TeamId).LogoUrlfullPath ?? null
            }).ToList();
            return ChampionshipTeams;
        }

        public async Task<List<MatchDto>> GetMatchesAsync(Championship championship)
        {
            var ChampionshipMatches=  championship.ChampionshipMatches.Select<ChampionshipMatch, MatchDto>(m =>
            {
                var Selectmatch = _context.Matches.FirstOrDefault(x => x.MatchId == m.MatchId);
                return new MatchDto
                {
                    MatchID = m.MatchId,
                    HomeTeamID = Selectmatch.HomeTeamId,
                    HomeTeam = Selectmatch.HomeTeam?.SportInstitutionName,
                    HomeTeamLogo = Selectmatch.HomeTeam?.LogoUrlfullPath,
                    AwayTeamID = Selectmatch.AwayTeamId,
                    AwayTeam = Selectmatch.AwayTeam?.SportInstitutionName,
                    AwayTeamLogo = Selectmatch.AwayTeam?.LogoUrlfullPath,
                    Date = m.Match.MatchDate?.ToString("dd/MM/yyyy"),
                    Time = m.Match.MatchTime?.ToString(/*"h:mm tt"*/),
                };
            }).ToList();
            return ChampionshipMatches;
        }
        
        public List<string> GetPlaygroundsAsync(Championship championship)
        {
            var playgroundNames = championship.ChampionshipsPlayGrounds
                .Select(cp =>
                {
                    var playground = _context.Playgrounds.FirstOrDefault(x => x.PlaygroundId == cp.PlayGroundId);
                    if (playground != null)
                        return $"{playground.PlaygroundName}";
                    else
                        return null;
                })
                .ToList(); // Convert to a list synchronously

            return playgroundNames;
        }
       
        public string GetSystemType(ChampionshipSystem? championshipSystem)
        {
            if (championshipSystem == null)
                return null;
            

            var ChampionshipSystemOptionsLeagueSystems= championshipSystem.ChampionshipSystemOptionsLeagueSystems != null
                ? "نظام الدورى"
                : (championshipSystem.ChampionshipSystemOptionsMixedLeagueSystems != null
                    ? "النظام المختلط"
                    : (championshipSystem.ChampionshipSystemOptionsEliminationSystems != null
                        ? "نظام خروج المغلوب"
                        : null));
            return ChampionshipSystemOptionsLeagueSystems;
        }
       
        public async Task<ChampionshipDetailsDashboardDto> CreateChampionshipDashboard(Championship championship)
        {
            var ageCategories = await GetAgeCategoriesAsync(championship);
            var teams = await GetTeamsAsync(championship);
            var matches = await GetMatchesAsync(championship);
            var playgrounds = GetPlaygroundsAsync(championship);

            var championshipSystem = championship.ChampionshipSystems.FirstOrDefault();
            var systemType = GetSystemType(championshipSystem);

            var championshipDashboardDetails = new ChampionshipDetailsDashboardDto
            {
                ChampionshipId= championship.ChampionshipsId,
                ChampionshipName= championship.ChampionshipsName,
                DateRange = $"{championship.StartDate:dd/MM/yyyy} - {championship.EndDate:dd/MM/yyyy}",
                CityId = championship.CityId,
                City = _context.Cities.FirstOrDefault(x => x.CityId == championship.CityId)?.CityArabicName,
                Gender = championship.IsMale ? "ذكور" : "إناث" ,
                AgeCategories = ageCategories,
                NumTeams = championship.NumTeams,
                TargetedCategory = _context.SportInstitutionSportInstitutionTypes.FirstOrDefault(x => x.SportInstitutionTypeId == championship.TargetedCategoriesId)?.SportInstitutionType,
                MatchDuration = $"{championshipSystem?.MatchDuration} دقيقة",
                BreakBetweenHalves = $"{championshipSystem?.BreakTime} دقيقة",
                Substitutions = championshipSystem?.Substitutions,
                NumberOfFormation = championshipSystem?.NumberOfFormation,
                NumberOfPlayersOnList = championshipSystem?.NumberOfPlayersOnList,
                systemType = systemType,
                Playgrounds = playgrounds,
                Teams = teams,
                Matches = matches
            };

            return championshipDashboardDetails;
        }
    
    
    }
}
