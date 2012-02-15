using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using MbUnit.Framework;

namespace Wonga.QA.Tests.Journeys
{
    public class ApiJourneys
    {
        [Test]
        public void ApiL0Journey()
        {
            Customer cust = CustomerBuilder.New().Build();
            if (Config.AUT == AUT.Wb)
            {
                Organisation comp = OrganisationBuilder.New(cust).Build();
                Application app = ApplicationBuilder.New(cust, comp).Build();
            }
            else
            {
                Application app = ApplicationBuilder.New(cust).Build();
            }
        }
    }
}
