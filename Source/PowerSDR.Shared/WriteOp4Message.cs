namespace PowerSDR.Shared
{
    internal class WriteOp4Message : PalMessage
    {
        private Opcode opcode;
        private uint data1;
        private float data2;

        public WriteOp4Message(Opcode opcode, uint data1, float data2)
        {
            this.opcode = opcode;
            this.data1 = data1;
            this.data2 = data2;
        }
    }
}