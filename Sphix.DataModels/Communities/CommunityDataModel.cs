using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sphix.DataModels.Communities
{
    [Table("Communities")]
   public class CommunityDataModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(1500)]
        public string Description { get; set; }
        [MaxLength(50)]
        public string Color { get; set; }
        [MaxLength(200)]
        public string ImageUrl { get; set; }
        [MaxLength(200)]
        public string CommunityUrl { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(200)]
        public string FooterLinkText { get; set; }
        public int DisplayIndex { get; set; }
    }
}
