using FileCreator.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace FileCreator.Helpers
{
    internal class IsValidLongBytesBasedOnSizeUnit : ValidationAttribute
    {
        public string SizeUnitPropertyName { get; private set; }

        public IsValidLongBytesBasedOnSizeUnit(string sizeUnitPropertyName)
        {
            this.SizeUnitPropertyName = sizeUnitPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext == null)
                throw new ArgumentException("validationContext is null.");

            if (validationContext.ObjectInstance == null)
                throw new ArgumentException("validationContext.ObjectInstance is null.");

            var sizeUnit = validationContext.ObjectInstance.GetPropertyValue(SizeUnitPropertyName) as Size?;
            if (sizeUnit == null)
                throw new ArgumentException(string.Format("Specified property '{0}' is not of enum type 'Size'.", SizeUnitPropertyName));

            long longValue;
            switch (sizeUnit)
            {
                case Size.B:
                    if (!long.TryParse(value as string, out longValue) || longValue > long.MaxValue)
                        return new ValidationResult(string.Format("Size in Bytes must be between 1 and {0}.", long.MaxValue));
                    break;

                case Size.KB:
                    if (!long.TryParse(value as string, out longValue) || (longValue > (long.MaxValue / 1024)))
                        return new ValidationResult(string.Format("Size in KiloBytes must be between 1 and {0}.", (long.MaxValue / 1024)));
                    break;

                case Size.MB:
                    if (!long.TryParse(value as string, out longValue) || (longValue > (long.MaxValue / 1024 / 1024)))
                        return new ValidationResult(string.Format("Size in MegaBytes must be between 1 and {0}.", (long.MaxValue / 1024 / 1024)));
                    break;

                case Size.GB:
                    if (!long.TryParse(value as string, out longValue) || (longValue > (long.MaxValue / 1024 / 1024 / 1024)))
                        return new ValidationResult(string.Format("Size in GigaBytes must be between 1 and '{0}'.", (long.MaxValue / 1024 / 1024 / 1024)));
                    break;

                default:
                    throw new ArgumentOutOfRangeException("Unexpected size unit.");
            }

            if (longValue < 1)
                return new ValidationResult("Value must be greater than zero.");

            return null;
        }
    }
}