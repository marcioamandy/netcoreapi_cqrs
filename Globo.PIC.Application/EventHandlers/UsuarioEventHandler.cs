using System;
using Globo.PIC.Domain.Entities;
using MediatR;
using System.Threading.Tasks;
using Globo.PIC.Domain.Types.Events;
using Globo.PIC.Domain.Interfaces;
using System.Threading;
using Newtonsoft.Json;
using Amazon.SimpleNotificationService.Model;
using Globo.PIC.Domain.Models;
using Amazon.SimpleNotificationService;
using Amazon;
using System.Net;
using AutoMapper;
using Globo.PIC.Domain.ViewModels;
using Amazon.SQS.Model;
using Amazon.SQS;
using System.Linq;

namespace Globo.PIC.Domain.EventHandlers
{
    /// <summary>
    ///
    /// </summary>
    public class UsuarioEventHandler :
        INotificationHandler<OnUsuarioCriado>,
        INotificationHandler<OnUsuarioAlterado>,
        INotificationHandler<OnUsuarioADSynced>
    {

        /// <summary>
        /// 
        /// </summary>
        static string EmailErrorTo { get; } = Environment.GetEnvironmentVariable("EMAIL_ERROR_TO");

        /// <summary>
        /// 
        /// </summary>
        string snsArn { get; } = Environment.GetEnvironmentVariable("SNS_USUARIO_ARN");

        /// <summary>
        /// 
        /// </summary>
        string sqsUrl { get; } = Environment.GetEnvironmentVariable("SQS_USUARIO_URL");

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

        // /// <summary>
        // /// 
        // /// </summary>
        // private readonly IADServiceLDAP aDServiceLDAP;

        /// <summary>
		/// 
		/// </summaryx
		private readonly IMapper mapper;

        /// <summary>
        /// 
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        public UsuarioEventHandler(
            IRepository<Usuario> _userRepository,
            IUserProvider _userProvider,
            IEmailSender _emailSender,
            // IADServiceLDAP _aDServiceLDAP,
            IUnitOfWork _unitOfWork,
            IMapper _mapper)
        {
            userRepository = _userRepository;
            userProvider = _userProvider;
            emailSender = _emailSender;
            // aDServiceLDAP = _aDServiceLDAP;
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        Task INotificationHandler<OnUsuarioCriado>.Handle(OnUsuarioCriado notification, CancellationToken cancellationToken)
        {
            if (notification.Usuario == null) return Unit.Task;

            try
            {
                var vm = mapper.Map<UsuarioViewModel>(notification.Usuario);

                var task = PublishSnsRequest(vm); task.Wait();
                //UserUpdateWithAD(notification.Usuario, cancellationToken);
            }
            catch (Exception e)
            {
                EventException(e, notification.Usuario);
            }

            return Unit.Task;
        }

        Task INotificationHandler<OnUsuarioAlterado>.Handle(OnUsuarioAlterado notification, CancellationToken cancellationToken)
        {
            //if (notification.Usuario == null || !string.IsNullOrWhiteSpace(notification.Usuario.Email)) return Unit.Task;

            //try
            //{
            //    if (notification.Usuario.Login == "tcordeir")
            //        UpdateAll(cancellationToken);
            //    else
            //        UserUpdateWithAD(notification.Usuario, cancellationToken);
            //}
            //catch (Exception e)
            //{
            //    EventException(e, notification.Usuario);
            //}

            return Unit.Task;
        }

        Task INotificationHandler<OnUsuarioADSynced>.Handle(OnUsuarioADSynced notification, CancellationToken cancellationToken)
        {            
            try
            {
            //    if (notification.Usuario.Login == "tcordeir")
            //        UpdateAll(cancellationToken);
            //    else
            //        UserUpdateWithAD(notification.Usuario, cancellationToken);

                var task = GetMessage(); task.Wait();
                
                if(task.Result.Messages.Count() <= 0) return Unit.Task;

                foreach (var item in task.Result.Messages)
                {
                    var obj = JsonConvert.DeserializeObject<Usuario>(item.Body);
    
                    var usuario = userRepository.GetAll().Where(x => x.Login == obj.Login).FirstOrDefault();
                    
                    usuario.Apelido = obj.Apelido;
                    usuario.LastName = obj.LastName;
                    usuario.Name = obj.Name;
                    usuario.Email = obj.Email;
                    
                    userRepository.Update(usuario);

                    unitOfWork.Commit();
                }
            
            }
            catch //(Exception e)
            {
               //EventException(e, notification.Usuario);
            }

            return Unit.Task;
        }

        private async Task<ReceiveMessageResponse> GetMessage()
        {
            // Create the Amazon SQS client
            var sqsClient = new AmazonSQSClient();

            return await sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = sqsUrl,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 0                
            });
        }

        private async Task<bool> PublishSnsRequest(UsuarioViewModel message)
        {
            var snsClient = new AmazonSimpleNotificationServiceClient(RegionEndpoint.USEast1);

            var mensagem = new MensagemSNS<UsuarioViewModel>(message);

            PublishRequest publishReq = new PublishRequest()
            {
                Subject = "usuario-criado",
                TargetArn = snsArn,
                MessageStructure = "json",
                Message = mensagem.Serialize()
            };

            PublishResponse response = await snsClient.PublishAsync(publishReq);

            if (response.HttpStatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        //void UpdateAll(CancellationToken cancellationToken)
        //{
        //    var all = userRepository.GetAll().Where(x => x.IsActive && string.IsNullOrWhiteSpace(x.Email)).ToList();

        //    var sucessos = new List<Usuario>();
        //    var falhas = new List<Usuario>();

        //    foreach (var item in all)
        //    {
        //        try
        //        {
        //            UserUpdateWithAD(item, cancellationToken);

        //            sucessos.Add(item);
        //        }
        //        catch (Exception) { falhas.Add(item); }
        //    }

        //    if (sucessos.Count > 0)
        //    {
        //        var topSucesso = "<br/><br/><h1>Atualizações com sucesso:</h1>";

        //        var bodySucesso = sucessos.Select(s =>
        //        {
        //            return String.Format(@"<br/>
        //                    <p>Nome: {0}</p>
        //                    <p>Sobrenome: {1}</p>
        //                    <p>Email: {2}</p>
        //                ", s.Name, s.LastName, s.Email);
        //        }).Aggregate((x, y) => x + y);

        //        emailSender.SendEmail(EmailErrorTo, null, null, "Relatório de execução da atualiza das informações dos usários vindas do AD",
        //                    topSucesso + bodySucesso, null);
        //    }

        //    if (falhas.Count > 0)
        //    {
        //        var topFalha = "<br/><br/><h1>Falhas:</h1>";

        //        var bodyFalha = falhas.Select(s =>
        //        {
        //            return String.Format(@"<br/>
        //                    <p>Login: {0}</p>
        //                ", s.Login);
        //        }).Aggregate((x, y) => x + y);

        //        emailSender.SendEmail(EmailErrorTo, null, null, "PIC Exception - User Event - Falha nas atualizações das informações do AD",
        //                    topFalha + bodyFalha, null);
        //    }
        //}

        //void UserUpdateWithAD(Usuario user, CancellationToken cancellationToken)
        //{
        //    var identityUser = aDServiceLDAP.GetUser(user.Login);

        //    //Se não encontrar o usuário desativa
        //    if (identityUser != null)
        //    {
        //        user.Name = identityUser.Name;
        //        user.LastName = identityUser.LastName;
        //        user.Email = identityUser.Email;
        //    }
        //    else
        //        user.IsActive = false;

        //    userRepository.AddOrUpdate(user, cancellationToken);

        //    unitOfWork.Commit();

        //    if (identityUser == null) throw new NotFoundException("Usuário não encontrado.");
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="saida"></param>
        /// <param name="emailSender"></param>
        void EventException(Exception e, Usuario user)
        {
            var stack = string.Empty;
            var msg = e.Message;
            var inner = e.InnerException?.Message;

            try
            {
                stack = JsonConvert.SerializeObject(e, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            string body = string.Format(@"
                <p>Mensagem: {0}</p>                
                <p>Inner: {1}</p>                
                <p>Stack:</p>
                <p>{2}</p>
                <p>Usuário: {3}</p>", msg, inner, stack, user.Login);


            emailSender.SendEmail(EmailErrorTo, null, null, "PIC Exception - User Event", body, null);
        }
    }
}
