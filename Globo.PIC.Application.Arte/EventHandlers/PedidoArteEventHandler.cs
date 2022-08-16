using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Infra.Notifications;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Types.Commands;
using Globo.PIC.Domain.Types.Events;
using Globo.PIC.Domain.Types.Events.Notificações;
using Globo.PIC.Domain.Types.Events.Notificações.Suprimentos;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Application.Arte.EventHandlers
{

    public class PedidoArteEventHandler
        :
        //INotificationHandler<OnNotificacaoVista>,
        INotificationHandler<OnDataNecessidadeAlterada>,
        INotificationHandler<OnDataReenvioAlterada>,
        INotificationHandler<OnCancelamentoPedidoSolicitado>,
        INotificationHandler<OnStatusPedidoArteAlterado>,
        INotificationHandler<OnCompradorAlterado>,
        INotificationHandler<OnBaseAlterada>,
        INotificationHandler<OnDevolucaoPedido>,
        INotificationHandler<OnDevolucaoPedidoItem>,
        INotificationHandler<OnAprovacaoPedido>,
        INotificationHandler<OnVerificarStatus>,
        //INotificationHandler<OnVerificarStatusHF>,
        INotificationHandler<OnAtualizarQtdeItens>,
        //INotificationHandler<OnEnviarPedidoOracle>,
        INotificationHandler<OnCancelamentoItem>,
        INotificationHandler<OnItemReprovado>,
        INotificationHandler<OnItemAprovado>,
        INotificationHandler<OnCancelamentoItemNegado>,
        INotificationHandler<OnCancelamentoItemSolicitado>,
        INotificationHandler<OnItemAtribuidoComprador>,
        INotificationHandler<OnItemEntregue>,
        INotificationHandler<OnNovoPedidoArte>,
        INotificationHandler<OnPedidoFinalizado>,
        //Notificações novas - Inicio
        INotificationHandler<OnCancelamentoAprovadoItem>,//feito
        INotificationHandler<OnCancelamentoNegadoItem>,//feito
        INotificationHandler<OnItemDevolvido>,//feito
        INotificationHandler<OnItemEntregueParcialmente>,//feito
        INotificationHandler<OnItensResolvidosPedidoFinalizado>,//feito
        INotificationHandler<OnPedidoEnviado>,//feito
        INotificationHandler<OnRCAcaReprovadoItem>,
        INotificationHandler<OnRCSemAcaReprovadaItem>,
        INotificationHandler<OnBaseAtribuiItem>,//feito
        INotificationHandler<OnDataNecessidadeItemSemAcaEditada>,//feito
        INotificationHandler<OnDevolucaoItensBase>,//feito
        INotificationHandler<OnPedidoRecebidoComprador>,//feito
        INotificationHandler<OnRecebeCancelamentoItem>,//feito
        INotificationHandler<OnReenvioItensDevolvidos>,//feito
                                                       //Notificações novas - fim
        INotificationHandler<OnDLCancelationLine>
    {
        private readonly ILineProxy line;

        /// <summary>
        /// 
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Prefix para assunto dos emails
        /// </summary>
        string EmailPrefixSubject { get; } = Environment.GetEnvironmentVariable("EMAIL_PREFIX_SUBJECT");

        /// <summary>
        /// 
        /// </summary>
        static string EmailErrorTo { get; } = Environment.GetEnvironmentVariable("EMAIL_ERROR_TO");

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Notificacao> notificationRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<StatusPedidoItemArte> statusPedidoItemRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItem> pedidoItemRepository;

        /// <summary>
		/// 
		/// </summary>
		private readonly IRepository<PedidoItemArte> pedidoItemArteRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Pedido> pedidoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoArte> pedidoArteRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItemArteCompra> pedidoItemCompraRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<PedidoItemArteEntrega> pedidoItemEntregaRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<StatusPedidoArte> statusPedidoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Usuario> userRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IUserProvider userProvider;

        /// <summary>
        ///
        /// </summary>
        private readonly IEmailSender emailSender;

        /// <summary>
		///
		/// </summary>
		private readonly INotificationMethods notificationMethods;

        private readonly IRepository<Equipe> equipeRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMediator mediator;

        private readonly IExpenditureProxy expenditureProxy;

        public PedidoArteEventHandler() { }

        private readonly IPurchaseRequisitionProxy purchaseRequisitionProxy;

        /// <summary>
        ///
        /// </summary>
        public EmailSettings emailSettings { get; }

        public PedidoArteEventHandler(
            IUnitOfWork _unitOfWork,
            IRepository<PedidoItem> _pedidoItemRepository,
            IRepository<PedidoItemArte> _pedidoItemArteRepository,
            IRepository<Notificacao> _notificationRepository,
            IRepository<StatusPedidoItemArte> _statusPedidoItemRepository,
            IRepository<Usuario> _userRepository,
            IRepository<StatusPedidoArte> _statusPedidoRepository,
            IRepository<Pedido> _pedidoRepository,
            IRepository<PedidoArte> _pedidoArteRepository,
            IRepository<PedidoItemArteCompra> _pedidoItemCompraRepository,
            IRepository<PedidoItemArteEntrega> _pedidoItemEntregaRepository,
            IUserProvider _userProvider,
            IMediator _mediator,
            IEmailSender _emailSender,
            ILineProxy _line,
            INotificationMethods _notificationMethods,
            EmailSettings _emailSettings,
            IPurchaseRequisitionProxy _purchaseRequisitionProxy,
            IExpenditureProxy _expenditureProxy,
            IRepository<Equipe> _equipeRepository)
        {
            unitOfWork = _unitOfWork;
            pedidoItemRepository = _pedidoItemRepository;
            pedidoItemArteRepository = _pedidoItemArteRepository;
            notificationRepository = _notificationRepository;
            statusPedidoItemRepository = _statusPedidoItemRepository;
            userRepository = _userRepository;
            statusPedidoRepository = _statusPedidoRepository;
            pedidoRepository = _pedidoRepository;
            pedidoArteRepository = _pedidoArteRepository;
            pedidoItemCompraRepository = _pedidoItemCompraRepository;
            pedidoItemEntregaRepository = _pedidoItemEntregaRepository;
            userProvider = _userProvider;
            mediator = _mediator;
            emailSettings = _emailSettings;
            line = _line;
            purchaseRequisitionProxy = _purchaseRequisitionProxy;
            emailSender = _emailSender;
            notificationMethods = _notificationMethods;
            expenditureProxy = _expenditureProxy;
            equipeRepository = _equipeRepository;
        }


        #region "Disparo  de Emails"

        private void EnviarEmailDataNecessidadeAlterada(Pedido pedido)
        {
            string pathDirectory = Directory.GetCurrentDirectory();
            var templatePathBody = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "body");
            var templatePathTitle = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "title");
            var templatePathContent = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "content");

            var htmlTemplate = File.ReadAllText(Path.Combine(templatePathBody, "TemplateDefaultNotification.html"));
            string emailTo = string.Empty;
            string title = string.Empty;
            string content = string.Empty;

            title = File.ReadAllText(Path.Combine(templatePathTitle, "TitleItemDataNecessidadeAlterada.html"));
            content = File.ReadAllText(Path.Combine(templatePathContent, "ContentItemDataNecessidadeAlterada.html"));

            htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", content)
                .Replace("{{titulo_pedido}}", pedido.Titulo)
                .Replace("{{url_base}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"))
                .Replace("{{pedido_id}}", pedido.Id.ToString());

            //envia o mesmo email para demandante e equipe e base de suprimentos;
            //A fazer: dando bug no put
            //Item em Análise, essa mensagem vai para a base do pedido
            //Item Atribuído ao Comprador, Solicitação de compra enviada, e aprovado, esse email vai para o comprador.
            //Não enviar e - mail para a equipe

            if (pedido.Itens.Where(a => a.PedidoItemArte.IdStatus == (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA).Count() > 0)
            {
                emailTo = pedido.PedidoArte.Base.Email + ";";
                emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
            }

            if (pedido.Itens.Where(a => a.PedidoItemArte.IdStatus == (int)PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR
            || a.PedidoItemArte.IdStatus == (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRAENVIADA
            || a.PedidoItemArte.IdStatus == (int)PedidoItemArteStatus.ITEM_APROVADO).Count() > 0)
            {
                emailTo = pedido.CriadoPor.Email + ";";
                emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
            }

            //if (pedido.Itens.Where(user => user.UserComprador != null).Distinct().Select(a => a.UserComprador).Count() > 0)
            //         {
            //	emailTo = pedido.Itens.Where(user => user.UserComprador != null).Distinct().Select(a => a.UserComprador.Email).Aggregate((x, y) => x + ";" + y);//envia para a equipe;

            //	emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
            //} 
        }

        private void EnviarEmailItemReenvio(PedidoItem pedidoItem)
        {
            string pathDirectory = Directory.GetCurrentDirectory();
            var templatePathBody = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "body");
            var templatePathTitle = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "title");
            var templatePathContent = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "content");

            var htmlTemplate = File.ReadAllText(Path.Combine(templatePathBody, "TemplateDefaultNotification.html"));
            string emailTo = string.Empty;
            string title = string.Empty;

            title = File.ReadAllText(Path.Combine(templatePathTitle, "TitleItemReenvio.html"));
            string content = File.ReadAllText(Path.Combine(templatePathContent, "ContentItemReenvio.html"));

            htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", content)
                .Replace("{{titulo_pedido}}", pedidoItem.Pedido.Titulo)
                .Replace("{{url_base}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"))
                .Replace("{{pedido_id}}", pedidoItem.IdPedido.ToString());

            //e - mail: reenviado do demandante para a base: apenas a base recebe o e - mail
            //e - mail: reenviado do demandante para o comprador: apenas o comprador recebe o e-mail(para os dois perfis, comprador externo e estruturado)

            if (pedidoItem.PedidoItemArte.FlagDevolvidoBase)
            {
                //email para o demandante
                emailTo = pedidoItem.Pedido.PedidoArte.Base.Email + ";";
                emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
            }
            else if (pedidoItem.PedidoItemArte.FlagDevolvidoComprador)
            {
                //email para a base
                emailTo = pedidoItem.PedidoItemArte.Comprador.Email + ";";
                emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
            }
        }

        ///// <summary>
        ///// envia email para demandante, base, comprador e equipe 
        ///// </summary>
        ///// <param name="pedidoItem"></param>
        private void EnviarEmailItemCancelado(PedidoItem pedidoItem)
        {

            string pathDirectory = Directory.GetCurrentDirectory();
            var templatePathBody = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "body");
            var templatePathTitle = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "title");
            var templatePathContent = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "content");
            string emailTo = string.Empty;
            string title = string.Empty;
            string content = string.Empty;
            List<string> emails = new List<string>();
            var htmlTemplate = File.ReadAllText(Path.Combine(templatePathBody, "TemplateDefaultNotification.html"));

            title = File.ReadAllText(Path.Combine(templatePathTitle, "TitleItemCancelado.html"));
            content = File.ReadAllText(Path.Combine(templatePathContent, "ContentItemCancelado.html"));

            htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", content)
                .Replace("{{titulo_pedido}}", pedidoItem.Pedido.Titulo)
                .Replace("{{url_base}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"))
                .Replace("{{pedido_id}}", pedidoItem.Pedido.Id.ToString());

            //email para o demandante
            emailTo = pedidoItem.Pedido.CriadoPor.Email + ";";
            //emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);

            //email para a equipe
            if (pedidoItem.Pedido.Equipe.Count() > 0)
            {
                emailTo = pedidoItem.Pedido.
                    Equipe.Distinct().
                    Select(a => a.Usuario.Email).Aggregate((x, y) => x + ";" + y);//envia para a equipe;
                //emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
            }

        }

        ///// <summary>
        ///// envia email para demandante, base, comprador e equipe 
        ///// </summary>
        ///// <param name="pedidoItem"></param>
        private void EnviarEmailItemDevolvido(PedidoItem pedidoItem)
        {
            string pathDirectory = Directory.GetCurrentDirectory();
            var templatePathBody = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "body");
            var templatePathTitle = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "title");
            var templatePathContent = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "content");
            string emailTo = string.Empty;
            string title = string.Empty;
            string content = string.Empty;
            List<string> emails = new List<string>();
            var htmlTemplate = File.ReadAllText(Path.Combine(templatePathBody, "TemplateDefaultNotification.html"));

            title = File.ReadAllText(Path.Combine(templatePathTitle, "TitleItemDevolvido.html"));
            content = File.ReadAllText(Path.Combine(templatePathContent, "ContentItemDevolvido.html"));

            htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", content)
                .Replace("{{titulo_pedido}}", pedidoItem.Pedido.Titulo)
                .Replace("{{url_base}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"))
                .Replace("{{pedido_id}}", pedidoItem.Pedido.Id.ToString());

            if (pedidoItem.PedidoItemArte.FlagDevolvidoBase)
            {
                //email para o demandante
                emailTo = pedidoItem.Pedido.CriadoPor.Email + ";";
                emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
            }
            else if (pedidoItem.PedidoItemArte.FlagDevolvidoComprador)
            {
                //email para a base
                emailTo = pedidoItem.Pedido.PedidoArte.Base.Email + ";";
                emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
            }
        }

        ///// <summary>
        ///// envia email apenas para o demandante
        ///// </summary>
        ///// <param name="pedido"></param>
        private void EnviarEmailItemReprovado(PedidoItem pedidoItem)
        {
            string pathDirectory = Directory.GetCurrentDirectory();
            var templatePathBody = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "body");
            var templatePathTitle = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "title");
            var templatePathContent = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "content");
            string emailTo = string.Empty;
            string title = string.Empty;
            string content = string.Empty;
            List<string> emails = new List<string>();
            var htmlTemplate = File.ReadAllText(Path.Combine(templatePathBody, "TemplateDefaultNotification.html"));

            title = File.ReadAllText(Path.Combine(templatePathTitle, "TitleItemReprovado.html"));
            content = File.ReadAllText(Path.Combine(templatePathContent, "ContentItemReprovado.html"));

            htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", content)
                .Replace("{{titulo_pedido}}", pedidoItem.Pedido.Titulo)
                .Replace("{{url_base}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"))
                .Replace("{{pedido_id}}", pedidoItem.IdPedido.ToString());

            emailTo = pedidoItem.Pedido.CriadoPor.Email + ";";
            emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
        }

        ///// <summary>
        ///// envia email apenas para o demandante
        ///// </summary>
        ///// <param name="pedido"></param>
        private void EnviarEmailItemAprovado(PedidoItem pedidoItem)
        {
            string pathDirectory = Directory.GetCurrentDirectory();
            var templatePathBody = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "body");
            var templatePathTitle = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "title");
            var templatePathContent = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "content");
            string emailTo = string.Empty;
            string title = string.Empty;
            string content = string.Empty;
            List<string> emails = new List<string>();
            var htmlTemplate = File.ReadAllText(Path.Combine(templatePathBody, "TemplateDefaultNotification.html"));

            title = File.ReadAllText(Path.Combine(templatePathTitle, "TitleItemAprovado.html"));
            content = File.ReadAllText(Path.Combine(templatePathContent, "ContentItemAprovado.html"));

            htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", content)
                .Replace("{{titulo_pedido}}", pedidoItem.Pedido.Titulo)
                .Replace("{{url_base}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"))
                .Replace("{{pedido_id}}", pedidoItem.IdPedido.ToString());

            emailTo = pedidoItem.Pedido.CriadoPor.Email + ";";

            emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
        }

        private void EnviarEmailItemEntregue(PedidoItem pedidoItem)
        {
            string pathDirectory = Directory.GetCurrentDirectory();
            var templatePathBody = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "body");
            var templatePathTitle = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "title");
            var templatePathContent = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "content");
            string emailTo = string.Empty;
            string title = string.Empty;
            string content = string.Empty;
            List<string> emails = new List<string>();
            var htmlTemplate = File.ReadAllText(Path.Combine(templatePathBody, "TemplateDefaultNotification.html"));

            title = File.ReadAllText(Path.Combine(templatePathTitle, "TitleItemEntregue.html"));
            content = File.ReadAllText(Path.Combine(templatePathContent, "ContentItemEntregue.html"));

            htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", content)
                .Replace("{{titulo_pedido}}", pedidoItem.Pedido.Titulo)
                .Replace("{{url_base}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"))
                .Replace("{{pedido_id}}", pedidoItem.IdPedido.ToString());

            emailTo = pedidoItem.Pedido.CriadoPor.Email + ";";

            emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
        }

        private void EnviarEmailPedidoFinalizado(Pedido pedido)
        {
            string pathDirectory = Directory.GetCurrentDirectory();
            var templatePathBody = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "body");
            var templatePathTitle = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "title");
            var templatePathContent = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "content");
            string emailTo = string.Empty;
            string title = string.Empty;
            string content = string.Empty;
            List<string> emails = new List<string>();
            var htmlTemplate = File.ReadAllText(Path.Combine(templatePathBody, "TemplateDefaultNotification.html"));

            title = File.ReadAllText(Path.Combine(templatePathTitle, "TitlePedidoFinalizado.html"));
            content = File.ReadAllText(Path.Combine(templatePathContent, "ContentPedidoFinalizado.html"));

            htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", content)
                .Replace("{{titulo_pedido}}", pedido.Titulo)
                .Replace("{{url_base}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"))
                .Replace("{{pedido_id}}", pedido.Id.ToString());

            //email demandante e base separado
            emailTo = pedido.CriadoPor.Email + ";";
            emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);

            //email para a equipe tudo junto
            if (pedido.Equipe.Count() > 0)
            {
                emailTo = pedido.Equipe.Distinct().Select(a => a.Usuario.Email).Aggregate((x, y) => x + ";" + y);//envia para a equipe;
                emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
            }
        }

        private void EnviarEmailNovoPedido(Pedido pedido)
        {
            string pathDirectory = Directory.GetCurrentDirectory();
            var templatePathBody = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "body");
            var templatePathTitle = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "title");
            var templatePathContent = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "content");

            string emailTo = string.Empty;
            string title = string.Empty;
            string content = string.Empty;
            List<string> emails = new List<string>();

            //1 demandante/equipe
            var htmlTemplate = File.ReadAllText(Path.Combine(templatePathBody, "TemplateDefaultNotification.html"));
            title = File.ReadAllText(Path.Combine(templatePathTitle, "TitlePedidoNovo.html"));

            content = File.ReadAllText(Path.Combine(templatePathContent, "ContentPedidoNovoBaseComprador.html"));
            htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", content)
                .Replace("{{titulo_pedido}}", pedido.Titulo)
                .Replace("{{url_base}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"))
                .Replace("{{pedido_id}}", pedido.Id.ToString());

            var queryUserFilter = userRepository.GetAll()
                .Where(u => u.IsActive.Equals(true) && !string.IsNullOrWhiteSpace(u.Email) &&
                 u.Roles.Any(r => r.Name.Equals(Role.GRANT_ADM_USUARIOS.ToString())))
                .AsQueryable()
                .Select(
                    user => new Usuario
                    {
                        Email = user.Email,
                        Name = user.Name,
                        LastName = user.LastName,
                        Login = user.Login,
                        IsActive = user.IsActive,
                        Roles = user.Roles.Where(role => role.Name.Equals(Role.GRANT_ADM_USUARIOS.ToString()))
                    }).ToList();

            foreach (var user in queryUserFilter)
            {
                emailTo = user.Email;
                emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
            }
        }

        ///// <summary>
        ///// envia email apenas para o demandante
        ///// </summary>
        ///// <param name="pedido"></param>
        private void EnviarEmailItemCancelamentoNegado(PedidoItem pedidoItem)
        {
            string pathDirectory = Directory.GetCurrentDirectory();
            var templatePathBody = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "body");
            var templatePathTitle = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "title");
            var templatePathContent = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "content");
            string emailTo = string.Empty;
            string title = string.Empty;
            string content = string.Empty;
            List<string> emails = new List<string>();
            var htmlTemplate = File.ReadAllText(Path.Combine(templatePathBody, "TemplateDefaultNotification.html"));

            title = File.ReadAllText(Path.Combine(templatePathTitle, "TitleItemCancelamentoNegado.html"));
            content = File.ReadAllText(Path.Combine(templatePathContent, "ContentItemCancelamentoNegado.html"));

            htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", content)
                .Replace("{{titulo_pedido}}", pedidoItem.Pedido.Titulo)
                .Replace("{{url_base}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"))
                .Replace("{{pedido_id}}", pedidoItem.IdPedido.ToString());

            //email para o demandante
            emailTo = pedidoItem.Pedido.CriadoPor.Email + ";";
            emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);

            //email para a equipe
            //if (pedidoItem.Pedido.Equipe.Count() > 0)
            //{
            //	emailTo = pedidoItem.Pedido.Equipe.Distinct().Select(a => a.User.Email).Aggregate((x, y) => x + ";" + y);//envia para a equipe;
            //	emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
            //}

        }

        private void EnviarEmailItemCancelamentoSolicitado(PedidoItem pedidoItem, CancellationToken cancellationToken)
        {
            string pathDirectory = Directory.GetCurrentDirectory();
            var templatePathBody = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "body");
            var templatePathTitle = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "title");
            var templatePathContent = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "content");
            string emailTo = string.Empty;
            string title = string.Empty;
            string content = string.Empty;

            /*Solicitação de Cancelamento de Itens do Pedido Noite no Parque de Diversões.
        	Item em Análise, essa mensagem vai para a base do pedido
        	Item Atribuído ao Comprador, Solicitação de compra enviada, ou aprovado, esse e-mail vai para o comprador estruturada.
        	O e-mail nunca vai para os dois perfis ao mesmo tempo
        	*/

            var idStatus = pedidoItem.PedidoItemArte.Status.Id;
            if (idStatus == (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA)
            {
                //email base

                var htmlTemplate = File.ReadAllText(Path.Combine(templatePathBody, "TemplateDefaultNotification.html"));
                title = File.ReadAllText(Path.Combine(templatePathTitle, "TitleItemCancelamentoSolicitado.html"));
                content = File.ReadAllText(Path.Combine(templatePathContent, "ContentItemCancelamentoSolicitado.html"));

                htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", content)
                    .Replace("{{titulo_pedido}}", pedidoItem.Pedido.Titulo)
                    .Replace("{{url_base}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"))
                    .Replace("{{pedido_id}}", pedidoItem.IdPedido.ToString());



                emailTo = pedidoItem.Pedido.PedidoArte.Base.Email + ";";
                emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
            }
            else if (idStatus == (int)PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR ||
                idStatus == (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRAENVIADA || idStatus == (int)PedidoItemArteStatus.ITEM_APROVADO)
            {
                var htmlTemplate = File.ReadAllText(Path.Combine(templatePathBody, "TemplateDefaultNotification.html"));
                title = File.ReadAllText(Path.Combine(templatePathTitle, "TitleItemCancelamentoSolicitado.html"));
                content = File.ReadAllText(Path.Combine(templatePathContent, "ContentItemCancelamentoSolicitado.html"));

                htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", content)
                    .Replace("{{titulo_pedido}}", pedidoItem.Pedido.Titulo)
                    .Replace("{{url_base}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"))
                    .Replace("{{pedido_id}}", pedidoItem.IdPedido.ToString());

                //email para o comprador
                emailTo = pedidoItem.PedidoItemArte.Comprador.Email + ";";
                emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
            }

        }
        private void EnviarEmailItemAtribuidoComprador(Pedido pedido)
        {
            if (pedido.CriadoPor.Email != string.Empty)
            {
                string emailTo = pedido.CriadoPor.Email + ";";
                string pathDirectory = Directory.GetCurrentDirectory();
                var templatePathBody = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "body");
                var templatePathTitle = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "title");
                var templatePathContent = Path.Combine(pathDirectory, "Assets", "Templates", "Email", "content");

                string title = string.Empty;
                string content = string.Empty;
                List<string> emails = new List<string>();
                var htmlTemplate = File.ReadAllText(Path.Combine(templatePathBody, "TemplateDefaultNotification.html"));

                title = File.ReadAllText(Path.Combine(templatePathTitle, "TitleItemAtribuidoComprador.html"));
                content = File.ReadAllText(Path.Combine(templatePathContent, "ContentItemAtribuidoComprador.html"));

                htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", content)
                    .Replace("{{titulo_pedido}}", pedido.Titulo)
                    .Replace("{{url_base}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"))
                    .Replace("{{pedido_id}}", pedido.Id.ToString());

                emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
            }
        }
        #endregion


        #region eventos
        Task INotificationHandler<OnBaseAlterada>.Handle(OnBaseAlterada notification, CancellationToken cancellationToken)
        {
            //Todo: Remover try quando bus de eventos estiver assincrono.
            //Enquanto este evendo estiver vindo da thread sincronda na query de notificações,
            // o try evita que a consulta seja travada com alguma exception.

            try
            {
                if (notification.Pedido.PedidoArte.IdStatus == (int)PedidoArteStatus.PEDIDO_ENVIADO)
                {
                    unitOfWork.BeginTransaction();

                    var statusVincularBase = statusPedidoRepository.GetById((int)PedidoArteStatus.PEDIDO_EMANDAMENTO, cancellationToken).GetAwaiter().GetResult();

                    var pedidoArte = pedidoArteRepository.GetById(notification.Pedido.PedidoArte.Id, cancellationToken).GetAwaiter().GetResult();

                    pedidoArte.Base = userRepository.GetByLogin(notification.Pedido.PedidoArte.BaseLogin, cancellationToken).GetAwaiter().GetResult();

                    pedidoArte.DataVinculoBase = DateTime.Now;

                    pedidoArte.IdStatus = (int)PedidoArteStatus.PEDIDO_EMANDAMENTO;

                    pedidoArte.Status = statusVincularBase;

                    pedidoArteRepository.AddOrUpdate(pedidoArte, cancellationToken);

                    var resultPedido = unitOfWork.SaveChanges();

                    if (!resultPedido) throw new ApplicationException("An error has occured.");

                    var statusVincularBaseItem = statusPedidoItemRepository.GetById((int)Domain.Enums.PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA, cancellationToken).GetAwaiter().GetResult();

                    foreach (var pedidoItem in notification.Pedido.Itens)
                    {
                        if (string.IsNullOrWhiteSpace(pedidoItem.RCs.FirstOrDefault().Acordo))
                        {
                            pedidoItem.PedidoItemArte.IdStatus = (int)Domain.Enums.PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA;

                            pedidoItem.PedidoItemArte.Status = statusVincularBaseItem;

                            pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);

                            var result = unitOfWork.SaveChanges();

                            if (!result) throw new ApplicationException("An error has occured.");

                            mediator.Send(new AddTrackingArte()
                            {
                                PedidoItemArteTracking = new PedidoItemArteTracking()
                                {
                                    IdPedidoItemArte = pedidoItem.PedidoItemArte.Id,
                                    StatusId = pedidoItem.PedidoItemArte.IdStatus,
                                    ChangeById = pedidoItem.Pedido.PedidoArte.BaseLogin
                                }
                            }, cancellationToken);
                        }
                    }

                    unitOfWork.CommitTransaction();

                    mediator.Publish(new OnBaseAtribuiItem()
                    {
                        Pedido = notification.Pedido
                    });
                }
            }
            catch (Exception error)
            {
                unitOfWork.RollbackTransaction();
                throw new Exception(error.Message);
            }

            return Task.FromResult(notification);
        }

        Task INotificationHandler<OnCompradorAlterado>.Handle(OnCompradorAlterado notification, CancellationToken cancellationToken)
        {
            //Todo: Remover try quando bus de eventos estiver assincrono.
            //Enquanto este evendo estiver vindo da thread sincronda na query de notificações, o try evita que a consulta seja travada com alguma exception.

            try
            {
                unitOfWork.BeginTransaction();

                if (notification.Pedido.PedidoArte.IdStatus != (int)PedidoArteStatus.PEDIDO_RASCUNHO && notification.Pedido.PedidoArte.IdStatus != (int)PedidoArteStatus.PEDIDO_ENVIADO)
                {
                    if (userProvider.IsRole(Role.PERFIL_BASE_SUPRIMENTOS))
                    {
                        var statusAtribuirComprador = statusPedidoItemRepository.GetById((int)Domain.Enums.PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR, cancellationToken).GetAwaiter().GetResult();

                        foreach (var pedidoItem in notification.Pedido.Itens)
                        {
                            if (string.IsNullOrWhiteSpace(pedidoItem.RCs.FirstOrDefault().Acordo))
                            {
                                pedidoItem.PedidoItemArte.IdStatus = (int)Domain.Enums.PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR;

                                pedidoItem.PedidoItemArte.Status = statusAtribuirComprador;

                                pedidoItem.PedidoItemArte.CompradoPorLogin = notification.CompradoPorLogin;

                                pedidoItem.PedidoItemArte.Comprador = userRepository.GetByLogin(notification.CompradoPorLogin, cancellationToken).GetAwaiter().GetResult();

                                //pedidoItem.IdTipo = notification.Pedido.IdTipo;

                                pedidoItem.PedidoItemArte.DataVinculoComprador = DateTime.Now;

                                pedidoItemArteRepository.AddOrUpdate(pedidoItem.PedidoItemArte, cancellationToken);

                                var result = unitOfWork.SaveChanges();

                                if (!result) throw new ApplicationException("An error has occured.");

                                pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);

                                result = unitOfWork.SaveChanges();

                                if (!result) throw new ApplicationException("An error has occured.");

                                pedidoItem.Observacao = notification.Pedido.Observacao;

                                mediator.Send(new AddTrackingArte()
                                {
                                    PedidoItemArteTracking = new PedidoItemArteTracking()
                                    {
                                        IdPedidoItemArte = pedidoItem.PedidoItemArte.Id,
                                        StatusId = pedidoItem.PedidoItemArte.IdStatus,
                                        ChangeById = notification.CompradoPorLogin
                                    }
                                }, cancellationToken);
                            }
                        }

                        unitOfWork.CommitTransaction();

                        var pedidoWithOutRoles = pedidoRepository.GetByIdPedidoWithOutRoles(notification.Pedido.Id, cancellationToken).GetAwaiter().GetResult();

                        mediator.Publish(new OnVerificarStatus() { Pedido = pedidoWithOutRoles });
                    }
                    else
                        throw new BadRequestException("Somente o perfil de Base Suprimentos pode atribuir um comprador.");
                }
            }
            catch (Exception error)
            {
                unitOfWork.RollbackTransaction();
                throw new Exception(error.Message);
            }

            return Task.FromResult(notification);
        }

        Task INotificationHandler<OnVerificarStatus>.Handle(OnVerificarStatus request, CancellationToken cancellationToken)
        {
            //Todo: Remover try quando bus de eventos estiver assincrono.
            //Enquanto este evendo estiver vindo da thread sincronda na query de notificações, o try evita que a consulta seja travada com alguma exception.
            bool isStatusFinalizado = true, isEstruturada = false, isExterna = false, isCancelado = false;
            int idTag = 0;
            try
            {

                foreach (var pedidoItem in request.Pedido.Itens.Select(a => a.PedidoItemArte))
                {
                    if (pedidoItem.IdTipo == (int)TagPedido.ESTRUTURADA)
                    {
                        isEstruturada = true;
                        idTag = (int)TagPedido.ESTRUTURADA;
                    }

                    if (pedidoItem.IdTipo == (int)TagPedido.EXTERNA)
                    {
                        isExterna = true;
                        idTag = (int)TagPedido.EXTERNA;
                    }

                    if (isStatusFinalizado)
                    {
                        switch (pedidoItem.IdStatus)
                        {
                            case (int)PedidoItemArteStatus.ITEM_APROVADO:
                                {
                                    isStatusFinalizado = true;
                                    break;
                                }
                            case (int)PedidoItemArteStatus.ITEM_CANCELAMENTONEGADO:
                                {
                                    isStatusFinalizado = true;
                                    break;
                                }
                            case (int)PedidoItemArteStatus.ITEM_REPROVADO:
                                {
                                    isStatusFinalizado = true;
                                    break;
                                }
                            case (int)PedidoItemArteStatus.ITEM_CANCELADO:
                                {
                                    int finalizados = request
                                         .Pedido
                                         .Itens
                                         .Select(a => a.PedidoItemArte).Where(a =>
                                         a.IdStatus.Equals((int)PedidoItemArteStatus.ITEM_CANCELADO)).Count();

                                    int total = request
                                         .Pedido
                                         .Itens
                                         .Select(a => a.PedidoItemArte).Count();

                                    isCancelado = (total == finalizados);
                                    isStatusFinalizado = false;
                                    break;
                                }
                            case (int)PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR:
                                {
                                    isStatusFinalizado = false;
                                    break;
                                }
                            case (int)PedidoItemArteStatus.ITEM_CANCELAMENTOSOLICITADO:
                                {
                                    isStatusFinalizado = false;
                                    break;
                                }
                            case (int)PedidoItemArteStatus.ITEM_DEVOLUCAO:
                                {
                                    isStatusFinalizado = false;
                                    break;
                                }
                            case (int)PedidoItemArteStatus.ITEM_REENVIO:
                                {
                                    isStatusFinalizado = false;
                                    break;
                                }
                            //case (int)PedidoItemArteStatus.ITEM_EMANALISE:
                            //    {
                            //        statusFinalizado = false;
                            //        break;
                            //    }
                            case (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRACRIADA:
                                {
                                    isStatusFinalizado = false;
                                    break;
                                }
                            case (int)PedidoItemArteStatus.ITEM_SOLICITACAODECOMPRAENVIADA:
                                {
                                    isStatusFinalizado = false;
                                    break;
                                }
                            case (int)PedidoItemArteStatus.ITEM_ENTREGUEPARCIALMENTE:
                                {
                                    isStatusFinalizado = false;
                                    break;
                                }
                            case (int)PedidoItemArteStatus.ITEM_ENTREGUE:
                                {
                                    isStatusFinalizado = true;
                                    break;
                                }
                            default:
                                {
                                    isStatusFinalizado = false;
                                    break;
                                }
                        }
                    }
                }

                unitOfWork.BeginTransaction();

                //var pedido = pedidoRepository.GetByIdPedidoWithOutRoles(request.Pedido.Id, cancellationToken).GetAwaiter().GetResult();


                if (isStatusFinalizado)
                {
                    var statusFinalizado = statusPedidoRepository.GetById((int)PedidoArteStatus.PEDIDO_FINALIZADO, cancellationToken).GetAwaiter().GetResult();
                    request.Pedido.PedidoArte.IdStatus = (int)PedidoArteStatus.PEDIDO_FINALIZADO;
                    request.Pedido.PedidoArte.Status = statusFinalizado;
                    mediator.Publish(new OnPedidoFinalizado() { Pedido = request.Pedido });
                }


                if (isCancelado)
                {

                    var statusCancelado = statusPedidoRepository.GetById((int)PedidoArteStatus.PEDIDO_CANCELADO, cancellationToken).GetAwaiter().GetResult();
                    request.Pedido.PedidoArte.IdStatus = (int)PedidoArteStatus.PEDIDO_CANCELADO;
                    request.Pedido.PedidoArte.Status = statusCancelado;
                }

                if (isEstruturada && isExterna)
                {
                    request.Pedido.IdTag = (int)TagPedido.MISTA;
                }
                else
                    request.Pedido.IdTag = idTag;

                pedidoRepository.AddOrUpdate(request.Pedido, cancellationToken);

                var resultPedido = unitOfWork.SaveChanges();

                if (!resultPedido) throw new ApplicationException("An error has occured.");

                unitOfWork.CommitTransaction();

            }
            catch (Exception error)
            {
                unitOfWork.RollbackTransaction();
                throw new Exception(error.Message);
            }

            return Task.FromResult(true);
        }

        Task INotificationHandler<OnAtualizarQtdeItens>.Handle(OnAtualizarQtdeItens request, CancellationToken cancellationToken)
        {
            //Todo: Remover try quando bus de eventos estiver assincrono.
            //Enquanto este evendo estiver vindo da thread sincronda na query de notificações, o try evita que a consulta seja travada com alguma exception.
            long qtde = 0, qtdePendenteCompra = 0, qtdePendenteEntrega = 0, qtdeCompra = 0, qtdeEntrega = 0, qtdeDevolvida = 0;
            try
            {

                var pedidoItem = pedidoItemRepository.GetById(request.PedidoItem.Id, cancellationToken).GetAwaiter().GetResult();
                if (pedidoItem == null)
                    throw new NotFoundException("id pedido item não encontrado!");

                var pedidoItemCompras = pedidoItemCompraRepository.ListByIdPedidoItemCompra(pedidoItem.Id, cancellationToken).GetAwaiter().GetResult();

                if (pedidoItemCompras.Count > 0)
                {
                    qtdeCompra = 0;
                    qtdeEntrega = 0;

                    foreach (var compra in pedidoItemCompras)
                    {
                        qtdeCompra += compra.Quantidade;

                    }
                }

                var pedidoItemEntregas = pedidoItemEntregaRepository.ListByIdPedidoItemEntrega(pedidoItem.Id, cancellationToken).GetAwaiter().GetResult();

                if (pedidoItemEntregas.Count > 0)
                {
                    foreach (var entrega in pedidoItemEntregas)
                    {
                        qtdeEntrega += entrega.Quantidade;
                    }
                }

                qtde = pedidoItem.Quantidade;
                qtdeDevolvida = pedidoItem.PedidoItemArte.QuantidadeDevolvida;
                qtdePendenteCompra = qtde - (qtdeCompra + qtdeDevolvida);
                qtdePendenteEntrega = qtdeCompra - qtdeEntrega;


                //unitOfWork.BeginTransaction();
                pedidoItem.Quantidade = qtde;
                pedidoItem.PedidoItemArte.QuantidadeComprada = qtdeCompra;
                pedidoItem.PedidoItemArte.QuantidadePendenteCompra = qtdePendenteCompra;
                pedidoItem.PedidoItemArte.QuantidadeDevolvida = qtdeDevolvida;
                pedidoItem.PedidoItemArte.QuantidadeEntregue = qtdeEntrega;
                pedidoItem.PedidoItemArte.QuantidadePendenteEntrega = qtdePendenteEntrega;

                pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);

                var resultPedidoItem = unitOfWork.SaveChanges();

                if (!resultPedidoItem) throw new ApplicationException("An error has occured.");

                // Entrega total
                if (pedidoItem.PedidoItemArte.QuantidadeEntregue ==
                    (pedidoItem.PedidoItemArte.QuantidadeComprada + pedidoItem.PedidoItemArte.QuantidadePendenteCompra + pedidoItem.PedidoItemArte.QuantidadeDevolvida)
                    && pedidoItem.PedidoItemArte.QuantidadeEntregue > 0)
                {
                    mediator.Send(new AddTrackingArte()
                    {
                        PedidoItemArteTracking = new PedidoItemArteTracking()
                        {
                            IdPedidoItemArte = pedidoItem.PedidoItemArte.Id,
                            StatusId = (int)PedidoItemArteStatus.ITEM_ENTREGUE,
                            ChangeById = userProvider.User.Login
                        }
                    }, cancellationToken);

                    mediator.Publish(new OnItemEntregue() { PedidoItem = pedidoItem });
                }

                //Entrega parcial
                if (pedidoItem.PedidoItemArte.QuantidadeEntregue <
                    (pedidoItem.PedidoItemArte.QuantidadeComprada + pedidoItem.PedidoItemArte.QuantidadePendenteCompra + pedidoItem.PedidoItemArte.QuantidadeDevolvida) &&
                    pedidoItem.PedidoItemArte.QuantidadeEntregue > 0)
                {
                    mediator.Send(new DeleteTrackingArte()
                    {
                        PedidoItemArteTracking = new PedidoItemArteTracking()
                        {
                            IdPedidoItemArte = pedidoItem.PedidoItemArte.Id,
                            StatusId = (int)PedidoItemArteStatus.ITEM_ENTREGUE,
                            ChangeById = userProvider.User?.Login
                        }
                    }, cancellationToken);

                    mediator.Send(new AddTrackingArte()
                    {
                        PedidoItemArteTracking = new PedidoItemArteTracking()
                        {
                            IdPedidoItemArte = pedidoItem.PedidoItemArte.Id,
                            StatusId = (int)PedidoItemArteStatus.ITEM_ENTREGUEPARCIALMENTE,
                            ChangeById = userProvider.User?.Login
                        }
                    }, cancellationToken);


                    mediator.Publish(new OnItemEntregue() { PedidoItem = pedidoItem });
                }

                if (pedidoItem.PedidoItemArte.QuantidadeEntregue == 0)
                {
                    mediator.Send(new DeleteTrackingArte()
                    {
                        PedidoItemArteTracking = new PedidoItemArteTracking()
                        {
                            IdPedidoItemArte = pedidoItem.PedidoItemArte.Id,
                            StatusId = (int)PedidoItemArteStatus.ITEM_ENTREGUE,
                            ChangeById = userProvider.User?.Login
                        }
                    }, cancellationToken);

                    mediator.Send(new DeleteTrackingArte()
                    {
                        PedidoItemArteTracking = new PedidoItemArteTracking()
                        {
                            IdPedidoItemArte = pedidoItem.PedidoItemArte.Id,
                            StatusId = (int)PedidoItemArteStatus.ITEM_ENTREGUEPARCIALMENTE,
                            ChangeById = userProvider.User?.Login
                        }
                    }, cancellationToken);
                }

                //unitOfWork.CommitTransaction();

            }
            catch (Exception error)
            {
                //unitOfWork.RollbackTransaction();
                throw new Exception(error.Message);
            }

            return Task.FromResult(true);
        }

        Task INotificationHandler<OnDataNecessidadeAlterada>.Handle(OnDataNecessidadeAlterada notification, CancellationToken cancellationToken)
        {
            bool finalTrans = false;
            //Todo: Remover try quando bus de eventos estiver assincrono.
            //Enquanto este evendo estiver vindo da thread sincronda na query de notificações, o try evita que a consulta seja travada com alguma exception.
            try
            {
                unitOfWork.BeginTransaction();
                //foreach (var pedidoItem in notification.Pedido.Itens.Where(x => x.PedidoItemArte.IdStatus != (int)Domain.Enums.PedidoItemArteStatus.ITEM_APROVADO))
                foreach (var pedidoItem in notification.Pedido.Itens)
                {
                    pedidoItem.DataNecessidade = notification.Pedido.PedidoArte.DataNecessidade;

                    pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);
                    //todo: remover persistência do EventHandle, persistir nos CommandHandlers
                    var result = unitOfWork.SaveChanges();
                }

                unitOfWork.CommitTransaction();

                finalTrans = true;

                mediator.Publish(new OnDataNecessidadeItemSemAcaEditada()
                {
                    Pedido = notification.Pedido
                });
            }
            catch (Exception error)
            {
                if (!finalTrans)
                    unitOfWork.RollbackTransaction();
                throw new Exception(error.Message);
            }

            return Task.FromResult(true);
        }

        Task INotificationHandler<OnDataReenvioAlterada>.Handle(OnDataReenvioAlterada notification, CancellationToken cancellationToken)
        {
            //Todo: Remover try quando bus de eventos estiver assincrono.
            //Enquanto este evendo estiver vindo da thread sincronda na query de notificações, o try evita que a consulta seja travada com alguma exception.

            try
            {
                unitOfWork.BeginTransaction();

                var status = statusPedidoItemRepository.GetById((int)PedidoItemArteStatus.ITEM_REENVIO, cancellationToken).GetAwaiter().GetResult();

                foreach (var pedidoItem in notification.Pedido.Itens.Where(x => x.PedidoItemArte.IdStatus == (int)PedidoItemArteStatus.ITEM_DEVOLUCAO))
                {
                    pedidoItem.PedidoItemArte.DataReenvio = notification.Pedido.PedidoArte.DataReenvio;

                    pedidoItem.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_DEVOLUCAO;

                    pedidoItem.PedidoItemArte.Status = status;

                    //todo: remover persistência do EventHandle, persistir nos CommandHandlers
                    pedidoItemArteRepository.AddOrUpdate(pedidoItem.PedidoItemArte, cancellationToken);

                    mediator.Publish(new OnReenvioItensDevolvidos()
                    {
                        Pedido = pedidoItem.Pedido,
                        PedidoItem = pedidoItem
                    });

                    var result = unitOfWork.SaveChanges();
                }

                unitOfWork.CommitTransaction();


            }
            catch (Exception error)
            {
                unitOfWork.RollbackTransaction();
                throw new Exception(error.Message);
            }

            return Task.FromResult(true);
        }

        Task INotificationHandler<OnDevolucaoPedido>.Handle(OnDevolucaoPedido request, CancellationToken cancellationToken)
        {
            try
            {
                unitOfWork.BeginTransaction();

                if (string.IsNullOrWhiteSpace(request.Pedido.JustificativaDevolucao))
                    throw new BadRequestException("Justificativa de devolução é obrigatória.");


                /*Recupera status de cancelamento*/
                //foreach (var pedidoItem in request.Pedido.Itens)
                foreach (var pedidoItem in request.Pedido.Itens)
                {
                    var requisicaoCompra = pedidoItem.RCs.FirstOrDefault();

                    if (requisicaoCompra != null && string.IsNullOrWhiteSpace(requisicaoCompra.Acordo))
                    {

                        /****************************************************************************************************/
                        if (userProvider.User.Login == pedidoItem.Pedido.PedidoArte.BaseLogin)
                        {
                            if (pedidoItem.PedidoItemArte.FlagDevolvidoBase)
                                throw new BadRequestException("Item já devolvido pela base.");

                            pedidoItem.PedidoItemArte.FlagDevolvidoBase = true;

                            mediator.Publish(new OnDevolucaoItensBase()
                            {
                                PedidoItem = pedidoItem
                            });

                        }
                        else if (userProvider.User.Login == pedidoItem.PedidoItemArte.CompradoPorLogin)
                        {
                            if (pedidoItem.PedidoItemArte.FlagDevolvidoComprador)
                                throw new BadRequestException("Item já devolvido pelo comprador.");

                            pedidoItem.PedidoItemArte.FlagDevolvidoComprador = true;
                        }
                        else
                            throw new BadRequestException("Usuário não possui vinculo com o item para solicitar devolução.");
                        /****************************************************************************************************/

                        pedidoItem.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_DEVOLUCAO;
                        pedidoItem.DevolvidoPorLogin = userProvider.User.Login;
                        pedidoItem.DataDevolucao = DateTime.Now;

                        var statusItemPedido = statusPedidoItemRepository.GetById(pedidoItem.PedidoItemArte.IdStatus, cancellationToken).GetAwaiter().GetResult();
                        pedidoItem.PedidoItemArte.Status = statusItemPedido;

                        pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);
                        //todo: remover persistência do EventHandle, persistir nos CommandHadlers
                        var resultItem = unitOfWork.SaveChanges();

                        if (!resultItem) throw new ApplicationException("An error has occured.");

                        /*
        				 Verificar se o logincomprador está preenchido, caso sim, enviar notificação para o comprador, senão notificação para a base
        				 */

                        mediator.Send(new AddTrackingArte()
                        {
                            PedidoItemArteTracking = new PedidoItemArteTracking()
                            {
                                IdPedidoItemArte = pedidoItem.PedidoItemArte.Id,
                                StatusId = pedidoItem.PedidoItemArte.IdStatus,
                                ChangeById = pedidoItem.DevolvidoPorLogin
                            }
                        }, cancellationToken);
                    }
                }

                var pedido = pedidoRepository.GetByIdPedidoWithOutRoles(request.Pedido.Id, cancellationToken).GetAwaiter().GetResult();

                pedido.JustificativaDevolucao = request.Pedido.JustificativaDevolucao;

                pedidoRepository.AddOrUpdate(pedido, cancellationToken);

                var result = unitOfWork.SaveChanges();

                if (!result) throw new ApplicationException("An error has occured.");

                unitOfWork.CommitTransaction();
            }
            catch (Exception error)
            {
                unitOfWork.RollbackTransaction();
                throw new Exception(error.Message);
            }

            return Task.FromResult(true);

        }

        Task INotificationHandler<OnDevolucaoPedidoItem>.Handle(OnDevolucaoPedidoItem request, CancellationToken cancellationToken)
        {
            try
            {
                //unitOfWork.BeginTransaction();

                if (string.IsNullOrWhiteSpace(request.PedidoItem.JustificativaDevolucao))
                    throw new BadRequestException("Justificativa de devolução é obrigatória.");


                var requisicaoCompra = request.PedidoItem.RCs.FirstOrDefault();

                /*Recupera status de cancelamento*/
                //foreach (var pedidoItem in request.Pedido.Itens)
                if (requisicaoCompra != null && string.IsNullOrWhiteSpace(requisicaoCompra.Acordo))
                {
                    request.PedidoItem.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_DEVOLUCAO;
                    request.PedidoItem.DevolvidoPorLogin = userProvider.User.Login;
                    request.PedidoItem.DataDevolucao = DateTime.Now;

                    var statusItemPedido = statusPedidoItemRepository.GetById(request.PedidoItem.PedidoItemArte.IdStatus, cancellationToken).GetAwaiter().GetResult();
                    request.PedidoItem.PedidoItemArte.Status = statusItemPedido;

                    pedidoItemRepository.AddOrUpdate(request.PedidoItem, cancellationToken);
                    //todo: remover persistência do EventHandle, persistir nos CommandHandlers
                    var resultItem = unitOfWork.SaveChanges();

                    if (!resultItem) throw new ApplicationException("An error has occured.");

                    /*
                    Verificar se o logincomprador está preenchido, caso sim, enviar notificação para o comprador, senão notificação para a base
                    */

                    mediator.Send(new AddTrackingArte()
                    {
                        PedidoItemArteTracking = new PedidoItemArteTracking()
                        {
                            IdPedidoItemArte = request.PedidoItem.PedidoItemArte.Id,
                            StatusId = request.PedidoItem.PedidoItemArte.IdStatus,
                            ChangeById = request.PedidoItem.DevolvidoPorLogin
                        }
                    }, cancellationToken);

                }

                var pedido = pedidoRepository.GetByIdPedidoWithOutRoles(request.PedidoItem.IdPedido, cancellationToken).GetAwaiter().GetResult();

                pedido.JustificativaDevolucao = request.PedidoItem.JustificativaDevolucao;

                pedidoRepository.AddOrUpdate(pedido, cancellationToken);

                var result = unitOfWork.SaveChanges();

                if (!result) throw new ApplicationException("An error has occured.");

                mediator.Publish(new OnItemDevolvido() { Pedido = pedido });

            }
            catch (Exception error)
            {
                //unitOfWork.RollbackTransaction();
                throw new Exception(error.Message);
            }


            return Task.FromResult(true);

        }

        Task INotificationHandler<OnAprovacaoPedido>.Handle(OnAprovacaoPedido request, CancellationToken cancellationToken)
        {
            try
            {
                //if (userProvider.IsRole(Role.PERFIL_SUPRIMENTOS) && userProvider.IsRole(Role.GRANT_BASE_SUPRIMENTO))
                //{
                unitOfWork.BeginTransaction();

                /*Recupera status de atribuicao*/
                foreach (var pedidoItem in request.Pedido.Itens)
                {
                    if ((pedidoItem.PedidoItemArte.IdStatus != (int)PedidoItemArteStatus.ITEM_ATRIBUIDOAOCOMPRADOR)
                        && (pedidoItem.PedidoItemArte.IdStatus != (int)Domain.Enums.PedidoItemArteStatus.ITEM_CANCELAMENTOSOLICITADO))
                    {
                        throw new BadRequestException(string.Format("O status do pedido item {0} não atribuido ao comprador.", pedidoItem.Id));
                    }
                    else
                    {
                        pedidoItem.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_APROVADO;
                        pedidoItem.AprovadoPorLogin = userProvider.User.Login;
                        pedidoItem.DataAprovacao = DateTime.Now;

                        var statusItemPedido = statusPedidoItemRepository.GetById(pedidoItem.PedidoItemArte.IdStatus, cancellationToken).GetAwaiter().GetResult();
                        pedidoItem.PedidoItemArte.Status = statusItemPedido;
                        //todo: remover persistência do EventHandle, persistir nos CommandHandlers
                        pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);

                        var resultItem = unitOfWork.SaveChanges();

                        if (!resultItem) throw new ApplicationException("An error has occured.");

                        /*
        				Verificar se o logincomprador está preenchido, caso sim, enviar notificação para o comprador, senão notificação para a base
        				*/
                        mediator.Send(new AddTrackingArte()
                        {
                            PedidoItemArteTracking = new PedidoItemArteTracking()
                            {
                                IdPedidoItemArte = pedidoItem.PedidoItemArte.Id,
                                StatusId = pedidoItem.PedidoItemArte.IdStatus,
                                ChangeById = pedidoItem.AprovadoPorLogin
                            }
                        }, cancellationToken);

                        mediator.Publish(new OnItemAprovado() { PedidoItem = pedidoItem });

                    }
                }

                var pedido = pedidoRepository.GetByIdPedidoWithOutRoles(request.Pedido.Id, cancellationToken).GetAwaiter().GetResult();

                pedido.PedidoArte.IdStatus = (int)PedidoArteStatus.PEDIDO_FINALIZADO;

                var statusPedido = statusPedidoRepository.GetById(pedido.PedidoArte.IdStatus, cancellationToken).GetAwaiter().GetResult();
                pedido.PedidoArte.Status = statusPedido;

                pedidoRepository.AddOrUpdate(pedido, cancellationToken);

                var result = unitOfWork.SaveChanges();

                if (!result) throw new ApplicationException("An error has occured.");

                unitOfWork.CommitTransaction();
                //}
                //else
                //	throw new BadRequestException("Somente o perfil de Comprador pode aprovar um pedido.");

            }
            catch (Exception error)
            {
                unitOfWork.RollbackTransaction();
                throw new Exception(error.Message);
            }

            return Task.FromResult(true);

        }

        Task INotificationHandler<OnCancelamentoPedidoSolicitado>.Handle(OnCancelamentoPedidoSolicitado request, CancellationToken cancellationToken)
        {
            //Todo: Remover try quando bus de eventos estiver assincrono.
            //Enquanto este evendo estiver vindo da thread sincronda na query de notificações, o try evita que a consulta seja travada com alguma exception.

            try
            {
                unitOfWork.BeginTransaction();

                bool allCanceled = true, allrequestscanceled = true;

                if ((userProvider.User.Login == request.Pedido.CriadoPorLogin) &&
                    (request.Pedido.PedidoArte.IdStatus != (int)PedidoArteStatus.PEDIDO_ENVIADO) &&
                    (request.Pedido.PedidoArte.IdStatus != (int)PedidoArteStatus.PEDIDO_RASCUNHO))
                    if (string.IsNullOrWhiteSpace(request.Pedido.JustificativaCancelamento))
                        throw new BadRequestException("Justificativa de cancelamento é obrigatória.");

                /*Recupera status de cancelamento*/
                foreach (var pedidoItem in request.Pedido.Itens)
                {
                    switch (request.Pedido.PedidoArte.IdStatus)
                    {
                        case (int)PedidoArteStatus.PEDIDO_ENVIADO:
                            {
                                if (userProvider.User.Login == pedidoItem.Pedido.CriadoPorLogin)
                                {
                                    var requisicaoCompra = pedidoItem.RCs.FirstOrDefault();
                                    if (requisicaoCompra != null && string.IsNullOrWhiteSpace(requisicaoCompra.Acordo))
                                    {

                                        pedidoItem.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_CANCELADO;
                                        pedidoItem.CanceladoPorLogin = userProvider.User.Login;
                                        pedidoItem.DataCancelamento = DateTime.Now;

                                        allCanceled = false;
                                    }
                                    else
                                    {
                                        allCanceled = false;
                                        allrequestscanceled = false;
                                        pedidoItem.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_CANCELAMENTOSOLICITADO;
                                    }
                                }
                                break;
                            }
                        case (int)PedidoArteStatus.PEDIDO_RASCUNHO:
                            {
                                if (userProvider.User.Login == pedidoItem.Pedido.CriadoPorLogin)
                                {
                                    pedidoItem.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_CANCELADO;
                                    pedidoItem.CanceladoPorLogin = userProvider.User.Login;
                                    pedidoItem.DataCancelamento = DateTime.Now;

                                    allCanceled = false;
                                }

                                break;
                            }
                        case (int)PedidoArteStatus.PEDIDO_EMANDAMENTO:
                            {
                                if (pedidoItem.PedidoItemArte.IdStatus == (int)PedidoItemArteStatus.ITEM_CANCELAMENTOSOLICITADO)
                                {
                                    if (userProvider.User.Login == pedidoItem.Pedido.PedidoArte.BaseLogin) //verificar se comprador pode cancelar no pedido diretamente, regra somente aplicada para login base
                                    {
                                        pedidoItem.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_CANCELADO;
                                        pedidoItem.CanceladoPorLogin = userProvider.User.Login;
                                        pedidoItem.DataCancelamento = DateTime.Now;

                                        allCanceled = false;
                                    }
                                    else
                                    {
                                        pedidoItem.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_CANCELAMENTOSOLICITADO;
                                        allrequestscanceled = false;
                                    }
                                }
                                else
                                {
                                    if (userProvider.User.Login == pedidoItem.Pedido.CriadoPorLogin)
                                    {
                                        pedidoItem.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_CANCELAMENTOSOLICITADO;
                                        allrequestscanceled = false;
                                    }
                                }

                                break;
                            }
                        default:
                            {
                                pedidoItem.PedidoItemArte.IdStatus = (int)PedidoItemArteStatus.ITEM_CANCELAMENTOSOLICITADO;
                                allrequestscanceled = false;
                                break;
                            }
                    }

                    var statusItemPedido = statusPedidoItemRepository.GetById(pedidoItem.PedidoItemArte.IdStatus, cancellationToken).GetAwaiter().GetResult();
                    pedidoItem.PedidoItemArte.Status = statusItemPedido;
                    //todo: remover persistência do EventHandle, persistir nos CommandHandlers
                    pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);

                    var result = unitOfWork.SaveChanges();

                    if (!result) throw new ApplicationException("An error has occured.");

                    /*
        			 Verificar se o logincomprador está preenchido, caso sim, enviar notificação para o comprador, senão notificação para a base
        			 */

                    mediator.Send(new AddTrackingArte()
                    {
                        PedidoItemArteTracking = new PedidoItemArteTracking()
                        {
                            IdPedidoItemArte = pedidoItem.PedidoItemArte.Id,
                            StatusId = pedidoItem.PedidoItemArte.IdStatus,
                            ChangeById = pedidoItem.Pedido.CriadoPorLogin
                        }
                    }, cancellationToken);

                    if (pedidoItem.PedidoItemArte.IdStatus == (int)PedidoItemArteStatus.ITEM_CANCELAMENTOSOLICITADO)
                    {
                        mediator.Publish(new OnCancelamentoItemSolicitado() { PedidoItem = pedidoItem });
                    }

                }


                var pedido = pedidoRepository.GetByIdPedidoWithOutRoles(request.Pedido.Id, cancellationToken).GetAwaiter().GetResult();

                var statusPedido =
                    statusPedidoRepository.GetById((int)PedidoArteStatus.PEDIDO_EMANDAMENTO, cancellationToken).GetAwaiter().GetResult();

                if (allCanceled)
                {
                    pedido.PedidoArte.DataSolicitacaoCancelamento = DateTime.Now;
                }


                if (request.Pedido.PedidoArte.IdStatus == (int)PedidoArteStatus.PEDIDO_RASCUNHO ||
                    request.Pedido.PedidoArte.IdStatus == (int)PedidoArteStatus.PEDIDO_ENVIADO)
                {
                    if (userProvider.User.Login == pedido.CriadoPorLogin)
                    {
                        statusPedido = statusPedidoRepository.GetById((int)PedidoArteStatus.PEDIDO_CANCELADO, cancellationToken).GetAwaiter().GetResult();
                        pedido.CanceladoPorLogin = userProvider.User.Login;
                        pedido.DataCancelamento = DateTime.Now;
                        pedido.PedidoArte.DataSolicitacaoCancelamento = null;
                        pedido.PedidoArte.IdStatus = (int)PedidoArteStatus.PEDIDO_CANCELADO;
                    }
                }
                else
                {
                    if (userProvider.User.Login == pedido.PedidoArte.BaseLogin)
                    {
                        if (allrequestscanceled)
                        {
                            statusPedido = statusPedidoRepository.GetById((int)PedidoArteStatus.PEDIDO_CANCELADO, cancellationToken).GetAwaiter().GetResult();
                            pedido.CanceladoPorLogin = userProvider.User.Login;
                            pedido.DataCancelamento = DateTime.Now;
                            pedido.PedidoArte.DataSolicitacaoCancelamento = null;
                            pedido.PedidoArte.IdStatus = (int)PedidoArteStatus.PEDIDO_CANCELADO;
                        }
                        else
                            pedido.PedidoArte.IdStatus = (int)PedidoArteStatus.PEDIDO_EMANDAMENTO;
                    }
                    else
                        pedido.PedidoArte.IdStatus = (int)PedidoArteStatus.PEDIDO_EMANDAMENTO;
                }

                pedido.PedidoArte.Status = statusPedido;

                pedidoRepository.AddOrUpdate(pedido, cancellationToken);

                unitOfWork.SaveChanges();

                unitOfWork.CommitTransaction();
            }
            catch (Exception error)
            {
                unitOfWork.RollbackTransaction();
                throw new Exception(error.Message);
            }

            return Task.FromResult(true);
        }

        Task INotificationHandler<OnStatusPedidoArteAlterado>.Handle(OnStatusPedidoArteAlterado request, CancellationToken cancellationToken)
        {
            var existPedido = pedidoArteRepository.GetById(request.PedidoArte.Id, cancellationToken).GetAwaiter().GetResult();

            switch (existPedido.IdStatus)
            {
                case (int)PedidoArteStatus.PEDIDO_RASCUNHO:
                    {
                        if ((request.PedidoArte.IdStatus != (int)PedidoArteStatus.PEDIDO_ENVIADO) && (request.PedidoArte.IdStatus != (int)PedidoArteStatus.PEDIDO_CANCELADO))
                            throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                        break;
                    }
                case (int)PedidoArteStatus.PEDIDO_ENVIADO:
                    {
                        if ((request.PedidoArte.IdStatus == (int)PedidoArteStatus.PEDIDO_RASCUNHO) && (request.PedidoArte.IdStatus == (int)PedidoArteStatus.PEDIDO_FINALIZADO))
                            throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                        break;
                    }
                case (int)PedidoArteStatus.PEDIDO_EMANDAMENTO:
                    {
                        if ((request.PedidoArte.IdStatus == (int)PedidoArteStatus.PEDIDO_RASCUNHO) && (request.PedidoArte.IdStatus == (int)PedidoArteStatus.PEDIDO_ENVIADO))
                            throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                        break;
                    }
                case (int)PedidoArteStatus.PEDIDO_FINALIZADO:
                    {
                        if ((request.PedidoArte.IdStatus != (int)PedidoArteStatus.PEDIDO_CANCELADO))
                            throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                        break;
                    }
                case (int)PedidoArteStatus.PEDIDO_CANCELADO:
                    {
                        if ((request.PedidoArte.IdStatus != (int)PedidoArteStatus.PEDIDO_CANCELADO))
                            throw new ApplicationException("Mudança do status do pedido não permitido para o status atual");

                        break;
                    }
                default:
                    break;
            }

            existPedido.IdStatus = request.PedidoArte.IdStatus;

            var existStatus = statusPedidoRepository.GetById(request.PedidoArte.IdStatus, cancellationToken).GetAwaiter().GetResult();

            if (existStatus == null)
                throw new NotFoundException("Status não encontrado");

            existPedido.Status = existStatus;

            pedidoArteRepository.AddOrUpdate(existPedido, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("An error has occured.");

            return Task.FromResult(true);
        }


        Task INotificationHandler<OnCancelamentoItem>.Handle(OnCancelamentoItem notification, CancellationToken cancellationToken)
        {
            mediator.Publish(new OnCancelamentoAprovadoItem()
            {
                Pedido = notification.PedidoItem.Pedido
            });
            //EnviarEmailItemCancelado(notification.PedidoItem);
            return Task.FromResult(true);
        }

        Task INotificationHandler<OnItemReprovado>.Handle(OnItemReprovado notification, CancellationToken cancellationToken)
        {
            EnviarEmailItemReprovado(notification.PedidoItem);
            return Task.FromResult(true);
        }

        Task INotificationHandler<OnItemAprovado>.Handle(OnItemAprovado notification, CancellationToken cancellationToken)
        {
            EnviarEmailItemAprovado(notification.PedidoItem);
            return Task.FromResult(true);
        }

        Task INotificationHandler<OnItemEntregue>.Handle(OnItemEntregue notification, CancellationToken cancellationToken)
        {
            mediator.Publish(new OnItemEntregueParcialmente()
            {
                Pedido = notification.PedidoItem.Pedido
            });
            //EnviarEmailItemEntregue(notification.PedidoItem);
            return Task.FromResult(true);
        }

        Task INotificationHandler<OnNovoPedidoArte>.Handle(OnNovoPedidoArte notification, CancellationToken cancellationToken)
        {
            mediator.Publish(new OnPedidoEnviado()
            {
                Pedido = notification.Pedido
            });
            //EnviarEmailNovoPedido(notification.Pedido);
            return Task.FromResult(true);
        }

        Task INotificationHandler<OnPedidoFinalizado>.Handle(OnPedidoFinalizado notification, CancellationToken cancellationToken)
        {

            mediator.Publish(new OnItensResolvidosPedidoFinalizado()
            {
                Pedido = notification.Pedido
            });

            //EnviarEmailPedidoFinalizado(notification.Pedido);
            return Task.FromResult(true);
        }

        Task INotificationHandler<OnCancelamentoItemNegado>.Handle(OnCancelamentoItemNegado notification, CancellationToken cancellationToken)
        {
            mediator.Publish(new OnCancelamentoNegadoItem()
            {
                Pedido = notification.PedidoItem.Pedido
            });
            //EnviarEmailItemCancelamentoNegado(notification.PedidoItem);
            return Task.FromResult(true);
        }

        Task INotificationHandler<OnCancelamentoItemSolicitado>.Handle(OnCancelamentoItemSolicitado notification, CancellationToken cancellationToken)
        {
            //EnviarEmailItemCancelamentoSolicitado(notification.PedidoItem, cancellationToken);
            mediator.Publish(new OnRecebeCancelamentoItem()
            {
                Pedido = notification.PedidoItem.Pedido
            });

            return Task.FromResult(true);
        }

        Task INotificationHandler<OnItemAtribuidoComprador>.Handle(OnItemAtribuidoComprador notification, CancellationToken cancellationToken)
        {
            mediator.Publish(new OnPedidoRecebidoComprador()
            {
                Pedido = notification.Pedido
            });
            //EnviarEmailItemAtribuidoComprador(notification.Pedido);
            return Task.FromResult(true);
        }

        Task INotificationHandler<OnDLCancelationLine>.Handle(OnDLCancelationLine notification, CancellationToken cancellationToken)
        {
            var result = line.DeleteCancelLineAsync(notification.Rc.HeadId, cancellationToken).Result;

            return Unit.Task;
        }

        private static string SerializeEntityObject(object entityObject)
        {
            return JsonConvert.SerializeObject(entityObject, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="saida"></param>
        /// <param name="emailSender"></param>
        void EventException(Exception e)
        {
            var stack = string.Empty;
            var msg = e.Message;
            var inner = e.InnerException?.Message;

            try
            {
                stack = JsonConvert.SerializeObject(e, new JsonSerializerSettings()
                {
                    //ReferenceLoopHandling = ReferenceLoopHandling.Ignore 
                });
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            string body = string.Format(@"
                Mensagem: {0}
                <br/><br/>
                Inner: {1}
                <br/><br/>
                Stack:<br/>{2}", msg, inner, stack);


            emailSender.SendEmail(EmailErrorTo, null, null, "PIC Exception - Event", body, null);
        }

        #endregion

        #region Notificações novas

        Task INotificationHandler<OnCancelamentoAprovadoItem>.Handle(OnCancelamentoAprovadoItem notification, CancellationToken cancellationToken)
        {
            try
            {
                string emailTo = "";

                List<Assign> assignLst = new List<Assign>();

                string title = "Solicitação de Cancelamento atendida para Itens do Pedido {0} {1}.";

                title = string.Format(
                    title,
                    notification.Pedido.Id,
                    notification.Pedido.Titulo);


                var link = string.Format("/lista-pedidos/pedido/{0}/", notification.Pedido.Id);

                var equipe = equipeRepository.GetAll()
                    .Where(a => a.IdPedido == notification.Pedido.Id).Distinct().ToList();

                if (equipe != null && equipe.Count() > 0)
                {
                    foreach (var item in equipe.Select(x => x.Usuario.Login))
                    {
                        if (item != null)
                        {
                            Assign assignTo = new Assign();
                            assignTo.Login = item;
                            assignLst.Add(assignTo);
                        }
                    }

                    emailTo = equipe.Distinct().
                           Select(a => a.Usuario.Email).Aggregate((x, y) => x + ";" + y);//envia para a equipe;

                    notificationMethods.SendNotificationsByEmail(title, "TemplateDefaultNotification.html", emailTo, link, EmailPrefixSubject);
                }

                SaveNotificacoes(title, assignLst, link);
            }
            catch (Exception e) { EventException(e); }

            return Unit.Task;
        }

        Task INotificationHandler<OnCancelamentoNegadoItem>.Handle(OnCancelamentoNegadoItem notification, CancellationToken cancellationToken)
        {
            try
            {
                string emailTo = "";

                List<Assign> assignLst = new List<Assign>();

                string title = "Solicitação de Cancelamento negada para Itens do Pedido {0} {1}.";


                title = string.Format(
                    title,
                    notification.Pedido.Id,
                    notification.Pedido.Titulo);


                var link = string.Format("/lista-pedidos/pedido/{0}/", notification.Pedido.Id);

                var equipe = equipeRepository.GetAll()
                    .Where(a => a.IdPedido == notification.Pedido.Id).Distinct().ToList();

                if (equipe != null && equipe.Count() > 0)
                {
                    foreach (var item in equipe.Select(x => x.Usuario.Login))
                    {
                        if (item != null)
                        {
                            Assign assignTo = new Assign();
                            assignTo.Login = item;
                            assignLst.Add(assignTo);
                        }
                    }

                    emailTo = equipe.Distinct().
                           Select(a => a.Usuario.Email).Aggregate((x, y) => x + ";" + y);//envia para a equipe;

                    notificationMethods.SendNotificationsByEmail(title, "TemplateDefaultNotification.html", emailTo, link, EmailPrefixSubject);
                }

                SaveNotificacoes(title, assignLst, link);
            }
            catch (Exception e) { EventException(e); }

            return Unit.Task;
        }
        Task INotificationHandler<OnItemDevolvido>.Handle(OnItemDevolvido notification, CancellationToken cancellationToken)
        {
            try
            {
                string emailTo = "";

                List<Assign> assignLst = new List<Assign>();

                string title = "Itens devolvidos no Pedido {0} {1}.";

                title = string.Format(
                    title,
                    notification.Pedido.Id, notification.Pedido.Titulo);

                var link = string.Format("/lista-pedidos/pedido/{0}/", notification.Pedido.Id);

                var equipe = equipeRepository.GetAll()
                   .Where(a => a.IdPedido == notification.Pedido.Id).Distinct().ToList();

                if (equipe != null && equipe.Count() > 0)
                {
                    foreach (var item in equipe.Select(x => x.Usuario.Login))
                    {
                        if (item != null)
                        {
                            Assign assignTo = new Assign();
                            assignTo.Login = item;
                            assignLst.Add(assignTo);
                        }
                    }

                    emailTo = equipe.Distinct().
                           Select(a => a.Usuario.Email).Aggregate((x, y) => x + ";" + y);//envia para a equipe;
                }

                SaveNotificacoes(title, assignLst, link);

                notificationMethods.SendNotificationsByEmail(title, "TemplateDefaultNotification.html", emailTo, link, EmailPrefixSubject);
            }
            catch (Exception e) { EventException(e); }

            return Unit.Task;
        }

        Task INotificationHandler<OnItemEntregueParcialmente>.Handle(OnItemEntregueParcialmente notification, CancellationToken cancellationToken)
        {
            try
            {
                string emailTo = "";

                List<Assign> assignLst = new List<Assign>();

                string title = "Pedido {0} {1} parcialmente entregue pelo Comprador.";

                title = string.Format(
                    title,
                    notification.Pedido.Id,
                    notification.Pedido.Titulo);

                var link = string.Format("/lista-pedidos/pedido/{0}/", notification.Pedido.Id);

                var equipe = equipeRepository.GetAll()
                    .Where(a => a.IdPedido == notification.Pedido.Id).Distinct().ToList();

                if (equipe != null && equipe.Count() > 0)
                {
                    foreach (var item in equipe.Select(x => x.Usuario.Login))
                    {
                        if (item != null)
                        {
                            Assign assignTo = new Assign();
                            assignTo.Login = item;
                            assignLst.Add(assignTo);
                        }
                    }

                    emailTo = equipe.Distinct().
                           Select(a => a.Usuario.Email).Aggregate((x, y) => x + ";" + y);//envia para a equipe;

                    notificationMethods.SendNotificationsByEmail(title, "TemplateDefaultNotification.html", emailTo, link, EmailPrefixSubject);
                }

                SaveNotificacoes(title, assignLst, link);
            }
            catch (Exception e) { EventException(e); }

            return Unit.Task;
        }

        Task INotificationHandler<OnItensResolvidosPedidoFinalizado>.Handle(OnItensResolvidosPedidoFinalizado notification, CancellationToken cancellationToken)
        {
            try
            {
                string emailTo = "";

                List<Assign> assignLst = new List<Assign>();

                string title = "Itens de Produção | Pedido {0} {1} finalizado.";

                title = string.Format(
                    title,
                    notification.Pedido.Id,
                    notification.Pedido.Titulo);


                var link = string.Format("/lista-pedidos/pedido/{0}/", notification.Pedido.Id);

                var equipe = equipeRepository.GetAll()
                    .Where(a => a.IdPedido == notification.Pedido.Id).Distinct().ToList();


                if (equipe != null && equipe.Count() > 0)
                {
                    foreach (var item in equipe.Select(x => x.Usuario.Login))
                    {
                        if (item != null)
                        {
                            Assign assignTo = new Assign();
                            assignTo.Login = item;
                            assignLst.Add(assignTo);
                        }
                    }

                    emailTo = equipe.Distinct().
                           Select(a => a.Usuario.Email).Aggregate((x, y) => x + ";" + y);//envia para a equipe;

                    notificationMethods.SendNotificationsByEmail(title, "TemplateDefaultNotification.html", emailTo, link, EmailPrefixSubject);
                }

                if (notification.Pedido.CriadoPor != null)
                    emailTo = emailTo + notification.Pedido.CriadoPor.Email + ";";


                SaveNotificacoes(title, assignLst, link);
            }
            catch (Exception e) { EventException(e); }

            return Unit.Task;
        }

        Task INotificationHandler<OnPedidoEnviado>.Handle(OnPedidoEnviado notification, CancellationToken cancellationToken)
        {
            try
            {
                string emailTo = "";

                List<Assign> assignLst = new List<Assign>();

                string title = "Você foi marcado em um novo Pedido {0} {1}.";

                title = string.Format(
                    title,
                    notification.Pedido.Id,
                    notification.Pedido.Titulo);


                var link = string.Format("/lista-pedidos/pedido/{0}/", notification.Pedido.Id);

                var equipe = equipeRepository.GetAll()
                    .Where(a => a.IdPedido == notification.Pedido.Id).Distinct().ToList();

                if (equipe != null && equipe.Count() > 0)
                {
                    foreach (var item in equipe.Select(x => x.Usuario.Login))
                    {
                        if (item != null)
                        {
                            Assign assignTo = new Assign();
                            assignTo.Login = item;
                            assignLst.Add(assignTo);
                        }
                    }

                    emailTo = equipe.Distinct().
                           Select(a => a.Usuario.Email).Aggregate((x, y) => x + ";" + y);//envia para a equipe;

                    notificationMethods.SendNotificationsByEmail(title, "TemplateDefaultNotification.html", emailTo, link, EmailPrefixSubject);
                }

                SaveNotificacoes(title, assignLst, link);
            }
            catch (Exception e)
            {
                EventException(e);
            }

            return Unit.Task;
        }

        Task INotificationHandler<OnRCAcaReprovadoItem>.Handle(OnRCAcaReprovadoItem notification, CancellationToken cancellationToken)
        {
            try
            {
                string emailTo = "";

                List<Assign> assignLst = new List<Assign>();

                string title = "Solicitação de Compra reprovada para Itens no Pedido {0} {1}.";

                title = string.Format(
                    title,
                    notification.Pedido.Id,
                    notification.Pedido.Titulo);

                Assign assignTo = new Assign();
                assignTo.Login = notification.Pedido.CriadoPorLogin;
                assignLst.Add(assignTo);


                var link = string.Format("/lista-pedidos/pedido/{0}/", notification.Pedido.Id);

                var equipe = equipeRepository.GetAll()
                    .Where(a => a.IdPedido == notification.Pedido.Id).Distinct().ToList();

                if (equipe != null && equipe.Count() > 0)
                {
                    foreach (var item in equipe.Select(x => x.Usuario.Login))
                    {
                        if (item != null)
                        {
                            assignTo.Login = item;
                            assignLst.Add(assignTo);
                        }
                    }

                    emailTo = equipe.Distinct().
                           Select(a => a.Usuario.Email).Aggregate((x, y) => x + ";" + y);//envia para a equipe;

                    if (notification.Pedido.CriadoPor != null)
                        emailTo = emailTo + notification.Pedido.CriadoPor.Email + ";";

                    notificationMethods.SendNotificationsByEmail(title, "TemplateDefaultNotification.html", emailTo, link, EmailPrefixSubject);
                }

                SaveNotificacoes(title, assignLst, link);
            }
            catch (Exception e) { EventException(e); }

            return Unit.Task;
        }

        Task INotificationHandler<OnRCSemAcaReprovadaItem>.Handle(OnRCSemAcaReprovadaItem notification, CancellationToken cancellationToken)
        {
            try
            {
                string emailTo = "";

                List<Assign> assignLst = new List<Assign>();

                string title = "Solicitação de Compra reprovada para Itens no Pedido {0}.";

                title = string.Format(
                    title,
                    notification.Pedido.Titulo);

                Assign assignTo = new Assign();
                assignTo.Login = notification.Pedido.CriadoPorLogin;
                assignLst.Add(assignTo);

                var link = string.Format("/lista-pedidos/pedido/{0}/", notification.Pedido.Id);
                var equipe = equipeRepository.GetAll()
                    .Where(a => a.IdPedido == notification.Pedido.Id).Distinct().ToList();

                if (equipe != null && equipe.Count() > 0)
                {
                    foreach (var item in equipe.Select(x => x.Usuario.Login))
                    {
                        if (item != null)
                        {
                            assignTo.Login = item;
                            assignLst.Add(assignTo);
                        }
                    }

                    emailTo = equipe.Distinct().
                           Select(a => a.Usuario.Email).Aggregate((x, y) => x + ";" + y);//envia para a equipe;

                    if (notification.Pedido.CriadoPor != null)
                        emailTo = emailTo + notification.Pedido.CriadoPor.Email + ";";


                    notificationMethods.SendNotificationsByEmail(title, "TemplateDefaultNotification.html", emailTo, link, EmailPrefixSubject);
                }

                SaveNotificacoes(title, assignLst, link);

            }
            catch (Exception error)
            {
                EventException(error);
            }

            return Unit.Task;
        }

        Task INotificationHandler<OnBaseAtribuiItem>.Handle(OnBaseAtribuiItem notification, CancellationToken cancellationToken)
        {
            try
            {
                string emailTo = "";

                List<Assign> assignLst = new List<Assign>();

                string title = "Itens atribuídos a você no Pedido {0} {1}.";

                title = string.Format(
                    title,
                    notification.Pedido.Id,
                    notification.Pedido.Titulo);

                if (notification.Pedido.PedidoArte.Base != null)
                {
                    emailTo = emailTo + notification.Pedido.PedidoArte.Base.Email + ";";

                    var link = string.Format("/lista-pedidos/pedido/{0}/", notification.Pedido.Id);

                    notificationMethods.SendNotificationsByEmail(title, "TemplateDefaultNotification.html", emailTo, link, EmailPrefixSubject);
                }
            }
            catch (Exception e) { EventException(e); }

            return Unit.Task;
        }

        Task INotificationHandler<OnDataNecessidadeItemSemAcaEditada>.Handle(OnDataNecessidadeItemSemAcaEditada notification, CancellationToken cancellationToken)
        {
            try
            {
                string emailTo = "";

                List<Assign> assignLst = new List<Assign>();

                string title = "Data/Hora de Necessidade editadas em Itens do Pedido {0} {1}.";

                title = string.Format(
                    title,
                    notification.Pedido.Id,
                    notification.Pedido.Titulo);

                var compradores = notification.Pedido.Itens.Select(a => a.PedidoItemArte).ToList()
                    .Select(b => b.Comprador).Distinct().ToList();

                if (compradores.Count > 0)
                {
                    foreach (var comprador in compradores)
                    {
                        if (comprador != null)
                        {
                            assignLst.Add(new Assign() { Login = comprador.Login });
                            emailTo += comprador.Email + ";";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(notification.Pedido.PedidoArte.BaseLogin) && notification.Pedido.PedidoArte.Base != null)
                {
                    Assign assignTo = new Assign();
                    //base
                    assignTo.Login = notification.Pedido.PedidoArte.BaseLogin;
                    assignLst.Add(assignTo); ;

                    if (notification.Pedido.PedidoArte.Base != null)
                        emailTo = emailTo + notification.Pedido.PedidoArte.Base.Email + ";";
                }

                if (assignLst.Count > 0)
                {
                    var link = string.Format("/lista-pedidos/pedido/{0}/", notification.Pedido.Id);

                    SaveNotificacoes(title, assignLst, link);

                    notificationMethods.SendNotificationsByEmail(title, "TemplateDefaultNotification.html", emailTo, link, EmailPrefixSubject);
                }
            }
            catch (Exception e) { EventException(e); }

            return Unit.Task;
        }

        Task INotificationHandler<OnDevolucaoItensBase>.Handle(OnDevolucaoItensBase notification, CancellationToken cancellationToken)
        {
            try
            {
                string emailTo = "";

                List<Assign> assignLst = new List<Assign>();

                string title = "Itens devolvidos no Pedido {0} {1}.";

                title = string.Format(
                    title,
                    notification.Pedido.Id, notification.Pedido.Titulo);

                var link = string.Format("/lista-pedidos/pedido/{0}/", notification.Pedido.Id);

                var equipe = equipeRepository.GetAll()
                   .Where(a => a.IdPedido == notification.Pedido.Id).Distinct().ToList();

                if (equipe != null && equipe.Count() > 0)
                {
                    foreach (var item in equipe.Select(x => x.Usuario.Login))
                    {
                        if (item != null)
                        {
                            Assign assignTo = new Assign();
                            assignTo.Login = item;
                            assignLst.Add(assignTo);
                        }
                    }

                    emailTo = equipe.Distinct().
                           Select(a => a.Usuario.Email).Aggregate((x, y) => x + ";" + y);//envia para a equipe;
                }

                SaveNotificacoes(title, assignLst, link);

                notificationMethods.SendNotificationsByEmail(title, "TemplateDefaultNotification.html", emailTo, link, EmailPrefixSubject);
            }
            catch (Exception e) { EventException(e); }

            return Unit.Task;
        }

        Task INotificationHandler<OnPedidoRecebidoComprador>.Handle(OnPedidoRecebidoComprador notification, CancellationToken cancellationToken)
        {
            try
            {
                string emailTo = "";

                List<Assign> assignLst = new List<Assign>();

                string title = "Novo pedido recebido.";

                var compradores = notification.Pedido.Itens.Select(a => a.PedidoItemArte).ToList()
                    .Select(b => b.Comprador).Distinct().ToList();

                if (compradores.Count > 0)
                {
                    foreach (var comprador in compradores)
                    {
                        if (comprador != null)
                        {
                            assignLst.Add(new Assign() { Login = comprador.Login });
                            emailTo += comprador.Email + ";";
                        }
                    }
                }


                var link = string.Format("/lista-pedidos/pedido/{0}/", notification.Pedido.Id);
                if (assignLst.Count > 0)
                {
                    SaveNotificacoes(title, assignLst, link);

                    notificationMethods.SendNotificationsByEmail(title, "TemplateDefaultNotification.html", emailTo, link, EmailPrefixSubject);
                }
            }
            catch (Exception e) { EventException(e); }

            return Unit.Task;
        }

        Task INotificationHandler<OnRecebeCancelamentoItem>.Handle(OnRecebeCancelamentoItem notification, CancellationToken cancellationToken)
        {
            try
            {
                string emailTo = "";

                List<Assign> assignLst = new List<Assign>();

                string title = "Solicitação de Cancelamento de Itens no Pedido {0} {1}.";

                title = string.Format(
                    title,
                    notification.Pedido.Id,
                    notification.Pedido.Titulo);

                var compradores = notification.Pedido.Itens.Select(a => a.PedidoItemArte).ToList()
                   .Select(b => b.Comprador).Distinct().ToList();

                if (compradores.Count > 0)
                {
                    foreach (var comprador in compradores)
                    {
                        if (comprador != null)
                        {
                            assignLst.Add(new Assign() { Login = comprador.Login });
                            emailTo += comprador.Email + ";";
                        }
                    }
                }

                if (!String.IsNullOrEmpty(notification.Pedido.PedidoArte.BaseLogin) && notification.Pedido.PedidoArte.Base != null)
                {
                    Assign assignTo = new Assign();
                    //base
                    assignTo.Login = notification.Pedido.PedidoArte.BaseLogin;
                    assignLst.Add(assignTo); ;

                    if (notification.Pedido.PedidoArte.Base != null)
                        emailTo = emailTo + notification.Pedido.PedidoArte.Base.Email + ";";
                }

                if (assignLst.Count > 0)
                {
                    var link = string.Format("/lista-pedidos/pedido/{0}/", notification.Pedido.Id);

                    SaveNotificacoes(title, assignLst, link);

                    notificationMethods.SendNotificationsByEmail(title, "TemplateDefaultNotification.html", emailTo, link, EmailPrefixSubject);
                }
            }
            catch (Exception e) { EventException(e); }

            return Unit.Task;
        }

        Task INotificationHandler<OnReenvioItensDevolvidos>.Handle(OnReenvioItensDevolvidos notification, CancellationToken cancellationToken)
        {
            try
            {
                string emailTo = "";
                List<Assign> assignLst = new List<Assign>();
                string title = "Itens reenviados no Pedido {0} {1}.";
                title = string.Format(
                    title,
                    notification.Pedido.Id,
                    notification.Pedido.Titulo);

                var compradores = notification.Pedido.Itens.Select(a => a.PedidoItemArte).ToList()
                    .Select(b => b.Comprador).Distinct().ToList();

                if (compradores.Count > 0)
                {
                    foreach (var comprador in compradores)
                    {
                        if (comprador != null)
                        {
                            assignLst.Add(new Assign() { Login = comprador.Login });
                            emailTo += comprador.Email + ";";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(notification.Pedido.PedidoArte.BaseLogin) && notification.Pedido.PedidoArte.Base != null)
                {
                    Assign assignTo = new Assign();
                    //base
                    assignTo.Login = notification.Pedido.PedidoArte.BaseLogin;
                    assignLst.Add(assignTo); ;

                    if (notification.Pedido.PedidoArte.Base != null)
                        emailTo = emailTo + notification.Pedido.PedidoArte.Base.Email + ";";
                }
                if (assignLst.Count > 0)
                {
                    var link = string.Format("/lista-pedidos/pedido/{0}/", notification.Pedido.Id);
                    SaveNotificacoes(title, assignLst, link);
                    notificationMethods.SendNotificationsByEmail(title, "TemplateDefaultNotification.html", emailTo, link, EmailPrefixSubject);
                }
            }
            catch (Exception e)
            {
                EventException(e);
            }

            return Unit.Task;
        }

        #endregion

        public void SaveNotificacoes(string title, List<Assign> assigns)
        {
            Notificacao notificacao = new Notificacao();

            notificacao.Title = string.Format(title);
            notificacao.CreatedAt = DateTime.Now;

            notificacao.Link = "";

            notificacao.Assigns = assigns;

            mediator.Send(new SaveNotificacao()
            {
                Notificacao = notificacao
            });
        }

        public void SaveNotificacoes(string title, List<Assign> assigns, string link)
        {
            Notificacao notificacao = new Notificacao();

            notificacao.Title = title;
            notificacao.CreatedAt = DateTime.Now;

            notificacao.Link = link;

            notificacao.Assigns = assigns;

            mediator.Send(new SaveNotificacao()
            {
                Notificacao = notificacao
            });
        }
    }


    #region comentadas
    //Task INotificationHandler<OnVerificarStatusHF>.Handle(OnVerificarStatusHF request, CancellationToken cancellationToken)
    //{
    //    //Todo: Fazer evento de verificar status do pedido que será chamado pelo HangFire
    //    try
    //    {
    //        //
    //        bool entrou = true;
    //    }
    //    catch (Exception error)
    //    {
    //        throw new Exception(error.Message);
    //    }

    //    return Task.FromResult(true);
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="notification"></param>
    ///// <param name="cancellationToken"></param>
    ///// <returns></returns>
    //Task INotificationHandler<OnNotificacaoVista>.Handle(OnNotificacaoVista notification, CancellationToken cancellationToken)
    //{
    //	//Todo: Remover try quando bus de eventos estiver assincrono.
    //	//Enquanto este evendo estiver vindo da thread sincronda na query de notificações, o try evita que a consulta seja travada com alguma exception.

    //	try
    //	{

    //		var toSet = notificationRepository.GetAll().Where(n =>
    //			notification.NotificacaoIds.Contains(n.Id) &&
    //			!n.Viewers.Where(r => userProvider.User.Login.Equals(r.Login)).Any()
    //		);

    //		foreach (var item in toSet.ToList())
    //			mediator.Send(new SaveViewer()
    //			{
    //				Viewer = new Viewer()
    //				{
    //					Login = userProvider.User.Login,
    //					NotificationId = item.Id
    //				}
    //			});
    //	}
    //	catch (Exception error)
    //	{
    //		throw new Exception(error.Message);
    //	}

    //	return Task.FromResult(notification);
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="notification"></param>
    ///// <param name="cancellationToken"></param>
    ///// <returns></returns>
    //Task INotificationHandler<OnLogon>.Handle(OnLogon onLogon, CancellationToken cancellationToken)
    //{
    //	DateTime agora = DateTime.Now.AddHours(-3);
    //	CultureInfo idioma = new CultureInfo("pt-BR");

    //	string title =
    //		string.Format(
    //			string.Format(
    //				"Novo acesso em {0}",
    //				agora.ToString("f", idioma)
    //			));

    //	//Notificacao notificacao = new Notificacao();

    //	//notificacao.Title = string.Format(string.Format("Novo acesso em {0}", DateTime.Now.ToLongDateString()));
    //	//notificacao.CreatedAt = DateTime.Now;
    //	//notificacao.Link = "/";

    //	//notificacao.Assigns = new List<Assign>() { new Assign(){
    //	//	Login = userProvider.User.Login
    //	//}};

    //	//var envio = mediator.Send(new SaveNotificacao()
    //	//{
    //	//	Notificacao = notificacao
    //	//});

    //	//envio.Wait();

    //	SendNotificationsByEmail(title);

    //	return Task.FromResult(onLogon);
    //}

    //private void SendNotificationsByEmail(string title)
    //{
    //	//var defaultEmailVM = new DefaultNotificationViewModel();

    //	//defaultEmailVM.Subject = string.Format("{0}{1}", emailSettings.SubjectPrefixToNotification, title);

    //	//var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Templates", "Email");
    //	//var htmlTemplate = System.IO.File.ReadAllText(Path.Combine(templatePath, "TemplateDefaultNotification.html"));

    //	//htmlTemplate = htmlTemplate.Replace("{{TEXTO}}", title);
    //	//htmlTemplate = htmlTemplate.Replace("{{LINK_ACTION}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"));
    //	//htmlTemplate = htmlTemplate.Replace("{{LINK_TEXT}}", "Ir para o site");

    //	//defaultEmailVM.Body = htmlTemplate;

    //	//defaultEmailVM.ToEmails.Add(userProvider.User.Email);

    //	//foreach (var email in defaultEmailVM.ToEmails)
    //	//{
    //	//	emailSender.SendEmail(email, null, null, defaultEmailVM.Subject, defaultEmailVM.Body, null);
    //	//}
    //}

    //private void SaveNotificacoes(string title, List<Assign> assigns)
    //{
    //	Notificacao notificacao = new Notificacao();

    //	notificacao.Title = string.Format(title);
    //	notificacao.CreatedAt = DateTime.Now;

    //	notificacao.Link = "";

    //	notificacao.Assigns = assigns;
    //	mediator.Send(new SaveNotificacao()
    //	{
    //		Notificacao = notificacao
    //	});
    //}


    //private static string SerializeEntityObject(object entityObject)
    //{
    //	return JsonConvert.SerializeObject(entityObject, Formatting.Indented,
    //	new JsonSerializerSettings()
    //	{
    //		ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //	});
    //}
    //async Task INotificationHandler<OnEnviarPedidoOracle>.Handle(OnEnviarPedidoOracle notification, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        var pedido = notification.Pedido;

    //        var itens = pedido.Itens.ToList();
    //        if (itens.Count() > 0)
    //        { 
    //            ///a arte vai ter apenas uma RC . então firstordefault
    //            foreach (var item in itens.Where(a=>a.RCs.Count()>0 ))
    //            {
    //                PurchaseRequisition purchaseRequisition = new PurchaseRequisition();
    //                var requisicaoCompra = item.RCs.FirstOrDefault();
    //                purchaseRequisition.ActiveRequisitionFlag = false ;
    //                purchaseRequisition.AdditionalInformation= "Solicitada por Portal de Intenção de Compras";
    //                purchaseRequisition.AuxiliaryAddress="";
    //                purchaseRequisition.FunctionalCurrencyCode= "BRL";
    //                purchaseRequisition.ContractNumber="";
    //                purchaseRequisition.ExternallyManagedFlag=false;
    //                purchaseRequisition.InterfaceSourceCode = "PIC";
    //                purchaseRequisition.PreparerEmail= "roberto.c.campello@accenture.com";
    //                purchaseRequisition.RequisitionBusinessUnit=userProvider.User.UnidadeNegocio.Codigo;
    //                purchaseRequisition.Type= "Req.Compra";

    //                if (item.DataNecessidade.Value.Date < DateTime.Now.Date)
    //                    throw new BadRequestException("Data necessidade está fora do período!");

    //                List<AccountingSegmentRequisition> accountingSegments = new List<AccountingSegmentRequisition>();
    //                accountingSegments.Add(new AccountingSegmentRequisition {   
    //                    Type = "centroDeCusto", 
    //                    Value = pedido.CentroCusto
    //                });

    //                accountingSegments.Add(new AccountingSegmentRequisition { 
    //                    Type = "finalidade", 
    //                    Value = pedido.Finalidade});

    //                List<Distribution> distributions = new List<Distribution>();
    //                var expenditures = await expenditureProxy.GetExpenditures(requisicaoCompra.Categoria, cancellationToken);
    //                var organization = expenditures.Where(a => a.OrganizationName == "Supplier Invoices").FirstOrDefault();

    //                distributions.Add(new Distribution { 
    //                    AccountingSegments = accountingSegments,
    //                    Expenditure = new Expenditure
    //                    {
    //                        Date=pedido.PedidoArte.DataNecessidade.Value.ToString("yyyy-MM-dd"),
    //                        OrganizationName= "300000069575707", //hard code (representa a Globo)
    //                        TypeName= organization.Id.ToString()
    //                    },
    //                    Number=1,
    //                    ProjectNumber=pedido.IdProjeto.ToString(),
    //                    Quantity=item.Quantidade,
    //                    TaskNumber=pedido.IdTarefa.ToString()
    //                });
    //                Line line = new Line();
    //                line.CurrencyCode = "BRL";
    //                line.DeliverToLocationCode = item.Pedido.DeliverToLocationCode;
    //                line.DestinationTypeCode = "EXPENSE";
    //                line.Item = requisicaoCompra.ItemCodigo;
    //                line.Number = 1;
    //                line.Quantity = item.Quantidade;
    //                line.UnitPrice = item.Valor;
    //                line.RequestedDeliveryDate = pedido.PedidoArte.DataNecessidade.Value.ToString("yyyy-MM-dd");
    //                line.RequesterEmail = pedido.CriadoPorLogin;
    //                line.TypeCode = "Goods";
    //                line.DestinationOrganizationCode = pedido.DestinationOrganizationCode;
    //                line.NegotiatedByPreparerFlag = true;

    //                if (!string.IsNullOrWhiteSpace(requisicaoCompra.Acordo))
    //                {
    //                    line.AgreementId = requisicaoCompra.AcordoId;
    //                    line.AgreementLineId = requisicaoCompra.AcordoLinhaId;
    //                }

    //                purchaseRequisition.Lines.Add(line);

    //                var retorno= await purchaseRequisitionProxy.PostPurchaseRequisitionAsync(purchaseRequisition, cancellationToken);


    //            }
    //        }
    //    }
    //    catch (Exception error)
    //    {
    //        throw new Exception(error.Message);
    //    }
    //}

    //async Task INotificationHandler<OnEnviarPedidoOracle>.Handle(OnEnviarPedidoOracle notification, CancellationToken cancellationToken)
    //{
    //	try
    //	{
    //		var pedido = notification.Pedido;
    //		var itens = pedido.Itens.Where(a => !string.IsNullOrWhiteSpace(a.Acordo)
    //			&& !string.IsNullOrWhiteSpace(a.IdItemOracle)
    //			&& string.IsNullOrWhiteSpace(a.RcPedido));

    //		if (itens.Count() > 0)
    //		{
    //			//Todo: Adicionar as variáveis de ambiente quando obtiver permissão para criar TaskDefinition e ParameterStore.#

    //			//var requisitioningBUIdEnvironmentVariable = Environment.GetEnvironmentVariable("OIC_REQUISITIONINGBUID");
    //			//long requisitioningBUId = long.Parse(requisitioningBUIdEnvironmentVariable);
    //			//var DestinationOrganizationIdEnvironmentVariable = Environment.GetEnvironmentVariable("OIC_DESTINATIONORGANIZATIONID");
    //			//long DestinationOrganizationId = long.Parse(DestinationOrganizationIdEnvironmentVariable);
    //			//var deliverToLocationCode = Environment.GetEnvironmentVariable("OIC_DELIVERTOLOCATIONCODE");
    //			//var requesterEmail = Environment.GetEnvironmentVariable("OIC_REQUESTEREMAIL");
    //			//var finalidade = Environment.GetEnvironmentVariable("OIC_FINALIDADE");
    //			//var preparerEmail = Environment.GetEnvironmentVariable("OIC_PREPAREREMAIL");
    //			long requisitioningBUId = 300000047455934;
    //			long DestinationOrganizationId = 300000047526566;
    //			var deliverToLocationCode = "SEDE - GCP ORC";
    //			var requesterEmail = "juliana.dos.reis@accenture.com";
    //			var finalidade = "F0000001";
    //			var preparerEmail = "pic@g.globo";

    //			if (string.IsNullOrEmpty(requesterEmail))
    //			{
    //				var userSolicitante = await userRepository.GetByLogin(pedido.LoginSolicitante, cancellationToken);
    //				requesterEmail = CriadoPor.Email;
    //			}


    //			var bodyHeader = SerializeEntityObject(new
    //			{
    //				RequisitioningBUId = requisitioningBUId,
    //				PreparerEmail = preparerEmail,
    //				Description = "Item de Material de Escritório",
    //				ExternallyManagedFlag = false
    //			});

    //			foreach (var item in itens)
    //			{
    //				var returnHeader = await purchaseRequisitionProxy.PostRequisitionHeaderAsync(bodyHeader, cancellationToken);
    //				string requisitionHeaderId = returnHeader.RequisitionHeaderId.ToString();
    //				//contador++;  
    //				if (item.DataNecessidade.Value.Date < DateTime.Now.Date)
    //					throw new BadRequestException("Data necessidade está fora do período!");

    //				long idItemOracle = 0;
    //				if (item.IdItemOracle != string.Empty)
    //					idItemOracle = long.Parse(item.IdItemOracle);
    //				else
    //					throw new BadRequestException("item sem id item oracle");

    //				var bodyLine = SerializeEntityObject(new
    //				{
    //					LineNumber = 1,
    //					LineTypeId = 1,
    //					LineTypeCode = "Goods",
    //					CurrencyCode = "BRL",
    //					DestinationTypeCode = "EXPENSE",
    //					UOM = "UN",
    //					DestinationType = "Expense",
    //					//RequesterEmail = "juliana.dos.reis@accenture.com",
    //					RequesterEmail = requesterEmail,
    //					RequestedDeliveryDate = item.DataNecessidade.Value.Date.ToString("yyyy-MM-dd"),
    //					DestinationOrganizationId = DestinationOrganizationId,//hard coded
    //					Quantity = item.Quantidade,
    //					Price = item.Valor,

    //					DeliverToLocationCode = deliverToLocationCode,
    //					ItemId = idItemOracle
    //				}); ;

    //				var returnLine = await purchaseRequisitionProxy.PostRequisitionLineAsync(requisitionHeaderId, bodyLine, cancellationToken);

    //				if (returnLine != null)
    //				{
    //					//3 - distributions
    //					string requisitionLineId = returnLine.RequisitionLineId.ToString();
    //					DistributionDFFClass distributionDFFClass = new DistributionDFFClass();

    //					distributionDFFClass.finalidade = finalidade;

    //					var bodyDistribution = SerializeEntityObject(new
    //					{
    //						DistributionNumber = 1,//contador,// é o mesmo que LineNumber=1
    //						Quantity = item.Quantidade,//é o mesmo que quantidade de itens.
    //						DFF = new[] { distributionDFFClass }
    //					});

    //					var returnDistribution = await purchaseRequisitionProxy.PostRequisitionDistributionWithLineAsync(requisitionHeaderId, requisitionLineId, bodyDistribution, cancellationToken);

    //					var bodySubmitRequisition = SerializeEntityObject(new
    //					{
    //						name = "submitRequisition",
    //						parameters = new string[] { }
    //					});

    //					var returnDistributionItem = await purchaseRequisitionProxy.PostRequisitionAsync(requisitionHeaderId, bodySubmitRequisition, cancellationToken);

    //					if (returnDistributionItem.Result == "SUCCESS")
    //					{
    //						item.RcPedido = returnHeader.Requisition;
    //						pedidoItemRepository.AddOrUpdate(item, cancellationToken);
    //						var result = unitOfWork.SaveChanges();
    //					}

    //				}
    //			}
    //		}
    //	}
    //	catch (Exception error)
    //	{
    //		throw new Exception(error.Message);
    //	}
    //}
    #endregion
}