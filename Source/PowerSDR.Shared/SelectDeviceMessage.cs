namespace PowerSDR.Shared
{
    internal class SelectDeviceMessage : PalMessage
    {
        private uint index;

        public SelectDeviceMessage(uint index)
        {
            this.index = index;
        }
    }
}