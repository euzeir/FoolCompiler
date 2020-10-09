using FoolCompiler.ExceptionHandling;
using System;
using System.Collections.Generic;

namespace FoolCompiler.CodeGeneration
{
    public class ExecuteVirtualMachine
    {
        public static readonly int MEMSIZE = 10000;      //memory size
        public static readonly int START_ADDRESS = 0;    //starting address
        private int[] _code;                             //code to execute
        private int[] _memory = new int[MEMSIZE];        //system's memory
        private int _ip = 0;                             //Instruction Pointer
        private int _sp = START_ADDRESS + MEMSIZE;       //Stack Pointer
        private int _fp = START_ADDRESS + MEMSIZE;       //Frame Pointer
        private int _ra;                                 //Return Address
        private int _rv;                                 //Return Value
        private int _hp = START_ADDRESS;                 //Heap Pointer
        private FoolHeap _heap = new FoolHeap(MEMSIZE);
        private HashSet<FoolHeapCell> _heapsInUse = new HashSet<FoolHeapCell>();
        List<string> output = new List<string>();       //Contains the output/errors

        public ExecuteVirtualMachine(int[] code)
        {
            _code = code;
        }
        public List<string> Cpu()
        {
            try
            {
                while (true)
                {
                    int byteCode = _code[_ip++];
                    int v1, v2;
                    int address;
                    switch (byteCode)
                    {
                        case SVMParser.PUSH:
                            Push(_code[_ip++]);
                            break;
                        case SVMParser.POP:
                            Pop();
                            break;
                        case SVMParser.ADD:
                            v1 = Pop();
                            v2 = Pop();
                            Push(v2 + v1);
                            break;
                        case SVMParser.MULT:
                            v1 = Pop();
                            v2 = Pop();
                            Push(v2 * v1);
                            break;
                        case SVMParser.DIV:
                            v1 = Pop();
                            v2 = Pop();
                            Push(v2 / v1);
                            break;
                        case SVMParser.SUB:
                            v1 = Pop();
                            v2 = Pop();
                            Push(v2 - v1);
                            break;
                        case SVMParser.STOREW:
                            address = Pop();
                            SetMemory(address, Pop());
                            break;
                        case SVMParser.LOADW:
                            Push(GetMemory(Pop()));
                            break;
                        case SVMParser.BRANCH:
                            address = _code[_ip];
                            _ip = address;
                            break;
                        case SVMParser.BRANCHEQ: //
                            address = _code[_ip++];
                            v1 = Pop();
                            v2 = Pop();
                            if (v2 == v1)
                            {
                                _ip = address;
                            }
                            break;
                        case SVMParser.BRANCHLESSEQ:
                            address = _code[_ip++];
                            v1 = Pop();
                            v2 = Pop();
                            if (v2 <= v1)
                            {
                                _ip = address;
                            }
                            break;
                        case SVMParser.JS:
                            address = Pop();
                            _ra = _ip;
                            _ip = address;
                            break;
                        case SVMParser.STORERA:
                            _ra = Pop();
                            break;
                        case SVMParser.LOADRA:
                            Push(_ra);
                            break;
                        case SVMParser.STORERV:
                            _rv = Pop();
                            break;
                        case SVMParser.LOADRV:
                            Push(_rv);
                            break;
                        case SVMParser.LOADFP:
                            Push(_fp);
                            break;
                        case SVMParser.STOREFP:
                            _fp = Pop();
                            break;
                        case SVMParser.COPYFP:
                            _fp = _sp;
                            break;
                        case SVMParser.STOREHP:
                            _hp = Pop();
                            break;
                        case SVMParser.LOADHP:
                            Push(_hp);
                            break;
                        case SVMParser.PRINT:
                            if (_sp < (START_ADDRESS + MEMSIZE))
                            {
                                var res = GetMemory(_sp);
                                output.Add(res.ToString());
                            }
                            else
                            {
                                output.Add("STACK IS EMPTY!\n");
                            }
                            break;
                        case SVMParser.NEW:
                            int dispatchTableAddress = Pop();
                            int argsNumber = Pop();
                            int[] arguments = new int[argsNumber];
                            for (int i = argsNumber - 1; i >= 0; i--)
                            {
                                arguments[i] = Pop();
                            }

                            FoolHeapCell memoryAllocated;
                            memoryAllocated = _heap.Allocate(argsNumber + 1);

                            _heapsInUse.Add(memoryAllocated);
                            int heapMemoryStart = memoryAllocated.GetIndex();
                            SetMemory(memoryAllocated.GetIndex(), dispatchTableAddress);
                            memoryAllocated = memoryAllocated.GetNext();

                            for (int i = 0; i < argsNumber; i++)
                            {
                                SetMemory(memoryAllocated.GetIndex(), arguments[i]);
                                memoryAllocated = memoryAllocated.GetNext();
                            }

                            Push(heapMemoryStart);

                            if (_heap.GetFreeAddress() > _hp)
                            {
                                _hp = _heap.GetFreeAddress();
                            }

                            if (_hp == -1)
                            {
                                throw new OutOfMemoryException("OUT OF MEMORY!" 
                                    + "You have allocated too much memory. No more memory!\n");
                            }
                            break;
                        case SVMParser.LOADMETHOD:
                            int codeAddress = Pop();
                            Push(_code[codeAddress]);
                            break;
                        case SVMParser.COPYTOPSTACK:
                            Push(GetMemory(_sp));
                            break;
                        case SVMParser.HALT:
                            return output;
                    }
                }
            }
            catch (Exception e)
            {
                if (e is FoolHeapOverflowError || e is FoolSegmentationFaultError || e is OutOfMemoryException)
                {
                    output.Add("ERROR(exVM): " + e.Message);
                }
                return output;
            }
        }

        private void SetMemory(int address, int value)
        {
            int location = address - START_ADDRESS;

            if (location < 0 || location >= MEMSIZE)
            {
                throw new FoolSegmentationFaultError();
            }
            _memory[location] = value;
        }

        private int GetMemory(int address)
        {
            int location = address - START_ADDRESS;

            if (location < 0 || location >= MEMSIZE)
            {
                throw new FoolSegmentationFaultError();
            }
            return _memory[location];
        }

        private int Pop()
        {
            try
            {
                int result = GetMemory(_sp);
                SetMemory(_sp++, 0);
                return result;

            }
            catch (FoolSegmentationFaultError)
            {
                throw new FoolSegmentationFaultError();
            }
        }

        private void Push(int v)
        {

            if (_sp - 1 < _hp)
            {
                throw new StackOverflowException("Stack OverFlow");
            }
            SetMemory(--_sp, v);
        }

        private void PrintStack()
        {
            Console.WriteLine("******** STACK STATE ********");
            int value = _sp - START_ADDRESS;
            if (!(value < MEMSIZE))
            {
                Console.WriteLine("The Stack is Empty!");
                Console.WriteLine();
                return;
            }
            for (int i = value; i < MEMSIZE; i++)
            {
                Console.WriteLine(_memory[i]);
            }
            Console.WriteLine();
        }
    }
}
