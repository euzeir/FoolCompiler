namespace FoolCompiler.Lib
{
    public class FoolLib
    {
        private static int _labelCount = 0;
        private static int _functionLabelCount = 0;
        private static string _functionCode = string.Empty;
        
        public static string CreateFreshLabel()
        {
            return "label" + (_labelCount++);
        }
        public static string CreateFreshFunctionLabel()
        {
            return "function" + (_functionLabelCount++);
        }
        public static void PutCode(string c)
        {
            _functionCode = _functionCode + "\n" + c;
        }
        public static string GetCode()
        {
            return _functionCode;
        }
    }
}
