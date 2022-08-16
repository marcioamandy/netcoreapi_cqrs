
//using OpenCqrs.Commands;
//using System;
//using System.Collections.Generic;
//using System.Threading;
//using Xunit;
//using OpenCqrs;
//using Moq;
//using Bogus;
//using TVGlobo.R2P.Parceria.Domain.Interfaces;
//using TVGlobo.R2P.Parceria.Domain.Entities;
//using TVGlobo.R2P.Parceria.Domain.Commands;
//using TVGlobo.R2P.Parceria.Application.Handlers;

namespace TVGlobo.R2P.Parceria.Tests.Application.Handlers
{
	/// <summary>
	/// 
	/// </summary>
	public class CommandHandlerTest
	{
		//private Mock<IRepository<Programme>> programmeRepository;
		//private Mock<IRepository<Supplier>> supplierRepository;
		//private Mock<IRepository<Order>> orderRepository;
		//private Mock<IRepository<OrderItem>> orderItemRepository;
		//private Mock<IRepository<Segment>> segmentRepository;
		//private Mock<IRepository<Product>> productRepository;
		//private Mock<IRepository<Department>> departmentRepository;
		//private Mock<IRepository<Contact>> contactRepository;
		//private Mock<IRepository<File>> fileRepository;
		//private Mock<IRepository<ItemFile>> itemFileRepository;
		//private Mock<IRepository<SupplierDepartment>> supplierDepartmentRepository;
		//private Mock<IRepository<SupplierProduct>> supplierProductRepository;
		//private Mock<IRepository<SupplierSegment>> supplierSegmentRepository;
		//private Mock<IRepository<Scenario>> scenarioRepository;
		//private Mock<IRepository<Status>> statusRepository;
		//private Mock<IRepository<Tracking>> trackingRepository;
		//private Mock<IRepository<User>> userRepository;
		//private Mock<IRepository<Store>> storeRepository;
		//private Mock<IRepository<Notification>> notificationRepository;
		//private Mock<IRepository<NegotiationType>> negotiationTypeRepository;
		//private Mock<IRepository<OrderFile>> orderFileRepository;
		//private Mock<IRepository<Viewer>> viewerRepository;
		//private Mock<IRepository<Reader>> readerRepository;
		//private Mock<IUserProvider> userProvider;
		//private Mock<IUnitOfWork> unitOfWork;
		//private Mock<IDispatcher> dispatcher;

		//private Supplier supplier;

		/// <summary>
		/// 
		/// </summary>
		//private DeleteSupplier deleteSupplier;


		//public CommandHandlerTest()
		//{
		//	supplier = CriarFornecedor();
		//	dispatcher = new Mock<IDispatcher>();
		//	unitOfWork = new Mock<IUnitOfWork>();
		//	programmeRepository = new Mock<IRepository<Programme>>();
		//	supplierRepository = new Mock<IRepository<Supplier>>();
		//	orderRepository = new Mock<IRepository<Order>>();
		//	orderItemRepository = new Mock<IRepository<OrderItem>>();
		//	segmentRepository = new Mock<IRepository<Segment>>();
		//	productRepository = new Mock<IRepository<Product>>();
		//	departmentRepository = new Mock<IRepository<Department>>();
		//	contactRepository = new Mock<IRepository<Contact>>();
		//	fileRepository = new Mock<IRepository<File>>();
		//	itemFileRepository = new Mock<IRepository<ItemFile>>();
		//	supplierDepartmentRepository = new Mock<IRepository<SupplierDepartment>>();
		//	supplierProductRepository = new Mock<IRepository<SupplierProduct>>();
		//	supplierSegmentRepository = new Mock<IRepository<SupplierSegment>>();
		//	scenarioRepository = new Mock<IRepository<Scenario>>();
		//	statusRepository = new Mock<IRepository<Status>>();
		//	trackingRepository = new Mock<IRepository<Tracking>>();
		//	userRepository = new Mock<IRepository<User>>();
		//	storeRepository = new Mock<IRepository<Store>>();
		//	notificationRepository = new Mock<IRepository<Notification>>();
		//	negotiationTypeRepository = new Mock<IRepository<NegotiationType>>();
		//	orderFileRepository = new Mock<IRepository<OrderFile>>();
		//	viewerRepository = new Mock<IRepository<Viewer>>();
		//	readerRepository = new Mock<IRepository<Reader>>();

		//	supplierRepository.Setup(s => s.Remove(supplier));
		//	unitOfWork.Setup(s => s.Commit()).Returns(true);
		//}

		//private Supplier CriarFornecedor()
		//{
		//	var filesNames = new[] { "apple", "banana", "orange", "strawberry", "kiwi" };

		//	var contacts = new Faker<Contact>()
		//		.RuleFor(p => p.Id, f => f.Random.Long(1, 10));

		//	var files = new Faker<File>()
		//		.RuleFor(p => p.Id, f => f.Random.Long(1, 10))
		//		.RuleFor(p => p.Name, f => f.Name.LastName())
		//		.RuleFor(p => p.OriginalName, f => f.Name.LastName());

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

		//[Fact]
		//public void RemoveFornecedorTeste()
		//{
		//	deleteSupplier = new DeleteSupplier()
		//	{
		//		Supplier = supplier,
		//		UserId = "Teste",
		//		Source = "v1",
		//		ExpectedVersion = 1,
		//		CancellationToken = CancellationToken.None
		//	};

		//	ICommandHandler<DeleteSupplier> commandHandler = this.GetCommandHandler();
		//	commandHandler.Handle(deleteSupplier);
		//}

		//private Supplier CriarFornecedor()
		//{
		//	//var filesNames = new[] { "apple", "banana", "orange", "strawberry", "kiwi" };

		//	//var contacts = new Faker<Contact>()
		//	//	.RuleFor(p => p.Id, f => f.Random.Long(1, 10));

		//	//var files = new Faker<File>()
		//	//	.RuleFor(p => p.Id, f => f.Random.Long(1, 10))
		//	//	.RuleFor(p => p.Name, f => f.)

		//	var supplier = new Faker<Supplier>()
		//		.RuleFor(p => p.Id, f => f.Random.Long(1, 10))
		//		//.RuleFor(p => p.Contacts, f => contacts.Generate(3))
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

		//private CommandHandler GetCommandHandler()
		//{
		//	return new CommandHandler(
		//		unitOfWork.Object,
		//		dispatcher.Object,
		//		userProvider.Object,
		//		programmeRepository.Object,
		//		supplierRepository.Object,
		//		segmentRepository.Object,
		//		storeRepository.Object,
		//		productRepository.Object,
		//		departmentRepository.Object,
		//		supplierDepartmentRepository.Object,
		//		supplierProductRepository.Object,
		//		supplierSegmentRepository.Object,
		//		contactRepository.Object,
		//		fileRepository.Object,
		//		itemFileRepository.Object,
		//		orderRepository.Object,
		//		orderItemRepository.Object,
		//		scenarioRepository.Object,
		//		statusRepository.Object,
		//		trackingRepository.Object,
		//		userRepository.Object,
		//		notificationRepository.Object,
		//		negotiationTypeRepository.Object,
		//		orderFileRepository.Object,
		//		viewerRepository.Object,
		//		readerRepository.Object
		//	);
		//}
	}
}
