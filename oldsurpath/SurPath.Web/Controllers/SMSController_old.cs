using SurPathWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using Twilio.AspNet.Common;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Messaging;

namespace SurPathWeb.Controllers
{
    //public class SMSController1 : ApiController
    //{
    //    // GET api/<controller>
    //    public IEnumerable<string> Get()
    //    {
    //        return new string[] { "value1", "value2" };
    //    }

    //    // GET api/<controller>/5
    //    public string Get(int id)
    //    {
    //        return "value";
    //    }

    //    // POST api/<controller>
    //    public void Post([FromBody]string value)
    //    {
    //    }

    //    // PUT api/<controller>/5
    //    public void Put(int id, [FromBody]string value)
    //    {
    //    }

    //    // DELETE api/<controller>/5
    //    public void Delete(int id)
    //    {
    //    }
    //}


    public class SMSController_old : TwilioController
    {

        [System.Web.Http.HttpGet]
        public TwiMLResult Index()
        {
            MessagingResponse mResponse = new MessagingResponse();
            mResponse.Message("Ok");

            return TwiML(mResponse);
        }

        //[System.Web.Http.HttpPost]
        //public ActionResult Index()
        //{
        //    // get the response
        //    var smsReply = Request.Form["Body"];
        //    // from the phone number, identify the donor, then identify the last client dept to notify them
        //    // pull the client dept's message (or use the default autoresponse)
        //    // send the response
        //    var response = new MessagingResponse();
        //    if (smsReply == "hello")
        //    {
        //        response.Message("Hi!");
        //    }
        //    else if (smsReply == "bye")
        //    {
        //        response.Message("Goodbye");
        //    }

        //    return TwiML(response);
        //}
    }





    //public class TwilioMessagingRequest
    //{
    //    public string Body { get; set; }
    //}

    //public class TwilioVoiceRequest
    //{
    //    public string From { get; set; }
    //}

    //public class IncomingController : ApiController
    //{
    //    //[System.Web.Http.Route("voice")]
    //    //[System.Web.Http.AcceptVerbs("POST")]
    //    //[ValidateTwilioRequest]
    //    //public HttpResponseMessage PostVoice([FromBody] TwilioVoiceRequest voiceRequest)
    //    //{
    //    //    var message =
    //    //        "Thanks for calling! " +
    //    //        $"Your phone number is {voiceRequest.From}. " +
    //    //        "I got your call because of Twilio's webhook. " +
    //    //        "Goodbye!";

    //    //    var response = new VoiceResponse();
    //    //    response.Say(message);
    //    //    response.Hangup();

    //    //    return ToResponseMessage(response.ToString());
    //    //}

    //    //[System.Web.Http.Route("message")]
    //    //[System.Web.Http.AcceptVerbs("POST")]
    //    //[ValidateTwilioRequest]
    //    //public HttpResponseMessage PostMessage([FromBody] TwilioMessagingRequest messagingRequest)
    //    //{


    //    //    //        // get the response
    //    //            var smsReply = messagingRequest.Body;
    //    //    //        // from the phone number, identify the donor, then identify the last client dept to notify them
    //    //    //        // pull the client dept's message (or use the default autoresponse)
    //    //    //        // send the response
    //    //    var message =
    //    //        $"Your text to me was {messagingRequest.Body.Length} characters long. " +
    //    //        "Webhooks are neat :)";

    //    //    var response = new MessagingResponse();
    //    //    response.Append(new Message(message));

    //    //    return ToResponseMessage(response.ToString());
    //    //}

    //    //private static HttpResponseMessage ToResponseMessage(string response)
    //    //{
    //    //    return new HttpResponseMessage
    //    //    {
    //    //        Content = new StringContent(response, Encoding.UTF8, "application/xml")
    //    //    };
    //    //}
    //}

}



