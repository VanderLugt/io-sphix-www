using AspNetCoreWebApplication.Models;
using CustomTwilioClient;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebApplication.Controllers
{
    public class ConferenceController : Controller
    {
        private readonly ITwilioVideoService _twilioVideoService;
        private ClaimAccessor _claimAccessor;
        public ConferenceController(ITwilioVideoService twilioVideoService)
        {
            _twilioVideoService = twilioVideoService;
        }
        public IActionResult Index()
        {
            //string roomId = _twilioVideoService.CreateVideoRoom("TestRoom1", 2, "https://ww2.sphix.io");
           // ViewBag.TwilioToken= _twilioVideoService.GetTwilioJwtToken("user1");
            return View();
        }
        [HttpGet("GetToken")]
        public IActionResult GetToken()
             => new JsonResult(new { token = _twilioVideoService.GetTwilioJwtToken("TestRoom1") });
    }
}