namespace PowerSDR.Shared
{
    internal class SetBufferSizeMessage : PalMessage
    {
        private uint val;

        public SetBufferSizeMessage(uint val)
        {
            this.val = val;
        }
    }
}