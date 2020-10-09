using System.Collections.Generic;
using System.Text;
using FoolCompiler.ExceptionHandling;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    public class FoolIdNode : IFoolNode
    {
        private string _id;
        private SymbolTableEntry _symbolTableEntry;
        private int _nestingLevel;
        private bool _isNegative;
        private string _operator;
        private bool _isBoolean;

        public FoolIdNode(string id, bool isNegative, string op)
        {
            _id = id;
            _isNegative = isNegative;
            _operator = op;
            _isBoolean = false;
        }


        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();

            try
            {
                _symbolTableEntry = environment.ControlFunction(_id);
                if (_symbolTableEntry.GetFoolType() is FoolObjectType)
                {
                    FoolObjectType declaredType = (FoolObjectType)_symbolTableEntry.GetFoolType();
                    result.AddRange(declaredType.UpdateFoolClassType(environment));
                }
                _nestingLevel = environment.GetNestingLevel();
            }
            catch (NotDeclaredNameErrorException e)
            {
                result.Add(e.Message);
            }
            return result;
        }

        public string CodeGeneration()
        {
            int i;
            string code = string.Empty;
            StringBuilder GetAR = new StringBuilder();

            for (i = 0; i < _nestingLevel - _symbolTableEntry.GetNestingLevel(); i++)
                GetAR.Append("lw\n");

            code = "push " +
                _symbolTableEntry.GetOffset() +
                "\n" +
                "lfp\n" +
                "add\n" +
                "lw\n";

            if (_isNegative)
            {
                if (_isBoolean)
                {
                    code = "push 1\n" +
                        code +
                        "sub\n";
                }
                else
                {
                    code += "push -1\nmult\n";
                }
            }
            return code;
        }

        public IFoolType TypeCheck()
        {
            if (_symbolTableEntry.GetFoolType() is FoolFunctionType)
            {
                throw new FoolTypeException("Oops... Error: Wrong usage of function identifier: " + _id + "\n");
            }
            if (_isNegative)
            {
                if (_operator.Equals("-") && _symbolTableEntry.GetFoolType().GetFoolType().Equals("bool"))
                {
                    throw new FoolTypeException("Oops... Error: Wrong usage of \"-\" for the variable " + _id + "\n");
                }
                else
                {
                    if (_operator.Equals("not") && _symbolTableEntry.GetFoolType().GetFoolType().Equals("int"))
                    {
                        throw new FoolTypeException("Oops... Error: Wrong usage of \"not\"! You cannot do negation to the variable " + _id + "\n"); 
                    }
                }
            }
            if (_symbolTableEntry.GetFoolType().GetFoolType().Equals("bool"))
            {
                _isBoolean = true;
            }
            return _symbolTableEntry.GetFoolType();
        }

        public string GetIdName()
        {
            return _id;
        }

        public SymbolTableEntry GetEntry()
        {
            return _symbolTableEntry;
        }
    }
}
