//    @file:    RssiHistory.cs
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
using System.Diagnostics;
using System.Linq;

namespace IoTLattice
{
    public class LimitedConcurrentQueue<ELEMENT> : ConcurrentQueue<ELEMENT>
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

