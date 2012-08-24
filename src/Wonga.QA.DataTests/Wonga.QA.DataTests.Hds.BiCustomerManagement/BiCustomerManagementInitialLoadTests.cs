using MbUnit.Framework;
using Wonga.QA.DataTests.Hds.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.DataTests.Hds.BiCustomerManagement
{
    [TestFixture(Order = 1)]
    [Owner(Owner.JagjitThind)]
    [Category("HDSTest")]
    [Category("InitialLoad")]
    [Category("BiCustomerManagement")]
    [Parallelizable(TestScope.All)]
    public class BiCustomerManagementInitialLoadTests
    {
        [Test]
        [Description("Run BiCustomerManagement initial load and confirm that it succeeds")]
        public void RunInitialLoadAndConfirmThatItSucceeds()
        {
            InitialLoad initialLoad = new InitialLoad();

            initialLoad.RunInitialLoadAndConfirmThatItSucceeds(HdsUtilitiesBase.WongaService.BiCustomerManagement);
        }
    }
}