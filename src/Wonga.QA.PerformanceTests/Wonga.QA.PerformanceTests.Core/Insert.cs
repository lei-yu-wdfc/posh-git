﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.PerformanceTests.Core
{
    public class Insert
    {
        Hashtable args = new Hashtable();
        public string Table;
        public string DatabaseName;

        /// <summary>
        /// Constructs Insert object
        /// </summary>
        /// <param name="database"></param>
        /// <param name="table">table name to insert to</param>
        public Insert(string database, string table)
        {
            this.Table = table;
            this.DatabaseName = database;
        }

        /// <summary>
        /// Adds item to Insert object
        /// </summary>
        /// <param name="name">item name</param>
        /// <param name="val">item value</param>
        public void Add(string name, object val)
        {
            args.Add(name, val);
        }

        /// <summary>
        /// Removes item from Insert object
        /// </summary>
        /// <param name="name">item name</param>
        public void Remove(string name)
        {
            try
            {
                args.Remove(name);
            }
            catch
            {
                throw (new Exception("No such item"));
            }
        }

        /// <summary>
        /// Test representatnion of the Insert object (SQL query)
        /// </summary>
        /// <returns>System.String</returns>
        public override string ToString()
        {
            var s1 = new StringBuilder();
            var s2 = new StringBuilder();

            IDictionaryEnumerator enumInterface = args.GetEnumerator();
            bool first = true;
            while (enumInterface.MoveNext())
            {
                if (first) first = false;
                else
                {
                    s1.Append(", ");
                    s2.Append(", ");
                }
                s1.Append(enumInterface.Key.ToString());
                s2.Append(enumInterface.Value.ToString());
            }

            return "INSERT INTO " + Table + " (" + s1 + ") VALUES (" + s2 + ");";
        }

        /// <summary>
        /// Gets or sets item into Insert object
        /// </summary>
        object this[string key]
        {
            get
            {
                return args[key];
            }
            set { args[key] = value; }
        }
    }
}
