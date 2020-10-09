using System.Collections.Generic;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    class FoolProgramSingleExpressionNode : IFoolNode
    {
        private IFoolNode _expression;

        public FoolProgramSingleExpressionNode(IFoolNode expression)
        {
            _expression = expression;
        }
        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            return _expression.CheckSemantics(environment);
        }

        public string CodeGeneration()
        {
            return _expression.CodeGeneration() + "halt\n";
        }

        public IFoolType TypeCheck()
        {
            return _expression.TypeCheck();
        }
    }
}
