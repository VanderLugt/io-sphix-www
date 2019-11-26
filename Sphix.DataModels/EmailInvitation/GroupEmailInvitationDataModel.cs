using Sphix.DataModels.User;
using Sphix.DataModels.UserCommunities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.EmailInvitation
{
    [Table("GroupEmailInvitations")]
   public class GroupEmailInvitationDataModel
    {
        public GroupEmailInvitationDataModel()
        {
            LastUpdate = DateTime.UtcNow;
            SentByUser = new UsersLoginDataModel();
            CommunityGroup = new CommunityGroupsDataModel();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public UsersLoginDataModel SentByUser { get; set; }
        public string ToEmailAddress { get; set; }
        //if SentTo value is 0 then it mean user is not register yet
        public long SentTo { get; set; }
        public CommunityGroupsDataModel CommunityGroup { get; set; }
        public DateTime SentOn { get; set; }
        public DateTime AcceptedOn { get; set; }
        public bool IsAccpeted { get; set; }
        public int ReSend { get; set; }
        public DateTime LastUpdate { get; set; }
        
    }
}
