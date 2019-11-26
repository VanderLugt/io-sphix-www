using Sphix.DataModels.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.UserCommunities
{
    [Table("UserCommunityOpenOfficeHours")]
    public class CommunityOpenOfficeHours
    {
        public CommunityOpenOfficeHours()
        {

            AddedDate = DateTime.UtcNow;
            this.User = new UsersLoginDataModel();
            this.CommunityGroups = new CommunityGroupsDataModel();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual UsersLoginDataModel User { get; set; }
        public virtual CommunityGroupsDataModel CommunityGroups { get; set; }
        [MaxLength(200)]
        [Required]
        public string OTitle { get; set; }
        [MaxLength(200)]
        [Required]
        public string OName { get; set; }
        [MaxLength(2000)]
        public string ODescription { get; set; }
        [MaxLength(50)]
        public string OFrequency { get; set; }
        public DateTime OFromDate { get; set; }
        public DateTime OToDate { get; set; }
        [MaxLength(50)]
        public string OTime { get; set; }
        [MaxLength(50)]
        public string OTimeDayName { get; set; }
        [MaxLength(50)]
        public string OTimeZone { get; set; }
        public int MaxAttendees { get; set; }
        [MaxLength(50)]
        public string WhoCanAttend { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsFirstMeeting { get; set; }
        [MaxLength(100)]
        public string NextMeetingToken { get; set; }
        public long LastSessionId { get; set; }
        public bool IsMeetingTokenUsed { get; set; }
        public bool AddHours { get; set; }
        
    }
   
}
