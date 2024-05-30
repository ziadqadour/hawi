using AutoMapper;
using Hawi.Dtos;
using Hawi.Models;

namespace Hawi.Extensions
{
    public class MappingProfile:Profile
    {
    
        public MappingProfile()
        {

            #region champoinShip

                CreateMap<CreateChampionshipDTO, Championship>()
                           //DB                                             //DTO
                .ForMember(dest => dest.UserProfileId, opt => opt.MapFrom(src => src.UserProfileId))
                .ForMember(dest => dest.ChampionshipsName, opt => opt.MapFrom(src => src.ChampionshipsName))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.NumTeams, opt => opt.MapFrom(src => src.NumTeams))
                .ForMember(dest => dest.TargetedCategoriesId, opt => opt.MapFrom(src => src.TargetedCategoriesId))
                .ForMember(dest => dest.IsMale, opt => opt.MapFrom(src => src.IsMale));

                CreateMap<CreateChampionshipDTO, ChampionshipSystem>()
               .ForMember(dest => dest.UserProfileId, opt => opt.MapFrom(src => src.UserProfileId))
               .ForMember(dest => dest.ChampionshipSystemOptionsId, opt => opt.MapFrom(src => src.ChampionshipSystemOptionsId))
               .ForMember(dest => dest.Substitutions, opt => opt.MapFrom(src => src.Substitutions))
               .ForMember(dest => dest.BreakTime, opt => opt.MapFrom(src => src.BreakTime))
               .ForMember(dest => dest.MatchDuration, opt => opt.MapFrom(src => src.MatchDuration))
               .ForMember(dest => dest.ChampionshipId, opt => opt.MapFrom(src => src.ChampionshipsId));

               CreateMap<CreateChampionshipDTO, ChampionshipSystemOptionsLeagueSystem>()
              .ForMember(dest => dest.IsLeague, opt => opt.MapFrom(src => src.IsLeague))
              .ForMember(dest => dest.ChampionshipSystemId, opt => opt.MapFrom(src => src.ChampionshipSystemId));

               CreateMap<CreateChampionshipDTO, ChampionshipSystemOptionsEliminationSystem>()
              .ForMember(dest => dest.HasExtraTime, opt => opt.MapFrom(src => src.HasExtraTime))
              .ForMember(dest => dest.ChampionshipSystemId, opt => opt.MapFrom(src => src.ChampionshipSystemId));

              CreateMap<CreateChampionshipDTO, ChampionshipSystemOptionsMixedLeagueSystem>()
             .ForMember(dest => dest.NumOfGroups, opt => opt.MapFrom(src => src.NumOfGroups))
             .ForMember(dest => dest.TeamsPerGroup, opt => opt.MapFrom(src => src.TeamsPerGroup))
             .ForMember(dest => dest.QualifiersNumberFromEachGroup, opt => opt.MapFrom(src => src.QualifiersNumberFromEachGroup))
             .ForMember(dest => dest.ChampionshipSystemId, opt => opt.MapFrom(src => src.ChampionshipSystemId));

             CreateMap<CreatePlayGroundDTO, Playground>()
            .ForMember(dest => dest.UserProfileId, opt => opt.MapFrom(src => src.UserProfileId))
            .ForMember(dest => dest.PlaygroundName, opt => opt.MapFrom(src => src.PlaygroundName))
            .ForMember(dest => dest.PlaygroundLocation, opt => opt.MapFrom(src => src.PlaygroundLocation))
            .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId))
            .ForMember(dest => dest.PlaygroundFloorId, opt => opt.MapFrom(src => src.PlaygroundFloorId))
            .ForMember(dest => dest.PlaygroundSizeId, opt => opt.MapFrom(src => src.PlaygroundSizeId))
            .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImagePath));

           #region for ChampionshipsPlayGround
              CreateMap<CreateChampionshipDTO, ChampionshipsPlayGround>()
             .ForMember(dest => dest.ChampionshipId, opt => opt.MapFrom(src => src.ChampionshipsId))
             .ForMember(dest => dest.UserProfileId, opt => opt.MapFrom(src => src.UserProfileId))
             .ForMember(dest => dest.PlayGroundId, opt => opt.Ignore()); // Ignore PlayGroundId for now

              CreateMap<CreateChampionshipDTO, List<ChampionshipsPlayGround>>()
               .ConvertUsing((src, dest, context) =>
               {
                        var championshipPlayGrounds = new List<ChampionshipsPlayGround>();

                        for (int i = 0; i < src.PlayGroundId.Count; i++)
                        {
                            var championshipsPlayGround = context.Mapper.Map<CreateChampionshipDTO, ChampionshipsPlayGround>(src);
                            championshipsPlayGround.PlayGroundId = src.PlayGroundId[i];
                            championshipPlayGrounds.Add(championshipsPlayGround);
                        }

                        return championshipPlayGrounds;
               });
           #endregion

           #region for ChampionshipTeam
                CreateMap<CreateChampionshipsTeamsDTO, ChampionshipTeam>()
               .ForMember(dest => dest.ChampionshipId, opt => opt.MapFrom(src => src.ChampionshipsId))
               .ForMember(dest => dest.UserProfileId, opt => opt.MapFrom(src => src.UserProfileId))
               .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
               .ForMember(dest => dest.TeamId, opt => opt.Ignore()); 

                CreateMap<CreateChampionshipsTeamsDTO, List<ChampionshipTeam>>()
                  .ConvertUsing((src, dest, context) =>
                  {
                      var ChampionshipTeams = new List<ChampionshipTeam>();

                      for (int i = 0; i < src.TeamId.Count; i++)
                      {
                          var championshipTeam = context.Mapper.Map<CreateChampionshipsTeamsDTO, ChampionshipTeam>(src);
                          championshipTeam.TeamId = src.TeamId[i];
                          ChampionshipTeams.Add(championshipTeam);
                      }

                      return ChampionshipTeams;
                  });
            #endregion

            CreateMap<CreateChampionshipsMatchDTO, ChampionshipMatch>();
            CreateMap<CreateChampionshipsMatchDTO, ChampionshipMatch>();
            #endregion

            // match
            CreateMap<AddAddChampionshipMathcRefereeDto, ChampionshipReferee>();
            CreateMap<MatchRefereeDto, MatchReferee>();
            CreateMap<MatchTechnicalTeamDto, MatchTechnicalTeam>();
            CreateMap<AddMatchUniformDto, MatchUniform>();
            CreateMap<FriendlyMatchDto, MatchFriendlyMatch>();
           


       
        }

    }
}
