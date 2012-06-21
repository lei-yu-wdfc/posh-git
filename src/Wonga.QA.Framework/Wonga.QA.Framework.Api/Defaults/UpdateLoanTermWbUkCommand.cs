using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.Wb.Uk
{
    public partial class UpdateLoanTermWbUkCommand
    {
        public override void Default()
        {
            Term = 25; //make the default here different from the term in CreateBusinessFixedInstallmentLoanApplicationWbUkCommand
        }
    }
}
