using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwilioSharp.MVC3.Controllers;
using TwilioSharp.Request;

namespace TBTracker.Controllers
{
    public class ReminderController : TwiMLController
    {
        [HttpPost]
        public ActionResult TextResponse(TextRequest request)
        {
            var answer = string.Format("{0} {1}", "Your phone number is ", request.From);
            return TwiML(response => response.Sms(answer));
        }

        [HttpPost]
        public ActionResult CallResponse(CallRequest request)
        {
            return TwiML(response => response.Say(request.From).Hangup());
        }
    }
}
