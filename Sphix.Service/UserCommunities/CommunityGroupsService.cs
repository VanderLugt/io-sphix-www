using AWSS3.Utility;
using Data.Context;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Sphix.DataModels.User;
using Sphix.DataModels.UserCommunities;
using Sphix.DataModels.UserCommunitiesGroups;
using Sphix.Service.Logger;
using Sphix.Service.UserCommunities.OpenOfficeHours;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities
{
    public class CommunityGroupsService : ICommunityGroupsService
    {
        private UnitOfWork _unitOfWork;
        private readonly IAWSS3Bucket _awsS3Bucket;
        private readonly EFDbContext _context;
        private readonly IOpenOfficeHoursService _openOfficeHoursService;
        private readonly ILoggerService _logger; 
        public CommunityGroupsService(EFDbContext context
            , IAWSS3Bucket awsS3Bucket
            ,IOpenOfficeHoursService openOfficeHoursService
            , ILoggerService logger
            )
        {
           _unitOfWork = new UnitOfWork(context);
            _context = context;
           _awsS3Bucket = awsS3Bucket;
            _openOfficeHoursService = openOfficeHoursService;
            _logger = logger;
        }
        public async Task<IList<CommunityGroupsListViewModel>> getCommunitiesGroupsList(SearchFilter model)
        {
            IList<CommunityGroupsListViewModel> list = new List<CommunityGroupsListViewModel>();
            await _context.LoadStoredProc("GetUserCommunitiesGroupsList")
                       .WithSqlParam("UserId", model.Id)
                       .WithSqlParam("Start", model.PageNumber)
                       .WithSqlParam("PageLimit", model.PageLimit)
                       .WithSqlParam("OrderBy", model.OrderBy)
                       .WithSqlParam("SearchValue", model.SearchValue)
                       .ExecuteStoredProcAsync((handler) =>
                       {
                           list = handler.ReadToList<CommunityGroupsListViewModel>();
                               // do something with your results.
                           });
            return list;
        }

        public async Task<IList<CommunityGroupsListViewModel>> getAdminCommunitiesGroupsList(CustomeSearchFilter model)
        {
            IList<CommunityGroupsListViewModel> list = new List<CommunityGroupsListViewModel>();
            await _context.LoadStoredProc("GetAdminCommunitiesGroupsList")
                       .WithSqlParam("Start", model.PageNumber)
                       .WithSqlParam("PageLimit", model.PageLimit)
                       .WithSqlParam("OrderBy", model.OrderBy)
                       .WithSqlParam("SearchValue", model.SearchValue)
                       .WithSqlParam("IsPublish", model.IsPublish)
                       .ExecuteStoredProcAsync((handler) =>
                       {
                           list = handler.ReadToList<CommunityGroupsListViewModel>();
                           // do something with your results.
                       });
            return list;
        }
        public async Task<IList<ViewCommunityGroupViewModel>> getCommunityGroupDetailAdmin(long Id)
        {
            IList<ViewCommunityGroupViewModel> list = new List<ViewCommunityGroupViewModel>();
            await _context.LoadStoredProc("getCommunityGroupViewAdmin")
                 .WithSqlParam("Id", Id)
                 .ExecuteStoredProcAsync((handler) =>
                 {
                     list = handler.ReadToList<ViewCommunityGroupViewModel>();
                 });
            if (list.Count > 0)
            {
                OpenOfficeHoursViewModel openOfficeHoursView = new OpenOfficeHoursViewModel();
                var openOfficeHours = await _unitOfWork.UserCommunityOpenOfficeHoursRepository.FindAllBy(c => c.CommunityGroups.Id == Id  );
                if (openOfficeHours.Count != 0)
                {
                    var _openHoursModel = openOfficeHours.FirstOrDefault();
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
                 list[0].OpenOfficeHours = openOfficeHoursView;

                //get events
                var events =await _unitOfWork.UserCommunityLiveEventsRepository.FindAllBy(c => c.CommunityGroups.Id == Id);
                List<LiveEventViewModel> liveEventViews = new List<LiveEventViewModel>();
                foreach (var item in events)
                {
                    liveEventViews.Add(new LiveEventViewModel {
                        Id=item.Id,
                        ETitle=item.ETitle,
                        ETime=item.ETime,
                        EName=item.EName,
                        EFromDate=item.EFromDate,
                        EFrequency=item.EFrequency,
                        EDescription=item.EDescription,
                        ETimeDayName=item.ETimeDayName,
                        ETimeZone=item.ETimeZone,
                        EToDate=item.EToDate,
                        MaxAttendees=item.MaxAttendees,
                        Participants=item.Participants,
                        WhoCanAttend=item.WhoCanAttend
                    });
                }
                list[0].liveEvents = liveEventViews;

                var articles =await _unitOfWork.UserCommunityArticlesRepository.FindAllBy(c => c.CommunityGroups.Id == Id);
                List<ArticleViewModel> articleViews = new List<ArticleViewModel>();
                foreach (var item in articles)
                {
                    articleViews.Add(new ArticleViewModel {
                        ArticleDescription=item.Description,
                         ArticleTitle=item.Title,
                         Id=item.Id,
                         ShareDocument=item.ShareDocUrl
                    });
                }
                list[0].articles = articleViews;
            }
            return list;
        }
        public async Task<BaseModel> SaveAsync(CommunityGroupViewModel model, IFormFile file, IFormFile aticleSharedoc)
        {
            BaseModel baseModel = new BaseModel();
            string articeDocUrl = string.Empty;
            
            var userData = await _unitOfWork.UserLoginRepository.GetByID(model.UserId);
            if (userData == null)
            {
                return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };
            }

            try
            {
                    if (file!=null && file.Length > 0) { 
                        string folderPath = "CommunityGroups/" + DateTime.Now.Year.ToString();
                        string fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);

                        string articleFolderPath = "ArcticlesShareDocuments/" + DateTime.Now.Year.ToString();
                        string articleFileName = string.Empty;
                         
                                if (file.Length > 0)
                                {
                                    model.DescriptionVideoUrl = folderPath + "/" + fileName;
                                }
                                if (aticleSharedoc!=null && aticleSharedoc.Length > 0)
                                {
                                    articleFileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(aticleSharedoc.FileName);
                                    articeDocUrl = articleFolderPath + "/" + articleFileName;
                                }

                                if (model.Id == 0)//insert record 
                                    {
                                        baseModel= await Save(model, userData, articeDocUrl);
                                    }
                                    else//update record
                                    {
                                        baseModel= await Update(model);
                                    }
                                    if (baseModel.Status == true)
                                    {
                                        if (file.Length > 0)
                                        {
                                            await _awsS3Bucket.UploadFileAsync(folderPath, file, fileName);
                                        }
                                        if (aticleSharedoc != null && aticleSharedoc.Length > 0)
                                        {
                                            await _awsS3Bucket.UploadFileAsync(articleFolderPath, aticleSharedoc, articleFileName);
                                        }
                                    }
                                 return baseModel;


                }
                    ///record insert without video file
                    else
                    {
                        if (model.Id == 0)//insert record 
                        {
                            return await Save(model, userData, articeDocUrl);
                        }
                        else//update record
                        {
                            model.DescriptionVideoUrl = "";
                            return await Update(model);
                        }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
                await _logger.AddAsync(new DataModels.Logger.LoggerDataModel {
                    AddedDate=DateTime.UtcNow,
                    Detail= trace.GetFrame(0).GetMethod().ReflectedType.FullName,
                    Message=ex.Message,
                    ErrorCode="Service",
                    Source= "CommunityGroupsService"
                });
                return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };
            }
        }
        public async Task<BaseModel> PublishCommunityGroupAsync(long Id,bool IsPublish)
        {
            try
            {
                CommunityGroupsDataModel communityGroupModel = await _unitOfWork.UserCommunityGroupsRepository.GetByID(Id);
                communityGroupModel.IsPublish = IsPublish;

                await _unitOfWork.UserCommunityGroupsRepository.Update(communityGroupModel);
                return new BaseModel { Status = true, Id = communityGroupModel.Id, Messsage = UMessagesInfo.RecordSaved };
            }
            catch (Exception)
            {

                return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };
            }
        }
        public async Task<BaseModel> DeleteCommunityGroupAsync(long Id)
        {
            try
            {
                CommunityGroupsDataModel communityGroupModel = await _unitOfWork.UserCommunityGroupsRepository.GetByID(Id);
                communityGroupModel.IsActive = false;

                await _unitOfWork.UserCommunityGroupsRepository.Update(communityGroupModel);
                return new BaseModel { Status = true, Id = communityGroupModel.Id, Messsage = UMessagesInfo.RecordDeleted };
            }
            catch (Exception)
            {

                return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };
            }
        }
        public async Task<EditCommunityGroupViewModel> getCommunityGroupDetail(long Id,long UserId)
        {
            EditCommunityGroupViewModel communityGroupView = new EditCommunityGroupViewModel();
               var _data = await _unitOfWork.UserCommunityGroupsRepository.FindAllBy(c => c.User.Id == UserId && c.Id == Id);
            if (_data.Count != 0)
            {
                var _result = _data.FirstOrDefault();
                communityGroupView.Id = _result.Id;
                communityGroupView.OgranizationsId = _result.CommunityId;
                communityGroupView.Title = _result.Title;
                communityGroupView.Description = _result.Description;
                communityGroupView.DescriptionVideoUrl = _result.DescriptionVideoUrl;
                communityGroupView.IsPublicGroup = _result.IsPublicGroup;
                    var _themes = await _unitOfWork.UserCommunityGroupThemeRepository.FindAllBy(c => c.CommunityGroups.Id == _result.Id && c.IsActive==true);
                    if (_themes.Count != 0) {
                        foreach (var item in _themes)
                        {
                            communityGroupView.ThemesId += item.CommunityTargetedGroupId.ToString() + ",";
                        }
                    }
             
                var _communityTargeted = await _unitOfWork.UserCommunityTargetedGroupsRepository.FindAllBy(c => c.CommunityGroups.Id == _result.Id && c.IsActive == true);
                if (_communityTargeted.Count != 0)
                {
                    foreach (var item in _communityTargeted)
                    {
                        communityGroupView.CommunityTargetedGroupId += item.CommunityTargetedGroupId.ToString() + ",";
                    }
                }
                //TargetedAssociations
                var _association = await _unitOfWork.UserCommunityTargetedAssociationsRepository.FindAllBy(c => c.CommunityGroups.Id == _result.Id && c.IsActive == true);
                if (_association.Count != 0)
                {
                    foreach (var item in _association)
                    {
                        communityGroupView.AssociationId += item.AssociationId.ToString();// + ",";
                    }
                }
                //_type1
                var _type1 = await _unitOfWork.UserCommunityTargetedType1Repository.FindAllBy(c => c.CommunityGroups.Id == _result.Id && c.IsActive == true);
                if (_type1.Count != 0)
                {
                    foreach (var item in _type1)
                    {
                        communityGroupView.Type1Id += item.TypeId.ToString();// + ",";
                    }
                }
                //_type2
                var _type2 = await _unitOfWork.UserCommunityTargetedType2Repository.FindAllBy(c => c.CommunityGroups.Id == _result.Id && c.IsActive == true);
                if (_type2.Count != 0)
                {
                    foreach (var item in _type2)
                    {
                        communityGroupView.Type2Id += item.TypeId.ToString();// + ",";
                    }
                }
                //Interests
                var _interests = await _unitOfWork.UserCommunityTargetedInterestRepository.FindAllBy(c => c.CommunityGroups.Id == _result.Id && c.IsActive == true);
                if (_interests.Count != 0)
                {
                    foreach (var item in _interests)
                    {
                        communityGroupView.TargetedInterestIds += item.InterestId.ToString() + ",";
                    }
                }
                OpenOfficeHoursViewModel openOfficeHoursView = new OpenOfficeHoursViewModel();
                var openOfficeHours = await _unitOfWork.UserCommunityOpenOfficeHoursRepository.FindAllBy(c => c.CommunityGroups.Id == _result.Id);
                if (openOfficeHours.Count != 0)
                {
                    var _openHoursModel = openOfficeHours.FirstOrDefault();
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
                communityGroupView.OpenOfficeHours = openOfficeHoursView;
             }
            return communityGroupView;
        }
        
        #region private
        private async Task<BaseModel> Save(CommunityGroupViewModel model, UsersLoginDataModel user,string articeDoclUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(model.TargetedInterestIds) && model.IsPublicGroup==false)
                {
                    return new BaseModel { Status = false, Messsage = "Please select Interest" };
                }
                CommunityGroupsDataModel communityGroupModel = new CommunityGroupsDataModel
                {
                    CreatedBy = model.UserId,
                    User=user,
                    Title = model.Title,
                    CommunityGroupURL = Urlhelper.GenerateSeoFriendlyURL(model.Title),
                    Description = model.Description,
                    DescriptionVideoUrl = model.DescriptionVideoUrl,
                    IsActive=true,
                    IsPublish=false,
                    IsPublicGroup=model.IsPublicGroup,
                    CommunityId= model.OgranizationsId
                };
                await _unitOfWork.UserCommunityGroupsRepository.Insert(communityGroupModel);
                //if group is not public then data will save in relation tables
                if (!model.IsPublicGroup)
                { 
                    //saveing data in relation tables
                    await SaveCommunityTargetedGroupsAsync(model.CommunityTargetedGroupId, user, communityGroupModel);
                    await SaveCommunityTargetedAssociationAsync(model.AssociationId, user, communityGroupModel);
                    //await SaveCommunityTargetedType1Async(model.Type1Id, user, communityGroupModel);
                    //await SaveCommunityTargetedType2Async(model.Type2Id, user, communityGroupModel);
                    await SaveCommunityTargetedInterestsAsync(model.TargetedInterestIds, user, communityGroupModel);
                }
                await SaveCommunityGroupsThemeAsync(model.ThemesId, user, communityGroupModel);
                //SaveOpenHours
                OpenOfficeHoursViewModel OpenOfficeHoursModel = JsonConvert.DeserializeObject<OpenOfficeHoursViewModel>(model.OpenOfficeHours);
                OpenOfficeHoursModel.OFromDate = SphixHelper.setDateFromDayName(OpenOfficeHoursModel.OTimeDayName, DateTime.Now.Date);
                OpenOfficeHoursModel.OToDate = OpenOfficeHoursModel.OFromDate;
                OpenOfficeHoursModel.IsFirstMeeting = true;
                await _openOfficeHoursService.SaveOpenHoursAsync(OpenOfficeHoursModel, user, communityGroupModel);
                //SaveLiveEvent
                LiveEventViewModel LiveEventModel = JsonConvert.DeserializeObject<LiveEventViewModel>(model.LiveEvent);
                await SaveLiveEvent(LiveEventModel, user, communityGroupModel);
                //SaveArticles
                ArticleViewModel ArticleModel = JsonConvert.DeserializeObject<ArticleViewModel>(model.Article);
                ArticleModel.ShareDocument = articeDoclUrl;
                await SaveArticles(ArticleModel, user, communityGroupModel);

                return new BaseModel { Status = true,Id= communityGroupModel.Id, Messsage = UMessagesInfo.RecordSaved };
            }
            catch (Exception ex)
            {
                return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };
            }
        }
        private async Task<BaseModel> Update(CommunityGroupViewModel model)
        {
            CommunityGroupsDataModel communityGroupModel = await _unitOfWork.UserCommunityGroupsRepository.GetByID(model.Id);
            try
            {
                communityGroupModel.Title = model.Title;
                communityGroupModel.Description = model.Description;
                if (!string.IsNullOrEmpty(model.DescriptionVideoUrl)) {
                     await _awsS3Bucket.DeleteFileAsync(communityGroupModel.DescriptionVideoUrl);
                    communityGroupModel.DescriptionVideoUrl = model.DescriptionVideoUrl;
                }
                communityGroupModel.CommunityGroupURL= Urlhelper.GenerateSeoFriendlyURL(model.Title);
                //communityGroupModel.IsActive = true;
                //communityGroupModel.IsPublish = true;
                communityGroupModel.IsPublicGroup = model.IsPublicGroup;
                communityGroupModel.AddedDate = DateTime.UtcNow;
                communityGroupModel.CommunityId = model.OgranizationsId;
                await _unitOfWork.UserCommunityGroupsRepository.Update(communityGroupModel);
                //if group is not public then data will save in relation tables
                if (!model.IsPublicGroup)
                {
                    //saving data in relation tables
                    await SaveCommunityTargetedGroupsAsync(model.CommunityTargetedGroupId, communityGroupModel.User, communityGroupModel);
                    await SaveCommunityTargetedAssociationAsync(model.AssociationId, communityGroupModel.User, communityGroupModel);
                    //await SaveCommunityTargetedType1Async(model.Type1Id, communityGroupModel.User, communityGroupModel);
                    //await SaveCommunityTargetedType2Async(model.Type2Id, communityGroupModel.User, communityGroupModel);
                    await SaveCommunityTargetedInterestsAsync(model.TargetedInterestIds, communityGroupModel.User, communityGroupModel);
                }
                await SaveCommunityGroupsThemeAsync(model.ThemesId, communityGroupModel.User, communityGroupModel);

                //SaveOpenHours
                OpenOfficeHoursViewModel OpenOfficeHoursModel = JsonConvert.DeserializeObject<OpenOfficeHoursViewModel>(model.OpenOfficeHours);
                //OpenOfficeHoursModel.OFromDate = SphixHelper.setDateFromDayName(OpenOfficeHoursModel.OTimeDayName, DateTime.Now.Date);
                //OpenOfficeHoursModel.OToDate = OpenOfficeHoursModel.OFromDate;
                await _openOfficeHoursService.SaveOpenHoursAsync(OpenOfficeHoursModel, communityGroupModel.User, communityGroupModel);

                return new BaseModel { Status = true, Id = communityGroupModel.Id, Messsage = UMessagesInfo.RecordSaved };
            }
            catch (Exception ex)
            {
                return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };
            }
        }

     
        private async Task<BaseModel> SaveLiveEvent(LiveEventViewModel model, UsersLoginDataModel user, CommunityGroupsDataModel communityGroupsData)
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
                WhoCanAttend = model.WhoCanAttend,
                Participants=model.Participants,
                Picture=model.Picture,
                IsActive = true,
            };
            await _unitOfWork.UserCommunityLiveEventsRepository.Insert(dataModel);
            return new BaseModel { Status = true, Id = dataModel.Id, Messsage = UMessagesInfo.RecordSaved };
        }
        private async Task<BaseModel> SaveArticles(ArticleViewModel model, UsersLoginDataModel user, CommunityGroupsDataModel communityGroupsData)
        {
            
            CommunityArticles dataModel = new CommunityArticles
            {
                CreatedBy = model.UserId,
                User = user,
                CommunityGroups = communityGroupsData,
                Title = model.ArticleTitle,
                Description = model.ArticleDescription,
                ShareDocUrl = model.ShareDocument,
                IsActive = true,
            };
            await _unitOfWork.UserCommunityArticlesRepository.Insert(dataModel);
            return new BaseModel { Status = true, Id = dataModel.Id, Messsage = UMessagesInfo.RecordSaved };
        }
        private async Task<bool> SaveCommunityTargetedGroupsAsync(string communitiesIds, UsersLoginDataModel user, CommunityGroupsDataModel communityGroupsData)
        {
            string[] communities = communitiesIds.Split(',');
            var targetGroups = await _unitOfWork.UserCommunityTargetedGroupsRepository.FindAllBy(c => c.CommunityGroups == communityGroupsData);
            foreach (var tagrgetGroup in targetGroups)
            {
                if (communities.Contains(tagrgetGroup.CommunityTargetedGroupId.ToString())==false)
                {
                    tagrgetGroup.IsActive = false;
                }
               else
                {
                    tagrgetGroup.IsActive = true;
                    communities = communities.Where(val => val != tagrgetGroup.CommunityTargetedGroupId.ToString()).ToArray();
                }
                await _unitOfWork.UserCommunityTargetedGroupsRepository.Update(tagrgetGroup);

            }

            foreach (var item in communities)
            {
                if (!string.IsNullOrEmpty(item))
                {
                   
                    UserCommunityTargetedGroupsDataModel userCommunities = new UserCommunityTargetedGroupsDataModel();
                    userCommunities.User = user;
                    userCommunities.CommunityGroups = communityGroupsData;
                    userCommunities.CommunityTargetedGroupId = Convert.ToInt32(item);
                    userCommunities.IsActive = true;
                    await _unitOfWork.UserCommunityTargetedGroupsRepository.Insert(userCommunities);
                }
                //Console.WriteLine(item);
            }
            communities = null; 
            return true;
        }
        private async Task<bool> SaveCommunityTargetedAssociationAsync(string associationIds, UsersLoginDataModel user, CommunityGroupsDataModel communityGroupsData)
        {
            string[] associations = associationIds.Split(',');
            var targetData = await _unitOfWork.UserCommunityTargetedAssociationsRepository.FindAllBy(c => c.CommunityGroups == communityGroupsData);
            foreach (var updateItem in targetData)
            {
                if (associations.Contains(updateItem.AssociationId.ToString()) == false)
                {
                    updateItem.IsActive = false;
                }
                else
                {
                    updateItem.IsActive = true;
                    associations = associations.Where(val => val != updateItem.AssociationId.ToString()).ToArray();
                }
                await _unitOfWork.UserCommunityTargetedAssociationsRepository.Update(updateItem);
            }

            foreach (var item in associations)
            {
                if (!string.IsNullOrEmpty(item))
                {

                    UserCommunityTargetedAssociationsDataModel userAssociation = new UserCommunityTargetedAssociationsDataModel();
                    userAssociation.User = user;
                    userAssociation.CommunityGroups = communityGroupsData;
                    userAssociation.AssociationId = Convert.ToInt32(item);
                    userAssociation.IsActive = true;
                    await _unitOfWork.UserCommunityTargetedAssociationsRepository.Insert(userAssociation);
                }
                //Console.WriteLine(item);
            }
            associations = null; 
            return true;
        }
        private async Task<bool> SaveCommunityTargetedType1Async(string type1Ids, UsersLoginDataModel user, CommunityGroupsDataModel communityGroupsData)
        {
            string[] type1s = type1Ids.Split(',');
            var targetData = await _unitOfWork.UserCommunityTargetedType1Repository.FindAllBy(c => c.CommunityGroups == communityGroupsData);
            foreach (var updateItem in targetData)
            {
                if (type1s.Contains(updateItem.TypeId.ToString()) == false)
                {
                    updateItem.IsActive = false;
                }
                else
                {
                    updateItem.IsActive = true;
                    type1s = type1s.Where(val => val != updateItem.TypeId.ToString()).ToArray();
                }
                await _unitOfWork.UserCommunityTargetedType1Repository.Update(updateItem);
            }

            foreach (var item in type1s)
            {
                if (!string.IsNullOrEmpty(item))
                {

                    UserCommunityTargetedType1DataModel type1 = new UserCommunityTargetedType1DataModel();
                    type1.User = user;
                    type1.CommunityGroups = communityGroupsData;
                    type1.TypeId = Convert.ToInt32(item);
                    type1.IsActive = true;
                    await _unitOfWork.UserCommunityTargetedType1Repository.Insert(type1);
                }
                //Console.WriteLine(item);
            }
            type1s = null;
            return true;
        }
        private async Task<bool> SaveCommunityTargetedType2Async(string type2Ids, UsersLoginDataModel user, CommunityGroupsDataModel communityGroupsData)
        {
            string[] type2s = type2Ids.Split(',');
            var targetData = await _unitOfWork.UserCommunityTargetedType2Repository.FindAllBy(c => c.CommunityGroups == communityGroupsData);
            foreach (var updateItem in targetData)
            {
                if (type2s.Contains(updateItem.TypeId.ToString()) == false)
                {
                    updateItem.IsActive = false;
                }
                else
                {
                    updateItem.IsActive = true;
                    type2s = type2s.Where(val => val != updateItem.TypeId.ToString()).ToArray();
                }
                await _unitOfWork.UserCommunityTargetedType2Repository.Update(updateItem);
            }

            foreach (var item in type2s)
            {
                if (!string.IsNullOrEmpty(item))
                {

                    UserCommunityTargetedType2DataModel type2 = new UserCommunityTargetedType2DataModel();
                    type2.User = user;
                    type2.CommunityGroups = communityGroupsData;
                    type2.TypeId = Convert.ToInt32(item);
                    type2.IsActive = true;
                    await _unitOfWork.UserCommunityTargetedType2Repository.Insert(type2);
                }
                //Console.WriteLine(item);
            }
            type2s = null;
            return true;
        }
        private async Task<bool> SaveCommunityTargetedInterestsAsync(string InterestsIds, UsersLoginDataModel user, CommunityGroupsDataModel communityGroupsData)
        {
            string[] interests = InterestsIds.Split(',');
            var targetData = await _unitOfWork.UserCommunityTargetedInterestRepository.FindAllBy(c => c.CommunityGroups == communityGroupsData);
            foreach (var updateItem in targetData)
            {
                if (interests.Contains(updateItem.InterestId.ToString()) == false)
                {
                    updateItem.IsActive = false;
                }
                else
                {
                    updateItem.IsActive = true;
                    interests = interests.Where(val => val != updateItem.InterestId.ToString()).ToArray();
                }
                await _unitOfWork.UserCommunityTargetedInterestRepository.Update(updateItem);
            }

            foreach (var item in interests)
            {
                if (!string.IsNullOrEmpty(item))
                {

                    UserCommunityTargetedInterestsDataModel interest = new UserCommunityTargetedInterestsDataModel();
                    interest.User = user;
                    interest.CommunityGroups = communityGroupsData;
                    interest.InterestId = Convert.ToInt32(item);
                    interest.IsActive = true;
                    await _unitOfWork.UserCommunityTargetedInterestRepository.Insert(interest);
                }
                //Console.WriteLine(item);
            }
            interests = null;
            return true;
        }
        private async Task<bool> SaveCommunityGroupsThemeAsync(string themesId, UsersLoginDataModel user, CommunityGroupsDataModel communityGroupsData)
        {
            string[] themes = themesId.Split(',');
            var communityGroupTheme = await _unitOfWork.UserCommunityGroupThemeRepository.FindAllBy(c => c.CommunityGroups == communityGroupsData);
            foreach (var themeItem in communityGroupTheme)
            {
                if (themes.Contains(themeItem.CommunityTargetedGroupId.ToString()) == false)
                {
                    themeItem.IsActive = false;
                }
                else
                {
                    themeItem.IsActive = true;
                    themes = themes.Where(val => val != themeItem.CommunityTargetedGroupId.ToString()).ToArray();
                }
                await _unitOfWork.UserCommunityGroupThemeRepository.Update(themeItem);
            }
            foreach (var item in themes)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    UserCommunityGroupThemeDataModel userCommunities = new UserCommunityGroupThemeDataModel();
                    userCommunities.User = user;
                    userCommunities.IsActive = true;
                    userCommunities.CommunityGroups = communityGroupsData;
                    userCommunities.CommunityTargetedGroupId = Convert.ToInt32(item);
                    await _unitOfWork.UserCommunityGroupThemeRepository.Insert(userCommunities);
                }
                //Console.WriteLine(item);
            }
            themes = null; 
            return true;
        }
        #endregion
    }
}
