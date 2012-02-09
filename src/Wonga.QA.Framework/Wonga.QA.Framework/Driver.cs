using System;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.Framework
{
    /*public class Driver
    {
        private ApiDriver _api;
        private MsmqDriver _msmq;
        private DbDriver _db;

        public ApiDriver Api
        {
            get { return _api ?? (_api = new ApiDriver()); }
            set { _api = value; }
        }

        public MsmqDriver Msmq
        {
            get { return _msmq ?? (_msmq = new MsmqDriver()); }
            set { _msmq = value; }
        }

        public DbDriver Db
        {
            get { return _db ?? (_db = new DbDriver()); }
            set { _db = value; }
        }

        public static DbDriver Db { get; set; }
    }*/

    public static class Driver
    {
        public static ApiDriver Api { get { return new ApiDriver(); } }
        public static MsmqDriver Msmq { get { return new MsmqDriver(); } }
        public static DbDriver Db { get { return new DbDriver(); } }
    }
}
