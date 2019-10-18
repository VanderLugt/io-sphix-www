using System;

namespace Sphix.ViewModels.UserCommunities
{
   public class OpenOfficeHoursTables
    {
        public long Id { get; set; }
        public long CommunityGroupId { get; set; }
        public string OTime { get; set; }
        public string OTimeZone { get; set; }
        public int MaxAttendees { get; set; }
        public string OFromDate { get; set; }
        public int BookedSeats { get; set; }
        public bool HasExpired { get; set; }
    }
}
