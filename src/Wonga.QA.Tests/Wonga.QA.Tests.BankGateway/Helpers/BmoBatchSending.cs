using System;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.Tests.BankGateway.Helpers
{
	public class BmoBatchSending : IDisposable
	{
		private const string _configKey = "BankGateway.Bmo.FileTransferTimes";

		private readonly dynamic _opsSagasCa = Drive.Data.OpsSagas.Db.SendBmoPaymentSagaEntity;

		public BmoBatchSending()
		{
            Drive.Data.Ops.SetServiceConfiguration(_configKey, DateTime.UtcNow.AddHours(2).TimeOfDay.ToString("%h") + ":00");
		}

		public void Dispose()
		{
            Drive.Data.Ops.SetServiceConfiguration(_configKey, string.Empty);

            var batchSaga = Do.Until(() => _opsSagasCa.All().First());
            Drive.Msmq.BankGatewayBmo.Send(new TimeoutMessage { SagaId = batchSaga.Id });
		}
	}
}