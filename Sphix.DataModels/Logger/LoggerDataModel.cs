using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.Logger
{
    [Table("LoggerManager")]
    public class LoggerDataModel
    {
        public LoggerDataModel()
        {
            AddedDate = DateTime.Now;
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(500)]
        public string Source { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
        [MaxLength(10)]
        public string ErrorCode { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
