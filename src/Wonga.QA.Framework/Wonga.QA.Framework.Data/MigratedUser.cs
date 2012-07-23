using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.Data
{
    public class MigratedUser
    {
        public Guid AccountId { get; set; }
        public String Login { get; set; }
        public String MigratedRunId { get; set; }
        public dynamic Password { get; set; }
    }
}
