using CorePush.Google;
using Hawi.Dtos;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Hawi.Extensions
{

    //public class NotificationService : INotificationService
    //{
    //    private readonly FcmNotificationSetting _fcmNotificationSetting;
    //    public NotificationService(IOptions<FcmNotificationSetting> settings)
    //    {
    //        _fcmNotificationSetting = settings.Value;
    //    }

    //    public async Task<ResponseModel> SendNotification(NotificationModel notificationModel)
    //    {
    //        ResponseModel response = new ResponseModel();
    //        try
    //        {
    //            if (notificationModel.IsAndroiodDevice)
    //            {
    //                /* FCM Sender (Android Device) */
    //                FcmSettings settings = new FcmSettings()
    //                {
    //                    SenderId = _fcmNotificationSetting.SenderId,
    //                    ServerKey = _fcmNotificationSetting.ServerKey
    //                };
    //                HttpClient httpClient = new HttpClient();

    //                string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
    //                string deviceToken = notificationModel.DeviceId;

    //                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
    //                httpClient.DefaultRequestHeaders.Accept
    //                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //                DataPayload dataPayload = new DataPayload();
    //                dataPayload.Title = notificationModel.Title;
    //                dataPayload.Body = notificationModel.Body;

    //                GoogleNotification notification = new GoogleNotification();
    //                notification.Data = dataPayload;
    //                notification.Notification = dataPayload;

    //                var fcm = new FcmSender(settings, httpClient);
    //                var fcmSendResponse = await fcm.SendAsync($"/topics/{deviceToken}", notification);

    //                if (fcmSendResponse.IsSuccess())
    //                {
    //                    response.IsSuccess = true;
    //                    response.Message = "Notification sent successfully";
    //                    return response;
    //                }
    //                else
    //                {
    //                    response.IsSuccess = false;
    //                    response.Message = fcmSendResponse.Results[0].Error;
    //                    return response;
    //                }
    //            }
    //            else
    //            {
    //                /* Code here for APN Sender (iOS Device) */
    //                //var apn = new ApnSender(apnSettings, httpClient);
    //                //await apn.SendAsync(notification, deviceToken);
    //            }
    //            return response;
    //        }
    //        catch (Exception ex)
    //        {
    //            response.IsSuccess = false;
    //            response.Message = "Something went wrong";
    //            return response;
    //        }
    //    }
    //}

    public interface INotificationService
    {
        Task<string> SendNotification(NotificationModel notificationModel);
    }
    public class NotificationService : INotificationService
    {
        private readonly FcmNotificationSetting _fcmNotificationSetting;

        public NotificationService(IOptions<FcmNotificationSetting> settings)
        {
            _fcmNotificationSetting = settings.Value;
        }

        public async Task<string> SendNotification(NotificationModel notificationModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                //if (notificationModel.IsAndroiodDevice)
                //{
                FcmSettings settings = new FcmSettings()
                {
                    SenderId = _fcmNotificationSetting.SenderId,
                    ServerKey = _fcmNotificationSetting.ServerKey
                };

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://fcm.googleapis.com/fcm/");

                string deviceToken = notificationModel.UserProfileToken;

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={settings.ServerKey}");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var data = new
                {
                    to = $"/topics/{deviceToken}",
                    notification = new
                    {
                        title = notificationModel.Title,
                        body = notificationModel.Body,
                    },
                    data = new
                    {
                        UserProfileid = notificationModel.UserProfileid,
                        userid = notificationModel.userid,
                        roleid = notificationModel.roleid,
                        NumOfUnReadNotificationForAllAcount = notificationModel.NumOfUnReadNotificationForAllAcount,
                        NumOfUnReadNotificationForUserProfile = notificationModel.NumOfUnReadNotificationForUserProfile,
                        ImageOfUserThatSendNotification = notificationModel.ImageOfUserThatSendNotification,
                    }
                };

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var fcmSendResponse = await httpClient.PostAsync("send", content);

                if (fcmSendResponse.IsSuccessStatusCode)
                {
                    response.IsSuccess = true;
                    response.Message = "Notification sent successfully";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = $"FCM Error: {fcmSendResponse.StatusCode}";
                }

               
                return null;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                response.IsSuccess = false;
                response.Message = "Something went wrong";
                return ex.Message;
            }

        }
    }
}
