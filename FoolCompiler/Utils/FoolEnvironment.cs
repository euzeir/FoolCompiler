using FoolCompiler.TypeDefinition;
using FoolCompiler.ExceptionHandling;
using System;
using System.Collections.Generic;

namespace FoolCompiler.Utils
{
    public class FoolEnvironment
    {
        private List<Dictionary<string, SymbolTableEntry>> _symbolTable = new List<Dictionary<string, SymbolTableEntry>>();
        private SymbolTableEntry _lastEntry = null;
        private int _offset = 0;

        public List<Dictionary<string, SymbolTableEntry>> GetSymbolTable()
        {
            return _symbolTable;
        }
        public void SetSymbolTable(List<Dictionary<string, SymbolTableEntry>> symbolTable)
        {
            _symbolTable = symbolTable;
        }
        public int GetOffset()
        {
            return _offset;
        }
        public void IncrementOffset()
        {
            _offset++;
        }
        public void DecrementOffset()
        {
            _offset--;
        }
        public void SetOffsetValue(int offset)
        {
            _offset = offset;
        }
        public int GetNestingLevel()
        {
            return _symbolTable.Count - 1;
        }

        public SymbolTableEntry GetLastSymbolTableEntry()
        {
            return _lastEntry;
        }
        public void InsertNewScope()
        {
            Dictionary<string, SymbolTableEntry> newScope = new Dictionary<string, SymbolTableEntry>();
            _symbolTable.Add(newScope);
        }
        public void RemoveScope()
        {
            var nestLevel = GetNestingLevel();
            _symbolTable.RemoveAt(nestLevel);
        }

        public FoolEnvironment InsertDeclaration(string declarationName, IFoolType fType, int offset)
        {
            try
            {
                SymbolTableEntry entry = new SymbolTableEntry(GetNestingLevel(), fType, offset);

                if (_symbolTable[GetNestingLevel()].ContainsKey(declarationName))
                {
                    throw new MultipleNameDeclarationErrorException(declarationName);
                }
                if (fType is FoolClassType)
                {
                    _lastEntry = entry;
                }
                _symbolTable[GetNestingLevel()].Add(declarationName, entry);
            }
            catch(FoolParserException e)
            {
                Console.WriteLine("Oops...a Parser Error has occured! {0}", e);
            }
            return this;
        }
        public FoolEnvironment SetDeclarationType(string declarationName, IFoolType fType, int offset)
        {
            try
            {
                SymbolTableEntry newEntry = new SymbolTableEntry(GetNestingLevel(), fType, offset);
                SymbolTableEntry oldEntry = _symbolTable[GetNestingLevel()][declarationName] = newEntry;
                if (fType is FoolClassType)
                {
                    _lastEntry = newEntry;
                }
                if (oldEntry == null)
                {
                    throw new NotDeclaredNameErrorException(declarationName);
                }
            }
            catch(FoolParserException e)
            {
                Console.WriteLine("Oops...a Parser Error has occured! {0}", e);
            }
            return this;
        }
        public FoolEnvironment InsertDeclarationInClassOrFunction(string declarationName, IFoolType fType, int offset, bool inside)
        {
            SymbolTableEntry entry = new SymbolTableEntry(GetNestingLevel(), fType, offset, inside);
            try
            {
                if (_symbolTable[GetNestingLevel()].ContainsKey(declarationName))
                {
                    throw new MultipleNameDeclarationErrorException(declarationName);
                }
                if (fType is FoolClassType)
                {
                    _lastEntry = entry;
                }
                _symbolTable[GetNestingLevel()].Add(declarationName, entry);
                return this;
            }
            catch
            {
                throw new MultipleNameDeclarationErrorException(declarationName);
            }
        }

        public SymbolTableEntry CheckDeclaredName(string declaredName)
        {
            int max = _symbolTable.Count - 1;

            while (_symbolTable[max] != null)
            {
                Dictionary<string, SymbolTableEntry> currentElement = _symbolTable[max];
                if (currentElement.ContainsKey(declaredName))
                {
                    return currentElement[declaredName];
                }
                max--;
                if (max == -1) break;
            }

            throw new NotDeclaredNameErrorException(declaredName);
        }

        public SymbolTableEntry ControlFunction(string declaredName)
        {
            int max = _symbolTable.Count - 1;

            while (_symbolTable[max] != null)
            {
                Dictionary<string, SymbolTableEntry> currentElement = _symbolTable[max];
                if ((currentElement.ContainsKey(declaredName)) && !(currentElement[declaredName].GetFoolType() is FoolFunctionType))
                {
                    return currentElement[declaredName];
                }
                max--;
                if (max == -1) break; 
            }

            throw new NotDeclaredNameErrorException(declaredName);
        }

        public IFoolType GetFoolType(string declaredName)
        {
            return CheckDeclaredName(declaredName).GetFoolType();
        }
    }
}
