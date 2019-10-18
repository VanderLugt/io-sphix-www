using Sphix.Utility;

namespace Sphix.ViewModels.Communities
{
   public class CommunityTypesListViewModel:BaseModel
    {
        public string Name { get; set; }
        public string Color { get; set; }
        private string imageUrl = string.Empty;
        public string ImageUrl {
            get {
                if (imageUrl == null)
                {
                    return imageUrl;
                }
                else
                {
                    return UMessagesInfo.AWSPublicURL + imageUrl;
                }
            }
            set {
                imageUrl = value;
                }
        }
        public string Description { get; set; }
        public string CommunityUrl { get; set; }
        public string IsLive { get; set; }
        public string FooterLinkText { get; set; }
    }
}
