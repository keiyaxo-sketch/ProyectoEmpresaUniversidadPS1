using System.Collections.Generic;
using System.Threading.Tasks;
using HuixinMotors.Entities;

namespace HuixinMotors.Data.Interfaces
{
    /// <summary>
    /// Interfaz que define los métodos de acceso a datos para la entidad Vehiculo.
    /// </summary>
    public interface IVehiculoRepository
    {
        /// <summary>
        /// Obtiene todos los vehículos registrados que se encuentran activos.
        /// </summary>
        /// <returns>Una lista de vehículos.</returns>
        Task<IEnumerable<Vehiculo>> GetAllAsync();

        /// <summary>
        /// Obtiene un vehículo por su identificador único (Id) si se encuentra activo.
        /// </summary>
        /// <param name="id">Identificador único del vehículo.</param>
        /// <returns>El vehículo correspondiente o null si no se encuentra.</returns>
        Task<Vehiculo?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene un vehículo por su Número de Identificación Vehicular (VIN) si se encuentra activo.
        /// </summary>
        /// <param name="vin">Número de Identificación Vehicular (VIN).</param>
        /// <returns>El vehículo correspondiente o null si no se encuentra.</returns>
        Task<Vehiculo?> GetByVinAsync(string vin);

        /// <summary>
        /// Determina si un VIN ya se encuentra registrado en el sistema, permitiendo excluir opcionalmente un ID específico (útil para actualizaciones).
        /// </summary>
        /// <param name="vin">Número de Identificación Vehicular (VIN) a verificar.</param>
        /// <param name="excludeId">Identificador del vehículo a excluir de la verificación (opcional).</param>
        /// <returns>true si el VIN ya existe; de lo contrario, false.</returns>
        Task<bool> ExistsVinAsync(string vin, int? excludeId = null);

        /// <summary>
        /// Agrega un nuevo vehículo al inventario.
        /// </summary>
        /// <param name="vehiculo">Entidad vehículo a agregar.</param>
        /// <returns>El vehículo agregado con su identificador generado.</returns>
        Task<Vehiculo> AddAsync(Vehiculo vehiculo);

        /// <summary>
        /// Actualiza los datos de un vehículo existente en el inventario.
        /// </summary>
        /// <param name="vehiculo">Entidad vehículo con los datos actualizados.</param>
        /// <returns>La entidad vehículo actualizada.</returns>
        Task<Vehiculo> UpdateAsync(Vehiculo vehiculo);

        /// <summary>
        /// Elimina lógicamente o físicamente un vehículo del inventario según su identificador único.
        /// </summary>
        /// <param name="id">Identificador único del vehículo.</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Realiza una búsqueda de vehículos filtrando por un término y un campo específico (VIN, Modelo o Marca).
        /// </summary>
        /// <param name="term">El término de búsqueda.</param>
        /// <param name="campo">El nombre del campo sobre el cual aplicar el filtro ("VIN", "Modelo" o "Marca").</param>
        /// <returns>Una lista de vehículos que coinciden con el criterio de búsqueda.</returns>
        Task<IEnumerable<Vehiculo>> SearchAsync(string term, string campo);
    }
}
