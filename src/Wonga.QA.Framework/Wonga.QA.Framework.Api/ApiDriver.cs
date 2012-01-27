using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public class ApiDriver
    {
        private ApiEndpoint _commands;
        private ApiEndpoint _queries;

        public ApiEndpoint Commands
        {
            get { return _commands ?? (_commands = new ApiEndpoint(Config.Api.Commands)); }
            set { _commands = value; }
        }

        public ApiEndpoint Queries
        {
            get { return _queries ?? (_queries = new ApiEndpoint(Config.Api.Queries)); }
            set { _queries = value; }
        }
    }
}
