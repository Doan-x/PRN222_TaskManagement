using PRN222_TaskManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace PRN222_TaskManagement.Mail
{
    public class MailHelper
    {
        private readonly MailSetting _mailSetting;
        private readonly ILogger<MailHelper> _logger;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Prn222TaskManagementContext _context;

        public MailHelper(IOptions<MailSetting> mailSetting, ILogger<MailHelper> logger, 
            LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor, Prn222TaskManagementContext context
)
        {
            _mailSetting = mailSetting.Value;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
            _context = context;
        }
        public async  System.Threading.Tasks.Task SendGmail(string receiverEmail, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.From = new MailAddress(_mailSetting.Mail, _mailSetting.DisplayName);
            mailMessage.To.Add(receiverEmail);
            mailMessage.IsBodyHtml = true;
            using var smtpClient = new SmtpClient(_mailSetting.Host, _mailSetting.Port)
            {
                Credentials = new NetworkCredential(_mailSetting.Mail, _mailSetting.AppPassword),
                EnableSsl = true
            };

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Send mail successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}
