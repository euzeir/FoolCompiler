using System;

namespace FoolCompiler.ExceptionHandling
{
    public class FoolHeapOverflowError : Exception
    {
        public FoolHeapOverflowError() : base("HeapOverflow!")
        {
        }
    }
}
