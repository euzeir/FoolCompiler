using System.Collections.Generic;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    class FoolNullNode : IFoolNode
    {
        private string _classId;
        public FoolNullNode(string classId)
        {
            _classId = classId;
        }
        public FoolNullNode()
        {
            _classId = string.Empty;
        }
        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            return new List<string>();
        }

        public string CodeGeneration()
        {
            string Generate = string.Empty;
            Generate = "push 0\n"
                + "push class"
                + _classId 
                + "\n" 
                + "new\n"
                ;
            return Generate;
        }

        public IFoolType TypeCheck()
        {
            return new FoolVoidType();
        }
    }
}
