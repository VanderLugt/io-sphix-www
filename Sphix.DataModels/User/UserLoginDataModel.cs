using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.User
{
    [Table("UsersLogin")]
   public class UsersLoginDataModel
    {
        public UsersLoginDataModel()
        {
            LastLoginDateTime = DateTime.UtcNow;
        }
        [Key]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime LastLoginDateTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime AddedDate { get; set; }
        public long UpdatedBy { get; set; }
        public long CreatedBy { get; set; }
    }
}
