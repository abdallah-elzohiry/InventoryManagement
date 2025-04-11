using System.Globalization;
using System.Windows.Controls;

namespace InventoryManagement.WPF.Validation;

public class RequiredFieldValidation : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (string.IsNullOrWhiteSpace(value as string))
            return new ValidationResult(false, "This field is required.");
        return ValidationResult.ValidResult;
    }

}
