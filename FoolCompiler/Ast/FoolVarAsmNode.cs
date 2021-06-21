using System;
using System.Collections.Generic;
using FoolCompiler.TypeDefinition;
using FoolCompiler.ExceptionHandling;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    public class FoolVarAsmNode : IFoolNode
    {
        private string _id;
        private IFoolType _type;
        private IFoolNode _expression;

        public FoolVarAsmNode(string id, IFoolType type, IFoolNode expression)
        {
            _id = id;
            _type = type;
            _expression = expression;
        }
        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();

            if (_type is FoolObjectType)
            {
                FoolObjectType declarationType = (FoolObjectType) _type;
                try
                {
                    environment.CheckDeclaredName(declarationType.GetFoolClassType().GetFoolClassName());
                }
                catch (NotDeclaredNameErrorException e)
                {
                    Console.WriteLine("Oops... Error: The Class [ " 
                        + declarationType.GetFoolClassType().GetFoolClassName() 
                        + " ] has not been declared!\n" + e.Message);
                    Environment.Exit(-1);
                }
                result.AddRange(declarationType.UpdateFoolClassType(environment));
            }

            result.AddRange(_expression.CheckSemantics(environment));

            try
            {
                environment.InsertDeclaration(_id, _type, environment.GetOffset());

                if (_expression.TypeCheck() is FoolVoidType)
                {
                    environment.CheckDeclaredName(_id).Initialize(false);
                }
                else
                {
                    environment.CheckDeclaredName(_id).Initialize(true);
                }
                environment.DecrementOffset();
            }
            catch (Exception e)
            {
                if (e is FoolTypeException || e is MultipleNameDeclarationErrorException || e is NotDeclaredNameErrorException)
                {
                    result.Add(e.Message);
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            return result;
        }

        public string CodeGeneration()
        {
            return _expression.CodeGeneration();
        }

        public IFoolType TypeCheck()
        {
            if ((_type is FoolObjectType) && (_expression is FoolNullNode))
            {
                return _type;
            }
            if (!_expression.TypeCheck().IsSubType(_type))
            {
                throw new FoolTypeException("Oops... Variable [ " + _id + " ] has a Wrong or Incompatible value\n");
            }
            return _type;
        }
    }
}
