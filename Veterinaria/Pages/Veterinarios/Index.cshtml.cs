using Microsoft.AspNetCore.Mvc.RazorPages;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Veterinarios;

public class IndexModel : PageModel
{
    private readonly VeterinariaService _svc;
    public IndexModel(VeterinariaService svc) => _svc = svc;

    public List<Veterinario> Veterinarios { get; set; } = [];
    public string? Busqueda { get; set; }

    public async Task OnGetAsync(string? busqueda)
    {
        Busqueda = busqueda;
        Veterinarios = await _svc.GetVeterinariosAsync(busqueda);
    }
}
