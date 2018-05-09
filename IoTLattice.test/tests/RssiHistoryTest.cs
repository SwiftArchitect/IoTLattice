﻿//    @file:    RssiHistoryTest.cs
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
    public class RssiHistoryTest
    {
        [Test()]
        public void TestRssiHistory()
        {
            ulong macAddress = 42;
            var rssiHistory = new RssiHistory(macAddress);

            rssiHistory.TrackRssi(new MockIoTIdentifiable(-60, macAddress));
            rssiHistory.TrackRssi(new MockIoTIdentifiable(-40, macAddress));

            Assert.AreEqual(-40, rssiHistory.LastRssi);
            Assert.AreEqual(-50, rssiHistory.AverageRssi);
        }
    }
}
