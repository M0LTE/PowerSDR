namespace PowerSDR.Shared
{
    public class ReadOp2Message : PalMessage
    {
        public Opcode Opcode;
        public uint Data1;
        public uint Data2;

        public ReadOp2Message(Opcode opcode, uint data1, uint data2)
        {
            this.Opcode = opcode;
            this.Data1 = data1;
            this.Data2 = data2;
        }
    }
}