using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Mascotas;

public class DetailsModel : PageModel
{
    private readonly VeterinariaService _svc;
    public DetailsModel(VeterinariaService svc) => _svc = svc;

    public Mascota? Mascota { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Mascota = await _svc.GetMascotaByIdAsync(id);
        if (Mascota is null) return NotFound();
        return Page();
    }
}
