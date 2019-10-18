using Sphix.DataModels.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.UserCommunities
{
    [Table("UserCommunityGroupTheme")]
    public class UserCommunityGroupThemeDataModel
    {
        public UserCommunityGroupThemeDataModel()
        {

            this.User = new UsersLoginDataModel();
            this.CommunityGroups = new CommunityGroupsDataModel();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual UsersLoginDataModel User { get; set; }
        public virtual CommunityGroupsDataModel CommunityGroups { get; set; }
        public int CommunityTargetedGroupId { get; set; }
        public bool IsActive { get; set; }
    }
}
