using Sphix.DataModels.Communities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.User
{
   [Table("UsersInterests")]
   public class UserInterestsDataModel
    {
        public UserInterestsDataModel()
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
        public int InterestId { get; set; }
        public bool IsActive { get; set; }
    }
}
