namespace FoolCompiler.CodeGeneration
{
    public class FoolHeapCell
    {
        private FoolHeapCell _next;
        private int _index;

        public FoolHeapCell(int index, FoolHeapCell next)
        {
            _index = index;
            _next = next;
        }

        public int GetIndex()
        {
            return _index;
        }

        public FoolHeapCell GetNext()
        {
            return _next;
        }
    }
}
