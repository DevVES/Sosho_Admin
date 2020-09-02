using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ValueValidation
/// </summary>
public class ValueValidation:IValidation
{
	public ValueValidation()
	{

	}
    public bool checkNumberStringIsCorrectOrNotAndIfYesGreaterThanZeroOrNot(string _para)
    {
        if (!String.IsNullOrWhiteSpace(_para))
        {
            decimal checkPara = 0;
            decimal.TryParse(_para, out checkPara);
            if (checkPara > 0)
                return true;
        }
        //Reach to this point suggest string is wrong
        return false;
    }
}