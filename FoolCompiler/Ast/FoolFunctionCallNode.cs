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

        public virtual List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();
            _nestingLevel = environment.GetNestingLevel();

            try
            {
                _symbolTableEntry = environment.CheckDeclaredName(_id);
            }
            catch (NotDeclaredNameErrorException)
            {
                result.Add("Oops... Function" + "[" + _id + "()]" + " is not declared\n");
            }

            foreach (IFoolNode argument in _parameterList)
            {
                result.AddRange(argument.CheckSemantics(environment));
            }
            return result;
        }

        public virtual string CodeGeneration()
        {
            StringBuilder parameterCodeGeneration = new StringBuilder();
            StringBuilder getActivationRecord = new StringBuilder();

            //parameters are always considered in reverse order
            for (int i = _parameterList.Count - 1; i >= 0; i--)
            {
                parameterCodeGeneration.Append(_parameterList[i].CodeGeneration());
            }

            if (_nestingLevel - _symbolTableEntry.GetNestingLevel() > 0)
            {
                getActivationRecord.Append("lfp\n");
                for (int j = 0; j < _nestingLevel - _symbolTableEntry.GetNestingLevel(); j++)
                {
                    getActivationRecord.Append("lw\n");
                }
                getActivationRecord.Append("sfp\n");
            }
            else
            {
                getActivationRecord.Append(string.Empty);
            }

            return "lfp\n" 
                + parameterCodeGeneration 
                + getActivationRecord 
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


        public virtual IFoolType TypeCheck()
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

