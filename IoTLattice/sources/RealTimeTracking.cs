using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IoTLattice
{
    public class RealTimeTracking
    {
        readonly ConcurrentDictionary<ulong, RssiHistory> rssis = new ConcurrentDictionary<ulong, RssiHistory>();
        readonly ConcurrentDictionary<ulong, IIoTIdentifiable> iots = new ConcurrentDictionary<ulong, IIoTIdentifiable>();
        readonly double obsolescence;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:IoTLattice.RealTimeTracking"/> class.
        /// </summary>
        /// <param name="obsolescence">Obsolescence: discard data older than this time in seconds</param>
        public RealTimeTracking(double obsolescence = 30)
        {
            this.obsolescence = obsolescence;
        }

        public void TrackIoT(IIoTIdentifiable iot)
        {
            var mac = iot.MACAddress();
            RssiHistory rssiHistory = (rssis.ContainsKey(mac))
                ? rssis[mac]
                : new RssiHistory(iot.MACAddress());

            rssiHistory.TrackRssi(iot);
            rssis[mac] = rssiHistory;
            iots[mac] = iot;
        }

        /// <summary>
        /// A list of IoT which have been recently seen above a given threshold
        /// </summary>
        /// <returns>All IoT within range, according to the rules</returns>
        /// <param name="seconds">Msaximum amount of time, in seconds, since IoT was last seen</param>
        /// <param name="rssi">Minimum signal strengh when last seen to qualify. Default: no minimum</param>
        public IEnumerable<IoTAverageRssi> WithinRange(double seconds, int rssi = -100)
        {
            DropObsoleteRecords();

            var expiration = DateTimeOffset.Now.AddSeconds(-seconds);
            var actives = rssis.Where(pair => (pair.Value.LastSeen >= expiration) && (pair.Value.LastRssi >= rssi));

            return actives.Select((KeyValuePair<ulong, RssiHistory> active) =>
            {
                var mac = active.Key;
                return new IoTAverageRssi(iots[mac], rssis[mac].AverageRssi);
            });
        }

        private void DropObsoleteRecords()
        {
            var outdated = DateTimeOffset.Now.AddSeconds(-obsolescence);
            var obsoletes = rssis.Where(pair => pair.Value.LastSeen <= outdated);
            foreach (var obsolete in obsoletes)
            {
                // Debug.WriteLine($"Removed obsolete {obsolete.Key}");
                var mac = obsolete.Key;
                rssis.TryRemove(mac, out RssiHistory rssiHistory);
                iots.TryRemove(mac, out IIoTIdentifiable ioTIdentifiable);
            }
        }
    }
}
