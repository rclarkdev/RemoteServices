using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace RemoteServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;
        private readonly IConfiguration _configuration;

        public EmailController(ILogger<EmailController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<EmailSettings> Get()
        {
            return new List<EmailSettings>();
        }

        [HttpPost("send")]
        public bool Send([FromBody] EmailSettings email)
        {
            var username = _configuration["EmailSettings:NetworkCredentials:Username"];
            var password = _configuration["EmailSettings:NetworkCredentials:Password"];
            var smtpClient = _configuration["EmailSettings:SmtpClient"];

            email.EmailDate = DateTime.Now;

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(email.FromEmail);
            mail.To.Add(email.ToEmail);

            mail.Subject = email.Subject;
            mail.Body = email.EmailBody + Environment.NewLine + Environment.NewLine + $"{email.FromName}";
            mail.Headers.Add("Message-Id",
                              String.Format("<{0}@{1}>",
                              Guid.NewGuid().ToString(),
                              "remoteservicesllc.com"));

            SmtpClient smtp = new SmtpClient(smtpClient);
            smtp.Credentials = new NetworkCredential(username, password);
            smtp.EnableSsl = true;
            smtp.Send(mail);

            return true;
        }
    }
}