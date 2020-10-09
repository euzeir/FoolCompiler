using System.Collections.Generic;
using FoolCompiler.ExceptionHandling;
using FoolCompiler.Lib;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    public class FoolLogicalOperationsNode : IFoolNode
    {
        private IFoolNode left;
        private IFoolNode right;
        private int operation;

        public FoolLogicalOperationsNode(int op, IFoolNode l, IFoolNode r)
        {
            right = r;
            left = l;
            operation = op;
        }
        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();

            result.AddRange(left.CheckSemantics(environment));
            result.AddRange(right.CheckSemantics(environment));
            return result;
        }

        public string CodeGeneration()
        {
            string label = FoolLib.CreateFreshLabel();
            string exit = FoolLib.CreateFreshLabel();

            switch (operation)
            {
                case FOOLLexer.AND:
                    return (left.CodeGeneration()
                        + "push 0\n"
                        + "beq "
                        + label
                        + "\n"
                        + right.CodeGeneration()
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
                    return (left.CodeGeneration()
                        + "push 1\n"
                        + "beq "
                        + label
                        + "\n"
                        + right.CodeGeneration()
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
                    return (left.CodeGeneration()
                        + right.CodeGeneration()
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
                    return left.CodeGeneration()
                        + "push 1\n"
                        + "add\n"
                        + right.CodeGeneration()
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
                    return right.CodeGeneration()
                        + left.CodeGeneration()
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
                    return right.CodeGeneration()
                        + "push 1\n"
                        + "add\n"
                        + left.CodeGeneration()
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
                    return left.CodeGeneration()
                        + right.CodeGeneration()
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
            IFoolType leftType = left.TypeCheck();
            IFoolType rightType = right.TypeCheck();

            if (operation == FOOLLexer.AND || operation == FOOLLexer.OR)
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
