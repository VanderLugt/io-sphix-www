
using Sphix.DataModels.User;
using Sphix.DataModels.UserCommunities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.UserCommunitiesGroups
{
    [Table("UserArticleComments")]
   public class UserArticleCommentsDataModel
    {
        public UserArticleCommentsDataModel()
        {
            this.CommentedBy = new UsersLoginDataModel();
            this.Article = new CommunityArticles();
            CommentedDate = DateTime.Now;
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ParentId { get; set; }
        public virtual CommunityArticles Article { get; set; }
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
