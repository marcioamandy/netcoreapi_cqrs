using System;
using System.Linq;
using System.ComponentModel;
using System.Runtime.Serialization;
using Globo.PIC.Domain.Entities;
using System.Collections.Generic;
using MediatR;
using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.ViewModels.Emails;
using System.IO;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Interfaces;

namespace Globo.PIC.Infra.Notifications
{
    public class NotificationMethods : INotificationMethods
    {
        /// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

        /// <summary>
		///
		/// </summary>
		public readonly EmailSettings emailSettings;

        /// <summary>
		/// 
		/// </summary>
		private readonly IUserProvider userProvider;

        /// <summary>
        ///
        /// </summary>
        private readonly IEmailSender emailSender;

        public NotificationMethods(
            IMediator _mediator,
            IEmailSender _emailSender,
            EmailSettings _emailSettings,
            IUserProvider _userProvider
        )
        {
            mediator = _mediator;
            userProvider = _userProvider;
            emailSettings = _emailSettings;
            emailSender = _emailSender;
        }

        //public void SaveNotificacoes(string title, List<Assign> assigns)
        //{
        //    Notificacao notificacao = new Notificacao();

        //    notificacao.Title = string.Format(title);
        //    notificacao.CreatedAt = DateTime.Now;

        //    notificacao.Link = "";

        //    notificacao.Assigns = assigns;

        //    mediator.Send(new SaveNotificacao()
        //    {
        //        Notificacao = notificacao
        //    });
        //}

        //public void SaveNotificacoes(string title, List<Assign> assigns, string link)
        //{
        //    Notificacao notificacao = new Notificacao();

        //    notificacao.Title = title;
        //    notificacao.CreatedAt = DateTime.Now;

        //    notificacao.Link = link;

        //    notificacao.Assigns = assigns;

        //    mediator.Send(new SaveNotificacao()
        //    {
        //        Notificacao = notificacao
        //    });
        //}

        public void SendNotificationsByEmail(string title)
        {
            var defaultEmailVM = new DefaultNotificationViewModel();

            defaultEmailVM.Subject = string.Format("{0}{1}", emailSettings.SubjectPrefixToNotification, title);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Templates", "Email", "body");
            var htmlTemplate = System.IO.File.ReadAllText(Path.Combine(templatePath, "TemplateDefaultNotification.html"));
            htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", title); 
            htmlTemplate = htmlTemplate.Replace("{{LINK_ACTION}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"));
            htmlTemplate = htmlTemplate.Replace("{{LINK_TEXT}}", "Ir para o site");

            defaultEmailVM.Body = htmlTemplate;

            defaultEmailVM.ToEmails.Add(userProvider.User.Email);

            foreach (var email in defaultEmailVM.ToEmails)
            {
                emailSender.SendEmail(email, null, null, defaultEmailVM.Subject, defaultEmailVM.Body, null);
            }
        }

        public void SendNotificationsByEmail(string title, string template, string link, string emailPrefixSubject)
        {
            var defaultEmailVM = new DefaultNotificationViewModel();

            defaultEmailVM.Subject = string.Format("{0}{1}", emailPrefixSubject, title);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Templates", "Email","body");
            var htmlTemplate = File.ReadAllText(Path.Combine(templatePath, template));

            htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", title); 
            htmlTemplate = htmlTemplate.Replace("{{LINK_ACTION}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB") + link);
            htmlTemplate = htmlTemplate.Replace("{{LINK_TEXT}}", "Ir para o site");

            defaultEmailVM.Body = htmlTemplate;

            defaultEmailVM.ToEmails.Add(userProvider.User.Email);

            foreach (var email in defaultEmailVM.ToEmails)
            {
                emailSender.SendEmail(email, null, null, defaultEmailVM.Subject, defaultEmailVM.Body, null);
            }
        }

        public void SendNotificationsByEmail(string title, string template, string Destinatarios, string link, string emailPrefixSubject)
        {
            var To = Destinatarios.Split(";");

            var defaultEmailVM = new DefaultNotificationViewModel();

            defaultEmailVM.Subject = string.Format("{0}{1}", emailPrefixSubject, title);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Templates", "Email","body");
            var htmlTemplate = File.ReadAllText(Path.Combine(templatePath, template));

            var url = Environment.GetEnvironmentVariable("URL_APPLICATION_WEB") + link; 

            htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", title);
            htmlTemplate = htmlTemplate.Replace("{{LINK_ACTION}}", url);
            htmlTemplate = htmlTemplate.Replace("{{LINK_TEXT}}", "Ir para o site");

            defaultEmailVM.Body = htmlTemplate;

            foreach (var s in To)
            {
                defaultEmailVM.ToEmails.Add(s);
            }

            foreach (var email in defaultEmailVM.ToEmails)
            {
                emailSender.SendEmail(email, null, null, defaultEmailVM.Subject, defaultEmailVM.Body, null);
            }
        }
    }
}
