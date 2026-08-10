using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Propietarios;

public class DeleteModel : PageModel
{
    private readonly VeterinariaService _svc;

    public DeleteModel(VeterinariaService svc) => _svc = svc;

    public Propietario? Propietario { get; set; }
    public bool TieneMascotas { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Propietario = await _svc.GetPropietarioByIdAsync(id);
        if (Propietario is null) return NotFound();

        TieneMascotas = await _svc.PropietarioTieneMascotasAsync(id);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (await _svc.PropietarioTieneMascotasAsync(id))
        {
            TempData["Error"] = "No se puede eliminar: el propietario tiene mascotas registradas.";
            return RedirectToPage("Index");
        }

        await _svc.EliminarPropietarioAsync(id);
        TempData["Exito"] = "Propietario eliminado correctamente.";
        return RedirectToPage("Index");
    }
}
