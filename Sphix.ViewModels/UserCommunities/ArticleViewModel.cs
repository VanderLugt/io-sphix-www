
using System;

namespace Sphix.ViewModels.UserCommunities
{
   public class ArticleViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string ArticleTitle { get; set; }
        public string ShareDocument { get; set; }
        public string ArticleDescription { get; set; }
        public long CommunityGroupsId { get; set; }
        public DateTime PostDate { get; set; }
    }
}
