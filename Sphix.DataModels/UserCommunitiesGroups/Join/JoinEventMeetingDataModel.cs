using Sphix.DataModels.User;
using Sphix.DataModels.UserCommunities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.UserCommunitiesGroups.Join
{
    [Table("UserJoinEventMeetings")]
    public class JoinEventMeetingDataModel
    {
        public JoinEventMeetingDataModel()
        {
            LiveEvent = new CommunityLiveEvents();
            User = new UsersLoginDataModel();
            JoinDateTime = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public CommunityLiveEvents LiveEvent { get; set; }
        public UsersLoginDataModel User { get; set; }
        public DateTime JoinDateTime { get; set; }
        public bool IsJoined { get; set; }
        public string TimeZone { get; set; }
        public string Note { get; set; }
    }
}
