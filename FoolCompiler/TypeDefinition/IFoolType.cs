namespace FoolCompiler.TypeDefinition
{
    public interface IFoolType
    {
        string GetId();
        bool IsSubType(IFoolType type);
        string GetFoolType();
    }
}
