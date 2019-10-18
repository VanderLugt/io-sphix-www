using System;
using System.Collections.Generic;
using System.Text;

namespace CustomTwilioClient
{
  public class TwilioVerifySettings
    {
        public string VerificationServiceSID { get; set; }
        public string AccountSID { get; set; }
        public string AuthToken { get; set; }
        public string APIKeySID { get; set; }
        public string APISecret { get; set; }
    }
}
