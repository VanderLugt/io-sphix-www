using Data.Context;
using Sphix.DataModels.UserCommunitiesGroups;
using Sphix.UnitOfWorks;
using Sphix.ViewModels.UserCommunities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sphix.ViewModels.CommunityGroupsFroentEnd;
using Sphix.ViewModels;
using Sphix.Utility;
using System;

namespace Sphix.Service.UserCommunities.ArticleComments
{
   public class ArticleCommentsService: IArticleCommentsService
    {
        private UnitOfWork _unitOfWork;
        private readonly EFDbContext _context;
        public ArticleCommentsService(EFDbContext context)
        {
            _unitOfWork = new UnitOfWork(context);
            _context = context;
        }
        public async Task<ArticleCommentsList> AddCommentAsync(ArticleCommentViewMoldel model)
        {
            try
            {
                UserArticleCommentsDataModel userArticleCommentsData = new UserArticleCommentsDataModel
                {
                    CommentText = model.CommentText,
                    CommentedBy = await _unitOfWork.UserLoginRepository.GetByID(model.UserId),
                    Article = await _unitOfWork.UserCommunityArticlesRepository.GetByID(model.ArticleId),
                    ParentId = model.ParentId,
                    IsActive = true
                };
                await _unitOfWork.ArticleCommentsRepository.Insert(userArticleCommentsData);

                var _userProfile = await _unitOfWork.UserProfileRepository.FindAllBy(c => c.User.Id == model.UserId);
                ArticleCommentsList articleComments = new ArticleCommentsList
                {
                    Id = userArticleCommentsData.Id
                    ,
                    CommentedDate = userArticleCommentsData.CommentedDate.ToString()
                    ,
                    CommentText = model.CommentText
                    ,
                    CommentedById = model.UserId
                    ,
                    ParentId = userArticleCommentsData.ParentId
                    ,
                    UserName = _userProfile[0].FirstName,
                    LoggedInUserId=model.UserId
                };

                return articleComments;

            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<BaseModel> EditCommentAsync(ArticleCommentViewMoldel model)
        {
            try
            {
                if(model.Id!=0)
                {
                   var _result  = await _unitOfWork.ArticleCommentsRepository.FindAllBy(c=>c.Id== model.Id && c.CommentedBy.Id==model.UserId);
                    UserArticleCommentsDataModel userArticleCommentsModel = _result[0];
                    if (userArticleCommentsModel.Id != 0)
                    {
                        userArticleCommentsModel.CommentText = model.CommentText;
                        userArticleCommentsModel.CommentedBy = await _unitOfWork.UserLoginRepository.GetByID(model.UserId);
                        await _unitOfWork.ArticleCommentsRepository.Update(userArticleCommentsModel);
                        return new BaseModel { Status = true, Messsage = UMessagesInfo.RecordSaved };
                    }
                }
                return new BaseModel { Status = false,Messsage=UMessagesInfo.Error };
               
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<IList<ArticleCommentsList>> GetArticleCommentsAsync(long ArticleId,long UserId)
        {
            IList<ArticleCommentsList> list = new List<ArticleCommentsList>();
            await _context.LoadStoredProc("GetArticleComments")
                       .WithSqlParam("ArticleId", ArticleId)
                       .WithSqlParam("UserId", UserId)
                       .ExecuteStoredProcAsync((handler) =>
                       {
                           list = handler.ReadToList<ArticleCommentsList>();
                           // do something with your results.
                       });
            
            return list;
        }
        public async Task<BaseModel> DeleteCommentAsync(ArticleCommentViewMoldel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    var _result = await _unitOfWork.ArticleCommentsRepository.FindAllBy(c => c.Id == model.Id && c.CommentedBy.Id == model.UserId);
                    UserArticleCommentsDataModel userArticleCommentsModel = _result[0];
                    if (userArticleCommentsModel.Id != 0)
                    {
                        userArticleCommentsModel.IsDeletedMessage = true;
                        userArticleCommentsModel.DeletedBy = model.UserId;
                        userArticleCommentsModel.MessageDeletedDate = DateTime.Now;
                        await _unitOfWork.ArticleCommentsRepository.Update(userArticleCommentsModel);
                        return new BaseModel { Status = true, Messsage = UMessagesInfo.RecordDeleted };
                    }
                }
                return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };

            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
