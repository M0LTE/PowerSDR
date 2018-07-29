using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerSDR.Shared
{
    public abstract class CommonPal
    {
        public SimpleRadio[] Scan()
        {
            int devs = PalManager.Instance.GetNumDevices();                 // get numer of radios found 

            //    System.Diagnostics.Trace.WriteLine("pal=============================");

            if (devs == 0) return null;

            SimpleRadio[] radios = new SimpleRadio[devs];

            for (uint i = 0; i < devs; i++)
            {
                uint model;
                uint sn;

                if (!PalManager.Instance.GetDeviceInfo(i, out model, out sn)) return null;

                Model m = Model.FLEX5000;
                if (model == 3) m = Model.FLEX3000;

                string serial = Misc.SerialToString(sn);   // radios serial#

                radios[i] = new SimpleRadio(m, i, serial, true);
            }

            return radios;
        }
    }
}
