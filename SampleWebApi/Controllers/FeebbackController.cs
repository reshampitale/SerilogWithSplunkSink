using Microsoft.AspNetCore.Mvc;
using Splunk.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace splunkWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        // GET: api/<FeebbackController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
            {
                return true;
            };

            var middleware = new HttpEventCollectorResendMiddleware(100);
            var ecSender = new HttpEventCollectorSender(new Uri("https://localhost:8088"),  //http port as set in global setting
                "08559b54-8b15-4adc-8980-cf57d90039b6",  // token
                null,
                HttpEventCollectorSender.SendMode.Sequential,
                0,
                0,
                0,
                middleware.Plugin
            );
            ecSender.OnError += o => Console.WriteLine(o.Message);
            ecSender.Send(Guid.NewGuid().ToString(), "INFO", null, "Morning Feebdack", null, null);
            return new string[] { "value1", "value2" };
        }

        // GET api/<FeebbackController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FeebbackController>
        [HttpPost]
        [Route("Message")]
        public ActionResult Post(Feedback feedback)//([FromBody] string value)
        {
            Feedback db = new Feedback();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //db.Add(feedback);
           // await db.SaveChangesAsync();



            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
            {
                return true;
            };

            var middleware = new HttpEventCollectorResendMiddleware(100);
            var ecSender = new HttpEventCollectorSender(new Uri("https://localhost:8088"),  //http port as set in global setting
                "08559b54-8b15-4adc-8980-cf57d90039b6",  // token
                null,
                HttpEventCollectorSender.SendMode.Sequential,
                0,
                0,
                0,
                middleware.Plugin
            );
            ecSender.OnError += o => Console.WriteLine(o.Message);
            ecSender.Send(Guid.NewGuid().ToString(), "INFO", null, "Morning Feebdack",null,null);
           // await ecSender.FlushAsync();

            return Ok("Thanks for your feedback");

        }

        // PUT api/<FeebbackController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FeebbackController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
