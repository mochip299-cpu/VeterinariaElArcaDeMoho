using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria.Models;

public class Propietario
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

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [StringLength(20, ErrorMessage = "El teléfono no puede superar 20 caracteres.")]
    [Phone(ErrorMessage = "Formato de teléfono inválido.")]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [StringLength(150, ErrorMessage = "El correo no puede superar 150 caracteres.")]
    [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
    [Display(Name = "Correo electrónico")]
    public string Correo { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Estado")]
    public EstadoGeneral Estado { get; set; } = EstadoGeneral.Activo;

    // Navegación
    public ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();

    // Propiedad calculada (no mapeada)
    [NotMapped]
    public string NombreCompleto => $"{Nombre} {Apellido}";
}
