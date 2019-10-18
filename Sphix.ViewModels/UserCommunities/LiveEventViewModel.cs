using System;
namespace Sphix.ViewModels.UserCommunities
{
   public class LiveEventViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long CommunityGroupId { get; set; }
        public string ETitle { get; set; }
        public string EName { get; set; }
        public string EDescription { get; set; }
        public string EFrequency { get; set; }
        public DateTime EFromDate { get; set; }
        public DateTime EToDate { get; set; }
        public string ETime { get; set; }
        public string ETimeDayName { get; set; }
        public string ETimeZone { get; set; }
        public int MaxAttendees { get; set; }
        public string WhoCanAttend { get; set; }
        public int Participants { get; set; }
        public int Observers { get; set; }
        public string Picture { get; set; }
        public bool IsSingleEvent { get; set; }
    }
}
