using Globo.PIC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Domain.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface INotificationMethods
    {
        void SendNotificationsByEmail(string title);

        void SendNotificationsByEmail(string title, string template, string link, string emailPrefixSubject);

        void SendNotificationsByEmail(string title, string template, string Destinatarios, string link, string emailPrefixSubject);

        //void SaveNotificacoes(string title, List<Assign> assigns);

        //void SaveNotificacoes(string title, List<Assign> assigns, string link);
    }
}
