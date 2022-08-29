using System;
using Plivo.XML;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PlivoMVC.Models;

namespace receivecallapp.Controllers
{
    public class ForwardCallController : Controller
    {
        public IActionResult Index()
        {
            Dictionary<string, string> parms = new();
            parms.Add("action", "url");
            parms.Add("method", "POST");
            parms.Add("inputType", "dtmf");
            parms.Add("digitEndTimeout", "2");
            parms.Add("redirect", "true");

            string supportNumber = "+";
            string salesNumber = "+";
            Plivo.XML.Response resp = new();

            Plivo.XML.Dial dial = new Plivo.XML.Dial(new Dictionary<string, string> { });
            dial.AddNumber($"{supportNumber}", new Dictionary<string, string> { });
            resp.Add(dial);

            var output = resp.ToString();
            Console.WriteLine(output);

            return this.Content(output, "text/xml");

        }

    }
}