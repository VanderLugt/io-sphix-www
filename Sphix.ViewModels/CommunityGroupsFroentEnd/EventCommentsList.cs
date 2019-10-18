using System;
namespace Sphix.ViewModels.CommunityGroupsFroentEnd
{
  public class EventCommentsList
    {
        public Int64 Id { get; set; }
        public Int64 ParentId { get; set; }
        public string CommentText { get; set; }
        public string CommentedDate { get; set; }
        public Int64 CommentedById { get; set; }
        public string UserName { get; set; }
        public Int64 LoggedInUserId { get; set; }
        public bool IsDeletedMessage { get; set; }
    }
}
