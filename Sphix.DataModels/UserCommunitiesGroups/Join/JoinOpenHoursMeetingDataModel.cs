using Sphix.DataModels.User;
using Sphix.DataModels.UserCommunities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.UserCommunitiesGroups.Join
{
    [Table("UserJoinCommunityOpenHourMeetings")]
    public class JoinOpenHoursMeetingDataModel
    {
        public JoinOpenHoursMeetingDataModel()
        {
            OpenOfficeHours = new CommunityOpenOfficeHours();
            User = new UsersLoginDataModel();
            JoinDateTime = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public CommunityOpenOfficeHours OpenOfficeHours { get; set; }
        public UsersLoginDataModel User { get; set; }
        public DateTime JoinDateTime { get; set; }
        public bool IsJoined { get; set; }
        public string TimeZone { get; set; }
        [MaxLength(500)]
        public string Note { get; set; }
    }
}
