using System.Globalization;
using System.Windows.Controls;

namespace InventoryManagement.WPF.Validation;

public class PositiveNumberValidation : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (int.TryParse(value?.ToString(), out int number))
        {
            if (number > 0)
                return ValidationResult.ValidResult;
            else
                return new ValidationResult(false, "Value must be greater than 0.");
        }

        return new ValidationResult(false, "Invalid number.");
    }
}
