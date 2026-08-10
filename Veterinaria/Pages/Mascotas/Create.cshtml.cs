using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Mascotas;

public class CreateModel : PageModel
{
    private readonly VeterinariaService _svc;
    public CreateModel(VeterinariaService svc) => _svc = svc;

    [BindProperty]
    public Mascota Mascota { get; set; } = new();

    public SelectList PropietariosSelectList { get; set; } = default!;

    public async Task OnGetAsync(int? propietarioId)
    {
        await CargarSelectListsAsync();
        if (propietarioId.HasValue)
            Mascota.PropietarioId = propietarioId.Value;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await CargarSelectListsAsync();
            return Page();
        }

        await _svc.CrearMascotaAsync(Mascota);
        TempData["Exito"] = $"Mascota {Mascota.Nombre} registrada correctamente.";
        return RedirectToPage("Index");
    }

    private async Task CargarSelectListsAsync()
    {
        var propietarios = await _svc.GetPropietariosAsync();
        PropietariosSelectList = new SelectList(
            propietarios.Select(p => new { p.Id, Nombre = p.NombreCompleto }),
            "Id", "Nombre");
    }
}
