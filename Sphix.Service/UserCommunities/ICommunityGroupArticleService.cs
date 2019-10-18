using Microsoft.AspNetCore.Http;
using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities
{
   public interface ICommunityGroupArticleService
    {
        Task<IList<CommunityGroupArticlesList>> getUserCommunityGroupArticlesListAsync(EventListSearchFilter model);
        Task<ArticleViewModel> getArticleDetailAsync(long eventId);
        Task<BaseModel> SaveAsync(ArticleViewModel model, IFormFile articleShareDocument);
    }
}
