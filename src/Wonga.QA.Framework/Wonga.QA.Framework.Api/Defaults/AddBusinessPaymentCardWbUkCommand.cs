﻿using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class AddBusinessPaymentCardWbUkCommand
    {
        public override void Default()
        {
            CardType = "VISA DEBIT";
            ExpiryDate = DateTime.Now.AddYears(4).ToDate(DateFormat.YearMonth);
            HolderName = Data.GetName();
            IsCreditCard = false;
            IsPrimary = false;
            IssueNo = 123;
            Number = 4444333322221111;
            OrganisationId = Data.GetId();
            PaymentCardId = Data.GetId();
            SecurityCode = 123;
            StartDate = DateTime.Now.AddYears(-2).ToDate(DateFormat.YearMonth);
        }
    }
}