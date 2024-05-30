using Newtonsoft.Json;

namespace Hawi.Dtos
{
    public class NotificationModel
    {

        [JsonProperty("deviceId")]
        public string? UserProfileToken { get; set; }
        [JsonProperty("title")]
        public string? Title { get; set; }
        [JsonProperty("body")]
        public string? Body { get; set; }
        public long UserProfileid { get; set; }
        public long userid { get; set; }
        public long roleid { get; set; }
        public long NumOfUnReadNotificationForAllAcount { get; set; }
        public long NumOfUnReadNotificationForUserProfile { get; set; }
        public string? NameOfUserThatSendNotification { get; set; }
        public string? ImageOfUserThatSendNotification { get; set; }
    }
    public class GoogleNotification
    {
        public class DataPayload
        {
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("body")]
            public string Body { get; set; }
        }
        [JsonProperty("priority")]
        public string Priority { get; set; } = "high";
        [JsonProperty("data")]
        public DataPayload Data { get; set; }
        [JsonProperty("notification")]
        public DataPayload Notification { get; set; }
    }


}
