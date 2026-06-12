using System;

namespace HuixinMotors.Entities.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando se intenta registrar o actualizar un vehículo con un VIN que ya existe en el sistema.
    /// </summary>
    public class DuplicateVinException : Exception
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DuplicateVinException"/> con un mensaje predeterminado.
        /// </summary>
        public DuplicateVinException() 
            : base("El Número de Identificación Vehicular (VIN) especificado ya se encuentra registrado.")
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DuplicateVinException"/> con un mensaje de error especificado.
        /// </summary>
        /// <param name="message">Mensaje que describe el error.</param>
        public DuplicateVinException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DuplicateVinException"/> con un mensaje de error especificado
        /// y la excepción interna que causó este error.
        /// </summary>
        /// <param name="message">Mensaje que describe el error.</param>
        /// <param name="innerException">La excepción que es la causa de la excepción actual.</param>
        public DuplicateVinException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
