using Microsoft.EntityFrameworkCore;
using Veterinaria.Data;
using Veterinaria.Models;

namespace Veterinaria.Services;

/// <summary>
/// Servicio centralizado de acceso a datos para la veterinaria.
/// Encapsula todas las operaciones CRUD y consultas frecuentes.
/// </summary>
public class VeterinariaService
{
    private readonly VeterinariaDbContext _db;

    public VeterinariaService(VeterinariaDbContext db) => _db = db;

    // ════════════════════════════════════════════════════════════════════════
    // PROPIETARIOS
    // ════════════════════════════════════════════════════════════════════════

    public async Task<List<Propietario>> GetPropietariosAsync(string? busqueda = null)
    {
        var query = _db.Propietarios.AsQueryable();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            busqueda = busqueda.Trim().ToLower();
            query = query.Where(p =>
                p.Nombre.ToLower().Contains(busqueda) ||
                p.Apellido.ToLower().Contains(busqueda) ||
                p.Correo.ToLower().Contains(busqueda) ||
                p.Telefono.Contains(busqueda));
        }

        return await query.OrderBy(p => p.Apellido).ThenBy(p => p.Nombre).ToListAsync();
    }

    public async Task<Propietario?> GetPropietarioByIdAsync(int id) =>
        await _db.Propietarios.Include(p => p.Mascotas).FirstOrDefaultAsync(p => p.Id == id);

    public async Task<bool> CorreoExisteAsync(string correo, int? excluirId = null) =>
        await _db.Propietarios.AnyAsync(p =>
            p.Correo.ToLower() == correo.ToLower() &&
            (excluirId == null || p.Id != excluirId));

    public async Task CrearPropietarioAsync(Propietario propietario)
    {
        _db.Propietarios.Add(propietario);
        await _db.SaveChangesAsync();
    }

    public async Task ActualizarPropietarioAsync(Propietario propietario)
    {
        _db.Propietarios.Update(propietario);
        await _db.SaveChangesAsync();
    }

    public async Task EliminarPropietarioAsync(int id)
    {
        var propietario = await _db.Propietarios.FindAsync(id);
        if (propietario is not null)
        {
            _db.Propietarios.Remove(propietario);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<bool> PropietarioTieneMascotasAsync(int id) =>
        await _db.Mascotas.AnyAsync(m => m.PropietarioId == id);

    // ════════════════════════════════════════════════════════════════════════
    // MASCOTAS
    // ════════════════════════════════════════════════════════════════════════

    public async Task<List<Mascota>> GetMascotasActivasAsync() =>
        await _db.Mascotas
            .Include(m => m.Propietario)
            .Where(m => m.Estado == EstadoGeneral.Activo)
            .OrderBy(m => m.Nombre)
            .ToListAsync();

    public async Task<List<Mascota>> GetMascotasAsync(string? busqueda = null, int? propietarioId = null)
    {
        var query = _db.Mascotas.Include(m => m.Propietario).AsQueryable();

        if (propietarioId.HasValue)
            query = query.Where(m => m.PropietarioId == propietarioId.Value);

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            busqueda = busqueda.Trim().ToLower();
            query = query.Where(m =>
                m.Nombre.ToLower().Contains(busqueda) ||
                m.Especie.ToLower().Contains(busqueda) ||
                (m.Raza != null && m.Raza.ToLower().Contains(busqueda)));
        }

        return await query.OrderBy(m => m.Nombre).ToListAsync();
    }

    public async Task<Mascota?> GetMascotaByIdAsync(int id) =>
        await _db.Mascotas.Include(m => m.Propietario).FirstOrDefaultAsync(m => m.Id == id);

    public async Task CrearMascotaAsync(Mascota mascota)
    {
        _db.Mascotas.Add(mascota);
        await _db.SaveChangesAsync();
    }

    public async Task ActualizarMascotaAsync(Mascota mascota)
    {
        _db.Mascotas.Update(mascota);
        await _db.SaveChangesAsync();
    }

    public async Task EliminarMascotaAsync(int id)
    {
        var mascota = await _db.Mascotas.FindAsync(id);
        if (mascota is not null)
        {
            _db.Mascotas.Remove(mascota);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<bool> MascotaTieneCitasAsync(int id) =>
        await _db.Citas.AnyAsync(c => c.MascotaId == id);

    // ════════════════════════════════════════════════════════════════════════
    // VETERINARIOS
    // ════════════════════════════════════════════════════════════════════════

    public async Task<List<Veterinario>> GetVeterinariosAsync(string? busqueda = null)
    {
        var query = _db.Veterinarios.AsQueryable();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            busqueda = busqueda.Trim().ToLower();
            query = query.Where(v =>
                v.Nombre.ToLower().Contains(busqueda) ||
                v.Apellido.ToLower().Contains(busqueda) ||
                v.Especialidad.ToLower().Contains(busqueda));
        }

        return await query.OrderBy(v => v.Apellido).ThenBy(v => v.Nombre).ToListAsync();
    }

    public async Task<List<Veterinario>> GetVeterinariosActivosAsync() =>
        await _db.Veterinarios
            .Where(v => v.Estado == EstadoGeneral.Activo)
            .OrderBy(v => v.Apellido)
            .ToListAsync();

    public async Task<Veterinario?> GetVeterinarioByIdAsync(int id) =>
        await _db.Veterinarios.FindAsync(id);

    public async Task CrearVeterinarioAsync(Veterinario veterinario)
    {
        _db.Veterinarios.Add(veterinario);
        await _db.SaveChangesAsync();
    }

    public async Task ActualizarVeterinarioAsync(Veterinario veterinario)
    {
        _db.Veterinarios.Update(veterinario);
        await _db.SaveChangesAsync();
    }

    public async Task EliminarVeterinarioAsync(int id)
    {
        var veterinario = await _db.Veterinarios.FindAsync(id);
        if (veterinario is not null)
        {
            _db.Veterinarios.Remove(veterinario);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<bool> VeterinarioTieneCitasAsync(int id) =>
        await _db.Citas.AnyAsync(c => c.VeterinarioId == id);

    // ════════════════════════════════════════════════════════════════════════
    // CITAS
    // ════════════════════════════════════════════════════════════════════════

    public async Task<List<Cita>> GetCitasAsync(
        string? busqueda = null,
        EstadoCita? estado = null,
        DateTime? desde = null,
        DateTime? hasta = null)
    {
        var query = _db.Citas
            .Include(c => c.Mascota).ThenInclude(m => m!.Propietario)
            .Include(c => c.Veterinario)
            .AsQueryable();

        if (estado.HasValue)
            query = query.Where(c => c.Estado == estado.Value);

        if (desde.HasValue)
            query = query.Where(c => c.FechaHoraAtencion >= desde.Value);

        if (hasta.HasValue)
            query = query.Where(c => c.FechaHoraAtencion <= hasta.Value);

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            busqueda = busqueda.Trim().ToLower();
            query = query.Where(c =>
                c.Mascota!.Nombre.ToLower().Contains(busqueda) ||
                c.Veterinario!.Apellido.ToLower().Contains(busqueda) ||
                c.Motivo.ToLower().Contains(busqueda));
        }

        return await query.OrderBy(c => c.FechaHoraAtencion).ToListAsync();
    }

    public async Task<List<Cita>> GetCitasPorVeterinarioHoyAsync(int veterinarioId)
    {
        var hoy  = DateTime.Today;
        var manana = hoy.AddDays(1);
        return await _db.Citas
            .Include(c => c.Mascota)
            .Include(c => c.Veterinario)
            .Where(c => c.VeterinarioId == veterinarioId
                     && c.FechaHoraAtencion >= hoy
                     && c.FechaHoraAtencion < manana)
            .OrderBy(c => c.FechaHoraAtencion)
            .ToListAsync();
    }

    public async Task<List<Cita>> GetCitasDeHoyAsync()
    {
        var hoy = DateTime.Today;
        var manana = hoy.AddDays(1);
        return await _db.Citas
            .Include(c => c.Mascota)
            .Include(c => c.Veterinario)
            .Where(c => c.FechaHoraAtencion >= hoy && c.FechaHoraAtencion < manana)
            .OrderBy(c => c.FechaHoraAtencion)
            .ToListAsync();
    }

    public async Task<Cita?> GetCitaByIdAsync(int id) =>
        await _db.Citas
            .Include(c => c.Mascota).ThenInclude(m => m!.Propietario)
            .Include(c => c.Veterinario)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task CrearCitaAsync(Cita cita)
    {
        _db.Citas.Add(cita);
        await _db.SaveChangesAsync();
    }

    public async Task ActualizarCitaAsync(Cita cita)
    {
        _db.Citas.Update(cita);
        await _db.SaveChangesAsync();
    }

    public async Task EliminarCitaAsync(int id)
    {
        var cita = await _db.Citas.FindAsync(id);
        if (cita is not null)
        {
            _db.Citas.Remove(cita);
            await _db.SaveChangesAsync();
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    // ESTADÍSTICAS PARA EL DASHBOARD
    // ════════════════════════════════════════════════════════════════════════

    public async Task<DashboardStats> GetDashboardStatsAsync()
    {
        var hoy = DateTime.Today;
        var manana = hoy.AddDays(1);

        return new DashboardStats
        {
            TotalPropietarios    = await _db.Propietarios.CountAsync(),
            PropietariosActivos  = await _db.Propietarios.CountAsync(p => p.Estado == EstadoGeneral.Activo),
            TotalMascotas        = await _db.Mascotas.CountAsync(),
            MascotasActivas      = await _db.Mascotas.CountAsync(m => m.Estado == EstadoGeneral.Activo),
            TotalVeterinarios    = await _db.Veterinarios.CountAsync(),
            VeterinariosActivos  = await _db.Veterinarios.CountAsync(v => v.Estado == EstadoGeneral.Activo),
            CitasHoy             = await _db.Citas.CountAsync(c => c.FechaHoraAtencion >= hoy && c.FechaHoraAtencion < manana),
            CitasPendientes      = await _db.Citas.CountAsync(c => c.Estado == EstadoCita.Pendiente),
            CitasCompletadas     = await _db.Citas.CountAsync(c => c.Estado == EstadoCita.Completada),
            CitasCanceladas      = await _db.Citas.CountAsync(c => c.Estado == EstadoCita.Cancelada),
        };
    }
}

/// <summary>DTO con las estadísticas resumidas para el dashboard.</summary>
public record DashboardStats
{
    public int TotalPropietarios   { get; init; }
    public int PropietariosActivos { get; init; }
    public int TotalMascotas       { get; init; }
    public int MascotasActivas     { get; init; }
    public int TotalVeterinarios   { get; init; }
    public int VeterinariosActivos { get; init; }
    public int CitasHoy            { get; init; }
    public int CitasPendientes     { get; init; }
    public int CitasCompletadas    { get; init; }
    public int CitasCanceladas     { get; init; }
}
