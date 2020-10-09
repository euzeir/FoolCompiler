using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;
using System.Collections.Generic;

namespace FoolCompiler.Ast
{
    public interface IFoolNode
    {
        IFoolType TypeCheck();
        List<string> CheckSemantics(FoolEnvironment environment);
        string CodeGeneration();
    }
}
