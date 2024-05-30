using Hawi.Dtos;
using Hawi.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Hawi.Extensions
{
    public class AddRecordsInDB
    {
        HawiContext _context = new HawiContext();

        public string AddNewUserProfile(long UserId, byte RoleId)
        {
            try
            {
                var newUserProfile = new UserProfile
                {
                    IsActive = true,
                    RoleId = RoleId,
                    UserId = UserId,
                };
                _context.UserProfiles.Add(newUserProfile);
                _context.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string AddSeason(DateTime StartDate , DateTime EndDate)
        {
            try
            {
                var newseason = new Season
                {
                    IsActive = true,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    SeasonName = $"{StartDate.Year}-{EndDate.Year}"
                };
                _context.Seasons.Add(newseason);
                _context.SaveChanges();
                    return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task CreateCountryEntity(CreateCountryCodeDto CountryDto, string englishFileName)
        {

            var country = new CityCountry
            {
                Code = CountryDto.Code,
                CountryArabicName = CountryDto.CountryArabicName,
                CountryEnglishName = CountryDto.CountryEnglishName,
                ImageUrl = "http://mobile.hawisports.com/image/Country/" + englishFileName,
                Length = CountryDto.Length,
                RegionCode = CountryDto.RegionCode,
                IsActive = true,
            };

           await _context.CityCountries.AddAsync(country);
           await _context.SaveChangesAsync();
 
        }
    }
}
