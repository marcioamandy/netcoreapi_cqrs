using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Globo.PIC.Domain.Entities;

namespace Globo.PIC.Domain.Types.Commands
{
    public class DeleteUserRole : DomainCommand
    {
        /// <summary>
        /// entidade UserRole
        /// </summary>
        [Description("UserRole")]
        public UserRole UserRole { get; set; }

        /// <summary>
        /// login de usuário
        /// </summary>
        public string Login { get; set; }


        /// <summary>
        /// nome da role
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// cancellation token
        /// </summary>
        public CancellationToken CancellationToken { get; set; }
    }
}
