using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Framework.Msmq.PublicMessages.Payments.PayLater.UK;

namespace Wonga.QA.ServiceTests.Risk.PayLater.UK
{
    [Parallelizable(TestScope.All), Ignore("Awaiting bug fixes")]
    public class CreatePayLaterApplicationTests 
    {
        private Guid _accountId;
        private Guid _applicationId;
        private Guid _paymentCardId;
        private decimal _totalAmount;
        private decimal _transactionFee;
        private decimal _installmentAmount;

        [SetUp]
        public void SetUp()
        {
            _accountId = Guid.NewGuid();
            _applicationId = Guid.NewGuid();
            _paymentCardId = Guid.NewGuid();
            _totalAmount = 101.10m;
            _transactionFee = 87.03m;
            _installmentAmount = 2.09m;
        }

        [Test]
        public void CreatePayLaterApplicationSaga_ShouldCreateAnApplication_WhenAllMessagesAreRecivedInAnyOrder_CreatePayLaterApplicationFirst()
        {
            CreatePayLaterApplication();

            CreateIPayLaterApplicationAdded();

            CreateICustomerTransactionFeeAdded();

            CreateIInstalmentAdded();

            AssertPayLaterApplicationCreated();
        }
       
        private void CreatePayLaterApplication()
        {
            Drive.Api.Commands.Post(new RiskCreatePayLaterApplicationUkCommand
                                        {
                                            AccountId = _accountId,
                                            ApplicationId = _applicationId,
                                            PaymentCardId = _paymentCardId,
                                            TotalAmount = _totalAmount
                                        });
        }

        private void CreateIPayLaterApplicationAdded()
        {
            Drive.Msmq.Risk.Send(new IPayLaterApplicationAdded
                                     {
                                         AccountId = _accountId,
                                         ApplicationId = _applicationId,
                                         CreatedOn = DateTime.UtcNow
                                     });
        }

        private void CreateICustomerTransactionFeeAdded()
        {
            Drive.Msmq.Risk.Send(new ICustomerTransactionFeeAdded
                                     {
                                         AccountId = _accountId,
                                         ApplicationId = _applicationId,
                                         TransactionFee = _transactionFee
                                     });
        }

        private void CreateIInstalmentAdded()
        {
            Drive.Msmq.Risk.Send(new IInstalmentAdded
                                     {
                                         AccountId = _accountId,
                                         ApplicationId = _applicationId,
                                         InstallmentAmount = _installmentAmount,
                                         InstallmentNumber = 1
                                     });
        }

        private void AssertPayLaterApplicationCreated()
        {
            Do.With.Message("No paylater application was found").Until(
                () => Drive.Data.PayLater.Db.Applications.FindByExternalId(_applicationId));
        }

        [Test]
        public void CreatePayLaterApplicationSaga_ShouldCreateAnApplication_WhenAllMessagesAreRecivedInAnyOrder_IPayLaterApplicationAddedFirst()
        {
            CreateIPayLaterApplicationAdded();

            CreatePayLaterApplication();

            CreateICustomerTransactionFeeAdded();

            CreateIInstalmentAdded();

            AssertPayLaterApplicationCreated();
        }

        [Test]
        public void CreatePayLaterApplicationSaga_ShouldCreateAnApplication_WhenAllMessagesAreRecivedInAnyOrder_ICustomerTransactionFeeAddedFirst()
        {
            CreateICustomerTransactionFeeAdded();

            CreateIPayLaterApplicationAdded();

            CreatePayLaterApplication();

            CreateIInstalmentAdded();

            AssertPayLaterApplicationCreated();
        }

        [Test]
        public void CreatePayLaterApplicationSaga_ShouldCreateAnApplication_WhenAllMessagesAreRecivedInAnyOrder_IInstalmentAddedFirst()
        {
            CreateIInstalmentAdded();

            CreateICustomerTransactionFeeAdded();

            CreateIPayLaterApplicationAdded();

            CreatePayLaterApplication();

            AssertPayLaterApplicationCreated();
        }
        
    }
}