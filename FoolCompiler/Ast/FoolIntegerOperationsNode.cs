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

        private IFoolNode _left;
        private IFoolNode _right;
        private string _operation;

        public FoolIntegerOperationsNode(IntegerOperationTypes operation, IFoolNode left, IFoolNode right)
        {
            _left = left;
            _right = right;

            switch(operation)
            {
                case IntegerOperationTypes.PLUS:
                    _operation = "add\n";
                    break;
                case IntegerOperationTypes.DIVISION:
                    _operation = "div\n";
                    break;
                case IntegerOperationTypes.MULT:
                    _operation = "mult\n";
                    break;
                case IntegerOperationTypes.MINUS:
                    _operation = "sub\n";
                    break;
                default:
                    Console.WriteLine("Oops... No operation [" + operation + "] defined!");
                    break;
            }
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
            return (_left.CodeGeneration() 
                + _right.CodeGeneration() 
                + _operation)
                ;
        }

        public IFoolType TypeCheck()
        {
            if (!(_left.TypeCheck().IsSubType(new FoolIntType())) || !(_right.TypeCheck().IsSubType(new FoolIntType()))) 
            {
                throw new FoolTypeException("Oops... Incompatible types, Integer type needed!");
            }
            return new FoolIntType();
        }
    }
}
