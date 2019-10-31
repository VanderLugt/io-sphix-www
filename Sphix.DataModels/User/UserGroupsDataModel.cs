using Sphix.DataModels.Communities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.User
{
    [Table("UsersGroups")]
   public class UserGroupsDataModel
    {
        public UserGroupsDataModel()
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
        public int GroupId { get; set; }
        public bool IsActive { get; set; }
    }
}
