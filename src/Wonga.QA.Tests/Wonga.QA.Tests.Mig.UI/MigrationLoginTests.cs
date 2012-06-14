using MbUnit.Framework;
using Wonga.QA.Tests.Ui;

namespace Wonga.QA.Tests.Migration
{
    public class MigrationLoginTests:UiTest
    {
        [Test]
        public void LoginTest()
        {
            Client.Login().LoginAs("claire_coe@lycos.co.uk", "kieran14");
        }
    }
}
