//using Moq;
//using FluentAssertions;
//using System.Threading;
//using System.Threading.Tasks;
//using TVGlobo.R2P.Parceria.Application.Queries;
//using TVGlobo.R2P.Parceria.Application.Services.Query;
//using TVGlobo.R2P.Parceria.Domain.Entities;
//using TVGlobo.R2P.Parceria.Domain.Interfaces;
//using TVGlobo.R2P.Parceria.Domain.Models;
//using Xunit;
//using System.Collections.Generic;

namespace TVGlobo.R2P.Parceria.Tests.Application.Services.Query
{
	/// <summary>
	/// 
	/// </summary>
	public class UserQueryHandlerTest
	{
		///// <summary>
		///// 
		///// </summary>
		//private readonly Mock<IRepository<User>> userRepository;

  //      /// <summary>
		///// 
		///// </summary>
		//private readonly Mock<IRepository<UserProgram>> userProgramRepository;

  //      /// <summary>
		///// 
		///// </summary>
		//private readonly Mock<PaggingConfiguration> paggingConfiguration;

  //      private User user;

        //private UserProgram userProgram;

        /// <summary>
        /// 
        /// </summary>
        public UserQueryHandlerTest()
		{
			//userRepository = new Mock<IRepository<User>>();
   //         userProgramRepository = new Mock<IRepository<UserProgram>>();
   //         paggingConfiguration = new Mock<PaggingConfiguration>();

   //         user = new User()
			//{
			//	Email = "tcordeiro@thera.com.br",
			//	IsActive = true,
			//	LastName = "Santos Cordeiro",
			//	Login = "tscord",
			//	Name = "Thiago",
   //             UserPrograms = GetUserPrograms("tscord")
   //         };

			//userRepository.Setup(r => r.GetByLogin("tscord", CancellationToken.None))
			//	.Returns(Task.FromResult(user));

   //         paggingConfiguration.Object.PerPage = 20;

        }

		///// <summary>
		///// 
		///// </summary>
		//[Fact]
		//public void Test_GetUserByLogin()
		//{
		//	//arg
		//	var query = new GetByLogin()
		//	{
		//		CancellationToken = CancellationToken.None,
		//		Login = "tscord"
		//	};

		//	//act
		//	IQueryHandlerAsync<GetByLogin, User> userHandler = GetUserQueryHandler();
		//	var task = userHandler.RetrieveAsync(query);

		//	task.Wait();

		//	var user = task.Result;

		//	//asset
		//	user.Should().BeOfType<User>().Which.Login.Should().Be("tscord");
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//private UserQueryHandler GetUserQueryHandler()
		//{
		//	return new UserQueryHandler(userRepository.Object, userProgramRepository.Object, paggingConfiguration.Object);
		//}

  //      /// <summary>
  //      /// 
  //      /// </summary>
  //      /// <returns></returns>
  //      private List<UserProgram> GetUserPrograms(string login)
  //      {
  //          List<UserProgram> progList = new List<UserProgram>();

  //          userProgram = new UserProgram()
  //          {
  //              Login = login,
  //              ProductCd = 9900
  //          };

  //          progList.Add(userProgram);

  //          return progList;
  //      }
    }
}
