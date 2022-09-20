using System;
using System.Collections.Generic;
using System.Diagnostics;
using Plivo.XML;
using Plivo;
using Microsoft.AspNetCore.Mvc;

namespace PlivoMVC.Controllers
{
    public class MessageController : Controller
    {

        public void SendSMS(string messageText, string destination_number)
        {
            var api = new PlivoApi();
            var response = api.Message.Create(
                src: "",
                dst: destination_number is null ? $"{destination_number}" : "+12163759300",
                text: $"{messageText}",
                url: "https://d5ee-2603-6010-8f02-c34-b194-c50e-62ab-2f66.ngrok.io/message/sendcallback");
            Console.WriteLine(response.MessageUuid);
        }

        [HttpPost]
        public IActionResult ReceiveText()
        {
            string fromNum = "";
            string toNum = "";
            string Msg = "";
            IHeaderDictionary heads = this.Request.Headers;
            var url = this.Request.PathBase + this.Request.Path;

            if(this.Request.Form is null)
                return StatusCode(418); // I'm a tea pot!
        
            var formData = this.Request.Form;
            
            foreach (var formEntry in formData)
            {
                Debug.WriteLine($"{formEntry.Key} : {formEntry.Value}");
                
                if (formEntry.Key == "Text")
                    Msg = formEntry.Value;
                else if (formEntry.Key == "From")
                    fromNum = formEntry.Value;
                else if (formEntry.Key == "To")
                    toNum = formEntry.Value;
            }
            var responseMessage = MessageReader(Msg);
            return SendCallBack($"{fromNum}", $"{toNum}", $"{responseMessage}");
        }

        public IActionResult SendCallBack(string destination_number, string source_number, string? messageText = "Hello, thank you for signing up!")
        {
            Dictionary<string, string> responseInfo = new Dictionary<string, string>()
            {
                {"src", source_number is null ? "" : $"{source_number}"},
                {"dst", destination_number is null ? "" : $"{destination_number}" } ,
                {"type", "sms"},
                {"callbackUrl", "https://edf3-2603-6010-8f02-c34-e518-77d-858-fdb6.ngrok.io/Message/sms_status/"},
                {"callbackMethod", "POST"}
            };

            Plivo.XML.Response resp = new Plivo.XML.Response();

            resp.AddMessage(messageText, responseInfo);

            return this.Content(resp.ToString(), "text/xml");
        }

        [HttpPost]
        public IActionResult SMS_Status()
        {
            Debug.WriteLine("*****************************************************");
            foreach (var head in this.Request.Form)
            {
                if (head.Key == "Status")
                {
                    Debug.WriteLine($"\n Key: {head.Key} \n Value: {head.Value}");
                }
                else if (head.Key == "SentTime")
                {
                    Debug.WriteLine($"\n Key: {head.Key} \n Value: {head.Value}");
                }
            }
            return StatusCode(200);
        }


        public string MessageReader(string message, string messageTime = "")
        {
            string lowermessage = message.ToLower();

            if (lowermessage == "yes")
            {
                return messageTime + "success";
            }
            else if(lowermessage == "no")
            {
                return "thank you subscribing to cat facts! :)";
            }
            else if(lowermessage == "yo")
            {
                return "what up?";
            }
            else if(lowermessage == "stop")
            {
                return "you have been removed from the list";
            }
            else
            {
                return "sorry, please subscribe to cat facts!";
            }
        }
    }
}