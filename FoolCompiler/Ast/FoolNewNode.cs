using System.Collections.Generic;
using System.Text;
using FoolCompiler.ExceptionHandling;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    public class FoolNewNode : IFoolNode
    {
        private string _id;
        private List<IFoolNode> _parameterList;
        private FoolClassType _classType;

        public FoolNewNode(string id, List<IFoolNode> arguments)
        {
            _id = id;
            _parameterList = arguments;
        }
        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();

            try
            {
                _classType = (FoolClassType)environment.CheckDeclaredName(_id).GetFoolType();
            }
            catch (NotDeclaredNameErrorException)
            {
                result.Add("Oops... Class [ " + _id + " ] is not declared!\n");
                return result;
            }

            if (_parameterList.Count != _classType.GetFieldList().Count)
            {
                return result;
            }

            if (_parameterList.Count > 0)
            {
                foreach (IFoolNode foolNode in _parameterList)
                {
                    foolNode.CheckSemantics(environment);
                }
            }

            return result;
        }

        public string CodeGeneration()
        {
            StringBuilder parameterCodeGeneration = new StringBuilder();
            foreach (IFoolNode argument in _parameterList)
            {
                parameterCodeGeneration.Append(argument.CodeGeneration());
            }
            return parameterCodeGeneration
                + "push " 
                + _parameterList.Count 
                + "\n"
                + "push class"
                + _id 
                + "\n" 
                + "new\n"
                ;
        }

        public IFoolType TypeCheck()
        {
            List<FoolFieldType> fieldList = _classType.GetFieldList();

            if (_parameterList.Count == _classType.GetFieldList().Count)
            {
                for (int i = 0; i < _parameterList.Count; i++)
                {
                    IFoolType parameterType = _parameterList[i].TypeCheck();
                    IFoolType fieldType = fieldList[i].GetFoolType();

                    if (!parameterType.IsSubType(fieldType))
                    {
                        int var = (i + 1) % 10;
                        switch (var)
                        {
                            case 1:
                                throw new FoolTypeException("Oops... Wrong type for the [ " + (i + 1) + "-st ] parameter" +
                                        "inside the constructor of :: " + _id);
                            case 2:
                                throw new FoolTypeException("Oops... Wrong type for the [ " + (i + 1) + "-nd ] parameter" +
                                        "inside the constructor of :: " + _id);
                            case 3:
                                throw new FoolTypeException("Oops... Wrong type for the [ " + (i + 1) + "-rd ] parameter" +
                                        "inside the constructor of :: " + _id);
                            default:
                                throw new FoolTypeException("Oops... Wrong type for the [ " + (i + 1) + "-th ] parameter" +
                                        "inside the constructor of :: " + _id);
                        }
                    }
                }
            }
            return new FoolObjectType(_classType);
        }
    }
}
