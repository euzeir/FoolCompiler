using System.Collections.Generic;

namespace FoolCompiler.TypeDefinition
{
    public class FoolFunctionType: IFoolType
    {
        private List<IFoolType> _parameters;
        private IFoolType _returnType;
        public FoolFunctionType(List<IFoolType> parameters, IFoolType returnType)
        {
            _parameters = parameters;
            _returnType = returnType;
        }


        public string GetId()
        {
            return "Return";
        }

        public bool IsSubType(IFoolType type)
        {
            bool res = false;
            if (type is FoolFunctionType)
            {
                FoolFunctionType functionType = (FoolFunctionType)type;
                res = true;
                if (_parameters.Count == functionType.GetParameters().Count)
                {
                    for (int i = 0; i <= _parameters.Count; i++)
                    {
                        bool isSubTypeOf = functionType.GetParameters()[i].IsSubType(_parameters[i]);
                        res = res && isSubTypeOf;
                    }
                    res = res && _returnType.IsSubType(functionType.GetReturnType());
                }
                else
                {
                    res = false;
                }
                return res;
            }
            else
            {
                res = false;
                return res;
            }
        }

        public string GetFoolType()
        {
            string ret = "fun";
            return ret;
        }

        public List<IFoolType> GetParameters()
        {
            return _parameters;
        }

        public IFoolType GetReturnType()
        {
            return _returnType;
        }
    }
}
