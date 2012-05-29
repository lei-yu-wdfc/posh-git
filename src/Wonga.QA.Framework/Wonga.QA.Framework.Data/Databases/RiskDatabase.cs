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

        public List<Guid> GetWorkflowsForApplication(Guid applicationId,RiskWorkflowTypes riskWorkflowType)
        {
           return Db.SelectAllByApplicationId(applicationId).ToList();
        }
    }
}
