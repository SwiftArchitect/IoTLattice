namespace IoTLattice
{
    /// <summary>
    /// A structure combining an IIoTIdentifiable and a RssiHistory
    /// </summary>
    public struct IoTAverageRssi
    {
        public readonly IIoTIdentifiable IoTIdentifiable;
        public readonly double AverageRssi;

        public IoTAverageRssi(IIoTIdentifiable ioTIdentifiable, double averageRssi)
        {
            IoTIdentifiable = ioTIdentifiable;
            AverageRssi = averageRssi;
        }
    }
}
