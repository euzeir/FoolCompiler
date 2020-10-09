using System;

namespace FoolCompiler.ExceptionHandling
{
    public class MultipleNameDeclarationErrorException: SystemException
    {
        public MultipleNameDeclarationErrorException(string str): base("Variable [ " + str + " ] has been already declared.\n")
        {
        }
    }
}
