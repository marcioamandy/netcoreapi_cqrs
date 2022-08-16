using AutoMapper;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.ViewModels;
using MediatR;
using System;
using System.Threading;

namespace Globo.PIC.Application.ResolverHandlers
{

	/// <summary>
	/// 
	/// </summary>
	public class PresignUrlResolverHandler :
		IMappingAction<PedidoItemConversaAnexo, ArquivoViewModel>,
		IMappingAction<PedidoItemAnexo, ArquivoViewModel>,
		IMappingAction<PedidoAnexo, ArquivoViewModel>,
		IMappingAction<PedidoItemArteCompraDocumentoAnexo, ArquivoViewModel>,
		IMappingAction<AcionamentoItemAnexo, ArquivoViewModel>,
		IMappingAction<ItemAnexo, ArquivoViewModel>
	{

		/// <summary>
		/// 
		/// </summary>
		private readonly IMediator mediator;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_mediator"></param>
		public PresignUrlResolverHandler(IMediator _mediator)
		{
			mediator = _mediator;
		}

		void IMappingAction<AcionamentoItemAnexo, ArquivoViewModel>.Process(AcionamentoItemAnexo source, ArquivoViewModel destination, ResolutionContext context)
		{
			var taskUrl = mediator.Send(new GetPreSignedUrl()
			{
				Key = source.NomeArquivo,
				FileName = source.NomeOriginal,
				Verb = "GET",
				Expires = DateTime.Now.AddDays(1)
			}, CancellationToken.None); taskUrl.Wait();

			destination.CaminhoArquivo = taskUrl.Result;
		}

		void IMappingAction<ItemAnexo, ArquivoViewModel>.Process(ItemAnexo source, ArquivoViewModel destination, ResolutionContext context)
		{
			var taskUrl = mediator.Send(new GetPreSignedUrl()
			{
				Key = source.NomeArquivo,
				FileName = source.NomeOriginal,
				Verb = "GET",
				Expires = DateTime.Now.AddDays(1)
			}, CancellationToken.None); taskUrl.Wait();

			destination.CaminhoArquivo = taskUrl.Result;
		}

		void IMappingAction<PedidoItemAnexo, ArquivoViewModel>.Process(PedidoItemAnexo source, ArquivoViewModel destination, ResolutionContext context)
		{
			var taskUrl = mediator.Send(new GetPreSignedUrl()
			{
				Key = source.NomeArquivo,
				FileName = source.NomeOriginal,
				Verb = "GET",
				Expires = DateTime.Now.AddDays(1)
			}, CancellationToken.None); taskUrl.Wait();

			destination.CaminhoArquivo = taskUrl.Result;
		}

		void IMappingAction<PedidoItemConversaAnexo, ArquivoViewModel>.Process(PedidoItemConversaAnexo source, ArquivoViewModel destination, ResolutionContext context)
		{
			var taskUrl = mediator.Send(new GetPreSignedUrl()
			{
				Key = source.NomeArquivo,
				FileName = source.NomeOriginal,
				Verb = "GET",
				Expires = DateTime.Now.AddDays(1)
			}, CancellationToken.None); taskUrl.Wait();

			destination.CaminhoArquivo = taskUrl.Result;
		}

		void IMappingAction<PedidoAnexo, ArquivoViewModel>.Process(PedidoAnexo source, ArquivoViewModel destination, ResolutionContext context)
		{
			var taskUrl = mediator.Send(new GetPreSignedUrl()
			{
				Key = source.NomeArquivo,
				FileName = source.NomeOriginal,
				Verb = "GET",
				Expires = DateTime.Now.AddDays(1)
			}, CancellationToken.None); taskUrl.Wait();

			destination.CaminhoArquivo = taskUrl.Result;
		}

		void IMappingAction<PedidoItemArteCompraDocumentoAnexo, ArquivoViewModel>.Process(PedidoItemArteCompraDocumentoAnexo source, ArquivoViewModel destination, ResolutionContext context)
		{
			var taskUrl = mediator.Send(new GetPreSignedUrl()
			{
				Key = source.NomeArquivo,
				FileName = source.NomeOriginal,
				Verb = "GET",
				Expires = DateTime.Now.AddDays(1)
			}, CancellationToken.None); taskUrl.Wait();

			destination.CaminhoArquivo = taskUrl.Result;
		}
	}
}
