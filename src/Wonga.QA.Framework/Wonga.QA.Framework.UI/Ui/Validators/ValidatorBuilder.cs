using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.UI.Ui.Validators
{
    public static class ValidatorBuilder
    {
        
        public static Validator Validator;
        
        public static Validator New(UiClient client)
        {
            Validator = new Validator(client); 
            return Validator;
        }

        public static Validator Default(UiClient client)
        {
            Validator = new Validator(client);
            Validator.Checks.Add(Validator.ErrorsCheck);
            Validator.Checks.Add(Validator.InvalidFormErrorCheck);
            Validator.Checks.Add(Validator.HeadersCheck);
            Validator.Checks.Add(Validator.TitleCheck);
            return Validator;
        }

        public static Validator WithoutErrorsCheck()
        {
            Validator.Checks.Remove(Validator.ErrorsCheck);
            return Validator;
        }


    }
}
