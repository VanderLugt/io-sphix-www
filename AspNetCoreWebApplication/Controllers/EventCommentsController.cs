using System;
using System.Threading.Tasks;
using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.UserCommunities.EventComments;
using Sphix.ViewModels.UserCommunities;

namespace AspNetCoreWebApplication.Controllers
{
    [CustomAuthorizeAttribute]
    public class EventCommentsController : Controller
    {
        private readonly IEventCommentsService _eventCommentsService;
        private ClaimAccessor _claimAccessor;
        public EventCommentsController(
            ClaimAccessor claimAccessor,
            IEventCommentsService eventCommentsService
         )
        {
            _claimAccessor = claimAccessor;
            _eventCommentsService = eventCommentsService;
        }
        [HttpPost]
        public async Task<IActionResult> addNewComment(EventCommentViewMoldel model)
        {
            try
            {
                model.UserId = _claimAccessor.UserId;
                var result = await _eventCommentsService.AddCommentAsync(model);
                return Json(new { status = true, messsage = "Thanks", result = result });

            }
            catch (Exception)
            {
                //Handle Error here..
            }
            return Json(new { error = true });
        }
        [HttpPost]
        public async Task<IActionResult> editComment(EventCommentViewMoldel model)
        {
            try
            {
                model.UserId = _claimAccessor.UserId;
                var result = await _eventCommentsService.EditCommentAsync(model);
                return Json(new { status = true, messsage = "Thanks" });
            }
            catch (Exception)
            {
                //Handle Error here..
            }
            return Json(new { error = true });
        }
        [HttpPost]
        public async Task<IActionResult> deleteComment(EventCommentViewMoldel model)
        {
            try
            {
                model.UserId = _claimAccessor.UserId;
                var result = await _eventCommentsService.DeleteCommentAsync(model);
                return Json(result);
            }
            catch (Exception)
            {
                //Handle Error here..
            }
            return Json(new { error = true });
        }
        public IActionResult AddEventCommentPartial()
        {
            ViewBag.Id = _claimAccessor.UserId;
            return PartialView("_addComment");
        }
        public async Task<IActionResult> LoadEventCommentsAsync(long eventId)
        {
            return Json(await _eventCommentsService.GetEventCommentsAsync(eventId, _claimAccessor.UserId));
        }
    }
}