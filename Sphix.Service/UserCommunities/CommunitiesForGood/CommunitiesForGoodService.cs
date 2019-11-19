using Data.Context;
using Microsoft.EntityFrameworkCore;
using Sphix.UnitOfWorks;
using Sphix.ViewModels;
using Sphix.ViewModels.CommunityGroupsFroentEnd;
using Sphix.ViewModels.UserCommunities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities.CommunitiesForGood
{
  public  class CommunitiesForGoodService: ICommunitiesForGoodService
    {
        private UnitOfWork _unitOfWork;
        private readonly EFDbContext _context;
        public CommunitiesForGoodService(EFDbContext context)
        {
            _unitOfWork = new UnitOfWork(context);
            _context = context;
        }
        public async Task<IList<CommunityGroupsFroentEndDataView>> getCommunitiesByCategoryIdAsync(SearchFilter model)
        {
            IList<CommunityGroupsFroentEndDataView> list = new List<CommunityGroupsFroentEndDataView>();
            await this._context.LoadStoredProc("GetActiveCommunitiesGroupsList")
                       .WithSqlParam("CommunityId", model.Id)
                       .WithSqlParam("UserId", model.UserId)
                       .WithSqlParam("Start", model.PageNumber)
                       .WithSqlParam("PageLimit", model.PageLimit)
                       .WithSqlParam("OrderBy", model.OrderBy)
                       // .WithSqlParam("SearchValue", model.SearchValue)
                       .ExecuteStoredProcAsync((handler) =>
                       {
                           list = handler.ReadToList<CommunityGroupsFroentEndDataView>();
                           // do something with your results.
                       });
            return list;
        }
        public async Task<IList<CommunityForGoodList>> getCommunitiesForGood(long UserId)
        {
            IList<CommunityForGoodList> list = new List<CommunityForGoodList>();
            await _context.LoadStoredProc("GetCommunitiesForGood")
                       .WithSqlParam("UserId", UserId)
                       .ExecuteStoredProcAsync((handler) =>
                       {
                           list = handler.ReadToList<CommunityForGoodList>();
                           // do something with your results.
                       });
            long _lastId = 0;
            foreach (var item in list.ToList().OrderBy(c=>c.Id).OrderByDescending(c=>c.TotalMembers))
            {
                if (item.Id == _lastId)
                {
                    list.Remove(item);
                    _lastId = 0;
                }
                else
                {
                    _lastId = item.Id;
                }
            }
            return list;
        }
        public async Task<CmmunityGroupDetailViewModel> getCommunityGroupDetail(long Id)
        {
            var _query = _unitOfWork.UserCommunityGroupsRepository.FindAllByQuery(c => c.Id == Id && c.IsActive == true);
            var _result =await _query.Include("User").ToListAsync();
            
            CmmunityGroupDetailViewModel model = new CmmunityGroupDetailViewModel();
            if(_result != null && _result.Count!=0)
            {
                var _userProfile = await _unitOfWork.UserProfileRepository.FindAllBy(c => c.User.Id == _result[0].User.Id);
                var _comunityDetail = await _unitOfWork.CommunityRepository.GetByID(_result[0].CommunityId);
                model.Id = _result[0].Id;
                model.Title = _result[0].Title;
                if (_comunityDetail != null)
                {
                    model.Color = _comunityDetail.Color;
                    model.CommunityUrl = _comunityDetail.CommunityUrl;
                    model.FooterLinkText = _comunityDetail.FooterLinkText;
                    model.ImageUrl = _comunityDetail.ImageUrl;
                    model.HeaderLogoUrl = _comunityDetail.HeaderLogo;
                }
                else
                {
                    model.Color = "#4D0B0A";
                }
                model.Description = _result[0].Description;
                model.VideoUrl = _result[0].DescriptionVideoUrl;
                //set posted user detail
                model.UserId = _result[0].User.Id;
                model.EmailAddress = _result[0].User.UserName;
                if (_userProfile != null)
                {
                    model.FullName = _userProfile[0].FirstName + ' ' + _userProfile[0].LastName;
                    model.UserProfilePic = _userProfile[0].ProfilePicture;
                }
            }
            OpenOfficeHoursViewModel openOfficeHours = new OpenOfficeHoursViewModel();
            var _openOfficeHour = await _unitOfWork.UserCommunityOpenOfficeHoursRepository.FindAllBy(c => c.CommunityGroups.Id == Id && c.IsFirstMeeting==true);
            if (_openOfficeHour != null && _openOfficeHour.Count != 0)
            {
                var _resultOpenHoursMeeting = await _unitOfWork.JoinOpenHoursMeetingRepository.FindAllBy(c => c.User.Id == model.UserId && c.OpenOfficeHours.Id == _openOfficeHour[0].Id);
                if (_resultOpenHoursMeeting != null && _resultOpenHoursMeeting.Count != 0)
                {
                    openOfficeHours.OTimeZone = _resultOpenHoursMeeting[0].TimeZone;
                    openOfficeHours.IsRegisterInMeeting = true;
                }
                else
                {
                    openOfficeHours.OTimeZone = _openOfficeHour[0].OTimeZone;
                }
                openOfficeHours.OTime = _openOfficeHour[0].OTime;
                openOfficeHours.OTitle = _openOfficeHour[0].OTitle;
                openOfficeHours.Id = _openOfficeHour[0].Id;
                openOfficeHours.OFrequency = _openOfficeHour[0].OFrequency;
                openOfficeHours.OTimeDayName = _openOfficeHour[0].OTimeDayName;
                openOfficeHours.OFromDate = _openOfficeHour[0].OFromDate;
            }
            model.OpenOfficeHours = openOfficeHours;
                return model;
        }
        public async Task<IList<CommunityGroupEventsListViewModel>> getActiveCommunityGroupEventsListAsync(EventListSearchFilter model)
        {
            IList<CommunityGroupEventsListViewModel> list = new List<CommunityGroupEventsListViewModel>();
            await _context.LoadStoredProc("GetActiveCommunityGroupEventsList")
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
        public async Task<IList<CommunityGroupArticlesList>> getActiveCommunityGroupArticlesListAsync(EventListSearchFilter model)
        {
            IList<CommunityGroupArticlesList> list = new List<CommunityGroupArticlesList>();
            await _context.LoadStoredProc("GetActiveCommunityGroupArticlesList")
                       .WithSqlParam("CommunityGroupsId", model.CommunityGroupsId)
                       .WithSqlParam("Start", model.PageNumber)
                       .WithSqlParam("PageLimit", model.PageLimit)
                       .WithSqlParam("OrderBy", model.OrderBy)
                       .WithSqlParam("SearchValue", model.SearchValue)
                       .ExecuteStoredProcAsync((handler) =>
                       {
                           list = handler.ReadToList<CommunityGroupArticlesList>();
                           // do something with your results.
                       });
            return list;
        }
        public async Task<OpenOfficeHoursViewModel> getCommunityGroupOpenHoursDetail(long Id)
        {
            OpenOfficeHoursViewModel openOfficeHoursView = new OpenOfficeHoursViewModel();
            var _openHoursModel = await _unitOfWork.UserCommunityOpenOfficeHoursRepository.GetByID(Id);
            if (_openHoursModel != null)
            {
               // var _openHoursModel = openOfficeHours.FirstOrDefault();
                openOfficeHoursView.Id = _openHoursModel.Id;
                openOfficeHoursView.MaxAttendees = _openHoursModel.MaxAttendees;
                openOfficeHoursView.ODescription = _openHoursModel.ODescription;
                openOfficeHoursView.OFrequency = _openHoursModel.OFrequency;
                openOfficeHoursView.OFromDate = _openHoursModel.OFromDate;
                openOfficeHoursView.OToDate = _openHoursModel.OToDate;
                openOfficeHoursView.OName = _openHoursModel.OName;
                openOfficeHoursView.OTime = _openHoursModel.OTime;
                openOfficeHoursView.OTimeDayName = _openHoursModel.OTimeDayName;
                openOfficeHoursView.OTimeZone = _openHoursModel.OTimeZone;
                openOfficeHoursView.OTitle = _openHoursModel.OTitle;
                openOfficeHoursView.WhoCanAttend = _openHoursModel.WhoCanAttend;
            }
            return openOfficeHoursView;
         }
        public async Task<IList<OpenOfficeHoursTables>> getOpenOfficeHoursTables(long CommunityGroupsId,string TimeZone)
        {
            IList<OpenOfficeHoursTables> list = new List<OpenOfficeHoursTables>();
            await _context.LoadStoredProc("getOpenOfficeHoursTablesDetail")
                       .WithSqlParam("CommunityGroupsId", CommunityGroupsId)
                        .WithSqlParam("TimeZone", TimeZone)
                       .ExecuteStoredProcAsync((handler) =>
                       {
                           list = handler.ReadToList<OpenOfficeHoursTables>();
                           // do something with your results.
                       });
            return list;
        }
        public async Task<LiveEventViewModel> getCommunityGroupEventDetail(long Id)
        {
            LiveEventViewModel liveEventViewModel = new LiveEventViewModel();
            if (Id == 0)
            {
                return liveEventViewModel;
            }
            var _result = await _unitOfWork.UserCommunityLiveEventsRepository.GetByID(Id);

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
        public async Task<ArticleViewModel> getArticleDetailAsync(long Id)
        {
            ArticleViewModel articleViewModel = new ArticleViewModel();
            if (Id == 0)
            {
                return articleViewModel;
            }
            var _result = await _unitOfWork.UserCommunityArticlesRepository.GetByID(Id);

            if (_result != null && _result.IsActive)
            {
                articleViewModel.ArticleTitle = _result.Title;
                articleViewModel.ArticleDescription = _result.Description;
                articleViewModel.Id = _result.Id;
                articleViewModel.ShareDocument = _result.ShareDocUrl;
                articleViewModel.PostDate = _result.AddedDate;


            }
            return articleViewModel;
        }
    }
}
