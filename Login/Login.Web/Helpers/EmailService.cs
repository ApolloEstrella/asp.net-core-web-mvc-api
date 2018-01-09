using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Login.Web.Helpers
{
    public class EmailService
    {
        public static void Send(string recipients, string subject, string body)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("123@gmail.com", "pwd");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("starsoft2k@gmail.com");
            mailMessage.To.Add(recipients);
            mailMessage.Body = body;
            mailMessage.Subject = subject;
            client.Send(mailMessage);
        }
    }
}
