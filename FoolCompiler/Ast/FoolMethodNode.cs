using System;
using System.Collections.Generic;
using System.Text;
using FoolCompiler.Lib;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;
using FoolCompiler.ExceptionHandling;

namespace FoolCompiler.Ast
{
    public class FoolMethodNode : FoolFunctionNode
    {
        protected string _className;

        public FoolMethodNode(string id, IFoolType foolType, List<FoolParameterNode> parameterList, List<IFoolNode> declarationList, List<IFoolNode> statementExpressions) : base(id, foolType, parameterList, declarationList, statementExpressions)
        {
        }
        public void SetClassName(string className)
        {
            _className = className;
        }
        public List<FoolParameterNode> GetFoolParameterNodes()
        {
            return _parameterList;
        }
        
        new public List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();
            List<IFoolType> parameterTypeList = new List<IFoolType>();

            foreach(FoolParameterNode paremeter in _parameterList)
            {
                parameterTypeList.Add(paremeter.GetFoolType());
            }
            if (_foolType is FoolObjectType)
            {
                FoolObjectType objectType = (FoolObjectType)_foolType;
                result.AddRange(objectType.UpdateFoolClassType(environment));
            }
            environment.InsertNewScope();
            try
            {
                SymbolTableEntry symbolTableEntry = environment.CheckDeclaredName(_className);
                environment.InsertDeclaration("this", new FoolObjectType((FoolClassType)symbolTableEntry.GetFoolType()), 0);
            }
            catch (Exception e)
            {
                if (e is NotDeclaredNameErrorException || e is MultipleNameDeclarationErrorException)
                {
                    Console.WriteLine("Oops! The name is not declared or is declared multiple times. {0}", e.Message);
                }
            }
            foreach (FoolParameterNode paremeter in _parameterList)
            {
                result.AddRange(paremeter.CheckSemantics(environment));
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

        new public IFoolType TypeCheck()
        {
            return base.TypeCheck();
        }

        new public string CodeGeneration()
        {
            StringBuilder addLocal = new StringBuilder();
            StringBuilder popLocal = new StringBuilder();
            StringBuilder popInputArgumentes = new StringBuilder();
            StringBuilder addBody = new StringBuilder();
            string functionLabel = FoolLib.CreateFreshFunctionLabel();

            foreach (IFoolNode declaration in _declarationList)
            {
                addLocal.Append(declaration.CodeGeneration());
                popLocal.Append("pop\n");
            }
            foreach (IFoolNode parameter in _parameterList)
            {
                popInputArgumentes.Append("pop\n");
            }
            foreach (IFoolNode statements in _statementExpressions)
            {
                addBody.Append(statements.CodeGeneration());
            }
            FoolLib.PutCode(functionLabel + ":\n" +
                "cfp\n" +
                "lra\n" +
                addLocal +
                addBody +
                "srv\n" +
                popLocal +
                "sra\n" +
                "pop\n" +
                popInputArgumentes +
                "sfp\n" +
                "lrv\n" +
                "lra\n" +
                "js\n");
            return functionLabel + "\n";
        }
    }
}
