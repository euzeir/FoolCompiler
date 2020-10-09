using System;
using System.Collections.Generic;
using FoolCompiler.CodeGeneration;
using FoolCompiler.ExceptionHandling;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    public class FoolClassNode : IFoolNode
    {
        protected string _id;
        protected List<FoolParameterNode> _fieldList;
        protected List<FoolMethodNode> _methodList;
        private string _superClassId;
        protected FoolClassType _classType;

        public FoolClassNode(string id, List<FoolParameterNode> fieldList, List<FoolMethodNode> methodList)
        {
            _id = id;
            _fieldList = fieldList;
            _methodList = methodList;
            _superClassId = null;
        }
        public FoolClassNode(string id, string superClassId, List<FoolParameterNode> fieldList, List<FoolMethodNode> methodList)
        {
            _id = id;
            _fieldList = fieldList;
            _methodList = methodList;
            _superClassId = superClassId;
        }
        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();
            List<FoolFieldType> symbolTableEntryFieldList = new List<FoolFieldType>();
            List<FoolMethodType> symbolTableEntryMethodList = new List<FoolMethodType>();
            Dictionary<string, IFoolType> parameterMapping = new Dictionary<string, IFoolType>();
            Dictionary<string, FoolFunctionType> methodMapping = new Dictionary<string, FoolFunctionType>();

            foreach (FoolParameterNode parameter in _fieldList)
            {
                var addToFieldList = new FoolFieldType(parameter.GetId(), parameter.GetFoolType());
                symbolTableEntryFieldList.Add(addToFieldList);
                parameterMapping.Add(parameter.GetId(), parameter.GetFoolType());
            }
            foreach (FoolMethodNode method in _methodList)
            {
                List<IFoolType> parameterType = new List<IFoolType>();
                foreach (FoolParameterNode parameter in method.GetFoolParameterNodes())
                {
                    if (parameter.GetFoolType() is FoolObjectType)
                    {
                        FoolObjectType parameterObjectType = (FoolObjectType)parameter.GetFoolType();
                        string declaredClass = parameterObjectType.GetFoolClassType().GetFoolClassName();
                        try
                        {
                            parameterType.Add(new FoolObjectType((FoolClassType) environment.CheckDeclaredName(declaredClass).GetFoolType()));
                        }
                        catch (NotDeclaredNameErrorException)
                        {
                            result.Add("Class " + declaredClass + " is not declared!\n");
                        }
                    }
                    else
                    {
                        parameterType.Add(parameter.GetFoolType());
                    }
                }
                FoolMethodType foolMethodType = new FoolMethodType(method.GetId(), new FoolFunctionType(parameterType, method.GetFoolType()));
                symbolTableEntryMethodList.Add(foolMethodType);
                methodMapping.Add(method.GetId(), new FoolFunctionType(parameterType, method.GetFoolType()));
            }

            FoolClassType superType = null;
            List<string> inheritedMethods = new List<string>();
            if (_superClassId != null)
            {
                try
                {
                    superType = (FoolClassType)environment.CheckDeclaredName(_superClassId).GetFoolType();
                    foreach (string str in superType.GetMethodsMapping().Keys)
                    {
                        if (!methodMapping.ContainsKey(str))
                        {
                            inheritedMethods.Add(str);
                        }
                    }
                }
                catch (NotDeclaredNameErrorException e)
                {
                    result.Add("Superclass " + e + " not declared!\n");
                    return result;
                }
            }

            _classType = new FoolClassType(_id, superType, symbolTableEntryFieldList, symbolTableEntryMethodList);
            try
            {
                environment.SetDeclarationType(_id, _classType, 0);
            }
            catch (NotDeclaredNameErrorException e)
            {
                result.Add(e.Message);
            }

            environment.InsertNewScope();

            foreach (string str in inheritedMethods)
            {
                try
                {
                    environment.InsertDeclaration(str, superType.GetMethodsMapping()[str], superType.GetMethodsOffsetMapping(str));
                }
                catch (Exception e)
                {
                    if (e is NotDeclaredNameErrorException || e is MultipleNameDeclarationErrorException)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }
            foreach (FoolMethodNode foolMethodNode in _methodList)
            {
                List<IFoolType> foolTypeList = new List<IFoolType>();
                foreach (FoolParameterNode parameter in foolMethodNode.GetFoolParameterNodes())
                {
                    foolTypeList.Add(parameter.GetFoolType());
                }
                try
                {
                    string str = foolMethodNode.GetId();
                    FoolFunctionType foolFunctionType = new FoolFunctionType(foolTypeList, foolMethodNode.GetFoolType());
                    int updatedUpdate = environment.GetOffset() + 1;
                    environment.InsertDeclaration(str, foolFunctionType, updatedUpdate);
                    environment.DecrementOffset();
                }
                catch (MultipleNameDeclarationErrorException)
                {
                    result.Add("Oops... Method [ " + foolMethodNode.GetId() + " ] is already declared!\n");
                    return result;
                }
            }

            environment.RemoveScope();

            if (_superClassId != null)
            {
                try
                {
                    if (!(environment.GetFoolType(_superClassId) is FoolClassType))
                    {
                        result.Add("Oops... Class " + _superClassId + " is not defined!\n");
                    }
                }
                catch (NotDeclaredNameErrorException)
                {
                    result.Add("Oops... Class " + _superClassId + " is not declared!\n");
                }
                if (_fieldList.Count >= superType.GetFieldList().Count)
                {
                    for (int i = 0; i < superType.GetFieldList().Count; i++)
                    {
                        FoolParameterNode currentParameter = _fieldList[i];
                        FoolFieldType superClassField = superType.GetFieldList()[i];
                        if (!superClassField.GetId().Equals(currentParameter.GetId()) || !superClassField.GetFoolType().GetFoolType().Equals(currentParameter.GetFoolType().GetFoolType()))
                        {
                            result.Add("Oops... Overriding of the fields are not allowed " + currentParameter.GetId() + " defined in the superclass!\n");
                        }
                    }
                }
                else
                {
                    result.Add("Subclass " + _id + " have different number of parameters from the superclass " + _superClassId + "!\n");
                }
                try
                {
                    SymbolTableEntry superClasEntry = environment.CheckDeclaredName(_superClassId);
                    Dictionary<string, IFoolType> superClassMethodMap = superType.GetMethodsMapping();

                    foreach (string method in methodMapping.Keys)
                    {
                        if (superClassMethodMap.ContainsKey(method))
                        {
                            if (!methodMapping[method].IsSubType(superClassMethodMap[method]))
                            {
                                result.Add("Override of" + method + " in class " + _id + " is not allowed!\n");
                            }
                        }
                    }
                }
                catch (NotDeclaredNameErrorException)
                {
                    result.Add("Superclass " + _superClassId + " is not defined!\n");
                }
            }
            return result;
        }

        public string CodeGeneration()
        {
            List<FoolDispatchTableEntry> dispatchTable;
            Dictionary<string, string> superClassMethodsMapping = new Dictionary<string, string>();
            Dictionary<string, string> currentClassMethodsMapping = new Dictionary<string, string>();

            if (_superClassId != null)
            {
                dispatchTable = FoolDispatchTable.GetDispatchTable(_superClassId);
            }
            else
            {
                dispatchTable = new List<FoolDispatchTableEntry>();
            }

            foreach (FoolDispatchTableEntry dTE in dispatchTable)
            {
                superClassMethodsMapping.Add(dTE.GetMethodId(), dTE.GetMethodLabel());
            }

            foreach (FoolMethodNode method in _methodList)
            {
                currentClassMethodsMapping.Add(method.GetId(), method.CodeGeneration());
            }

            for (int i = 0; i < dispatchTable.Count; i++)
            {
                string OldMethodName = dispatchTable[i].GetMethodId();
                string NewMethodName = currentClassMethodsMapping[OldMethodName];
                if (NewMethodName != null)
                {
                    dispatchTable[i] = new FoolDispatchTableEntry(OldMethodName, NewMethodName);
                }
            }
            foreach (FoolMethodNode method in _methodList)
            {
                string currentMethodId = method.GetId();
                if (superClassMethodsMapping[currentMethodId] == null)
                {
                    dispatchTable.Add(new FoolDispatchTableEntry(currentMethodId, currentClassMethodsMapping[currentMethodId]));
                }
            }
            return string.Empty;
        }

        public IFoolType TypeCheck()
        {
            try
            {
                foreach (FoolParameterNode parameter in _fieldList)
                {
                    parameter.TypeCheck();
                }
                foreach (FoolMethodNode method in _methodList)
                {
                    method.TypeCheck();
                }
            }
            catch
            {
                throw new FoolTypeException("Oops... TypeChecking error!");
            }
            return _classType;
        }

        public List<string> CheckMethods(FoolEnvironment environment)
        {
            List<string> result = new List<string>();

            environment.InsertNewScope();
            foreach(FoolParameterNode parameterNode in _fieldList)
            {
                if (parameterNode.GetFoolType() is FoolObjectType)
                {
                    FoolObjectType objectType = (FoolObjectType)parameterNode.GetFoolType();
                    try
                    {
                        environment.CheckDeclaredName(objectType.GetFoolClassType().GetFoolClassName());
                    }
                    catch (NotDeclaredNameErrorException)
                    {
                        result.Add("Oops... Class " + objectType.GetFoolClassType().GetFoolClassName() + " is not declared!\n");
                    }
                    FoolClassType subClassAsParameter = objectType.GetFoolClassType();
                    if (subClassAsParameter.IsSubType(this._classType))
                    {
                        result.Add("Oops... Cannot use subclass in superclass constructor!\n");
                    }
                }
                result.AddRange(parameterNode.CheckSemantics(environment));
            }
            foreach (FoolMethodNode methodNode in _methodList)
            {
                result.AddRange(methodNode.CheckSemantics(environment));
            }
            environment.RemoveScope();
            return result;
        }

        public string GetId()
        {
            return _id;
        }

        public List<FoolParameterNode> GetFieldList()
        {
            return _fieldList;
        }

        public List<FoolMethodNode> GetMethodList()
        {
            return _methodList;
        }

        public string GetSuperClassId()
        {
            return _superClassId;
        }
    }
}
