using Data.Context;
using Sphix.DataModels.User;
using Sphix.DataModels.UserCommunities;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities
{
   public class CommunityGroupEventService: ICommunityGroupEventService
    {
        private UnitOfWork _unitOfWork;
       // private readonly IAWSS3Bucket _awsS3Bucket;
        private readonly EFDbContext _context;
        public CommunityGroupEventService(EFDbContext context)
        {
            _unitOfWork = new UnitOfWork(context);
            _context = context;
        }
        
        public async Task<BaseModel> SaveEventAsync(LiveEventViewModel model)
        {
            if (string.IsNullOrEmpty(model.ETime))
            {
                return new BaseModel { Status=false,Messsage= "Please set the event time." };
            }
            model.EName = model.ETitle;
            if (model.Id==0)
            {
                UsersLoginDataModel _user = await _unitOfWork.UserLoginRepository.GetByID(model.UserId);
                CommunityGroupsDataModel communityGroupsData = await _unitOfWork.UserCommunityGroupsRepository.GetByID(model.CommunityGroupId);
                return await Save(model, _user, communityGroupsData);
            }
            else
            {
                return await Update(model);
            }
            
        }
        public async Task<IList<CommunityGroupEventsListViewModel>> getUserCommunityGroupEventsListAsync(EventListSearchFilter model)
        {
            IList<CommunityGroupEventsListViewModel> list = new List<CommunityGroupEventsListViewModel>();
            await _context.LoadStoredProc("GetUserCommunityGroupEventsList")
                       .WithSqlParam("UserId", model.Id)
                       .WithSqlParam("CommunityGroupsId", model.CommunityGroupsId)
                       .WithSqlParam("Start", model.PageNumber)
                       .WithSqlParam("PageLimit", model.PageLimit)
                       .WithSqlParam("OrderBy", model.OrderBy)
                       .WithSqlParam("SearchValue", model.SearchValue)
                       .ExecuteStoredProcAsync((handler) =>
                       {
                           list = handler.ReadToList<CommunityGroupEventsListViewModel>();
                           // do something with your results.
                       });
            return list;
        }
        public async Task<LiveEventViewModel> getEventDetailAsync(long eventId)
        {
            LiveEventViewModel liveEventViewModel = new LiveEventViewModel();
            if (eventId == 0)
            {
                return liveEventViewModel;
            }
            var _result=await _unitOfWork.UserCommunityLiveEventsRepository.GetByID(eventId);
            
            if (_result != null)
            {
                liveEventViewModel.EDescription = _result.EDescription;
                liveEventViewModel.EFrequency = _result.EFrequency;
                liveEventViewModel.EFromDate = _result.EFromDate;
                liveEventViewModel.EName = _result.EName;
                liveEventViewModel.ETime = _result.ETime;
                liveEventViewModel.ETimeDayName = _result.ETimeDayName;
                liveEventViewModel.ETimeZone = _result.ETimeZone;
                liveEventViewModel.ETitle = _result.ETitle;
                liveEventViewModel.EToDate = _result.EToDate;
                liveEventViewModel.Id = _result.Id;
                liveEventViewModel.MaxAttendees = _result.MaxAttendees;
                liveEventViewModel.Participants = _result.Participants;
                liveEventViewModel.Picture = _result.Picture;
                liveEventViewModel.WhoCanAttend = _result.WhoCanAttend;
            }
            return liveEventViewModel;
        }

        #region private function
        private async Task<BaseModel> Save(LiveEventViewModel model, UsersLoginDataModel user, CommunityGroupsDataModel communityGroupsData)
        {
            CommunityLiveEvents dataModel = new CommunityLiveEvents
            {
                CreatedBy = model.UserId,
                User = user,
                CommunityGroups = communityGroupsData,
                ETitle = model.ETitle,
                EName = model.EName,
                EDescription = model.EDescription,
                EFrequency = model.EFrequency,
                EFromDate = model.EFromDate,
                EToDate = model.EToDate,
                ETime = model.ETime,
                ETimeDayName = model.ETimeDayName,
                ETimeZone = model.ETimeZone,
                MaxAttendees = model.MaxAttendees,
                Observers=model.Observers,
                WhoCanAttend = model.WhoCanAttend,
                Participants = model.Participants,
                Picture = model.Picture,
                IsActive = true,
                IsSingleEvent=model.IsSingleEvent
            };
            await _unitOfWork.UserCommunityLiveEventsRepository.Insert(dataModel);
               return new BaseModel { Status = true, Id = dataModel.Id, Messsage = UMessagesInfo.RecordSaved };
        }
        private async Task<BaseModel> Update(LiveEventViewModel model)
        {
            CommunityLiveEvents dataModel =await _unitOfWork.UserCommunityLiveEventsRepository.GetByID(model.Id);

            dataModel.ETitle = model.ETitle;
            dataModel.EName = model.EName;
            dataModel.EDescription = model.EDescription;
            dataModel.EFrequency = model.EFrequency;
            dataModel.EFromDate = model.EFromDate;
            dataModel.EToDate = model.EToDate;
            dataModel.ETime = model.ETime;
            dataModel.ETimeDayName = model.ETimeDayName;
            dataModel.ETimeZone = model.ETimeZone;
            dataModel.MaxAttendees = model.MaxAttendees;
            dataModel.WhoCanAttend = model.WhoCanAttend;
            dataModel.Observers = model.Observers;
            dataModel.Participants = model.Participants;
            dataModel.Picture = model.Picture;
            dataModel.IsActive = true;
            dataModel.IsSingleEvent = model.IsSingleEvent;
            await _unitOfWork.UserCommunityLiveEventsRepository.Update(dataModel);
            return new BaseModel { Status = true, Id = dataModel.Id, Messsage = UMessagesInfo.RecordSaved };
        }
        #endregion
    }

}
