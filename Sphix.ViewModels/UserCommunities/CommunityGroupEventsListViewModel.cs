namespace Sphix.ViewModels.UserCommunities
{
   public class CommunityGroupEventsListViewModel:BaseModel
    {
        public long UserId { get; set; }
        public string Title { get; set; }
        public string EFromDate { get; set; }
        public string EToDate { get; set; }
        public string IsLive { get; set; }
        public long CommunityGroupsId { get; set; }
        public int MaxAttendees { get; set; }
        public int TotalJoinedMembers { get; set; }
    }
}
