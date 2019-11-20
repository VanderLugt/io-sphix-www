using System;
using System.ComponentModel.DataAnnotations;

namespace Sphix.ViewModels.UserCommunities
{
   public class OpenOfficeHoursViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long CommunityGroupId { get; set; }
        [Required]
        public string OTitle { get; set; }
        public string OName { get; set; }
        public string ODescription { get; set; }
        public string OFrequency { get; set; }
        public DateTime OFromDate { get; set; }
        public DateTime OToDate { get; set; }
        public string OTime { get; set; }
        public string OTimeDayName { get; set; }
        public string OTimeZone { get; set; }
        public int MaxAttendees { get; set; }
        public string WhoCanAttend { get; set; }
        public bool IsRegisterInMeeting { get; set; }
        public bool IsFirstMeeting { get; set; }
        public string NextMeetingToken { get; set; }
        public long LastSessionId { get; set; }
        public bool IsMeetingTokenUsed { get; set; }
        public bool AddHours { get; set; }
    }
}
