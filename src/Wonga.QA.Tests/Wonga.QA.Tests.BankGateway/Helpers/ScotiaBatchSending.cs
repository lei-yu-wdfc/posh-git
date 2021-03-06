using System;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.Tests.BankGateway.Helpers
{
	public class ScotiaBatchSending : IDisposable
	{
	    private const string _configKey = "BankGateway.Scotiabank.FileTransferTimes";

        private readonly dynamic _opsSagasCa = Drive.Data.OpsSagas.Db.SendScotiaPaymentSagaEntity;

		public ScotiaBatchSending()
		{
            Drive.Data.Ops.SetServiceConfiguration(_configKey, DateTime.UtcNow.AddHours(2).TimeOfDay.ToString("%h") + ":00");
		}

		public void Dispose()
		{
            Drive.Data.Ops.SetServiceConfiguration(_configKey, string.Empty);

            var batchSaga = Do.Until(() => _opsSagasCa.All().First());
			Drive.Msmq.BankGatewayScotia.Send(new TimeoutMessage { SagaId = batchSaga.Id });
		}
	}
}