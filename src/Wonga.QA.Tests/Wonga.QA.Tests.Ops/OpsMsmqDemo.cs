using System;
using System.Data.Linq;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq.Ops;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ops
{
    public class OpsMsmqDemo
    {
        [Test, AUT]
        public void CreateAccountMessage()
        {
            CreateAccountCommand message = new CreateAccountCommand
            {
                AccountId = Data.GetId(),
                Login = Data.GetEmail(),
                Password = Data.GetPassword()
            };

            Drivers.Msmq.Ops.Send(message);

            Thread.Sleep(1000);

            AccountEntity account = Drivers.Db.Ops.Accounts.Where(a => a.ExternalId == message.AccountId).Single();
            Assert.AreEqual(message.AccountId, account.ExternalId);
        }
    }
}
