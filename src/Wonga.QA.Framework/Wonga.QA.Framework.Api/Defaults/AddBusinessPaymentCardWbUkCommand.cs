using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class AddBusinessPaymentCardWbUkCommand
    {
        public override void Default()
        {
            CardType = "VISA DEBIT";
            ExpiryDate = DateTime.Now.AddYears(4).ToString("yyyy-MM");
            HolderName = Data.GetName();
            IsCreditCard = "0";
            IsPrimary = "0";
            IssueNo = "123";
            Number = "4444333322221111";
            OrganisationId = Data.GetId();
            PaymentCardId = Data.GetId();
            SecurityCode = "123";
            StartDate = DateTime.Now.AddYears(-2).ToString("yyyy-MM");
        }
    }
}
