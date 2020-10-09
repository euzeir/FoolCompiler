namespace FoolCompiler.TypeDefinition
{
    public class FoolVoidType : IFoolType
    {
        public string GetId()
        {
            string ret = "Void";
            return ret;
        }

        public bool IsSubType(IFoolType type)
        {
            bool res = GetId().Equals(type.GetId());
            return res;
        }

        string IFoolType.GetFoolType()
        {
            string res = "void";
            return res;
        }
    }
}
