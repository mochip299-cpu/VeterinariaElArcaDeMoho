using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Citas;

public class DetailsModel : PageModel
{
    private readonly VeterinariaService _svc;
    public DetailsModel(VeterinariaService svc) => _svc = svc;

    public Cita? Cita { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Cita = await _svc.GetCitaByIdAsync(id);
        if (Cita is null) return NotFound();
        return Page();
    }
}
