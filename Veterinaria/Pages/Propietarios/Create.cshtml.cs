using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Propietarios;

public class CreateModel : PageModel
{
    private readonly VeterinariaService _svc;

    public CreateModel(VeterinariaService svc) => _svc = svc;

    [BindProperty]
    public Propietario Propietario { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        if (await _svc.CorreoExisteAsync(Propietario.Correo))
        {
            ModelState.AddModelError("Propietario.Correo", "Ya existe un propietario con ese correo.");
            return Page();
        }

        await _svc.CrearPropietarioAsync(Propietario);
        TempData["Exito"] = $"Propietario {Propietario.NombreCompleto} registrado correctamente.";
        return RedirectToPage("Index");
    }
}
