using System.Collections.Generic;
using System.Text;
using FoolCompiler.ExceptionHandling;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    public class FoolFunctionCallNode : IFoolNode
    {
        protected string _id;
        protected List<IFoolNode> _parameterList;
        protected SymbolTableEntry _symbolTableEntry;
        protected int _nestingLevel;

        public FoolFunctionCallNode(string id, List<IFoolNode> argumentList)
        {
            _id = id;
            _parameterList = argumentList;
        }

        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();

            try
            {
                _symbolTableEntry = environment.CheckDeclaredName(_id);
            }
            catch (NotDeclaredNameErrorException)
            {
                result.Add("Oops... Function" + "[" + _id + "()]" + " is not declared\n");
            }
            _nestingLevel = environment.GetNestingLevel();

            foreach (IFoolNode arguments in _parameterList)
            {
                result.AddRange(arguments.CheckSemantics(environment));
            }
            return result;
        }

        public string CodeGeneration()
        {
            StringBuilder parameterCodeGeneration = new StringBuilder();
            StringBuilder GetAR = new StringBuilder();

            for (int i = _parameterList.Count - 1; i >= 0; i--)
            {
                parameterCodeGeneration.Append(_parameterList[i].CodeGeneration());
            }

            if (_nestingLevel - _symbolTableEntry.GetNestingLevel() > 0)
            {
                GetAR.Append("lfp\n");
                for (int j = 0; j < _nestingLevel - _symbolTableEntry.GetNestingLevel(); j++)
                {
                    GetAR.Append("lw\n");
                }
                GetAR.Append("sfp\n");
            }
            else
            {
                GetAR.Append(string.Empty);
            }

            return "lfp\n" 
                + parameterCodeGeneration 
                + GetAR 
                + "lfp\n" 
                + "push " 
                + _symbolTableEntry.GetOffset()
                + "\n"
                + "lfp\n" 
                + "add\n" 
                + "lw\n" 
                + "js\n"
                ;
            }


        public IFoolType TypeCheck()
        {
            FoolFunctionType functionType = null;

            if (_symbolTableEntry.GetFoolType().GetId().Equals("Return"))
            {
                functionType = (FoolFunctionType)_symbolTableEntry.GetFoolType();
            }
            else
            {
                throw new FoolTypeException("Oops... Illegal call, " + "[" + _id + "]" + " is not a function");
            }

            List<IFoolType> parameters = functionType.GetParameters();

            if (parameters.Count != _parameterList.Count)
            {
                throw new FoolTypeException("Oops... Wrong number of parameters when calling function: " + "[" + _id + "]");
            }

            for (int i = 0; i < parameters.Count; i++)
            {
                if (!_parameterList[i].TypeCheck().IsSubType(parameters[i]))
                {
                    throw new FoolTypeException("Oops... Wrong type of parameter in position " + (i + 1) + " while calling function " + "[" + _id + "]");
                }
            }
            return functionType.GetReturnType();
        }

        public List<IFoolNode> GetArguments()
        {
            return _parameterList;
        }

        public string GetId()
        {
            return _id;
        }
    }
}

