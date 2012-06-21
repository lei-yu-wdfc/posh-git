using System;
using System.Diagnostics;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.ServiceTests.Risk
{
	[TestFixture]
	public abstract class ServiceTestBase
	{
		protected string CARD_EXPIRY_DATE_FORMAT;

		#region IDs
		protected Guid ApplicationId { get; private set; }
		protected virtual void GenerateIds()
		{
			ApplicationId = Guid.NewGuid();
			Console.WriteLine("Application ID: " + ApplicationId);
			Debug.WriteLine("Application ID: " + ApplicationId);
		}
		#endregion

		
		#region Message 

		protected virtual void InitialiseCommands()
		{
			Messages = new MessageFactoryCollection();
		}

		protected void Send(params MsmqMessage[] msgs)
		{
			msgs.ForEach(x => Drive.Msmq.Risk.Send(x));
		}

		protected MessageFactoryCollection Messages { private set; get; }

		protected void SendAllMessages()
		{
			Send(Messages);
		}
		#endregion

		#region Test Settings

		protected DateTime TestAsOf { get; set; }

		#endregion

		[SetUp]
		public void SetUp()
		{
			GenerateIds();
			InitialiseCommands();

			BeforeEachTest();
		}

		protected virtual void BeforeEachTest()
		{
			TestAsOf = DateTime.Now;
		}
	}
}