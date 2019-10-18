using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.Authorization
{
    [Table("RoleMaster")]
    public class RoleDataModel
    {
        public RoleDataModel()
        {
            AddedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string RoleName { get; set; }
        [MaxLength(50)]
        public string RoleColor { get; set; }
        [MaxLength(1500)]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime AddedDate { get; set; }
        public long UpdatedBy { get; set; }
        public long CreatedBy { get; set; }
    }
}
