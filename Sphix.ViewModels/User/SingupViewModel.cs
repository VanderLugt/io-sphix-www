using Sphix.Utility;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sphix.ViewModels.User
{
   public class SignUpViewModel: BaseModel
    {
       
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string CommunityId { get; set; }
        public int GroupId { get; set; }
        public int AssociationId { get; set; }
        public int Type1ListId { get; set; }
        public int Type2ListId { get; set; }
        public string InterestsId { get; set; }
        public bool UseMyLocation { get; set; }
        public bool IsKickstartarActive { get; set; }
        public bool IsFinanciallyActive { get; set; }
        public bool IsCertifyTAndC { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ProfileLink { get; set; }

    }
    public class UserShortProfileViewModel : BaseModel
    {
        
        //private readonly IHostingEnvironment _env;
        private string profilePicture = "/img/avatar.png";
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Roles { get; set; }
        public string ProfilePicture 
        {
            get
            {
                if (profilePicture == null)
                {
                    profilePicture = "/img/avatar.png";
                    return profilePicture;
                }
                else {

                    if (profilePicture.Contains("/img/avatar.png")) {
                        return profilePicture;
                    }
                    else {
                        return UMessagesInfo.AWSPublicURL + profilePicture;
                    }
                    
                }

            }
            set { profilePicture = value; }

        }
        public List<CommunityGroups> communityGroups { get; set; }
    }
    public class CommunityGroups
    {
        public string Name { get; set; }
        public long Id { get; set; }
    }
}
