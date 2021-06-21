using System;
using System.Collections.Generic;
using FoolCompiler.ExceptionHandling;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Utils;

namespace FoolCompiler.Ast
{
    class FoolProgramClassExpressionNode : IFoolNode
    {
        private List<FoolClassNode> _classNodeDeclarations;
        private IFoolNode _expressionOrStatements;

        public FoolProgramClassExpressionNode(List<FoolClassNode> classExpressionList, IFoolNode letInNode)
        {
            _classNodeDeclarations = classExpressionList;
            _expressionOrStatements = letInNode;
        }
        public List<string> CheckSemantics(FoolEnvironment environment)
        {
            List<string> result = new List<string>();
            environment.InsertNewScope();

            foreach (FoolClassNode classNode in _classNodeDeclarations)
            {
                List<FoolFieldType> fields = new List<FoolFieldType>();
                List<FoolMethodType> methods = new List<FoolMethodType>();

                foreach (FoolMethodNode methodNode in classNode.GetMethodList())
                {
                    string metId = methodNode.GetId();
                    IFoolType metType = new FoolFunctionType(new List<IFoolType>(), methodNode.GetFoolType());
                    methods.Add(new FoolMethodType(metId, metType));
                }
                string clsId = classNode.GetId();
                FoolClassType classType = new FoolClassType(clsId, new FoolClassType(classNode.GetSuperClassId()), fields, methods);

                try
                {
                    environment.InsertDeclaration(clsId, classType, 0);
                }
                catch (MultipleNameDeclarationErrorException) 
                {
                    result.Add("Oops... Class [ " + classNode.GetId() + " ] is already declared!");
                }
            }
            foreach (FoolClassNode classNode in _classNodeDeclarations)
            {
                result.AddRange(classNode.CheckSemantics(environment));
            }
            foreach (FoolClassNode classNode in _classNodeDeclarations)
            {
                result.AddRange(classNode.CheckMethods(environment));
            }

            result.AddRange(_expressionOrStatements.CheckSemantics(environment));
            environment.RemoveScope();
            return result;
        }

        public string CodeGeneration()
        {
            List<FoolClassNode> classNode = new List<FoolClassNode>();
            Dictionary<string, FoolClassNode> classMapping = new Dictionary<string, FoolClassNode>();
            string nameDeclaration = string.Empty;

            IEnumerator<FoolClassNode> enumerator = _classNodeDeclarations.GetEnumerator();
            int i = 0;
            while (enumerator.MoveNext())
            {
                
                FoolClassNode classDeclaration = (FoolClassNode)enumerator.Current;
                if (string.IsNullOrEmpty(classDeclaration.GetSuperClassId()))
                {
                    i++;
                    classNode.Add(classDeclaration);
                    classMapping.Add(classDeclaration.GetId(), classDeclaration);
                    if (i < _classNodeDeclarations.Count)
                    {
                       _classNodeDeclarations.Remove(enumerator.Current);
                    }
                    if (i == _classNodeDeclarations.Count)
                    {
                        _classNodeDeclarations.Remove(enumerator.Current);
                        break;
                    }
                }
            }
            while (_classNodeDeclarations.Count != 0)
            {
                int j = 0;
                j++;
                enumerator = _classNodeDeclarations.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    FoolClassNode subClass = (FoolClassNode) enumerator.Current;
                    string superClassName = subClass.GetSuperClassId();
                    FoolClassNode superClass = classMapping[superClassName];
                    if (superClass != null)
                    {
                        classNode.Add(subClass);
                        classMapping.Add(subClass.GetId(), subClass);
                        if (i < _classNodeDeclarations.Count)
                            {
                                _classNodeDeclarations.Remove(enumerator.Current);
                            }
                        if (j == _classNodeDeclarations.Count)
                        {
                            _classNodeDeclarations.Remove(enumerator.Current);
                            break;
                        }
                    }
                }
            }
            foreach (FoolClassNode cNode in classNode)
            {
                nameDeclaration += cNode.CodeGeneration();
            }

            return nameDeclaration
                + _expressionOrStatements.CodeGeneration();
        }

        public IFoolType TypeCheck()
        {
            try
            {
                foreach (FoolClassNode classNode in _classNodeDeclarations)
                {
                    classNode.TypeCheck();
                }
            }
            catch
            {
                throw new FoolTypeException("Oops... TypeChecking error!");
            }

            return _expressionOrStatements.TypeCheck();
        }
    }
}
