namespace IoTLattice.test
{
    class MockIoTIdentifiable : IIoTIdentifiable
    {
        readonly double rssi;
        readonly ulong macAddress;

        public MockIoTIdentifiable(double rssi, ulong macAddress)
        {
            this.rssi = rssi;
            this.macAddress = macAddress;
        }

        public ulong MACAddress() => macAddress;
        double IIoTIdentifiable.RSSI() => rssi;
    }
}
