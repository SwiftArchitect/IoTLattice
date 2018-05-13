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
