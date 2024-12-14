using IDECore.DTOs;
using IDECore.Service.Interface;
using System.Net;
using System.Net.Mail;

namespace IDECore.Service
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmail(EmailDTO emailDTO)
        {
            var eamilTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "ConfirmEmail.html");
            var emailBody = await File.ReadAllTextAsync(eamilTemplatePath);
            emailBody = emailBody.Replace("{{confirmationLink}}", emailDTO.Body);

            var message = new MailMessage
            {
                Body = emailBody,
                From = new MailAddress("Accapt@accaptacounting.ir", "IDEDotNet"),
                Subject = emailDTO.Subject,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(emailDTO.To));

            using (var smtpClient = new SmtpClient("webmail.accaptacounting.ir", 587))
            {
                smtpClient.Credentials = new NetworkCredential("Accapt@accaptacounting.ir", "mahan.z.road0908");
                smtpClient.EnableSsl = false;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                await smtpClient.SendMailAsync(message);
            }
        }
    }
}
