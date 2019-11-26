using System.Threading.Tasks;
using Data.Context;
using Sphix.DataModels.UserCommunitiesGroups;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;

namespace Sphix.Service.UserCommunities.OpenOfficeHours.OpenOfficeHoursMeetingsStatus
{
    public class OpenOfficeHoursMeetingsStatusService : IOpenOfficeHoursMeetingsStatusService
    {
        private UnitOfWork _unitOfWork;
        public OpenOfficeHoursMeetingsStatusService(EFDbContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }
        public async Task<BaseModel> SaveAsync(OpenOfficeHoursMeetingsStatusDataModel model)
        {
            if (model.CreatedBy == 0 && model.MeetingId==0)
            {
                return new BaseModel { Status = false,  Messsage = UMessagesInfo.Error };
            }
            if (model.Id == 0)
            {
                await _unitOfWork.OpenOfficeHoursMeetingsStatusRepository.Insert(model);
             
            }
            
            return new BaseModel { Status = true, Id = model.Id, Messsage = UMessagesInfo.RecordSaved };
        }
    }
}
