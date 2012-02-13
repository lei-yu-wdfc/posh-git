using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Svc;

namespace Wonga.QA.Framework
{
    public static class Driver
    {
        public static ApiDriver Api { get { return new ApiDriver(); } }
        public static SvcDriver Svc { get { return new SvcDriver(); } }
        public static MsmqDriver Msmq { get { return new MsmqDriver(); } }
        public static DbDriver Db { get { return new DbDriver(); } }
    }
}
