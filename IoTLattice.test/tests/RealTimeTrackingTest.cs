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
            var ioTAverageRssis = tracking.WithinRange(seconds: 1, rssi: -40);
            foreach( var ioTAverageRssi in ioTAverageRssis) {
                var macAddress = ioTAverageRssi.IoTIdentifiable.MACAddress();
                var averageRssi = ioTAverageRssi.AverageRssi;
                actual += $" {macAddress}:{averageRssi}";
            }
            Assert.AreEqual(expected, actual);
        }
    }
}
