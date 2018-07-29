using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerSDR.Shared
{
    class GetDeviceInfoResponseMessage : PalResponse
    {
        public bool Result { get; set; }
        public uint Model { get; internal set; }
        public uint Sn { get; internal set; }
    }
}
