using FoolCompiler.ExceptionHandling;
using FoolCompiler.TypeDefinition;
using System;
using System.Collections.Generic;
using static FoolCompiler.Ast.FoolIntegerOperationsNode;
using static FOOLParser;

namespace FoolCompiler.Ast
{
    public class FoolVisitor: FOOLBaseVisitor<IFoolNode>
    {
        public override IFoolNode VisitSingleExp(SingleExpContext context)
        {
            return new FoolProgramSingleExpressionNode(Visit(context.exp()));
        }

        public override IFoolNode VisitLetInExp(LetInExpContext context)
        {
            List<IFoolNode> declarations = new List<IFoolNode>();

            foreach (DecContext dec in context.let().dec())
            {
                declarations.Add(Visit(dec));
            }

            if (context.exp() != null)
            {
                return new FoolProgramLetInExpressionNode(declarations, Visit(context.exp()));
            }
            else
            {
                List<IFoolNode> statements = new List<IFoolNode>();
                foreach (StmContext stm in context.stms().stm())
                {
                    statements.Add(Visit(stm));
                }
                return new FoolProgramLetInExpressionNode(declarations, statements);
            }
        }

        public override IFoolNode VisitClassExp(ClassExpContext context)
        {
            List<FoolClassNode> classExpList = new List<FoolClassNode>();
            List<IFoolNode> letDeclaration = new List<IFoolNode>();
            IFoolNode letIn;

            foreach (ClassdecContext cdc in context.classdec())
            {
                classExpList.Add((FoolClassNode) Visit(cdc));
            }

            if (context.let() != null)
            {
                foreach (DecContext decContext in context.let().dec())
                {
                    letDeclaration.Add(Visit(decContext));
                }
            }
            
            if (context.exp() != null)
            {
                letIn = new FoolProgramLetInExpressionNode(letDeclaration, Visit(context.exp()));
            }
            else
            {
                List<IFoolNode> statements = new List<IFoolNode>();
                foreach (StmContext stm in context.stms().stm())
                {
                    statements.Add(Visit(stm));
                }

                letIn = new FoolProgramLetInExpressionNode(letDeclaration, statements);
            }
            return new FoolProgramClassExpressionNode(classExpList, letIn);
        }

        public override IFoolNode VisitClassdec(ClassdecContext context)
        {
            try
            {
                List<FoolParameterNode> parameterNodes = new List<FoolParameterNode>();
                List<FoolMethodNode> methodNodes = new List<FoolMethodNode>();

                for (int i = 0; i < context.vardec().Length; i++)
                {
                    VardecContext vardecContext = context.vardec(i);
                    parameterNodes.Add(new FoolParameterNode(vardecContext.ID().GetText(), Visit(vardecContext.type()).TypeCheck(), i + 1, true)); 
                }

                foreach (MetContext metContext in context.met())
                {
                    FoolMethodNode methodNode = (FoolMethodNode)Visit(metContext);
                    string idClass = context.ID(0).GetText();
                    methodNode.SetClassName(idClass);
                    methodNodes.Add(methodNode);
                }

                if (context.ID(1) == null)
                {
                    return new FoolClassNode(context.ID(0).GetText(), parameterNodes, methodNodes);
                }
                else
                {
                    return new FoolClassNode(context.ID(0).GetText(), context.ID()[1].GetText(), parameterNodes, methodNodes);
                }
            }
            catch (FoolTypeException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public override IFoolNode VisitMet(MetContext context)
        {
            List<FoolParameterNode> parameterNodes = new List<FoolParameterNode>();
            List<IFoolNode> nestedDec = new List<IFoolNode>();
            List<IFoolNode> expressionOrStatement = new List<IFoolNode>();
            IFoolType type;

            try
            {
                for (int i = 0; i < context.fun().vardec().Length; i++)
                {
                    VardecContext vardecContext = context.fun().vardec()[i];
                    parameterNodes.Add(new FoolParameterNode(vardecContext.ID().GetText(), Visit(vardecContext.type()).TypeCheck(), i + 1, true));
                }
                if (context.fun().letVar() != null)
                {
                    foreach (VarasmContext vc in context.fun().letVar().varasm())
                    {
                        nestedDec.Add(Visit(vc));
                    }
                }
                if (context.fun().exp() != null)
                {
                    expressionOrStatement.Add(Visit(context.fun().exp()));
                }
                else
                {
                    foreach (StmContext stmContext in context.fun().stms().stm())
                    {
                        expressionOrStatement.Add(Visit(stmContext));
                    }
                }
                if (context.fun().type() == null)
                {
                    type = new FoolVoidType();
                }
                else
                {
                    type = Visit(context.fun().type()).TypeCheck();
                }
                return new FoolMethodNode(context.fun().ID().GetText(), type, parameterNodes, nestedDec, expressionOrStatement);
            }
            catch (FoolTypeException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public override IFoolNode VisitVarasm(VarasmContext context)
        {
            try
            {
                IFoolType type = Visit(context.vardec().type()).TypeCheck();
                IFoolNode expressionNode;

                if (context.exp() != null)
                {
                    expressionNode = Visit(context.exp());
                }
                else
                {
                    string classId = string.Empty;
                    if (context.vardec().type().ID() != null)
                    {
                        classId = context.vardec().type().ID().GetText();
                    }
                    expressionNode = new FoolNullNode(classId);
                }
                return new FoolVarAsmNode(context.vardec().ID().GetText(), type, expressionNode);
            }
            catch (FoolTypeException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public override IFoolNode VisitFun(FunContext context)
        {
            try
            {
                List<FoolParameterNode> parameterNodes = new List<FoolParameterNode>();
                List<IFoolNode> decs = new List<IFoolNode>();
                List<IFoolNode> expressionOrStatements = new List<IFoolNode>();
                IFoolType type;

                for (int i = 0; i < context.vardec().Length; i++)
                {
                    VardecContext vardecContext = context.vardec()[i];
                    parameterNodes.Add(new FoolParameterNode(vardecContext.ID().GetText(), Visit(vardecContext.type()).TypeCheck(), i + 1));
                }
                if (context.letVar() != null)
                {
                    foreach (VarasmContext varasmContext in context.letVar().varasm())
                    {
                        decs.Add(Visit(varasmContext));
                    }
                }
                if (context.exp() != null)
                {
                    expressionOrStatements.Add(Visit(context.exp()));
                }
                else
                {
                    foreach (StmContext stmContext in context.stms().stm())
                    {
                        expressionOrStatements.Add(Visit(stmContext));
                    }
                }
                if (context.type() != null)
                {
                    type = Visit(context.type()).TypeCheck();
                }
                else
                {
                    type = new FoolVoidType();
                }
                return new FoolFunctionNode(context.ID().GetText(), type, parameterNodes, decs, expressionOrStatements);
            }
            catch (FoolTypeException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public override IFoolNode VisitType(TypeContext context)
        {
            return new FoolTypeNode(context.GetText());
        }

        public override IFoolNode VisitExp(ExpContext context)
        {
            if (context.right == null)
            {
                return Visit(context.left);
            }
            else if (context.PLUS() != null)
            {
                return new FoolIntegerOperationsNode(IntegerOperationTypes.PLUS, Visit(context.left), Visit(context.right));
            }
            else
            {
                return new FoolIntegerOperationsNode(IntegerOperationTypes.MINUS, Visit(context.left), Visit(context.right));
            }
        }

        public override IFoolNode VisitTerm(TermContext context)
        {
            if (context.right == null)
            {
                return Visit(context.left);
            }
            else if (context.TIMES() != null)
            {
                return new FoolIntegerOperationsNode(IntegerOperationTypes.MULT, Visit(context.left), Visit(context.right));
            }
            else
            {
                return new FoolIntegerOperationsNode(IntegerOperationTypes.DIVISION, Visit(context.left), Visit(context.right));
            }
        }

        public override IFoolNode VisitFactor(FactorContext context)
        {
            if (context.right == null)
            {
                return Visit(context.left);
            }
            else
            {
                switch (context.logicoperator.Type)
                {
                    case FOOLLexer.EQ:
                        return new FoolLogicalOperationsNode(FOOLLexer.EQ, Visit(context.left), Visit(context.right));
                    case FOOLLexer.LESSEREQUAL:
                        return new FoolLogicalOperationsNode(FOOLLexer.LESSEREQUAL, Visit(context.left), Visit(context.right));
                    case FOOLLexer.LESSERTHAN:
                        return new FoolLogicalOperationsNode(FOOLLexer.LESSERTHAN, Visit(context.left), Visit(context.right));
                    case FOOLLexer.GREATEREQUAL:
                        return new FoolLogicalOperationsNode(FOOLLexer.GREATEREQUAL, Visit(context.left), Visit(context.right));
                    case FOOLLexer.GREATERTHAN:
                        return new FoolLogicalOperationsNode(FOOLLexer.GREATERTHAN, Visit(context.left), Visit(context.right));
                    case FOOLLexer.AND:
                        return new FoolLogicalOperationsNode(FOOLLexer.AND, Visit(context.left), Visit(context.right));
                    case FOOLLexer.OR:
                        return new FoolLogicalOperationsNode(FOOLLexer.OR, Visit(context.left), Visit(context.right));
                    default:
                        try
                        {
                            throw new Exception("Operator not found!\n");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            return null;
                        }
                }
            }
        }

        public override IFoolNode VisitIntVal(IntValContext context)
        {
            int prefix = int.Parse(context.INTEGER().GetText());

            if (context.MINUS() != null)
            {
                return new FoolIntNode(-prefix);
            }
            else
            {
                return new FoolIntNode(prefix);
            }
        }

        public override IFoolNode VisitBoolVal(BoolValContext context)
        {
            bool flag;
            if (context.NOT() != null)
            {
                return new FoolBooleanNode(!Boolean.TryParse(context.GetText(), out flag));
            }
            else
            {
                return new FoolBooleanNode(Boolean.TryParse(context.GetText(), out flag));
            }
        }

        public override IFoolNode VisitBaseExp(BaseExpContext context)
        {
            return Visit(context.exp());
        }

        public override IFoolNode VisitIfExp(IfExpContext context)
        {
            if (context.elseBranch != null)
            {
                return new FoolIfNode(Visit(context.cond), Visit(context.thenBranch), Visit(context.elseBranch), false);
            }
            else
            {
                return new FoolIfNode(Visit(context.cond), Visit(context.thenBranch), false);
            }
        }

        public override IFoolNode VisitIfStm(IfStmContext context)
        {
            if (context.elseBranch != null)
            {
                return new FoolIfNode(Visit(context.cond), Visit(context.thenBranch), Visit(context.elseBranch), true);
            }
            else
            {
                return new FoolIfNode(Visit(context.cond), Visit(context.thenBranch), true);
            }
        }

        public override IFoolNode VisitVarId(VarIdContext context)
        {
            if (context.MINUS() == null && context.NOT() == null)
            {
                return new FoolIdNode(context.ID().GetText(), false, string.Empty);
            }
            else
            {
                if (context.MINUS() == null)
                {
                    return new FoolIdNode(context.ID().GetText(), true, "not");
                }
                else
                {
                    return new FoolIdNode(context.ID().GetText(), true, "-");
                }
            }
        }

        public override IFoolNode VisitFunExp(FunExpContext context)
        {
            return base.VisitFunExp(context);
        }

        public override IFoolNode VisitMethExp(MethExpContext context)
        {
            List<IFoolNode> parameterNodes = new List<IFoolNode>();

            foreach (ExpContext exp in context.functioncall().exp())
            {
                parameterNodes.Add(Visit(exp));
            }
            return new FoolMethodCallNode(context.ID().GetText(), context.functioncall().ID().GetText(), parameterNodes);
        }

        public override IFoolNode VisitFunctioncall(FunctioncallContext context)
        {
            List<IFoolNode> arguments = new List<IFoolNode>();
            bool isMethod = false;
            Antlr4.Runtime.ParserRuleContext parserRuleContext = context;

            while (parserRuleContext.Parent != null)
            {
                if (parserRuleContext.GetText().Contains("class"))
                {
                    isMethod = true;
                    break;
                }

                parserRuleContext = (Antlr4.Runtime.ParserRuleContext)parserRuleContext.Parent;
            }

            foreach (ExpContext exp in context.exp())
            {
                arguments.Add(Visit(exp));
            }

            if (isMethod)
            {
                return new FoolMethodCallNode("this", context.ID().GetText(), arguments);
            }
            else
            {
                return new FoolFunctionCallNode(context.ID().GetText(), arguments);
            }
        }

        public override IFoolNode VisitNewExp(NewExpContext context)
        {
            List<IFoolNode> arguments = new List<IFoolNode>();

            foreach (ExpContext expContext in context.exp())
            {
                arguments.Add(Visit(expContext));
            }
            return new FoolNewNode(context.ID().GetText(), arguments);
        }

        public override IFoolNode VisitAsmStm(AsmStmContext context)
        {
            if (context.exp() != null)
            {
                return new FoolAsmStmExpressionNode(context.ID().GetText(), Visit(context.exp()));
            }
            return new FoolAsmStmExpressionNode(context.ID().GetText(), new FoolNullNode());
        }

        public override IFoolNode VisitStms(StmsContext context)
        {
            return base.VisitStms(context);
        }

        public override IFoolNode VisitLet(LetContext context)
        {
            return base.VisitLet(context);
        }

        public override IFoolNode VisitDec(DecContext context)
        {
            return base.VisitDec(context);
        }

        public override IFoolNode VisitVardec(VardecContext context)
        {
            return base.VisitVardec(context);
        }

        public override IFoolNode VisitLetVar(LetVarContext context)
        {
            return base.VisitLetVar(context);
        }
    }
}
