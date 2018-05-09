//    @file:    RealTimeTrackingTest.cs
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

using NUnit.Framework;

namespace IoTLattice.test
{
    [TestFixture()]
    public class RealTimeTrackingTest
    {
        [Test()]
        public void TestRealTimeTracking()
        {
            var tracking = new RealTimeTracking();
            tracking.TrackIoT(new MockIoTIdentifiable(-40, 42));
            tracking.TrackIoT(new MockIoTIdentifiable(-60, 43));

            var expected = "> 42:-40";
            var actual = ">";
            var ioTRssiHistories = tracking.WithinRange(seconds: 1, rssi: -40);
            foreach( var ioTRssiHistory in ioTRssiHistories) {
                var macAddress = ioTRssiHistory.ioTIdentifiable.MACAddress();
                var rssiHistory = ioTRssiHistory.rssiHistory;
                actual += $" {macAddress}:{rssiHistory.AverageRssi}";
            }
            Assert.AreEqual(expected, actual);
        }
    }
}
