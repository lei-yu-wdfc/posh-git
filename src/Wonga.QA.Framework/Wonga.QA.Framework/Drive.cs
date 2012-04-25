using System;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Data;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Email;
using Wonga.QA.Framework.Mocks;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Svc;
using Wonga.QA.Framework.ThirdParties;

namespace Wonga.QA.Framework
{
    public static class Drive
    {
        public static ApiDriver Api { get { return new ApiDriver(); } }
        public static CsDriver Cs { get { return new CsDriver(); } }        
        public static DbDriver Db { get { return new DbDriver(); } }
        public static SvcDriver Svc { get { return new SvcDriver(); } }
        public static MsmqDriver Msmq { get { return new MsmqDriver(); } }
        public static ThirdPartyDriver ThirdParties { get { return new ThirdPartyDriver(); } }
        public static MockDriver Mocks { get { return new MockDriver(); } }
        public static DataDriver Data { get { return new DataDriver(); } }
        public static EmailDriver Email {get {return new EmailDriver(); }}
    }
}
