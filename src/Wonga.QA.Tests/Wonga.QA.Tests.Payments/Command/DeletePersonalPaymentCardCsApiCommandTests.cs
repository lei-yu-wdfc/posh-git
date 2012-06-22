using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Command
{
    [TestFixture]
    [AUT(AUT.Uk)]
    [Parallelizable(TestScope.Self)]
    public class DeletePersonalPaymentCardCsApiCommandTests
    {
		[Test, AUT(AUT.Uk)]
        [Description("Populates customer account with payment cards, deletes one of them using DeletePersonalPaymentCard Cs API" +
                     " command and then verifies that this card has been marked as deleted")]
        public void Command_DeletesPaymentCard_FromCustomersPaymentCards()
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
                                             Number = "1111222233334445",
                                             SecurityCode = "227",
                                             ExpiryDate = lastDayOfThisMonth.AddMonths(20),
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
            //populate user account with cards
            Drive.Cs.Commands.Post(requests);
            Do.Until(() =>
                         {
                             PersonalPaymentCardEntity[] customerCards = customer.GetPersonalPaymentCards();
                             foreach (var cardToAdd in cardsToAdd)
                             {
                                 var c = customerCards
                                     .SingleOrDefault(p => p.AccountId == customer.Id &&
                                                           // SH: The basic date comparison will fail if the current month has 30 days, but the calculated expiry date has 31.
                                                           p.PaymentCardsBaseEntity.ExpiryDate.Month == cardToAdd.ExpiryDate.Month &&
                                                           p.PaymentCardsBaseEntity.ExpiryDate.Year == cardToAdd.ExpiryDate.Year && 
                                                           p.PaymentCardsBaseEntity.MaskedNumber == cardToAdd.Number.MaskedCardNumber() && 
                                                           p.PaymentCardsBaseEntity.Type == cardToAdd.Type && 
                                                           p.PaymentCardsBaseEntity.DeletedOn == null);
                                 if (c == null) return false;
                             }
                             return true;
                         });
            PersonalPaymentCardEntity[] pCards = customer.GetPersonalPaymentCards();

            //Cannot delete the first card - it's the primary.. deleting the second.
            PersonalPaymentCardEntity firstCard = pCards.Where(p => p.PaymentCardsBaseEntity.MaskedNumber == cardsToAdd[1].Number.MaskedCardNumber()).Single();

            var deleteRequest = new DeletePersonalPaymentCardCommand()
                                    {
                                        PaymentCardId = firstCard.PaymentCardsBaseEntity.ExternalId,
                                    };
            //delete the card
            Drive.Cs.Commands.Post(deleteRequest);
            //check if the card has been marked as deleted
            Do.Until(() =>
                         {
                             var deletedCard = customer.GetPersonalPaymentCards()
                                 .Single(c=>c.PaymentCardsBaseEntity.ExternalId == (Guid)deleteRequest.PaymentCardId);
                             return deletedCard.PaymentCardsBaseEntity.DeletedOn != null;
                         });
        }
    }
}