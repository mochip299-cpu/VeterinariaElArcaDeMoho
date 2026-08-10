using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Mascotas;

public class EditModel : PageModel
{
    private readonly VeterinariaService _svc;
    public EditModel(VeterinariaService svc) => _svc = svc;

    [BindProperty]
    public Mascota Mascota { get; set; } = new();

    public SelectList PropietariosSelectList { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var mascota = await _svc.GetMascotaByIdAsync(id);
        if (mascota is null) return NotFound();

        Mascota = mascota;
        await CargarSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await CargarSelectListsAsync();
            return Page();
        }

        await _svc.ActualizarMascotaAsync(Mascota);
        TempData["Exito"] = "Mascota actualizada correctamente.";
        return RedirectToPage("Index");
    }

    private async Task CargarSelectListsAsync()
    {
        var propietarios = await _svc.GetPropietariosAsync();
        PropietariosSelectList = new SelectList(
            propietarios.Select(p => new { p.Id, Nombre = p.NombreCompleto }),
            "Id", "Nombre", Mascota.PropietarioId);
    }
}
