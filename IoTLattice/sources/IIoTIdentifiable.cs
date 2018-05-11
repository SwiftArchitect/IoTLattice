namespace IoTLattice
{
    /// <summary>
    /// IoT (Internet of things) transmitor: something that emits a signal
    /// </summary>
    public interface IIoTIdentifiable
    {

        /// <summary>
        /// The Media Access Control address (Unique identifier)
        /// </summary>
        ulong MACAddress();

        /// <summary>
        /// The Received Signal Strength Indication (in dBm)
        /// </summary>
        double RSSI();
    }
}
