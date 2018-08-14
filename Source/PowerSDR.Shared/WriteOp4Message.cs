namespace PowerSDR.Shared
{
    public class WriteOp4Message : PalMessage
    {
        public Opcode Opcode;
        public uint Data1;
        public float Data2;

        public WriteOp4Message(Opcode opcode, uint data1, float data2)
        {
            this.Opcode = opcode;
            this.Data1 = data1;
            this.Data2 = data2;
        }
    }
}