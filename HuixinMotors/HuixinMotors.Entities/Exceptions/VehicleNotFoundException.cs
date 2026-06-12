using System;

namespace HuixinMotors.Entities.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando no se encuentra un vehículo solicitado en el inventario.
    /// </summary>
    public class VehicleNotFoundException : Exception
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="VehicleNotFoundException"/> con un mensaje predeterminado.
        /// </summary>
        public VehicleNotFoundException() 
            : base("El vehículo solicitado no fue encontrado en el inventario.")
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="VehicleNotFoundException"/> con un mensaje de error especificado.
        /// </summary>
        /// <param name="message">Mensaje que describe el error.</param>
        public VehicleNotFoundException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="VehicleNotFoundException"/> con un mensaje de error especificado
        /// y una referencia a la excepción interna que es la causa de esta excepción.
        /// </summary>
        /// <param name="message">Mensaje que describe el error.</param>
        /// <param name="innerException">La excepción que es la causa de la excepción actual.</param>
        public VehicleNotFoundException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
