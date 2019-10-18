using Sphix.ViewModels;
using Sphix.ViewModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphix.Service.User.Associations
{
   public interface IAssociationsSettingService
    {
        Task<BaseModel> SaveAssociationsAsync(AssociationsModel model);
        Task<IList<AssociationsList>> getUserAssociationsAsync(long userId);
        Task<SignUpStep3ViewModel> getEditUserAssociationsAsync(int id, long userId);

    }
}
