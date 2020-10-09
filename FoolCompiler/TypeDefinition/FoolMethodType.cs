namespace FoolCompiler.TypeDefinition
{
    public class FoolMethodType
    {
        private string _id;
        private IFoolType _foolType;

        public FoolMethodType(string id, IFoolType foolType)
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
