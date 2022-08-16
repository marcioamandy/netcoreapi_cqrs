using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;

namespace Globo.PIC.Infra.Email
{
	/// <summary>
	/// 
	/// </summary>
	public class EmailSender : IEmailSender
	{

		string Domain { get; } = Environment.GetEnvironmentVariable("SMTP_DOMAIN");

		string Port { get; } = Environment.GetEnvironmentVariable("SMTP_PORT");

		string User { get; } = Environment.GetEnvironmentVariable("SMTP_USERNAME");

		string Pass { get; } = Environment.GetEnvironmentVariable("SMTP_PASSWORD");

		string From { get; } = Environment.GetEnvironmentVariable("SMTP_FROM");

		string NameFrom { get; } = Environment.GetEnvironmentVariable("SMTP_NAME_FROM");

		string CCO { get; } = Environment.GetEnvironmentVariable("SMTP_CCO");

		/// <summary>
		/// 
		/// </summary>
		private readonly ILogger logger;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_logger"></param>
		/// <param name="_emailSettings"></param>
		public EmailSender(ILogger<EmailSender> _logger)
		{
			logger = _logger;
		}

		public void SendEmail(string to, string cc, string replayTo, string subject, string message, Attachment attachment)
		{
			try
			{
				var mailMessage = produceMailMessage(to, cc, replayTo, subject, message, attachment);
				using (SmtpClient smtp = new SmtpClient(Domain, int.Parse(Port)))
				{
					smtp.EnableSsl = true;
					//smtp.UseDefaultCredentials = false;
					//smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
					smtp.Credentials = new NetworkCredential(User, Pass);
					smtp.Timeout = 10000;

					smtp.Send(mailMessage);
				}
			}
			catch (Exception e)
			{
				string error = "";
				error += "Domain: "+Domain + "\n <br>";
				error += "Domain: " + Port + "\n <br>";
				error += "User: "+ User + "\n <br>";
				error += "Pass: " + Pass + "\n <br>";
				error += "From: " + From + "\n <br>";
				error += "NameFrom: " + NameFrom + "\n <br>";
				error += "CCO: " + CCO +"\n <br>";
				error += "SendMail Erro ao enviar o e-mail." + e.FormatErrorMessage();
				throw new Exception(error);
			}
		}

		public Task SendEmailAsync(string to, string cc, string replayTo, string subject, string message, Attachment attachment)
		{
			try
			{
				var mailMessage = produceMailMessage(to, cc, replayTo, subject, message, attachment);
				using (SmtpClient smtp = new SmtpClient(Domain, int.Parse(Port)))
				{
					smtp.EnableSsl = true;
					//smtp.UseDefaultCredentials = false;
					//smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
					smtp.Credentials = new NetworkCredential(User, Pass);
					smtp.Timeout = 10000;

					return smtp.SendMailAsync(mailMessage);
				}

				#region Log Information

				//StringBuilder sb = new StringBuilder();

				//sb.AppendLine("Email enviado com sucesso...");
				//sb.AppendLine(string.Format("De: {0}", mail.From));
				//sb.AppendLine(string.Format("Para: {0}", mail.To));

				//if (mail.CC != null)
				//	sb.AppendLine(string.Format("CC: {0}", mail.CC));

				//sb.AppendLine(string.Format("Responder Para: {0}", mail.ReplyToList));
				//sb.AppendLine(string.Format("Assunto: {0}", mail.Subject));
				//sb.AppendLine(string.Format("Mensagem: {0}", mail.Body));

				//logger.LogInformation(sb.ToString());

				#endregion Log Information
			}
			catch (Exception e)
			{
				throw new Exception("Erro ao enviar o e-mail." + e.FormatErrorMessage());
			}
		}

		MailMessage produceMailMessage(string to, string cc, string replayTo, string subject, string message, Attachment attachment)
		{
			//var encoding = Encoding.UTF8;
			MailMessage mail = new MailMessage() { From = new MailAddress(From, NameFrom) };

			try
			{
				
				if (!string.IsNullOrWhiteSpace(replayTo))
				{
					var replayToList = replayTo.Split(";");
					foreach (var email in replayToList)
					{
						if (!string.IsNullOrWhiteSpace(email))
							mail.ReplyToList.Add(new MailAddress(email));
					}
				}

				if (!string.IsNullOrWhiteSpace(to))
				{
					var toList = to.Split(";");
					foreach (var email in toList)
					{
						if (!string.IsNullOrWhiteSpace(email))
							mail.To.Add(new MailAddress(email));
					}
				}

				if (!string.IsNullOrWhiteSpace(cc))
				{
					var ccList = cc.Split(";");
					foreach (var ccItem in ccList)
					{
						if (!string.IsNullOrWhiteSpace(ccItem))
							mail.CC.Add(new MailAddress(ccItem));
					}
				}

				if (!string.IsNullOrWhiteSpace(CCO))
				{
					var ccList = CCO.Split(";");
					foreach (var ccItem in ccList)
					{
						if (!string.IsNullOrWhiteSpace(ccItem))
							mail.Bcc.Add(new MailAddress(ccItem));
					}
				}

				mail.Subject = subject;
				mail.Body = message;
				mail.IsBodyHtml = true;
				mail.Priority = MailPriority.Normal;
				//mail.BodyEncoding = encoding;
				//mail.SubjectEncoding = encoding;
				//mail.HeadersEncoding = encoding;				

				if (attachment != null)
					mail.Attachments.Add(attachment);
			}
			catch(Exception error)
            {
				throw new Exception("EmailSender" + error.Message + error.StackTrace.ToString());
            }
			return mail;
		}
	}
}
