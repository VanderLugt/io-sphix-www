namespace Sphix.ViewModels.UserCommunities
{
   public class JoinCommunityOpenHoursMeetingViewModel
    {
        public long Id { get; set; }
        public long OpenOfficeHoursId { get; set; }
        public long UserId { get; set; }
        public bool IsJoin { get; set; }
        public string TimeZone { get; set; }
    }
}
