using System;
using Plivo.XML;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PlivoMVC.Models;

namespace receivecallapp.Controllers
{
    public class ReceivecallController : Controller
    {
        public IActionResult Index()
        {
            Plivo.XML.Response resp = new Plivo.XML.Response();
            resp.AddSpeak("Hello, you just received your first call",
                new Dictionary<string, string>() {
                {
                    "loop",
                    "3"
                }
                });
            var output = resp.ToString();
            Console.WriteLine(output);

            return this.Content(output, "text/xml");
        }

        public void IVRRoot()
        {
            // this will accept the call, play the greeting, and handle input. Then it will forward to sales or support.
        }
    }
}