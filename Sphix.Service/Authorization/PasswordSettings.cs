using System;
using System.Collections.Generic;
using System.Text;

namespace Sphix.Service.Authorization
{
  public class PasswordSettings
    {
        //ISphixTool
        public string PassPhrase { get; set; }
        //SHA256
        public string HashAlgorithm { get; set; }
        //!2S0p1D9h9I556x7
        public string InitVector { get; set; }
    }
}
