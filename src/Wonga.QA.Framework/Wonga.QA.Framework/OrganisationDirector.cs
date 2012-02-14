using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework
{
    public class OrganisationDirector
    {
        protected Guid _accountId;

        public OrganisationDirector(Guid accountId)
        {
            _accountId = accountId;
        }
    }
}
