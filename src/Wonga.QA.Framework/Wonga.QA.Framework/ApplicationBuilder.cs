﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Za;
using Wonga.QA.Framework.Api.Requests.Risk.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Framework.Old;
using CreateFixedTermLoanApplicationUkCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.Uk.CreateFixedTermLoanApplicationUkCommand;
using CreateFixedTermLoanApplicationCaCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.Ca.CreateFixedTermLoanApplicationCaCommand;
using CreateFixedTermLoanApplicationZaCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.Za.CreateFixedTermLoanApplicationZaCommand;
using RiskCreateFixedTermLoanApplicationCommand = Wonga.QA.Framework.Api.Requests.Risk.Commands.RiskCreateFixedTermLoanApplicationCommand;
using SignApplicationCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.SignApplicationCommand;
using SubmitApplicationBehaviourCommand = Wonga.QA.Framework.Api.Requests.Risk.Commands.SubmitApplicationBehaviourCommand;
using SubmitClientWatermarkCommand = Wonga.QA.Framework.Api.Requests.Risk.Commands.SubmitClientWatermarkCommand;
using SubmitUidAnswersCommand = Wonga.QA.Framework.Api.Requests.Risk.Commands.SubmitUidAnswersCommand;
using VerifyFixedTermLoanCommand = Wonga.QA.Framework.Api.Requests.Risk.Commands.VerifyFixedTermLoanCommand;

namespace Wonga.QA.Framework
{
    public class ApplicationBuilder
    {
        protected Guid Id;
        protected Customer Customer;
        protected decimal LoanAmount;
        protected Date PromiseDate;
        protected ApplicationDecisionStatus? Decision = ApplicationDecisionStatus.Accepted;
        protected int LoanTerm;
        //protected IovationMockResponse IovationBlackBox;
        protected string IovationBlackBox;
        protected Dictionary<int, List<bool>> EidSessionInteraction = new Dictionary<int, List<bool>>();

        //WB specific members
        protected List<CustomerBuilder> Guarantors;
        protected Boolean SignGuarantors;
        protected Boolean CreateGuarantors;
        protected Boolean WithSigning=true;

        protected Action _setPromiseDateAndLoanTerm;
        private Func<int> _getDaysUntilStartOfLoan;

        //PromoCode specific members
        protected Guid? PromoCodeId;
        protected decimal? TransmissionFeeDiscount;

        #region Private Members

        private int GetLoanTermFromPromiseDate()
        {
            return (int)
                   (PromiseDate.DateTime.Subtract(DateTime.Today).TotalDays -
                    _getDaysUntilStartOfLoan());
        }

        private Date GetPromiseDateFromLoanTerm(int loanTerm)
        {
            return DateTime.Today.AddDays(loanTerm + _getDaysUntilStartOfLoan()).
                ToDate(DateFormat.Date);
        }

        private static string GetFailedCheckpointFromApplicationDecisionResponse(ApiResponse response)
        {
            return response != null ? response.Values["FailedCheckpoint"].FirstOrDefault() : null;
        }

        private int GetDaysUntilStartOfLoan()
        {
            return 0;
        }

        private static String UserActionId(String xmlString)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            var findUserActionId = xmlDoc.SelectNodes(string.Format("//UserActionId"));
            return findUserActionId[0].FirstChild.Value;
        }

        private static String AnswerEidQuestionsAccordingToEidSessionInteraction(String xmlString, int eidSessionNumber, Dictionary<int, List<bool>> eidSessionInteraction)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            var eidAnswers = string.Empty;

            var allUidQuestions = xmlDoc.SelectNodes(string.Format("//UidQuestion"));

            for (var questionNumber = 1; questionNumber <= allUidQuestions.Count; questionNumber++)
            {
                var question = xmlDoc.SelectNodes(string.Format("//UidQuestion[Id={0}]/Text", questionNumber));
                var answer = EidAnswers.GetEidAnswer(question[0].InnerText);

                var allAnswers = xmlDoc.SelectNodes(string.Format("//UidQuestion[Id={0}]/Answers/UidAnswer", questionNumber));

                for (var answerNumber = 1; answerNumber <= allAnswers.Count; answerNumber++)
                {
                    var possibleAnswer = xmlDoc.SelectNodes(string.Format("//UidQuestion[Id={0}]/Answers/UidAnswer[AnswerId={1}]/Text", questionNumber, answerNumber));
                    if ((possibleAnswer[0].InnerText).Equals(answer) || (possibleAnswer[0].InnerText).Equals("NONE OF THE ABOVE"))
                    {
                        if (eidSessionInteraction[eidSessionNumber][questionNumber - 1])
                            eidAnswers = eidAnswers + ParseAnswerToXmlString(questionNumber, answerNumber);
                        else
                            eidAnswers = eidAnswers + ParseAnswerToXmlString(questionNumber, getWrongAnswer(answerNumber));
                        break;
                    }
                }
            }

            return eidAnswers;
        }

        private static int getWrongAnswer(int answerNumber)
        {
            return answerNumber < 5 ? answerNumber + 1 : answerNumber - 1;
        }

        private static String ParseAnswerToXmlString(int questionNumber, int answerNumber)
        {
            return
                @"<UidQuestionAnswer>
                            <QuestionId>" + questionNumber + "</QuestionId>" +
                "<AnswerId>" + answerNumber + "</AnswerId>" +
                "</UidQuestionAnswer> ";
        }

        #endregion

        protected ApplicationBuilder()
        {
            Id = Guid.NewGuid();
            LoanAmount = Get.GetLoanAmount();
            IovationBlackBox = IovationMockResponse.Unknown.ToString();

            _setPromiseDateAndLoanTerm = () =>
            {
                PromiseDate = Get.GetPromiseDate();
                LoanTerm = GetLoanTermFromPromiseDate();
            };

            _getDaysUntilStartOfLoan = GetDaysUntilStartOfLoan;
        }

        // Migration tests override
        protected ApplicationBuilder(Date promiseDate, decimal loanAmount)
        {
            Id = Guid.NewGuid();
            LoanAmount = loanAmount;
            IovationBlackBox = IovationMockResponse.Unknown.ToString();

            _setPromiseDateAndLoanTerm = () =>
            {
                PromiseDate = promiseDate;
                LoanTerm = GetLoanTermFromPromiseDate();
            };

            _getDaysUntilStartOfLoan = GetDaysUntilStartOfLoan;
        }

        public static ApplicationBuilder New(Customer mainApplicant)
        {
            return new ApplicationBuilder { Customer = mainApplicant };
        }

        //method needed for migration testing

        public static ApplicationBuilder New(Customer mainApplicant, Date promiseDate, decimal loanAmount)
        {
            return new ApplicationBuilder(promiseDate,loanAmount) { Customer = mainApplicant};
        }

        public static ApplicationBuilder New(Customer mainApplicant, Organisation company)
        {
            return new BusinessApplicationBuilder(mainApplicant, company);
        }

        public virtual Application Build()
        {
            if (Config.AUT == AUT.Wb)
            {
                throw new NotImplementedException(
                    "WB product should be using factory method with organization parameter");
            }

            _setPromiseDateAndLoanTerm();
            PromiseDate.DateFormat = DateFormat.Date;

            var requests = new List<ApiRequest>
            {
                SubmitApplicationBehaviourCommand.New(r => r.ApplicationId = Id),
                SubmitClientWatermarkCommand.New(r =>
                {
                    r.ApplicationId = Id;
                    r.AccountId = Customer.Id;
                    r.BlackboxData = IovationBlackBox.ToString();
                })
            };

            switch (Config.AUT)
            {
                case AUT.Uk:
                    //wait for the card to be ready
                    var card = Customer.GetPaymentCard();
                    requests.AddRange(new ApiRequest[]
                    {
                        CreateFixedTermLoanApplicationUkCommand.New(r =>
                        {
                            r.ApplicationId = Id;
                            r.AccountId = Customer.Id;
                            r.BankAccountId = Customer.GetBankAccount();
                            r.PaymentCardId = card;
                            r.LoanAmount = LoanAmount;
                            r.PromiseDate = PromiseDate;
                            r.PromoCodeId = PromoCodeId;
                            r.TransmissionFeeDiscount = TransmissionFeeDiscount;
                        }),
                        RiskCreateFixedTermLoanApplicationCommand.New(r =>
                        {
                            r.ApplicationId = Id;
                            r.AccountId = Customer.Id;
                            r.BankAccountId = Customer.GetBankAccount();
                            r.LoanAmount = LoanAmount;
                            r.PromiseDate = PromiseDate;
                            r.PaymentCardId = card;
                        }),
                        VerifyFixedTermLoanCommand.New(r =>
                        {
                            r.AccountId = Customer.Id;
                            r.ApplicationId = Id;
                        })
                    });
                    break;

                case AUT.Ca:
                    // Start of Loan is different for CA
                    _getDaysUntilStartOfLoan = GetDaysUntilStartOfLoanForCa;
                    _setPromiseDateAndLoanTerm();

                    requests.AddRange(new ApiRequest[]
                    {
                        CreateFixedTermLoanApplicationCaCommand.New(r =>
                        {
                            r.ApplicationId = Id;
                            r.AccountId = Customer.Id;
                            r.BankAccountId = Customer.GetBankAccount();
                            r.LoanAmount = LoanAmount;
                            r.PromiseDate = PromiseDate;
                            r.Province = Customer.Province;
                        }),
                        RiskCreateFixedTermLoanApplicationCommand.New(r =>
                        {
                            r.ApplicationId = Id;
                            r.AccountId = Customer.Id;
                            r.BankAccountId = Customer.GetBankAccount();
                            r.LoanAmount = LoanAmount;
                            r.PromiseDate = PromiseDate;
                        }),
                        VerifyFixedTermLoanCommand.New(r =>
                        {
                            r.AccountId = Customer.Id;
                            r.ApplicationId = Id;
                        })
                    });
                    break;

                case AUT.Za:
                    requests.AddRange(new ApiRequest[]
                    {
                        CreateFixedTermLoanApplicationZaCommand.New(r =>
                        {
                            r.ApplicationId = Id;
                            r.AccountId = Customer.Id;
                            r.BankAccountId = Customer.GetBankAccount();
                            r.LoanAmount = LoanAmount;
                            r.PromiseDate = PromiseDate;
                        }),
                        RiskCreateFixedTermLoanApplicationCommand.New(r =>
                        {
                            r.ApplicationId = Id;
                            r.AccountId = Customer.Id;
                            r.BankAccountId = Customer.GetBankAccount();
                            r.LoanAmount = LoanAmount;
                            r.PromiseDate = PromiseDate;
                        }),
                        VerifyFixedTermLoanCommand.New(r =>
                        {
                            r.AccountId = Customer.Id;
                            r.ApplicationId = Id;
                        })
                    });
                    break;

                default:
                    throw new NotImplementedException();
            }

            Drive.Api.Commands.Post(requests);

            switch (Config.AUT)
            {
                case AUT.Ca:
                    foreach (var keyValuePair in EidSessionInteraction)
                    {
                        Do.Until(
                            () =>
                            (ApplicationDecisionStatus)
                            Enum.Parse(typeof (ApplicationDecisionStatus),
                                       Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = Id }).
                                           Values
                                           ["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Pending);

                        Do.Until(() => Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = Id }).Values["SequenceId"].SingleOrDefault() == keyValuePair.Key.ToString());

                        var xmlString =
                            (Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = Id }).Body);
                        xmlString = xmlString.Replace("xmlns=\"http://www.wonga.com/api/3.0\"", "");

                        var userActionId = UserActionId(xmlString);

                        var eidAnswers = AnswerEidQuestionsAccordingToEidSessionInteraction(xmlString, keyValuePair.Key, EidSessionInteraction);

                        Drive.Api.Commands.Post(new SubmitUidAnswersCommand { Answers = eidAnswers, UserActionId = userActionId });
                    }
                    break;
            }

            //Need to wait for risk app to be created - this will solve the RISK_APPLICATION_NOT_FOUND . SingleOrDefault != null DOES NOT WORK
            Do.With.Timeout(2).Message("The RiskApplication entity was not created").Until(() => Drive.Data.Risk.Db.RiskApplications.FindAllBy(ApplicationId: Id).Count() > 0);

            ApiResponse response = null;
            Do.With.Timeout(3).Until(() => (ApplicationDecisionStatus)
                                           Enum.Parse(typeof (ApplicationDecisionStatus), (response = Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = Id })).Values["ApplicationDecisionStatus"].Single()) == Decision
                                           || Decision == null);

            switch (Decision)
            {
                case null:
                    Do.With.Timeout(TimeSpan.FromSeconds(20)).Until(() => Drive.Db.GetCheckpointDefinitionsForApplication(Id).Count() > 0);
                    Do.With.Timeout(TimeSpan.FromSeconds(20)).Until(() => Drive.Db.GetVerificationDefinitionsForApplication(Id).Count() > 0);
                    return new Application(Id);

                case ApplicationDecisionStatus.Pending:
                    return new Application(Id);

                case ApplicationDecisionStatus.Declined:
                    return new Application(Id, GetFailedCheckpointFromApplicationDecisionResponse(response));
            }

            if(!WithSigning)
                return new Application { Id = Id, BankAccountId = Customer.BankAccountId, LoanAmount = LoanAmount, LoanTerm = LoanTerm, BankAccountNumber = Customer.BankAccountNumber };

            Drive.Api.Commands.Post(new SignApplicationCommand { AccountId = Customer.Id, ApplicationId = Id });


            ApiRequest summary = Config.AUT == AUT.Za
                                     ? new GetAccountSummaryZaQuery { AccountId = Customer.Id }
                                     : (ApiRequest) new GetAccountSummaryQuery { AccountId = Customer.Id };


            Do.Until(() => Drive.Api.Queries.Post(summary).Values["HasCurrentLoan"].Single() == "true");

            Do.With.Timeout(TimeSpan.FromSeconds(10)).Watch(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id).Transactions.Count);

            return new Application { Id = Id, BankAccountId = Customer.BankAccountId, LoanAmount = LoanAmount, LoanTerm = LoanTerm, BankAccountNumber = Customer.BankAccountNumber };
        }

        #region Public Helper "With" Methods

        public ApplicationBuilder WithLoanAmount(decimal loanAmount)
        {
            LoanAmount = loanAmount;
            return this;
        }

        public ApplicationBuilder WithPromiseDate(Date promiseDate)
        {
            _setPromiseDateAndLoanTerm = () =>
            {
                PromiseDate = promiseDate;
                LoanTerm = GetLoanTermFromPromiseDate();
            };

            return this;
        }

        public ApplicationBuilder WithLoanTerm(int loanTerm)
        {
            _setPromiseDateAndLoanTerm = () =>
            {
                LoanTerm = loanTerm;
                PromiseDate = GetPromiseDateFromLoanTerm(loanTerm);
            };

            return this;
        }

        public ApplicationBuilder WithEidSessionInteraction(Dictionary<int, List<bool>> EidSessionInteraction)
        {
            this.EidSessionInteraction = EidSessionInteraction;
            return this;
        }

        public ApplicationBuilder WithExpectedDecision(ApplicationDecisionStatus decision)
        {
            Decision = decision;
            return this;
        }

        public ApplicationBuilder WithoutExpectedDecision()
        {
            Decision = null;
            return this;
        }

        public ApplicationBuilder WithIovationBlackBox(IovationMockResponse iovationBlackBox)
        {
            return WithIovationBlackBox(iovationBlackBox.ToString());
        }

        public ApplicationBuilder WithIovationBlackBox(string iovationBlackBox)
        {
            IovationBlackBox = iovationBlackBox;
            return this;
        }

        public ApplicationBuilder WithOutSigning()
        {
            WithSigning = false; 
            return this;
        }
        public ApplicationBuilder WithPromoCode(Guid promoCodeId)
        {
            PromoCodeId = promoCodeId;
            return this;
        }

        public ApplicationBuilder WithTransmissionFeeDiscount(decimal transmissionFeeDiscount)
        {
            TransmissionFeeDiscount = transmissionFeeDiscount;
            return this;
        }

        #endregion

        private int GetDaysUntilStartOfLoanForCa()
        {
            return DateHelper.GetNumberOfDaysUntilStartOfLoanForCa();
        }
    }
}