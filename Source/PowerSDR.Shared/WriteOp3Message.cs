namespace PowerSDR.Shared
{
    public class WriteOp3Message : PalMessage
    {
        public Opcode Opcode;
        public int Data1;
        public int Data2;

        public WriteOp3Message(Opcode opcode, int data1, int data2)
        {
            this.Opcode = opcode;
            this.Data1 = data1;
            this.Data2 = data2;
        }
    }
}