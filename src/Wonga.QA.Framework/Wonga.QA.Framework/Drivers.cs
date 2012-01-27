using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Db;

namespace Wonga.QA.Framework
{
    public static class Drivers
    {
        private static ApiDriver _api;
        private static DbDriver _db;

        public static ApiDriver Api
        {
            get { return _api ?? (_api = new ApiDriver()); }
            set { _api = value; }
        }

        public static DbDriver Db
        {
            get { return _db ?? (_db = new DbDriver()); }
            set { _db = value; }
        }
    }
}
