using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Citas;

public class DeleteModel : PageModel
{
    private readonly VeterinariaService _svc;
    public DeleteModel(VeterinariaService svc) => _svc = svc;

    public Cita? Cita { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Cita = await _svc.GetCitaByIdAsync(id);
        if (Cita is null) return NotFound();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await _svc.EliminarCitaAsync(id);
        TempData["Exito"] = "Cita eliminada correctamente.";
        return RedirectToPage("Index");
    }
}
