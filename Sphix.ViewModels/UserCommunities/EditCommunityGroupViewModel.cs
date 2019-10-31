using Sphix.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sphix.ViewModels.UserCommunities
{
   public class EditCommunityGroupViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        private string descriptionVideoUrl = string.Empty;
        public string DescriptionVideoUrl
        {
            get
            {
                if (descriptionVideoUrl == null)
                {
                    return descriptionVideoUrl;
                }
                else
                {
                    return UMessagesInfo.AWSPublicURL + descriptionVideoUrl;
                }

            }
            set { descriptionVideoUrl = value; }
        }
        public int OgranizationsId { get; set; }
        public string CommunityTargetedGroupId { get; set; }
        public string ThemesId { get; set; }
        public string AssociationId { get; set; }
        public string Type1Id { get; set; }
        public string Type2Id { get; set; }
        public string TargetedInterestIds { get; set; }
        public bool IsPublicGroup { get; set; }
        public OpenOfficeHoursViewModel OpenOfficeHours { get; set; }
    }
}
