﻿namespace PowerSDR.Shared
{
    public class ReadOp1Message : PalMessage
    {
        public Opcode Opcode;
        public uint Data1;
        public uint Data2;

        public ReadOp1Message(Opcode opcode, uint data1, uint data2)
        {
            this.Opcode = opcode;
            this.Data1 = data1;
            this.Data2 = data2;
        }
    }
}