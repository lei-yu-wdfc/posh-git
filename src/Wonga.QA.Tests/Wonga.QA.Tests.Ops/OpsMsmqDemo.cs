using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Msmq.Ops;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ops
{
    public class OpsMsmqDemo
    {
        [Test, AUT]
        public void CreateAccountMessage()
        {
            CreateAccountCommand command = new CreateAccountCommand
            {
                AccountId = Data.GetId(),
                Login = Data.GetEmail(),
                Password = Data.GetPassword(),
            };

            Driver.Msmq.Ops.Send(command);

            AccountEntity account = Do.Until(() => Driver.Db.Ops.Accounts.Single(a => a.ExternalId == command.AccountId));
            Assert.AreEqual(command.Login, account.Login);
        }
    }
}
