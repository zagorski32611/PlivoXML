using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace PlivoMVC.Controllers
{
    public class VoicemailController : Controller
    {
        public IActionResult Index()
        {
            Plivo.XML.Response resp = new Plivo.XML.Response();
            resp.AddSpeak("Please leave a message. Press the star key when you're done",
                new Dictionary<string, string>() { });
            resp.AddRecord(new Dictionary<string, string>() {
                {"action", "https://<yourdomain>.com/get_recording/"},
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
    }
}