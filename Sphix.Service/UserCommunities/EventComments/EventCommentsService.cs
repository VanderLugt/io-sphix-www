using Data.Context;
using Sphix.DataModels.UserCommunitiesGroups;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.CommunityGroupsFroentEnd;
using Sphix.ViewModels.UserCommunities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities.EventComments
{
   public class EventCommentsService:IEventCommentsService
    {
        private UnitOfWork _unitOfWork;
        private readonly EFDbContext _context;
        public EventCommentsService(EFDbContext context)
        {
            _unitOfWork = new UnitOfWork(context);
            _context = context;
        }
        public async Task<EventCommentsList> AddCommentAsync(EventCommentViewMoldel model)
        {
            try
            {
                if (model.EventId == 0)
                {
                    return null; 
                }
                UserEventCommentsDataModel uModel = new UserEventCommentsDataModel
                {
                    CommentText = model.CommentText,
                    CommentedBy = await _unitOfWork.UserLoginRepository.GetByID(model.UserId),
                    Events = await _unitOfWork.UserCommunityLiveEventsRepository.GetByID(model.EventId),
                    ParentId = model.ParentId,
                    IsActive = true
                };
                await _unitOfWork.EventCommentsRepository.Insert(uModel);

                var _userProfile = await _unitOfWork.UserProfileRepository.FindAllBy(c => c.User.Id == model.UserId);
                EventCommentsList articleComments = new EventCommentsList
                {
                    Id = uModel.Id,
                    CommentedDate = uModel.CommentedDate.ToString(),
                    CommentText = model.CommentText,
                    CommentedById = model.UserId,
                    ParentId = uModel.ParentId,
                    UserName = _userProfile[0].FirstName,
                    LoggedInUserId = model.UserId
                };

                return articleComments;

            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<BaseModel> EditCommentAsync(EventCommentViewMoldel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    var _result = await _unitOfWork.EventCommentsRepository.FindAllBy(c => c.Id == model.Id && c.CommentedBy.Id == model.UserId);
                    UserEventCommentsDataModel uModel = _result[0];
                    if (uModel.Id != 0)
                    {
                        uModel.CommentText = model.CommentText;
                        uModel.CommentedBy = await _unitOfWork.UserLoginRepository.GetByID(model.UserId);
                        await _unitOfWork.EventCommentsRepository.Update(uModel);
                        return new BaseModel { Status = true, Messsage = UMessagesInfo.RecordSaved };
                    }
                }
                return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };

            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<IList<EventCommentsList>> GetEventCommentsAsync(long ArticleId, long UserId)
        {
            IList<EventCommentsList> list = new List<EventCommentsList>();
            await _context.LoadStoredProc("GetEventComments")
                       .WithSqlParam("EventId", ArticleId)
                       .WithSqlParam("UserId", UserId)
                       .ExecuteStoredProcAsync((handler) =>
                       {
                           list = handler.ReadToList<EventCommentsList>();
                           // do something with your results.
                       });

            return list;
        }
        public async Task<BaseModel> DeleteCommentAsync(EventCommentViewMoldel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    var _result = await _unitOfWork.EventCommentsRepository.FindAllBy(c => c.Id == model.Id && c.CommentedBy.Id == model.UserId);
                    UserEventCommentsDataModel uModle = _result[0];
                    if (uModle.Id != 0)
                    {
                        uModle.IsDeletedMessage = true;
                        uModle.DeletedBy = model.UserId;
                        uModle.MessageDeletedDate = DateTime.UtcNow;
                        await _unitOfWork.EventCommentsRepository.Update(uModle);
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
