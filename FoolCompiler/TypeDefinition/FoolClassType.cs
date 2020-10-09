using FoolCompiler.ExceptionHandling;
using System.Collections.Generic;

namespace FoolCompiler.TypeDefinition
{
    public class FoolClassType : IFoolType
    {
        private string _className = string.Empty;
        private FoolClassType _superType = null;
        private List<FoolFieldType> _fieldList = new List<FoolFieldType>();
        private List<FoolMethodType> _methodList = new List<FoolMethodType>();

        public FoolClassType(string className, FoolClassType superType, List<FoolFieldType> fields, List<FoolMethodType> methods )
        {
            _className = className;
            _superType = superType;
            _fieldList = fields;
            _methodList = methods;
        }
        public FoolClassType(string className)
        {
            _className = className;
        }
        public string GetId()
        {
            string res = string.Empty;
            res = "Class";
            return res;
        }
        public bool IsSubType(IFoolType foolType)
        {
            if (foolType is FoolClassType)
            {
                FoolClassType classType = (FoolClassType)foolType;
                if (this.GetFoolClassName().Equals(classType.GetFoolClassName()))
                {
                    return true;
                }
                if (_superType != null)
                {
                    FoolClassType superClassType = GetSuperType();
                    if (superClassType.IsSubType(foolType))
                    {
                        return true;
                    }
                    if (classType.GetSuperClassName() != null)
                    {
                        if (superClassType.GetFoolClassName().Equals(classType.GetSuperClassName()))
                        {
                            return true;
                        }
                    }
                    while (superClassType.GetSuperType() != null)
                    {
                        superClassType = superClassType.GetSuperType();
                        if (superClassType.GetFoolClassName().Equals(classType.GetFoolClassName()))
                        {
                            return true;
                        }
                        while (classType.GetSuperType() != null)
                        {
                            classType = classType.GetSuperType();
                            if (superClassType.GetFoolClassName().Equals(classType.GetFoolClassName()))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public string GetFoolType()
        {
            string res = string.Empty;
            res = "class " + _className;
            return res;
        }
        public void setSuperType(FoolClassType superType)
        {
            _superType = superType;
        }
        public string GetFoolClassName()
        {
            return _className;
        }
        public FoolClassType GetSuperType()
        {
            return _superType;
        }
        public string GetSuperClassName()
        {
            return _superType.GetFoolClassName();
        }
        public List<FoolFieldType> GetFieldList()
        {
            return _fieldList;
        }
        public List<FoolMethodType> GetMethodList()
        {
            return _methodList;
        }

        public Dictionary<string, IFoolType> GetMethodsMapping()
        {
            Dictionary<string, IFoolType> methodsMapping = new Dictionary<string, IFoolType>();
            if (GetSuperType() != null)
            {
                Dictionary<string, IFoolType> superMethodsMapping = GetSuperType().GetMethodsMapping();
                foreach (string method in superMethodsMapping.Keys)
                {
                    methodsMapping.Add(method, superMethodsMapping[method]);
                }
                foreach (FoolMethodType method in _methodList)
                {
                    methodsMapping.Add(method.GetId(), method.GetFoolType());
                }
            }
            return methodsMapping;
        }

        public int GetMethodsOffsetMapping(string methodName)
        {
            Dictionary<string, int> methodsMapping = new Dictionary<string, int>();
            var offset = methodsMapping[methodName];
            try
            {
                if (offset != 0)
                {
                    return offset++;
                }
            }
            catch
            {
                throw new NotDeclaredNameErrorException(methodName);
            }
            return offset;
        }

        public Dictionary<string, int> SuperClassMethodsMappint()
        {
            if (GetSuperType() == null)
            {
                Dictionary<string, int> methodsMapping = new Dictionary<string, int>();
                foreach (FoolMethodType method in _methodList)
                {
                    methodsMapping.Add(method.GetId(), methodsMapping.Count);
                }
                return methodsMapping;
            }
            else
            {
                Dictionary<string, int> superMethodsMapping = new Dictionary<string, int>();
                foreach (FoolMethodType method in _methodList)
                {
                    if (!superMethodsMapping.ContainsKey(method.GetId()))
                    {
                        superMethodsMapping.Add(method.GetId(), superMethodsMapping.Count);
                    }
                }
                return superMethodsMapping;
            }
        }

        public IFoolType GetMethodType(string methodName)
        {
            FoolMethodType methodType = null;
            foreach (FoolMethodType methodT in _methodList)
            {
                if (methodT.GetId().Equals(methodName))
                {
                    methodType = methodT;
                }
            }
            if (methodType != null)
            {
                return methodType.GetFoolType();
            }
            else
            {
                return GetSuperType().GetMethodType(methodName);
            }
        }
    }
}
