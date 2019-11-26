using Sphix.DataModels.User;
using Sphix.DataModels.UserCommunities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.UserCommunitiesGroups
{
    [Table("UserEventComments")]
    public class UserEventCommentsDataModel
    {
        public UserEventCommentsDataModel()
        {
            this.CommentedBy = new UsersLoginDataModel();
            this.Events = new CommunityLiveEvents();
            CommentedDate = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ParentId { get; set; }
        public virtual CommunityLiveEvents Events { get; set; }
        [MaxLength(2000)]
        [Required]
        public string CommentText { get; set; }
        public virtual UsersLoginDataModel CommentedBy { get; set; }
        public DateTime CommentedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeletedMessage { get; set; }
        public DateTime MessageDeletedDate { get; set; }
        public long DeletedBy { get; set; }
    }
}
