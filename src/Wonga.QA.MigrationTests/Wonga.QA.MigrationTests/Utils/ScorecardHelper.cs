﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Ops.Queries;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Payments.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Risk.Commands;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Risk.Queries;
using Wonga.QA.Framework.Core;
using log4net.Config;

namespace Wonga.QA.MigrationTests.Utils
{
    internal static class ScorecardHelper
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal static IEnumerable<V2RequestFile> GetDirectoryFiles()
        {
            var inputPath = ConfigurationManager.AppSettings["inputDir"];
            var inputFileExtensionFilter = ConfigurationManager.AppSettings["inputFileExtension"];

            var directory = new DirectoryInfo(inputPath);
            var fileList = directory.GetFiles(inputFileExtensionFilter);
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
        internal static Guid RunV3LnJourneyFromV2LnCdeRequest(cde_request v2CdeRequest)
        {
            XmlConfigurator.Configure();
            Guid accountId = GetMigratedCustomerAccountId(v2CdeRequest.applicant_details[0].email_address,
                                                          v2CdeRequest.authentication[0].password);
            Logger.Info("------------------------------------------------------------------------------");
            Logger.InfoFormat("{0}: Running V3 for EmailAddress:{1} / Password:{2} / AccountId:{3}", DateTime.Now, v2CdeRequest.applicant_details[0].email_address, v2CdeRequest.authentication[0].password,accountId);

            Guid paymentCardId = GetMigratedUserPaymentCardId(accountId);
            Logger.InfoFormat("{0}: Found the following payment card Id: {1}", DateTime.Now,paymentCardId);

            Guid bankAccountId = GetMigratedUserBankAccountId(accountId);
            Logger.InfoFormat("{0}: Found the following bank account Id: {1}", DateTime.Now,bankAccountId);

            Guid applicationId = Guid.NewGuid();
            Logger.InfoFormat("{0}: Creating List of API commands for ApplicationId:{1}", DateTime.Now,applicationId);

            var listOfCommands = new List<ApiRequest>
                                     {
                                         SubmitApplicationBehaviourCommand.New(command =>
                                                                                   {
                                                                                       command.ApplicationId = applicationId;
                                                                                       command.TermSliderPosition = v2CdeRequest.proposal_details[0].cash_price;
                                                                                       command.AmountSliderPosition = v2CdeRequest.proposal_details[0].term;
                                                                                   }),
                                         SubmitClientWatermarkCommand.New(command =>
                                                                              {
                                                                                  command.AccountId = accountId;
                                                                                  command.ApplicationId = applicationId;
                                                                                  command.ClientIPAddress = v2CdeRequest.applicant_details[0].ip_address;
                                                                                  command.BlackboxData = v2CdeRequest.additional_information[0].Iovation[0].Result;
                                                                              }),
                                         CreateFixedTermLoanApplicationUkCommand.New(command =>
                                                                                         {
                                                                                             command.AccountId = accountId;
                                                                                             //command.AffiliateId = null;
                                                                                             command.ApplicationId = applicationId;
                                                                                             command.BankAccountId = bankAccountId;
                                                                                             command.Currency = CurrencyCodeEnum.GBP;
                                                                                             command.LoanAmount = v2CdeRequest.proposal_details[0].cash_price;
                                                                                             command.PaymentCardId = paymentCardId;
                                                                                             command.PromiseDate = v2CdeRequest.additional_information[0].pay_away_date;
                                                                                             //command.PromoCodeId = null;
                                                                                         }),
                                         RiskCreateFixedTermLoanApplicationCommand.New(command =>
                                                                                           {
                                                                                               command.AccountId = accountId;
                                                                                               command.ApplicationId = applicationId;
                                                                                               command.BankAccountId = bankAccountId;
                                                                                               command.Currency = CurrencyCodeEnum.GBP;
                                                                                               command.LoanAmount = v2CdeRequest.proposal_details[0].cash_price;
                                                                                               command.PaymentCardId = paymentCardId;
                                                                                               command.PromiseDate = v2CdeRequest.additional_information[0].pay_away_date;
                                                                                           }),
                                         VerifyFixedTermLoanCommand.New(command =>
                                                                            {
                                                                                command.AccountId = accountId;
                                                                                command.ApplicationId = applicationId;
                                                                            })
                                     };
            //Note: Post the commands to the API + log the responses
            foreach (var command in listOfCommands)
            {
                Logger.InfoFormat("{0}: Sending request for {1}", DateTime.Now, command);
                var response = Drive.Api.Commands.Post(command);
                Logger.InfoFormat("{0}: Receiving response for {1} - {2}", DateTime.Now, command,response);
            }

            //Note: Wait for the Risk Entity to be created / When created write to log file
            Do.With.Timeout(2).Message("The RiskApplication entity was not created").Until(() => Drive.Data.Risk.Db.RiskApplications.FindAllBy(ApplicationId: applicationId).Count() > 0);
            Logger.InfoFormat("{0}: Risk application created ", DateTime.Now);

            //Note: Get application decision other then Pending - We do not expect only Accepted
            Do.With.Timeout(3).Until(() => (ApplicationDecisionStatus)
                                            Enum.Parse(typeof(ApplicationDecisionStatus), (Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = applicationId })).Values["ApplicationDecisionStatus"].Single()) != ApplicationDecisionStatus.Pending);


            //Note: Sign the application
            var signApplicationCommand = new SignApplicationCommand()
                                             {
                                                 AccountId = accountId,
                                                 ApplicationId = applicationId,
                                             };
            Drive.Api.Commands.Post(signApplicationCommand);
            Logger.InfoFormat("{0}: Sending {1} ", DateTime.Now,signApplicationCommand);

            return applicationId;
        }

        private static Guid GetMigratedUserBankAccountId(Guid accountId)
        {
            var bankAccountId = Drive.Data.Payments.Db.PersonalBankAccounts.FindAllByAccountId(accountId).SingleOrDefault().BankAccountId;
            return Drive.Data.Payments.Db.BankAccountsBase.FindAllByBankAccountId(bankAccountId).Single().Single().ExternalId;
        }
        private static Guid GetMigratedUserPaymentCardId(Guid accountId)
        {
            Int32 primaryPaymentCardId =
                Drive.Data.Payments.Db.AccountPreferences.FindAllByAccountId(accountId).SingleOrDefault().
                    PrimaryPaymentCardId;

            return
                Drive.Data.Payments.Db.PaymentCardsBase.FindAllByPaymentCardId(primaryPaymentCardId).Single().ExternalId;
        }
        private static Guid GetMigratedCustomerAccountId(String email, String password)
        {
            return Guid.Parse
                (Drive.Api.Queries.Post
                     (new GetAccountQuery {Login = email, Password = password}).Values["AccountId"].Single());
        }
        private static String ReadContentsOfFile(FileSystemInfo file)
        {
            return File.ReadAllText(file.FullName);
        }

        internal static void GenerateCsv(Guid applicationId)
        {
            dynamic pmmlFactorsTable = Drive.Data.Risk.Db.PmmlFactors;
            dynamic factorsTable = Drive.Data.Risk.Db.Factors;
            dynamic riskWorkflowsTable = Drive.Data.Risk.Db.RiskWorkflows;
            var riskWorkflowId = riskWorkflowsTable.FindAllByApplicationId(applicationId).Single();

            dynamic accounts = pmmlFactorsTable.All()
                .Join(riskWorkflowsTable)
                .On(RiskWorkflowId: riskWorkflowsTable.RiskWorkflowId)
                .WithOne(riskWorkflowsTable)
                .Join(factorsTable)
                .On(FactorId: factorsTable.FactorId)
                .WithOne(factorsTable)
                .Where(riskWorkflowsTable.RiskWorkflowId == riskWorkflowId)
                .ToList();

            var outputPath = ConfigurationManager.AppSettings["outputDir"];
            var outputCsvFileName = ConfigurationManager.AppSettings["outputCsvFileName"];
            using (var writer = new WriteCsv(new DirectoryInfo(outputPath) + outputCsvFileName))
            {
                GenerateColumName(writer);
                foreach (var album in accounts)
                {
                    var row = new CsvRow
                                  {
                                      album.Data,
                                      Convert.ToString(album.FactorId),
                                      Convert.ToString(album.PmmlFactorId),
                                      Convert.ToString(album.RiskApplicationId),
                                      Convert.ToString(album.RiskWorkflowId)
                                  };
                    writer.WriteRow(row);
                }
            }
        }
        private static void GenerateColumName(WriteCsv writer)
        {
            var columName = new CsvRow {"Id", "FactorId", "PmmlFactorId", "RiskApplicationId", "RiskWorkflowId"};
            writer.WriteRow(columName);
        }
    }
}
