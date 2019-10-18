using System;
using System.Collections.Generic;
using System.Text;

namespace Sphix.ViewModels.User
{
   public class EmailsViewModel
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
    }
}
