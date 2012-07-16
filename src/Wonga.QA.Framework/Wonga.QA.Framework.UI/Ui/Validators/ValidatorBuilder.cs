using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.UI.Ui.Validators
{
    public class ValidatorBuilder
    {
        
        public static Validator Validator;


        public ValidatorBuilder New(UiClient client)
        {
            Validator = new Validator(client);
            return this;
        }

       	public  ValidatorBuilder Default(UiClient client)
        {
            Validator = new Validator(client);
            Validator.Checks.Add(Validator.ErrorsCheck);
            Validator.Checks.Add(Validator.InvalidFormErrorCheck);
            Validator.Checks.Add(Validator.HeadersCheck);
            Validator.Checks.Add(Validator.TitleCheck);
            return this;
        }

        public ValidatorBuilder WithoutErrorsCheck()
        {
            Validator.Checks.Remove(Validator.ErrorsCheck);
            return this; 
        }

        public Validator Build()
        {
            return Validator;
        }


    }
}
