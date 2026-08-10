using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Mascotas;

public class DeleteModel : PageModel
{
    private readonly VeterinariaService _svc;
    public DeleteModel(VeterinariaService svc) => _svc = svc;

    public Mascota? Mascota { get; set; }
    public bool TieneCitas { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Mascota = await _svc.GetMascotaByIdAsync(id);
        if (Mascota is null) return NotFound();

        TieneCitas = await _svc.MascotaTieneCitasAsync(id);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (await _svc.MascotaTieneCitasAsync(id))
        {
            TempData["Error"] = "No se puede eliminar: la mascota tiene citas registradas.";
            return RedirectToPage("Index");
        }

        await _svc.EliminarMascotaAsync(id);
        TempData["Exito"] = "Mascota eliminada correctamente.";
        return RedirectToPage("Index");
    }
}
