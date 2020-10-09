using FoolCompiler.ExceptionHandling;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;
using System.Collections.Generic;
using System.Text;

namespace FoolCompiler.Ast
{
    public class FoolAsmStmExpressionNode : IFoolNode
    {
        private string _id;
        private IFoolNode _expressionBody;
        private SymbolTableEntry _symbolTableEntry;
        private int _nestingLevel;

        public FoolAsmStmExpressionNode(string body, IFoolNode expression)
        {
            _id = body;
            _expressionBody = expression;
        }
        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();
            try
            {
                result.AddRange(_expressionBody.CheckSemantics(environment));
                _symbolTableEntry = environment.CheckDeclaredName(_id);
                _nestingLevel = environment.GetNestingLevel();
            }
            catch (NotDeclaredNameErrorException e)
            {
                result.Add("Oops... \"Assignment Failed\" " + e.Message + "\n");
            }
            return result;
        }

        public string CodeGeneration()
        {
            string resultingExpression = string.Empty;
            StringBuilder Generate = new StringBuilder();
            for (int i = 0; i < (_nestingLevel - _symbolTableEntry.GetNestingLevel()); i++)
            {
                Generate.Append("lw\n");
            }
            resultingExpression = _expressionBody.CodeGeneration() +
                "push" + _symbolTableEntry.GetOffset() + "\n" +
                "lfp\n" + Generate + 
                "add\n" + 
                "sw\n";
            return resultingExpression;
        }

        //public string DoPrint(string indent)
        //{
        //    throw new NotImplementedException();
        //}

        public IFoolType TypeCheck()
        {
            if (_symbolTableEntry.GetFoolType() is FoolObjectType && _expressionBody is FoolNullNode)
            {
                throw new FoolTypeException("You can't use NULL for variable " + _id + "\n");
            }
            IFoolType expressionType = _expressionBody.TypeCheck();
            IFoolType idType = _symbolTableEntry.GetFoolType();

            if (!expressionType.IsSubType(idType))
            {
                throw new FoolTypeException("Incompatible values for " + _id + "\n");
            }
            return new FoolVoidType();
        }
    }
}
