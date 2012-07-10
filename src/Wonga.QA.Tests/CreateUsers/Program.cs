using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MbUnit.Framework;
using NHamcrest.Core;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Helpers;

namespace CreateUsers
{
    static class Program
    {
        public static int AvailableCustomerCount;
        public static int NumberOfUsersToCreate;
        public const int Threshold = 1000;
        public static bool IsRun;
        public static TimeSpan NightRun = new TimeSpan(20, 00, 00);
        public static TimeSpan DayRun = new TimeSpan(15, 33, 00);

        static void Main()
        {
            IsRun = true;
            Run();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        public static bool Between(this TimeSpan value, TimeSpan left, TimeSpan right)
        {
            return value > left && value < right;
        }

     
        public static bool IsScheduledRun()
        {
            var nightRunUpper = NightRun.Add(new TimeSpan(00, 00, 10));
            var dayRunUpper = DayRun.Add(new TimeSpan(00, 00, 10));
            var now = DateTime.Now.TimeOfDay;
            //return true if current time is between specified run time and 10 seconds after specified runtime
            return now.Between(DayRun, dayRunUpper) || now.Between(NightRun, nightRunUpper);
        }

        public static void Run()
        {
            var helper = VanillaCustomerHelper.New().WithEnvironment(Config.AUT, Config.SUT);
            while (IsRun)
            {
                if (!IsScheduledRun()) continue;
                AvailableCustomerCount = helper.Count();
                helper.DeleteAllUsedUsers();

                NumberOfUsersToCreate = Threshold - AvailableCustomerCount;
                if (NumberOfUsersToCreate > 0)
                    Console.WriteLine(String.Format("Attempting user creation on {0}", DateTime.Now));
                    CreateUsersInParallel();
            }
        }


        public static void Notify(int exceptionCount, int toCreate, int created)
        {
            string recipient = "ben.ifie@wonga.com";
            string subject = "Users Not Created";
            string body = created == 0 
               ? String.Format("Test Creation application has stopped running on {0}/{1}.\n\nThere were {2} exceptions encountered. View Logs.\n\nOut of {3} users, none were created", Config.AUT, Config.SUT, exceptionCount, toCreate)
               : String.Format("Test Creation application has stopped running on {0}/{1}.\n\nThere were {2} exceptions encountered. View Logs.\n\nOut of {3} users, only {4} were created", Config.AUT, Config.SUT, exceptionCount, toCreate, created);
            SendMail(recipient, subject, body);
        }

        public static void SendMail(string messageRecepient, string messageSubject, string messageBody)
        {
            const string senderEmail = "testusercreation@gmail.com";

            var to = new MailAddress(messageRecepient);
            var from = new MailAddress(senderEmail);
            var message = new MailMessage(from, to);
            message.Subject = messageSubject;
            message.Body = messageBody;
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential(senderEmail, "Usercreation");
                client.EnableSsl = true;
                Console.WriteLine("Sending an e-mail message to {0} at {1} by using the SMTP host={2}.",
                to.User, to.Host, client.Host);
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught whilst Sending Email Message: {0}", ex);
                }
            }
            
        }


        public static void Create(int thread)
        {
            if (NumberOfUsersToCreate <= 0)
                return;
            var c = new VanillaCustomer();
            c.InsertUserToDb();
            Console.WriteLine(String.Format("User number {0} with email {1} successfully created by thread Id:{2}", thread.ToString(CultureInfo.InvariantCulture), c.Email, Thread.CurrentThread.ManagedThreadId));
            AvailableCustomerCount++;
            NumberOfUsersToCreate--;
        }

        private static void CreateUsersInParallel()
        {
            int intendedNumberOfUsersToCreate = NumberOfUsersToCreate;
            
            // Use ConcurrentQueue to enable safe enqueueing from multiple threads.
            var exceptions = new ConcurrentQueue<Exception>();
            var po = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            var cts = new CancellationTokenSource();
            po.CancellationToken = cts.Token;

            // Run a task so that we can cancel Parallel.ForEach Loop from another thread if too many exceptions are encountered
            Task.Factory.StartNew(() =>
            {
                while (exceptions.Count < 1)
                {
                    //do nothing
                }
                cts.Cancel();
                int created = intendedNumberOfUsersToCreate - NumberOfUsersToCreate;
                Notify(exceptions.Count, intendedNumberOfUsersToCreate, created);
                IsRun = false;
            });
            
            Console.WriteLine("No of users to create is {0}", NumberOfUsersToCreate);
            var amountToCreate = Enumerable.Range(0, NumberOfUsersToCreate).ToArray();
            try
            {
                Parallel.ForEach(amountToCreate, po, toCreate =>
                {
                    try
                    {
                        Create(toCreate);
                        po.CancellationToken.ThrowIfCancellationRequested();
                    }
                    // Store the exception and continue with the loop.
                    catch (Exception e)
                    {
                        exceptions.Enqueue(e);
                    }
                });
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
    }
}
