using System;

namespace PowerSDR.Shared
{
    public class SimpleRadio
    {
        public SimpleRadio(Model model, object accessObj, string serial, bool present)
        {
            Model = model;
            AccessObject = (UInt32)accessObj;
            SerialNumber = serial;
            Present = present;
        }

        public bool Present { get; set; }
        public Model Model { get; set; }
        public string Nickname { get; set; }
        public string SerialNumber { get; set; }
        public UInt32 AccessObject { get; set; }
    }
}