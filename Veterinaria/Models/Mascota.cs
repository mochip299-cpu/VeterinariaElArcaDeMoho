using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria.Models;

public class Mascota
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La especie es obligatoria.")]
    [StringLength(60, ErrorMessage = "La especie no puede superar 60 caracteres.")]
    [Display(Name = "Especie")]
    public string Especie { get; set; } = string.Empty;

    [StringLength(80, ErrorMessage = "La raza no puede superar 80 caracteres.")]
    [Display(Name = "Raza")]
    public string? Raza { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de nacimiento")]
    public DateOnly FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El color es obligatorio.")]
    [StringLength(60, ErrorMessage = "El color no puede superar 60 caracteres.")]
    [Display(Name = "Color")]
    public string Color { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Estado")]
    public EstadoGeneral Estado { get; set; } = EstadoGeneral.Activo;

    // FK Propietario
    [Required(ErrorMessage = "El propietario es obligatorio.")]
    [Display(Name = "Propietario")]
    public int PropietarioId { get; set; }

    [ForeignKey(nameof(PropietarioId))]
    public Propietario? Propietario { get; set; }

    // Navegación
    public ICollection<Cita> Citas { get; set; } = new List<Cita>();

    // Propiedad calculada (no mapeada)
    [NotMapped]
    public int Edad
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            int años = hoy.Year - FechaNacimiento.Year;
            if (FechaNacimiento > hoy.AddYears(-años)) años--;
            return años;
        }
    }
}
