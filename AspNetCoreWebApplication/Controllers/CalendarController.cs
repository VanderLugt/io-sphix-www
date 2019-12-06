using Microsoft.AspNetCore.Mvc;
using Sphix.DataModels.Event;
using System;
using System.Collections.Generic;

namespace AspNetCoreWebApplication.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetEvents()
        {
            List<EventList> evnts = new List<EventList>();
            for (int i = 1; i < 16; i++)
            {
                if (i % 2 == 0)
                {
                    evnts.Add(new EventList
                    {
                        Description = "Event E0" + i.ToString(),
                        End = DateTime.Now.AddDays(i),
                        IsFullDay = true,
                        Id = i,
                        Start = DateTime.Now.AddDays(i),
                        Subject = "Event detailss" + i.ToString(),
                        ThemeColor = "green"
                    });
                    evnts.Add(new EventList
                    {
                        Description = "Open Office hours H0" + i.ToString(),
                        End = DateTime.Now.AddDays(i),
                        IsFullDay = true,
                        Id = i,
                        Start = DateTime.Now.AddDays(i),
                        Subject = "Open Office hours" + i.ToString(),
                        ThemeColor = "#000"
                    });
                }
            }
           
            //  var events = dc.Events.ToList();
            return Json(evnts);
        }
    }
}