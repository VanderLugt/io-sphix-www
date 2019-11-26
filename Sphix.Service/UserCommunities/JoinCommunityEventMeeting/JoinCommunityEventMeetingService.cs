using Data.Context;
using Sphix.DataModels.UserCommunitiesGroups.Join;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;
using System;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities.JoinCommunityEventMeeting
{
   public class JoinCommunityEventMeetingService : IJoinCommunityEventMeetingService
    {
        private UnitOfWork _unitOfWork;
        public JoinCommunityEventMeetingService(EFDbContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }
        public async Task<BaseModel> SaveAsync(JoinCommunityOpenHoursMeetingViewModel model)
        {
            try
            {
                var _result = await _unitOfWork.JoinEventMeetingRepository.FindAllBy(c => c.User.Id == model.UserId && c.LiveEvent.Id == model.OpenOfficeHoursId);
                if (_result != null && _result.Count != 0)
                {
                    return new BaseModel { Status = false, Messsage = UMessagesInfo.MeetingJoinAlready };
                }
                if (model.OpenOfficeHoursId == 0)
                {
                    return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };
                }
                JoinEventMeetingDataModel joinEventMeeting = new JoinEventMeetingDataModel();

                joinEventMeeting.LiveEvent = await _unitOfWork.UserCommunityLiveEventsRepository.GetByID(model.OpenOfficeHoursId);
                joinEventMeeting.User = await _unitOfWork.UserLoginRepository.GetByID(model.UserId);
                joinEventMeeting.IsJoined = true;
                joinEventMeeting.JoinDateTime = DateTime.UtcNow;
                joinEventMeeting.TimeZone = model.TimeZone;
                await _unitOfWork.JoinEventMeetingRepository.Insert(joinEventMeeting);
                return new BaseModel { Id = joinEventMeeting.Id, Status = true, Messsage = UMessagesInfo.RecordSaved,Data=joinEventMeeting.LiveEvent.EFromDate.ToShortDateString()+" at"+joinEventMeeting.LiveEvent.ETime+" "+joinEventMeeting.LiveEvent.ETimeZone };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
