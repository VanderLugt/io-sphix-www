using System;
using System.Collections.Generic;
using System.Text;

namespace Sphix.ViewModels.User
{
  public class UserClaimsViewModel: BaseModel
    {
        public string UserName { get; set; }
        public string Roles { get; set; }
    }
}
