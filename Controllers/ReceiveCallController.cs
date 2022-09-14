using System;
using Plivo.XML;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PlivoMVC.Models;
using Ivrphonetree.Controllers;

namespace receivecallapp.Controllers
{
    public class ReceivecallController : Controller
    {
        public IActionResult Index()
        {
            Plivo.XML.Response resp = new Plivo.XML.Response();
            resp.AddSpeak("Hello, incoming call received.", new Dictionary<string, string>() {});
            resp.AddPlay("https://s3.amazonaws.com/plivocloud/music.mp3", new Dictionary<string, string>() {});
            var output = resp.ToString();
            Console.WriteLine(output);
            
            return this.Content(output, "text/xml");
        }

        public void IVRRoot()
        {
            // this will accept the call, play the greeting, and handle input. Then it will forward to sales or support.
        }

        public void SendOutMessage(string messageText, string destination_number)
        {
            var messagecontrol = new MessageController();
            messagecontrol.SendSMS($"{messageText}", $"{destination_number}");
        }
    }
}