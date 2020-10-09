using System;

namespace FoolCompiler.ExceptionHandling
{
    public class NotDeclaredNameErrorException: SystemException
    {
        public NotDeclaredNameErrorException(string str): base("\"" + str + "\" not declared.\n")
        {
        }
    }
}
