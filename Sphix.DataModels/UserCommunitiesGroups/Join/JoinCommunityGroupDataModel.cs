using Sphix.DataModels.User;
using Sphix.DataModels.UserCommunities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.UserCommunitiesGroups.Join
{
   [Table("UserJoinCommunityGroup")]
   public class JoinCommunityGroupDataModel
    {
        public JoinCommunityGroupDataModel()
        {
            CommunityGroup = new CommunityGroupsDataModel();
            User = new UsersLoginDataModel();
            JoinDateTime = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public CommunityGroupsDataModel CommunityGroup { get; set; }
        public UsersLoginDataModel User { get; set; }
        public DateTime JoinDateTime { get; set; }
        public bool IsJoined { get; set; }
    }
}
