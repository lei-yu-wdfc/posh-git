using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Cs
{
	public class UcgDriver
	{
		private ApiEndpoint _commands;
		private ApiEndpoint _queries;

		public ApiEndpoint Commands
		{
			get { return _commands ?? (_commands = new ApiEndpoint(Config.Ucg.Commands)); }
			set { _commands = value; }
		}

		public ApiEndpoint Queries
		{
			get { return _queries ?? (_queries = new ApiEndpoint(Config.Ucg.Queries)); }
			set { _queries = value; }
		}
	}
}
