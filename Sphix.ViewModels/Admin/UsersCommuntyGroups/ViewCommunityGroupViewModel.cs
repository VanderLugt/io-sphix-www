using Sphix.Utility;
using System.Collections.Generic;

namespace Sphix.ViewModels.UserCommunities
{
   public class ViewCommunityGroupViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        private string descriptionVideoUrl = string.Empty;
        public bool IsPublish { get; set; }
        public string DescriptionVideoUrl
        {
            get
            {
                if (descriptionVideoUrl == null)
                {
                    return descriptionVideoUrl;
                }
                else
                {
                    return UMessagesInfo.AWSPublicURL + descriptionVideoUrl;
                }

            }
            set { descriptionVideoUrl = value; }
        }
        public string CommunityName { get; set; }
        public string TargetedGroups { get; set; }
        public string Themes { get; set; }
        public OpenOfficeHoursViewModel OpenOfficeHours { get; set; }
        public List<LiveEventViewModel> liveEvents { get; set; }
        public List<ArticleViewModel> articles { get; set; }
    }
}
