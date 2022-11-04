using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace RemoteServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("Test")]
        public bool Test([FromBody] string email)
        {
            //return email?.Equals("TestInput") == true;

            //var smtpClient = new SmtpClient("m04.internetmailserver.net")
            //{
            //    Port = 25,
            //    Credentials = new NetworkCredential("username", "password"),
            //    EnableSsl = true,
            //};

            //var mailMessage = new MailMessage
            //{
            //    From = new MailAddress("email"),
            //    Subject = "subject",
            //    Body = "<h1>Hello</h1>",
            //    IsBodyHtml = true,
            //};
            //mailMessage.To.Add("recipient");

            //smtpClient.Send(mailMessage);

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("postmaster@remoteservicesllc.com");
            mail.To.Add("rclarkdev1980@gmail.com");

            mail.Subject = "Test email sent from System.Net.Mail";
            mail.Body = "Mail test";
            mail.Headers.Add("Message-Id",
                              String.Format("<{0}@{1}>",
                              Guid.NewGuid().ToString(),
                              "remoteservicesllc.com"));

            SmtpClient smtp = new SmtpClient("m04.internetmailserver.net");

            NetworkCredential Credentials = new NetworkCredential("postmaster@remoteservicesllc.com", "True2Myself!");
            smtp.Credentials = Credentials;
            smtp.EnableSsl = true;
            smtp.Send(mail);
            //lblMessage.Text = "Mail Sent";

            return true;
        }
    }
}