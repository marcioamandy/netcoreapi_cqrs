using System.Net.Mail;
using System.Threading.Tasks;

namespace Globo.PIC.Domain.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEmailSender
    {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="to"></param>
		/// <param name="CC"></param>
		/// <param name="replayTo"></param>
		/// <param name="subject"></param>
		/// <param name="message"></param>
		/// <param name="attachment"></param>
		/// <returns></returns>
    Task SendEmailAsync(string to, string cc, string replayTo, string subject, string message, Attachment attachment);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="to"></param>
		/// <param name="CC"></param>
		/// <param name="replayTo"></param>
		/// <param name="subject"></param>
		/// <param name="message"></param>
		/// <param name="attachment"></param>
		/// <returns></returns>
		void SendEmail(string to, string cc, string replayTo, string subject, string message, Attachment attachment);
	}
}
