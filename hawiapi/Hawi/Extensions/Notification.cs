using Hawi.Dtos;
using Hawi.Models;
using Microsoft.EntityFrameworkCore;

namespace Hawi.Extensions
{
    public class Notification
    {
        HawiContext _context = new HawiContext();
        public async Task< NotificationModel> CreateRealTimeNotificationModel(UserDataForNotificationDto userData, string message, UserProfile userProfile)
        {
            return new NotificationModel
            {
                UserProfileToken = userProfile.Token,
                Title = userData.NameOfUserThatSendNotification,
                Body = message,
                UserProfileid = userData.UserProfileId,
                userid = userData.UserId,
                roleid = userData.RoleId,
                NumOfUnReadNotificationForAllAcount = userData.NumOfUnReadNotificationForAllAcount,
                NumOfUnReadNotificationForUserProfile = userData.NumOfUnReadNotificationForUserProfile,
                ImageOfUserThatSendNotification = userData.ImageOfUserThatSendNotification,
            };
        }

        public async Task<RealTimeNotification> CreateDBObject(long userProfileId, long pendingUserProfileId, string message, long invitationId)
        {
            return new RealTimeNotification
            {
                FromUserProfileId = userProfileId,
                ToUserProfileId = pendingUserProfileId,
                TargetTypeId = 1,
                InvitationId = invitationId,
                TargetId = userProfileId,
                IsRead = false,
                ContentMessage = message,
            };
        }


        //diffrence for above "that take TargetId and TargetTypeId"
        public async Task<RealTimeNotification> CreateDBObjectForNotification(long userProfileId, long pendingUserProfileId, string message, long invitationId,long TargetId, byte TargetTypeId)
        {
            return new RealTimeNotification
            {
                FromUserProfileId = userProfileId,
                ToUserProfileId = pendingUserProfileId,
                TargetTypeId = TargetTypeId,
                InvitationId = invitationId,
                TargetId = TargetId,
                IsRead = false,
                ContentMessage = message,
            };
        }


    }
}
