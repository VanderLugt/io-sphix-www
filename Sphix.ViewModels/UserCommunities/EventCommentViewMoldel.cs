namespace Sphix.ViewModels.UserCommunities
{
   public class EventCommentViewMoldel
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public long EventId { get; set; }
        public string CommentText { get; set; }
        public long UserId { get; set; }
    }
}
