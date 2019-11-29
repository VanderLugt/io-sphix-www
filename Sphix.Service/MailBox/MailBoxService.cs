using System.Threading.Tasks;
using Data.Context;
using Sphix.DataModels.Logger;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;

namespace Sphix.Service.MailBox
{
    public class MailBoxService:IMailBoxService
    {
        private UnitOfWork _unitOfWork;
        public MailBoxService(EFDbContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }
        public async Task<BaseModel> SaveAsync(MailSentBoxDataModel model)
        {
            await _unitOfWork.MailSentBoxRepository.Insert(model);
            return new BaseModel { Status = true, Messsage = UMessagesInfo.RecordSaved };
        }
    }
}
