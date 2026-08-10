using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Veterinarios;

public class CreateModel : PageModel
{
    private readonly VeterinariaService _svc;
    public CreateModel(VeterinariaService svc) => _svc = svc;

    [BindProperty]
    public Veterinario Veterinario { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        await _svc.CrearVeterinarioAsync(Veterinario);
        TempData["Exito"] = $"Veterinario {Veterinario.NombreCompleto} registrado correctamente.";
        return RedirectToPage("Index");
    }
}
