using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sphix.ViewModels
{
    public class RoleViewModel : BaseModel
    {
        [MaxLength(50)]
        [Required]
        public string RoleName { get; set; }
        [MaxLength(50)]
        public string RoleColor { get; set; }
        [MaxLength(1500)]
        public string Description { get; set; }
    }
}
