namespace PowerSDR.Shared
{
    internal class ReadOp2Message : PalMessage
    {
        private Opcode opcode;
        private uint data1;
        private uint data2;

        public ReadOp2Message(Opcode opcode, uint data1, uint data2)
        {
            this.opcode = opcode;
            this.data1 = data1;
            this.data2 = data2;
        }
    }
}