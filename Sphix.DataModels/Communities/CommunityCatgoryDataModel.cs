using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.Communities
{
    [Table("CommunitiesCatgories")]
   public class CommunityCatgoryDataModel
    {
        
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CommunityId { get; set; }
        public int Type { get; set; }
        [MaxLength(200)]
        [Required]
        public string Name { get; set; }
        [MaxLength(1500)]
        public string Description { get; set; }
        [MaxLength(50)]
        public string Color { get; set; }
        [MaxLength(200)]
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        
    }
}
