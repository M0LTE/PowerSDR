namespace PowerSDR.Shared
{
    public class GetDeviceInfoMessage : PalMessage
    {
        public uint Index;

        public GetDeviceInfoMessage(uint index)
        {
            this.Index = index;
        }
    }
}