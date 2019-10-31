using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sphix.ViewModels.UserCommunities
{
   public class CommunityGroupViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DescriptionVideoUrl { get; set; }
        public string OpenOfficeHours { get; set; }
        public string LiveEvent { get; set; }
        public string Article { get; set; }
        public int OgranizationsId { get; set; }
        public string CommunityTargetedGroupId { get; set; }
        public string ThemesId { get; set; }
        public string AssociationId { get; set; }
        //public string Type1Id { get; set; }
        //public string Type2Id { get; set; }
        public string TargetedInterestIds { get; set; }
        public bool IsPublicGroup { get; set; }
    }
    
}
