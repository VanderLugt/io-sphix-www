using Sphix.DataModels.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.Authorization
{
    [Table("UsersEmailVerifications")]
    public class EmailVerificationDataModel
    {
        public EmailVerificationDataModel()
        {
            AddedDate = DateTime.UtcNow;
           // VerificationDate = DateTime.UtcNow;
            this.User = new UsersLoginDataModel();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual UsersLoginDataModel User { get; set; }
        [MaxLength(100)]
        [Required]
        public string EmailAddress { get; set; }
        [MaxLength(200)]
        public string VerificationCode { get; set; }
        public bool IsVerified { get; set; }
        public DateTime VerificationDate { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
