using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
//using Wonga.QA.Framework.Api.Exceptions;
//using Wonga.QA.Framework.Api.Requests;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    class GetApplicationAprTest
    {
        [Test, AUT(AUT.Uk), JIRA("UK-1122"), Owner(Owner.DenisRyzhkov), Pending("Commented out until query is added to the API")]
        public void GetApplicationAprWihtNullPromoCode()
        {
            //decimal apr = 28.7m;
            //Guid custId = Guid.Parse("5C09C656-721A-4CDD-8EF8-410CC5343DE3");
            //var cust = new Customer(custId);

            //Guid aplId = Guid.Parse("148F9ED2-C75F-470B-A8E1-CAAC83A4F1EB");
            //var apl = new Application(aplId);

            //ApiResponse parm =
            //    Drive.Api.Queries.Post(new GetApplicationAprQuery { MerchantId = apl.Id, PromoCodeId = null, TotalValue = 1000.0m });

            //Assert.IsNotNull(parm);
            //Assert.AreEqual(apr, decimal.Parse(parm.Values["Apr"].Single()));
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1122"), Owner(Owner.DenisRyzhkov), Pending("Commented out until query is added to the API")]
        public void GetApplicationAprWihtCorrectPromoCode()
        {
        //    decimal apr = 28.7m;
        //    Guid custId = Guid.Parse("5C09C656-721A-4CDD-8EF8-410CC5343DE3");
        //    var cust = new Customer(custId);

        //    Guid aplId = Guid.Parse("148F9ED2-C75F-470B-A8E1-CAAC83A4F1EB");
        //    var apl = new Application(aplId);

        //    ApiResponse promo = Drive.Api.Queries.Post(new GetPersonalPromoCodeQuery { AccountId = cust.Id, });

        //    Guid promoId = Guid.Parse(promo.Values["PromoCode"].Single());

        //    ApiResponse parm = Drive.Api.Queries.Post(new GetApplicationAprQuery { MerchantId = apl.Id, PromoCodeId = promoId, TotalValue = 1000.0m });

        //    Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(new GetApplicationAprQuery { MerchantId = apl.Id, PromoCodeId = promoId, TotalValue = 0.0m }));

        }

        [Test, AUT(AUT.Uk), JIRA("UK-1122"), Owner(Owner.DenisRyzhkov), Pending("Commented out until query is added to the API")]
        public void GetApplicationAprIncorrectPromoCode()
        {
        //    Guid aplId = Guid.Parse("148F9ED2-C75F-470B-A8E1-CAAC83A4F1EB");
        //    var apl = new Application(aplId);

        //    Guid promoId = Guid.Parse("148F9ED2-C75F-470B-A8E1-CAAC83A4F1EB");

        //    ApiResponse parm = Drive.Api.Queries.Post(new GetApplicationAprQuery { MerchantId = apl.Id, PromoCodeId = promoId, TotalValue = 1000.0m });

        //    Assert.AreEqual("Payments_PromoCode_Invalid", parm.Values["Apr"].Single());
        }
    }
}
