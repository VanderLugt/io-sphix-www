using Sphix.DataModels.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.Authorization
{
    [Table("UsersRestPasswordLink")]
    public class RestPasswordLinkDataModel
    {
        public RestPasswordLinkDataModel()
        {
            CreatedDate = DateTime.Now;
            this.User = new UsersLoginDataModel();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual UsersLoginDataModel User { get; set; }
        [MaxLength(200)]
        public string VerificationCode { get; set; }
        public bool IsRequestToRestPassword { get; set; }
        public bool IsRequestHasUsed { get; set; }
        public DateTime VerificationDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
