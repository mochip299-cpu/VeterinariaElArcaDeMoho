using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Propietarios;

public class DetailsModel : PageModel
{
    private readonly VeterinariaService _svc;

    public DetailsModel(VeterinariaService svc) => _svc = svc;

    public Propietario? Propietario { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Propietario = await _svc.GetPropietarioByIdAsync(id);
        if (Propietario is null) return NotFound();
        return Page();
    }
}
