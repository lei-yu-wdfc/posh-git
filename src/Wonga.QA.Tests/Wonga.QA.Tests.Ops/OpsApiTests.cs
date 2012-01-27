using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ops
{
    public class OpsApiTests
    {
        [Test, AUT]
        public void CreateAndGetAccount()
        {
            CreateAccountCommand command = CreateAccountCommand.Random();
            Drivers.Api.Commands.Post(command);
            
            ApiResponse response = Drivers.Api.Queries.Post(new GetAccountQuery
            {
                Login = command.Login,
                Password = command.Password
            });

            Assert.AreEqual(command.AccountId, Guid.Parse(response.Values["AccountId"].Single()));
        }
    }
}
