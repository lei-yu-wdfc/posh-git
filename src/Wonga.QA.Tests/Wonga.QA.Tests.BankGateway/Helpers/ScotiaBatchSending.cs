using System;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.Tests.BankGateway.Helpers
{
	public class ScotiaBatchSending : IDisposable
	{
        private readonly string _configKey = "BankGateway.Scotiabank.FileTransferTimes";
		private readonly string _originalSchedule;

	    private readonly dynamic _serviceConfig = Drive.Data.Ops.Db.ServiceConfigurations;
	    private readonly dynamic _opsSagasCa = Drive.Data.OpsSagas.Db.SendScotiaPaymentSagaEntity;

		public ScotiaBatchSending()
		{
			// Pause Cash-out schedule
            var config = _serviceConfig.FindBy(_configKey);
			_originalSchedule = config.Value;
			config.Value = DateTime.UtcNow.AddHours(2).TimeOfDay.ToString();
            _serviceConfig.SubmitChanges();
		}

		public void Dispose()
		{
			// Restore cash-out schedule
            var config = _serviceConfig.FindBy(_configKey);
			config.Value = _originalSchedule;
            _serviceConfig.SubmitChanges();

            var batchSaga = Do.Until(() => _opsSagasCa.FindAll.First());
			Drive.Msmq.BankGatewayScotia.Send(new TimeoutMessage { SagaId = batchSaga.Id });
		}
	}
}