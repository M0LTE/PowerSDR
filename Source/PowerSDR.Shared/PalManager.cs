using System;

namespace PowerSDR
{
    /// <summary>
    /// Allow an alternative IPal instance to be dropped in to all code that preveiously referenced Pal directly, 
    /// e.g. for testing, for abstraction over a network...
    /// </summary>
    public sealed class PalManager
    {
        static IPal _instance = null;
        public static IPal Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("Need to initialise PalManager.Instance before using it");
                }

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
    }
}
