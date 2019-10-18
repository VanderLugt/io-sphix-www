using System;
using System.ComponentModel.DataAnnotations;

namespace Sphix.ViewModels.EmailInvitation
{
  public class GroupEmailInvitationViewModel
    {
        public Guid Id { get; set; }
        public long SentByUser { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string ToEmailAddress { get; set; }
        //if SentTo value is 0 then it mean user is not register yet
        public long SentTo { get; set; }
        public long CommunityGroup { get; set; }
        public bool IsAccpeted { get; set; }
        public int ReSend { get; set; }
    }
}
