using Sphix.ViewModels;
using Sphix.ViewModels.CommunityGroupsFroentEnd;
using Sphix.ViewModels.UserCommunities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Sphix.Service.UserCommunities.EventComments
{
   public interface IEventCommentsService
    {
        Task<EventCommentsList> AddCommentAsync(EventCommentViewMoldel model);
        Task<BaseModel> EditCommentAsync(EventCommentViewMoldel model);
        Task<BaseModel> DeleteCommentAsync(EventCommentViewMoldel model);
        Task<IList<EventCommentsList>> GetEventCommentsAsync(long ArticleId, long UserId);
    }
}
