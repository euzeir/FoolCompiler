namespace FoolCompiler.TypeDefinition
{
    public class FoolFieldType
    {
        private string _id;
        private IFoolType _foolType;

        public FoolFieldType(string id, IFoolType foolType)
        {
            _id = id;
            _foolType = foolType;
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
