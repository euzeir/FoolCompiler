using FoolCompiler.TypeDefinition;

namespace FoolCompiler.Utils
{
    public class SymbolTableEntry
    {
        private IFoolType _foolType;
        private int _nestingLevel;
        private int _offset;
        private bool _isClass;
        private bool _isInitialized;

        public SymbolTableEntry(int nestingLevel, int offset)
        {
            _nestingLevel = nestingLevel;
            _offset = offset;
            _isInitialized = true;
        }
        public SymbolTableEntry(int nestingLevel, IFoolType foolType, int offset)
        {
            _nestingLevel = nestingLevel;
            _foolType = foolType;
            _offset = offset;
            _isInitialized = true;
        }
        public SymbolTableEntry(int nestingLevel, IFoolType foolType)
        {
            _nestingLevel = nestingLevel;
            _foolType = foolType;
            _isInitialized = true;
        }
        public SymbolTableEntry(int nestingLevel, IFoolType foolType, int offset, bool isClass)
        {
            _nestingLevel = nestingLevel;
            _foolType = foolType;
            _offset = offset;
            _isClass = isClass;
            _isInitialized = true;
        }

        public IFoolType GetFoolType()
        {
            return _foolType;
        }
        public int GetOffset()
        {
            return _offset;
        }
        public int GetNestingLevel()
        {
            return _nestingLevel;
        }
        public bool IsInitialized()
        {
            return _isInitialized;
        }
        public void Initialize(bool initalize)
        {
            _isInitialized = initalize;
        }
    }
}
