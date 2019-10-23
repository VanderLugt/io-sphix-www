using Sphix.DataModels.Communities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.User
{
    [Table("UsersAssociations")]
    public class UserAssociationsDataModel
        {
        public UserAssociationsDataModel()
        {
            this.User = new UsersLoginDataModel();
            this.Community = new CommunityDataModel();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual UsersLoginDataModel User { get; set; }
        public CommunityDataModel Community { get; set; }
        [Required]
        public int AssociationId { get; set; }
        public bool IsActive { get; set; }
    }
}
