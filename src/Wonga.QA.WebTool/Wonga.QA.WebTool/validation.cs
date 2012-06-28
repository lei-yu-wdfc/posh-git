using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Wonga.QA.WebTool
{
    static public class Validation
    {
        static public bool onlyNumeral(string stringForCheck)
        {
            try
            {
                Encoding enc = Encoding.GetEncoding("us-ascii",
                                          new EncoderExceptionFallback(),
                                          new DecoderExceptionFallback());
                byte[] bytes = enc.GetBytes(stringForCheck);

                foreach (byte bt in bytes)
                {
                    if((bt>=48)&&(bt<=57))
                    {
                       return true;
                    }   
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}