namespace FoolCompiler.CodeGeneration
{
    public class FoolDispatchTableEntry
    {
        private string _methodId;
        private string _methodLabel;

        public FoolDispatchTableEntry(string methodId, string methodLabel)
        {
            _methodId = methodId;
            _methodLabel = methodLabel;
        }
        public string GetMethodId()
        {
            return _methodId;
        }
        public string GetMethodLabel()
        {
            return _methodLabel;
        }
    }
}
