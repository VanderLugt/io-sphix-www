using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.UserCommunitiesGroups
{
    [Table("UserCommunityOpenOfficeHoursMeetingsStatus")]
    public class OpenOfficeHoursMeetingsStatusDataModel
    {
        public OpenOfficeHoursMeetingsStatusDataModel()
        {
            CreatedOn = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long MeetingId { get; set; }
        public long CreatedBy { get; set; }
        [MaxLength(20)]
        [Required]
        public string MeetingStatus { get; set; }
        [MaxLength(200)]
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
      
    }
}
