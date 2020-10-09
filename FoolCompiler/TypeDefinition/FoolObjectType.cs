using FoolCompiler.Utils;
using FoolCompiler.ExceptionHandling;
using System.Collections.Generic;

namespace FoolCompiler.TypeDefinition
{
    public class FoolObjectType : IFoolType
    {
        private FoolClassType _foolClassType;

        public FoolObjectType(FoolClassType foolClassType)
        {
            _foolClassType = foolClassType;
        }
        public FoolClassType GetFoolClassType()
        {
            return _foolClassType;
        }
        public string GetId()
        {
            string ret = "Object";
            return ret;
        }

        public bool IsSubType(IFoolType foolType)
        {
            if (foolType is FoolObjectType)
            {
                return _foolClassType.IsSubType(((FoolObjectType)foolType).GetFoolClassType());
            }
            return false;
        }

        public string GetFoolType()
        {
            string res = string.Empty;
            res = "object " + _foolClassType.GetFoolClassName();
            return res;
        }

        public List<string> UpdateFoolClassType(FoolEnvironment environment)
        {
            List<string> result = new List<string>();
            try
            {
                _foolClassType = (FoolClassType)environment.GetFoolType(_foolClassType.GetFoolClassName());
            }
            catch (NotDeclaredNameErrorException e)
            {
                try
                {
                    throw new NotDeclaredNameErrorException(_foolClassType.GetFoolClassName());
                }
                catch (NotDeclaredNameErrorException ee)
                {
                    result.Add(ee.Message);
                }
            }
            return result;
        }
    }
}
