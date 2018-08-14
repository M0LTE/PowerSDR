namespace PowerSDR.Shared
{
    public class WriteOp2Message : PalMessage
    {
        public Opcode Opcode;
        public float Data1;
        public uint Data2;

        public WriteOp2Message(Opcode opcode, float data1, uint data2)
        {
            this.Opcode = opcode;
            this.Data1 = data1;
            this.Data2 = data2;
        }
    }
}