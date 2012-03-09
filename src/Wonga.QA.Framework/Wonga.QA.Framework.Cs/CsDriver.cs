using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Cs
{
    public class CsDriver
    {
        private CsEndpoint _commands;
        private CsEndpoint _queries;

        public CsEndpoint Commands
        {
            get { return _commands ?? (_commands = new CsEndpoint(Config.Cs.Commands)); }
            set { _commands = value; }
        }

        public CsEndpoint Queries
        {
            get { return _queries ?? (_queries = new CsEndpoint(Config.Cs.Queries)); }
            set { _queries = value; }
        }
    }
}
