using System.Collections.Generic;
using System.Text;
using FoolCompiler.ExceptionHandling;
using FoolCompiler.Lib;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    public class FoolFunctionNode : IFoolNode
    {
        protected string _id;
        protected IFoolType _foolType;
        protected List<FoolParameterNode> _parameterList;
        protected List<IFoolNode> _declarationList;
        protected List<IFoolNode> _statementExpressions;

        public FoolFunctionNode(string id, IFoolType foolType, List<FoolParameterNode> parameterList, List<IFoolNode> declarationList, List<IFoolNode> statementExpressions)
        {
            _id = id;
            _foolType = foolType;
            _parameterList = parameterList;
            _declarationList = declarationList;
            _statementExpressions = statementExpressions;
        }
        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();
            List<IFoolType> parameters = new List<IFoolType>();

            foreach (FoolParameterNode parameter in _parameterList)
            {
                parameters.Add(parameter.GetFoolType());
            }
            if (_foolType is FoolObjectType)
            {
                FoolObjectType objectType = (FoolObjectType)_foolType;
                result.AddRange(objectType.UpdateFoolClassType(environment));
            }
            environment.InsertNewScope();
            foreach (FoolParameterNode parameter in _parameterList)
            {
                result.AddRange(parameter.CheckSemantics(environment));
            }
            if (_declarationList.Count > 0)
            {
                environment.SetOffsetValue(-2);
                foreach (IFoolNode foolNode in _declarationList)
                {
                    result.AddRange(foolNode.CheckSemantics(environment));
                }
            }
            foreach (IFoolNode foolNode in _statementExpressions)
            {
                result.AddRange(foolNode.CheckSemantics(environment));
            }
            environment.RemoveScope();
            return result;
        }

        public string CodeGeneration()
        {
            string functionLable = FoolLib.CreateFreshFunctionLabel();
            StringBuilder declCode = new StringBuilder();
            StringBuilder popDecl = new StringBuilder();
            StringBuilder popParl = new StringBuilder();
            StringBuilder stmsExpCode = new StringBuilder();

            foreach (IFoolNode declaration in _declarationList)
            {
                declCode.Append(declaration.CodeGeneration());
                popDecl.Append("pop\n");
            }
            foreach (IFoolNode parameter in _parameterList)
            {
                popParl.Append("pop\n");
            }
            foreach (IFoolNode statement in _statementExpressions)
            {
                stmsExpCode.Append(statement.CodeGeneration());
            }

            FoolLib.PutCode(
                functionLable
                + ":\n" 
                + "cfp\n"
                + "lra\n" 
                + declCode 
                + stmsExpCode 
                + "srv\n" 
                + popDecl 
                + "sra\n" 
                + popParl 
                + "pop\n" 
                + "sfp\n" 
                + "lrv\n" 
                + "lra\n" 
                + "js\n"
                )
                ;
            return "push " + functionLable + "\n";
        }

        public IFoolType TypeCheck()
        {
            List<IFoolType> parameterTypeList = new List<IFoolType>();
            foreach (FoolParameterNode parameter in _parameterList)
            {
                parameterTypeList.Add(parameter.GetFoolType());
            }
            if (_declarationList.Count > 0)
            {
                foreach (IFoolNode foolDeclaration in _declarationList)
                {
                    foolDeclaration.TypeCheck();
                }
            }
            IFoolType bodyType;
            foreach (IFoolNode foolNode in _statementExpressions)
            {
                bodyType = foolNode.TypeCheck();
                if (!bodyType.IsSubType(_foolType))
                {
                    throw new FoolTypeException("Incompatible type returned from function [ " + _id + ". ]");
                }
            }
            return new FoolFunctionType(parameterTypeList, _foolType);
        }

        public void AddNewParameter(IFoolNode parameter)
        {
            FoolParameterNode parameterNode = (FoolParameterNode)parameter;
            _parameterList.Add(parameterNode);
        }

        public List<FoolParameterNode> GetFunctionParameters()
        {
            return _parameterList;
        }

        public string GetId()
        {
            return _id;
        }

        public IFoolType GetFoolType()
        {
            return _foolType;
        }
    }
}
