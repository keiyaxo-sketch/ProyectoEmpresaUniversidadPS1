namespace HuixinMotors.Entities
{
    /// <summary>
    /// Representa los diferentes estados de inventario en los que puede encontrarse un vehículo eléctrico.
    /// </summary>
    public enum EstadoInventario
    {
        /// <summary>
        /// El vehículo está disponible para la venta inmediata.
        /// </summary>
        Disponible,

        /// <summary>
        /// El vehículo se encuentra reservado por un cliente (con apartado o enganche).
        /// </summary>
        Reservado,

        /// <summary>
        /// El vehículo ya ha sido vendido al cliente final.
        /// </summary>
        Vendido,

        /// <summary>
        /// El vehículo está en tránsito desde el fabricante o puerto hacia la concesionaria.
        /// </summary>
        EnTransito,

        /// <summary>
        /// El vehículo ha sido retirado de la venta de forma definitiva (siniestro, obsolescencia, etc.).
        /// </summary>
        DeBaja
    }
}
