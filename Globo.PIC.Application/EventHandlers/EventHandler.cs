using System;
using System.Linq;
using Globo.PIC.Domain.Entities;
using MediatR;
using System.Threading.Tasks;
using Globo.PIC.Domain.Types.Events;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Types.Commands;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;
using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Exceptions;
using Globo.PIC.Domain.Enums;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Globo.PIC.Domain.EventHandlers
{
	/// <summary>
	///
	/// </summary>
	public class EventHandler
		:
		INotificationHandler<OnNotificacaoVista>,
		INotificationHandler<OnLogon>,
		INotificationHandler<OnVerificarStatusHF>,
		INotificationHandler<OnAtualizarQtdeItens>,
        INotificationHandler<OnEnviarPedidoOracle>,
		INotificationHandler<OnNovoPedido>
	{
		 
		/// <summary>
		/// 
		/// </summary>
		private readonly IUnitOfWork unitOfWork;

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
		private readonly IRepository<Pedido> pedidoRepository;

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
		private readonly IRepository<StatusArte> statusPedidoRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<User> userRepository;

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
		private readonly IMediator mediator;

		private readonly IPurchaseRequisitionProxy proxyOIC;

		public EventHandler(IPurchaseRequisitionProxy _proxyOIC)
		{
			_proxyOIC = proxyOIC;
		}
		private readonly IPurchaseRequisitionProxy purchaseRequisitionProxy;
		/// <summary>
		///
		/// </summary>
		public EmailSettings emailSettings { get; }

		public EventHandler(
			IUnitOfWork _unitOfWork,
			IRepository<Notificacao> _notificationRepository,
			IRepository<User> _userRepository,
			IRepository<StatusArte> _statusPedidoRepository,
			IRepository<Pedido> _pedidoRepository,
			IUserProvider _userProvider,
			IMediator _mediator,
			IEmailSender _emailSender,
			EmailSettings _emailSettings,
			IPurchaseRequisitionProxy _purchaseRequisitionProxy)
		{
			unitOfWork = _unitOfWork;
			notificationRepository = _notificationRepository;
			userRepository = _userRepository;
			statusPedidoRepository = _statusPedidoRepository;
			pedidoRepository = _pedidoRepository;
			userProvider = _userProvider;
			mediator = _mediator;
			emailSettings = _emailSettings;
			purchaseRequisitionProxy = _purchaseRequisitionProxy;
			emailSender = _emailSender; 
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

			if (pedido.Itens.Where(a=>a.PedidoItemArte.FirstOrDefault().IdStatus==(int)StatusItem.ITEM_EMANALISE).Count()>0)
            {
				emailTo = pedido.PedidoArte.FirstOrDefault().UserBase.Email + ";";
				emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
			}

			if (pedido.Itens.Where(a => a.PedidoItemArte.FirstOrDefault().IdStatus == (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR
			|| a.PedidoItemArte.FirstOrDefault().IdStatus == (int)StatusItem.ITEM_SOLICITACAODECOMPRAENVIADA
			|| a.PedidoItemArte.FirstOrDefault().IdStatus == (int)StatusItem.ITEM_APROVADO).Count() > 0)
			{
				emailTo = pedido.PedidoArte.FirstOrDefault().UserSolicitante.Email + ";"; 
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

			if (pedidoItem.PedidoItemArte.FirstOrDefault().FlagDevolvidoBase)
			{
				//email para o demandante
				emailTo = pedidoItem.Pedido.PedidoArte.FirstOrDefault().UserBase.Email + ";";
				emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
			}
			else if (pedidoItem.PedidoItemArte.FirstOrDefault().FlagDevolvidoComprador)
			{
				//email para a base
				emailTo = pedidoItem.PedidoItemArte.FirstOrDefault().UserComprador.Email + ";";
				emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
			} 
			 
		}
		/// <summary>
		/// envia email para demandante, base, comprador e equipe 
		/// </summary>
		/// <param name="pedidoItem"></param>
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
			emailTo = pedidoItem.Pedido.PedidoArte.FirstOrDefault().UserSolicitante.Email + ";";
			emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);

			//email para a equipe
			if (pedidoItem.Pedido.Equipe.Count() > 0)
			{
				emailTo = pedidoItem.Pedido.Equipe.Distinct().Select(a => a.User.Email).Aggregate((x, y) => x + ";" + y);//envia para a equipe;
				emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
			}
			 
		}
		/// <summary>
		/// envia email para demandante, base, comprador e equipe 
		/// </summary>
		/// <param name="pedidoItem"></param>
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

			if (pedidoItem.PedidoItemArte.FirstOrDefault().FlagDevolvidoBase)
			{
				//email para o demandante
				emailTo = pedidoItem.Pedido.PedidoArte.FirstOrDefault().UserSolicitante.Email + ";";
				emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
			}
			else if (pedidoItem.PedidoItemArte.FirstOrDefault().FlagDevolvidoComprador)
            {
				//email para a base
				emailTo = pedidoItem.Pedido.PedidoArte.FirstOrDefault().UserBase.Email + ";";
				emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
			} 
		}

		/// <summary>
		/// envia email apenas para o demandante
		/// </summary>
		/// <param name="pedido"></param>
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

			emailTo = pedidoItem.Pedido.PedidoArte.FirstOrDefault().UserSolicitante.Email + ";";
			emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
		}

		/// <summary>
		/// envia email apenas para o demandante
		/// </summary>
		/// <param name="pedido"></param>
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

			emailTo = pedidoItem.Pedido.PedidoArte.FirstOrDefault().UserSolicitante.Email + ";";

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

			emailTo = pedidoItem.Pedido.PedidoArte.FirstOrDefault().UserSolicitante.Email + ";";

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
			emailTo = pedido.PedidoArte.FirstOrDefault().UserSolicitante.Email + ";";
			emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);

			//email para a equipe tudo junto
			if (pedido.Equipe.Count() > 0)
			{
				emailTo = pedido.Equipe.Distinct().Select(a => a.User.Email).Aggregate((x, y) => x + ";" + y);//envia para a equipe;
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
					user=> new User
                    {
						Email = user.Email,
						Name = user.Name,
						LastName = user.LastName,
						Login = user.Login,
						IsActive = user.IsActive,
						Roles = user.Roles.Where(role=>role.Name.Equals(Role.GRANT_ADM_USUARIOS.ToString()))
                    }).ToList();

			foreach (var user in queryUserFilter)
			{
				emailTo = user.Email;
				emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
			} 
		}
		/// <summary>
		/// envia email apenas para o demandante
		/// </summary>
		/// <param name="pedido"></param>
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
			emailTo = pedidoItem.Pedido.PedidoArte.FirstOrDefault().UserSolicitante.Email + ";";
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

			var idStatus = pedidoItem.PedidoItemArte.FirstOrDefault().StatusPedidoItemArte.Id;
			if (idStatus == (int)StatusItem.ITEM_EMANALISE) 
			{
				//email base

				var htmlTemplate = File.ReadAllText(Path.Combine(templatePathBody, "TemplateDefaultNotification.html"));
				title = File.ReadAllText(Path.Combine(templatePathTitle, "TitleItemCancelamentoSolicitado.html"));
				content = File.ReadAllText(Path.Combine(templatePathContent, "ContentItemCancelamentoSolicitado.html"));

				htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", content)
					.Replace("{{titulo_pedido}}", pedidoItem.Pedido.Titulo)
					.Replace("{{url_base}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"))
					.Replace("{{pedido_id}}", pedidoItem.IdPedido.ToString());


				
				emailTo = pedidoItem.Pedido.PedidoArte.FirstOrDefault().UserBase.Email + ";";
				emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
			}
			else if (idStatus == (int)StatusItem.ITEM_ATRIBUIDOAOCOMPRADOR ||
				idStatus == (int)StatusItem.ITEM_SOLICITACAODECOMPRAENVIADA || idStatus == (int)StatusItem.ITEM_APROVADO )
			{
				var htmlTemplate = File.ReadAllText(Path.Combine(templatePathBody, "TemplateDefaultNotification.html"));
				title = File.ReadAllText(Path.Combine(templatePathTitle, "TitleItemCancelamentoSolicitado.html"));
				content = File.ReadAllText(Path.Combine(templatePathContent, "ContentItemCancelamentoSolicitado.html"));

				htmlTemplate = htmlTemplate.Replace("{{conteudo_email}}", content)
					.Replace("{{titulo_pedido}}", pedidoItem.Pedido.Titulo)
					.Replace("{{url_base}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"))
					.Replace("{{pedido_id}}", pedidoItem.IdPedido.ToString());

				//email para o comprador
				emailTo = pedidoItem.PedidoItemArte.FirstOrDefault().UserComprador.Email + ";";
				emailSender.SendEmail(emailTo, null, null, title, htmlTemplate, null);
			}
		
		}
		private void EnviarEmailItemAtribuidoComprador(Pedido pedido)
		{
			if (pedido.PedidoArte.FirstOrDefault().UserSolicitante.Email != string.Empty)
			{
				string emailTo = pedido.PedidoArte.FirstOrDefault().UserSolicitante.Email + ";";
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

		Task INotificationHandler<OnVerificarStatusHF>.Handle(OnVerificarStatusHF request, CancellationToken cancellationToken)
		{
            //Todo: Fazer evento de verificar status do pedido que será chamado pelo HangFire
            try
			{
				//
 				bool entrou = true;
			}
			catch (Exception error)
			{
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

				var pedidoItemCompras = pedidoItemCompraRepository.ListByIdPedidoItemCompra(request.PedidoItem.Id, cancellationToken).GetAwaiter().GetResult();

				if (pedidoItemCompras.Count > 0)
				{
					qtdeCompra = 0;
					qtdeEntrega = 0;

					foreach (var compra in pedidoItemCompras)
					{
						qtdeCompra += compra.Quantidade;

					}
				}

				var pedidoItemEntregas = pedidoItemEntregaRepository.ListByIdPedidoItemEntrega(request.PedidoItem.Id, cancellationToken).GetAwaiter().GetResult();

				if (pedidoItemEntregas.Count > 0)
				{
					foreach (var entrega in pedidoItemEntregas)
					{
						qtdeEntrega += entrega.Quantidade;
					}
				}

				qtde = pedidoItem.Quantidade;
				qtdeDevolvida = pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadeDevolvida;
				qtdePendenteCompra = qtde - (qtdeCompra + qtdeDevolvida);
				qtdePendenteEntrega = qtdeCompra - qtdeEntrega;
				

				//unitOfWork.BeginTransaction();
				pedidoItem.Quantidade = qtde;
				pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadeComprada = qtdeCompra;
				pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadePendenteCompra = qtdePendenteCompra;
				pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadeDevolvida = qtdeDevolvida;
				pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadeEntregue = qtdeEntrega;
				pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadePendenteEntrega = qtdePendenteEntrega;

				pedidoItemRepository.AddOrUpdate(pedidoItem, cancellationToken);

				var resultPedidoItem = unitOfWork.SaveChanges();

				if (!resultPedidoItem) throw new ApplicationException("An error has occured.");

				// Entrega total
				if (pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadeEntregue == 
					(pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadeComprada + pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadePendenteCompra + pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadeDevolvida) && pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadeEntregue > 0)
				{
					mediator.Send(new AddTrackingArte()
					{
						TrackingArte = new TrackingArte()
						{
							IdPedidoItem = pedidoItem.Id,
							StatusId = (int) StatusItem.ITEM_ENTREGUE,
							ChangeById = userProvider.User.Login
						}
					}, cancellationToken);

					mediator.Publish(new OnItemEntregue() { PedidoItem = pedidoItem });
				}

				//Entrega parcial
				if (pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadeEntregue < (pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadeComprada + pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadePendenteCompra + pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadeDevolvida) && pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadeEntregue > 0)
				{
					mediator.Send(new DeleteTrackingArte()
					{
						TrackingArte = new TrackingArte()
						{
							IdPedidoItem = pedidoItem.Id,
							StatusId = (int)StatusItem.ITEM_ENTREGUE,
							ChangeById = userProvider.User.Login
						}
					}, cancellationToken);

					mediator.Send(new AddTrackingArte()
					{
						TrackingArte = new TrackingArte()
						{
							IdPedidoItem = pedidoItem.Id,
							StatusId = (int)StatusItem.ITEM_ENTREGUEPARCIALMENTE,
							ChangeById = userProvider.User.Login
						}
					}, cancellationToken);


					mediator.Publish(new OnItemEntregue() { PedidoItem = pedidoItem });
				}

				if (pedidoItem.PedidoItemArte.FirstOrDefault().QuantidadeEntregue == 0)
				{
					mediator.Send(new DeleteTrackingArte()
					{
						TrackingArte = new TrackingArte()
						{
							IdPedidoItem = pedidoItem.Id,
							StatusId = (int)StatusItem.ITEM_ENTREGUE,
							ChangeById = userProvider.User.Login
						}
					}, cancellationToken);

					mediator.Send(new DeleteTrackingArte()
					{
						TrackingArte = new TrackingArte()
						{
							IdPedidoItem = pedidoItem.Id,
							StatusId = (int)StatusItem.ITEM_ENTREGUEPARCIALMENTE,
							ChangeById = userProvider.User.Login
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="notification"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task INotificationHandler<OnNotificacaoVista>.Handle(OnNotificacaoVista notification, CancellationToken cancellationToken)
		{
			//Todo: Remover try quando bus de eventos estiver assincrono.
			//Enquanto este evendo estiver vindo da thread sincronda na query de notificações, o try evita que a consulta seja travada com alguma exception.

			try
			{

				var toSet = notificationRepository.GetAll().Where(n =>
					notification.NotificacaoIds.Contains(n.Id) &&
					!n.Viewers.Where(r => userProvider.User.Login.Equals(r.Login)).Any()
				);

				foreach (var item in toSet.ToList())
					mediator.Send(new SaveViewer()
					{
						Viewer = new Viewer()
						{
							Login = userProvider.User.Login,
							NotificationId = item.Id
						}
					});
			}
			catch (Exception error) {
				throw new Exception(error.Message);
			}

			return Task.FromResult(notification);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="notification"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task INotificationHandler<OnLogon>.Handle(OnLogon onLogon, CancellationToken cancellationToken)
		{
			DateTime agora = DateTime.Now.AddHours(-3);
			CultureInfo idioma = new CultureInfo("pt-BR");

			string title =
				string.Format(
					string.Format(
						"Novo acesso em {0}",
						agora.ToString("f", idioma)
					));

			//Notificacao notificacao = new Notificacao();

			//notificacao.Title = string.Format(string.Format("Novo acesso em {0}", DateTime.Now.ToLongDateString()));
			//notificacao.CreatedAt = DateTime.Now;
			//notificacao.Link = "/";

			//notificacao.Assigns = new List<Assign>() { new Assign(){
			//	Login = userProvider.User.Login
			//}};

			//var envio = mediator.Send(new SaveNotificacao()
			//{
			//	Notificacao = notificacao
			//});

			//envio.Wait();

			SendNotificationsByEmail(title);

			return Task.FromResult(onLogon);
		}

		private void SendNotificationsByEmail(string title)
		{
			//var defaultEmailVM = new DefaultNotificationViewModel();

			//defaultEmailVM.Subject = string.Format("{0}{1}", emailSettings.SubjectPrefixToNotification, title);

			//var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Templates", "Email");
			//var htmlTemplate = System.IO.File.ReadAllText(Path.Combine(templatePath, "TemplateDefaultNotification.html"));

			//htmlTemplate = htmlTemplate.Replace("{{TEXTO}}", title);
			//htmlTemplate = htmlTemplate.Replace("{{LINK_ACTION}}", Environment.GetEnvironmentVariable("URL_APPLICATION_WEB"));
			//htmlTemplate = htmlTemplate.Replace("{{LINK_TEXT}}", "Ir para o site");

			//defaultEmailVM.Body = htmlTemplate;

			//defaultEmailVM.ToEmails.Add(userProvider.User.Email);

			//foreach (var email in defaultEmailVM.ToEmails)
			//{
			//	emailSender.SendEmail(email, null, null, defaultEmailVM.Subject, defaultEmailVM.Body, null);
			//}
		}

		private void SaveNotificacoes(string title, List<Assign> assigns)
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

         
		private static string SerializeEntityObject(object entityObject)
		{
			return JsonConvert.SerializeObject(entityObject, Formatting.Indented,
			new JsonSerializerSettings()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			});
		}

        async Task INotificationHandler<OnEnviarPedidoOracle>.Handle(OnEnviarPedidoOracle notification, CancellationToken cancellationToken)
        {
			try
			{
				var pedido = notification.Pedido;
				var itens = pedido.Itens.Where(a => !string.IsNullOrWhiteSpace(a.Acordo)
					&& !string.IsNullOrWhiteSpace(a.IdItemOracle)
					&& string.IsNullOrWhiteSpace(a.RcPedido));

				if (itens.Count() > 0)
				{
					//Todo: Adicionar as variáveis de ambiente quando obtiver permissão para criar TaskDefinition e ParameterStore.#

					//var requisitioningBUIdEnvironmentVariable = Environment.GetEnvironmentVariable("OIC_REQUISITIONINGBUID");
					//long requisitioningBUId = long.Parse(requisitioningBUIdEnvironmentVariable);
					//var DestinationOrganizationIdEnvironmentVariable = Environment.GetEnvironmentVariable("OIC_DESTINATIONORGANIZATIONID");
					//long DestinationOrganizationId = long.Parse(DestinationOrganizationIdEnvironmentVariable);
					//var deliverToLocationCode = Environment.GetEnvironmentVariable("OIC_DELIVERTOLOCATIONCODE");
					//var requesterEmail = Environment.GetEnvironmentVariable("OIC_REQUESTEREMAIL");
					//var finalidade = Environment.GetEnvironmentVariable("OIC_FINALIDADE");
					//var preparerEmail = Environment.GetEnvironmentVariable("OIC_PREPAREREMAIL");
					long requisitioningBUId = 300000047455934;
					long DestinationOrganizationId = 300000047526566;
					var deliverToLocationCode = "SEDE - GCP ORC";
					var requesterEmail = "juliana.dos.reis@accenture.com";
					var finalidade = "F0000001";
					var preparerEmail = "pic@g.globo";

					if (string.IsNullOrEmpty(requesterEmail))
					{
						var userSolicitante = await userRepository.GetByLogin(pedido.PedidoArte.FirstOrDefault().LoginSolicitante, cancellationToken);
						requesterEmail = userSolicitante.Email;
					}


					var bodyHeader = SerializeEntityObject(new
					{
						RequisitioningBUId = requisitioningBUId,
						PreparerEmail = preparerEmail,
						Description = "Item de Material de Escritório",
						ExternallyManagedFlag = false
					});

					foreach (var item in itens)
					{
						var returnHeader = await purchaseRequisitionProxy.PostRequisitionHeaderAsync(bodyHeader, cancellationToken);
						string requisitionHeaderId = returnHeader.RequisitionHeaderId.ToString();
						//contador++;  
						if (item.DataNecessidade.Value.Date < DateTime.Now.Date)
							throw new BadRequestException("Data necessidade está fora do período!");

						long idItemOracle = 0;
						if (item.IdItemOracle != string.Empty)
							idItemOracle = long.Parse(item.IdItemOracle);
						else
							throw new BadRequestException("item sem id item oracle");

						var bodyLine = SerializeEntityObject(new
						{
							LineNumber = 1,
							LineTypeId = 1,
							LineTypeCode = "Goods",
							CurrencyCode = "BRL",
							DestinationTypeCode = "EXPENSE",
							UOM = "UN",
							DestinationType = "Expense",
							//RequesterEmail = "juliana.dos.reis@accenture.com",
							RequesterEmail = requesterEmail,
							RequestedDeliveryDate = item.DataNecessidade.Value.Date.ToString("yyyy-MM-dd"),
							DestinationOrganizationId = DestinationOrganizationId,//hard coded
							Quantity = item.Quantidade,
							Price = item.Valor,

							DeliverToLocationCode = deliverToLocationCode,
							ItemId = idItemOracle
						}); ;

						var returnLine = await purchaseRequisitionProxy.PostRequisitionLineAsync(requisitionHeaderId, bodyLine, cancellationToken);

						if (returnLine != null)
						{
							//3 - distributions
							string requisitionLineId = returnLine.RequisitionLineId.ToString();
							DistributionDFFClass distributionDFFClass = new DistributionDFFClass();

							distributionDFFClass.finalidade = finalidade;

							var bodyDistribution = SerializeEntityObject(new
							{
								DistributionNumber = 1,//contador,// é o mesmo que LineNumber=1
								Quantity = item.Quantidade,//é o mesmo que quantidade de itens.
								DFF = new[] { distributionDFFClass }
							});

							var returnDistribution = await purchaseRequisitionProxy.PostRequisitionDistributionWithLineAsync(requisitionHeaderId, requisitionLineId, bodyDistribution, cancellationToken);

							var bodySubmitRequisition = SerializeEntityObject(new
							{
								name = "submitRequisition",
								parameters = new string[] { }
							});

							var returnDistributionItem = await purchaseRequisitionProxy.PostRequisitionAsync(requisitionHeaderId, bodySubmitRequisition, cancellationToken);

							if (returnDistributionItem.Result == "SUCCESS")
							{
								item.RcPedido = returnHeader.Requisition;
								pedidoItemRepository.AddOrUpdate(item, cancellationToken);
								var result = unitOfWork.SaveChanges();
							}

						}
					}
				}
			}
			catch (Exception error)
			{
				throw new Exception(error.Message);
			} 
		}

		Task INotificationHandler<OnNovoPedido>.Handle(OnNovoPedido notification, CancellationToken cancellationToken)
		{
			EnviarEmailNovoPedido(notification.Pedido);
			return Task.FromResult(true);
		}

    }
}

