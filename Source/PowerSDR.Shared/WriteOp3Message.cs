namespace PowerSDR.Shared
{
    internal class WriteOp3Message : PalMessage
    {
        private Opcode opcode;
        private int data1;
        private int data2;

        public WriteOp3Message(Opcode opcode, int data1, int data2)
        {
            this.opcode = opcode;
            this.data1 = data1;
            this.data2 = data2;
        }
    }
}