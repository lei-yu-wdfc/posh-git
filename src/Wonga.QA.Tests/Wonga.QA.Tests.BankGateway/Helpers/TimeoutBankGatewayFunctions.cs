using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.Tests.BankGateway.Helpers
{
    public static class TimeoutBankGatewayFunctions
    {
        private const string ScotiaCashIn = "BankGateway.Scotiabank.CashInBatchQueueId";
        private const string ScotiaCashOut = "BankGateway.Scotiabank.CashOutBatchQueueId";

        public static void TimeoutScotiaCashOut()
        {
            var scotiaCashOutQueue = new Guid(Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == ScotiaCashOut).Value);
            Driver.Msmq.BankGateway.Send(new TimeoutMessage { SagaId = scotiaCashOutQueue });
        }

        public static void TimeoutScotiaCashIn()
        {
            var scotiaCashInQueue = new Guid(Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == ScotiaCashIn).Value);
            Driver.Msmq.BankGateway.Send(new TimeoutMessage { SagaId = scotiaCashInQueue });
        }
    }
}
