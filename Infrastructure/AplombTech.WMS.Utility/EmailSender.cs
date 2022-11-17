using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Utility
{
	public class EmailSender
	{
		private static readonly HttpClient HttpClient = new HttpClient();
		public static string SendEmail (string to, string from, string subject, string body)
		{
			try
			{
				string smtpAddress = "wasa-noreply@sinepulse.net";
				string smtpPassword = "hX3uJtj9";
				from = "wasa-noreply@sinepulse.net";

				string uri = "http://emailservice.azurewebsites.net/EmailService.svc/SendMail?to=" + to + "&from=" +
										 from + "&subject=" + subject + "&body=" + body + "&smtpAddress=" + smtpAddress +
										 "&smtpPassword=" + smtpPassword;

				var result = HttpClient.PostAsync(uri, null).Result;
				result.EnsureSuccessStatusCode();
				string responseString = result.Content.ReadAsStringAsync().Result;

				if (responseString.Contains("Success"))
				{
					return "Email Sent";
				}
				else
				{
					return "Email Failed";
				}
			}
			catch (Exception ex)
			{
				return "Email Failed";
			}
		}

		public static bool SendEmail (string to, string subject, string body)
		{
			try
			{
        string smtpAddress = "wasa-noreply@sinepulse.com";
        string smtpPassword = "hX3uJtj9";

        MailMessage mailMessage = new MailMessage();

        string[] mailTos = to.Split(';');
        foreach (string email in mailTos)
        {
          mailMessage.To.Add(email.Trim());
        }
        mailMessage.From = new MailAddress("wasa-noreply@sinepulse.com");
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        mailMessage.IsBodyHtml = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        var smtpClient = new SmtpClient("mail.sinepulse.com", 587)
        {
          Credentials = new NetworkCredential(smtpAddress, smtpPassword),
          EnableSsl = true,
          DeliveryMethod = SmtpDeliveryMethod.Network,
          Timeout = 10000,
          //UseDefaultCredentials = false
        };
        smtpClient.Send(mailMessage);
        mailMessage.Dispose();
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }
	}
}
