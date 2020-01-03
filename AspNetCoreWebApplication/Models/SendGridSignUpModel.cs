using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApplication.Models
{
    public class SendGridSignupRequestModel
    {
        [Required]
        [Display(Name = "Community")]
        public int CommunityId { get; set; }
        public List<SelectListItem> Communities { get; set; }
        [Required]
        public string FirstName { get; set; }
        //public string LastName
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailId { get; set; }
    }
    public class SendGridSignupResponseModel
    {
        public int CommunityId { get; set; }
        public string FirstName { get; set; }
        //public string LastName
        public string Email { get; set; }
    }
}
