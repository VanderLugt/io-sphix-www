using Sphix.DataModels.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sphix.DataModels.UserCommunities
{
    [Table("UserCommunityLiveEvents")]
    public class CommunityLiveEvents
    {
        public CommunityLiveEvents()
        {

            AddedDate = DateTime.Now;
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
        public string ETitle { get; set; }
        [MaxLength(200)]
        [Required]
        public string EName { get; set; }
        [MaxLength(2000)]
        public string EDescription { get; set; }
        [MaxLength(50)]
        public string EFrequency { get; set; }
        public DateTime EFromDate { get; set; }
        public DateTime EToDate { get; set; }
        [MaxLength(50)]
        public string ETime { get; set; }
        [MaxLength(50)]
        public string ETimeDayName { get; set; }
        [MaxLength(50)]
        public string ETimeZone { get; set; }
        public int MaxAttendees { get; set; }
        [MaxLength(50)]
        public string WhoCanAttend { get; set; }
        public int Participants { get; set; }
        public int Observers { get; set; }
        [MaxLength(300)]
        public string Picture { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsSingleEvent { get; set; }
    }
}
