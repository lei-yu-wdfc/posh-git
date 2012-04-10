using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture]
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
                              GetPersonalPaymentCardsQuery query = new GetPersonalPaymentCardsQuery() { AccountId = customer.Id };
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