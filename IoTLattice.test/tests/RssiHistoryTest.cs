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
