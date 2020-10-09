using System;
using System.Collections.Generic;
using System.Linq;

namespace FoolCompiler.ExceptionHandling
{
    public class FoolLexerException: Exception
    {
        public FoolLexerException(string error): base(error)
        {
        }
    }
}
