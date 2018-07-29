namespace PowerSDR.Shared
{
    internal class GetDeviceInfoMessage : PalMessage
    {
        private uint index;

        public GetDeviceInfoMessage(uint index)
        {
            this.index = index;
        }
    }
}