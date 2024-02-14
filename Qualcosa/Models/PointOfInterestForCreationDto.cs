using System.ComponentModel.DataAnnotations;

namespace CityInfoAPI.Models
{
    public class PointOfInterestForCreationDto
    {
        [Required(ErrorMessage = "You should provide a name value")] //Il NOME deve essere per forza presenete, nel caso uscirà l'errore di ErrorMessage!
        [MaxLength(50)] // Lunghezza masssima del nome
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)] // Lunghezza massima di description consentita
        public string? Description { get; set; }
    }
}
