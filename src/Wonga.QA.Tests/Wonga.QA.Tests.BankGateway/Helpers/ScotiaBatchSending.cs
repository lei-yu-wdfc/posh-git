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
	    private readonly string _originalSchedule;

        private readonly dynamic _opsSagasCa = Drive.Data.OpsSagas.Db.SendScotiaPaymentSagaEntity;

		public ScotiaBatchSending()
		{
            _originalSchedule = Drive.Db.GetServiceConfiguration(_configKey).Value;
            Drive.Db.SetServiceConfiguration(_configKey, DateTime.UtcNow.AddHours(2).TimeOfDay.ToString("%h")+":00");
		}

		public void Dispose()
		{
            Drive.Db.SetServiceConfiguration(_configKey, _originalSchedule);

            var batchSaga = Do.Until(() => _opsSagasCa.All().First());
			Drive.Msmq.BankGatewayScotia.Send(new TimeoutMessage { SagaId = batchSaga.Id });
		}
	}
}