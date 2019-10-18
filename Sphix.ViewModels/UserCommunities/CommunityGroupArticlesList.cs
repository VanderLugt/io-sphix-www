namespace Sphix.ViewModels.UserCommunities
{
   public class CommunityGroupArticlesList: BaseModel
    {
        public long UserId { get; set; }
        public string Title { get; set; }
        public string IsLive { get; set; }
        public string PostDate { get; set; }
        public long CommunityGroupsId { get; set; }
    }
}
