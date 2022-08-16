//using AutoMapper;
//using Bogus;
//using FluentAssertions;
//using Moq;
//using OpenCqrs;
//using OpenCqrs.Commands;
//using OpenCqrs.Dependencies;
//using OpenCqrs.Domain;
//using OpenCqrs.Events;
//using System;
//using System.Collections.Generic;
//using System.Threading;
//using TVGlobo.R2P.Parceria.Domain.Commands;
//using TVGlobo.R2P.Parceria.Domain.Entities;
//using Xunit;

namespace TVGlobo.R2P.Parceria.Tests.API.Controller
{
	public class SupplierControllerTest
	{

		//private ICommandSender _sut;

		//private Mock<IHandlerResolver> _handlerResolver;
		//private Mock<IEventPublisher> _eventPublisher;
		//private Mock<IEventStore> _eventStore;
		//private Mock<ICommandStore> _commandStore;
		//private Mock<IEventFactory> _eventFactory;

		//private Mock<ICommandHandler<DeleteSupplier>> _commandHandler;

		//private DeleteSupplier deleteSupplier;

		/// <summary>
		/// 
		/// </summary>
		public SupplierControllerTest()
		{
			//var supplier = CriarFornecedor();

			//deleteSupplier = new DeleteSupplier()
			//{
			//	Supplier = supplier,
			//	UserId = "Teste",
			//	Source = "v1",
			//	ExpectedVersion = 1,
			//	CancellationToken = CancellationToken.None
			//};			

			//_commandHandler = new Mock<ICommandHandler<DeleteSupplier>>();
			//_commandHandler
			//	.Setup(x => x.Handle(deleteSupplier));

			//_handlerResolver = new Mock<IHandlerResolver>();
			//_handlerResolver
			//	.Setup(x => x.ResolveHandler<ICommandHandler<DeleteSupplier>>())
			//	.Returns(_commandHandler.Object);

			//_sut = new CommandSender(_handlerResolver.Object, _eventPublisher.Object, _eventFactory.Object, _eventStore.Object, _commandStore.Object);
		}

		//private Supplier CriarFornecedor()
		//{
			//var filesNames = new[] { "apple", "banana", "orange", "strawberry", "kiwi" };

			//var contacts = new Faker<Contact>()
			//	.RuleFor(p => p.Id, f => f.Random.Long(1, 10));

			//var files = new Faker<File>()
			//	.RuleFor(p => p.Id, f => f.Random.Long(1, 10))
			//	.RuleFor(p => p.Name, f => f.)

		//	var supplier = new Faker<Supplier>()
		//		.RuleFor(p => p.Id, f => f.Random.Long(1, 10))
		//		.RuleFor(p => p.Contacts, f => contacts.Generate(3))
		//		.RuleFor(p => p.Contacts, f => new List<Contact>())
		//		.RuleFor(p => p.Files, f => new List<File>())
		//		.RuleFor(p => p.Orders, f => new List<Order>())
		//		.RuleFor(p => p.SupplierDepartments, f => new List<SupplierDepartment>())
		//		.RuleFor(p => p.SupplierSegments, f => new List<SupplierSegment>())
		//		.RuleFor(p => p.SupplierProducts, f => new List<SupplierProduct>())
		//		.RuleFor(p => p.CpfCnpj, f => new Randomizer().Replace("###.###.###-##"))
		//		.RuleFor(p => p.CreatedAt, f => f.Date.Past())
		//		.RuleFor(p => p.Facebook, f => f.Internet.Url())
		//		.RuleFor(p => p.Instagram, f => f.Internet.Url())
		//		.RuleFor(p => p.IsActive, f => true)
		//		.RuleFor(p => p.Name, f => f.Name.FirstName())
		//		.RuleFor(p => p.Note, f => f.Lorem.Text())
		//		.RuleFor(p => p.Site, f => f.Internet.Url())
		//		.RuleFor(p => p.UpdatedAt, f => DateTime.Now);

		//	return supplier.Generate();
		//}

		//private QueryHandler GetQueryHandler()
		//{
		//	return new QueryHandler(
		//}


		//[Fact]
		//public void RemoveFornecedor_OK()
		//{

		//	_sut.Send(deleteSupplier);

		//	_commandHandler.Verify(x => x.Handle(deleteSupplier), Times.Once);

			////Arrange


			//var taskSup = Task.FromResult(supplier);

			//	.Setup(m => m.Han<GetById, Supplier>(new GetById()
			//{
			//	Id = supplier.Id,
			//	CancellationToken = CancellationToken.None
			//})).Returns(taskSup);

			//var esperado = dispatcher.Awaiting(m => m.Object.GetResultAsync<GetById, Supplier>(new GetById()
			//{
			//	Id = supplier.Id,
			//	CancellationToken = CancellationToken.None
			//}), r => r);

			//esperado. Wait();

			//var resul = esperado.Result;

			////Act
			//var result = controller.Delete(supplier.Id.ToString(), CancellationToken.None);

			//Assert
			//(((Microsoft.AspNetCore.Mvc.BadRequestObjectResult)result).StatusCode == 200).Should().BeTrue();
			//(((Microsoft.AspNetCore.Mvc.BadRequestObjectResult)result).StatusCode == 332).Should().BeFalse();
		//}

		//[Fact]
		//public void RemoveFornecedor_BadRequest()
		//{
		//	//Arrange
		//	var supplier = new Faker<Supplier>()
		//		.RuleFor(p => p.Id, f => f.Random.Long(-1000, -30))
		//		.Generate();

		//	//Act
		//	var result = controller.Delete(supplier.Id.ToString(), CancellationToken.None);

		//	//Assert
		//	(((Microsoft.AspNetCore.Mvc.BadRequestObjectResult)result).StatusCode == 400).Should().BeTrue();
		//	(((Microsoft.AspNetCore.Mvc.BadRequestObjectResult)result).StatusCode == 332).Should().BeFalse();
		//}

	}
}
