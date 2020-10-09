namespace FoolCompiler.TypeDefinition
{
    public class FoolIntType : IFoolType
    {
        public string GetFoolType()
        {
            return "int";
        }

        public string GetId()
        {
            return "Int";
        }

        public bool IsSubType(IFoolType type)
        {
            return (GetId().Equals(type.GetId()));
        }
    }
}
