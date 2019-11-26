using Sphix.DataModels.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sphix.DataModels.UserCommunities
{
    [Table("UserCommunityArticles")]
  public class CommunityArticles
    {
        public CommunityArticles()
        {

            AddedDate = DateTime.UtcNow;
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
        public string Title { get; set; }
        public string Description { get; set; }
        [MaxLength(300)]
        public string ShareDocUrl { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime AddedDate { get; set; }

        public static implicit operator CommunityArticles(CommunityLiveEvents v)
        {
            throw new NotImplementedException();
        }
    }
}
