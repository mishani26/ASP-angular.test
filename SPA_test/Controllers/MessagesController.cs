using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SPA_test.XML_Helpers;
using SPA_test.Models;

namespace SPA_test.Controllers
{
    public class MessagesController : Controller
    {
        // GET: Messages
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult SendMessage(string messageVal)
        {
            XML_Helpers.XML_Helpers.SendMessage(messageVal);
            List<MassageViewModel> MessageList = XML_Helpers.XML_Helpers.GetAllMessages();
            return Json(MessageList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllMessages()
        {
            List<MassageViewModel> MessageList = XML_Helpers.XML_Helpers.GetAllMessages();
            return Json(MessageList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCurrentUserMessages()
        {
            List<MassageViewModel> MessageList = XML_Helpers.XML_Helpers.GetCurrentUserMessages();
            return Json(MessageList, JsonRequestBehavior.AllowGet);
        }
    }
}