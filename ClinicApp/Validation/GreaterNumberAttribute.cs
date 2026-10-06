using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class GreaterNumberAttribute : ValidationAttribute
    {
        private readonly string _cmpPropName;

        public GreaterNumberAttribute(string cmpPropName)
        {
            _cmpPropName = cmpPropName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            
            var compProp = validationContext.ObjectType.GetProperty(_cmpPropName);
            if (compProp == null)
            {
                return new ValidationResult($"Unknown property: {_cmpPropName}");
            }

            // 3. Fetch target property value
            var cmpVal = compProp.GetValue(validationContext.ObjectInstance);
            if (cmpVal == null) return ValidationResult.Success;

            // 4. Try converting both values to decimal safely
            try
            {
                decimal comparisonValue = Convert.ToDecimal(cmpVal);
                decimal currentValue = Convert.ToDecimal(value);

                // 5. Compare numbers
                if (currentValue >= comparisonValue)
                {
                    return ValidationResult.Success;
                }

                string errorMsg = ErrorMessage ?? $"{validationContext.DisplayName} must be greater than Old Value.";
                return new ValidationResult(errorMsg);
            }
            catch (Exception)
            {
                return new ValidationResult($"Could not compare numeric values between {validationContext.DisplayName} and {_cmpPropName}.");
            }
        }
    }
}