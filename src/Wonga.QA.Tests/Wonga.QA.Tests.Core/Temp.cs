using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;


namespace Wonga.QA.Tests.Core
{
    class Temp
    {
        [Test, AUT(AUT.Wb, AUT.Uk)]
        public void CreateL0Application()
        {
            var customer = CustomerBuilder.New().Build();

            switch (Config.AUT)
            {
                case AUT.Wb:
                    {
                        var company = CompanyBuilder.New(customer).Build();
                        ApplicationBuilder.New(customer, company).Build();
                    }
                    break;
                default:
                    ApplicationBuilder.New(customer).Build();
                    break;
            }
        }
    }
}
