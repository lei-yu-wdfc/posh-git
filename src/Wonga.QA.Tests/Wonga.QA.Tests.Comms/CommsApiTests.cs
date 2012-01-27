using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
    public class CommsApiTests
    {
        [Test, AUT(AUT.Uk, AUT.Za, AUT.Ca)]
        public void SaveAndGetCustomerDetails()
        {
            Guid id = Guid.NewGuid();

            ApiRequest command;
            switch (Config.AUT)
            {
                case AUT.Uk:
                    command = SaveCustomerDetailsUkCommand.Random(r => r.AccountId = id);
                    break;
                case AUT.Za:
                    command = SaveCustomerDetailsZaCommand.Random(r =>
                    {
                        r.AccountId = id;
                        r.NationalNumber = Data.GetNIN((DateTime)r.DateOfBirth, (GenderEnum)r.Gender);
                        r.MaidenName = (GenderEnum)r.Gender == GenderEnum.Female ? r.MaidenName : null;
                    });
                    break;
                case AUT.Ca:
                    command = SaveCustomerDetailsCaCommand.Random(r => r.AccountId = id);
                    break;
                default:
                    throw new NotImplementedException();
            }

            Drivers.Api.Commands.Post(command);
            Drivers.Api.Queries.Post(new GetCustomerDetailsQuery() { AccountId = id });
        }
    }
}
