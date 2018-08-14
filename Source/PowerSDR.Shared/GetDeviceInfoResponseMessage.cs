using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerSDR.Shared
{
    public class GetDeviceInfoResponse : PalResponse
    {
        public bool Result { get; set; }
        public uint Model { get; set; }
        public uint Sn { get; set; }
    }
}
