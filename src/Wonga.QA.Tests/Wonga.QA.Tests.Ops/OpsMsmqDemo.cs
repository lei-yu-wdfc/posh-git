using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Bi;
using Wonga.QA.Framework.Db.FileStorage;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Framework.Msmq.Ops;
using Wonga.QA.Tests.Core;
using MSSQLDeploy = Wonga.QA.Framework.Db.FileStorage.MSSQLDeploy;

namespace Wonga.QA.Tests.Ops
{
    public class OpsMsmqDemo
    {
        [Test, AUT]
        public void CreateAccountMessage()
        {
            Drivers drivers = new Drivers();

            CreateAccountCommand message = null;

            /*for (var i = 0; i < 1; i++)
                Drivers.Msmq.Ops.Send(message = new CreateAccountCommand
                {
                    AccountId = Data.GetId(),
                    Login = Data.GetEmail(),
                    Password = Data.GetPassword()
                });*/

            /*Thread.Sleep(1000);

            AccountEntity account = ;
            Assert.AreEqual(message.AccountId, account.ExternalId);*/

            //Do.While(() => Trace.Write("foo"));

            FileStorageDatabase database = drivers.Db.FileStorage;
            Table<MSSQLDeploy> table = database.MSSQLDeploys;
            MSSQLDeploy entity = table.First();

            Console.WriteLine(entity.Refresh().MD5);


        }


    }

    public static class Ext
    {
        public static T GetField<T>(this Object instance, String name)
        {
            return (T)instance.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance);
        }
    }
}
