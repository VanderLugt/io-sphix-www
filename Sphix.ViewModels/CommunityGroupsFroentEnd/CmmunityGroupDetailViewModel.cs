using Sphix.Utility;
using Sphix.ViewModels.UserCommunities;

namespace Sphix.ViewModels.CommunityGroupsFroentEnd
{
   public class CmmunityGroupDetailViewModel: BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }
        private string videoUrl = string.Empty;
        public string VideoUrl {
            get {
                if (string.IsNullOrEmpty(videoUrl))
                {
                    return videoUrl;
                }
                else
                {
                    return UMessagesInfo.AWSPublicURL + videoUrl;
                }

            }
            set { videoUrl = value; }
        }
        private string profilePic = string.Empty;
        public string UserProfilePic {
            get {
                if (string.IsNullOrEmpty(profilePic))
                {
                    return profilePic;
                }
                else
                {
                    return UMessagesInfo.AWSPublicURL + profilePic;
                }
            }
            set { profilePic = value; }
        }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Color { get; set; }
        public string CommunityName { get; set; }
        public string CommunityUrl { get; set; }
        public OpenOfficeHoursViewModel OpenOfficeHours { get; set; }
    }
}
