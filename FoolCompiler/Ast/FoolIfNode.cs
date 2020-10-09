using System.Collections.Generic;
using FoolCompiler.ExceptionHandling;
using FoolCompiler.Lib;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    public class FoolIfNode : IFoolNode
    {
        private IFoolNode _conditionNode;
        private IFoolNode _thenNode;
        private IFoolNode _elseNode;
        bool _isAnIfStatement;

        public FoolIfNode(IFoolNode conditionNode, IFoolNode thenNode, IFoolNode elseNode, bool isAnIfStatement)
        {
            _conditionNode = conditionNode;
            _thenNode = thenNode;
            _elseNode = elseNode;
            _isAnIfStatement = isAnIfStatement;
        }

        public FoolIfNode(IFoolNode conditionNode, IFoolNode thenNode, bool isAnIfStatement)
        {
            _conditionNode = conditionNode;
            _thenNode = thenNode;
            _elseNode = null;
            _isAnIfStatement = isAnIfStatement;
        }

        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();

            result.AddRange(_conditionNode.CheckSemantics(environment));
            result.AddRange(_thenNode.CheckSemantics(environment));
            if (_elseNode != null)
            {
                result.AddRange(_elseNode.CheckSemantics(environment));
            }
            return result;
        }

        public string CodeGeneration()
        {
            string result = string.Empty;
            string then = FoolLib.CreateFreshLabel();
            string exit = FoolLib.CreateFreshLabel();

            result += _conditionNode.CodeGeneration()
                + "push 1\n"
                ;

            if (_elseNode != null)
            {
                result += "beq " 
                    + then 
                    + "\n" 
                    + _elseNode.CodeGeneration() 
                    + "b " 
                    + exit 
                    + "\n" 
                    + then 
                    + ":\n"
                    + _thenNode.CodeGeneration() 
                    + exit 
                    + ":\n"
                    ;
            }
            else
            {
                result += "beq " 
                    + then 
                    + "\n" 
                    + "push -10000000\n" 
                    + "b " 
                    + exit 
                    + "\n" 
                    + then 
                    + ":\n" 
                    + _thenNode.CodeGeneration() 
                    + exit 
                    + ":\n"
                    ;
            }

            return result;
        }

        public IFoolType TypeCheck()
        {
            if (!_conditionNode.TypeCheck().IsSubType(new FoolBooleanType()))
            {
                throw new FoolTypeException("Oops... The condition of \"if-branch\" is not a Boolean!\n");
            }

            IFoolType thenType = _thenNode.TypeCheck();
            if (_elseNode == null)
            {
                return thenType;
            }

            IFoolType elseType = _elseNode.TypeCheck();
            if (thenType.IsSubType(elseType))
            {
                return elseType;
            }
            else if (elseType.IsSubType(thenType))
            {
                return thenType;
            }
            else
            {
                throw new FoolTypeException("Oops... Incompatible types in the \"then\" and \"else\" branches!\n");
            }
        }
    }
}
