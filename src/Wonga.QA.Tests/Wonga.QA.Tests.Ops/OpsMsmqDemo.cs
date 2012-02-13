using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Comms;
using Wonga.QA.Tests.Core;
using CreateAccountCommand = Wonga.QA.Framework.Msmq.Ops.CreateAccountCommand;

namespace Wonga.QA.Tests.Ops
{
    public class OpsMsmqDemo
    {
        [Test, AUT]
        public void CreateAccountCommand()
        {
            CreateAccountCommand command = new CreateAccountCommand
            {
                AccountId = Data.GetId(),
                Login = Data.GetEmail(),
                Password = Data.GetPassword(),
            };

            Driver.Msmq.Ops.Send(command);
            Do.Until(() => Driver.Db.Ops.Accounts.Single(a => a.ExternalId == command.AccountId));

            ApiResponse response = Driver.Api.Queries.Post(new GetAccountQuery { Login = command.Login, Password = command.Password });
            Assert.AreEqual(command.AccountId, Guid.Parse(response.Values["AccountId"].Single()));
        }

        [Test]
        public void Foo()
        {
            Driver.Msmq.FileStorage.Send(new Framework.Msmq.Comms.SaveFileCommand { FileId = Data.GetId(), SagaId = Data.GetId(), Content = new Byte[] { 42 } });
            //Driver.Msmq.FileStorage.Send(new Framework.Msmq.FileStorage.SaveFileCommand { FileId = Data.GetId(), Content = new Byte[] { 42 } });
        }
    }
}
