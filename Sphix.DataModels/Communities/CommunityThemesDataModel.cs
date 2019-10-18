using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.Communities
{
    [Table("CommunityThemes")]
    public class CommunityThemesDataModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CommunityId { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(1500)]
        public string Description { get; set; }
        [MaxLength(50)]
        public string Color { get; set; }
        [MaxLength(500)]
        public string BackGroundImage { get; set; }
        public bool IsActive { get; set; }
    }
}
