namespace PowerSDR.Shared
{
    public class SetBufferSizeMessage : PalMessage
    {
        public uint Val;

        public SetBufferSizeMessage(uint val)
        {
            this.Val = val;
        }
    }
}