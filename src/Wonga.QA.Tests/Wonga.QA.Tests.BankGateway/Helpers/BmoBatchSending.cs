using System;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.Tests.BankGateway.Helpers
{
	public class BmoBatchSending : IDisposable
	{
        private readonly string _configKey = "BankGateway.Bmo.FileTransferTimes";
		private readonly string _originalSchedule;

		public BmoBatchSending()
		{
			// Pause Cash-out schedule
			var driver = Drive.Db.Ops;
            var config = driver.ServiceConfigurations.First(sc => sc.Key == _configKey);
			_originalSchedule = config.Value;
			config.Value = DateTime.UtcNow.AddHours(2).TimeOfDay.ToString();
			driver.SubmitChanges();
		}

		public void Dispose()
		{
			// Restore cash-out schedule
			var driver = Drive.Db.Ops;
            var config = driver.ServiceConfigurations.First(sc => sc.Key == _configKey);
			config.Value = _originalSchedule;
			driver.SubmitChanges();

			var batchSaga = Do.Until(() => Drive.Db.OpsSagasCa.SendBmoPaymentSagaEntities.First());
			Drive.Msmq.BankGatewayBmo.Send(new TimeoutMessage { SagaId = batchSaga.Id });
		}
	}
}