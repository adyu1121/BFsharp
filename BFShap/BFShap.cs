using System;
using System.Collections.Generic;

namespace BrainFuck.BFShap
{
    public class BFShap
    {
        private enum symbolList
        {
            Plus = '+',
            Minus = '-',
            PointUp = '>',
            PointDown = '<',
            Input = ',',
            OutPut = '.',
            LoopStart = '[',
            LoopEnd = ']'
        }
        public delegate char Input();
        public delegate void Output(char ASCII);

        private const ushort MemoryLength = 32768;

        public long this[int i]
        {
            get { return Memory[i]; }
            set { Memory[i] = value; }
        }
        public string Code
        {
            get
            {
                string result = string.Empty;
                foreach (symbolList symbol in BFCode)
                {
                    result += (char)symbol;
                }
                return result;
            }
            set
            {
                BFCode.Clear();
                Init();
                foreach (char ch in value)
                {
                    if(Enum.IsDefined(typeof(symbolList), (int)ch))
                        BFCode.Add((symbolList)ch);
                }
            }
        }
        public int Index { get; private set; }
        public int Pointer { get; private set; }

        private long[] Memory = new long[MemoryLength];
        private Stack<int> LoopStack = new Stack<int>();
        private Error LastError = new Error(0,-1);

        private List<symbolList> BFCode = new List<symbolList>();
        private Input InputFunc;
        private Output OutputFunc;

        public BFShap(Input input, Output output) : this(string.Empty, input, output) { }
        public BFShap(string code, Input input, Output output)
        {
            Init();
            Code = code;
            SetIn(input);
            SetOut(output);
        }

        public void Init()
        {
            Index = 0;
            Pointer = 0;
            Array.Fill(Memory, 0);
            LoopStack.Clear();
            LastError = new Error(0, -1);
        }
        public void SetIn(Input input)
        {
            InputFunc = input;
        }
        public void SetOut(Output output)
        {
            OutputFunc = output;
        }
        public Error GetLastError()
        {
            return LastError;
        }
        public bool Step(int loop)
        {
            for (int i = loop; i != 0; i--)
            {
                bool result = Stap();
                if (!result) return false;
            }
            return true;
        }
        public bool Step()
        {
            if (Index > BFCode.Count - 1)
                if (LoopStack.Count == 0) return SetError(ErrorrCode.CodeEnd);
                else return SetError(ErrorrCode.LoopIsUnend);
            switch (BFCode[Index])
            {
                case symbolList.Plus:
                    if (Memory[Pointer] == long.MaxValue) return SetError(ErrorrCode.Overflow);
                    Memory[Pointer]++;
                    break;
                case symbolList.Minus:
                    if (Memory[Pointer] == long.MinValue) return SetError(ErrorrCode.Underflow);
                    Memory[Pointer]--;
                    break;
                case symbolList.PointUp:
                    if (Pointer == MemoryLength - 1) return SetError(ErrorrCode.MemoryOver);
                    Pointer++;
                    break;
                case symbolList.PointDown:
                    if (Pointer == 0) return SetError(ErrorrCode.MemoryUnder);
                    Pointer--;
                    break;
                case symbolList.Input:
                    Memory[Pointer] = InputFunc();
                    break;
                case symbolList.OutPut:
                    OutputFunc((char)Memory[Pointer]);
                    break;
                case symbolList.LoopStart:
                    if (Memory[Pointer] != 0)
                    {
                        LoopStack.Push(Index);
                    }
                    else
                    {
                        int NestedLoopCount = 0;
                        while (true)
                        {
                            Index++;
                            if (Index > BFCode.Count - 1) return SetError(ErrorrCode.LoopIsUnend);
                            if (BFCode[Index] == symbolList.LoopStart)
                            {
                                NestedLoopCount++;
                            }
                            if (BFCode[Index] == symbolList.LoopEnd)
                            {
                                if (NestedLoopCount != 0)
                                {
                                    NestedLoopCount--;
                                }
                                else break;
                            }
                        }
                    }
                    break;
                case symbolList.LoopEnd:
                    if (LoopStack.Count == 0) return SetError(ErrorrCode.LoopIsUnstart);
                    Index = LoopStack.Pop() - 1;
                    break;
            }
            Index++;
            return true;
        }
        public override string ToString() 
        {
            return Code;
        }
        private bool SetError(ErrorrCode error)
        {
            LastError = new Error(error, Index);
            return false;
        }
    }
}
