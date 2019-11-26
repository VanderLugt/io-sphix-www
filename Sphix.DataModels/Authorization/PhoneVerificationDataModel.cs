using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.Authorization
{
    [Table("UsersPhoneVerification")]
    public class PhoneVerificationDataModel
    {
      
        public PhoneVerificationDataModel()
        {
            AddedDate = DateTime.UtcNow;
            VerificationDate = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(20)]
        [Required]
        public string PhoneNumber { get; set; }
        [MaxLength(10)]
        public string VerificationCode { get; set; }
        public bool IsVerified { get; set; }
        public DateTime VerificationDate { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
