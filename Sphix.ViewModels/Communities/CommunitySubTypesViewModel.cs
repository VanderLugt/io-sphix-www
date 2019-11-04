using System.ComponentModel.DataAnnotations;

namespace Sphix.ViewModels.Communities
{
   public class CommunitySubTypesViewModel
    {
        public int Id { get; set; }
        public int CommunityId { get; set; }
        public int Type { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
