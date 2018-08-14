namespace PowerSDR.Shared
{
    public class SelectDeviceMessage : PalMessage
    {
        public uint Index;

        public SelectDeviceMessage(uint index)
        {
            this.Index = index;
        }
    }
}