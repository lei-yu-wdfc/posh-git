using System;

namespace Wonga.QA.MigrationCore
{
    public class MigratedUser
    {
        public Guid AccountId { get; set; }
        public String Login { get; set; }
        public String MigratedRunId { get; set; }
        public dynamic Password { get; set; }
    }
}
