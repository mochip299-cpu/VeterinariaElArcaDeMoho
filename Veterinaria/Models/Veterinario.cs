using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria.Models;

public class Veterinario
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(100, ErrorMessage = "El apellido no puede superar 100 caracteres.")]
    [Display(Name = "Apellido")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "La especialidad es obligatoria.")]
    [StringLength(120, ErrorMessage = "La especialidad no puede superar 120 caracteres.")]
    [Display(Name = "Especialidad")]
    public string Especialidad { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [StringLength(20, ErrorMessage = "El teléfono no puede superar 20 caracteres.")]
    [Phone(ErrorMessage = "Formato de teléfono inválido.")]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Estado")]
    public EstadoGeneral Estado { get; set; } = EstadoGeneral.Activo;

    // Navegación
    public ICollection<Cita> Citas { get; set; } = new List<Cita>();

    // Propiedad calculada (no mapeada)
    [NotMapped]
    public string NombreCompleto => $"Dr(a). {Nombre} {Apellido}";
}
