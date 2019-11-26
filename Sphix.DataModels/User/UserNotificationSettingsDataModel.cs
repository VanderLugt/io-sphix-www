using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.User
{
  [Table("UsersNotificationSettings")]
  public class UserNotificationSettingsDataModel
    {
        public UserNotificationSettingsDataModel()
        {
            this.User = new UsersLoginDataModel();
            ModifiedDate = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual UsersLoginDataModel User { get; set; }
        public bool BlogSubscription { get; set; }
        public bool Like { get; set; }
        public bool Comments { get; set; }
        public bool Followis { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime AddedDate { get; set; }
        public long UpdatedBy { get; set; }
        public long CreatedBy { get; set; }
    }
}
