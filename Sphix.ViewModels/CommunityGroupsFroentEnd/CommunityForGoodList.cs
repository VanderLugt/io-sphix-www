
using Sphix.Utility;
using System;

namespace Sphix.ViewModels.CommunityGroupsFroentEnd
{
  public  class CommunityForGoodList
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        private string imageUrl = string.Empty;
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
        public string IsLive { get; set; }
        public string FooterLinkText { get; set; }
        public string Description { get; set; }
        public bool IsAlreadyJoined { get; set; }
        public string CommunityGroupURL { get; set; }
    }
}
