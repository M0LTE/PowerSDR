namespace PowerSDR
{
    public class Misc
    {
        public static string SerialToString(uint serial)
        {
            string s = "";
            s += ((byte)(serial)).ToString("00");
            s += ((byte)(serial >> 8)).ToString("00") + "-";
            s += ((ushort)(serial >> 16)).ToString("0000");
            return s;
        }
    }
}
