using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Data;
using Wonga.QA.Framework.Data.Enums.Risk;

namespace Wonga.QA.Framework.Data
{
    public class RiskDatabase : QAFDatabase
    {
        public RiskDatabase(string connectionString):base(connectionString)
        {
            
        }

        public dynamic GetWorkflowsForApplication(Guid applicationId)
        {
           return Db.RiskWorkflows.FindAllByApplicationId(applicationId);
        }
    }
}
