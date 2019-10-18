namespace Sphix.ViewModels.UserCommunities
{
   public class ArticleCommentViewMoldel
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public long ArticleId { get; set; }
        public string CommentText { get; set; }
        public long UserId { get; set; }
    }
}
