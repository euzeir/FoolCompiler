using System.Collections.Generic;
using FoolCompiler.TypeDefinition;
using FoolCompiler.ExceptionHandling;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    public class FoolParameterNode : IFoolNode
    {
        protected string _id;
        protected IFoolType _foolType;
        protected int _offset;
        protected bool _isInsideClass;

        public FoolParameterNode(string id, IFoolType foolType, int offset)
        {
            _id = id;
            _foolType = foolType;
            _offset = offset;
            _isInsideClass = false;
        }
        public FoolParameterNode(string id, IFoolType foolType, int offset, bool isInsideClass)
        {
            _id = id;
            _foolType = foolType;
            _offset = offset;
            _isInsideClass = isInsideClass;
        }

        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();
            try
            {
                environment.InsertDeclarationInClassOrFunction(_id, _foolType, _offset, _isInsideClass);
            }
            catch (MultipleNameDeclarationErrorException e)
            {
                result.Add(e.Message);
            }
            return result;
        }

        public string CodeGeneration()
        {
            string res = string.Empty;
            return res;
        }

        public IFoolType TypeCheck()
        {
            try
            {
                return _foolType;
            }
            catch
            {
                throw new FoolTypeException("Bad Type!");
            }
        }

        public string GetId()
        {
            return _id;
        }
        
        public IFoolType GetFoolType()
        {
            return _foolType;
        }
    }
}
