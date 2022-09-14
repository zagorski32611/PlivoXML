using System;
using System.Collections.Generic;
using System.Diagnostics;
using Plivo.XML;
using Plivo;
using Microsoft.AspNetCore.Mvc;

namespace Ivrphonetree.Controllers
{
    public class IvrController : Controller
    {
        // Message that Plivo reads when the caller dials in
        string IvrMessage = "Hello! Thanks for calling. To speak with Sales, press 1. To speak with Support, press 2.";
        // Message that Plivo reads when the caller does nothing
        string NoinputMessage = "Sorry, I didn't catch that. Please hang up and try again";
        // Message that Plivo reads when the caller enters an invalid number
        string WronginputMessage = "Sorry, that's not a valid entry";
        // Sales Phone Number
        string salesPhoneNumber = "+";

        string ngrokHost = "https://ae20-2603-6010-8f02-c34-998e-cb35-2ca2-e44.ngrok.io";

        // Support Phone number
        string supprtPhoneNumber = "+";

        // GET: /IVR/
        public IActionResult Index()
        {
            var resp = new Response();
            Plivo.XML.GetInput get_input = new
                Plivo.XML.GetInput("",
                    new Dictionary<string, string>()
                    {
                        {"action", $"{ngrokHost}/ivr/firstbranch/"},
                        {"method", "POST"},
                        {"digitEndTimeout", "5"},
                        {"inputType", "dtmf"},
                        {"redirect", "true"},
                    });

            resp.Add(get_input);
            get_input.AddSpeak(IvrMessage,
                new Dictionary<string, string>() { });
            resp.AddSpeak(NoinputMessage,
                new Dictionary<string, string>() { });

            var output = resp.ToString();
            return this.Content(output, "text/xml");
        }

        // First branch of IVR phone tree
        public IActionResult FirstBranch()
        {
            string digit = Request.Query["Digits"];

            //string xdigit = Request.Form["Digits"];

            //Debug.WriteLine($"Digit pressed : {this.Request.Form.R}");

            var resp = new Response();

            if (digit == "1")
            {
                string getinput_action_sales = $"{ngrokHost}/ivr/salesBranch/";

                Plivo.XML.Dial dial = new Plivo.XML.Dial(new Dictionary<string, string>());
                dial.AddNumber(salesPhoneNumber, new Dictionary<string, string>() { });

            }
            else if (digit == "2")
            {
                string get_input_action_support = $"{ngrokHost}/ivr/supportBranch/";
                Plivo.XML.Dial dial = new Plivo.XML.Dial(new Dictionary<string, string>());
                dial.AddNumber(supprtPhoneNumber, new Dictionary<string, string>() { });
                resp.Add(dial);
            }
            else
            {
                // Add Speak XML tag
                resp.AddSpeak(WronginputMessage, new Dictionary<string, string>() { });
            }

            Debug.WriteLine(resp.ToString());

            var output = resp.ToString();
            return this.Content(output, "text/xml");
        }

        public IActionResult supportBranch()
        {
            // Forward to my phone number.
            // If I don't answer, prompt for a voice mail
            // record & transcribe VM
            Plivo.XML.Response resp = new Plivo.XML.Response();

            resp.AddSpeak("Hello! Thank you for calling the Help Desk.", new Dictionary<string, string>() { });
            var output = resp.ToString();
            Console.WriteLine(output);
            return this.Content(output, "text/xml");
        }

        public IActionResult salesBranch()
        {
            Plivo.XML.Response resp = new Plivo.XML.Response();

            resp.AddSpeak("Hello! Thank you for calling the Sales Team", new Dictionary<string, string>() { });
            var output = resp.ToString();
            Console.WriteLine(output);

            return this.Content(output, "text/xml");
        }

    }
}
