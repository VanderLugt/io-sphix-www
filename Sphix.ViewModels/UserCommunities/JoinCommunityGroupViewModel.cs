
namespace Sphix.ViewModels.UserCommunities
{
   public class JoinCommunityGroupViewModel
    {
        public long Id { get; set; }
        public long CommunityGroupId { get; set; }
        public long UserId { get; set; }
        public bool IsJoin { get; set; }
    }
}
