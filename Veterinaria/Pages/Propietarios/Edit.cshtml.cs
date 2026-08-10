using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Propietarios;

public class EditModel : PageModel
{
    private readonly VeterinariaService _svc;

    public EditModel(VeterinariaService svc) => _svc = svc;

    [BindProperty]
    public Propietario Propietario { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var propietario = await _svc.GetPropietarioByIdAsync(id);
        if (propietario is null) return NotFound();

        Propietario = propietario;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        if (await _svc.CorreoExisteAsync(Propietario.Correo, Propietario.Id))
        {
            ModelState.AddModelError("Propietario.Correo", "Ya existe otro propietario con ese correo.");
            return Page();
        }

        await _svc.ActualizarPropietarioAsync(Propietario);
        TempData["Exito"] = "Propietario actualizado correctamente.";
        return RedirectToPage("Index");
    }
}
