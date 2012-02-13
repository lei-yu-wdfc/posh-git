using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    public class PaymentsApiTests
    {
        [Test, AUT(AUT.Uk, AUT.Za, AUT.Ca), Parallelizable]
        public void CreateFixTermLoanApplciation()
        {
            Customer customer = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(customer).Build();
            application.Repay();
        }
    }
}
