using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Veterinarios;

public class DeleteModel : PageModel
{
    private readonly VeterinariaService _svc;
    public DeleteModel(VeterinariaService svc) => _svc = svc;

    public Veterinario? Veterinario { get; set; }
    public bool TieneCitas { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Veterinario = await _svc.GetVeterinarioByIdAsync(id);
        if (Veterinario is null) return NotFound();

        TieneCitas = await _svc.VeterinarioTieneCitasAsync(id);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (await _svc.VeterinarioTieneCitasAsync(id))
        {
            TempData["Error"] = "No se puede eliminar: el veterinario tiene citas registradas.";
            return RedirectToPage("Index");
        }

        await _svc.EliminarVeterinarioAsync(id);
        TempData["Exito"] = "Veterinario eliminado correctamente.";
        return RedirectToPage("Index");
    }
}
