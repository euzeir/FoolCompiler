using FoolCompiler.ExceptionHandling;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoolCompiler.Ast
{
    public class FoolMethodCallNode: FoolFunctionCallNode
    {
        private string _iid;
        private int _objectOffset;
        private int _objectNestingLevel;
        private IFoolType _methodType;
        private int _methodOffset;

        public FoolMethodCallNode(string id, string methodId, List<IFoolNode> parameters): base(methodId, parameters)
        {
            _iid = id;
        }

        new public List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();

            _nestingLevel = environment.GetNestingLevel();
            FoolClassType classType = null;
            IFoolType objectType;

            if (_iid.Equals("this"))
            {
                objectType = environment.GetLastSymbolTableEntry().GetFoolType();
                _objectOffset = 0;

                if (objectType is FoolClassType)
                {
                    classType = (FoolClassType) objectType;
                    _objectNestingLevel = 3;
                }
            }
            else
            {
                try
                {
                    SymbolTableEntry objectEntry = environment.CheckDeclaredName(_iid);
                    
                    if (!objectEntry.IsInitialized())
                    {
                        Console.WriteLine("The object is not initialized!\nExit.");
                        Environment.Exit(-1);
                    }
                    objectType = objectEntry.GetFoolType();
                    _objectOffset = objectEntry.GetOffset();
                    _objectNestingLevel = objectEntry.GetNestingLevel();

                    try
                    {
                        environment.CheckDeclaredName("this");
                        _nestingLevel--;
                    }
                    catch (NotDeclaredNameErrorException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    if (objectType is FoolObjectType)
                    {
                        classType = ((FoolObjectType)objectType).GetFoolClassType();
                    }
                    else
                    {
                        result.Add("The method " + _iid + " is invoked from a " + "type that is not an Object\n");
                        return result;
                    }
                }
                catch (NotDeclaredNameErrorException e)
                {
                    Console.WriteLine("Message = {0}", e.Message);
                    Console.WriteLine("StackTrace = {0}", e.StackTrace);
                }
                
            }
            try
            {
                SymbolTableEntry classEntry;

                if (_iid.Equals("this"))
                {
                    classEntry = environment.GetLastSymbolTableEntry();
                }
                else
                {
                    classEntry = environment.CheckDeclaredName(classType.GetFoolClassName());
                }

                FoolClassType objectClass = (FoolClassType)classEntry.GetFoolType();
                _methodOffset = objectClass.GetMethodsOffsetMapping(_id);
                _methodType = objectClass.GetMethodType(_id);
            }
            catch (NotDeclaredNameErrorException e)
            {
                Console.WriteLine(e.Message);
            }

            if (_methodType != null)
            {
                foreach (IFoolNode parameter in _parameterList)
                {
                    result.AddRange(parameter.CheckSemantics(environment));
                }
            }
            else
            {
                result.Add("The object " + _iid + " doesn't have a method" + " name" + _id + "\n");
            }
            return result;
        }

        new public string CodeGeneration()
        {
            StringBuilder parameterCodeGeneration = new StringBuilder();
            StringBuilder GetAR = new StringBuilder();
            for (int i = _parameterList.Count; i >= 0; i--)
            {
                parameterCodeGeneration.Append(_parameterList[i].CodeGeneration());
            }
            for (int i = 0; i < _nestingLevel - _objectNestingLevel; i++)
            {
                GetAR.Append("lw\n");
            }

            return "lfp\n"
                + parameterCodeGeneration
                + "push "
                + _objectOffset
                + "\n"
                + "lfp\n"
                + GetAR
                + "add\n"
                + "lw\n"
                + "cts\n"
                + "lw\n"
                + "push "
                + (_methodOffset - 1)
                + "\n"
                + "add\n"
                + "loadm\n"
                + "js\n"
                ;
        }

        new public IFoolType TypeCheck()
        {
            FoolFunctionType returnType = (FoolFunctionType)_methodType;
            List<IFoolType> parameterTypes = returnType.GetParameters();

            if (parameterTypes.Count != _parameterList.Count)
            {
                throw new FoolTypeException("Oops... Wrong number of arguments for " + " method " + _id + " invocation\n");
            }

            for (int i = 0; i < _parameterList.Count; i++)
            {
                if (_parameterList[i].TypeCheck() is FoolObjectType && (!((FoolIdNode)_parameterList[i]).GetEntry().IsInitialized()))
                {
                    throw new FoolTypeException("Oops... The parameter " + ((FoolIdNode)_parameterList[i]).GetIdName() + " is not initialized!\n");
                }
                if (!_parameterList[i].TypeCheck().IsSubType(parameterTypes[i]))
                {
                    throw new FoolTypeException("The parameter " + ((FoolIdNode)_parameterList[i]).GetIdName() + " inside the invocation of the method " + _id + "\n");
                }
            }
            return returnType.GetReturnType();
        }

        public int GetObjectOffset()
        {
            return _objectOffset;
        }

        public void SetObjectOffset(int offset)
        {
            _objectOffset = offset;
        }

        public int GetObjectNestingLevel()
        {
            return _objectNestingLevel;
        }

        public void SetObjectNestingLevel(int objectNestingLevel)
        {
            _objectNestingLevel = objectNestingLevel;
        }

        public IFoolType GetMethodType()
        {
            return _methodType;
        }

        public void SetMethodType(IFoolType methodType)
        {
            _methodType = methodType;
        }
    }
}
