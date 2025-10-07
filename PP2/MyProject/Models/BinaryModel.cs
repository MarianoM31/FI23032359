using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq; // Necesario para usar .All()

namespace MyProject.Models
{
    public class BinaryModel
    {
        [Required(ErrorMessage = "El valor de a es obligatorio")]
        [RegularExpression("^[01]+$", ErrorMessage = "Solo se permiten caracteres 0 y 1")]
        [StringLength(8, MinimumLength = 2, ErrorMessage = "La longitud debe ser entre 2 y 8")]
        [CustomValidation(typeof(BinaryModel), nameof(ValidarMultiploDe2))]
        [CustomValidation(typeof(BinaryModel), nameof(ValidarNoSoloCeros))]
        public string A { get; set; }

        [Required(ErrorMessage = "El valor de b es obligatorio")]
        [RegularExpression("^[01]+$", ErrorMessage = "Solo se permiten caracteres 0 y 1")]
        [StringLength(8, MinimumLength = 2, ErrorMessage = "La longitud debe ser entre 2 y 8")]
        [CustomValidation(typeof(BinaryModel), nameof(ValidarMultiploDe2))]
        [CustomValidation(typeof(BinaryModel), nameof(ValidarNoSoloCeros))]
        public string B { get; set; }

        public static ValidationResult ValidarMultiploDe2(string value, ValidationContext context)
        {
            if (!string.IsNullOrEmpty(value) && value.Length % 2 != 0)
            {
                return new ValidationResult("La longitud debe ser múltiplo de 2 (2,4,6,8)");
            }
            return ValidationResult.Success;
        }

        public static ValidationResult ValidarNoSoloCeros(string value, ValidationContext context)
        {
            if (!string.IsNullOrEmpty(value) && value.All(c => c == '0'))
            {
                return new ValidationResult("El valor tiene que ser mayor a 0");
            }
            return ValidationResult.Success;
        }
    }
}
