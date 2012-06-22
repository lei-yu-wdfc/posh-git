using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Queries;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture]
    [Parallelizable(TestScope.Self)]
    [AUT(AUT.Uk), JIRA("UK-1103")]
    public class CardRepaymentRequestCsApiTests
    {
        private Guid _cardId;
        private decimal _loanAmount;

        [SetUp]
        public void Setup()
        {
            _cardId = Guid.NewGuid();
            _loanAmount = 202.72m;
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1196")]
        public void RepayWithCard_CreatesCardPaymentRequestEntry()
        {
            Customer customer = CustomerBuilder.New().Build();

            const string cardType = "Mastercard";
            const string cardNumber = "1111222233334444";
            const string cardCode = "777";
            DateTime expiryDate = DateTime.UtcNow.AddMonths(5);
            customer.AddPaymentCard(cardType, cardNumber, cardCode, expiryDate, false);

            int? paymentCardId = null;
            Do.Until(() =>
                         {
                             var paymentCardsData = Drive.Data.Payments.Db.PaymentCardsBase;
                             var d = paymentCardsData.FindAll(paymentCardsData.Type == cardType 
                                 && paymentCardsData.MaskedNumber == cardNumber.MaskedCardNumber()).Single();
                             paymentCardId = d.PaymentCardId;
                             return d;
            });

            Application application = ApplicationBuilder.New(customer).WithLoanAmount(_loanAmount).Build();

            Guid paymentId = Guid.NewGuid();

            var repayCommand = new CsRepayWithPaymentCardCommand()
                                   {
                                       AccountId = customer.Id,
                                       Amount = _loanAmount,
                                       SalesforceUser = "Mr Agent",
                                       Currency = CurrencyCodeIso4217Enum.GBP,
                                       CV2 = cardCode,
                                       PaymentCardId = _cardId,
                                       PaymentId = paymentId
                                   };

            Drive.Cs.Commands.Post(repayCommand);

            Do.Until(()=>
                         {
                             var repaymentRequests = Drive.Data.Payments.Db.PaymentCardRepaymentRequests;
                             var result = repaymentRequests.FindAll(repaymentRequests.ApplicationId == application.Id
                                                                    && repaymentRequests.Amount == _loanAmount
                                                                    && repaymentRequests.PaymentCardId == paymentCardId
                                                                    && repaymentRequests.ExternalId == paymentId);
                             return result;
                         });
        }
    }


    [TestFixture]
    [Parallelizable(TestScope.Self)]
    public class GetPersonalPaymentCardsCsapiQueryTests
    {
        [Test, AUT(AUT.Uk), JIRA("UK-1194")]
        public void Query_ShouldReturnAllPersonalPaymentCards_WhenPersonalCardsPresentForAccount()
        {
            DateTime today = DateTime.Today;
            DateTime lastDayOfThisMonth = new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);

            var cardsToAdd = new[]
                                 {
                                     new
                                         {
                                             Type = "Mastercard",
                                             Number = "4444333322221111",
                                             SecurityCode = "222",
                                             ExpiryDate = lastDayOfThisMonth.AddMonths(7),
                                             IsPrimary = true,
                                         },
                                     new
                                         {
                                             Type = "Visa Debit",
                                             Number = "4444333322221111",
                                             SecurityCode = "227",
                                             ExpiryDate = lastDayOfThisMonth.AddMonths(20),
                                             IsPrimary = false,
                                         }
                                 };

            Customer customer = CustomerBuilder.New().Build();

            foreach (var card in cardsToAdd)
            {
                customer.AddPaymentCard(card.Type, card.Number, card.SecurityCode, card.ExpiryDate, card.IsPrimary);
            }

            PersonalPaymentCardEntity[] cards = customer.GetPersonalPaymentCards();
            Do.Until(() =>
                          {
                              var query = new GetPersonalPaymentCardsQuery() { AccountId = customer.Id };
                              CsResponse response = Drive.Cs.Queries.Post(query);
                              if (response == null) return false;
                              if (response.Values["Account"].Any(v => v != customer.Id.ToString())) return false;
                              foreach (var addedCard in cards)
                              {
                                  if (!(response.Values["CardType"].Any(v => v == addedCard.PaymentCardsBaseEntity.Type))) return false;
                                  if (!(response.Values["MaskedNumber"].Any(v => v == addedCard.PaymentCardsBaseEntity.MaskedNumber))) return false;
                                  if (!(response.Values["ExpiryDate"].Any(v => DateTime.Parse(v) == addedCard.PaymentCardsBaseEntity.ExpiryDate))) return false;
                              }
                              return true;
                          });
        }
    }
}