using Data.Context;
using Microsoft.EntityFrameworkCore;
using Sphix.DataModels.User;
using Sphix.DataModels.UserCommunities;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;
using System;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities.OpenOfficeHours
{
    public class OpenOfficeHoursService:IOpenOfficeHoursService
    {
        private UnitOfWork _unitOfWork;
        private readonly EFDbContext _context;
        public OpenOfficeHoursService(EFDbContext context)
        {
            _unitOfWork = new UnitOfWork(context);
            _context = context;
        }
        public async Task<CommunityOpenOfficeHours> getOpenHoursAsync(long Id)
        {
            return await _unitOfWork.UserCommunityOpenOfficeHoursRepository.GetByID(Id);
        }
        public async Task<CommunityOpenOfficeHours> getOpenHoursAsync(string token)
        {
            var _query =  _unitOfWork.UserCommunityOpenOfficeHoursRepository.FindAllByQuery(c => c.NextMeetingToken == token);
            var _result = await _query.Include("CommunityGroups").ToListAsync();
            return _result[0];
        }
        /// <summary>
        /// Save and update data in Open Office Hours table
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user">if user model is null then please pass UserId into the OpenOfficeHoursViewModel</param>
        /// <param name="communityGroupsData">if communityGroupsData model is null then please pass CommunityGroupId into the OpenOfficeHoursViewModel</param>
        /// <returns></returns>
        public async Task<BaseModel> SaveOpenHoursAsync(OpenOfficeHoursViewModel model, UsersLoginDataModel user, CommunityGroupsDataModel communityGroupsData)
        {
            if(user==null)
            {
                user = await _unitOfWork.UserLoginRepository.GetByID(model.UserId);
            }
            if(communityGroupsData==null)
            {
                communityGroupsData = await _unitOfWork.UserCommunityGroupsRepository.GetByID(model.CommunityGroupId);
            }
            if (model.Id == 0)
            {
                CommunityOpenOfficeHours dataModel = new CommunityOpenOfficeHours
                {
                    CreatedBy = user.Id,
                    User = user,
                    CommunityGroups = communityGroupsData,
                    OTitle = model.OTitle,
                    OName = model.OName,
                    ODescription = model.ODescription,
                    OFrequency = model.OFrequency,
                    OFromDate = setDateFromDayName(model.OTimeDayName, model.OFromDate),
                    OToDate = setDateFromDayName(model.OTimeDayName, model.OFromDate),
                    OTime = model.OTime,
                    OTimeDayName = model.OTimeDayName,
                    OTimeZone = model.OTimeZone,
                    MaxAttendees = model.MaxAttendees,
                    WhoCanAttend = model.WhoCanAttend,
                    IsActive = true,
                    IsFirstMeeting = model.IsFirstMeeting,
                    NextMeetingToken = model.NextMeetingToken,
                    LastSessionId=model.LastSessionId,
                    AddHours=model.AddHours
                    
                };
                await _unitOfWork.UserCommunityOpenOfficeHoursRepository.Insert(dataModel);
                if (model.AddHours)
                {
                   await _context.LoadStoredProc("AddHoursInOpenOfficeHours")
                        .WithSqlParam("Id", dataModel.Id)
                       .ExecuteStoredProcAsync((handler) =>
                       {
                           // do something with your results.
                       });

                }
                return new BaseModel { Status = true, Id = dataModel.Id, Messsage = UMessagesInfo.RecordSaved };
            }
            else
            {
                var openOfficeHoursModel = await _unitOfWork.UserCommunityOpenOfficeHoursRepository.GetByID(model.Id);
                openOfficeHoursModel.OTitle = model.OTitle;
                openOfficeHoursModel.OName = model.OName;
                openOfficeHoursModel.ODescription = model.ODescription;
                openOfficeHoursModel.OFrequency = model.OFrequency;
                openOfficeHoursModel.OFromDate = setDateFromDayName(model.OTimeDayName, model.OFromDate);
                openOfficeHoursModel.OToDate = openOfficeHoursModel.OFromDate;
                openOfficeHoursModel.OTime = model.OTime;
                openOfficeHoursModel.OTimeDayName = model.OTimeDayName;
                openOfficeHoursModel.OTimeZone = model.OTimeZone;
                openOfficeHoursModel.MaxAttendees = model.MaxAttendees;
                openOfficeHoursModel.WhoCanAttend = model.WhoCanAttend;
                openOfficeHoursModel.IsActive = true;
                //openOfficeHoursModel.AddHours = model.AddHours;
                //openOfficeHoursModel.IsFirstMeeting = model.IsFirstMeeting;
                await _unitOfWork.UserCommunityOpenOfficeHoursRepository.Update(openOfficeHoursModel);
                return new BaseModel { Status = true, Id = model.Id, Messsage = UMessagesInfo.RecordSaved };
            }

        }
        public async Task<BaseModel> CheckTableIsExist(long userId,long communityId,string TimeZone,DateTime fromDateTime)
        {
            var _result = await _unitOfWork.UserCommunityOpenOfficeHoursRepository.FindAllBy(c => c.User.Id == userId && c.CommunityGroups.Id == communityId && c.OTimeZone == TimeZone && c.OFromDate.ToShortDateString()== fromDateTime.ToShortDateString());
            if (_result.Count > 0)
            {
                return new BaseModel { Status = true, Messsage = UMessagesInfo.RecordExist };
            }
            else
            {
                return new BaseModel { Status =false };
            }
        }
        /// <summary>
        /// This function will set IsMeetingTokenUsed=true this mean this token is used for create next week meeting
        /// </summary>
        /// <param name="Id">Open Office Hours Id</param>
        /// <returns>Base Model</returns>
        public async Task<BaseModel> UpdateNextMeetingToken(long Id)
        {
            var openOfficeHoursModel = await _unitOfWork.UserCommunityOpenOfficeHoursRepository.GetByID(Id);
            openOfficeHoursModel.IsMeetingTokenUsed = true;
            await _unitOfWork.UserCommunityOpenOfficeHoursRepository.Update(openOfficeHoursModel);
            //IsMeetingTokenUsed
            return new BaseModel { Status = true, Id = openOfficeHoursModel.Id, Messsage = UMessagesInfo.RecordSaved };
        }
        private DateTime setDateFromDayName(string dayName, DateTime date)
        {
            DateTime todayDate = DateTime.Now;
            var days = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday",
                          "Saturday", "Sunday" };
            int _dayIndex = Array.IndexOf(days, dayName);
            int _todayDayIndex = Array.IndexOf(days, date.DayOfWeek.ToString());
            if (date.Date != todayDate.Date)
            {
                if (_todayDayIndex > _dayIndex)
                {
                    _dayIndex = (7 - (_todayDayIndex - _dayIndex));
                    return date.AddDays(_dayIndex);
                }
                return date.AddDays(_dayIndex - 1);
            }
            return date;
        }
    }
}
