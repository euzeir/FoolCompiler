using System;
using System.Collections.Generic;
using FoolCompiler.ExceptionHandling;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    public class FoolIntegerOperationsNode : IFoolNode
    {
        public enum IntegerOperationTypes {
            PLUS,
            MINUS,
            MULT,
            DIVISION
        };

        private IFoolNode left;
        private IFoolNode right;
        private string operation;

        public FoolIntegerOperationsNode(IntegerOperationTypes ops, IFoolNode l, IFoolNode r)
        {
            left = l;
            right = r;

            switch(ops)
            {
                case IntegerOperationTypes.PLUS:
                    operation = "add\n";
                    break;
                case IntegerOperationTypes.DIVISION:
                    operation = "div\n";
                    break;
                case IntegerOperationTypes.MULT:
                    operation = "mult\n";
                    break;
                case IntegerOperationTypes.MINUS:
                    operation = "sub\n";
                    break;
                default:
                    Console.WriteLine("Oops... There is not defined such an operator you have used!");
                    break;
            }
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
            return (left.CodeGeneration() 
                + right.CodeGeneration() 
                + operation)
                ;
        }

        public IFoolType TypeCheck()
        {
            if (!(left.TypeCheck().IsSubType(new FoolIntType())) || !(right.TypeCheck().IsSubType(new FoolIntType()))) 
            {
                throw new FoolTypeException("Oops... Incompatible types, Integer type needed!");
            }
            return new FoolIntType();
        }
    }
}
