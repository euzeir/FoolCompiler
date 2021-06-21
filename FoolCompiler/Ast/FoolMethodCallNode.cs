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

        public override List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();

            this._nestingLevel = environment.GetNestingLevel();
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
                        Console.WriteLine("Oops... The object is not initialized!\nExit.");
                        Environment.Exit(-1);
                    }
                    objectType = objectEntry.GetFoolType();
                    _objectOffset = objectEntry.GetOffset();
                    _objectNestingLevel = objectEntry.GetNestingLevel();

                    try
                    {
                        environment.CheckDeclaredName("this");
                        this._nestingLevel--;
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
                        result.Add("Oops... The method " + _id + " is invoked from a " + " NON Object type!\n");
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
                result.Add("Oops... There is no method named " + _id + "for the object" + _iid + "\n");
            }
            return result;
        }

        public override string CodeGeneration()
        {
            StringBuilder parameterCodeGeneration = new StringBuilder();
            StringBuilder getActivationRecord = new StringBuilder();
            for (int i = _parameterList.Count - 1; i >= 0; i--)
            {
                parameterCodeGeneration.Append(_parameterList[i].CodeGeneration());
            }
            for (int i = 0; i < _nestingLevel - _objectNestingLevel; i++)
            {
                getActivationRecord.Append("lw\n");
            }

            return "lfp\n"
                + parameterCodeGeneration
                + "push "
                + _objectOffset
                + "\n"
                + "lfp\n"
                + getActivationRecord
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

        public override IFoolType TypeCheck()
        {
            FoolFunctionType returnType = (FoolFunctionType)_methodType;
            List<IFoolType> parameterTypes = returnType.GetParameters();

            if (parameterTypes.Count != _parameterList.Count)
            {
                throw new FoolTypeException("Oops... Invocation of method " + _id + " with wrong number of arguments\n");
            }

            for (int i = 0; i < _parameterList.Count; i++)
            {
                if (_parameterList[i].TypeCheck() is FoolObjectType && (!((FoolIdNode)_parameterList[i]).GetEntry().IsInitialized()))
                {
                    throw new FoolTypeException("Oops... Parameter " + ((FoolIdNode)_parameterList[i]).GetIdName() + " is not initialized!\n");
                }
                if (!_parameterList[i].TypeCheck().IsSubType(parameterTypes[i]))
                {
                    throw new FoolTypeException("The parameter " + ((FoolIdNode)_parameterList[i]).GetIdName() + " inside the invocation of the method " + _id + " has not the correct type\n");
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
