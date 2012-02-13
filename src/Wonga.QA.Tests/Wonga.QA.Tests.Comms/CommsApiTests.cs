using System;
using MbUnit.Framework;
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
            ApiDriver api = new ApiDriver();
            Guid id = Guid.NewGuid();

            ApiRequest command;
            switch (Config.AUT)
            {
                /*case AUT.Uk:
                    command = SaveCustomerDetailsUkCommand.New(r => r.AccountId = id);
                    break;*/
                case AUT.Za:
                    command = SaveCustomerDetailsZaCommand.New(r =>
                    {
                        r.AccountId = id;
                        r.NationalNumber = Data.GetNIN((Date)r.DateOfBirth, (GenderEnum)r.Gender == GenderEnum.Female);
                        r.MaidenName = (GenderEnum)r.Gender == GenderEnum.Female ? r.MaidenName : null;
                    });
                    break;
                /*case AUT.Ca:
                    command = SaveCustomerDetailsCaCommand.New(r => r.AccountId = id);
                    break;*/
                default:
                    throw new NotImplementedException();
            }

            api.Commands.Post(command);
            api.Queries.Post(new GetCustomerDetailsQuery() { AccountId = id });
        }
    }
}
