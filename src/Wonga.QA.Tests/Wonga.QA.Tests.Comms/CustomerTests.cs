using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
    [TestFixture]
    [Parallelizable(TestScope.All)]
    public class CustomerTests
    {
        private CustomerBuilder _customerBuilder;
        private Customer _customer;
        private Guid _customerGuid;

        [SetUp, AUT]
        public void Setup()
        {
            _customerGuid = Guid.NewGuid();

            _customerBuilder = CustomerBuilder.New(_customerGuid)
                .WithCountyInAddress("MyCounty")
                .WithDistrictInAddress("MyDistrict")
                .WithFlatInAddress("MyFlat")
                .WithHouseNameInAddress("MyHouse")
                .WithHouseNumberInAddress("MyHouseNumber")
                .WithMiddleName("John")
                .WithStreetInAddress("MyStreet")
                .WithTownInAddress("MyTown");

            _customer = _customerBuilder.Build();

        }

        [Test]
        public void TestAddressOverridesCorrectlySaved()
        {
            var AddressEntity = Driver.Db.Comms.Addresses.Single(a => a.AccountId == _customer.Id);
            Assert.AreEqual(AddressEntity.Street, "MyStreet");
            Assert.AreEqual(AddressEntity.County, "MyCounty");
            Assert.AreEqual(AddressEntity.District, "MyDistrict");
            Assert.AreEqual(AddressEntity.Flat, "MyFlat");
            Assert.AreEqual(AddressEntity.HouseName, "MyHouse");
            Assert.AreEqual(AddressEntity.HouseNumber, "MyHouseNumber");
            Assert.AreEqual(AddressEntity.Town, "MyTown");
            
        }
    }
}
