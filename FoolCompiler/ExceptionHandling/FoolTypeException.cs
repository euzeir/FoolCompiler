using System;

namespace FoolCompiler.ExceptionHandling
{
    public class FoolTypeException: SystemException
    {
        public FoolTypeException(string msg) : base("Error: " + msg + "\n")
        {
        }
    }
}
