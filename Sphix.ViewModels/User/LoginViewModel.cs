using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sphix.ViewModels.User
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string UserName { get; set; }
    }
   public class LoginViewModel: ForgotPasswordViewModel
    {
        
        [Required]
        public string Password { get; set; }
    }
    public class ResetPaswordViewModel: LoginViewModel
    {
        [Required]
        public string Token { get; set; }
    }
}
