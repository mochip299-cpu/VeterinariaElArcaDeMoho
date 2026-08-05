using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria.Models;

public class Cita
{
    [Key]
    public int Id { get; set; }

    // FK Mascota
    [Required(ErrorMessage = "La mascota es obligatoria.")]
    [Display(Name = "Mascota")]
    public int MascotaId { get; set; }

    [ForeignKey(nameof(MascotaId))]
    public Mascota? Mascota { get; set; }

    // FK Veterinario
    [Required(ErrorMessage = "El veterinario es obligatorio.")]
    [Display(Name = "Veterinario")]
    public int VeterinarioId { get; set; }

    [ForeignKey(nameof(VeterinarioId))]
    public Veterinario? Veterinario { get; set; }

    [Required(ErrorMessage = "La fecha y hora de atención son obligatorias.")]
    [DataType(DataType.DateTime)]
    [Display(Name = "Fecha y hora de atención")]
    public DateTime FechaHoraAtencion { get; set; }

    [Required(ErrorMessage = "El motivo es obligatorio.")]
    [StringLength(300, ErrorMessage = "El motivo no puede superar 300 caracteres.")]
    [Display(Name = "Motivo")]
    public string Motivo { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Estado de la cita")]
    public EstadoCita Estado { get; set; } = EstadoCita.Pendiente;

    [StringLength(1000, ErrorMessage = "El diagnóstico no puede superar 1000 caracteres.")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Diagnóstico")]
    public string? Diagnostico { get; set; }

    // Auditoría
    [Display(Name = "Creado el")]
    public DateTime CreadoEn { get; set; } = DateTime.Now;

    // Propiedad calculada (no mapeada)
    [NotMapped]
    public bool EsHoy => FechaHoraAtencion.Date == DateTime.Today;

    [NotMapped]
    public bool EsPasada => FechaHoraAtencion < DateTime.Now && Estado == EstadoCita.Pendiente;
}
