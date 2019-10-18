using Sphix.DataModels.UserCommunitiesGroups;
using Sphix.ViewModels;
using Sphix.ViewModels.CommunityGroupsFroentEnd;
using Sphix.ViewModels.UserCommunities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities.ArticleComments
{
   public interface IArticleCommentsService
    {
        Task<ArticleCommentsList> AddCommentAsync(ArticleCommentViewMoldel model);
        Task<BaseModel> EditCommentAsync(ArticleCommentViewMoldel model);
        Task<BaseModel> DeleteCommentAsync(ArticleCommentViewMoldel model);
        Task<IList<ArticleCommentsList>> GetArticleCommentsAsync(long ArticleId, long UserId);
    }
}
