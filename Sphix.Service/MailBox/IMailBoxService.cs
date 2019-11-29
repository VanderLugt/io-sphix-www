using Sphix.DataModels.Logger;
using Sphix.ViewModels;
using System.Threading.Tasks;

namespace Sphix.Service.MailBox
{
    public interface IMailBoxService
    {
        Task<BaseModel> SaveAsync(MailSentBoxDataModel model);
    }
}
