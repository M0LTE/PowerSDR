namespace PowerSDR.Shared
{
    internal class WriteOp1Message : PalMessage
    {
        private Opcode opcode;
        private uint data1;
        private uint data2;

        public WriteOp1Message(Opcode opcode, uint data1, uint data2)
        {
            this.opcode = opcode;
            this.data1 = data1;
            this.data2 = data2;
        }
    }
}