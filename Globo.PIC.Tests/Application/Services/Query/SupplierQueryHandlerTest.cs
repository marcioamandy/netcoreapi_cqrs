using System;
using Xunit;
using Moq;
using System.Linq;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using TVGlobo.Projetos.PIC.Application.Queries;
using TVGlobo.Projetos.PIC.Application.Services.Query;
using TVGlobo.Projetos.PIC.Domain.Entities;
using TVGlobo.Projetos.PIC.Domain.Interfaces;
using TVGlobo.Projetos.PIC.Domain.Models;
using System.Collections.Generic;
using MockQueryable.Moq;
using TVGlobo.Projetos.PIC.Domain.Enums;
using MediatR;

namespace TVGlobo.Projetos.PIC.Tests.Application.Services.Query
{
	/// <summary>
	/// 
	/// </summary>
	public class SupplierQueryHandlerTest
	{

		/// <summary>
		/// 
		/// </summary>
		private readonly Mock<IRepository<Supplier>> supplierRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly Mock<IRepository<SupplierProduct>> supplierProductRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly Mock<IRepository<SupplierSegment>> supplierSegmentRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly Mock<IRepository<SupplierDepartment>> supplierDepartmentRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly Mock<IRepository<SupplierType>> supplierTypeRepository;

		/// <summary>
		/// 
		/// </summary>
		private readonly Mock<PaggingConfiguration> paggingConfiguration;

		/// <summary>
		/// 
		/// </summary>
		private readonly Mock<IUserProvider> userProvider;

		/// <summary>
		/// 
		/// </summary>
		private Supplier supplier;

		/// <summary>
		/// 
		/// </summary>
		private List<Supplier> listSuppliers;

		/// <summary>
		/// 
		/// </summary>
		private List<SupplierType> listSupplierTypes;

		/// <summary>
		/// 
		/// </summary>
		private IMediator mediator;

		/// <summary>
		/// 
		/// </summary>
		public SupplierQueryHandlerTest(IMediator _mediator)
		{
			supplierRepository = new Mock<IRepository<Supplier>>();
			supplierProductRepository = new Mock<IRepository<SupplierProduct>>();
			supplierSegmentRepository = new Mock<IRepository<SupplierSegment>>();
			supplierDepartmentRepository = new Mock<IRepository<SupplierDepartment>>();
			supplierTypeRepository = new Mock<IRepository<SupplierType>>();
			paggingConfiguration = new Mock<PaggingConfiguration>();
			userProvider = new Mock<IUserProvider>();
			mediator = _mediator;

			#region Setup

			//LoadDataCollection();

			//userProvider.Setup(r => r.User)
			//	.Returns(new IdentityUser()
			//	{
			//		Authorization = new Authorization()
			//		{
			//			Roles = new List<string>() { Roles.FACILIDADE_PERFIL.ToString() }
			//		},
			//		Email = "thiago.cordeiro_thera@prestador.globo",
			//		HasDepartment = true,
			//		IsActive = true,
			//		Login = "tscord",
			//		Name = "Thiago",
			//		LastName = "Santos Cordeiro"
			//	});

			paggingConfiguration.Object.PerPage = 20;

			#endregion Setup
		}
		
		/// <summary>
		/// 
		/// </summary>
		//[Fact]
		//public async void GetBySupplierTypeFilter_Test()
		//{
		//	// Arrange
		//	var query = new GetAll()
		//	{
		//		CancellationToken = CancellationToken.None
		//	};

		//	supplierTypeRepository.Setup(r => r.GetAll())
		//		.Returns(listSuppliers.AsQueryable().BuildMock().Object);

		//	// Act
		//	IQueryHandlerAsync<GetBySupplierTypeFilter, List<SupplierType>> supplierHandler = GetSupplierQueryHandler();

		//	var suppliers = await supplierHandler.RetrieveAsync(query);

		//	// Assert
		//	suppliers.Should().BeOfType<List<SupplierType>>();
		//}

		/// <summary>
		/// 
		/// </summary>
		[Fact]
		public async void GetAllSupplier_Test()
		{
			// Arrange
			var query = new GetAll()
			{
				CancellationToken = CancellationToken.None
			};

			supplierRepository.Setup(r => r.GetAll())
				.Returns(listSuppliers.AsQueryable().BuildMock().Object);

			// Act
			//IQueryHandlerAsync<GetAll, List<Supplier>> supplierHandler = GetSupplierQueryHandler();

			//var suppliers = await supplierHandler.RetrieveAsync(query);

			// Assert
			//suppliers.Should().BeOfType<List<Supplier>>();
		}

		/// <summary>
		/// Cenários de GetByFilterSupplierFoundOne
		/// </summary>
		/// <returns></returns>
		//public static IEnumerable<object[]> GetByFilterSupplierFoundOne_Parameters()
		//{
		//	yield return new object[]
		//	{
		//		new GetBySupplierFilter() {
		//			Filter = new SupplierFilterViewModel()
		//			{
		//				Name = "Fornecedor 1",
		//				Page = 1
		//			},
		//			CancellationToken = CancellationToken.None
		//		}
		//	};

		//	yield return new object[]
		//	{
		//		new GetBySupplierFilter() {
		//			Filter = new SupplierFilterViewModel()
		//			{
		//				IsActive = false,
		//				Page = 1
		//			},
		//			CancellationToken = CancellationToken.None
		//		}
		//	};

		//	yield return new object[]
		//	{
		//		new GetBySupplierFilter() {
		//			Filter = new SupplierFilterViewModel()
		//			{
		//				DepartmentIds = new long[] { 2 },
		//				Page = 1
		//			},
		//			CancellationToken = CancellationToken.None
		//		}
		//	};
		//}

		/// <summary>
		/// 
		/// </summary>
		//[Theory]
		//[MemberData(nameof(GetByFilterSupplierFoundOne_Parameters))]
		//public async void GetByFilterSupplierFoundOne_Test(GetBySupplierFilter query)
		//{
		//	// Arrange
		//	supplierRepository.Setup(r => r.GetAll())
		//		.Returns(listSuppliers.AsQueryable().BuildMock().Object);

		//	// Act
		//	IQueryHandlerAsync<GetBySupplierFilter, List<Supplier>> supplierHandler = GetSupplierQueryHandler();

		//	var suppliers = await supplierHandler.RetrieveAsync(query);
			
		//	// Assert
		//	suppliers.Should().NotBeEmpty().And.HaveCount(1);
		//}

		/// <summary>
		/// 
		/// </summary>
		//[Theory]
		//[MemberData(nameof(GetByFilterSupplierFoundOne_Parameters))]
		//public async void GetByFilterCountSupplierFoundOne_Test(GetBySupplierFilter query)
		//{
		//	// Arrange
		//	supplierRepository.Setup(r => r.GetAll())
		//		.Returns(listSuppliers.AsQueryable().BuildMock().Object);

		//	// Act
		//	IQueryHandlerAsync<GetBySupplierFilter, int> supplierHandler = GetSupplierQueryHandler();

		//	var suppliers = await supplierHandler.RetrieveAsync(query);

		//	// Assert
		//	suppliers.Should().Be(1);
		//}

		/// <summary>
		/// 
		/// </summary>
		//[Fact]
		//public async void GetSupplierById_Test()
		//{
		//	// Arrange
		//	var query = new GetById()
		//	{
		//		CancellationToken = CancellationToken.None,
		//		Id = 1
		//	};

		//	supplierRepository.Setup(r => r.GetById(1, CancellationToken.None))
		//		.Returns(Task.FromResult(supplier));

		//	// Act
		//	IQueryHandlerAsync<GetById, Supplier> supplierHandler = GetSupplierQueryHandler();

		//	var user = await supplierHandler.RetrieveAsync(query);

		//	// Assert
		//	user.Should().BeOfType<Supplier>();
		//}

		//private void LoadDataCollection()
		//{
		//	supplier = new Supplier()
		//	{
		//		Id = 1,
		//		Name = "Fornecedor 1",
		//		CpfCnpj = "45457875457",
		//		Facebook = "face.com",
		//		Instagram = "insta.com",
		//		Note = "Note",
		//		Site = "site.com",
		//		Contacts = new List<Contact>() {
		//			new Contact() {
		//				Id = 1,
		//				Name = "Nome do Contato",
		//				Area = "Area do Contato",
		//				Email = "email@email.com",
		//				PrimaryPhone = "2121212121",
		//				SecondaryPhone= "2121212121",
		//				SupplierId = 1
		//			}
		//		},
		//		CreatedAt = DateTime.Now,
		//		UpdatedAt = DateTime.Now,
		//		IsActive = true
		//	};

		//	listSuppliers = new List<Supplier>()
		//	{
		//		supplier,
		//		new Supplier()
		//		{
		//			Id = 1,
		//			Name = "Fornecedor 2",
		//			CpfCnpj = "1111111111111",
		//			Facebook = "face.com",
		//			Instagram = "insta.com",
		//			Note = "Note",
		//			Site = "site.com",
		//			Contacts = new List<Contact>() {
		//				new Contact() {
		//					Id = 1,
		//					Name = "Nome do Contato",
		//					Area = "Area do Contato",
		//					Email = "email@email.com",
		//					PrimaryPhone = "2121212121",
		//					SecondaryPhone= "2121212121",
		//					SupplierId = 1
		//				}
		//			},
		//			SupplierDepartments = new List<SupplierDepartment>() { new SupplierDepartment() { DepartmentId = 2, SupplierId = 1 } },
		//			CreatedAt = DateTime.Now,
		//			UpdatedAt = DateTime.Now,
		//			IsActive = false
		//		}
		//	};

		//	//listSupplierTypes = new List<SupplierType>()
		//	//{
		//	//	new SupplierType()
		//	//	{
		//	//		Id = 1, 
		//	//		Name = "Tipo de Forne",
		//	//		Stores = 
		//	//	}
		//	//}

		//}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		//private SupplierQueryHandler GetSupplierQueryHandler()
		//{
		//	return new SupplierQueryHandler(supplierRepository.Object,
		//									supplierProductRepository.Object,
		//									supplierSegmentRepository.Object,
		//									supplierDepartmentRepository.Object,
		//									supplierTypeRepository.Object,
		//									paggingConfiguration.Object,
		//									userProvider.Object);

		//}
	}
}
