using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Security.Cryptography;
using System.Text;

namespace WebApi.Services
{

    public class EmailHelper
    {

        public void Send(string to, string subject, string html)
        {
            var smtpUser = System.Environment.GetEnvironmentVariable("SmtpUser", EnvironmentVariableTarget.Process);

            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(smtpUser));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            var smtpHost = System.Environment.GetEnvironmentVariable("SmtpHost", EnvironmentVariableTarget.Process);
            var smtpPort = int.Parse(System.Environment.GetEnvironmentVariable("SmtpPort", EnvironmentVariableTarget.Process));
            var smtpPass = System.Environment.GetEnvironmentVariable("SmtpPass", EnvironmentVariableTarget.Process);

            using var smtp = new SmtpClient();
            try
            {
                smtp.Connect(smtpHost, smtpPort, SecureSocketOptions.Auto);
                smtp.Authenticate(smtpUser, smtpPass);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch(Exception e)
            {
                
            }
        }

        public string GetUnsubscribeHash(string email)
        {
            var secret = System.Environment.GetEnvironmentVariable("Secret", EnvironmentVariableTarget.Process);

            using (HashAlgorithm algorithm = SHA256.Create())
            {
                return GetHashString(algorithm.ComputeHash(Encoding.UTF8.GetBytes(secret + email)));
            }
        }
        private string GetHashString(byte[] hash)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

    }
}