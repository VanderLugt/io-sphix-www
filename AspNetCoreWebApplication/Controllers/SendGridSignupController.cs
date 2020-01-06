using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using SendGrid;
using Sphix.Service.Communities;
using Sphix.Service.UserCommunities.CommunitiesForGood;
namespace AspNetCoreWebApplication.Controllers
{
    public class SendGridSignupController : Controller
    {
        private ClaimAccessor _claimAccessor;
        private readonly ICommunitiesForGoodService _communitiesForGoodService;
        private readonly ICommunitiesService _communitiesService;
        private readonly string _sendGridBaseAddress = "https://api.sendgrid.com";
        private readonly string _sendGridRequestKey = "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";
        private readonly string _verificationToken = "5aa1117e-4c53-458d-94fd-14f4b0589961";
        //CommunitiesService : ICommunitiesService

        public SendGridSignupController(ClaimAccessor claimAccessor
            , ICommunitiesForGoodService communitiesForGoodService
            , ICommunitiesService communitiesService)
        {
            _communitiesForGoodService = communitiesForGoodService;
            _claimAccessor = claimAccessor;
            _communitiesService = communitiesService;
        }
        public IActionResult Index(string token)
        {
            if ((this._verificationToken ?? "").ToLower() != (token ?? "").ToLower())
            {
                return RedirectToAction("Error");
            }
            SendGridSignupRequestModel model = new SendGridSignupRequestModel();
            model.Communities = Task.Run(() => _communitiesService.GetActiveCommunities()).Result.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = Convert.ToString(x.Value)
            }).ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult Index(SendGridSignupRequestModel model)
        {
            try
            {
                JArray array = new JArray();
                array.Add(model.EmailId);
                JArray contactArray = new JArray();
                JObject contact = new JObject();
                contact.Add("address_line_1", "string (optional)");
                contact.Add("address_line_2", "string (optional)");
                contact.Add("alternate_emails", array);
                contact.Add("city", "string (optional)");
                contact.Add("country", "string (optional)");
                contact.Add("email", model.EmailId);
                contact.Add("first_name", model.FirstName);
                contact.Add("last_name", "string (optional)");
                contact.Add("postal_code", "string (optional)");
                contact.Add("state_province_region", "string (optional)");
                JObject custom_fields = new JObject();
                custom_fields.Add("e3_T", Convert.ToString(model.CommunityId));
                contact.Add("custom_fields", custom_fields);
                contactArray.Add(contact);

                JObject objectToSend = new JObject();
                objectToSend.Add("contacts", contactArray);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(_sendGridBaseAddress + "/v3/marketing/contacts");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + this._sendGridRequestKey);
                httpWebRequest.Method = "PUT";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(objectToSend);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
                model.Communities = Task.Run(() => _communitiesService.GetActiveCommunities()).Result.Select(x => new SelectListItem()
                {
                    Text = x.Text,
                    Value = Convert.ToString(x.Value)
                }).ToList();
                model.IsSuccess = true;
                model.Message = "Form submitted successfully !";
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = "Some error occured during request. Please try again later !";
            }
            return View("Views/SendGridSignup/Index.cshtml", model);
        }
        public IActionResult Error()
        {
            ViewBag.Message = "Invalid token!";
            return View();
        }
        public IActionResult Test()
        {
            return View();
        }
    }
}