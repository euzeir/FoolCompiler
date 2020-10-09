using System.Collections.Generic;
using FoolCompiler.ExceptionHandling;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    public class FoolBooleanNode : IFoolNode
    {
        private bool _booleanValue;
        public FoolBooleanNode(bool booleanValue)
        {
            _booleanValue = booleanValue;
        }
        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            return new List<string>();
        }

        public string CodeGeneration()
        {
            return "push " + (_booleanValue ? 1 : 0) + "\n";
        }

        public IFoolType TypeCheck()
        {
            try
            {
                return new FoolBooleanType();
            }
            catch
            {
                throw new FoolTypeException("Type Exception!");
            }
        }
    }
}
