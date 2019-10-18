using Sphix.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sphix.Service.Authorization
{
  public interface IRoleService
    {
        Task<BaseModel> SaveAsync(RoleViewModel model);
        Task<BaseModel> Delete(long Id);
        Task<IList<RoleViewModel>> GetList(SearchFilter model);
    }
}
