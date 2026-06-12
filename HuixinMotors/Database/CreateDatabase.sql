-- ============================================================================
-- Script de Creación de la Base de Datos para Huixin Motors
-- Sistema de Inventario de Vehículos Eléctricos
-- Materia: Sistemas I
-- ============================================================================

-- 1. Creación de la Base de Datos
CREATE DATABASE HuixinMotorsDB;
GO

USE HuixinMotorsDB;
GO

-- 2. Creación de la Tabla Vehiculos
CREATE TABLE Vehiculos (
    -- Identificador único auto-incremental (Primary Key)
    Id INT IDENTITY(1,1) NOT NULL,

    -- Vehicle Identification Number (17 caracteres, alfanumérico excluyendo I, O, Q)
    -- Se valida mediante CHECK constraint que tenga exactamente 17 caracteres de los permitidos.
    VIN NVARCHAR(17) NOT NULL,

    -- Modelo del vehículo (Obligatorio)
    Modelo NVARCHAR(100) NOT NULL,

    -- Marca del vehículo (Obligatorio)
    Marca NVARCHAR(100) NOT NULL,

    -- Color de la carrocería del vehículo
    Color NVARCHAR(50) NOT NULL,

    -- Año de fabricación (CHECK constraint para rango entre 2000 y 2030)
    Año INT NOT NULL,

    -- Autonomía estimada en kilómetros (CHECK constraint mayor a cero)
    AutonomiaKm INT NOT NULL,

    -- Capacidad de la batería en Kilovatios-hora (CHECK constraint mayor a cero)
    CapacidadBateriaKWh DECIMAL(10,2) NOT NULL,

    -- Potencia del motor en Caballos de Fuerza (CHECK constraint mayor a cero)
    PotenciaMotorHP INT NOT NULL,

    -- Estado del vehículo en el inventario (CHECK constraint para valores permitidos)
    EstadoInventario NVARCHAR(20) NOT NULL,

    -- Fecha de ingreso del vehículo al inventario
    FechaIngreso DATE NOT NULL,

    -- Precio de venta al público en decimal (CHECK constraint mayor o igual a cero)
    PrecioVenta DECIMAL(18,2) NOT NULL,

    -- Flag de borrado lógico / estado activo (Default 1)
    Activo BIT NOT NULL DEFAULT 1,

    -- Fecha y hora de creación del registro en formato UTC (Default GETUTCDATE())
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    -- Fecha y hora de la última modificación en formato UTC (Null al crearse)
    FechaModificacion DATETIME2 NULL,

    -- Restricciones de Llave Primaria y Única
    CONSTRAINT PK_Vehiculos PRIMARY KEY (Id),
    CONSTRAINT UQ_Vehiculos_VIN UNIQUE (VIN),

    -- Restricciones de Validación (CHECK Constraints)
    -- Validación estricta del VIN: exactamente 17 caracteres, solo letras mayúsculas alfanuméricas excepto I, O, Q
    CONSTRAINT CK_Vehiculos_VIN_Format CHECK (
        VIN LIKE '[A-HJ-NPR-Z0-9][A-HJ-NPR-Z0-9][A-HJ-NPR-Z0-9][A-HJ-NPR-Z0-9][A-HJ-NPR-Z0-9][A-HJ-NPR-Z0-9][A-HJ-NPR-Z0-9][A-HJ-NPR-Z0-9][A-HJ-NPR-Z0-9][A-HJ-NPR-Z0-9][A-HJ-NPR-Z0-9][A-HJ-NPR-Z0-9][A-HJ-NPR-Z0-9][A-HJ-NPR-Z0-9][A-HJ-NPR-Z0-9][A-HJ-NPR-Z0-9][A-HJ-NPR-Z0-9]'
    ),
    CONSTRAINT CK_Vehiculos_Año CHECK (Año BETWEEN 2000 AND 2030),
    CONSTRAINT CK_Vehiculos_AutonomiaKm CHECK (AutonomiaKm > 0),
    CONSTRAINT CK_Vehiculos_CapacidadBateriaKWh CHECK (CapacidadBateriaKWh > 0.00),
    CONSTRAINT CK_Vehiculos_PotenciaMotorHP CHECK (PotenciaMotorHP > 0),
    CONSTRAINT CK_Vehiculos_PrecioVenta CHECK (PrecioVenta >= 0.00),
    CONSTRAINT CK_Vehiculos_EstadoInventario CHECK (
        EstadoInventario IN ('Disponible', 'Reservado', 'Vendido', 'EnTransito', 'DeBaja')
    )
);
GO

-- 3. Índices no clúster de rendimiento
-- Índice para acelerar búsquedas y filtros por estado de inventario
CREATE NONCLUSTERED INDEX IX_Vehiculos_EstadoInventario 
ON Vehiculos (EstadoInventario);
GO

-- Índice compuesto para optimizar búsquedas por marca y año
CREATE NONCLUSTERED INDEX IX_Vehiculos_Marca_Año 
ON Vehiculos (Marca, Año);
GO

-- 4. Trigger para actualizar automáticamente la FechaModificacion en cada UPDATE
CREATE TRIGGER TR_Vehiculos_UpdateFechaModificacion
ON Vehiculos
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Solo actualizar si el registro ha cambiado realmente.
    -- Evita bucles infinitos y optimiza actualizaciones.
    UPDATE Vehiculos
    SET FechaModificacion = GETUTCDATE()
    FROM Vehiculos v
    INNER JOIN inserted i ON v.Id = i.Id;
END;
GO
