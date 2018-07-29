namespace PowerSDR.Shared
{
    internal class WriteOp2Message : PalMessage
    {
        private Opcode opcode;
        private float data1;
        private uint data2;

        public WriteOp2Message(Opcode opcode, float data1, uint data2)
        {
            this.opcode = opcode;
            this.data1 = data1;
            this.data2 = data2;
        }
    }
}