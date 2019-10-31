using Sphix.DataModels.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.UserCommunities
{
    [Table("UserCommunitiesGroups")]
  public  class CommunityGroupsDataModel
    {
        public CommunityGroupsDataModel()
        {

            AddedDate = DateTime.Now;
            this.User = new UsersLoginDataModel();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual UsersLoginDataModel User { get; set; }
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        [MaxLength(2000)]
        public string Description { get; set; }
        [MaxLength(500)]
        public string DescriptionVideoUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublish { get; set; }
        public long CreatedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public int CommunityId { get; set; }
        public string CommunityGroupURL { get; set; }
        public bool IsPublicGroup { get; set; }
    }
}
