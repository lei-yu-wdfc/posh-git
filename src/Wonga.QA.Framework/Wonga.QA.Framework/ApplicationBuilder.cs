using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Helpers;

namespace Wonga.QA.Framework
{
    public class ApplicationBuilder
    {
        protected Guid _id;
        protected Customer _customer;
        protected decimal _loanAmount;
        protected Date _promiseDate;
        protected ApplicationDecisionStatusEnum _decision = ApplicationDecisionStatusEnum.Accepted;
        protected int _loanTerm;
        protected string _iovationBlackBox;
        protected Dictionary<int, List<bool>> _eidSessionInteraction = new Dictionary<int, List<bool>>();

        private Action _setPromiseDateAndLoanTerm;
        private Func<int> _getDaysUntilStartOfLoan;

        protected ApplicationBuilder()
        {
            _id = Guid.NewGuid();
            _loanAmount = Get.GetLoanAmount();
            _iovationBlackBox = "foobar";

            _setPromiseDateAndLoanTerm = () =>
                                  {
                                      _promiseDate = Get.GetPromiseDate();
                                      _loanTerm = GetLoanTermFromPromiseDate();
                                  };

            _getDaysUntilStartOfLoan = GetDaysUntilStartOfLoan;
        }

        private int GetLoanTermFromPromiseDate()
        {
            return (int)
                   (_promiseDate.DateTime.Subtract(DateTime.Today).TotalDays -
                    _getDaysUntilStartOfLoan());
        }

        public static ApplicationBuilder New(Customer customer)
        {
            return new ApplicationBuilder { _customer = customer };
        }

        public static ApplicationBuilder New(Customer customer, Organisation company)
        {
            return new BusinessApplicationBuilder(customer, company);
        }

        public ApplicationBuilder WithLoanAmount(decimal loanAmount)
        {
            _loanAmount = loanAmount;
            return this;
        }

        public ApplicationBuilder WithPromiseDate(Date promiseDate)
        {
            _setPromiseDateAndLoanTerm = () =>
                                  {
                                      _promiseDate = promiseDate;
                                      _loanTerm = GetLoanTermFromPromiseDate();
                                  };

            return this;
        }

        public ApplicationBuilder WithLoanTerm(int loanTerm)
        {
            _setPromiseDateAndLoanTerm = () =>
                                             {
                                                 _loanTerm = loanTerm;
                                                 _promiseDate = GetPromiseDateFromLoanTerm(loanTerm);
                                             };

            return this;
        }

        public ApplicationBuilder WithEidSessionInteraction(Dictionary<int, List<bool>> EidSessionInteraction)
        {
            _eidSessionInteraction = EidSessionInteraction;
            return this;
        }

        private Date GetPromiseDateFromLoanTerm(int loanTerm)
        {
            return DateTime.Today.AddDays(loanTerm + _getDaysUntilStartOfLoan()).
                ToDate(DateFormat.Date);
        }

        public ApplicationBuilder WithExpectedDecision(ApplicationDecisionStatusEnum decision)
        {
            _decision = decision;
            return this;
        }

        public ApplicationBuilder WithIovationBlackBox(string iovationBlackBox)
        {
            _iovationBlackBox = iovationBlackBox;
            return this;
        }

        public virtual Application Build()
        {
			
            if (Config.AUT == AUT.Wb)
            {
                throw new NotImplementedException(
                    "WB product should be using factory method with organization parameter");
            }

            _setPromiseDateAndLoanTerm();
			_promiseDate.DateFormat = DateFormat.Date;

            List<ApiRequest> requests = new List<ApiRequest>
            {
                SubmitApplicationBehaviourCommand.New(r => r.ApplicationId = _id),
                SubmitClientWatermarkCommand.New(r => { r.ApplicationId=_id; r.AccountId = _customer.Id;
                                                          r.BlackboxData = _iovationBlackBox;
                })
            };

            switch (Config.AUT)
            {
                case AUT.Uk:
                    //wait for the card to be ready
                    Do.Until(_customer.GetPaymentCard);
                    requests.AddRange(new ApiRequest[]{
                        CreateFixedTermLoanApplicationCommand.New(r =>
                        {
                            r.ApplicationId = _id;
                            r.AccountId = _customer.Id;
                            r.BankAccountId = _customer.GetBankAccount();
                            r.PaymentCardId = _customer.GetPaymentCard();
                        	r.LoanAmount = _loanAmount;
                        	r.PromiseDate = _promiseDate;
                        }),
                        VerifyFixedTermLoanCommand.New(r=>
                        {
                            r.AccountId = _customer.Id; 
                            r.ApplicationId = _id;
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
                            r.ApplicationId = _id;
                            r.AccountId = _customer.Id;
                            r.BankAccountId = _customer.GetBankAccount();
                            r.LoanAmount = _loanAmount;
                        	r.PromiseDate = _promiseDate;
                        }),
                        VerifyFixedTermLoanCommand.New(r=>
                        {
                            r.AccountId = _customer.Id; 
                            r.ApplicationId = _id;
                        })
                    });
                    break;

                default:
                    requests.AddRange(new ApiRequest[]{
                        CreateFixedTermLoanApplicationCommand.New(r =>
                        {
                            r.ApplicationId = _id;
                            r.AccountId = _customer.Id;
                            r.BankAccountId = _customer.GetBankAccount();
                        	r.LoanAmount = _loanAmount;
                        	r.PromiseDate = _promiseDate;
                        }),
                        VerifyFixedTermLoanCommand.New(r=>
                        {
                            r.AccountId = _customer.Id; 
                            r.ApplicationId = _id;
                        })
                    });
                    break;
            }
            
            Driver.Api.Commands.Post(requests);

            switch (Config.AUT)
            {
                case AUT.Ca:
                    foreach (var keyValuePair in _eidSessionInteraction)
                    {
                        Do.Until(
                            () =>
                            (ApplicationDecisionStatusEnum)
                            Enum.Parse(typeof(ApplicationDecisionStatusEnum),
                                        Driver.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = _id }).
                                            Values
                                            ["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatusEnum.Pending);

                        Do.Until(() => Driver.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = _id }).Values["SequenceId"].SingleOrDefault() == keyValuePair.Key.ToString());

                        var xmlString =
                            (Driver.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = _id }).Body);
                        xmlString = xmlString.Replace("xmlns=\"http://www.wonga.com/api/3.0\"", "");

                        var userActionId = UserActionId(xmlString);

                        var eidAnswers = AnswerEidQuestionsAccordingToEidSessionInteraction(xmlString, keyValuePair.Key, _eidSessionInteraction);

                        Driver.Api.Commands.Post(new SubmitUidAnswersCommand { Answers = eidAnswers, UserActionId = userActionId });
                    }
                    break;
            }

            ApiResponse response = null;
            Do.With().Timeout(3).Until(() => (ApplicationDecisionStatusEnum)
                Enum.Parse(typeof(ApplicationDecisionStatusEnum), (response = Driver.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = _id })).Values["ApplicationDecisionStatus"].Single()) == _decision);

            if (_decision == ApplicationDecisionStatusEnum.Declined)
            {
                return new Application(_id, GetFailedCheckpointFromApplicationDecisionResponse(response));
            }

            Driver.Api.Commands.Post(new SignApplicationCommand { AccountId = _customer.Id, ApplicationId = _id });



            ApiRequest summary = Config.AUT == AUT.Za
                                     ? new GetAccountSummaryZaQuery {AccountId = _customer.Id}
                                     :  (ApiRequest)new GetAccountSummaryQuery { AccountId = _customer.Id };


            Do.Until(() => Driver.Api.Queries.Post(summary).Values["HasCurrentLoan"].Single() == "true");

            Do.With().Timeout(TimeSpan.FromSeconds(10)).Watch(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == _id).Transactions.Count);

            return new Application {Id = _id, BankAccountId = _customer.BankAccountId, LoanAmount = _loanAmount, LoanTerm = _loanTerm};
        }

    	private static string GetFailedCheckpointFromApplicationDecisionResponse(ApiResponse response)
    	{
    		return response != null ? response.Values["FailedCheckpoint"].FirstOrDefault() : null;
    	}

    	private int GetDaysUntilStartOfLoan()
        {
            return 0;
        }

        private int GetDaysUntilStartOfLoanForCa()
        {
            return DateHelper.GetNumberOfDaysUntilStartOfLoanForCa();
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
                        if (eidSessionInteraction[eidSessionNumber][questionNumber-1])
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
                            <QuestionId>"+questionNumber+"</QuestionId>" +
                            "<AnswerId>"+answerNumber+"</AnswerId>"+
                        "</UidQuestionAnswer> ";
        }
    }
}
