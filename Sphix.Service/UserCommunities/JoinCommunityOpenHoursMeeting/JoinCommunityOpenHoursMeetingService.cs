using System;
using System.Threading.Tasks;
using Data.Context;
using Sphix.DataModels.UserCommunitiesGroups.Join;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;

namespace Sphix.Service.UserCommunities.JoinCommunityOpenHoursMeeting
{
    public class JoinCommunityOpenHoursMeetingService : IJoinCommunityOpenHoursMeetingService
    {
        private UnitOfWork _unitOfWork;
        public JoinCommunityOpenHoursMeetingService(EFDbContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }
        public async Task<BaseModel> SaveAsync(JoinCommunityOpenHoursMeetingViewModel model)
        {
            try
            {
                if (model.UserId == 0)
                {
                    return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };
                }
                var _result = await _unitOfWork.JoinOpenHoursMeetingRepository.FindAllBy(c => c.User.Id == model.UserId && c.OpenOfficeHours.Id == model.OpenOfficeHoursId);
                if (_result != null && _result.Count != 0)
                {
                    return new BaseModel { Status = false,Messsage=UMessagesInfo.MeetingJoinAlready };
                }
                
                JoinOpenHoursMeetingDataModel joinOpenHoursMeeting = new JoinOpenHoursMeetingDataModel();

                joinOpenHoursMeeting.OpenOfficeHours = await _unitOfWork.UserCommunityOpenOfficeHoursRepository.GetByID(model.OpenOfficeHoursId);
                joinOpenHoursMeeting.User = await _unitOfWork.UserLoginRepository.GetByID(model.UserId);
                joinOpenHoursMeeting.IsJoined = true;
                joinOpenHoursMeeting.JoinDateTime = DateTime.UtcNow;
                joinOpenHoursMeeting.TimeZone = model.TimeZone;
                await _unitOfWork.JoinOpenHoursMeetingRepository.Insert(joinOpenHoursMeeting);
                return new BaseModel { Id = joinOpenHoursMeeting.Id, Status = true, Messsage = UMessagesInfo.RecordSaved,Data = joinOpenHoursMeeting.OpenOfficeHours.OFromDate.ToShortDateString()+" at "+joinOpenHoursMeeting.OpenOfficeHours.OTime +" "+ joinOpenHoursMeeting.OpenOfficeHours.OTimeZone };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
