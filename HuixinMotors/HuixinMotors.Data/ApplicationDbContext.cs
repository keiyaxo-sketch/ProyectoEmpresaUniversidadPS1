using Microsoft.EntityFrameworkCore;
using HuixinMotors.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HuixinMotors.Data
{
    /// <summary>
    /// Contexto de la base de datos de Entity Framework Core para la aplicación Huixin Motors.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ApplicationDbContext"/> utilizando las opciones especificadas.
        /// </summary>
        /// <param name="options">Las opciones de configuración para este contexto.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Colección de entidades Vehiculo en la base de datos.
        /// </summary>
        public DbSet<Vehiculo> Vehiculos { get; set; } = null!;

        /// <summary>
        /// Configura el modelo y las relaciones de base de datos utilizando Fluent API.
        /// </summary>
        /// <param name="modelBuilder">El constructor de modelos utilizado para configurar las entidades.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehiculo>(entity =>
            {
                // Mapeo explícito a la tabla
                entity.ToTable("Vehiculos");

                // Clave Primaria
                entity.HasKey(v => v.Id);

                // Configuración y restricciones del VIN (Único)
                entity.HasIndex(v => v.VIN)
                      .IsUnique()
                      .HasDatabaseName("UQ_Vehiculos_VIN");

                // Configuración de conversión del Enum EstadoInventario a string en la Base de Datos
                entity.Property(v => v.EstadoInventario)
                      .HasConversion<string>()
                      .HasMaxLength(20)
                      .IsRequired();

                // Índices de rendimiento especificados en el script SQL
                entity.HasIndex(v => v.EstadoInventario)
                      .HasDatabaseName("IX_Vehiculos_EstadoInventario");

                entity.HasIndex(v => new { v.Marca, v.Año })
                      .HasDatabaseName("IX_Vehiculos_Marca_Año");

                // Configuraciones de precisión decimal
                entity.Property(v => v.CapacidadBateriaKWh)
                      .HasPrecision(10, 2)
                      .IsRequired();

                entity.Property(v => v.PrecioVenta)
                      .HasPrecision(18, 2)
                      .IsRequired();

                // Valores por defecto
                entity.Property(v => v.Activo)
                      .HasDefaultValue(true);

                entity.Property(v => v.FechaCreacion)
                      .HasDefaultValueSql("GETUTCDATE()");
            });
        }

        /// <summary>
        /// Guarda todos los cambios realizados en este contexto en la base de datos de forma asíncrona,
        /// interceptando modificaciones para actualizar la fecha de modificación de forma automática.
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Guarda todos los cambios realizados en este contexto en la base de datos de forma síncrona,
        /// interceptando modificaciones para actualizar la fecha de modificación de forma automática.
        /// </summary>
        public override int SaveChanges()
        {
            ApplyAuditInformation();
            return base.SaveChanges();
        }

        /// <summary>
        /// Intercepta las entidades modificadas para asignarles la fecha y hora actual en formato UTC.
        /// </summary>
        private void ApplyAuditInformation()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(e => e.Entity is Vehiculo && e.State == EntityState.Modified);

            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity is Vehiculo vehiculo)
                {
                    vehiculo.FechaModificacion = DateTime.UtcNow;
                }
            }
        }
    }
}
