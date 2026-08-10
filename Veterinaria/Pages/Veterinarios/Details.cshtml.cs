using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Veterinarios;

public class DetailsModel : PageModel
{
    private readonly VeterinariaService _svc;
    public DetailsModel(VeterinariaService svc) => _svc = svc;

    public Veterinario? Veterinario { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        // Cargamos el veterinario con sus citas e incluimos la mascota en cada cita
        Veterinario = await _svc.GetVeterinarioByIdAsync(id);
        if (Veterinario is null) return NotFound();

        // Enriquecemos las citas con la mascota para mostrar su nombre
        var citas = await _svc.GetCitasAsync(estado: null, desde: null, hasta: null);
        Veterinario.Citas = citas.Where(c => c.VeterinarioId == id).ToList();

        return Page();
    }
}
