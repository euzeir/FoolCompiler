using System;
using System.Collections.Generic;

namespace FoolCompiler.ExceptionHandling
{
    class FoolSemanticException: Exception
    {
        public FoolSemanticException(List<string> errors)
        {
            foreach (var error in errors)
            {
               new Exception(error);
            }
        }
    }
}
