namespace IoTLattice
{
    /// <summary>
    /// A structure combining an IIoTIdentifiable and a RssiHistory
    /// </summary>
    public struct IoTRssiHistory
    {
        public IIoTIdentifiable ioTIdentifiable;
        public RssiHistory rssiHistory;
    }
}
