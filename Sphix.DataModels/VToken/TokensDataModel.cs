using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.VToken
{
    [Table("VerificationTokens")]
    public class TokensDataModel
    {
        public TokensDataModel()
        {
            CreatedOn = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(200)]
        public string Token { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime TokenUsedOn { get; set; }
        public long TokenForId { get; set; }
        [MaxLength(100)]
        public string TokenForTableName { get; set; }
    }
}
