using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Payments.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Risk.Commands;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Risk.Queries;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.MigrationTests.Utils
{
    internal static class ScorecardHelper
    {
        internal static IEnumerable<V2RequestFile> GetDirectoryFiles()
        {
            var directory = new DirectoryInfo(@"C:\\test_files");
            var fileList = directory.GetFiles("*.log");
            return (from file in fileList
                    where file.Length > 0
                    select new V2RequestFile { FileName = file.Name, XmlTextContent = ReadContentsOfFile(file) }).ToList();
        }
        internal static T DeserializeFromXml<T>(String xmlContent)
        {
            xmlContent = xmlContent.Replace("Category: Trace, Priority: 0, ClientIP: , ClientName: ", "");
            T returnedXmlClass = default(T);

            using (TextReader reader = new StringReader(xmlContent))
            {
                try
                {
                    returnedXmlClass = (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                }
                catch (InvalidOperationException)
                {
                }
            }

            return returnedXmlClass;
        }
        internal static Guid RunV3LnJourney(cde_request v2CdeRequest)
        {
            //Note:Find out the relating accountId of the existing customer? Maybe a new private method?
            Guid accountId = Guid.NewGuid();

            //Note:Get the existing migrated customer payment card id
            Guid paymentCardId = Guid.NewGuid();

            //Note:Get the existing migrated customer bank accountId
            Guid bankAccountId = Guid.NewGuid();

            //Note:Replace the nulls with values from V2 request
            Guid applicationId = Guid.NewGuid();
            var listOfCommands = new List<ApiRequest>
                                     {
                                         SubmitApplicationBehaviourCommand.New(command =>
                                                                                   {
                                                                                       command.ApplicationId = applicationId;
                                                                                       command.TermSliderPosition = null;
                                                                                       command.AmountSliderPosition = null;
                                                                                   }),
                                         SubmitClientWatermarkCommand.New(command =>
                                                                              {
                                                                                  command.AccountId = null;
                                                                                  command.ApplicationId = applicationId;
                                                                                  command.ClientIPAddress = null;
                                                                                  command.BlackboxData = null;
                                                                              }),
                                         CreateFixedTermLoanApplicationUkCommand.New(command =>
                                                                                         {
                                                                                             command.AccountId = accountId;
                                                                                             command.AffiliateId = null;
                                                                                             command.ApplicationId = applicationId;
                                                                                             command.BankAccountId = bankAccountId;
                                                                                             command.Currency = CurrencyCodeEnum.GBP;
                                                                                             command.LoanAmount = null;
                                                                                             command.PaymentCardId = paymentCardId;
                                                                                             command.PromiseDate = null;
                                                                                             command.PromoCodeId = null;
                                                                                         }),
                                         RiskCreateFixedTermLoanApplicationCommand.New(command =>
                                                                                           {
                                                                                               command.AccountId = accountId;
                                                                                               command.ApplicationId = applicationId;
                                                                                               command.BankAccountId = bankAccountId;
                                                                                               command.Currency = CurrencyCodeEnum.GBP;
                                                                                               command.LoanAmount = null;
                                                                                               command.PaymentCardId = paymentCardId;
                                                                                               command.PromiseDate = null;
                                                                                           }),
                                         VerifyFixedTermLoanCommand.New(command =>
                                                                            {
                                                                                command.AccountId = accountId;
                                                                                command.ApplicationId = applicationId;
                                                                            })
                                     };
            //Note: Post the commands to the API + log the responses

            //Note: Wait for the Risk Entity to be created
            Do.With.Timeout(2).Message("The RiskApplication entity was not created").Until(() => Drive.Data.Risk.Db.RiskApplications.FindAllBy(ApplicationId: applicationId).Count() > 0);

            //Note: Get application decision other then Pending - We do not expect only Accepted
            Do.With.Timeout(3).Until(() => (ApplicationDecisionStatus)
                                            Enum.Parse(typeof(ApplicationDecisionStatus), ( Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = applicationId })).Values["ApplicationDecisionStatus"].Single()) != ApplicationDecisionStatus.Pending);


            //Note: Sign the application? SignApplicationCommand
            Drive.Api.Commands.Post(new SignApplicationCommand()
                                        {
                                            AccountId = accountId,
                                            ApplicationId = applicationId,

                                        });
            

            return applicationId;
        }


        private static String ReadContentsOfFile(FileSystemInfo file)
        {
            return File.ReadAllText(file.FullName);
        }
    }
}
