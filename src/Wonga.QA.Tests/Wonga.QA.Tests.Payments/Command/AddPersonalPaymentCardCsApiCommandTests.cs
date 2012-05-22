using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Tests.Payments.Command
{
    [Parallelizable(TestScope.Self)]
    [TestFixture]
    public class AddPersonalPaymentCardCsApiCommandTests
    {
        [Test] 
        [Description("Adds a collection of payments cards using AddPersonalPaymentCard Cs API command and verifies all of these cards have been added to users account")]
        [AUT(AUT.Uk)]
        public void Command_AddsPersonalPaymentCards_ToCustomersPaymentsCards()
        {
            DateTime today = DateTime.Today;
            DateTime lastDayOfThisMonth = new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);

            var customer = CustomerBuilder.New().Build();
            var cardsToAdd = new[]
                                 {
                                     new
                                         {
                                             Type = "Visa Debit",
                                             Number = "1111222233334444",
                                             SecurityCode = "222",
                                             ExpiryDate = lastDayOfThisMonth.AddMonths(7),
                                             IsPrimary = true,
                                             IsCreditCard = false,
                                             HolderName="Jane Doe"
                                         },
                                     new
                                         {
                                             Type = "American Express",
                                             Number = "4444333322221113",
                                             SecurityCode = "227",
                                             ExpiryDate = lastDayOfThisMonth.AddMonths(19),
                                             IsPrimary = false,
                                             IsCreditCard = false,
                                             HolderName="John Smith"
                                         }
                                 };
            var requests = cardsToAdd.Select(card => new AddPersonalPaymentCardCommand()
                                        {
                                            AccountId = customer.Id, 
                                            CardType = card.Type,
                                            ExpiryDate = card.ExpiryDate.ToString("yyyy-MM"),
                                            IsPrimary = card.IsPrimary,
                                            Number = card.Number,
                                            SecurityCode = card.SecurityCode,
                                            IsCreditCard = card.IsCreditCard,
                                            HolderName = card.HolderName
                                        });
            Drive.Cs.Commands.Post(requests);
            Do.Until(() =>
                         {
                             PersonalPaymentCardEntity[] customerCards = customer.GetPersonalPaymentCards();
                             foreach (var cardToAdd in cardsToAdd)
                             {
                                 var c = customerCards
                                     .SingleOrDefault(p => p.AccountId == customer.Id &&
                                                           //Comparing expiry dates doesn't work properly if the current month has 31 days and the expiry date month has 30. Hence this...
                                                           p.PaymentCardsBaseEntity.ExpiryDate.Month == cardToAdd.ExpiryDate.Month &&
                                                           p.PaymentCardsBaseEntity.ExpiryDate.Year == cardToAdd.ExpiryDate.Year && 
                                                           p.PaymentCardsBaseEntity.MaskedNumber == cardToAdd.Number.MaskedCardNumber() && 
                                                           p.PaymentCardsBaseEntity.Type == cardToAdd.Type);
                                 if (c == null) return false;
                             }
                             return true;
                         });
        }
    }
}