using Microsoft.AspNetCore.Mvc.RazorPages;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages;

public class IndexModel : PageModel
{
    private readonly VeterinariaService _svc;
    public IndexModel(VeterinariaService svc) => _svc = svc;

    public DashboardStats Stats    { get; set; } = new();
    public List<Cita>     CitasHoy { get; set; } = [];
    public int CitasPasadasPendientes { get; set; }

    public async Task OnGetAsync()
    {
        Stats    = await _svc.GetDashboardStatsAsync();
        CitasHoy = await _svc.GetCitasDeHoyAsync();
        var todasCitas = await _svc.GetCitasAsync();
        CitasPasadasPendientes = todasCitas
            .Count(c => c.FechaHoraAtencion < DateTime.Now && c.Estado == Veterinaria.Models.EstadoCita.Pendiente);
    }
}
