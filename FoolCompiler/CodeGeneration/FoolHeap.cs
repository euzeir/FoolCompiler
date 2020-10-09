using FoolCompiler.ExceptionHandling;

namespace FoolCompiler.CodeGeneration
{
    public class FoolHeap
    {
        private FoolHeapCell _headCell;
        private int _heapSize;

        public FoolHeap(int size)
        {
            FoolHeapCell[] cellsList = new FoolHeapCell[size];
            _heapSize = size;
            cellsList[size - 1] = new FoolHeapCell(ExecuteVirtualMachine.START_ADDRESS + size - 1, null);
            for (int i = size - 2; i >= 0; i--)
            {
                cellsList[i] = new FoolHeapCell(ExecuteVirtualMachine.START_ADDRESS + i, cellsList[i + 1]);
            }
            _headCell = cellsList[0];
        }
        public FoolHeapCell Allocate(int size)
        {
            if (_heapSize < size)
            {
                throw new FoolHeapOverflowError();
            }

            FoolHeapCell result = _headCell;
            FoolHeapCell lastItem = _headCell;
            
            for (int i = 1; i < size; i++)
            {
                lastItem = lastItem.GetNext();
            }
            _headCell = lastItem.GetNext();
            _heapSize = _heapSize - size;
            return result;
        }

        public int GetFreeAddress()
        {
            if (_headCell != null)
            {
                return _headCell.GetIndex();
            }
            else
            {
                return -1;
            }
        }
    }
}
