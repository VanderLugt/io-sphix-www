using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.Logger
{
    [Table("MailSentBox")]
    public class MailSentBoxDataModel
    {
        public MailSentBoxDataModel()
        {
            SentDateTime = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public long SentForId { get; set; }
        [MaxLength(100)]
        public string SentForTableName { get; set; }
        [MaxLength(500)]
        public string ToEMailId { get; set; }
        [MaxLength(100)]
        public string FromEMailId { get; set; }
        [MaxLength(100)]
        public string MessageType { get; set; }
        [MaxLength(200)]
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime SentDateTime { get; set; }
        public DateTime ReadDateTime { get; set; }
        public bool IsSent { get; set; }
        public bool IsRead { get; set; }
        public long SentBy { get; set; }
        public long SentToUserId { get; set; }
    }
}
