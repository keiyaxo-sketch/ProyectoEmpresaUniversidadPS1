using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HuixinMotors.Entities;
using HuixinMotors.Entities.Exceptions;
using HuixinMotors.Data.Interfaces;

namespace HuixinMotors.Data.Repositories
{
    /// <summary>
    /// Implementación de acceso a datos para la entidad Vehiculo utilizando Entity Framework Core.
    /// </summary>
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="VehiculoRepository"/> con el contexto especificado.
        /// </summary>
        /// <param name="context">El contexto de la base de datos.</param>
        public VehiculoRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Obtiene todos los vehículos registrados y activos en el inventario de forma asíncrona.
        /// </summary>
        public async Task<IEnumerable<Vehiculo>> GetAllAsync()
        {
            return await _context.Vehiculos
                .AsNoTracking()
                .Where(v => v.Activo)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un vehículo por su identificador único (Id) si se encuentra activo, de forma asíncrona.
        /// </summary>
        public async Task<Vehiculo?> GetByIdAsync(int id)
        {
            return await _context.Vehiculos
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id && v.Activo);
        }

        /// <summary>
        /// Obtiene un vehículo por su VIN si se encuentra activo, de forma asíncrona.
        /// </summary>
        public async Task<Vehiculo?> GetByVinAsync(string vin)
        {
            if (string.IsNullOrWhiteSpace(vin))
                return null;

            return await _context.Vehiculos
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.VIN == vin && v.Activo);
        }

        /// <summary>
        /// Determina si un VIN ya se encuentra registrado en el sistema para otro vehículo activo.
        /// </summary>
        public async Task<bool> ExistsVinAsync(string vin, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(vin))
                return false;

            return await _context.Vehiculos
                .AnyAsync(v => v.VIN == vin && v.Activo && (!excludeId.HasValue || v.Id != excludeId.Value));
        }

        /// <summary>
        /// Agrega un nuevo vehículo al inventario. Controla que el VIN sea único.
        /// </summary>
        public async Task<Vehiculo> AddAsync(Vehiculo vehiculo)
        {
            if (vehiculo == null)
                throw new ArgumentNullException(nameof(vehiculo));

            // Validar unicidad del VIN antes de insertar
            if (await ExistsVinAsync(vehiculo.VIN))
            {
                throw new DuplicateVinException($"El VIN '{vehiculo.VIN}' ya está registrado en el inventario.");
            }

            await _context.Vehiculos.AddAsync(vehiculo);
            await _context.SaveChangesAsync();
            return vehiculo;
        }

        /// <summary>
        /// Actualiza los datos de un vehículo existente. Lanza excepciones si no existe o si el VIN está duplicado.
        /// </summary>
        public async Task<Vehiculo> UpdateAsync(Vehiculo vehiculo)
        {
            if (vehiculo == null)
                throw new ArgumentNullException(nameof(vehiculo));

            // Buscar la entidad existente y activa
            var dbVehiculo = await _context.Vehiculos
                .FirstOrDefaultAsync(v => v.Id == vehiculo.Id && v.Activo);

            if (dbVehiculo == null)
            {
                throw new VehicleNotFoundException($"No se encontró el vehículo con Id {vehiculo.Id} en el inventario.");
            }

            // Validar unicidad del VIN excluyendo al vehículo actual
            if (await ExistsVinAsync(vehiculo.VIN, vehiculo.Id))
            {
                throw new DuplicateVinException($"El VIN '{vehiculo.VIN}' ya está asignado a otro vehículo en el inventario.");
            }

            // Actualizar propiedades individuales para evitar sobrescribir FechaCreacion
            dbVehiculo.VIN = vehiculo.VIN;
            dbVehiculo.Modelo = vehiculo.Modelo;
            dbVehiculo.Marca = vehiculo.Marca;
            dbVehiculo.Color = vehiculo.Color;
            dbVehiculo.Año = vehiculo.Año;
            dbVehiculo.AutonomiaKm = vehiculo.AutonomiaKm;
            dbVehiculo.CapacidadBateriaKWh = vehiculo.CapacidadBateriaKWh;
            dbVehiculo.PotenciaMotorHP = vehiculo.PotenciaMotorHP;
            dbVehiculo.EstadoInventario = vehiculo.EstadoInventario;
            dbVehiculo.FechaIngreso = vehiculo.FechaIngreso;
            dbVehiculo.PrecioVenta = vehiculo.PrecioVenta;
            
            // Si el cliente modificó explícitamente el flag de borrado lógico
            dbVehiculo.Activo = vehiculo.Activo;

            await _context.SaveChangesAsync();
            return dbVehiculo;
        }

        /// <summary>
        /// Realiza el borrado lógico de un vehículo en el inventario. Lanza excepción si no existe.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var dbVehiculo = await _context.Vehiculos
                .FirstOrDefaultAsync(v => v.Id == id && v.Activo);

            if (dbVehiculo == null)
            {
                throw new VehicleNotFoundException($"No se encontró el vehículo con Id {id} para eliminar.");
            }

            // Borrado lógico
            dbVehiculo.Activo = false;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Busca vehículos activos que coincidan con el término de búsqueda en el campo especificado (VIN, Modelo o Marca).
        /// </summary>
        public async Task<IEnumerable<Vehiculo>> SearchAsync(string term, string campo)
        {
            if (string.IsNullOrWhiteSpace(term) || string.IsNullOrWhiteSpace(campo))
            {
                return Enumerable.Empty<Vehiculo>();
            }

            IQueryable<Vehiculo> query = _context.Vehiculos
                .AsNoTracking()
                .Where(v => v.Activo);

            string campoLower = campo.Trim().ToLowerInvariant();
            string termTrimmed = term.Trim();

            switch (campoLower)
            {
                case "vin":
                    query = query.Where(v => v.VIN.Contains(termTrimmed));
                    break;
                case "modelo":
                    query = query.Where(v => v.Modelo.Contains(termTrimmed));
                    break;
                case "marca":
                    query = query.Where(v => v.Marca.Contains(termTrimmed));
                    break;
                default:
                    // Campo no soportado
                    return Enumerable.Empty<Vehiculo>();
            }

            return await query.ToListAsync();
        }
    }
}
