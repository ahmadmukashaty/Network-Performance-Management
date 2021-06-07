using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace E_mailing
{
    public class Email
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string CCAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Host { get; set; }

        public void Send()
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.IsBodyHtml = true;
            mail.To.Add(ToAddress);
            mail.From = new MailAddress(FromAddress);
            mail.Subject = Subject;
            mail.Body = Body;
            mail.CC.Add(CCAddress);
            mail.Priority = System.Net.Mail.MailPriority.High;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(FromAddress, "");
            client.Port = 25;
            client.Host = Host;
            client.EnableSsl = true;
            client.UseDefaultCredentials = true;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                client.Send(mail);
            }
            catch (Exception ex)
            {
               Console.Write(ex.Message);
            }
        }
    }
}
