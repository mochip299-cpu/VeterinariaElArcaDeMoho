using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Veterinarios;

public class EditModel : PageModel
{
    private readonly VeterinariaService _svc;
    public EditModel(VeterinariaService svc) => _svc = svc;

    [BindProperty]
    public Veterinario Veterinario { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var vet = await _svc.GetVeterinarioByIdAsync(id);
        if (vet is null) return NotFound();
        Veterinario = vet;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        await _svc.ActualizarVeterinarioAsync(Veterinario);
        TempData["Exito"] = "Veterinario actualizado correctamente.";
        return RedirectToPage("Index");
    }
}
