using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Wonga.QA.PerformanceTests.Core
{
    public class TestRunner
    {
        public String TestName = ConfigurationManager.AppSettings["tests"];
        public String DllName = ConfigurationManager.AppSettings["dllName"];
        public String TypeName = ConfigurationManager.AppSettings["namespace"];

        /// <summary>
        /// Run the given Test using the Dll
        /// </summary>
        public void Run()
        {
            if (!IsMemberExists(TestName))
            {
                Console.WriteLine("Could not find any matching test name " + TestName + " in " + DllName);
                return;
            }

            Assembly assembly = Assembly.LoadFrom(DllName);
            var type = assembly.GetType(TypeName);
            var targetObj = Activator.CreateInstance(type);

            foreach (var m in type.GetMembers())
            {
                if (!m.Name.Equals(TestName)) continue;
                type.InvokeMember(m.Name,
                                  BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance,
                                  Type.DefaultBinder,
                                  targetObj,
                                  null);
            }
        }

        /// <summary>
        /// Returns all memebrs part of the given Dll and namespace
        /// </summary>
        /// <returns></returns>
        public MemberInfo[] GetAllMembers(String dllName, String typeName)
        {
            Assembly assembly = Assembly.LoadFrom(dllName);
            var type = assembly.GetType(typeName);

            return type.GetMembers();
        }

        /// <summary>
        /// Member with the given name exists?
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public Boolean IsMemberExists(string memberName)
        {
            var members = GetAllMembers(DllName, TypeName);

            foreach (var memberInfo in members)
            {
                if (memberInfo.Name.Equals(memberName))
                    return true;
            }
            return false;
        }
    }
}
