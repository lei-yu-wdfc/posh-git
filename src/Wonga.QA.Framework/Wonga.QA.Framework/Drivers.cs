using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.Framework
{
    public static class Drivers
    {
        private static ApiDriver _api;
        private static MsmqDriver _msmq;
        private static DbDriver _db;

        public static ApiDriver Api
        {
            get { return _api ?? (_api = new ApiDriver()); }
            set { _api = value; }
        }

        public static MsmqDriver Msmq
        {
            get { return _msmq ?? (_msmq = new MsmqDriver()); }
            set { _msmq = value; }
        }

        public static DbDriver Db
        {
            get { return _db ?? (_db = new DbDriver()); }
            set { _db = value; }
        }
    }
}
