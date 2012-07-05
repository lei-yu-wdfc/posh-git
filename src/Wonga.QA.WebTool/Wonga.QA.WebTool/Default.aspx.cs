using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MbUnit.Framework;
using Microsoft.Win32;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.CommonApi;
using Wonga.QA.Framework.Data;
using Wonga.QA.Framework.ThirdParties;
using Wonga.QA.Framework.Msmq;


namespace Wonga.QA.WebTool
{
    public enum customerEnum
    {
        L0Customer,
        LNCustomer,
        Arrears
    }
    public  enum AUT
    {
        Uk, Za, Ca, Wb, Pl
    }
    public  enum SUT
    {
        RC, RCRelease, WIP, WIPRelese
    }

    public partial class _Default : System.Web.UI.Page
    {
        Customer customer;
        Application application;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                foreach (var item in Enum.GetNames(typeof(AUT)))
                {
                    _aut.Items.Add(item);
                }
                foreach (var item in Enum.GetNames(typeof(SUT)))
                {
                    _sut.Items.Add(item);
                }
                foreach (var item in Enum.GetNames(typeof(customerEnum)))
                {
                    _customer.Items.Add(item);
                }
            }
            else
            {
            }
            if (_customer.SelectedValue == "Arrears")
            {
                _parameters.Style.Add("display", "block;");
            }
        }


        protected void runButton_Click(object sender, EventArgs e)
        {
            if ((_aut.SelectedValue == "") || (_sut.SelectedValue == "") || (_customer.SelectedValue == ""))
            {
                errorLabel.Text = "You need note all fields!";
                wrapperp.Style.Add("display", "block;");
                resultTable.Style.Add("display", "block;");
                return;
            }
            if (_customer.SelectedValue == "Arrears")
            {
                if (!Validation.onlyNumeral(_parameters.Text))
                {
                    errorLabel.Text = "You enter not valid data into field!";
                    wrapperp.Style.Add("display", "block;");
                    resultTable.Style.Add("display", "block;");
                    return;
                }
            }

            try
            {
                ConfigSetter.Setter(_aut.SelectedValue, _sut.SelectedValue);
            }
            catch
            {
                errorLabel.Text = "Some problem with server!";
                resultTable.Style.Add("display", "block;");
                wrapperp.Style.Add("display", "block;");
                return;
            }

            try
            {
                switch (_customer.SelectedValue)
                {
                    case "L0Customer":
                        customer = CustomerBuilder
                            .New()
                            .WithEmailAddress(Get.RandomEmail())
                            .WithForename(Get.GetName())
                            .WithSurname(Get.RandomString(10))
                            .Build();
                        break;
                    case "LNCustomer":
                        customer = CustomerBuilder
                            .New()
                            .WithEmailAddress(Get.RandomEmail())
                            .WithForename(Get.GetName())
                            .WithSurname(Get.RandomString(10))
                            .Build();
                        application = ApplicationBuilder
                            .New(customer)
                            .Build();
                        application.RepayOnDueDate();
                        break;
                    case "Arrears":
                        customer = CustomerBuilder
                            .New()
                            .WithEmailAddress(Get.RandomEmail())
                            .WithForename(Get.GetName())
                            .WithSurname(Get.RandomString(10))
                            .Build();
                        application = ApplicationBuilder
                            .New(customer)
                            .Build();
                        application.PutIntoArrears(Convert.ToUInt32(_parameters.Text));
                        break;
                }
                progressing_icon.Style.Add("display", "none");
            }
            catch (Exception exc)
            {
                errorLabel.Text = "Some problem at the server!" + Connections.GetDbConn("WIP2", false);
                resultTable.Style.Add("display", "block;");
                wrapperp.Style.Add("display", "block;");
                return;
            }
            resultTable.Style.Add("display", "block");
            TableMaker tbMaker = new TableMaker();
            resultTable.Rows.Add(tbMaker.tbRow("Email:", customer.Email));
            resultTable.Rows.Add(tbMaker.tbRow("Password:", "Passw0rd"));
            resultTable.Rows.Add(tbMaker.tbRow("Name:", customer.GetCustomerFullName()));
            resultTable.Rows.Add(tbMaker.tbRow("Mobile:", customer.GetCustomerMobileNumber()));
        }

        protected void errorLabel_Unload(object sender, EventArgs e)
        {
        }

        protected void updateButton_Click(object sender, EventArgs e)
        {
            repos rep = new repos();
            errorLabel.Text = rep.Update();
            wrapperp.Style.Add("display", "block;");
            return;
        }

        protected void _customer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_customer.SelectedValue != "Arrears")
            {
            }
        }
    }
}
