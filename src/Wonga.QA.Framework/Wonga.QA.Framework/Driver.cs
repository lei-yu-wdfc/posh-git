using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Svc;
using Wonga.QA.Framework.ThirdParties;

namespace Wonga.QA.Framework
{
    public static class Driver
    {
        public static ApiDriver Api { get { return new ApiDriver(); } }
        public static CsDriver Cs { get { return new CsDriver(); } }
        public static SvcDriver Svc { get { return new SvcDriver(); } }
        public static MsmqDriver Msmq { get { return new MsmqDriver(); } }
        public static DbDriver Db { get { return new DbDriver(); } }
        public static ThirdPartyDriver ThirdParties { get { return new ThirdPartyDriver(); } }
    }
}
