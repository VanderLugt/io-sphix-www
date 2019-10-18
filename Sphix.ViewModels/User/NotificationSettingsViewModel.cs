using System;
using System.Collections.Generic;
using System.Text;

namespace Sphix.ViewModels.User
{
  public  class NotificationSettingsViewModel
    {
        public long UserId { get; set; }
        public bool BlogSubscription { get; set; }
        public bool Like { get; set; }
        public bool Comments { get; set; }
        public bool Followis { get; set; }
    }
}
