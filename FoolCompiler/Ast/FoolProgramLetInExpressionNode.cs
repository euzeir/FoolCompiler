using System.Collections.Generic;
using System.Text;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Lib;
using FoolCompiler.Utils;
using FoolCompiler.ExceptionHandling;

namespace FoolCompiler.Ast
{
    class FoolProgramLetInExpressionNode : IFoolNode
    {
        private List<IFoolNode> _declarationList;
        private List<IFoolNode> _statements;
        private IFoolNode _expression;

        public FoolProgramLetInExpressionNode(List<IFoolNode> declarationList, List<IFoolNode> statements)
        {
            _declarationList = declarationList;
            _expression = null;
            _statements = statements; 
        }

        public FoolProgramLetInExpressionNode(List<IFoolNode> declarationList, IFoolNode toVisit)
        {
            _declarationList = declarationList;
            _expression = toVisit;
            _statements = null;
        }
        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();
            environment.InsertNewScope();

            if (_declarationList.Count > 0)
            {
                environment.SetOffsetValue(-1);

                foreach(IFoolNode foolNode in _declarationList)
                {
                    if (foolNode is FoolFunctionNode)
                    {
                        FoolFunctionNode functionNode = (FoolFunctionNode)foolNode;
                        List<IFoolType> parameterTypes = new List<IFoolType>();

                        foreach (FoolParameterNode parameterNode in functionNode.GetFunctionParameters())
                        {
                            parameterTypes.Add(parameterNode.GetFoolType());
                        }

                        try
                        {
                            environment.InsertDeclaration(functionNode.GetId(), new FoolFunctionType(parameterTypes, functionNode.GetFoolType()), environment.GetOffset());
                            environment.DecrementOffset();
                        }
                        catch (MultipleNameDeclarationErrorException)
                        {
                            result.Add("Oops... Function [ " + functionNode.GetId() + " ] is already declare\n");
                            return result;
                        }
                    }
                }
                foreach (IFoolNode foolNode in _declarationList)
                {
                    result.AddRange(foolNode.CheckSemantics(environment));
                }
            }
            if (_expression != null)
            {
                result.AddRange(_expression.CheckSemantics(environment));
            }
            else
            {
                if (_statements.Count > 0)
                {
                    foreach (IFoolNode foolNode in _statements)
                    {
                        result.AddRange(foolNode.CheckSemantics(environment));
                    }
                }
            }
            environment.RemoveScope();
            return result;
        }

        public string CodeGeneration()
        {
            StringBuilder declarationCodeGeneration = new StringBuilder();

            foreach (IFoolNode declarations in _declarationList)
            {
                if (declarations is FoolFunctionNode)
                {
                    declarationCodeGeneration.Append(declarations.CodeGeneration());
                }
            }
            foreach (IFoolNode declarations in _declarationList)
            {
                if (!(declarations is FoolFunctionNode))
                {
                    declarationCodeGeneration.Append(declarations.CodeGeneration());
                }
            }

            if (_expression != null)
            {
                return declarationCodeGeneration
                    + _expression.CodeGeneration()
                    + "halt\n"
                    + FoolLib.GetCode()
                    ;
            }
            else
            {
                StringBuilder statementsCodeGeneration = new StringBuilder();

                foreach (IFoolNode statement in _statements)
                {
                    statementsCodeGeneration.Append(statement.CodeGeneration());
                }
                return declarationCodeGeneration.ToString()
                    + statementsCodeGeneration
                    + "halt\n"
                    + FoolLib.GetCode();
            }
        }

        public IFoolType TypeCheck()
        {
            if (_expression != null)
            {
                foreach (IFoolNode foolNode in _declarationList)
                {
                    foolNode.TypeCheck();
                }
                return _expression.TypeCheck();
            }
            else
            {
                foreach (IFoolNode foolNode in _declarationList)
                {
                    foolNode.TypeCheck();
                }
                foreach (IFoolNode foolNode in _statements)
                {
                    foolNode.TypeCheck();
                }
                return new FoolVoidType();
            }
        }
    }
}
