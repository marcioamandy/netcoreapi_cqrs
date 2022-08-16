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
using Microsoft.EntityFrameworkCore.Infrastructure; 
using Globo.PIC.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Globo.PIC.Domain.EventHandlers
{
	/// <summary>
	///
	/// </summary>
	public class ScheduleEventHandler
		:
		INotificationHandler<OnScheduleConteudoSync>

	{
		 
		/// <summary>
		/// 
		/// </summary>
		private readonly IUnitOfWork unitOfWork;
		 
		/// <summary>
		/// 
		/// </summary>
		private readonly IRepository<Conteudo> conteudoRepository;

        private readonly IDbContextFactory<MdiDbContext> conteudoFactory;
         
        /// <summary>
        /// 
        /// </summary>
        private readonly IConteudoServiceProxy conteudoServiceProxy;

		
		public ScheduleEventHandler(IUnitOfWork _unitOfWork,  
            IConteudoServiceProxy _conteudoServiceProxy, 
            IRepository<Conteudo> _conteudoRepository,
            IDbContextFactory<MdiDbContext> _conteudoFactory)
		{
			unitOfWork = _unitOfWork;
			conteudoServiceProxy = _conteudoServiceProxy;
			conteudoRepository = _conteudoRepository;
            conteudoFactory = _conteudoFactory;

        }


        async Task INotificationHandler<OnScheduleConteudoSync>.Handle(OnScheduleConteudoSync notification, CancellationToken cancellationToken)
        {

            List<Conteudo> conteudosPic = new List<Conteudo>();

            var conteudoBruto = await conteudoServiceProxy.GetConteudos();


            List<Conteudo> conteudos = new List<Conteudo>();
            if (conteudoBruto != null)
                conteudos = conteudoBruto.Where(a => !a.Codigo.ToUpper().StartsWith("RAG-")).ToList();
             

          
            try
            {


                using (var context = (MdiDbContext)unitOfWork.GetContextFactory())
                {
                    if (context.Database.GetCommandTimeout()>1)
                        context.Database.SetCommandTimeout(0); 

                    conteudosPic = context.Conteudo.ToList();
                   
                    foreach (var conteudo in conteudos)
                    {
                        if (string.IsNullOrEmpty(conteudo.Codigo) ||
                            string.IsNullOrEmpty(conteudo.Nome) || 
                            string.IsNullOrEmpty(conteudo.Status))
                            continue;

                        Conteudo verificaConteudo = new Conteudo(); 
                       
                        verificaConteudo = conteudosPic.Where(a => a.Codigo == conteudo.Codigo).FirstOrDefault();

                        //verifica se já está na base
                        if (verificaConteudo != null){
                            //verifica se algo mudou
                            if (conteudo.Nome!= verificaConteudo.Nome ||
                                conteudo.Status  != verificaConteudo.Status){

                                 
                                verificaConteudo.Status = conteudo.Status;
                                verificaConteudo.Codigo = conteudo.Codigo;

                                if (conteudo.Nome.Length > 500)
                                    verificaConteudo.Nome = conteudo.Nome.Substring(0, 499);
                                else
                                    verificaConteudo.Nome = conteudo.Nome;

                                context.Conteudo.Update(verificaConteudo);
                                context.SaveChanges();
                            }
                            else
                            {
                                //se não mudar nada, vai para o próximo.
                                continue;
                            }
                        }
                        else
                        {
                            //se não existir na base então insere
                            verificaConteudo = new Conteudo();
                            verificaConteudo.Status = conteudo.Status;
                            verificaConteudo.Codigo = conteudo.Codigo;

                            if (conteudo.Nome.Length > 500)
                                verificaConteudo.Nome = conteudo.Nome.Substring(0, 499);
                            else
                                verificaConteudo.Nome = conteudo.Nome;

                            context.Conteudo.Add(verificaConteudo);
                            context.SaveChanges();

                        } 
                    }
                }
            }
            catch(Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}

