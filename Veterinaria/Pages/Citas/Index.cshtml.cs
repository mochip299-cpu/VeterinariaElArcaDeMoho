using Microsoft.AspNetCore.Mvc.RazorPages;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Citas;

public class IndexModel : PageModel
{
    private readonly VeterinariaService _svc;
    public IndexModel(VeterinariaService svc) => _svc = svc;

    public List<Cita>    Citas        { get; set; } = [];
    public string?       Busqueda     { get; set; }
    public EstadoCita?   EstadoFiltro { get; set; }
    public DateTime?     Desde        { get; set; }
    public DateTime?     Hasta        { get; set; }

    public async Task OnGetAsync(string? busqueda, int? estado, DateTime? desde, DateTime? hasta)
    {
        Busqueda     = busqueda;
        EstadoFiltro = estado.HasValue ? (EstadoCita)estado.Value : null;
        Desde        = desde;
        Hasta        = hasta;

        Citas = await _svc.GetCitasAsync(busqueda, EstadoFiltro, desde, hasta);
    }
}
