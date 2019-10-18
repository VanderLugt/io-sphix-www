using Sphix.DataModels.User;
using Sphix.DataModels.UserCommunities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.UserCommunitiesGroups
{
    [Table("UserCommunityTargetedTypeOne")]
    public class UserCommunityTargetedType1DataModel
    {
        public UserCommunityTargetedType1DataModel()
        {
            this.User = new UsersLoginDataModel();
            this.CommunityGroups = new CommunityGroupsDataModel();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual UsersLoginDataModel User { get; set; }
        public virtual CommunityGroupsDataModel CommunityGroups { get; set; }
        public int TypeId { get; set; }
        public bool IsActive { get; set; }
    }
}
