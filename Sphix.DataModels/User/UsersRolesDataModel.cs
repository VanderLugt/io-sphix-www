using Sphix.DataModels.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sphix.DataModels.User
{
    [Table("UsersRoles")]
    public class UsersRolesDataModel
    {
        public UsersRolesDataModel()
        {
            this.User = new UsersLoginDataModel();
            this.Role = new RoleDataModel();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual UsersLoginDataModel User { get; set; }
        public virtual RoleDataModel Role { get; set; }

    }
}
