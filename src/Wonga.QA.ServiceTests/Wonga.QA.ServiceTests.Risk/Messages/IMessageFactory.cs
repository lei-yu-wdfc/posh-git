using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.ServiceTests.Risk
{
	public interface IMessageFactory
	{
		MsmqMessage MsmqMessage { get; }
		void Initialise();
		void ApplyDefaults();
	}
}