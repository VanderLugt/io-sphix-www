using System;
using System.Collections.Generic;
using System.Text;

namespace Sphix.ViewModels
{
   public class CustomeSearchFilter: SearchFilter
    {
        public string IsPublish { get; set; }
    }
    public class EventListSearchFilter : SearchFilter
    {
        public string CommunityGroupsId { get; set; }
    }
}
