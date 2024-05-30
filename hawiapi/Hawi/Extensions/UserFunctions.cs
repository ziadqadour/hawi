using AutoMapper;
using Hawi.Dtos;
using Hawi.Models;
using Microsoft.EntityFrameworkCore;
using PhoneNumbers;

namespace Hawi.Repository
{
    public class UserFunctions
    {
        private readonly IMapper _mapper;
        HawiContext _context = new HawiContext();
        public UserFunctions(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<User> CreateUserEntity(UserForRegistrationDto user)
        {
            var userEntity = new User
            {
                Name = user.Name,
                Mobile = user.Mobile,
                Password = user.Password,
                CityCountryId = user.CountryCodeId,
                IsMale = user.IsMale,
                UserStatusId = 1,
                LastUpdate = DateTime.Now,
                LastLoginRoleId = 1,
                CityId = user.CityId,
                IsActive = true,
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return userEntity;
        }

        public async Task<UserProfile> CreateUserProfile(long UserId, byte RoleId)
        {
            var userProfile = new UserProfile
            {
                UserId = UserId,
                RoleId = RoleId,
                IsActive = true,
                LastUpdate = DateTime.Now,
            };

            await _context.UserProfiles.AddAsync(userProfile);
            await _context.SaveChangesAsync();

            return userProfile;
        }

        public async Task<string> CheckPhoneNumber(string Mobile, CityCountry countrycode)
        {
            try
            {
                var regionCode = countrycode.RegionCode;

                string? code = countrycode.Code;
                string phoneNumber = code + Mobile;

                PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
                try
                {
                    PhoneNumber parsedNumber = phoneNumberUtil.Parse(phoneNumber, $"{regionCode}");
                    if (phoneNumberUtil.IsValidNumber(parsedNumber) == false) return "!الرقم الذى ادخلتة غير صحيح";
                }
                catch (NumberParseException)
                {
                    return "!الرقم الذى ادخلتة غير صحيح";
                }

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> CheckUserProfileExists(long userprofileid)
        {
            var userProfile = await _context.UserProfiles
                .Where(x => x.UserProfileId.Equals(userprofileid) && x.IsActive == true)
                .FirstOrDefaultAsync();

            if (userProfile == null) return "!لم يتم العثور على هذا المستخدم ";
            return null;
        }

        public string CheckUserAcountExists(long UserId)
        {
            var user = _context.Users.Where(x => x.UserId == UserId && x.IsActive == true).FirstOrDefault();
            if (user == null)
                return "!لم يتم العثور على المستخدم الذى ادخلته ";
            return null;
        }
        public string GetUserImage(long userProfileId, byte roleId)
        {
            string defaultImage = "http://mobile.hawisports.com/image/defultprofile.png";
            string imgPath = null;

            if (roleId == 5 || roleId == 7 || roleId == 8 || roleId == 11)
            {
                var sportInstitutionImage = _context.SportInstitutions
                    .Where(x => x.UserProfileId == userProfileId)
                    .FirstOrDefault();

                if (sportInstitutionImage != null)
                {
                    if (!string.IsNullOrEmpty(sportInstitutionImage.LogoUrlfullPath))
                        imgPath = sportInstitutionImage.LogoUrlfullPath;
                    else
                        imgPath = defaultImage;
                }
            }
            else
            {
                var userProfileImage = _context.UserProfileImages
                    .Where(x => x.UserProfileId == userProfileId && x.ImageTypeId == 1)
                    .FirstOrDefault();

                if (userProfileImage != null)
                    imgPath = _context.Images.FirstOrDefault(x => x.ImageId == userProfileImage.ImageId)?.ImageUrlfullPath;
                else
                    imgPath = defaultImage;
            }

            return imgPath ?? defaultImage;
        }

        public string GetUserBackgroundImage(long userProfileId, byte roleId)
        {
            string defaultBackgroundImage = "http://mobile.hawisports.com/image/coverprofileapp.png";
            string backgroundImage = null;

            if (roleId == 5 || roleId == 7 || roleId == 8 || roleId == 11)
            {
                var sportInstitutionImage = _context.SportInstitutions
                    .Where(x => x.UserProfileId == userProfileId)
                    .FirstOrDefault();

                if (sportInstitutionImage != null)
                {
                    if (!string.IsNullOrEmpty(sportInstitutionImage.BackGroundUrlfullPath))
                        backgroundImage = sportInstitutionImage.BackGroundUrlfullPath;
                    else
                        backgroundImage = defaultBackgroundImage;
                }
            }
            else
            {
                var userProfileImages = _context.UserProfileImages
                    .Where(x => x.UserProfileId == userProfileId)
                    .ToList();

                if (userProfileImages != null && userProfileImages.Count != 0)
                {
                    var selectBackgroundImage = userProfileImages.FirstOrDefault(x => x.ImageTypeId == 12);

                    if (selectBackgroundImage != null)
                        backgroundImage = _context.Images.FirstOrDefault(x => x.ImageId == selectBackgroundImage.ImageId)?.ImageUrlfullPath;
                    else
                        backgroundImage = defaultBackgroundImage;
                }
                else
                {
                    backgroundImage = defaultBackgroundImage;
                }
            }

            return backgroundImage ?? defaultBackgroundImage;
        }

        public LocationInfo GetUserLocation(UserProfile userprofile)
        {
            LocationInfo locationInfo = new LocationInfo();
            string llocation = null;
            string locationurl = null;
            SportInstitutionBranch MainBrancheLocation = null;

            if (userprofile.RoleId == 5 || userprofile.RoleId == 7 || userprofile.RoleId == 8 || userprofile.RoleId == 11)
            {
                var sportinstituation = _context.SportInstitutions.Where(x => x.UserProfileId == userprofile.UserProfileId).FirstOrDefault();

                if (sportinstituation != null)
                    MainBrancheLocation = _context.SportInstitutionBranches?.Where(x => x.SportInstitutionId == sportinstituation.SportInstitutionId
                                       && x.SportInstitutionBranchTypeId == 1)?.FirstOrDefault();

                if (MainBrancheLocation == null)
                {
                    llocation = null;
                    locationurl = null;
                }
                else
                {
                    var cityDistict = MainBrancheLocation.CityDistricts ?? null;
                    var countryId = _context.Cities.FirstOrDefault(x => x.CityId == MainBrancheLocation.CityId).CityCountryId ?? null;
                    var country = (countryId != null) ? _context.CityCountries.FirstOrDefault(x => x.CityCountryId == countryId).CountryArabicName : null;
                    var city = _context.Cities.FirstOrDefault(x => x.CityId == MainBrancheLocation.CityId).CityArabicName ?? null;
                    llocation = $"{city} _ {country}";
                    locationurl = MainBrancheLocation.Location;
                }
            }
            else
            {
                var user = _context.Users.FirstOrDefault(x => x.UserId == userprofile.UserId);
                var CountryName = _context.CityCountries.FirstOrDefault(x => x.CityCountryId == user.CityCountryId);
                var cityname = _context.Cities.FirstOrDefault(x => x.CityId == user.CityId);
                var city = cityname?.CityArabicName;
                var country = CountryName?.CountryArabicName;
                llocation = $"{city}-{country}";
            }
            locationInfo.LLocation = llocation;
            locationInfo.LocationUrl = locationurl;
            return locationInfo;
        }

        public string GetUserName(byte roleid, long userprofileid, long UserID)
        {
            string name = null;

            if (roleid == 5 || roleid == 7 || roleid == 8 || roleid == 11)
            {
                var selectSportinstituation = _context.SportInstitutions
                      .Where(x => x.UserProfileId == userprofileid).FirstOrDefault();

                if (selectSportinstituation != null)
                    name = selectSportinstituation.SportInstitutionName;
            }
            else
            {
                var selectuser = _context.Users.Where(x => x.UserId == UserID).FirstOrDefault();

                if (selectuser != null)
                    name = selectuser.Name;
            }

            return name;
        }

        public async Task CheckUserToDelete()
        {

            var CheckUserToDeleteResult = await _context.UsersUsersActivationArchieves
                .Where(x => x.DateToDelete == DateTime.UtcNow)
                .ToListAsync();
            //
        }

        public bool IsSportsorganization(long userprofileid)
        {
            var checkrole = _context.UserProfiles.Where(x => x.UserProfileId == userprofileid).FirstOrDefault();
            if (checkrole.RoleId == 5 || checkrole.RoleId == 7 || checkrole.RoleId == 8 || checkrole.RoleId == 11)
                return true;
            return false;

        }

        public DateTime AddTimeToDateInTraining(double TrainingTime, DateTime date)
        {
            int hours = (int)TrainingTime;
            int minutes = (int)((TrainingTime - hours) * 100);

            TimeSpan trainingTimeSpan = new TimeSpan(hours, minutes, 0);

            DateTime newDateTime = date.Date + trainingTimeSpan;

            return newDateTime;

        }

        public async Task<UserDataForNotificationDto> GetUserDataForNotificationByProfileId(long ReceiverUserProfileId, long SenderUserProfileId)
        {
            //ReceiverUserProfileId that notification will Receive it 
            //SenderUserProfileId user that notification send from it to other user 
            var checkReceiverUserProfile = _context.UserProfiles.Where(x => x.UserProfileId == ReceiverUserProfileId).FirstOrDefault();
            var checkSenderUserProfile = _context.UserProfiles.Where(x => x.UserProfileId == SenderUserProfileId).FirstOrDefault();

            var UserId = checkReceiverUserProfile.UserId;
            var RoleId = checkReceiverUserProfile.RoleId;


            var NumOfUnReadNotificationForAllAcount = await _context.RealTimeNotifications
                         .CountAsync(n => n.ToUserProfile.UserId == UserId && n.IsRead == false);

            var NumOfUnReadNotificationForUserProfile = await _context.RealTimeNotifications
                        .CountAsync(n => n.ToUserProfileId == ReceiverUserProfileId && n.IsRead == false);

            var UserDataForNotificationDto = new UserDataForNotificationDto
            {
                UserProfileId = ReceiverUserProfileId,
                UserId = UserId,
                RoleId = RoleId,
                NumOfUnReadNotificationForAllAcount = NumOfUnReadNotificationForAllAcount,
                NumOfUnReadNotificationForUserProfile = NumOfUnReadNotificationForUserProfile,
                ImageOfUserThatSendNotification = GetUserImage(SenderUserProfileId, checkSenderUserProfile.RoleId),
                NameOfUserThatSendNotification = GetUserName(checkSenderUserProfile.RoleId, SenderUserProfileId, checkSenderUserProfile.UserId)
            };

            return UserDataForNotificationDto;
        }

        public object GetFullUserBasicdata(long userid)
        {
            var selectedUserBasicData = _context.UserBasicData.Where(x => x.UserId == userid).FirstOrDefault();
            if (selectedUserBasicData == null)
                return null;

            var qualification = _context.UserBasicDataQualificationTypes.Where(x => x.QualificationTypeId == selectedUserBasicData.QualificationTypeId).FirstOrDefault();

            var InternationalFavoriteTeam = _context.CityCountries.Where(x => x.CityCountryId == selectedUserBasicData.InternationalFavoriteTeamId).FirstOrDefault();

            var nationality = _context.CityCountries.Where(x => x.CityCountryId == selectedUserBasicData.NationalityId).FirstOrDefault();

            var LocalFavoriteClub = _context.Clubs.Where(x => x.ClubId == selectedUserBasicData.LocalFavoriteClubId).FirstOrDefault();

            var InternationalFavoriteClub = _context.Clubs.Where(x => x.ClubId == selectedUserBasicData.InternationalFavoriteClubId).FirstOrDefault();

            var basicdata = _context.UserBasicData
             .Where(x => x.UserId == userid)
             .Select(x => new
             {
                 x.UserBasicDataId,
                 x.UserId,
                 x.Idnumber,
                 x.FirstName,
                 x.FatherName,
                 x.GrandFatherName,
                 x.FamilyName,
                 x.Height,
                 x.Weight,
                 x.Dob,
                 x.Pob,
                 NationalityId = x.NationalityId ?? null,
                 Nationality = x.Nationality != null ? x.Nationality.CountryArabicName : null,
                 x.PlaceOfResidence,
                 x.LocalFavoritePlayer,
                 x.InternationalFavoritePlayer,
                 LocalFavoriteClubId = x.LocalFavoriteClubId ?? null,
                 LocalFavoriteClub = x.LocalFavoriteClub != null ? x.LocalFavoriteClub.ClubArabicName : null,
                 InternationalFavoriteClubId = x.InternationalFavoriteClubId ?? null,
                 InternationalFavoriteClub = x.InternationalFavoriteClub != null ? x.InternationalFavoriteClub.ClubArabicName : null,
                 InternationalFavoriteTeamId = x.InternationalFavoriteTeamId ?? null,
                 InternationalFavoriteTeam = x.InternationalFavoriteTeam != null ? x.InternationalFavoriteTeam.CountryArabicName : null,
                 QualificationTypeId = x.QualificationTypeId ?? null,
                 qualification = x.QualificationType != null ? x.QualificationType.QualificationType : null,
                 PlayerFeetId = x.PlayerFeetId ?? null,
                 PlayerFeet = x.PlayerFeet.FootName ?? null,
                 PlayerMainPlaceId = x.PlayerMainPlaceId ?? null,
                 PlayerMainPlaceI = x.PlayerMainPlace.PlayerPlace ?? null,
                 PlayerSecondaryPlaceId = x.PlayerSecondaryPlaceId ?? null,
                 PlayerSecondaryPlace = x.PlayerSecondaryPlace.PlayerPlace ?? null,
                 CityCountryId = _context.Users.FirstOrDefault(n => n.UserId == x.UserId).CityCountryId ?? null,
                 CountryName = _context.Users.FirstOrDefault(n => n.UserId == x.UserId).CityCountry.CountryArabicName ?? null,
                 CityId = (_context.Users.FirstOrDefault(n => n.UserId == x.UserId).CityId != null) ? (_context.Users.FirstOrDefault(n => n.UserId == x.UserId).CityId) : null,
                 CityName = (_context.Users.FirstOrDefault(n => n.UserId == x.UserId).City.CityArabicName != null) ? (_context.Users.FirstOrDefault(n => n.UserId == x.UserId).City.CityArabicName) : null,
                 UserNeckName = _context.Users.FirstOrDefault(n => n.UserId == x.UserId).Name,
                 UserImageInBasicData = selectedUserBasicData.OfficialPhotoForMatches ?? null,
             })
             .FirstOrDefault();

            return basicdata;
        }

        public List<SearchUserByPhoneDto> SearchUserByPhone(string phoneNumber, long UserProfileID, long branchId = 0)
        {
            try
            {
                var user = _context.Users.Where(x => x.Mobile == phoneNumber && x.IsActive == true).FirstOrDefault();

                List<SearchUserByPhoneDto> searchResult = new List<SearchUserByPhoneDto>();

                List<UserProfile> userprofile = new List<UserProfile>();

                if (user == null) return searchResult;

                if (branchId == 0)
                    userprofile = _context.UserProfiles
                        .Where(x => x.UserId == user.UserId && x.IsActive == true
                           && (x.RoleId == 1 /*|| x.RoleId == 2*/) && x.UserProfileId != UserProfileID).ToList();
                else
                {
                    var checkprofileInBranch = _context.UserProfiles.Where(x => x.UserId == user.UserId && x.IsActive == true && x.UserProfileId != UserProfileID).ToList();
                    foreach (var profile in checkprofileInBranch)
                    {
                        var checkThatUserProfileINBranchOrNot = _context.SportInstitutionBelongs
                              .Where(x => x.SportInstitutionBranchId == branchId &&
                               x.BelongUserProfileId == profile.UserProfileId).FirstOrDefault();

                        if (checkThatUserProfileINBranchOrNot != null)
                        {
                            var Finduserprofile = _context.UserProfiles
                                .Where(x => x.UserProfileId == checkThatUserProfileINBranchOrNot.BelongUserProfileId && x.IsActive == true && x.RoleId == 1).FirstOrDefault();
                            if (Finduserprofile != null)
                                userprofile.Add(Finduserprofile);
                        }
                    }
                }

                foreach (var result in userprofile)
                {
                    var x = new SearchUserByPhoneDto
                    {
                        Userprofileid = result.UserProfileId,
                        UserName = GetUserName(result.RoleId, result.UserProfileId, result.UserId),
                        Userrole = _context.Roles.FirstOrDefault(x => x.RoleId == result.RoleId).Role1,
                        userimage = GetUserImage(result.UserProfileId, result.RoleId),
                    };
                    searchResult.Add(x);
                }
                return searchResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<object> getUserImageByUserProfileID(OwnerParameters ownerParameters, long userprofileid, byte isalpom = 1)
        {
            var articleViedio = _context.ArticleVideos
                .Where(ai => ai.Article.UserProfileId == userprofileid && ai.Article.IsActive == true)
                .Select(ai => new ImageInfo
                {
                    ImageId = ai.VideoId,
                    ImageUrl = ai.Video.VideoUrl,
                    Date = ai.Video.CreateDate,
                    IsVideo = true,
                })
                 .Take(1)
                 .ToList();

            var articleImages = _context.ArticleImages
                .Where(ai => ai.Article.UserProfileId == userprofileid && ai.Article.IsActive == true)
                .Select(ai => new ImageInfo
                {
                    ImageId = ai.ImageId,
                    ImageUrl = ai.Image.ImageUrlfullPath,
                    Date = ai.Image.CreateDate,
                    IsVideo = false,
                })
                .Take(1)
                .ToList();

            var sportInstitutionlogoImages = _context.SportInstitutions
                .Where(si => si.UserProfileId == userprofileid)
                .Select(si => new ImageInfo
                {
                    ImageUrl = si.LogoUrlfullPath,
                    Date = si.CreateDate,
                    IsVideo = false,
                })
                .Take(1)
                .ToList();

            var uniformImages = _context.Uniforms
                .Where(u => u.UserProfileId == userprofileid)
                .Select(u => new ImageInfo
                {
                    ImageId = u.UniformImage.ImageId,
                    ImageUrl = u.UniformImage.ImageUrlfullPath,
                    Date = u.CreateDate,
                    IsVideo = false,
                })
                .Take(1)
                .ToList();

            var trainingImages = _context.Training
                .Where(t => t.UserProfileId == userprofileid && (t.TrainingImageId != null || t.TrainingImageId != 0))
                .Select(t => new ImageInfo
                {
                    ImageId = t.TrainingImage.ImageId,
                    ImageUrl = t.TrainingImage.ImageUrlfullPath,
                    Date = t.CreateDate,
                    IsVideo = false,
                })
                .Take(1)
                .ToList();

            var userProfileImages = _context.UserProfileImages
                .Where(upi => upi.UserProfileId == userprofileid && upi.ImageTypeId == 1)
                .Select(upi => new ImageInfo
                {
                    ImageId = upi.ImageId,
                    ImageUrl = upi.Image.ImageUrlfullPath,
                    Date = upi.CreateDate,
                    IsVideo = false,
                })
                .Take(1)
                .ToList();

            var userAlbums = _context.ImageAlbums
                 .Where(album => album.UserProfileId == userprofileid)
                 .Select(album => new resultt
                 {
                     alpumId = album.ImageAlbumsId,
                     Name = album.AlbumName,
                     IsDefault = false,
                     Image = album.ImageAlbumImages.Select(albumImage => new ImageInfo
                     {
                         ImageId = albumImage.Image.ImageId,
                         ImageUrl = albumImage.Image.ImageUrlfullPath,
                         Date = albumImage.CreateDate,
                     })
                     .Take(1)
                     .ToList()
                 })
                 .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                 .Take(ownerParameters.PageSize)
                 .ToList();

            if (isalpom == 1)
            {
                //List<resultt> resulttList = new List<resultt>();
                List<object> resulttList = new List<object>();

                resultt articleImagesResult = new resultt { Name = "المقالات", alpumId = 1, Image = articleImages };
                resultt sportInstitutionProofImagesResult = new resultt { Name = "المنشاه الرياضية", alpumId = 2, Image = sportInstitutionlogoImages };
                resultt uniformImagesResult = new resultt { Name = "الزى الرياضى", alpumId = 5, Image = uniformImages };
                resultt trainingImagesResult = new resultt { Name = "التمارين", alpumId = 3, Image = trainingImages };
                resultt userProfileImagesResult = new resultt { Name = "الملف الشخصى", alpumId = 4, Image = userProfileImages };
                resultt VedioResult = new resultt { Name = "فديوهات المقالات", alpumId = 6, Image = articleViedio };

                if (ownerParameters.PageNumber == 1)
                {
                    resulttList.Add(VedioResult);
                    resulttList.Add(articleImagesResult);
                    resulttList.Add(sportInstitutionProofImagesResult);
                    resulttList.Add(uniformImagesResult);
                    resulttList.Add(trainingImagesResult);
                    resulttList.Add(userProfileImagesResult);
                }
                resulttList.AddRange(userAlbums);
                return resulttList;
            }

            else if (isalpom == 0)
            {
                var allImages = articleImages
                .Concat(sportInstitutionlogoImages)
                .Concat(uniformImages)
                .Concat(trainingImages)
                .Concat(userProfileImages)
                .Concat(_context.ImageAlbums
                .Where(album => album.UserProfileId == userprofileid)
                .SelectMany(album => album.ImageAlbumImages)
                .Select(albumImage => new ImageInfo
                {
                    ImageId = albumImage.ImageId,
                    // alpumId = albumImage.AlbumId,
                    ImageUrl = albumImage.Image.ImageUrlfullPath,
                    Date = albumImage.CreateDate
                }))
                .OrderByDescending(image => image.Date)
                .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                .Take(ownerParameters.PageSize)
                .ToList();
                // return Ok(allImages);
                return allImages.OfType<object>().ToList();
            }

            return null;
        }


    }
}
