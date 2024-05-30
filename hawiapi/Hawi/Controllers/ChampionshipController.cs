using AutoMapper;
using Hawi.Dtos;
using Hawi.Extensions;
using Hawi.Models;
using Hawi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hawi.Controllers
{
    [Route("hawi/Championship/[action]")]
    [ApiController]
    public class ChampionshipController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ChampionshipFunctions _Championship;
        private readonly ImageFunctions _imageFunctions;
        private readonly UserFunctions _UserFunctions;
        private readonly HawiContext _context;
        public ChampionshipController(IMapper mapper, ChampionshipFunctions championship, ImageFunctions imageFunctions, HawiContext context, UserFunctions userFunctions)
        {
            _mapper = mapper;
            _Championship = championship;
            _imageFunctions = imageFunctions;
            _context = context;
            _UserFunctions = userFunctions;
        }

        //start refactor  
        [HttpGet(Name = "GetChampionshipSystemOption")]
        public async Task<IActionResult> GetChampionshipSystemOption()
        {
            var ChampionshipSystemOption = await _context.ChampionshipSystemOptions
                .Select(x => new
                {
                    x.ChampionshipSystemOptionsId,
                    x.ChampionshipSystemOption1,
                }).ToListAsync();

            return Ok(ChampionshipSystemOption);
        }

        [HttpGet(Name = "GetPlaygroundByCityId")]
        public async Task<IActionResult> GetPlaygroundByCityId(long CityId)
        {
            var results = await _context.Playgrounds
            .Where(x => x.CityId == CityId)
            .Select(x => new
            {
                x.PlaygroundId,
                x.PlaygroundName,
                PlaygroundCity = _context.Cities
                    .Where(c => c.CityId == x.CityId)
                    .Select(c => c.CityArabicName)
                    .FirstOrDefault(),
            })
           .ToListAsync();

            return Ok(results);
        }

        [HttpGet(Name = "ShowChampionshipInDashboard")]
        public async Task<IActionResult> ShowChampionshipInDashboard(long UserProfileId)
        {
            try
            {
                var championships = await _context.Championships
                    .Select(x => new DashBoardChampionshipDTO
                    {
                        ChampionshipsId = x.ChampionshipsId,
                        ChampionshipsName = x.ChampionshipsName,
                        IsMale = x.IsMale ? "ذكور" : "اناث",
                        StartDate = x.StartDate,
                        TargetCategoryId = x.TargetedCategoriesId,
                        AgeCategories = x.ChampionshipAgeCategories.Select(a => a.AgeCategory.AgeCategory).ToList(),
                        CityArabicName = x.City.CityArabicName,
                        StatusOfChampionship = _Championship.CheckChampionshipStatus(x, DateTime.UtcNow),
                        OwnerChampionshipUserProfileID = (long)x.UserProfileId,
                        OwnerChampionshipName = _UserFunctions.GetUserName(x.UserProfile.RoleId, (long)x.UserProfileId, x.UserProfile.UserId),
                        OwnerChampionshipImage = _UserFunctions.GetUserImage((long)x.UserProfileId, x.UserProfile.RoleId),
                    })
                    .ToListAsync();

                return Ok(championships);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "ShowChampionshipDetailsInDashboard")]
        public async Task<IActionResult> ShowChampionshipDetailsInDashboard(long ChampionshipId)
        {
            try
            {
                var championship = await _Championship.GetChampionshipAsync(ChampionshipId);
                //get
                var championshipDashboard = await _Championship.CreateChampionshipDashboard(championship);

                return Ok(championshipDashboard);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetPlayGroundOfSpeceficChampionship")]
        public async Task<IActionResult> GetPlayGroundOfSpeceficChampionship(long championshipID)
        {
            var ChampionshipPlaygrounds = await _context.ChampionshipsPlayGrounds
                              .Where(x => x.ChampionshipId == championshipID)
                              .Select(x => new
                              {
                                  PlaygroundID = x.PlayGroundId,
                                  playgroundName = x.PlayGround.PlaygroundName,
                                  playgroundCityId = x.PlayGround.CityId,
                                  playgroundCityName = x.PlayGround.City.CityArabicName,
                              })
                               .ToListAsync();
            return Ok(ChampionshipPlaygrounds);
        }

        [HttpGet(Name = "GetTeamsOfChampionship")]
        public async Task<IActionResult> GetTeamsOfChampionship(long championshipID)
        {
            var selectResult = await _context.ChampionshipTeams
               .Where(x => x.ChampionshipId == championshipID)
               .Select(x => new
               {
                   x.TeamId,
                   x.Team.SportInstitutionId,
                   x.Team.SportInstitutionName,
               })
               .ToListAsync();
            return Ok(selectResult);
        }

        //

        [HttpPost(Name = "CreateChampionship")]
        public async Task<IActionResult> CreateChampionship([FromBody] CreateChampionshipDTO championship)
        {
            using (var transiction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region validate
                    var checkUser = await _context.UserProfiles.Where(x => x.UserProfileId == championship.UserProfileId).FirstOrDefaultAsync();
                    if (checkUser == null) return NotFound($"!{championship.UserProfileId}لم يتم العثور على المستخدم ");
                    #endregion

                    #region add championship main data

                    var newChampionship = _mapper.Map<CreateChampionshipDTO, Championship>(championship);
                    _context.Championships.Add(newChampionship);
                    await _context.SaveChangesAsync();
                    championship.ChampionshipsId = newChampionship.ChampionshipsId;

                    #endregion

                    #region add age category if exist "complex senario to use automapper"
                    if (championship.AgeCategoryId != null && championship.AgeCategoryId.Count > 0)
                    {
                        var championshipAgeCategories = championship.AgeCategoryId
                        .Select(ageCategoryId => new ChampionshipAgeCategory
                        {
                            ChampionshipId = newChampionship.ChampionshipsId,
                            AgeCategoryId = ageCategoryId
                        })
                        .ToList();
                        _context.ChampionshipAgeCategories.AddRange(championshipAgeCategories);
                    }
                    #endregion

                    #region add ChampionshipSystem

                    var newChampionshipSystem = _mapper.Map<CreateChampionshipDTO, ChampionshipSystem>(championship);
                    await _context.ChampionshipSystems.AddAsync(newChampionshipSystem);
                    await _context.SaveChangesAsync();
                    championship.ChampionshipSystemId = newChampionshipSystem.ChampionshipSystemId;

                    #endregion

                    #region add ChampionshipSystemOption
                    //نظام الدورى 3 // نظام خروج المغلوب 1  // 2  النظام المختلط 
                    await _Championship.AddChampionshipSystemAsync(championship);
                    await _context.SaveChangesAsync();

                    #endregion

                    #region ChampionshipsPlayGround
                    if (championship.PlayGroundId != null && championship.PlayGroundId.Count > 0)
                    {
                        var championshipsPlayGroundList = _mapper.Map<List<ChampionshipsPlayGround>>(championship);
                        await _context.ChampionshipsPlayGrounds.AddRangeAsync(championshipsPlayGroundList);
                    }
                    await _context.SaveChangesAsync();
                    transiction.Commit();
                    return Ok("تم الاضافة بنجاح");

                    #endregion
                }
                catch (Exception ex)
                {
                    transiction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost(Name = "CreatePlayGround")]
        public async Task<IActionResult> CreatePlayGround([FromForm] CreatePlayGroundDTO PlayGroundDTO)
        {
            using (var transiction = _context.Database.BeginTransaction())
            {
                try
                {
                    //check  name of playground I f Exist Before 
                    var checkExistOfPlayGroudWithName = _context.Playgrounds
                        .Where(x => x.PlaygroundName == PlayGroundDTO.PlaygroundName
                         && x.CityId == PlayGroundDTO.CityId)
                        .FirstOrDefault();
                    if (checkExistOfPlayGroudWithName != null) return Conflict("!هذا الملعب بهذا الاسم لنفس المدينة تم ادخالة من قبل ");

                    //  mapDto 
                    var newplayground = _mapper.Map<CreatePlayGroundDTO, Playground>(PlayGroundDTO);


                    // uploadImage to server and check directory exist or not 
                    //create Directory If Not Exist
                    var directory = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\PlayGround\";
                    await _imageFunctions.CheckDirectoryExist(directory);

                    //generate name and upload to server 
                    string GenerateNewName = _imageFunctions.ChangeImageName(PlayGroundDTO.Image);
                    string imagepath = directory + GenerateNewName;

                    var uploadimageToServer = await _imageFunctions.UploadImageToServerAsync(PlayGroundDTO.Image, imagepath);
                    if (uploadimageToServer != null) return BadRequest(uploadimageToServer);

                    var imagepathInDB = $"http://mobile.hawisports.com/image/Users/PlayGround/" + GenerateNewName;

                    //  Add 
                    PlayGroundDTO.ImagePath = imagepathInDB;
                    await _context.Playgrounds.AddAsync(newplayground);

                    await _context.SaveChangesAsync();
                    transiction.Commit();
                    return Ok("تم الاضافة بنجاح");

                }
                catch (Exception ex)
                {
                    transiction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost(Name = "CreateChampionshipTeams")]
        public async Task<IActionResult> CreateChampionshipTeams([FromBody] CreateChampionshipsTeamsDTO championshipTeam)
        {
            using (var transiction = _context.Database.BeginTransaction())
            {
                try
                {
                    var ChampionshipTeamsList = _mapper.Map<List<ChampionshipTeam>>(championshipTeam);
                    await _context.ChampionshipTeams.AddRangeAsync(ChampionshipTeamsList);

                    await _context.SaveChangesAsync();
                    transiction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    transiction.Rollback();
                    return BadRequest(ex.Message);
                }
            }

        }

        [HttpPost(Name = "CreateChampionshipMatchs")]
        public async Task<IActionResult> CreateChampionshipMatchs([FromBody] CreateChampionshipsMatchDTO match)
        {
            try
            {

                var Match = await _context.Matches.Where(x => x.MatchId == match.MatchId).FirstOrDefaultAsync();
                if (Match == null) return NotFound("لم يتم العثور على الماتش");

                var ChampionshipMatchs = _mapper.Map<CreateChampionshipsMatchDTO, ChampionshipMatch>(match);
                await _context.ChampionshipMatchs.AddAsync(ChampionshipMatchs);

                var matchDuration = _context.ChampionshipSystems.FirstOrDefault(x => x.ChampionshipId == match.ChampionshipId).MatchDuration;
                if (matchDuration != null)
                    Match.MatchDuration = matchDuration;

                var BreakTime = _context.ChampionshipSystems.FirstOrDefault(x => x.ChampionshipId == match.ChampionshipId).BreakTime;
                if (BreakTime != null)
                    Match.HalfTimeBreak = (byte?)BreakTime;

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost(Name = "AddChampionshipMathcReferee")]
        public async Task<IActionResult> AddChampionshipMathcReferee([FromBody] AddAddChampionshipMathcRefereeDto Referees)
        {
            try
            {
                var CheckMatchRefereeExist = await _context.MatchReferees
                    .Where(x => x.MatchRefereeId == Referees.MatchRefereeId)
                    .FirstOrDefaultAsync();

                if (CheckMatchRefereeExist == null) return NotFound("!لم يتم اسناد حكام بعد ");

                var newChampionshipMathcReferee = _mapper.Map<ChampionshipReferee>(Referees);
                await _context.ChampionshipReferees.AddAsync(newChampionshipMathcReferee);
                await _context.SaveChangesAsync();

                return Ok("تم الاضافة بنجاح");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //not finish
        [HttpDelete(Name = "DeleteChampionship")]
        public async Task<IActionResult> DeleteChampionship(long ChampionshipID)
        {
            try
            {

                return Ok("تم الحذف بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(Name = "DeleteTeamFromChampionship")]
        public async Task<IActionResult> DeleteTeamFromChampionship(long ChampionshipsId, long UserProfileId, long TeamId)
        {
            try
            {
                var CheckPermitionToDelete = await _context.ChampionshipTeams
                    .Where(x => x.ChampionshipTeamId == TeamId && x.ChampionshipId == ChampionshipsId)
                    .FirstOrDefaultAsync();

                if (CheckPermitionToDelete == null) return NotFound("!هذا الفريق غير مضاف لهذة البطولة");
                if (CheckPermitionToDelete.UserProfileId != UserProfileId) return Unauthorized("ليس لديك الصلاحية لهذا");

                _context.ChampionshipTeams.Remove(CheckPermitionToDelete);
                await _context.SaveChangesAsync();

                return Ok("تم الحذف");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(Name = "DeletePlayGroundFromChampionship")]
        public async Task<IActionResult> DeletePlayGroundFromChampionship(long ChampionshipPlayGroundId, long UserProfileId)
        {
            try
            {
                var checkChampionshipPlayGround = await _context.ChampionshipsPlayGrounds
                    .Where(x => x.ChampionshipPlayGroundId == ChampionshipPlayGroundId)
                    .FirstOrDefaultAsync();

                if (checkChampionshipPlayGround == null) return NotFound("!لم يتم العثور على هذا الملعب للبطولة");

                if (checkChampionshipPlayGround.UserProfileId != UserProfileId || checkChampionshipPlayGround.Championship.UserProfileId != UserProfileId)
                    return Unauthorized("!ليس لديك الصلاحية لهذا");

                _context.ChampionshipsPlayGrounds.Remove(checkChampionshipPlayGround);
                await _context.SaveChangesAsync();

                return Ok("!تم الحذف بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
