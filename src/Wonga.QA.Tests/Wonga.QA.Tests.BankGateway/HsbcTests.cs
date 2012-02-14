using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
    [Parallelizable(TestScope.All)]
    public class HsbcTests
    {
        [Test, AUT(AUT.Uk), JIRA("UK-494")]
        public void SortCodeUpdates()
        {

        }
    }
}
