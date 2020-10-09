using System.Collections.Generic;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    public class FoolTypeNode : IFoolNode
    {
        private string _typeName;
        private IFoolType _foolType;

        public FoolTypeNode(string typeName)
        {
            _typeName = typeName;

            switch (_typeName)
            {
                case "int":
                    _foolType = new FoolIntType();
                    break;
                case "bool":
                    _foolType = new FoolBooleanType();
                    break;
                case "void":
                    _foolType = new FoolVoidType();
                    break;
                default:
                    _foolType = new FoolObjectType(new FoolClassType(_typeName));
                    break;
            }
        }
        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            return new List<string>();
        }

        public string CodeGeneration()
        {
            return string.Empty;
        }

        public IFoolType TypeCheck()
        {
            return _foolType; 
        }
    }
}
