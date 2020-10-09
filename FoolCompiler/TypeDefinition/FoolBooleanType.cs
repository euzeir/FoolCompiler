namespace FoolCompiler.TypeDefinition
{
    class FoolBooleanType : IFoolType
    {
        public string GetId()
        {
            return "Boolean";
        }

        public bool IsSubType(IFoolType type)
        {
            return (GetId().Equals(type.GetId()));
        }

        public string GetFoolType()
        {
            return "bool";
        }
    }
}
