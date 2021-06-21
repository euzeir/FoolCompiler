using Antlr4.Runtime;
using FoolCompiler.TypeDefinition;
using FoolCompiler.Ast;
using FoolCompiler.CodeGeneration;
using FoolCompiler.ExceptionHandling;
using FoolCompiler.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Configuration;

namespace FoolCompiler
{
    class Program
    {   
        static void Main(string[] args)
         {
            try
            {
                string inputFilePath = ConfigurationManager.AppSettings.Get("inputFilePath");
                string outputFilePath = ConfigurationManager.AppSettings.Get("outputFilePath");
                SelectInputFile sip = new SelectInputFile();
                var com = sip.Start();
                string fileName = inputFilePath + com + ".fool";
                ICharStream input = CharStreams.fromPath(fileName);

                //LEXER
                Console.WriteLine("[<<<<< LEXICAL ANALYSIS >>>>>]");
                FOOLLexer lexer = new FOOLLexer(input);
                if (lexer.lexicalErrors.Count > 0)
                {
                    foreach (var e in lexer.lexicalErrors)
                    {
                        throw new FoolLexerException(e);
                    } 
                }
                CommonTokenStream tokens = new CommonTokenStream(lexer);
                Console.WriteLine("\nOK, DONE!");
                Console.WriteLine("*****************************");
                //PARSER
                Console.WriteLine("[<<<<< SYNTAX ANALYSIS >>>>>]");
                FOOLParser parser = new FOOLParser(tokens);
                FOOLParser.ProgContext progContext = parser.prog();
                if (parser.NumberOfSyntaxErrors > 0)
                    throw new FoolParserException("Errors: " + parser.NumberOfSyntaxErrors + "\n");
                Console.WriteLine("\nOK, DONE!");
                Console.WriteLine("*****************************");
                //SEMANTIC
                Console.WriteLine("[<<<<< SEMANTIC ANALYSIS >>>>>]");
                FoolVisitor visitor = new FoolVisitor();
                IFoolNode ast = visitor.Visit(progContext);
                FoolEnvironment environment = new FoolEnvironment();
                List<string> error = ast.CheckSemantics(environment);
                if (error.Count > 0) throw new FoolSemanticException(error);
                Console.WriteLine("\nOK, DONE!");
                Console.WriteLine("*****************************");
                //TYPE CHECKING
                Console.WriteLine("[<<<<< TYPE CHECKING >>>>>]");
                IFoolType type = ast.TypeCheck();
                Console.WriteLine("Type checking: " + type.GetFoolType().ToUpper());
                Console.WriteLine("\nOK, DONE!");
                Console.WriteLine("*****************************");
                //CODE GENERATION
                Console.WriteLine("[<<<<< CODE GENERATION >>>>>]");
                string code = ast.CodeGeneration();
                code += "\n" + FoolDispatchTable.CodeGenerationOfDispatchTable();

                if (File.Exists(outputFilePath))
                {
                    File.Delete(outputFilePath);
                }
                if (!File.Exists(outputFilePath))
                {
                    using (FileStream fs = File.Create(outputFilePath))
                    {
                        Byte[] data = new UTF8Encoding(true).GetBytes(code);
                        fs.Write(data);
                    }
                }

                if (!type.GetFoolType().ToUpper().Equals("VOID"))
                {
                    AddPrintCodeGeneration();
                }
                ICharStream inputASM = CharStreams.fromPath(outputFilePath);
                SVMLexer lexerASM = new SVMLexer(inputASM);
                CommonTokenStream tokensASM = new CommonTokenStream(lexerASM);
                SVMParser parserASM = new SVMParser(tokensASM);
                parserASM.assembly();

                if (lexerASM.errors.Count > 0)
                {
                    foreach (var ex in lexerASM.errors)
                    {
                        throw new FoolLexerException(ex);
                    }
                }
                if (parserASM.GetNumberOfSyntaxErrors() > 0)
                {
                    throw new FoolParserException("Oops... Parser Error in SVM");
                }

                List<int> GetCode = parserASM.GetCode();

                int[] byteCode = new int[GetCode.Count];
                for (int i = 0; i < GetCode.Count; i++)
                {
                    byteCode[i] = GetCode[i];
                }

                ExecuteVirtualMachine virtualMachine = new ExecuteVirtualMachine(byteCode);

                List<string> output = virtualMachine.Cpu();

                string outputString = "No output! Probably Stack Overflow...";
                if (output.Count > 0)
                {
                    outputString = output[output.Count - 1];
                }
                if (outputString.Equals("-10000000"))
                {
                    Console.WriteLine("\nRESULT: " + "STACK IS EMPTY!");
                }
                else
                {
                    if (outputString.Equals("1") && ast.TypeCheck().GetFoolType().Equals("bool"))
                    {
                        outputString = "True";
                    }
                    if (outputString.Equals("0") && ast.TypeCheck().GetFoolType().Equals("bool"))
                    {
                        outputString = "False";
                    }

                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nRESULT: " + outputString);
                    ConsoleKeyInfo cki;

                    while (true)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine(">> Execution Terminated!\n>> Please press ESC or X to close the window.");
                        cki = Console.ReadKey(true);
                        Console.WriteLine($"  You pressed: {cki.Key}\n");
                        if (cki.Key == ConsoleKey.Escape || cki.Key == ConsoleKey.X) break;
                    }
                }
            }
            catch (Exception e)
            {
                if (e is FoolHeapOverflowError || 
                    e is FoolSegmentationFaultError ||
                    e is StackOverflowException ||
                    e is IOException ||
                    e is FoolLexerException ||
                    e is FoolParserException ||
                    e is FoolSemanticException ||
                    e is FoolTypeException ||
                    e is Exception)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Main: " + e.Message);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("*****************************");
                    Console.WriteLine("Press any key to terminate...");
                    Console.ReadKey();
                }
            }

        }

        public static void AddPrintCodeGeneration()
        {
            try
            {
                string path = ConfigurationManager.AppSettings.Get("outputFilePath");
                string line;
                List<string> lines = new List<string>();

                using (StreamReader streamReader = File.OpenText(path))
                {
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (line.Contains("halt"))
                        {
                            line = line.Replace("halt", "print\nhalt");
                        }
                        lines.Add(line + "\n");
                    }
                }

                using (StreamWriter streamWriter = new StreamWriter(path))
                {
                    foreach (string str in lines)
                    {
                        streamWriter.Write(str);
                        streamWriter.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
