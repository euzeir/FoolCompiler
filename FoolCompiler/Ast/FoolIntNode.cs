using System.Collections.Generic;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    public class FoolIntNode : IFoolNode
    {
        private int _value;

        public FoolIntNode(int value)
        {
            _value = value;
        }
        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            return new List<string>();
        }

        public string CodeGeneration()
        {
            return "push " + _value + "\n";
        }

        public IFoolType TypeCheck()
        {
            return new FoolIntType();
        }
    }
}
