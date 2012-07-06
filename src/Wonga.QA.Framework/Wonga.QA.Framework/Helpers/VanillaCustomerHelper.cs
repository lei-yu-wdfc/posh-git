using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Helpers
{
    public class VanillaCustomerHelper
    {
        private AUT _aut;
        private SUT _sut;
        private readonly dynamic _db;
   
        public VanillaCustomerHelper()
        {
            _aut = Config.AUT;
            _sut = Config.SUT;
            _db = Do.Until(() => Drive.Data.QaData.Db);
        }

        public int Count()
        {
            return Do.Until(() => _db.TestUsers.FindAllByIsUsed(0).Count());
        }

        public VanillaCustomer GetVanillaCustomer()
        {
            try
            {
                using (var tx = _db.BeginTransaction())
                {
                    var unusedCus = tx.TestUsers.FindByIsUsed(0);
                    unusedCus.IsUsed = 1;
                    tx.TestUsers.Update(unusedCus);
                    tx.Commit();
                    return new VanillaCustomer(unusedCus.Email, unusedCus.Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Exception caught whilst attempting to get user from TestUsers table: {0}", ex));
                return new VanillaCustomer();
            }
        }

        public void DeleteAllUsedUsers()
        {
            try
            {
                using (var tx = _db.BeginTransaction())
                {
                    tx.TestUsers.DeleteAll(_db.TestUsers.IsUsed == 1);
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Exception caught whilst attempting to delete used users from QAData.TestUsers table: {0}", ex));
            }
        }

        public static VanillaCustomerHelper New()
        {
            return new VanillaCustomerHelper();
        }

        public VanillaCustomerHelper WithEnvironment(AUT aut, SUT sut)
        {
            _aut = aut;
            _sut = sut;
            return this;
        }

    }

    public class VanillaCustomer
    {
        private readonly string _email;
        private readonly Guid _id;
        private readonly int _isUsed;

        public string Email
        {
            get { return _email; }
        }

        public Guid Id
        {
            get { return _id; }
        }

        public int IsUsed
        {
            get { return _isUsed; }
        }

        public VanillaCustomer()
        {
            _email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(_email).Build();
            Application application = ApplicationBuilder.New(customer).Build();
            application.RepayOnDueDate();
            _id = customer.Id;
            _isUsed = 0;
        }

        public void InsertUserToDb()
        {
            var testUsersTable = Drive.Data.QaData.Db.TestUsers;
            dynamic cus = new ExpandoObject();
            cus.Email = Email;
            cus.Id = Id;
            cus.IsUsed = IsUsed;
            testUsersTable.Insert(cus);

            Do.With.Timeout(2).Until(() => testUsersTable.All().Select(Email: Email, Id: Id).Single());
        }

        public VanillaCustomer(string email, Guid id)
        {
            _email = email;
            _id = id;
            _isUsed = 1;
        }
    }


}
