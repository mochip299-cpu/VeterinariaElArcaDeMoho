using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Veterinaria.Models;
using Veterinaria.Services;

namespace Veterinaria.Pages.Citas;

public class EditModel : PageModel
{
    private readonly VeterinariaService _svc;
    public EditModel(VeterinariaService svc) => _svc = svc;

    [BindProperty]
    public Cita Cita { get; set; } = new();

    public SelectList MascotasSelectList     { get; set; } = default!;
    public SelectList VeterinariosSelectList { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var cita = await _svc.GetCitaByIdAsync(id);
        if (cita is null) return NotFound();

        Cita = cita;
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

        await _svc.ActualizarCitaAsync(Cita);
        TempData["Exito"] = "Cita actualizada correctamente.";
        return RedirectToPage("Index");
    }

    private async Task CargarSelectListsAsync()
    {
        var mascotas = await _svc.GetMascotasAsync();
        MascotasSelectList = new SelectList(
            mascotas.Select(m => new { m.Id, Nombre = $"{m.Nombre} ({m.Propietario?.NombreCompleto})" }),
            "Id", "Nombre", Cita.MascotaId);

        var vets = await _svc.GetVeterinariosActivosAsync();
        VeterinariosSelectList = new SelectList(
            vets.Select(v => new { v.Id, Nombre = v.NombreCompleto }),
            "Id", "Nombre", Cita.VeterinarioId);
    }
}
