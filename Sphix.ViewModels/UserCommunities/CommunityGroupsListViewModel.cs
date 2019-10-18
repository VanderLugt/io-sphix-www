namespace Sphix.ViewModels.UserCommunities
{
   public class CommunityGroupsListViewModel: BaseModel
    {
        public long UserId { get; set; }
        public string Title { get; set; }
        public string PostDate { get; set; }
        public string IsPublish { get; set; }
        public string IsLive { get; set; }
    }
}
