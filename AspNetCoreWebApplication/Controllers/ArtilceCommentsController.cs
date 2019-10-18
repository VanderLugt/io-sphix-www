using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.UserCommunities.ArticleComments;
using Sphix.ViewModels.UserCommunities;
using System;
using System.Threading.Tasks;

namespace AspNetCoreWebApplication.Controllers
{
    [CustomAuthorizeAttribute]
    public class ArticleCommentsController : Controller
    {
        private readonly IArticleCommentsService _articleCommentsService;
        private ClaimAccessor _claimAccessor;
        public ArticleCommentsController(
            ClaimAccessor claimAccessor,
            IArticleCommentsService ArticleCommentsService
            )
        {
            _claimAccessor = claimAccessor;
            _articleCommentsService = ArticleCommentsService;
        }
        [HttpPost]
        public async Task<IActionResult> addNewComment(ArticleCommentViewMoldel model)
        {
            try
            {
                model.UserId = _claimAccessor.UserId;
                var result =await _articleCommentsService.AddCommentAsync(model);
                return Json(new { status = true, messsage="Thanks", result = result });

            }
            catch (Exception)
            {
                //Handle Error here..
            }

            return Json(new { error = true });
        }
        [HttpPost]
        public async Task<IActionResult> editComment(ArticleCommentViewMoldel model)
        {
            try
            {
                model.UserId = _claimAccessor.UserId;
                var result = await _articleCommentsService.EditCommentAsync(model);
                return Json(new { status = true, messsage = "Thanks" });
            }
            catch (Exception)
            {
                //Handle Error here..
            }

            return Json(new { error = true });
        }
        [HttpPost]
        public async Task<IActionResult> deleteComment(ArticleCommentViewMoldel model)
        {
            try
            {
                model.UserId = _claimAccessor.UserId;
                var result = await _articleCommentsService.DeleteCommentAsync(model);
                return Json(result);
            }
            catch (Exception)
            {
                //Handle Error here..
            }

            return Json(new { error = true });
        }
        public IActionResult AddArticleCommentPartial()
        {
            ViewBag.Id = _claimAccessor.UserId;
            return PartialView("_addComment");
        }
        public async Task<IActionResult> LoadArticleCommentsAsync(long articleId)
        {
            return Json(await _articleCommentsService.GetArticleCommentsAsync(articleId,_claimAccessor.UserId));
        }
    }
}