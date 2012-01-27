using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public abstract class ApiRequest
    {
        public String Serialize()
        {
            String root = GetType().GetAttribute<XmlRootAttribute>().ElementName;
            PropertyInfo[] properties = GetType().GetProperties();

            if (!properties.Any())
                return String.Format("<{0} />", root);

            StringBuilder builder = new StringBuilder().AppendFormat("<{0}>", root);
            foreach (PropertyInfo property in properties)
            {
                Object value = property.GetValue(this, null);
                if (value == null)
                    builder.AppendFormat("<{0} xsi:nil=\"true\" />", property.Name);
                else
                {
                    String convert = Data.ToString(value);
                    if (String.IsNullOrEmpty(convert))
                        builder.AppendFormat("<{0} />", property.Name);
                    else
                        builder.AppendFormat("<{0}>{1}</{0}>", property.Name, convert);
                }
            }
            return builder.AppendFormat("</{0}>", root).ToString();
        }

        public ApiRequest Randomize()
        {
            foreach (PropertyInfo property in GetType().GetProperties())
                switch (property.Name)
                {
                    case "TermSliderPosition":
                    case "AmountSliderPosition":
                        property.SetValue(this, "Min", null);
                        break;
                    case "PaymentCardId":
                        property.SetValue(this, null, null);
                        break;
                    case "PromoCodeId":
                    case "AffiliateId":
                        property.SetValue(this, null, null);
                        break;
                    case "LoanAmount":
                        property.SetValue(this, Data.RandomInt(100, 400), null);
                        break;
                        ;
                    case "Currency":
                        property.SetValue(this, Data.RandomEnum<CurrencyCodeEnum>(), null);
                        break;
                    case "PromiseDate":
                        property.SetValue(this, DateTime.Today.AddDays(10).ToDate(DateFormat.Date), null);
                        break;
                    case "AccountId":
                    case "AddressId":
                    case "VerificationId":
                    case "BankAccountId":
                    case "ApplicationId":
                        property.SetValue(this, Data.GetId(), null);
                        break;

                    case "DateOfBirth":
                        property.SetValue(this, Data.GetDoB(), null);
                        break;

                    case "Gender":
                        property.SetValue(this, Data.RandomEnum<GenderEnum>(), null);
                        break;

                    case "Login":
                    case "Email":
                        property.SetValue(this, Data.GetEmail(), null);
                        break;

                    case "Password":
                        property.SetValue(this, Data.GetPassword(), null);
                        break;

                    case "Forename":
                    case "Surname":
                    case "MiddleName":
                    case "MaidenName":
                    case "EmployerName":
                        property.SetValue(this, Data.GetName(), null);
                        break;
                    case "HomePhone":
                    case "WorkPhone":
                        property.SetValue(this, Data.GetPhone(), null);
                        break;
                    case "Title":
                        property.SetValue(this, Data.RandomEnum<TitleEnum>(), null);
                        break;
                    case "NationalNumber":
                        property.SetValue(this, null, null);
                        break;
                    case "MarriedInCommunityProperty":
                    case "AcceptMarketingContact":
                        property.SetValue(this, Data.RandomBoolean(), null);
                        break;
                    case "MaritalStatus":
                        property.SetValue(this, Data.RandomEnum<MaritalStatusEnum>(), null);
                        break;
                    case "OccupancyStatus":
                        property.SetValue(this, Data.RandomEnum<OccupancyStatusEnum>(), null);
                        break;
                    case "HomeLanguage":
                        property.SetValue(this, Data.RandomEnum<LanguageZaEnum>(), null);
                        break;
                    case "Dependants":
                        property.SetValue(this, Data.RandomInt(10), null);
                        break;
                    case "VehicleRegistration":
                        property.SetValue(this, null, null);
                        break;
                    case "SecretQuestion":
                    case "SecretAnswer":
                        property.SetValue(this, Data.RandomString(10, 100), null);
                        break;
                    case "Pin":
                        property.SetValue(this, "0000", null);
                        break;
                    case "Flat":
                    case "HouseNumber":
                        property.SetValue(this, Data.RandomInt(1, 1000), null);
                        break;
                    case "HouseName":
                    case "Street":
                    case "District":
                    case "Town":
                    case "County":
                    case "HolderName":
                        property.SetValue(this, Data.RandomString(10, 100), null);
                        break;
                    case "BankName":
                        property.SetValue(this, "ABSA", null);
                        break;
                    case "AccountType":
                        property.SetValue(this, "Savings", null);
                        break;
                    case "Postcode":
                        property.SetValue(this, "0300", null);
                        break;
                    case "CountryCode":
                        property.SetValue(this, Data.RandomEnum<CountryCodeEnum>().ToString().ToUpper(), null);
                        break;
                    case "AtAddressFrom":
                    case "AtAddressTo":
                        property.SetValue(this, Data.RandomDate(), null);
                        break;
                    case "AccountNumber":
                        property.SetValue(this, (Int64)((9999999999999 - 1000000) * Data.Random()) + 1000000, null);
                        break;
                    case "AccountOpenDate":
                        property.SetValue(this, Data.RandomDateTime(), null);
                        break;
                    case "IsPrimary":
                        //property.SetValue(this, RandomBoolean();
                        property.SetValue(this, true, null);
                        break;
                    case "NetMonthlyIncome":
                        property.SetValue(this, Data.RandomInt(1, 100000), null);
                        break;
                    case "IncomeFrequency":
                        property.SetValue(this, Data.RandomEnum<IncomeFrequencyEnum>(), null);
                        break;
                    case "NextPayDate":
                        property.SetValue(this, Data.RandomDate(DateTime.Today, DateTime.Today.AddMonths(1)), null);
                        break;
                    case "Status":
                        property.SetValue(this, Data.RandomEnum<EmploymentStatusEnum>(), null);
                        break;
                    case "EmploymentIndustry":
                        property.SetValue(this, Data.RandomEnum<EmploymentIndustryEnum>(), null);
                        break;
                    case "EmploymentPosition":
                        //property.SetValue(this, RandomEnum<EmploymentPositionEnum>();
                        property.SetValue(this, "Engineering", null);
                        break;
                    case "StartDate":
                        property.SetValue(this, Data.RandomDate(), null);
                        break;
                    case "PaidDirectDeposit":
                        property.SetValue(this, Data.RandomBoolean(), null);
                        break;
                    case "MobilePhone":
                        property.SetValue(this, "0720000000", null);
                        break;
                    default:
                        throw new NotImplementedException(property.Name);
                }
            return this;
        }
    }

    public abstract class ApiRequest<T> : ApiRequest where T : ApiRequest<T>, new()
    {
        public new T Randomize()
        {
            return (T)base.Randomize();
        }

        public static T Random(Action<T> action = null)
        {
            T t = new T().Randomize();
            if (action != null)
                action(t);
            return t;
        }
    }
}
