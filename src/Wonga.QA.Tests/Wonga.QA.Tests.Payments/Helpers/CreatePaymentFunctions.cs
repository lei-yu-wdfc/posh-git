using System;
using Wonga.QA.Framework.Db.Payments;

namespace Wonga.QA.Tests.Payments.Helpers
{
    public static class CreatePaymentFunctions
    {
        public static TransactionEntity CreateRowOfTypeTransaction()
        {
            var currentTimeStamp = DateTime.Today;

            return new TransactionEntity()
            {
                TransactionId = 0000,
                ExternalId = new Guid("00000000-0000-0000-0000-000000000000"),
                ApplicationId = 333,
                PostedOn = currentTimeStamp,
                Scope = 0,
                Type = null,
                Amount = (decimal)0.00,
                Mir = null,
                Currency = 0,
                ComponentTransactionId = new Guid("00000000-0000-0000-0000-000000000000"),
                Reference = null,
                CreatedOn = currentTimeStamp,
                UserId = null
            };

        }

        public static VariableInterestRateDetailEntity CreateRowOfTypeVariableInterestRateDetail()
        {
            return new VariableInterestRateDetailEntity()
            {
                VariableInterestRateDetailId = 0,
                VariableInterestRateId = 1,
                Day = 0,
                MonthlyInterestRate = 0
            };
        }
    }
}
