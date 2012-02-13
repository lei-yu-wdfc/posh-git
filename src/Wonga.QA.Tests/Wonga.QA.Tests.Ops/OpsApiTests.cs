using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ops
{
    [AUT]
    public class OpsApiTests
    {
        [Test, AUT, Parallelizable]
        public void CreateAndGetAccount()
        {
            ApiDriver api = new ApiDriver();

            CreateAccountCommand command = CreateAccountCommand.New();
            api.Commands.Post(command);

            ApiResponse response = Do.Until(() => api.Queries.Post(new GetAccountQuery
            {
                Login = command.Login,
                Password = command.Password
            }));

            Assert.AreEqual(command.AccountId, Guid.Parse(response.Values["AccountId"].Single()));
        }
    }
}
