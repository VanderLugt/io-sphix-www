using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.UserCommunitiesGroups.PublishCommunityGroupLink
{
    [Table("UserCommunityGroupPublishLinks")]
    public class UserCommunityGroupPublishLinksDataModel
    {
        public UserCommunityGroupPublishLinksDataModel()
        {
            CreatedDate = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long CommunityGroupId { get; set; }
        [MaxLength(200)]
        public string VerificationCode { get; set; }
        public DateTime VerificationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPublished { get; set; }
    }
}
