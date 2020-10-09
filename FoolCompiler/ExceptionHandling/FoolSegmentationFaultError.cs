using System;

namespace FoolCompiler.ExceptionHandling
{
    public class FoolSegmentationFaultError : SystemException
    {
        public FoolSegmentationFaultError() : base("SEGMENTATION FAULT: TRYING TO ACCESS MEMORY WITHOUT PERMISSION!")
        {
        }
    }
}
