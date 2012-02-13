using Wonga.QA.Framework.Core;
using System;

namespace Wonga.QA.Framework.Api
{
    public partial class AddPaymentCardCommand
    {
        public override void Default()
        {
            AccountId = Data.GetId();
            PaymentCardId = Data.GetId();
            CardType = "VISA DEBIT";
            Number = 4444333322221111;
            HolderName = Data.GetName();
            StartDate = DateTime.Now.AddYears(-2).ToDate(DateFormat.YearMonth);
            ExpiryDate = DateTime.Now.AddYears(4).ToDate(DateFormat.YearMonth);
            IsPrimary = true;
            IssueNo = 123;
            IsCreditCard = false;
            SecurityCode = 123;
        }
    }
}
