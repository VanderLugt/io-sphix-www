using AWSS3.Utility;
using Data.Context;
using Microsoft.AspNetCore.Http;
using Sphix.DataModels.User;
using Sphix.DataModels.UserCommunities;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities
{
   public class CommunityGroupArticleService: ICommunityGroupArticleService
    {
        private UnitOfWork _unitOfWork;
        private readonly EFDbContext _context;
        private readonly IAWSS3Bucket _awsS3Bucket;
        public CommunityGroupArticleService(EFDbContext context, IAWSS3Bucket awsS3Bucket)
        {
            _unitOfWork = new UnitOfWork(context);
            _context = context;
            _awsS3Bucket = awsS3Bucket;
        }
        public async Task<BaseModel> SaveAsync(ArticleViewModel model, IFormFile articleShareDocument)
        {
            try
            {
                //save with attcahed doc
                BaseModel baseModel = new BaseModel();
                string articeDocUrl = string.Empty;
                string articleFolderPath = "ArcticlesShareDocuments/" + DateTime.Now.Year.ToString();
                string articleFileName = string.Empty;
                if (articleShareDocument != null && articleShareDocument.Length > 0)
                {
                    articleFileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(articleShareDocument.FileName);
                    articeDocUrl = articleFolderPath + "/" + articleFileName;
                    model.ShareDocument = articeDocUrl;
                }
                if (model.Id == 0)
                {
                    UsersLoginDataModel _user = await _unitOfWork.UserLoginRepository.GetByID(model.UserId);
                    CommunityGroupsDataModel communityGroupsData = await _unitOfWork.UserCommunityGroupsRepository.GetByID(model.CommunityGroupsId);
                    baseModel= await Save(model, _user, communityGroupsData);
                }
                else
                {
                    baseModel = await Update(model);
                }
                if (baseModel.Status == true)
                {
                    if (articleShareDocument != null && articleShareDocument.Length > 0)
                    {
                        await _awsS3Bucket.UploadFileAsync(articleFolderPath, articleShareDocument, articleFileName);
                    }
                }
                return baseModel;
            }
            catch (Exception)
            {

                return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };
            }
        }
        public async Task<ArticleViewModel> getArticleDetailAsync(long Id)
        {
            ArticleViewModel articleViewModel = new ArticleViewModel();
            if (Id == 0)
            {
                return articleViewModel;
            }
            var _result = await _unitOfWork.UserCommunityArticlesRepository.GetByID(Id);

            if (_result != null)
            {
                articleViewModel.ArticleTitle = _result.Title;
                articleViewModel.ArticleDescription = _result.Description;
                articleViewModel.Id = _result.Id;
                articleViewModel.ShareDocument = _result.ShareDocUrl;
            }
            return articleViewModel;
        }
        public async Task<IList<CommunityGroupArticlesList>> getUserCommunityGroupArticlesListAsync(EventListSearchFilter model)
        {
            IList<CommunityGroupArticlesList> list = new List<CommunityGroupArticlesList>();
            await _context.LoadStoredProc("GetUserCommunityGroupArticlesList")
                       .WithSqlParam("UserId", model.Id)
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
        private async Task<BaseModel> Save(ArticleViewModel model, UsersLoginDataModel user, CommunityGroupsDataModel communityGroupsData)
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
        private async Task<BaseModel> Update(ArticleViewModel model)
        {
            CommunityArticles dataModel =await _unitOfWork.UserCommunityArticlesRepository.GetByID(model.Id);

            dataModel.Title = model.ArticleTitle;
            dataModel.Description = model.ArticleDescription;
            if (!string.IsNullOrEmpty(model.ShareDocument))
            {
                await _awsS3Bucket.DeleteFileAsync(dataModel.ShareDocUrl);
                dataModel.ShareDocUrl = model.ShareDocument;
            }
            
            dataModel.IsActive = true;
            dataModel.AddedDate = DateTime.UtcNow;
            await _unitOfWork.UserCommunityArticlesRepository.Update(dataModel);
            return new BaseModel { Status = true, Id = dataModel.Id, Messsage = UMessagesInfo.RecordSaved };
        }
    }
}
