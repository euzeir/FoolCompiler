using System;

namespace FoolCompiler.ExceptionHandling
{
    public class FoolParserException: Exception
    {
        public FoolParserException(string error) : base(error)
        {

        }
    }
}
