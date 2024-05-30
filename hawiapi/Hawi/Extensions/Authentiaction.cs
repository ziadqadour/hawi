using Hawi.Models;
using Hawi.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Hawi.Extensions
{
    public class Authentiaction
    {
        public async Task<string> GenerateJwtToken(IConfiguration configuration, long UserId, byte RoleId, UserFunctions _userFunctions, HawiContext _context)
        {
            try
            {
                var role = await _context.Roles.Where(x => x.RoleId == RoleId).FirstOrDefaultAsync();
                var userProfile = await _context.UserProfiles.Where(up => up.UserId == UserId && up.RoleId == RoleId).FirstOrDefaultAsync();
                var checkIfAdmin = await _context.UserProfiles.Where(x => x.UserId == UserId && x.RoleId == 6).FirstOrDefaultAsync();

                bool completeBasicData = true;
                var checkUserBasicData = await _context.UserBasicData.Where(x => x.UserId == userProfile.UserId).FirstOrDefaultAsync();
                if (checkUserBasicData == null) completeBasicData = false;
                if (checkUserBasicData != null)
                {
                    if (checkUserBasicData.Dob == null) completeBasicData = false;
                    if (checkUserBasicData.FirstName == null) completeBasicData = false;
                    if (checkUserBasicData.NationalityId == null) completeBasicData = false;
                }

                var name = _userFunctions.GetUserName(RoleId, userProfile.UserProfileId, UserId);

                var image = _userFunctions.GetUserImage(userProfile.UserProfileId, RoleId);

                var location = _userFunctions.GetUserLocation(userProfile);

                var unreadNotificationCount = await _context.RealTimeNotifications
                           .CountAsync(n => n.ToUserProfileId == userProfile.UserProfileId && n.IsRead == false);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == UserId);


                byte IsAdmin = 0;
                if (checkIfAdmin != null) IsAdmin = 1;

                var claims = new List<Claim>
                {
                    new Claim("UserId", UserId.ToString(), ClaimValueTypes.Integer64),
                    new Claim("UserProfileId", userProfile.UserProfileId.ToString(), ClaimValueTypes.Integer64),
                    new Claim("Name", name),
                    new Claim("Password", user.Password),
                    new Claim("Mobile", user.Mobile.ToString()),
                    new Claim("CountryId", user.CityCountryId.ToString(), ClaimValueTypes.Integer64),
                    new Claim("CityId", user.CityId.ToString(), ClaimValueTypes.Integer64),
                    new Claim("IsMale", user.IsMale.ToString(), ClaimValueTypes.Boolean),
                    new Claim("Role", role.Role1),
                    new Claim("RoleId", RoleId.ToString(), ClaimValueTypes.Integer64),
                    new Claim("IsAdmin", IsAdmin.ToString(), ClaimValueTypes.Integer64),
                    new Claim("ImgUrl", image),
                    new Claim("completeBrofileBasicDataOrNotForBerson", completeBasicData.ToString(), ClaimValueTypes.Boolean),
                    new Claim("unreadNotificationCount", unreadNotificationCount.ToString(), ClaimValueTypes.Integer64),
                };

                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(
                    claims: claims,
                    signingCredentials: creds
                );

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                user.LastLoginDate = DateTime.Now;
                user.LastLoginRoleId = RoleId;
                userProfile.Token = jwt;

                await _context.SaveChangesAsync();

                return jwt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
