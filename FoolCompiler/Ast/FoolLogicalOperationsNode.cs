using System.Collections.Generic;
using FoolCompiler.ExceptionHandling;
using FoolCompiler.Lib;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    public class FoolLogicalOperationsNode : IFoolNode
    {
        private IFoolNode _left;
        private IFoolNode _right;
        private int _operation;

        public FoolLogicalOperationsNode(int operation, IFoolNode left, IFoolNode right)
        {
            _right = right;
            _left = left;
            _operation = operation;
        }
        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();

            result.AddRange(_left.CheckSemantics(environment));
            result.AddRange(_right.CheckSemantics(environment));
            return result;
        }

        public string CodeGeneration()
        {
            string label = FoolLib.CreateFreshLabel();
            string exit = FoolLib.CreateFreshLabel();

            switch (_operation)
            {
                case FOOLLexer.AND:
                    return (_left.CodeGeneration()
                        + "push 0\n"
                        + "beq "
                        + label
                        + "\n"
                        + _right.CodeGeneration()
                        + "push 0\n"
                        + "beq "
                        + label
                        + "\n"
                        + "push 1\n"
                        + "b "
                        + exit
                        + "\n"
                        + label
                        + ":\n"
                        + "push 0\n"
                        + exit
                        + ":\n")
                        ;
                case FOOLLexer.OR:
                    return (_left.CodeGeneration()
                        + "push 1\n"
                        + "beq "
                        + label
                        + "\n"
                        + _right.CodeGeneration()
                        + "push 1\n"
                        + "beq "
                        + label
                        + "\n"
                        + "push 0\n"
                        + "b "
                        + exit
                        + "\n"
                        + label
                        + ":\n"
                        + "push 1\n"
                        + exit
                        + ":\n")
                        ;
                case FOOLLexer.EQ:
                    return (_left.CodeGeneration()
                        + _right.CodeGeneration()
                        + "beq "
                        + label
                        + "\n"
                        + "push 0\n"
                        + "b "
                        + exit
                        + "\n"
                        + label
                        + ":\n"
                        + "push 1\n"
                        + exit
                        + ":\n")
                        ;
                case FOOLLexer.LESSERTHAN:
                    return _left.CodeGeneration()
                        + "push 1\n"
                        + "add\n"
                        + _right.CodeGeneration()
                        + "bleq "
                        + label
                        + "\n"
                        + "push 0\n"
                        + "b "
                        + exit
                        + "\n"
                        + label
                        + ":\n"
                        + "push 1\n"
                        + exit
                        + ":\n"
                        ;
                case FOOLLexer.GREATEREQUAL:
                    return _right.CodeGeneration()
                        + _left.CodeGeneration()
                        + "bleq "
                        + label
                        + "\n"
                        + "push 0\n"
                        + "b "
                        + exit
                        + "\n"
                        + label
                        + ":\n"
                        + "push 1\n"
                        + exit
                        + ":\n"
                        ;
                case FOOLLexer.GREATERTHAN:
                    return _right.CodeGeneration()
                        + "push 1\n"
                        + "add\n"
                        + _left.CodeGeneration()
                        + "bleq "
                        + label
                        + "\n"
                        + "push 0\n"
                        + "b "
                        + exit
                        + "\n"
                        + label
                        + ":\n"
                        + "push 1\n"
                        + exit
                        + ":\n"
                        ;
                case FOOLLexer.LESSEREQUAL:
                    return _left.CodeGeneration()
                        + _right.CodeGeneration()
                        + "bleq "
                        + label
                        + "\n"
                        + "push 0\n"
                        + "b "
                        + exit
                        + "\n"
                        + label
                        + ":\n"
                        + "push 1\n"
                        + exit
                        + ":\n"
                        ;
                default:
                    return string.Empty;
            }
        }

        public IFoolType TypeCheck()
        {
            IFoolType leftType = _left.TypeCheck();
            IFoolType rightType = _right.TypeCheck();

            if (_operation == FOOLLexer.AND || _operation == FOOLLexer.OR)
            {
                if (!(leftType.IsSubType(new FoolBooleanType())) || !(rightType.IsSubType(new FoolBooleanType())))
                {
                    throw new FoolTypeException("Oops... Incompatible type, Booleans needed\n");
                }
            }
            else
            {
                if (!(leftType.IsSubType(new FoolIntType())) || !(rightType.IsSubType(new FoolIntType())))
                {
                    throw new FoolTypeException("Oops... Incompatible type, Integers needed\n");
                }
            }
            return new FoolBooleanType();
        }
    }
}
