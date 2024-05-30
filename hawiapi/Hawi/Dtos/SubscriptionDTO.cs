namespace Hawi.Dtos
{
    public class FollowUpSuggestion
    {
        public long UserProfileId { get; set; }
        public byte RoleId { get; set; }
        public string Role { get; set; }
        public string? UserName { get; set; }
        public string? UserImage { get; set; }
        public long NumberOfFollowers { get; set; }
    }
    
}
