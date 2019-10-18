using Sphix.DataModels.Communities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.User
{
    [Table("UsersSubCommunities")]
   public class UserSubCommunitiesDataModel
    {
        public UserSubCommunitiesDataModel()
        {
            this.User = new UsersLoginDataModel();
            this.Community = new CommunityDataModel();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual UsersLoginDataModel User { get; set; }
        public CommunityDataModel Community { get; set; }
        public int GroupId { get; set; }
        public int Association { get; set; }
        public int Type1Id { get; set; }
        public int Type2ListId { get; set; }
    }
}
