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
                    methods.Add(new FoolMethodType(methodNode.GetId(), new FoolFunctionType(new List<IFoolType>(), methodNode.GetFoolType())));
                }
                FoolClassType classType = new FoolClassType(classNode.GetId(), new FoolClassType(classNode.GetSuperClassId()), fields, methods);

                try
                {
                    environment.InsertDeclaration(classNode.GetId(), classType, 0);
                }
                catch (MultipleNameDeclarationErrorException) 
                {
                    result.Add("Oops... Class [ " + classNode.GetId() + " ] is already declared!\n");
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

            var index = _classNodeDeclarations.Count;
            for (int i = 1; i <= index; i++)
            {
                if (_classNodeDeclarations[i + 1] != null)
                {
                    FoolClassNode classDeclaration = (FoolClassNode)_classNodeDeclarations[i + 1];
                   // if (classDeclaration.GetSuperClassId() == null || classDeclaration.GetSuperClassId() == string.Empty) ;
                    if (string.IsNullOrEmpty(classDeclaration.GetSuperClassId()))
                    {
                        classNode.Add(classDeclaration);
                        classMapping.Add(classDeclaration.GetId(), classDeclaration);
                    }
                }
            }
            while (_classNodeDeclarations.Count != 0)
            {
                for (int i = 1; i <= index; i++)
                {
                    if (_classNodeDeclarations[i + 1] != null)
                    {
                        FoolClassNode subClass = (FoolClassNode) _classNodeDeclarations[i + 1];
                        string superClassName = subClass.GetSuperClassId();
                        FoolClassNode superClass = classMapping[superClassName];
                        if (superClass != null)
                        {
                            classNode.Add(subClass);
                            classMapping.Add(subClass.GetId(), subClass);
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
            foreach (FoolClassNode classNode in _classNodeDeclarations)
            {
                classNode.TypeCheck();
            }
            return _expressionOrStatements.TypeCheck();
        }
    }
}
