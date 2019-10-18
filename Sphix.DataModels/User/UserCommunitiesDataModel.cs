using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.User
{
   [Table("UsersCommunities")]
   public class UserCommunitiesDataModel
    {
        public UserCommunitiesDataModel()
        {
            this.User = new UsersLoginDataModel();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual UsersLoginDataModel User { get; set; }
        public Int64 CommunityId { get; set; }
        public bool IsActive { get; set; }
    }
}
