using System;
using System.Collections.Generic;
using System.Diagnostics;
using Plivo.XML;
using Plivo;
using Microsoft.AspNetCore.Mvc;

namespace Ivrphonetree.Controllers
{
    public class MessageController : Controller
    {
       
        public void SendSMS(string messageText, string destination_number)
        {
            var api = new PlivoApi();
            var response = api.Message.Create(
                src: "15392034790",
                dst: destination_number is null ? $"{destination_number}" : "+12163759300",
                text: $"{messageText}",
                url: "https://webhook.site/ae526e29-a6e8-420b-9c3c-1237517cd809");
            
            Console.WriteLine(response.MessageUuid);
        }


        public IActionResult ReceiveText()
        {
            string fromNum = "";
            string toNum = "";
            string Msg = "";

            var formData = this.Request.Form;
            
            foreach(var formEntry in formData)
            {
                Debug.WriteLine($"{formEntry.Key} : {formEntry.Value}" );
                if(formEntry.Key == "Text")
                    Msg = formEntry.Value;
                else if(formEntry.Key == "From")
                    fromNum = formEntry.Value;
                else if(formEntry.Key == "To")
                    toNum = formEntry.Value;
            }

            switch (Msg.ToUpper())
            {
                case "YES":
                    return SendCallBack($"{fromNum}", $"{toNum}");
                case "YO":
                    return SendCallBack($"{fromNum}", $"{toNum}", "what up?");
                default:
                    return SendCallBack($"{fromNum}", $"{toNum}", "Sorry, I don't recognize that");
            }

        }

        public IActionResult SendCallBack(string destination_number, string source_number, string? messageText = "Hello, thank you for signing up!")
        {
            Dictionary<string, string> responseInfo = new Dictionary<string, string>()
			{
				{"src", source_number is null ? "13307655512" : $"{source_number}"},
				{"dst", destination_number is null ? "12163759300" : $"{destination_number}" } ,
				{"type", "sms"},
				{"callbackUrl", "https://edf3-2603-6010-8f02-c34-e518-77d-858-fdb6.ngrok.io/Message/sms_status/"},
				{"callbackMethod", "POST"}
			};

            Plivo.XML.Response resp = new Plivo.XML.Response();

			resp.AddMessage(messageText, responseInfo);
            
            return this.Content(resp.ToString(), "text/xml");
        }

        public IActionResult SMS_Status()
        {
            foreach(var head in this.Request.Headers)
            {
                //Debug.WriteLine($"{head.Key} : {head.Value}");
            }
            //Debug.WriteLine("Plivo Callback String:" + this.Request.Headers.ToArray());
            return StatusCode(200);
        }
							
    }
}