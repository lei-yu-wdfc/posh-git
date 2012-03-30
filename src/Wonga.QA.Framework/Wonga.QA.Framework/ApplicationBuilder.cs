using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Helpers;

namespace Wonga.QA.Framework
{
    public class ApplicationBuilder
    {
        protected Guid Id;
        protected Customer Customer;
        protected decimal LoanAmount;
        protected Date PromiseDate;
        protected ApplicationDecisionStatus Decision = ApplicationDecisionStatus.Accepted;
        protected int LoanTerm;
        protected string IovationBlackBox;
        protected Dictionary<int, List<bool>> EidSessionInteraction = new Dictionary<int, List<bool>>();

        //WB specific members
        protected List<Customer> Guarantors;

        protected Action _setPromiseDateAndLoanTerm;
        private Func<int> _getDaysUntilStartOfLoan;

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
            IovationBlackBox = "Unknown";

            _setPromiseDateAndLoanTerm = () =>
                                  {
                                      PromiseDate = Get.GetPromiseDate();
                                      LoanTerm = GetLoanTermFromPromiseDate();
                                  };

            _getDaysUntilStartOfLoan = GetDaysUntilStartOfLoan;
        }

        public static ApplicationBuilder New(Customer mainApplicant)
        {
            return new ApplicationBuilder { Customer = mainApplicant };
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
                SubmitClientWatermarkCommand.New(r => { r.ApplicationId=Id; r.AccountId = Customer.Id;
                                                          r.BlackboxData = IovationBlackBox;
                })
            };

            switch (Config.AUT)
            {
                case AUT.Uk:
                    //wait for the card to be ready
                    Do.Until(Customer.GetPaymentCard);
                    requests.AddRange(new ApiRequest[]{
                        CreateFixedTermLoanApplicationCommand.New(r =>
                        {
                            r.ApplicationId = Id;
                            r.AccountId = Customer.Id;
                            r.BankAccountId = Customer.GetBankAccount();
                            r.PaymentCardId = Customer.GetPaymentCard();
                        	r.LoanAmount = LoanAmount;
                        	r.PromiseDate = PromiseDate;
                        }),
                        VerifyFixedTermLoanCommand.New(r=>
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

                    requests.AddRange(new ApiRequest[]{
                        CreateFixedTermLoanApplicationCommand.New(r =>
                        {
                            r.ApplicationId = Id;
                            r.AccountId = Customer.Id;
                            r.BankAccountId = Customer.GetBankAccount();
                            r.LoanAmount = LoanAmount;
                        	r.PromiseDate = PromiseDate;
                        }),
                        VerifyFixedTermLoanCommand.New(r=>
                        {
                            r.AccountId = Customer.Id; 
                            r.ApplicationId = Id;
                        })
                    });
                    break;

                default:
                    requests.AddRange(new ApiRequest[]{
                        CreateFixedTermLoanApplicationCommand.New(r =>
                        {
                            r.ApplicationId = Id;
                            r.AccountId = Customer.Id;
                            r.BankAccountId = Customer.GetBankAccount();
                        	r.LoanAmount = LoanAmount;
                        	r.PromiseDate = PromiseDate;
                        }),
                        VerifyFixedTermLoanCommand.New(r=>
                        {
                            r.AccountId = Customer.Id; 
                            r.ApplicationId = Id;
                        })
                    });
                    break;
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
                            Enum.Parse(typeof(ApplicationDecisionStatus),
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

            ApiResponse response = null;
            Do.With.Timeout(3).Until(() => (ApplicationDecisionStatus)
                Enum.Parse(typeof(ApplicationDecisionStatus), (response = Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = Id })).Values["ApplicationDecisionStatus"].Single()) == Decision);

            if (Decision == ApplicationDecisionStatus.Declined)
            {
                return new Application(Id, GetFailedCheckpointFromApplicationDecisionResponse(response));
            }

            Drive.Api.Commands.Post(new SignApplicationCommand { AccountId = Customer.Id, ApplicationId = Id });



            ApiRequest summary = Config.AUT == AUT.Za
                                     ? new GetAccountSummaryZaQuery { AccountId = Customer.Id }
                                     : (ApiRequest)new GetAccountSummaryQuery { AccountId = Customer.Id };


            Do.Until(() => Drive.Api.Queries.Post(summary).Values["HasCurrentLoan"].Single() == "true");

            Do.With.Timeout(TimeSpan.FromSeconds(10)).Watch(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id).Transactions.Count);

            return new Application { Id = Id, BankAccountId = Customer.BankAccountId, LoanAmount = LoanAmount, LoanTerm = LoanTerm };
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

        public ApplicationBuilder WithIovationBlackBox(string iovationBlackBox)
        {
            IovationBlackBox = iovationBlackBox;
            return this;
        }
        
        #endregion

		private int GetDaysUntilStartOfLoanForCa()
		{
			return DateHelper.GetNumberOfDaysUntilStartOfLoanForCa();
		}
    }
}
