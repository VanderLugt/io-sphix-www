using Sphix.Utility;

namespace Sphix.ViewModels.CommunityGroupsFroentEnd
{
   public class CommunityGroupsFroentEndDataView: BaseModel
    {
        public string Title { get; set; }
        public string CommunityName { get; set; }
        public string PostDate { get; set; }
        public string CommunityGroupURL { get; set; }
        public string Color { get; set; }
        private string imageUrl = string.Empty;
        private string headerLogoUrl = string.Empty;
        public string HeaderLogoUrl
        {
            get
            {
                if (headerLogoUrl == null)
                {
                    return headerLogoUrl;
                }
                else
                {
                    return UMessagesInfo.AWSPublicURL + headerLogoUrl;
                }

            }
            set { headerLogoUrl = value; }
        }
        public string ImageUrl
        {
            get
            {
                if (imageUrl == null)
                {
                    return imageUrl;
                }
                else
                {
                    return UMessagesInfo.AWSPublicURL + imageUrl;
                }
            }
            set
            {
                imageUrl = value;
            }
        }
        public string CommunityUrl { get; set; }
        public string FooterLinkText { get; set; }
    }
}
