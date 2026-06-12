using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HuixinMotors.Entities
{
    /// <summary>
    /// Representa un vehículo eléctrico en el sistema de inventario de Huixin Motors.
    /// </summary>
    public class Vehiculo
    {
        /// <summary>
        /// Identificador único del vehículo (Clave Primaria, Autoincremental).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Número de Identificación Vehicular (VIN). Debe ser único y cumplir con el formato estándar de 17 caracteres alfanuméricos (excluyendo I, O, Q).
        /// </summary>
        [Required(ErrorMessage = "El VIN es obligatorio.")]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "El VIN debe tener exactamente 17 caracteres.")]
        [RegularExpression(@"^[A-HJ-NPR-Z0-9]{17}$", ErrorMessage = "El VIN no cumple con el formato estándar permitido (17 caracteres alfanuméricos sin I, O, Q).")]
        public string VIN { get; set; } = string.Empty;

        /// <summary>
        /// Modelo del vehículo.
        /// </summary>
        [Required(ErrorMessage = "El modelo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El modelo no puede superar los 100 caracteres.")]
        public string Modelo { get; set; } = string.Empty;

        /// <summary>
        /// Marca del vehículo.
        /// </summary>
        [Required(ErrorMessage = "La marca es obligatoria.")]
        [StringLength(100, ErrorMessage = "La marca no puede superar los 100 caracteres.")]
        public string Marca { get; set; } = string.Empty;

        /// <summary>
        /// Color de la carrocería del vehículo.
        /// </summary>
        [Required(ErrorMessage = "El color es obligatorio.")]
        [StringLength(50, ErrorMessage = "El color no puede superar los 50 caracteres.")]
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Año de fabricación del vehículo (Rango válido: 2000 a 2030).
        /// </summary>
        [Required(ErrorMessage = "El año es obligatorio.")]
        [Range(2000, 2030, ErrorMessage = "El año debe estar en el rango de 2000 a 2030.")]
        public int Año { get; set; }

        /// <summary>
        /// Autonomía estimada del vehículo expresada en kilómetros (Debe ser mayor a 0).
        /// </summary>
        [Required(ErrorMessage = "La autonomía es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La autonomía debe ser mayor a 0 km.")]
        public int AutonomiaKm { get; set; }

        /// <summary>
        /// Capacidad de la batería expresada en Kilovatios-hora (KWh) (Debe ser mayor a 0).
        /// </summary>
        [Required(ErrorMessage = "La capacidad de la batería es obligatoria.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La capacidad de la batería debe ser mayor a 0 KWh.")]
        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal CapacidadBateriaKWh { get; set; }

        /// <summary>
        /// Potencia del motor eléctrico expresada en Caballos de Fuerza (HP) (Debe ser mayor a 0).
        /// </summary>
        [Required(ErrorMessage = "La potencia del motor es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La potencia del motor debe ser mayor a 0 HP.")]
        public int PotenciaMotorHP { get; set; }

        /// <summary>
        /// Estado actual del vehículo en el inventario (Disponible, Reservado, Vendido, EnTransito, DeBaja).
        /// </summary>
        [Required(ErrorMessage = "El estado del inventario es obligatorio.")]
        public EstadoInventario EstadoInventario { get; set; }

        /// <summary>
        /// Fecha en la que ingresó el vehículo a la concesionaria.
        /// </summary>
        [Required(ErrorMessage = "La fecha de ingreso es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        /// <summary>
        /// Precio de venta estimado al público (Debe ser mayor o igual a 0).
        /// </summary>
        [Required(ErrorMessage = "El precio de venta es obligatorio.")]
        [Range(0.00, double.MaxValue, ErrorMessage = "El precio de venta debe ser mayor o igual a 0.")]
        [Column(TypeName = "DECIMAL(18,2)")]
        public decimal PrecioVenta { get; set; }

        /// <summary>
        /// Indica si el vehículo está activo en el inventario o ha sido borrado de forma lógica (Default: true).
        /// </summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Fecha y hora de registro del vehículo en el sistema (Formato UTC).
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha y hora de la última modificación en los datos del vehículo (Formato UTC).
        /// </summary>
        public DateTime? FechaModificacion { get; set; }
    }
}
