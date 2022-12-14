using System;
using Plivo.XML;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ivrphonetree.Controllers
{
    public class CallSystemsController : Controller
    {
        // Message that Plivo reads when the caller dials in
        string IvrMessage = "Thank you for calling. Your call is not important to us. Press 1 to contact sales. Press 2 to contact support. Press # when done.";
        // Message that Plivo reads when the caller does nothing
        string NoinputMessage = "Sorry, I didn't catch that. Please hang up and try again";
        // Message that Plivo reads when the caller enters an invalid number
        string WronginputMessage = "Sorry, that's not a valid entry";
        // Sales Phone Number
        string salesPhoneNumber = "+13307655512";
        string supprtPhoneNumber = "+12166004473";
        string ngrokHost = "https://0f04-2603-6010-8f02-c34-c5ae-f989-aa7c-4fc0.ngrok.io";

        // GET: /<controller>/
        public IActionResult Index()
        {
            var resp = new Response();
            Plivo.XML.GetInput get_input = new
                Plivo.XML.GetInput("",
                    new Dictionary<string, string>()
                    {
                        {"action", $"{ngrokHost}/CallSystems/firstbranch/"},
                        {"method", "POST"},
                        {"finishOnKey", "#"},
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
        [HttpPost]
        public IActionResult FirstBranch()
        {
            var requestDictionary = this.Request.Form;
            var incomingDigit = requestDictionary.Where(x => x.Key.Contains("Digit")).Select(x => x.Value);

            var resp = new Response();

            if (incomingDigit.FirstOrDefault() == "1")
            {

                Plivo.XML.Dial dial = new Plivo.XML.Dial(new
                Dictionary<string, string>());

                dial.AddNumber(salesPhoneNumber,
                new Dictionary<string, string>() { });
                resp.Add(dial);
            }
            else if (incomingDigit.FirstOrDefault() == "2")
            {
                Plivo.XML.Dial dial = new Plivo.XML.Dial(new
                Dictionary<string, string>());
                dial.AddNumber(supprtPhoneNumber,
                new Dictionary<string, string>() { });
                resp.Add(dial);
            }
            else
            {
                resp.AddSpeak(WronginputMessage, new Dictionary<string, string>() { });
            }

            Debug.WriteLine(resp.ToString());
            var output = resp.ToString();
            return this.Content(output, "text/xml");
        }

        public IActionResult Support()
        {
            Plivo.XML.Response resp = new Plivo.XML.Response();
            resp.AddSpeak("The support team is buried in tickets. Please leave a voicemail or open a ticket.", new Dictionary<string, string>() { });
            resp.AddRecord(new Dictionary<string, string>() {
                {"action", $"{ngrokHost}/callsystems/receiveTranscription/"},
                {"finishOnKey", ""},
                {"maxLength", "20"},
                {"playBeep", "true"},
                {"timeout", "15"}
            });
            resp.AddSpeak("Recording not received",
                new Dictionary<string, string>() { });

            var output = resp.ToString();
            return this.Content(output, "text/xml");
        }

        public IActionResult Sales()
        {
            Plivo.XML.Response resp = new Plivo.XML.Response();
            resp.AddSpeak("The sales team is busy closing deals, please leave a message.", new Dictionary<string, string>() { });
            resp.AddRecord(new Dictionary<string, string>() {
                {"action", $"{ngrokHost}/callsystems/receiveTranscription/"}, 
                {"finishOnKey", ""},
                {"transcriptionType","auto" },
                {"transcriptionUrl", $"{ngrokHost}/Callsystems/receiveTranscription/" },
                {"maxLength", "20"},
                {"playBeep", "true"},
                {"timeout", "15"}
            });
            resp.AddSpeak("Recording not received",
                new Dictionary<string, string>() { });

            var output = resp.ToString();
            return this.Content(output, "text/xml");
        }

        public IActionResult receiveTranscription()
        {
            Debug.WriteLine($"Plivo Callback from host: {Request.Host.Value}");
            var downloadUrl = "";
            foreach (var thing in Request.Form)
            {
                //Debug.WriteLine(thing.Key + " " + thing.Value);
                if (thing.Key == "RecordUrl")
                {
                    downloadUrl = thing.Value;
                    Debug.WriteLine($"***** Incoming Voicemail: ***** \n {thing.Value}");
                }
                else if(thing.Key == "transcription")
                {
                    Debug.WriteLine($"***** Incoming Transcription: ***** \n \"{thing.Value}\"");
                }
            }
            return StatusCode(200);
        }
    }
}