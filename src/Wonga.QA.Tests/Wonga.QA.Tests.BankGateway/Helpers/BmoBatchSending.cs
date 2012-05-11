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
        private readonly string _configKey = "BankGateway.Bmo.FileTransferTimes";
		private readonly string _originalSchedule;

        private readonly dynamic _opsSagasCa = Drive.Data.OpsSagas.Db.SendBmoPaymentSagaEntity;

		public BmoBatchSending()
		{
            _originalSchedule = Drive.Data.Ops.GetServiceConfiguration<string>(_configKey);
            Drive.Data.Ops.SetServiceConfiguration(_configKey, DateTime.UtcNow.AddHours(2).TimeOfDay.ToString("%h") + ":00");
		}

		public void Dispose()
		{
            Drive.Data.Ops.SetServiceConfiguration(_configKey, _originalSchedule);

            var batchSaga = Do.Until(() => _opsSagasCa.All().First());
            Drive.Msmq.BankGatewayBmo.Send(new TimeoutMessage { SagaId = batchSaga.Id });
		}
	}
}