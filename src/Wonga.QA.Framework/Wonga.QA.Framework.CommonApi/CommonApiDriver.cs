using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.CommonApi
{
    public class CommonApiDriver
    {
        private CommonApiEndpoint _commands;

        public CommonApiEndpoint Commands
        {
            get { return _commands ?? (_commands = new CommonApiEndpoint(Config.CommonApi.Commands)); }
            set { _commands = value; }
        }
    }
}
