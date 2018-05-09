//    @file:    RealTimeTracking.cs
//    @project: IoTLattice
//
//    @author:  Xavier Schott
//              https://www.swiftarchitect.com/
//              https://www.linkedin.com/in/xavierschott/
//              https://github.com/SwiftArchitect
//              https://stackoverflow.com/story/swiftarchitect
//
//    @license: https://opensource.org/licenses/MIT
//    Copyright (c) 2018, Xavier Schott
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in
//    all copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//    THE SOFTWARE.

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
        public IEnumerable<IoTRssiHistory> WithinRange(double seconds, int rssi = -100)
        {
            DropObsoleteRecords();

            var expiration = DateTimeOffset.Now.AddSeconds(-seconds);
            var actives = rssis.Where(pair => (pair.Value.LastSeen >= expiration) && (pair.Value.LastRssi >= rssi));
            var result = new List<IoTRssiHistory>();
            foreach (var active in actives)
            {
                var mac = active.Key;
                result.Add(new IoTRssiHistory() { ioTIdentifiable = iots[mac], rssiHistory = rssis[mac] });
            }
            return result;
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
