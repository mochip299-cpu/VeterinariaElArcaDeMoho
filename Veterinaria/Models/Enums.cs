namespace Veterinaria.Models;

/// <summary>Estado general para Propietario, Mascota y Veterinario.</summary>
public enum EstadoGeneral
{
    Activo = 0,
    Inactivo = 1
}

/// <summary>Estado del ciclo de vida de una cita.</summary>
public enum EstadoCita
{
    Pendiente = 0,
    Completada = 1,
    Cancelada = 2
}
