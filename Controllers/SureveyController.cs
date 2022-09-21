using System;
using System.Collections.Generic;
using System.Diagnostics;
using Plivo.XML;
using Plivo;
using Microsoft.AspNetCore.Mvc;

namespace PlivoMVC.Controllers
{
    public class SurveyController : Controller
    {
        string ngrokHost = "";

        public IActionResult Index(string phoneNumber)
        {
            MessageController message = new();
            message.SendSMS("Did you find out all the information you needed? Please reply 'Yes' or 'No'", phoneNumber);

            return this.Content("OK");
        }

        public IActionResult survey_response()
        {
            
            string from_number = Request.Form["From"];
            string to_number = Request.Form["To"];
            string text = Request.Form["Text"];
            string body;
            

            Debug.WriteLine($"Incoming message from: {from_number} To: {to_number} \n \"{text}\"");

            if (text.ToLower() == "yes")
            {
                body = "Thank you for your feedback";
            }
            else if (text.ToLower() == "no")
            {
                body = "We apologize for the inconvenience. A representative will contact you to assist you";
            }
            else
            {
                body = string.Format("Response received was {0}, which is invalid. Please reply with either 'Yes' or 'No'", text);
            }

            Plivo.XML.Response resp = new Plivo.XML.Response();
            resp.AddMessage(body, new Dictionary<string, string>()
            {
                {"src", to_number},
                {"dst", from_number},
                {"type", "sms"}
            });
            var output = resp.ToString();
            Console.WriteLine(output);
            return this.Content(output, "text/xml");
        }
    }
}