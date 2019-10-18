using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sphix.DataModels.User
{
    [Table("UsersProfile")]
   public class UsersProfileDataModel
    {
        public UsersProfileDataModel()
        {
            this.ModifiedDate = DateTime.Now;
            this.User = new UsersLoginDataModel();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual UsersLoginDataModel User { get; set; }
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        [MaxLength(50)]
        public string Street { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string Zip { get; set; }
        [MaxLength(50)]
        public int CountryId { get; set; }
        [MaxLength(200)]
        public string WebSite { get; set; }
        [MaxLength(200)]
        public string Company { get; set; }
        [MaxLength(20)]
        public string Latitude { get; set; }
        [MaxLength(20)]
        public string Longitude { get; set; }
        public bool? UseMyLocation { get; set; }
        [MaxLength(500)]
        public string ProfilePicture { get; set; }
        [MaxLength(200)]
        public string ProfileLink { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime AddedDate { get; set; }
        public long UpdatedBy { get; set; }
        public long CreatedBy { get; set; }
        public bool IsKickstartarActive { get; set; }
        public bool IsFinanciallyActive { get; set; }
        public bool IsCertifyTAndC { get; set; }

    }
}
