using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sphix.ViewModels.User
{
    public class UserProfileViewModel : BaseModel
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileLink { get; set; }
        public string UserName { get; set; }
        public bool IsVerified { get; set; }
    }

}
