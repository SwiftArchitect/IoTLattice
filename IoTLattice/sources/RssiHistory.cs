using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace IoTLattice
{
    internal class LimitedConcurrentQueue<ELEMENT> : ConcurrentQueue<ELEMENT>
    {
        public readonly int Limit;

        public LimitedConcurrentQueue(int limit)
        {
            Limit = limit;
        }

        public new void Enqueue(ELEMENT element)
        {
            base.Enqueue(element);
            if (Count > Limit)
            {
                TryDequeue(out ELEMENT discard);
            }
        }
    }

    /// <summary>
    /// Rssi history for an object adopting the IIoTIdentifiable interface
    /// </summary>
    public class RssiHistory
    {
        public IIoTIdentifiable IoT { get; private set; }
        public double AverageRssi => rssis.Average();
        public readonly ulong MACAddress;

        public DateTimeOffset LastSeen { get; private set; }
        public double LastRssi => IoT.RSSI();

        readonly LimitedConcurrentQueue<double> rssis;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:IoTLattice.RssiHistory"/> class.
        /// </summary>
        /// <param name="macAddress">Iot MAC address.</param>
        /// <param name="historyLimit">History size, with a short queue if not specified.</param>
        public RssiHistory(ulong macAddress, int historyLimit = 20)
        {
            rssis = new LimitedConcurrentQueue<double>(historyLimit);
            MACAddress = macAddress;
        }

        /// <summary>
        /// Rssi changed: update history, record last time
        /// </summary>
        /// <returns>T</returns>
        /// <param name="iot">The IoT.</param>
        public void TrackRssi(IIoTIdentifiable iot)
        {
            Debug.Assert(MACAddress == iot.MACAddress());
            if (MACAddress == iot.MACAddress())
            {
                IoT = iot;
                rssis.Enqueue(iot.RSSI());
                LastSeen = DateTimeOffset.Now;
            }
        }
    }
}
